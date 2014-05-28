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
    /// MainMaterialCtrl.xaml 的交互逻辑
    /// </summary>
    public partial class MainMaterialCtrl : UserControl
    {
        private List<Material> _materiallist = new List<Material>();
        public MainMaterialCtrl()
        {
            InitializeComponent();
        }
        public MainMaterialCtrl(List<XMLNodeItem> tenderDataNodes, List<XMLNodeItem> bidDataNodes, object owner)
        {
            InitializeComponent();
            Global.SysContext = DispatcherSynchronizationContext.Current;
            Init(tenderDataNodes, bidDataNodes, owner);
        }

        public void Init(List<XMLNodeItem> tenderDataNodes, List<XMLNodeItem> bidDataNodes, object owner)
        {
            (owner as HLWindowExt).StartProgress();
            lv_data.CreateListViewDataBindColumns("序号", "TNumber", 50);
            lv_data.CreateListViewDataBindColumns("名称", "TName", 200);
            lv_data.CreateListViewDataBindColumns("规格", "TSpecification", 180);
            lv_data.CreateListViewDataBindColumns("单位", "TUnit", 40);
            lv_data.CreateListViewDataBindColumns("数量", "TQuantity", 50);

            lv_data.CreateListViewDataBindColumns("序号", "BNumber", 50);
            lv_data.CreateListViewDataBindColumns("名称", "BName", 200);
            lv_data.CreateListViewDataBindColumns("规格", "BSpecification", 180);
            lv_data.CreateListViewDataBindColumns("单位", "BUnit", 40);
            lv_data.CreateListViewDataBindColumns("数量", "BQuantity", 50);

            lv_data.CreateListViewDataBindColumns("检查结果", "CheckResult", 50);
            new Thread(o =>
              {
                  List<XMLNodeItem> xmlTenderNodeItems = XMLOperat.GetDataChildNodes("主要材料子目", tenderDataNodes);
                  List<XMLNodeItem> xmlBidNodeItems = XMLOperat.GetDataChildNodes("主要材料子目", bidDataNodes);
                  AnalysisData(xmlTenderNodeItems, xmlBidNodeItems);

                  Global.SysContext.Send(a =>
                  {
                      lv_data.DataItemSource = _materiallist;
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
                    Material material = ConvertToMaterial(TenderNode, BidNodeItems);
                    _materiallist.Add(material);
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
                    Material material = ConvertToMaterial(TenderNode, BidNodeItems);
                    _materiallist.Add(material);
                }
            }
        }
        private Material ConvertToMaterial(XMLNodeItem TnodeItem, XMLNodeItem BnodeItem)
        {
            Material material = new Material();
            if (TnodeItem != null)
            {
                for (int i = 0; i < TnodeItem.Attributes.Count; i++)
                {
                    if (TnodeItem.Attributes[i].Name == "序号")
                        material.TNumber = TnodeItem.Attributes[i].Value;
                    if (TnodeItem.Attributes[i].Name == "名称")
                        material.TName = TnodeItem.Attributes[i].Value;
                    if (TnodeItem.Attributes[i].Name == "规格")
                        material.TSpecification = TnodeItem.Attributes[i].Value;
                    if (TnodeItem.Attributes[i].Name == "单位")
                        material.TUnit = TnodeItem.Attributes[i].Value;
                    if (TnodeItem.Attributes[i].Name == "数量")
                        material.TQuantity = TnodeItem.Attributes[i].Value;
                }
            }
            if (BnodeItem != null)
            {
                for (int i = 0; i < BnodeItem.Attributes.Count; i++)
                {
                    if (BnodeItem.Attributes[i].Name == "序号")
                        material.BNumber = BnodeItem.Attributes[i].Value;
                    if (BnodeItem.Attributes[i].Name == "名称")
                        material.BName = BnodeItem.Attributes[i].Value;
                    if (BnodeItem.Attributes[i].Name == "规格")
                        material.BSpecification = BnodeItem.Attributes[i].Value;
                    if (BnodeItem.Attributes[i].Name == "单位")
                        material.BUnit = BnodeItem.Attributes[i].Value;
                    if (BnodeItem.Attributes[i].Name == "数量")
                        material.BQuantity = BnodeItem.Attributes[i].Value;
                }
            }
            //颜色
            bool b = true;
            if (material.TNumber.Replace(" ", "") != material.BNumber.Replace(" ", ""))
                b = false;
            if (material.TName.Replace(" ", "") != material.BName.Replace(" ", ""))
                b = false;
            if (material.TSpecification.Replace(" ", "") != material.BSpecification.Replace(" ", ""))
                b = false;
            if (material.TUnit.Replace(" ", "") != material.BUnit.Replace(" ", ""))
                b = false;
            if (material.TQuantity.Replace(" ", "") != material.TQuantity.Replace(" ", ""))
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
