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

namespace XMLContrast.Control
{
    /// <summary>
    /// 招标工程结构检查
    /// </summary>
    public partial class EngineeringStructuresCtrl : UserControl
    {
        List<EngineeringStructures> _dataTitems = new List<EngineeringStructures>();
        List<EngineeringStructures> _dataBitems = new List<EngineeringStructures>();
        public EngineeringStructuresCtrl()
        {
            InitializeComponent();
        }
        public EngineeringStructuresCtrl(List<XMLNodeItem> tenderDataNodes, List<XMLNodeItem> bidDataNodes)
        {
            InitializeComponent();
            Init(tenderDataNodes, bidDataNodes);
        }
        public void Init(List<XMLNodeItem> tenderDataNodes, List<XMLNodeItem> bidDataNodes)
        {
            List<XMLNodeItem> xmlTenderNodeItems = XMLOperat.GetDataChildNodes("单位工程", tenderDataNodes);
            List<XMLNodeItem> xmlBidNodeItems = XMLOperat.GetDataChildNodes("单位工程", bidDataNodes);


            AnalysisData(xmlTenderNodeItems, xmlBidNodeItems);
            // GridLineVisibility="Visible" GridLineBrush="Black"
            lv_dataT.CreateListViewDataBindColumns("", "ICO", "ItemIcon", 50, new Thickness(2));
            lv_dataT.CreateListViewDataBindColumns("工程结构", "Name", 200);


            lv_dataB.CreateListViewDataBindColumns("", "ICO", "ItemIcon", 50, new Thickness(2));
            lv_dataB.CreateListViewDataBindColumns("工程结构", "Name", 200);

            lv_dataB.CreateListViewDataBindColumns("检查结果", "CheckResult", 100);
            lv_dataT.DataItemSource = _dataTitems;
            lv_dataB.DataItemSource = _dataBitems;
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
                    SetMaterial(TenderNode, BidNodeItems);
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
                    SetMaterial(TenderNode, BidNodeItems);
                }
            }
        }

        private void SetMaterial(XMLNodeItem TnodeItem, XMLNodeItem BnodeItem)
        {
            EngineeringStructures TStructures = new EngineeringStructures();
            if (TnodeItem != null)
            {
                TStructures.ItemIcon = "/XMLContrast;component/Resources/gc.ico";
                for (int i = 0; i < TnodeItem.Attributes.Count; i++)
                {
                    if (TnodeItem.Attributes[i].Name == "名称")
                        TStructures.Name = TnodeItem.Attributes[i].Value;
                }
                _dataTitems.Add(TStructures);
            }
            EngineeringStructures BStructures = new EngineeringStructures();
            if (BnodeItem != null)
            {
                BStructures.ItemIcon = "/XMLContrast;component/Resources/gc.ico";
                for (int i = 0; i < BnodeItem.Attributes.Count; i++)
                {
                    if (BnodeItem.Attributes[i].Name == "名称")
                        BStructures.Name = BnodeItem.Attributes[i].Value;
                }
            }
            //颜色
            bool b = true;
            if (TStructures.Name.Replace(" ", "") != BStructures.Name.Replace(" ", ""))
                b = false;
            if (!b)
                b = false;
            if (b)
                BStructures.CheckResult = "正确";
            else
            {
                BStructures.Foreground = Brushes.Red;
                BStructures.CheckResult = "错误";
            }

            _dataBitems.Add(BStructures);
        }

    }
}
