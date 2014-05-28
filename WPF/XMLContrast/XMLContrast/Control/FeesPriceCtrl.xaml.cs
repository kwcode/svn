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
    public partial class FeesPriceCtrl : HLUserControlExt
    {
        public FeesPriceCtrl()
        {
            InitializeComponent();

        }
        public FeesPriceCtrl(List<XMLNodeItem> tenderDataNodes, List<XMLNodeItem> bidDataNodes, object owner)
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
                Dictionary<XMLNodeItem, List<XMLNodeItem>> dicTenderNodes = StructureTenderNodes("单位工程", "清单子目", tenderDataNodes);
                Dictionary<XMLNodeItem, List<XMLNodeItem>> dicbidNodes = StructureTenderNodes("单位工程", "清单子目", bidDataNodes);
                List<UnitPrice> Tdataitems = AnalysisData(dicTenderNodes, true);//招标
                List<UnitPrice> Bdataitems = AnalysisData(dicbidNodes, false);//招标
                List<UnitPrice> ContrastDataItems = ContrastDatas(Tdataitems, Bdataitems);

                ListCollectionView dataList = new ListCollectionView(ContrastDataItems);
                dataList.GroupDescriptions.Add(new PropertyGroupDescription("GroupName"));
                Global.SysContext.Send(a =>
                {
                    lv_DataView.ItemsSource = dataList;
                    (owner as HLWindowExt).StopProgress();
                }, null);
            }).Start();


        }

        private List<UnitPrice> ContrastDatas(List<UnitPrice> Tdataitems, List<UnitPrice> Bdataitems)
        {
            List<UnitPrice> list = new List<UnitPrice>();
            if (Tdataitems.Count >= Bdataitems.Count)
            {
                for (int i = 0; i < Tdataitems.Count; i++)
                {
                    UnitPrice feesrate = new UnitPrice();

                    feesrate = Tdataitems[i];
                    if (Bdataitems != null && Bdataitems.Count != 0)
                    {
                        if (Bdataitems[i] != null)
                        {
                            feesrate.BName = Bdataitems[i].BName;
                            feesrate.BEngineer = Bdataitems[i].BEngineer;
                            feesrate.BPrice = Bdataitems[i].BPrice;
                        }
                    }
                    bool b = true;
                    if (feesrate.TName.Replace(" ", "") != feesrate.BName.Replace(" ", ""))
                        b = false;
                    if (feesrate.TEngineer.Replace(" ", "") != feesrate.BEngineer.Replace(" ", ""))
                        b = false;
                    if (feesrate.TPrice.Replace(" ", "") != feesrate.BPrice.Replace(" ", ""))
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
                    UnitPrice feesrate = new UnitPrice();
                    feesrate = Bdataitems[i];
                    if (Tdataitems != null && Tdataitems.Count != 0)
                    {
                        if (Bdataitems[i] != null)
                        {
                            feesrate.TName = Tdataitems[i].TName;
                            feesrate.TEngineer = Tdataitems[i].TEngineer;
                            feesrate.TPrice = Tdataitems[i].TPrice;
                        }
                    }
                    bool b = true;
                    if (feesrate.TName.Replace(" ", "") != feesrate.BName.Replace(" ", ""))
                        b = false;
                    if (feesrate.TEngineer.Replace(" ", "") != feesrate.BEngineer.Replace(" ", ""))
                        b = false;
                    if (feesrate.TPrice.Replace(" ", "") != feesrate.BPrice.Replace(" ", ""))
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

        private List<UnitPrice> AnalysisData(Dictionary<XMLNodeItem, List<XMLNodeItem>> dicNodes, bool isT)
        {
            List<UnitPrice> list = new List<UnitPrice>();
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
                        UnitPrice feesrates = new UnitPrice();
                        feesrates.PrimaryKey = index;
                        for (int i = 0; i < node.Attributes.Count; i++)
                        {
                            if (node.Attributes[i].Name == "项目名称")
                                feesrates.TName = node.Attributes[i].Value;
                            if (node.Attributes[i].Name == "综合单价")
                                feesrates.TPrice = node.Attributes[i].Value;
                            if (node.Attributes[i].Name == "工程量")
                                feesrates.TEngineer = node.Attributes[i].Value;
                        }
                        feesrates.GroupName = groupName;
                        index++;
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
                        UnitPrice feesrates = new UnitPrice();
                        feesrates.PrimaryKey = index;
                        for (int i = 0; i < node.Attributes.Count; i++)
                        {
                            if (node.Attributes[i].Name == "项目名称")
                                feesrates.BName = node.Attributes[i].Value;
                            if (node.Attributes[i].Name == "综合单价")
                                feesrates.BPrice = node.Attributes[i].Value;
                            if (node.Attributes[i].Name == "工程量")
                                feesrates.BEngineer = node.Attributes[i].Value;
                        }
                        feesrates.GroupName = groupName;
                        index++;
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
