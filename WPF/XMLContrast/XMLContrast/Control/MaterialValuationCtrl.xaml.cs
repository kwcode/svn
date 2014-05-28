using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPF.CustomControl;
using System.Windows.Threading;
using System.Threading;

namespace XMLContrast.Control
{
    /// <summary>
    /// 暂列金额
    /// </summary>
    public partial class MaterialValuationCtrl : UserControl
    {
        private List<MaterialValuation> _ProvisionalSums = new List<MaterialValuation>();
        public MaterialValuationCtrl()
        {
            InitializeComponent();
        }
        public MaterialValuationCtrl(List<XMLNodeItem> tenderDataNodes, List<XMLNodeItem> bidDataNodes, object owner)
        {
            InitializeComponent();
            Global.SysContext = DispatcherSynchronizationContext.Current;
            Init(tenderDataNodes, bidDataNodes, owner);
        }

        public void Init(List<XMLNodeItem> tenderDataNodes, List<XMLNodeItem> bidDataNodes, object owner)
        {
            (owner as HLWindowExt).StartProgress();
            lv_data.CreateListViewDataBindColumns("名称", "TName", 150);
            lv_data.CreateListViewDataBindColumns("计量单位", "TUnit", 80);
            lv_data.CreateListViewDataBindColumns("规格", "TSpecification", 100);
            lv_data.CreateListViewDataBindColumns("型号", "TModel", 100);
            lv_data.CreateListViewDataBindColumns("单价", "TPrice", 80);

            lv_data.CreateListViewDataBindColumns("名称", "BName", 150);
            lv_data.CreateListViewDataBindColumns("计量单位", "BUnit", 80);
            lv_data.CreateListViewDataBindColumns("规格", "BSpecification", 100);
            lv_data.CreateListViewDataBindColumns("型号", "BModel", 100);
            lv_data.CreateListViewDataBindColumns("单价", "BPrice", 80);

            lv_data.CreateListViewDataBindColumns("检查结果", "CheckResult", 50);
            new Thread(o =>
              {
                  List<XMLNodeItem> xmlTenderNodeItems = XMLOperat.GetDataChildNodes("暂估材料子目", tenderDataNodes);
                  List<XMLNodeItem> xmlBidNodeItems = XMLOperat.GetDataChildNodes("暂估材料子目", bidDataNodes);
                  AnalysisData(xmlTenderNodeItems, xmlBidNodeItems);

                  Global.SysContext.Send(a =>
                  {
                      lv_data.DataItemSource = _ProvisionalSums;
                      (owner as HLWindowExt).StopProgress();
                  }, null);
              }).Start();

        }
        private void AnalysisData(List<XMLNodeItem> xmlTenderNodeItems, List<XMLNodeItem> xmlBidNodeItems)
        {
            if (xmlTenderNodeItems.Count >= xmlBidNodeItems.Count)
            {
                for (int i = 0; i < xmlTenderNodeItems.Count; i++)
                {
                    XMLNodeItem BidNodeItems = null;
                    if (xmlBidNodeItems != null && xmlBidNodeItems.Count != 0)
                        BidNodeItems = xmlBidNodeItems[i];
                    XMLNodeItem TenderNode = null;
                    if (xmlTenderNodeItems != null && xmlTenderNodeItems.Count != 0)
                        TenderNode = xmlTenderNodeItems[i];
                    MaterialValuation material = ConvertToMaterial(TenderNode, BidNodeItems);
                    _ProvisionalSums.Add(material);
                }
            }
            else
            {
                for (int i = 0; i < xmlBidNodeItems.Count; i++)
                {
                    XMLNodeItem BidNodeItems = null;
                    if (xmlBidNodeItems != null && xmlBidNodeItems.Count != 0)
                        BidNodeItems = xmlBidNodeItems[i];
                    XMLNodeItem TenderNode = null;
                    if (xmlTenderNodeItems != null && xmlTenderNodeItems.Count != 0)
                        TenderNode = xmlTenderNodeItems[i];
                    MaterialValuation material = ConvertToMaterial(TenderNode, BidNodeItems);
                    _ProvisionalSums.Add(material);
                }
            }
        }
        private MaterialValuation ConvertToMaterial(XMLNodeItem TnodeItem, XMLNodeItem BnodeItem)
        {
            MaterialValuation material = new MaterialValuation();
            if (TnodeItem != null)
            {
                for (int i = 0; i < TnodeItem.Attributes.Count; i++)
                {
                    if (TnodeItem.Attributes[i].Name == "序号")
                        material.TNumber = TnodeItem.Attributes[i].Value;
                    if (TnodeItem.Attributes[i].Name == "材料名称")
                        material.TName = TnodeItem.Attributes[i].Value;
                    if (TnodeItem.Attributes[i].Name == "规格")
                        material.TSpecification = TnodeItem.Attributes[i].Value;
                    if (TnodeItem.Attributes[i].Name == "型号")
                        material.TModel = TnodeItem.Attributes[i].Value;
                    if (TnodeItem.Attributes[i].Name == "计量单位")
                        material.TUnit = TnodeItem.Attributes[i].Value;
                    if (TnodeItem.Attributes[i].Name == "单价")
                        material.TPrice = TnodeItem.Attributes[i].Value;


                }
            }
            if (BnodeItem != null)
            {
                for (int i = 0; i < BnodeItem.Attributes.Count; i++)
                {
                    if (BnodeItem.Attributes[i].Name == "序号")
                        material.BNumber = BnodeItem.Attributes[i].Value;
                    if (BnodeItem.Attributes[i].Name == "材料名称")
                        material.BName = BnodeItem.Attributes[i].Value;
                    if (BnodeItem.Attributes[i].Name == "规格")
                        material.BSpecification = BnodeItem.Attributes[i].Value;
                    if (BnodeItem.Attributes[i].Name == "型号")
                        material.BModel = BnodeItem.Attributes[i].Value;
                    if (BnodeItem.Attributes[i].Name == "计量单位")
                        material.BUnit = BnodeItem.Attributes[i].Value;
                    if (BnodeItem.Attributes[i].Name == "单价")
                        material.BPrice = BnodeItem.Attributes[i].Value;
                }
            }
            //颜色
            bool b = true;
            if (material.TNumber.Replace(" ", "") != material.BNumber.Replace(" ", ""))
                b = false;
            if (material.TName.Replace(" ", "") != material.BName.Replace(" ", ""))
                b = false;
            if (material.TUnit.Replace(" ", "") != material.BUnit.Replace(" ", ""))
                b = false;
            if (material.TModel.Replace(" ", "") != material.BModel.Replace(" ", ""))
                b = false;
            if (material.TSpecification.Replace(" ", "") != material.BSpecification.Replace(" ", ""))
                b = false;
            if (material.TPrice.Replace(" ", "") != material.BPrice.Replace(" ", ""))
                b = false;
            if (!b)
                b = false;
            if (b)
                material.CheckResult = "正确";
            else
            {
                material.CheckResult = "错误";
                material.Foreground = Brushes.Red;
            }
            return material;
        }

    }
}
