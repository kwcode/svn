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
    public partial class 税金 : HLUserControlExt
    {
        public 税金()
        {
            InitializeComponent();

        }
        public 税金(List<XMLNodeItem> tenderDataNodes, List<XMLNodeItem> bidDataNodes, object owner)
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

                Dictionary<XMLNodeItem, Dictionary<XMLNodeItem, List<XMLNodeItem>>> dicTNodes = XMLOperat.GetTreeListNodes("单项工程", "单位工程", "税金", tenderDataNodes);
                if (dicTNodes.Count == 0)
                {
                    dicTNodes = XMLOperat.GetTreeListNodes("总工程", "单位工程", "税金", tenderDataNodes);
                }
                Dictionary<XMLNodeItem, Dictionary<XMLNodeItem, List<XMLNodeItem>>> dicBNodes = XMLOperat.GetTreeListNodes("单项工程", "单位工程", "税金", bidDataNodes);
                if (dicBNodes.Count == 0)
                {
                    dicBNodes = XMLOperat.GetTreeListNodes("总工程", "单位工程", "税金", bidDataNodes);
                }
                List<TreeFeesObj税金> Tdataitems = AnalysisData(dicTNodes, true);//招标
                List<TreeFeesObj税金> Bdataitems = AnalysisData(dicBNodes, false);//招标
                List<TreeFeesObjTB税金> dataList = ContrastDatas(Tdataitems, Bdataitems);
                Global.SysContext.Send(a =>
                            {
                                lv_DataView.ItemsSource = dataList;
                                (owner as HLWindowExt).StopProgress();
                            }, null);
            }).Start();


        }

        private List<TreeFeesObjTB税金> ContrastDatas(List<TreeFeesObj税金> Tdataitems, List<TreeFeesObj税金> Bdataitems)
        {
            List<TreeFeesObjTB税金> list = new List<TreeFeesObjTB税金>();
            if (Tdataitems.Count >= Bdataitems.Count)
            {
                for (int i = 0; i < Tdataitems.Count; i++)
                {
                    TreeFeesObjTB税金 obj = new TreeFeesObjTB税金();
                    TreeFeesObj税金 TObj = Tdataitems[i];
                    obj.GroupName = TObj.GroupName;
                    obj.TRate = TObj.Rate;
                    if (Bdataitems != null && Bdataitems.Count != 0)
                    {
                        if (i >= Bdataitems.Count) continue;
                        if (Bdataitems[i] != null)
                        {
                            TreeFeesObj税金 BObj = Bdataitems[i];
                            obj.BRate = BObj.Rate;
                            CreateTreeDatas(obj.Children, TObj.Children, BObj.Children);
                        }
                    }
                    bool b = true;
                    if (obj.TRate.Replace(" ", "") != obj.BRate.Replace(" ", ""))
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
                    TreeFeesObjTB税金 obj = new TreeFeesObjTB税金();
                    TreeFeesObj税金 BObj = Bdataitems[i];
                    obj.BRate = BObj.Rate;
                    if (Tdataitems != null && Tdataitems.Count != 0)
                    {
                        if (i >= Tdataitems.Count) continue;
                        if (Tdataitems[i] != null)
                        {
                            TreeFeesObj税金 TObj = Tdataitems[i];
                            obj.GroupName = TObj.GroupName;
                            obj.TRate = TObj.Rate;
                            CreateTreeDatas(obj.Children, TObj.Children, BObj.Children);
                        }
                    }
                    bool b = true;
                    if (obj.TRate.Replace(" ", "") != obj.BRate.Replace(" ", ""))
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

        private void CreateTreeDatas(ObservableCollection<TreeFeesObjTB税金> dataChilds, ObservableCollection<TreeFeesObj税金> Tdataitems, ObservableCollection<TreeFeesObj税金> Bdataitems)
        {
            if (Tdataitems.Count >= Bdataitems.Count)
            {
                for (int i = 0; i < Tdataitems.Count; i++)
                {
                    TreeFeesObjTB税金 obj = new TreeFeesObjTB税金();
                    TreeFeesObj税金 TObj = Tdataitems[i];
                    obj.GroupName = TObj.GroupName;
                    obj.TRate = TObj.Rate; 
                    if (Bdataitems != null && Bdataitems.Count != 0)
                    {
                        if (i >= Bdataitems.Count) continue;
                        if (Bdataitems[i] != null)
                        {
                            TreeFeesObj税金 BObj = Bdataitems[i];
                            obj.BRate = BObj.Rate; 
                            CreateTreeDatas(obj.Children, TObj.Children, BObj.Children);
                        }

                    }
                    bool b = true;
                    if (obj.TRate.Replace(" ", "") != obj.BRate.Replace(" ", ""))
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
                    TreeFeesObjTB税金 obj = new TreeFeesObjTB税金();
                    TreeFeesObj税金 BObj = Bdataitems[i];
                    obj.BRate = BObj.Rate; 
                    if (Tdataitems != null && Tdataitems.Count != 0)
                    {
                        if (i >= Tdataitems.Count) continue;
                        if (Tdataitems[i] != null)
                        {
                            TreeFeesObj税金 TObj = Tdataitems[i];
                            obj.GroupName = TObj.GroupName;
                            obj.TRate = TObj.Rate; 
                            CreateTreeDatas(obj.Children, TObj.Children, BObj.Children);
                        }
                    }
                    bool b = true;
                    if (obj.TRate.Replace(" ", "") != obj.BRate.Replace(" ", ""))
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

        private List<TreeFeesObj税金> AnalysisData(Dictionary<XMLNodeItem, Dictionary<XMLNodeItem, List<XMLNodeItem>>> dicNodes, bool isT)
        {
            List<TreeFeesObj税金> list = new List<TreeFeesObj税金>();
            if (isT)
            {
                if (dicNodes == null || dicNodes.Count == 0) return list;
                foreach (KeyValuePair<XMLNodeItem, Dictionary<XMLNodeItem, List<XMLNodeItem>>> item in dicNodes)
                {
                    TreeFeesObj税金 rootTree = new TreeFeesObj税金();
                    string root = "";
                    for (int i = 0; i < item.Key.Attributes.Count; i++)
                    {
                        if (item.Key.Attributes[i].Name == "名称")
                            root = item.Key.Attributes[i].Value;

                    }
                    rootTree.GroupName = root;
                    foreach (KeyValuePair<XMLNodeItem, List<XMLNodeItem>> child in item.Value)
                    {
                        TreeFeesObj税金 secondTree = new TreeFeesObj税金();
                        string second = "";
                        for (int i = 0; i < child.Key.Attributes.Count; i++)
                        {
                            if (child.Key.Attributes[i].Name == "名称")
                                second = child.Key.Attributes[i].Value;
                        }
                        secondTree.GroupName = second;
                        foreach (XMLNodeItem node in child.Value)
                        {
                            TreeFeesObj税金 obj = new TreeFeesObj税金();
                            for (int i = 0; i < node.Attributes.Count; i++)
                            {
                                if (node.Attributes[i].Name == "费率")
                                    obj.Rate = node.Attributes[i].Value; 
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
                    TreeFeesObj税金 rootTree = new TreeFeesObj税金();
                    string root = "";
                    for (int i = 0; i < item.Key.Attributes.Count; i++)
                    {
                        if (item.Key.Attributes[i].Name == "名称")
                            root = item.Key.Attributes[i].Value;

                    }
                    rootTree.GroupName = root;
                    foreach (KeyValuePair<XMLNodeItem, List<XMLNodeItem>> child in item.Value)
                    {
                        TreeFeesObj税金 secondTree = new TreeFeesObj税金();
                        string second = "";
                        for (int i = 0; i < child.Key.Attributes.Count; i++)
                        {
                            if (child.Key.Attributes[i].Name == "名称")
                                second = child.Key.Attributes[i].Value;
                        }
                        secondTree.GroupName = second;
                        foreach (XMLNodeItem node in child.Value)
                        {
                            TreeFeesObj税金 obj = new TreeFeesObj税金();
                            for (int i = 0; i < node.Attributes.Count; i++)
                            {
                                if (node.Attributes[i].Name == "费率")
                                    obj.Rate = node.Attributes[i].Value; 
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
    public class TreeFeesObj税金
    {
        public TreeFeesObj税金() { _children = new ObservableCollection<TreeFeesObj税金>(); }
        private string _Rate = string.Empty;
        private string _GroupName = string.Empty;

        public string Rate { get { return _Rate; } set { _Rate = value; } }
        public string GroupName { get { return _GroupName; } set { _GroupName = value; } }

        private ObservableCollection<TreeFeesObj税金> _children;
        public ObservableCollection<TreeFeesObj税金> Children
        {
            get { return _children; }
            set { _children = value; }
        }
    }
    public class TreeFeesObjTB税金
    {
        public TreeFeesObjTB税金() { _children = new ObservableCollection<TreeFeesObjTB税金>(); }
        private string _TRate = string.Empty;
        private string _BRate = string.Empty;
        private Brush _Foreground = Brushes.Black;
        private string _ItemIcon = "/XMLContrast;component/Resources/gc.ico";
        private Thickness _Margin = new Thickness(2);

        public string TRate { get { return _TRate; } set { _TRate = value; } }

        public string BRate { get { return _BRate; } set { _BRate = value; } }

        public string CheckResult { get; set; }
        public string GroupName { get; set; }
        public string ItemIcon { get { return _ItemIcon; } set { _ItemIcon = value; } }
        public Brush Foreground { get { return _Foreground; } set { _Foreground = value; } }
        private bool _IsExpanded = true;
        public bool IsExpanded { get { return _IsExpanded; } set { _IsExpanded = value; } }
        public Thickness Margin { get { return _Margin; } set { _Margin = value; } }
        private ObservableCollection<TreeFeesObjTB税金> _children;
        public ObservableCollection<TreeFeesObjTB税金> Children
        {
            get { return _children; }
            set { _children = value; }
        }
    }
}
