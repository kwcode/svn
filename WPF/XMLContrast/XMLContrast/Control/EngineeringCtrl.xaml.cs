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
    public partial class EngineeringCtrl : HLUserControlExt
    {
        public EngineeringCtrl()
        {
            InitializeComponent();

        }
        public EngineeringCtrl(List<XMLNodeItem> tenderDataNodes, List<XMLNodeItem> bidDataNodes, object owner)
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
                //List<XMLNodeItem> xmlTenderNodeItems = XMLOperat.GetDataChildNodes("单项工程", tenderDataNodes);
                //List<XMLNodeItem> xmlBidNodeItems = XMLOperat.GetDataChildNodes("单项工程", bidDataNodes);
                Dictionary<XMLNodeItem, List<XMLNodeItem>> dicTenderNodes = StructureTenderNodes("单项工程", "单位工程", tenderDataNodes);
                Dictionary<XMLNodeItem, List<XMLNodeItem>> dicbidNodes = StructureTenderNodes("单项工程", "单位工程", bidDataNodes);
                List<FeesRates> Tdataitems = AnalysisData(dicTenderNodes, true);//招标
                List<FeesRates> Bdataitems = AnalysisData(dicbidNodes, false);//招标
                List<FeesRates> ContrastDataItems = ContrastDatas(Tdataitems, Bdataitems);

                ListCollectionView dataList = new ListCollectionView(ContrastDataItems);
                dataList.GroupDescriptions.Add(new PropertyGroupDescription("GroupName"));
                Global.SysContext.Send(a =>
                {
                    lv_DataView.ItemsSource = dataList;
                    (owner as HLWindowExt).StopProgress();
                }, null);
            }).Start();


        }

        private List<FeesRates> ContrastDatas(List<FeesRates> Tdataitems, List<FeesRates> Bdataitems)
        {
            List<FeesRates> list = new List<FeesRates>();
            if (Tdataitems.Count >= Bdataitems.Count)
            {
                for (int i = 0; i < Tdataitems.Count; i++)
                {
                    FeesRates feesrate = new FeesRates();

                    feesrate = Tdataitems[i];
                    if (Bdataitems != null && Bdataitems.Count != 0)
                    {
                        if (Bdataitems[i] != null)
                        {
                            feesrate.BNumber = Bdataitems[i].BNumber;
                            feesrate.BProjectName = Bdataitems[i].BProjectName;
                            feesrate.BCalculatedBasis = Bdataitems[i].BCalculatedBasis;
                            feesrate.BRate = Bdataitems[i].BRate;
                        }
                    }
                    bool b = true;
                    //if (feesrate.TNumber.Replace(" ", "") != feesrate.BNumber.Replace(" ", ""))
                    //    b = false;
                    if (feesrate.TProjectName.Replace(" ", "") != feesrate.BProjectName.Replace(" ", ""))
                        b = false;
                    //if (feesrate.TCalculatedBasis.Replace(" ", "") != feesrate.BCalculatedBasis.Replace(" ", ""))
                    //    b = false;
                    //if (feesrate.TRate.Replace(" ", "") != feesrate.BRate.Replace(" ", ""))
                    //    b = false;

                    if (b)
                        feesrate.CheckResult = "正确";
                    else
                    {
                        feesrate.Foreground = Brushes.Red;
                        feesrate.CheckResult = "错误";
                    }
                    feesrate.ItemIcon = "/XMLContrast;component/Resources/gc.ico";
                    list.Add(feesrate);
                }
            }
            else
            {
                for (int i = 0; i < Bdataitems.Count; i++)
                {
                    FeesRates feesrate = new FeesRates();

                    feesrate = Bdataitems[i];
                    if (Tdataitems != null && Tdataitems.Count != 0)
                    {
                        if (Bdataitems[i] != null)
                        {
                            feesrate.TNumber = Tdataitems[i].TNumber;
                            feesrate.TProjectName = Tdataitems[i].TProjectName;
                            feesrate.TCalculatedBasis = Tdataitems[i].TCalculatedBasis;
                            feesrate.TRate = Tdataitems[i].TRate;
                        }
                    }
                    bool b = true;
                    if (feesrate.TNumber.Replace(" ", "") != feesrate.BNumber.Replace(" ", ""))
                        b = false;
                    if (feesrate.TProjectName.Replace(" ", "") != feesrate.BProjectName.Replace(" ", ""))
                        b = false;
                    //if (feesrate.TCalculatedBasis.Replace(" ", "") != feesrate.BCalculatedBasis.Replace(" ", ""))
                    //    b = false;
                    if (feesrate.TRate.Replace(" ", "") != feesrate.BRate.Replace(" ", ""))
                        b = false;

                    if (b)
                        feesrate.CheckResult = "正确";
                    else
                    {
                        feesrate.Foreground = Brushes.Red;
                        feesrate.CheckResult = "错误";
                    }
                    feesrate.ItemIcon = "/XMLContrast;component/Resources/gc.ico";
                    list.Add(feesrate);
                }
            }
            return list;
        }

        private List<FeesRates> AnalysisData(Dictionary<XMLNodeItem, List<XMLNodeItem>> dicNodes, bool isT)
        {
            List<FeesRates> list = new List<FeesRates>();
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
                        FeesRates feesrates = new FeesRates();
                        feesrates.PrimaryKey = index;
                        for (int i = 0; i < node.Attributes.Count; i++)
                        {
                            if (node.Attributes[i].Name == "名称")
                                feesrates.TProjectName = node.Attributes[i].Value;
                            feesrates.TRate = node.Attributes[i].Value;
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
                        FeesRates feesrates = new FeesRates();
                        feesrates.PrimaryKey = index;
                        for (int i = 0; i < node.Attributes.Count; i++)
                        {
                            if (node.Attributes[i].Name == "编码")
                                feesrates.BNumber = node.Attributes[i].Value;
                            if (node.Attributes[i].Name == "名称")
                                feesrates.BProjectName = node.Attributes[i].Value;
                            if (node.Attributes[i].Name == "取费基础")
                                feesrates.BCalculatedBasis = node.Attributes[i].Value;
                            if (node.Attributes[i].Name == "费率")
                                feesrates.BRate = node.Attributes[i].Value;
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
