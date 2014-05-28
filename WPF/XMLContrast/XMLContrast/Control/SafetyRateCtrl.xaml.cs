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
using System.Threading;
using System.Windows.Threading;

namespace XMLContrast.Control
{
    /// <summary>
    /// FeesRatesCtrl.xaml 的交互逻辑
    /// </summary>
    public partial class SafetyRateCtrl : HLUserControlExt
    {
        public SafetyRateCtrl()
        {
            InitializeComponent();

        }
        public SafetyRateCtrl(List<XMLNodeItem> tenderDataNodes, List<XMLNodeItem> bidDataNodes, object owner)
        {
            Global.SysContext = DispatcherSynchronizationContext.Current;
            InitializeComponent();
            Init(tenderDataNodes, bidDataNodes, owner);
        }
        public void Init(List<XMLNodeItem> tenderDataNodes, List<XMLNodeItem> bidDataNodes, object owner)
        {
            (owner as HLWindowExt).StartProgress();

            new Thread(o =>
            {
                // 清单子目  单位工程 
                List<XMLNodeItem> xmlTenderNodeItems = XMLOperat.GetDataChildNodes("单位工程", tenderDataNodes);
                List<XMLNodeItem> xmlBidNodeItems = XMLOperat.GetDataChildNodes("单位工程", bidDataNodes);
                Dictionary<XMLNodeItem, List<XMLNodeItem>> dicTenderNodes = StructureTenderNodes("单位工程", "安全文明施工费", tenderDataNodes);
                Dictionary<XMLNodeItem, List<XMLNodeItem>> dicbidNodes = StructureTenderNodes("单位工程", "安全文明施工费", bidDataNodes);
                List<SafetyRate> Tdataitems = AnalysisData(dicTenderNodes, true);//招标
                List<SafetyRate> Bdataitems = AnalysisData(dicbidNodes, false);//招标
                List<SafetyRate> ContrastDataItems = ContrastDatas(Tdataitems, Bdataitems);

                Global.SysContext.Send(a =>
                {
                    lv_DataView.ItemsSource = ContrastDataItems;
                    (owner as HLWindowExt).StopProgress();
                }, null);
            }).Start();


        }

        private List<SafetyRate> ContrastDatas(List<SafetyRate> Tdataitems, List<SafetyRate> Bdataitems)
        {
            List<SafetyRate> list = new List<SafetyRate>();
            if (Tdataitems.Count >= Bdataitems.Count)
            {
                for (int i = 0; i < Tdataitems.Count; i++)
                {
                    SafetyRate feesrate = new SafetyRate();
                    feesrate = Tdataitems[i];
                    if (Bdataitems != null && Bdataitems.Count != 0)
                    {
                        if (Bdataitems[i] != null)
                        {
                            feesrate.BRate = Bdataitems[i].BRate;
                            feesrate.BStructure = Bdataitems[i].BStructure;
                        }
                    }
                    bool b = true;
                    if (feesrate.TRate.Replace(" ", "") != feesrate.BRate.Replace(" ", ""))
                        b = false;
                    if (feesrate.TStructure.Replace(" ", "") != feesrate.BStructure.Replace(" ", ""))
                        b = false;

                    if (b)
                        feesrate.CheckResult = "正确";
                    else
                    {
                        feesrate.CheckResult = "错误";
                        feesrate.Foreground = Brushes.Red;
                    }
                    feesrate.ItemIcon = "/XMLContrast;component/Resources/gc.ico";
                    list.Add(feesrate);
                }
            }
            else
            {
                for (int i = 0; i < Bdataitems.Count; i++)
                {
                    SafetyRate feesrate = new SafetyRate();
                    feesrate = Bdataitems[i];
                    if (Tdataitems != null && Tdataitems.Count != 0)
                    {
                        if (Tdataitems[i] != null)
                        {
                            feesrate.TRate = Tdataitems[i].TRate;
                            feesrate.TStructure = Tdataitems[i].TStructure;
                        }
                    }
                    bool b = true;
                    if (feesrate.TRate.Replace(" ", "") != feesrate.BRate.Replace(" ", ""))
                        b = false;
                    if (feesrate.TStructure.Replace(" ", "") != feesrate.BStructure.Replace(" ", ""))
                        b = false;

                    if (b)
                        feesrate.CheckResult = "正确";
                    else
                    {
                        feesrate.CheckResult = "错误";
                        feesrate.Foreground = Brushes.Red;
                    }
                    feesrate.ItemIcon = "/XMLContrast;component/Resources/gc.ico";
                    list.Add(feesrate);
                }
            }
            return list;
        }

        private List<SafetyRate> AnalysisData(Dictionary<XMLNodeItem, List<XMLNodeItem>> dicNodes, bool isT)
        {
            List<SafetyRate> list = new List<SafetyRate>();
            if (isT)
            {
                if (dicNodes == null || dicNodes.Count == 0) return list;
                int index = 1;
                foreach (KeyValuePair<XMLNodeItem, List<XMLNodeItem>> item in dicNodes)
                {
                    string groupName = "";
                    for (int i = 0; i < item.Key.Attributes.Count; i++)
                    {
                        if (item.Key.Attributes[i].Name == "名称")
                            groupName = item.Key.Attributes[i].Value;
                    }
                    foreach (XMLNodeItem node in item.Value)
                    {
                        SafetyRate feesrates = new SafetyRate();
                        feesrates.PrimaryKey = index;
                        for (int i = 0; i < node.Attributes.Count; i++)
                        {
                            if (node.Attributes[i].Name == "费率")
                                feesrates.TRate = node.Attributes[i].Value;
                        }
                        index++;
                        feesrates.TStructure = groupName;
                        list.Add(feesrates);
                    }
                }
            }
            else
            {
                if (dicNodes == null || dicNodes.Count == 0) return list;
                int index = 1;
                foreach (KeyValuePair<XMLNodeItem, List<XMLNodeItem>> item in dicNodes)
                {
                    string groupName = "";
                    for (int i = 0; i < item.Key.Attributes.Count; i++)
                    {
                        if (item.Key.Attributes[i].Name == "名称")
                            groupName = item.Key.Attributes[i].Value;
                    }
                    foreach (XMLNodeItem node in item.Value)
                    {
                        SafetyRate feesrates = new SafetyRate();
                        feesrates.PrimaryKey = index;
                        for (int i = 0; i < node.Attributes.Count; i++)
                        {
                            if (node.Attributes[i].Name == "费率")
                                feesrates.BRate = node.Attributes[i].Value;
                        }
                        index++;
                        feesrates.BStructure = groupName;
                        list.Add(feesrates);
                    }
                }
            }
            return list;
        }


        private Dictionary<XMLNodeItem, List<XMLNodeItem>> StructureTenderNodes(string groupName, string name, List<XMLNodeItem> dataNodes)
        {
            Dictionary<XMLNodeItem, List<XMLNodeItem>> diclist = new Dictionary<XMLNodeItem, List<XMLNodeItem>>();
            List<XMLNodeItem> keys = XMLOperat.GetDataChildNodes(groupName, dataNodes);
            foreach (XMLNodeItem item in keys)
            {
                List<XMLNodeItem> values = XMLOperat.GetDataChildNodes(name, item.ChildNodes);
                diclist.Add(item, values);
            }
            return diclist;
        }

    }
}
