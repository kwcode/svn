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
    public partial class ProvisionalSumsSubheadCtrl : UserControl
    {
        private List<ProvisionalSums> _ProvisionalSums = new List<ProvisionalSums>();
        public ProvisionalSumsSubheadCtrl()
        {
            InitializeComponent();
        }
        public ProvisionalSumsSubheadCtrl(List<XMLNodeItem> tenderDataNodes, List<XMLNodeItem> bidDataNodes, object owner)
        {
            InitializeComponent();
            Global.SysContext = DispatcherSynchronizationContext.Current;
            Init(tenderDataNodes, bidDataNodes, owner);
        }

        public void Init(List<XMLNodeItem> tenderDataNodes, List<XMLNodeItem> bidDataNodes, object owner)
        {
            (owner as HLWindowExt).StartProgress();
            lv_data.CreateListViewDataBindColumns("名称", "TName", 200);
            lv_data.CreateListViewDataBindColumns("计量单位", "TUnit", 100);
            lv_data.CreateListViewDataBindColumns("暂定金额", "TProvisionalSums", 100);

            lv_data.CreateListViewDataBindColumns("名称", "BName", 200);
            lv_data.CreateListViewDataBindColumns("计量单位", "BUnit", 100);
            lv_data.CreateListViewDataBindColumns("暂定金额", "BProvisionalSums", 100);

            lv_data.CreateListViewDataBindColumns("检查结果", "CheckResult", 50);
            new Thread(o =>
              {
                  List<XMLNodeItem> xmlTenderNodeItems = XMLOperat.GetDataChildNodes("暂列金额子目", tenderDataNodes);
                  List<XMLNodeItem> xmlBidNodeItems = XMLOperat.GetDataChildNodes("暂列金额子目", bidDataNodes);
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
                    ProvisionalSums material = ConvertToMaterial(TenderNode, BidNodeItems);
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
                    ProvisionalSums material = ConvertToMaterial(TenderNode, BidNodeItems);
                    _ProvisionalSums.Add(material);

                }
            }
        }
        private ProvisionalSums ConvertToMaterial(XMLNodeItem TnodeItem, XMLNodeItem BnodeItem)
        {
            ProvisionalSums material = new ProvisionalSums();
            if (TnodeItem != null)
            {
                for (int i = 0; i < TnodeItem.Attributes.Count; i++)
                {
                    if (TnodeItem.Attributes[i].Name == "序号")
                        material.TNumber = TnodeItem.Attributes[i].Value;
                    if (TnodeItem.Attributes[i].Name == "项目名称")
                        material.TName = TnodeItem.Attributes[i].Value;
                    if (TnodeItem.Attributes[i].Name == "计量单位")
                        material.TUnit = TnodeItem.Attributes[i].Value;
                    if (TnodeItem.Attributes[i].Name == "暂定金额")
                        material.TProvisionalSums = TnodeItem.Attributes[i].Value;
                }
            }
            if (BnodeItem != null)
            {
                for (int i = 0; i < BnodeItem.Attributes.Count; i++)
                {
                    if (BnodeItem.Attributes[i].Name == "序号")
                        material.BNumber = BnodeItem.Attributes[i].Value;
                    if (BnodeItem.Attributes[i].Name == "项目名称")
                        material.BName = BnodeItem.Attributes[i].Value;
                    if (BnodeItem.Attributes[i].Name == "计量单位")
                        material.BUnit = BnodeItem.Attributes[i].Value;
                    if (BnodeItem.Attributes[i].Name == "暂定金额")
                        material.BProvisionalSums = BnodeItem.Attributes[i].Value;
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
            if (material.TProvisionalSums.Replace(" ", "") != material.BProvisionalSums.Replace(" ", ""))
                b = false;
            if (!b)
                b = false;
            if (b)
                material.CheckResult = "正确";
            else
            {
                material.Foreground = Brushes.Red;
                material.CheckResult = "错误";
            }
            return material;
        }

    }
}
