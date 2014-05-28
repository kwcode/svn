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
    public partial class ProfessionalEngineeringValuationCtrl : UserControl
    {
        private List<ProfessionalValuation> _ProvisionalSums = new List<ProfessionalValuation>();
        public ProfessionalEngineeringValuationCtrl()
        {
            InitializeComponent();
        }
        public ProfessionalEngineeringValuationCtrl(List<XMLNodeItem> tenderDataNodes, List<XMLNodeItem> bidDataNodes, object owner)
        {
            InitializeComponent();
            Global.SysContext = DispatcherSynchronizationContext.Current;
            Init(tenderDataNodes, bidDataNodes, owner);
        }

        public void Init(List<XMLNodeItem> tenderDataNodes, List<XMLNodeItem> bidDataNodes, object owner)
        {
            (owner as HLWindowExt).StartProgress();
            lv_data.CreateListViewDataBindColumns("名称", "TName", 200);
            lv_data.CreateListViewDataBindColumns("金额", "TMoney", 100);

            lv_data.CreateListViewDataBindColumns("名称", "BName", 200);
            lv_data.CreateListViewDataBindColumns("金额", "BMoney", 100);

            lv_data.CreateListViewDataBindColumns("检查结果", "CheckResult", 50);
            new Thread(o =>
              {
                  List<XMLNodeItem> xmlTenderNodeItems = XMLOperat.GetDataChildNodes("专业工程暂估价", tenderDataNodes);
                  List<XMLNodeItem> xmlBidNodeItems = XMLOperat.GetDataChildNodes("专业工程暂估价", bidDataNodes);
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
                    ProfessionalValuation material = ConvertToMaterial(TenderNode, BidNodeItems);
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
                    ProfessionalValuation material = ConvertToMaterial(TenderNode, BidNodeItems);
                    _ProvisionalSums.Add(material);
                }
            }
        }
        private ProfessionalValuation ConvertToMaterial(XMLNodeItem TnodeItem, XMLNodeItem BnodeItem)
        {
            ProfessionalValuation material = new ProfessionalValuation();
            if (TnodeItem != null)
            {
                for (int i = 0; i < TnodeItem.Attributes.Count; i++)
                {
                    if (TnodeItem.Attributes[i].Name == "序号")
                        material.TName = TnodeItem.Attributes[i].Value;
                    if (TnodeItem.Attributes[i].Name == "金额")
                        material.TMoney = TnodeItem.Attributes[i].Value;


                }
            }
            if (BnodeItem != null)
            {
                for (int i = 0; i < BnodeItem.Attributes.Count; i++)
                {
                    if (BnodeItem.Attributes[i].Name == "序号")
                        material.BName = BnodeItem.Attributes[i].Value;
                    if (BnodeItem.Attributes[i].Name == "金额")
                        material.BMoney = BnodeItem.Attributes[i].Value;
                }
            }
            //颜色
            bool b = true;
            if (material.TName.Replace(" ", "") != material.BName.Replace(" ", ""))
                b = false;
            if (material.TMoney.Replace(" ", "") != material.BMoney.Replace(" ", ""))
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
