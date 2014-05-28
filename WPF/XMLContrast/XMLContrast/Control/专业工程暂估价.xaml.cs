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
    public partial class 专业工程暂估价 : HLUserControlExt
    {
        public 专业工程暂估价()
        {
            InitializeComponent();

        }
        public 专业工程暂估价(List<XMLNodeItem> tenderDataNodes, List<XMLNodeItem> bidDataNodes, object owner)
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

                Dictionary<XMLNodeItem, Dictionary<XMLNodeItem, List<XMLNodeItem>>> dicTNodes = XMLOperat.GetTreeListNodes("单项工程", "单位工程", "专业工程暂估价", tenderDataNodes);
                Dictionary<XMLNodeItem, Dictionary<XMLNodeItem, List<XMLNodeItem>>> dicBNodes = XMLOperat.GetTreeListNodes("单项工程", "单位工程", "专业工程暂估价", bidDataNodes);
                List<TreeFeesObj专业工程暂估价> Tdataitems = AnalysisData(dicTNodes, true);//招标
                List<TreeFeesObj专业工程暂估价> Bdataitems = AnalysisData(dicBNodes, false);//招标
                List<TreeFeesObjTB专业工程暂估价> dataList = ContrastDatas(Tdataitems, Bdataitems);
                Global.SysContext.Send(a =>
                            {
                                lv_DataView.ItemsSource = dataList;
                                (owner as HLWindowExt).StopProgress();
                            }, null);
            }).Start();


        }

        private List<TreeFeesObjTB专业工程暂估价> ContrastDatas(List<TreeFeesObj专业工程暂估价> Tdataitems, List<TreeFeesObj专业工程暂估价> Bdataitems)
        {
            List<TreeFeesObjTB专业工程暂估价> list = new List<TreeFeesObjTB专业工程暂估价>();
            if (Tdataitems.Count >= Bdataitems.Count)
            {
                for (int i = 0; i < Tdataitems.Count; i++)
                {
                    TreeFeesObjTB专业工程暂估价 obj = new TreeFeesObjTB专业工程暂估价();
                    TreeFeesObj专业工程暂估价 TObj = Tdataitems[i];
                    obj.GroupName = TObj.GroupName;
                    obj.TName = TObj.Name;
                    if (Bdataitems != null && Bdataitems.Count != 0)
                    {
                        if (i >= Bdataitems.Count) continue;
                        if (Bdataitems[i] != null)
                        {
                            TreeFeesObj专业工程暂估价 BObj = Bdataitems[i];
                            obj.BName = BObj.Name;
                            CreateTreeDatas(obj.Children, TObj.Children, BObj.Children);
                        }
                    }
                    bool b = true;
                    if (obj.TName.Replace(" ", "") != obj.BName.Replace(" ", ""))
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
                    TreeFeesObjTB专业工程暂估价 obj = new TreeFeesObjTB专业工程暂估价();
                    TreeFeesObj专业工程暂估价 BObj = Bdataitems[i];
                    obj.BName = BObj.Name;
                    if (Tdataitems != null && Tdataitems.Count != 0)
                    {
                        if (i >= Tdataitems.Count) continue;
                        if (Tdataitems[i] != null)
                        {
                            TreeFeesObj专业工程暂估价 TObj = Tdataitems[i];
                            obj.GroupName = TObj.GroupName;
                            obj.TName = TObj.Name;
                            CreateTreeDatas(obj.Children, TObj.Children, BObj.Children);
                        }
                    }
                    bool b = true;
                    if (obj.TName.Replace(" ", "") != obj.BName.Replace(" ", ""))
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

        private void CreateTreeDatas(ObservableCollection<TreeFeesObjTB专业工程暂估价> dataChilds, ObservableCollection<TreeFeesObj专业工程暂估价> Tdataitems, ObservableCollection<TreeFeesObj专业工程暂估价> Bdataitems)
        {
            if (Tdataitems.Count >= Bdataitems.Count)
            {
                for (int i = 0; i < Tdataitems.Count; i++)
                {
                    TreeFeesObjTB专业工程暂估价 obj = new TreeFeesObjTB专业工程暂估价();
                    TreeFeesObj专业工程暂估价 TObj = Tdataitems[i];
                    obj.GroupName = TObj.GroupName;
                    obj.TName = TObj.Name;
                    obj.TPrice = TObj.Price;
                    if (Bdataitems != null && Bdataitems.Count != 0)
                    {
                        if (i >= Bdataitems.Count) continue;
                        if (Bdataitems[i] != null)
                        {
                            TreeFeesObj专业工程暂估价 BObj = Bdataitems[i];
                            obj.BName = BObj.Name;
                            obj.BPrice = BObj.Price;
                            CreateTreeDatas(obj.Children, TObj.Children, BObj.Children);
                        }

                    }
                    bool b = true;
                    if (obj.TName.Replace(" ", "") != obj.BName.Replace(" ", ""))
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
                    TreeFeesObjTB专业工程暂估价 obj = new TreeFeesObjTB专业工程暂估价();
                    TreeFeesObj专业工程暂估价 BObj = Bdataitems[i];
                    obj.BName = BObj.Name;
                    obj.BPrice = BObj.Price;
                    if (Tdataitems != null && Tdataitems.Count != 0)
                    {
                        if (i >= Tdataitems.Count) continue;
                        if (Tdataitems[i] != null)
                        {
                            TreeFeesObj专业工程暂估价 TObj = Tdataitems[i];
                            obj.GroupName = TObj.GroupName;
                            obj.TName = TObj.Name;
                            obj.TPrice = TObj.Price;
                            CreateTreeDatas(obj.Children, TObj.Children, BObj.Children);
                        }
                    }
                    bool b = true;
                    if (obj.TName.Replace(" ", "") != obj.BName.Replace(" ", ""))
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
        }

        private List<TreeFeesObj专业工程暂估价> AnalysisData(Dictionary<XMLNodeItem, Dictionary<XMLNodeItem, List<XMLNodeItem>>> dicNodes, bool isT)
        {
            List<TreeFeesObj专业工程暂估价> list = new List<TreeFeesObj专业工程暂估价>();
            if (isT)
            {
                if (dicNodes == null || dicNodes.Count == 0) return list;
                foreach (KeyValuePair<XMLNodeItem, Dictionary<XMLNodeItem, List<XMLNodeItem>>> item in dicNodes)
                {
                    TreeFeesObj专业工程暂估价 rootTree = new TreeFeesObj专业工程暂估价();
                    string root = "";
                    for (int i = 0; i < item.Key.Attributes.Count; i++)
                    {
                        if (item.Key.Attributes[i].Name == "名称")
                            root = item.Key.Attributes[i].Value;

                    }
                    rootTree.GroupName = root;
                    foreach (KeyValuePair<XMLNodeItem, List<XMLNodeItem>> child in item.Value)
                    {
                        TreeFeesObj专业工程暂估价 secondTree = new TreeFeesObj专业工程暂估价();
                        string second = "";
                        for (int i = 0; i < child.Key.Attributes.Count; i++)
                        {
                            if (child.Key.Attributes[i].Name == "名称")
                                second = child.Key.Attributes[i].Value;
                        }
                        secondTree.GroupName = second;
                        foreach (XMLNodeItem node in child.Value)
                        {
                            TreeFeesObj专业工程暂估价 obj = new TreeFeesObj专业工程暂估价();
                            for (int i = 0; i < node.Attributes.Count; i++)
                            {
                                if (node.Attributes[i].Name == "序号")
                                    obj.Name = node.Attributes[i].Value;
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
                    TreeFeesObj专业工程暂估价 rootTree = new TreeFeesObj专业工程暂估价();
                    string root = "";
                    for (int i = 0; i < item.Key.Attributes.Count; i++)
                    {
                        if (item.Key.Attributes[i].Name == "名称")
                            root = item.Key.Attributes[i].Value;

                    }
                    rootTree.GroupName = root;
                    foreach (KeyValuePair<XMLNodeItem, List<XMLNodeItem>> child in item.Value)
                    {
                        TreeFeesObj专业工程暂估价 secondTree = new TreeFeesObj专业工程暂估价();
                        string second = "";
                        for (int i = 0; i < child.Key.Attributes.Count; i++)
                        {
                            if (child.Key.Attributes[i].Name == "名称")
                                second = child.Key.Attributes[i].Value;
                        }
                        secondTree.GroupName = second;
                        foreach (XMLNodeItem node in child.Value)
                        {
                            TreeFeesObj专业工程暂估价 obj = new TreeFeesObj专业工程暂估价();
                            for (int i = 0; i < node.Attributes.Count; i++)
                            {
                                if (node.Attributes[i].Name == "序号")
                                    obj.Name = node.Attributes[i].Value;
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
    public class TreeFeesObj专业工程暂估价
    {
        public TreeFeesObj专业工程暂估价() { _children = new ObservableCollection<TreeFeesObj专业工程暂估价>(); }
        private string _Name = string.Empty;
        private string _GroupName = string.Empty;
        private string _Price = string.Empty;

        public string Name { get { return _Name; } set { _Name = value; } }
        public string GroupName { get { return _GroupName; } set { _GroupName = value; } }
        public string Price { get { return _Price; } set { _Price = value; } }

        private ObservableCollection<TreeFeesObj专业工程暂估价> _children;
        public ObservableCollection<TreeFeesObj专业工程暂估价> Children
        {
            get { return _children; }
            set { _children = value; }
        }
    }
    public class TreeFeesObjTB专业工程暂估价
    {
        public TreeFeesObjTB专业工程暂估价() { _children = new ObservableCollection<TreeFeesObjTB专业工程暂估价>(); }
        private string _TName = string.Empty;
        private string _TPrice = string.Empty;

        private string _BName = string.Empty;
        private string _BPrice = string.Empty;
        private Brush _Foreground = Brushes.Black;
        private string _ItemIcon = "/XMLContrast;component/Resources/gc.ico";
        private Thickness _Margin = new Thickness(2);

        public string TName { get { return _TName; } set { _TName = value; } }
        public string TPrice { get { return _TPrice; } set { _TPrice = value; } }

        public string BName { get { return _BName; } set { _BName = value; } }
        public string BPrice { get { return _BPrice; } set { _BPrice = value; } }

        public string CheckResult { get; set; }
        public string GroupName { get; set; }
        public string ItemIcon { get { return _ItemIcon; } set { _ItemIcon = value; } }
        public Brush Foreground { get { return _Foreground; } set { _Foreground = value; } }
        private bool _IsExpanded = true;
        public bool IsExpanded { get { return _IsExpanded; } set { _IsExpanded = value; } }
        public Thickness Margin { get { return _Margin; } set { _Margin = value; } }
        private ObservableCollection<TreeFeesObjTB专业工程暂估价> _children;
        public ObservableCollection<TreeFeesObjTB专业工程暂估价> Children
        {
            get { return _children; }
            set { _children = value; }
        }
    }
}
