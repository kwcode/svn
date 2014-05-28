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
using System.Collections.ObjectModel;

namespace XMLContrast.Control
{
    /// <summary>
    /// FeesRatesCtrl.xaml 的交互逻辑
    /// </summary>
    public partial class 总承包服务费子目 : HLUserControlExt
    {
        public 总承包服务费子目()
        {
            InitializeComponent();

        }
        public 总承包服务费子目(List<XMLNodeItem> tenderDataNodes, List<XMLNodeItem> bidDataNodes, object owner)
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

                Dictionary<XMLNodeItem, Dictionary<XMLNodeItem, List<XMLNodeItem>>> dicTNodes = XMLOperat.GetTreeListNodes("单项工程", "单位工程", "总承包服务费子目", tenderDataNodes);
                Dictionary<XMLNodeItem, Dictionary<XMLNodeItem, List<XMLNodeItem>>> dicBNodes = XMLOperat.GetTreeListNodes("单项工程", "单位工程", "总承包服务费子目", bidDataNodes);
                List<TreeFeesObj总承包服务费子目> Tdataitems = AnalysisData(dicTNodes, true);//招标
                List<TreeFeesObj总承包服务费子目> Bdataitems = AnalysisData(dicBNodes, false);//招标
                List<TreeFeesObjTB总承包服务费子目> dataList = ContrastDatas(Tdataitems, Bdataitems);
                Global.SysContext.Send(a =>
                            {
                                if (dataList.Count > 0)
                                {
                                    dataList[0].IsExpanded = true;
                                    if (dataList[0].Children.Count > 0)
                                        dataList[0].Children[0].IsExpanded = true;
                                }
                                lv_DataView.ItemsSource = dataList;
                                (owner as HLWindowExt).StopProgress();
                            }, null);
            }).Start();


        }

        private List<TreeFeesObjTB总承包服务费子目> ContrastDatas(List<TreeFeesObj总承包服务费子目> Tdataitems, List<TreeFeesObj总承包服务费子目> Bdataitems)
        {
            List<TreeFeesObjTB总承包服务费子目> list = new List<TreeFeesObjTB总承包服务费子目>();
            if (Tdataitems.Count >= Bdataitems.Count)
            {
                for (int i = 0; i < Tdataitems.Count; i++)
                {
                    TreeFeesObjTB总承包服务费子目 obj = new TreeFeesObjTB总承包服务费子目();
                    TreeFeesObj总承包服务费子目 TObj = Tdataitems[i];
                    obj.GroupName = TObj.GroupName;
                    obj.TRate = TObj.Rate;
                    obj.TName = TObj.Name;
                    obj.TServices = TObj.Services;
                    if (Bdataitems != null && Bdataitems.Count != 0)
                    {
                        if (i >= Bdataitems.Count) continue;
                        if (Bdataitems[i] != null)
                        {
                            TreeFeesObj总承包服务费子目 BObj = Bdataitems[i];
                            obj.BRate = BObj.Rate;
                            obj.BName = BObj.Name;
                            obj.BServices = BObj.Services;
                            CreateTreeDatas(obj.Children, TObj.Children, BObj.Children);
                        }
                    }
                    bool b = true;
                    if (obj.TRate.Replace(" ", "") != obj.BRate.Replace(" ", ""))
                        b = false;
                    if (obj.TName.Replace(" ", "") != obj.BName.Replace(" ", ""))
                        b = false;
                    if (obj.TServices.Replace(" ", "") != obj.BServices.Replace(" ", ""))
                        b = false;
                    if (obj.TPrice.Replace(" ", "") != obj.BPrice.Replace(" ", ""))
                        b = false;
                    if (b)
                        obj.CheckResult = "正确";
                    else
                    {
                        obj.Foreground = Brushes.Red;
                        obj.CheckResult = "错误";
                    }
                    obj.ItemIcon = "/XMLContrast;component/Resources/gc.ico";
                    obj.Margin = new Thickness(2);
                    list.Add(obj);
                }
            }
            else
            {
                for (int i = 0; i < Bdataitems.Count; i++)
                {
                    TreeFeesObjTB总承包服务费子目 obj = new TreeFeesObjTB总承包服务费子目();
                    TreeFeesObj总承包服务费子目 BObj = Bdataitems[i];
                    obj.BRate = BObj.Rate;
                    obj.BName = BObj.Name;
                    obj.BServices = BObj.Services;
                    if (Tdataitems != null && Tdataitems.Count != 0)
                    {
                        if (i >= Tdataitems.Count) continue;
                        if (Tdataitems[i] != null)
                        {
                            TreeFeesObj总承包服务费子目 TObj = Tdataitems[i];
                            obj.GroupName = TObj.GroupName;
                            obj.TRate = TObj.Rate;
                            obj.TName = TObj.Name;
                            obj.TServices = TObj.Services;
                            CreateTreeDatas(obj.Children, TObj.Children, BObj.Children);
                        }
                    }
                    bool b = true;
                    if (obj.TRate.Replace(" ", "") != obj.BRate.Replace(" ", ""))
                        b = false;
                    if (obj.TName.Replace(" ", "") != obj.BName.Replace(" ", ""))
                        b = false;
                    if (obj.TServices.Replace(" ", "") != obj.BServices.Replace(" ", ""))
                        b = false;
                    if (obj.TPrice.Replace(" ", "") != obj.BPrice.Replace(" ", ""))
                        b = false;
                    if (b)
                        obj.CheckResult = "正确";
                    else
                    {
                        obj.Foreground = Brushes.Red;
                        obj.CheckResult = "错误";
                    }
                    obj.ItemIcon = "/XMLContrast;component/Resources/gc.ico";
                    obj.Margin = new Thickness(2);
                    list.Add(obj);
                }
            }
            return list;
        }

        private void CreateTreeDatas(ObservableCollection<TreeFeesObjTB总承包服务费子目> dataChilds, ObservableCollection<TreeFeesObj总承包服务费子目> Tdataitems, ObservableCollection<TreeFeesObj总承包服务费子目> Bdataitems)
        {
            if (Tdataitems.Count >= Bdataitems.Count)
            {
                for (int i = 0; i < Tdataitems.Count; i++)
                {
                    TreeFeesObjTB总承包服务费子目 obj = new TreeFeesObjTB总承包服务费子目();
                    TreeFeesObj总承包服务费子目 TObj = Tdataitems[i];
                    obj.GroupName = TObj.GroupName;
                    obj.TRate = TObj.Rate;
                    obj.TName = TObj.Name;
                    obj.TServices = TObj.Services;
                    obj.TPrice = TObj.Price;
                    if (Bdataitems != null && Bdataitems.Count != 0)
                    {
                        if (i >= Bdataitems.Count) continue;
                        if (Bdataitems[i] != null)
                        {
                            TreeFeesObj总承包服务费子目 BObj = Bdataitems[i];
                            obj.BRate = BObj.Rate;
                            obj.BName = BObj.Name;
                            obj.BServices = BObj.Services;
                            obj.BPrice = BObj.Price;
                            CreateTreeDatas(obj.Children, TObj.Children, BObj.Children);
                        }

                    }
                    bool b = true;
                    if (obj.TRate.Replace(" ", "") != obj.BRate.Replace(" ", ""))
                        b = false;
                    if (obj.TName.Replace(" ", "") != obj.BName.Replace(" ", ""))
                        b = false;
                    if (obj.TServices.Replace(" ", "") != obj.BServices.Replace(" ", ""))
                        b = false;
                    if (obj.TPrice.Replace(" ", "") != obj.BPrice.Replace(" ", ""))
                        b = false;
                    if (b)
                        obj.CheckResult = "正确";
                    else
                    {
                        obj.Foreground = Brushes.Red;
                        obj.CheckResult = "错误";
                    }
                    obj.ItemIcon = "/XMLContrast;component/Resources/gc.ico";
                    obj.Margin = new Thickness(10, 2, 2, 2);
                    dataChilds.Add(obj);
                }
            }
            else
            {
                for (int i = 0; i < Bdataitems.Count; i++)
                {
                    TreeFeesObjTB总承包服务费子目 obj = new TreeFeesObjTB总承包服务费子目();
                    TreeFeesObj总承包服务费子目 BObj = Bdataitems[i];
                    obj.BRate = BObj.Rate;
                    obj.BName = BObj.Name;
                    obj.BServices = BObj.Services;
                    obj.BPrice = BObj.Price;
                    if (Tdataitems != null && Tdataitems.Count != 0)
                    {
                        if (i >= Tdataitems.Count) continue;
                        if (Tdataitems[i] != null)
                        {
                            TreeFeesObj总承包服务费子目 TObj = Tdataitems[i];
                            obj.GroupName = TObj.GroupName;
                            obj.TRate = TObj.Rate;
                            obj.TName = TObj.Name;
                            obj.TServices = TObj.Services;
                            obj.TPrice = TObj.Price;
                            CreateTreeDatas(obj.Children, TObj.Children, BObj.Children);
                        }
                    }
                    bool b = true;
                    if (obj.TRate.Replace(" ", "") != obj.BRate.Replace(" ", ""))
                        b = false;
                    if (obj.TName.Replace(" ", "") != obj.BName.Replace(" ", ""))
                        b = false;
                    if (obj.TServices.Replace(" ", "") != obj.BServices.Replace(" ", ""))
                        b = false;
                    if (b)
                        obj.CheckResult = "正确";
                    else
                    {
                        obj.Foreground = Brushes.Red;
                        obj.CheckResult = "错误";
                    }
                    obj.ItemIcon = "/XMLContrast;component/Resources/gc.ico";
                    obj.Margin = new Thickness(10, 2, 2, 2);
                    dataChilds.Add(obj);
                }
            }
        }

        private List<TreeFeesObj总承包服务费子目> AnalysisData(Dictionary<XMLNodeItem, Dictionary<XMLNodeItem, List<XMLNodeItem>>> dicNodes, bool isT)
        {
            List<TreeFeesObj总承包服务费子目> list = new List<TreeFeesObj总承包服务费子目>();
            if (isT)
            {
                if (dicNodes == null || dicNodes.Count == 0) return list;
                foreach (KeyValuePair<XMLNodeItem, Dictionary<XMLNodeItem, List<XMLNodeItem>>> item in dicNodes)
                {
                    TreeFeesObj总承包服务费子目 rootTree = new TreeFeesObj总承包服务费子目();
                    string root = "";
                    for (int i = 0; i < item.Key.Attributes.Count; i++)
                    {
                        if (item.Key.Attributes[i].Name == "名称")
                            root = item.Key.Attributes[i].Value;

                    }
                    rootTree.GroupName = root;
                    foreach (KeyValuePair<XMLNodeItem, List<XMLNodeItem>> child in item.Value)
                    {
                        TreeFeesObj总承包服务费子目 secondTree = new TreeFeesObj总承包服务费子目();
                        string second = "";
                        for (int i = 0; i < child.Key.Attributes.Count; i++)
                        {
                            if (child.Key.Attributes[i].Name == "名称")
                                second = child.Key.Attributes[i].Value;
                        }
                        secondTree.GroupName = second;
                        foreach (XMLNodeItem node in child.Value)
                        {
                            TreeFeesObj总承包服务费子目 obj = new TreeFeesObj总承包服务费子目();
                            for (int i = 0; i < node.Attributes.Count; i++)
                            {
                                if (node.Attributes[i].Name == "费率")
                                    obj.Rate = node.Attributes[i].Value;
                                if (node.Attributes[i].Name == "项目名称")
                                    obj.Name = node.Attributes[i].Value;
                                if (node.Attributes[i].Name == "服务内容")
                                    obj.Services = node.Attributes[i].Value;
                                if (node.Attributes[i].Name == "金额")
                                    obj.Price = node.Attributes[i].Value;
                            }
                            secondTree.Children.Add(obj);
                        }
                        rootTree.Children.Add(secondTree);
                    }
                    list.Add(rootTree);
                }
            }
            else
            {
                if (dicNodes == null || dicNodes.Count == 0) return list;
                foreach (KeyValuePair<XMLNodeItem, Dictionary<XMLNodeItem, List<XMLNodeItem>>> item in dicNodes)
                {
                    TreeFeesObj总承包服务费子目 rootTree = new TreeFeesObj总承包服务费子目();
                    string root = "";
                    for (int i = 0; i < item.Key.Attributes.Count; i++)
                    {
                        if (item.Key.Attributes[i].Name == "名称")
                            root = item.Key.Attributes[i].Value;

                    }
                    rootTree.GroupName = root;
                    foreach (KeyValuePair<XMLNodeItem, List<XMLNodeItem>> child in item.Value)
                    {
                        TreeFeesObj总承包服务费子目 secondTree = new TreeFeesObj总承包服务费子目();
                        string second = "";
                        for (int i = 0; i < child.Key.Attributes.Count; i++)
                        {
                            if (child.Key.Attributes[i].Name == "名称")
                                second = child.Key.Attributes[i].Value;
                        }
                        secondTree.GroupName = second;
                        foreach (XMLNodeItem node in child.Value)
                        {
                            TreeFeesObj总承包服务费子目 obj = new TreeFeesObj总承包服务费子目();
                            for (int i = 0; i < node.Attributes.Count; i++)
                            {
                                if (node.Attributes[i].Name == "费率")
                                    obj.Rate = node.Attributes[i].Value;
                                if (node.Attributes[i].Name == "项目名称")
                                    obj.Name = node.Attributes[i].Value;
                                if (node.Attributes[i].Name == "服务内容")
                                    obj.Services = node.Attributes[i].Value;
                                if (node.Attributes[i].Name == "金额")
                                    obj.Price = node.Attributes[i].Value;
                            }
                            secondTree.Children.Add(obj);
                        }
                        rootTree.Children.Add(secondTree);
                    }
                    list.Add(rootTree);
                }
            }
            return list;
        }

    }
    public class TreeFeesObj总承包服务费子目
    {
        public TreeFeesObj总承包服务费子目() { _children = new ObservableCollection<TreeFeesObj总承包服务费子目>(); }
        private string _Rate = string.Empty;
        private string _Name = string.Empty;
        private string _Services = string.Empty;
        private string _GroupName = string.Empty;
        private string _Price = string.Empty;


        public string Rate { get { return _Rate; } set { _Rate = value; } }
        public string Name { get { return _Name; } set { _Name = value; } }
        public string Services { get { return _Services; } set { _Services = value; } }
        public string GroupName { get { return _GroupName; } set { _GroupName = value; } }
        public string Price { get { return _Price; } set { _Price = value; } }

        private ObservableCollection<TreeFeesObj总承包服务费子目> _children;
        public ObservableCollection<TreeFeesObj总承包服务费子目> Children
        {
            get { return _children; }
            set { _children = value; }
        }
    }
    public class TreeFeesObjTB总承包服务费子目
    {
        public TreeFeesObjTB总承包服务费子目() { _children = new ObservableCollection<TreeFeesObjTB总承包服务费子目>(); }
        private string _TRate = string.Empty;
        private string _TName = string.Empty;
        private string _TServices = string.Empty;
        private string _TPrice = string.Empty;

        private string _BRate = string.Empty;
        private string _BName = string.Empty;
        private string _BSpecification = string.Empty;
        private string _BPrice = string.Empty;
        private Brush _Foreground = Brushes.Black;
        private string _ItemIcon = "/XMLContrast;component/Resources/gc.ico";
        private Thickness _Margin = new Thickness(2);

        public string TRate { get { return _TRate; } set { _TRate = value; } }
        public string TName { get { return _TName; } set { _TName = value; } }
        public string TServices { get { return _TServices; } set { _TServices = value; } }
        public string TPrice { get { return _TPrice; } set { _TPrice = value; } }

        public string BRate { get { return _BRate; } set { _BRate = value; } }
        public string BName { get { return _BName; } set { _BName = value; } }
        public string BServices { get { return _BSpecification; } set { _BSpecification = value; } }
        public string BPrice { get { return _BPrice; } set { _BPrice = value; } }

        public string CheckResult { get; set; }
        public string GroupName { get; set; }
        public string ItemIcon { get { return _ItemIcon; } set { _ItemIcon = value; } }
        public Brush Foreground { get { return _Foreground; } set { _Foreground = value; } }
        public Thickness Margin { get { return _Margin; } set { _Margin = value; } }
        private bool _IsExpanded = false;
        public bool IsExpanded { get { return _IsExpanded; } set { _IsExpanded = value; } }
        private ObservableCollection<TreeFeesObjTB总承包服务费子目> _children;
        public ObservableCollection<TreeFeesObjTB总承包服务费子目> Children
        {
            get { return _children; }
            set { _children = value; }
        }
    }
}
