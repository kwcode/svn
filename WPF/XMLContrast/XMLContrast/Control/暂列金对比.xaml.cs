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
    public partial class 暂列金对比 : HLUserControlExt
    {
        public 暂列金对比()
        {
            InitializeComponent();

        }
        public 暂列金对比(List<XMLNodeItem> tenderDataNodes, List<XMLNodeItem> bidDataNodes, object owner)
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

                Dictionary<XMLNodeItem, Dictionary<XMLNodeItem, List<XMLNodeItem>>> dicTNodes = XMLOperat.GetTreeListNodes("单项工程", "单位工程", "暂列金额子目", tenderDataNodes);
                if (dicTNodes.Count == 0)
                {
                    dicTNodes = XMLOperat.GetTreeListNodes("总工程", "单位工程", "暂列金额子目", tenderDataNodes);
                }
                Dictionary<XMLNodeItem, Dictionary<XMLNodeItem, List<XMLNodeItem>>> dicBNodes = XMLOperat.GetTreeListNodes("单项工程", "单位工程", "暂列金额子目", bidDataNodes);
                if (dicBNodes.Count == 0)
                {
                    dicBNodes = XMLOperat.GetTreeListNodes("总工程", "单位工程", "暂列金额子目", bidDataNodes);
                }
                List<TreeFeesObj暂列金> Tdataitems = AnalysisData(dicTNodes, true);//招标
                List<TreeFeesObj暂列金> Bdataitems = AnalysisData(dicBNodes, false);//招标
                List<TreeFeesObjTB暂列金> dataList = ContrastDatas(Tdataitems, Bdataitems);
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

        private List<TreeFeesObjTB暂列金> ContrastDatas(List<TreeFeesObj暂列金> Tdataitems, List<TreeFeesObj暂列金> Bdataitems)
        {
            List<TreeFeesObjTB暂列金> list = new List<TreeFeesObjTB暂列金>();
            if (Tdataitems.Count >= Bdataitems.Count)
            {
                for (int i = 0; i < Tdataitems.Count; i++)
                {
                    TreeFeesObjTB暂列金 obj = new TreeFeesObjTB暂列金();
                    TreeFeesObj暂列金 TObj = Tdataitems[i];
                    obj.GroupName = TObj.GroupName;
                    obj.TUnit = TObj.Unit;
                    obj.TName = TObj.Name;
                    obj.TProvisionalSums = TObj.ProvisionalSums;
                    if (Bdataitems != null && Bdataitems.Count != 0)
                    {
                        if (i >= Bdataitems.Count) continue;
                        if (Bdataitems[i] != null)
                        {
                            TreeFeesObj暂列金 BObj = Bdataitems[i];
                            obj.BUnit = BObj.Unit;
                            obj.BName = BObj.Name;
                            obj.BProvisionalSums = BObj.ProvisionalSums;
                            CreateTreeDatas(obj.Children, TObj.Children, BObj.Children);
                        }
                    }
                    bool b = true;
                    if (obj.TUnit.Replace(" ", "") != obj.BUnit.Replace(" ", ""))
                        b = false;
                    if (obj.TName.Replace(" ", "") != obj.BName.Replace(" ", ""))
                        b = false;
                    if (obj.TProvisionalSums.Replace(" ", "") != obj.BProvisionalSums.Replace(" ", ""))
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
                    TreeFeesObjTB暂列金 obj = new TreeFeesObjTB暂列金();
                    TreeFeesObj暂列金 BObj = Bdataitems[i];
                    obj.BUnit = BObj.Unit;
                    obj.BName = BObj.Name;
                    obj.BProvisionalSums = BObj.ProvisionalSums;
                    if (Tdataitems != null && Tdataitems.Count != 0)
                    {
                        if (i >= Tdataitems.Count) continue;
                        if (Tdataitems[i] != null)
                        {
                            TreeFeesObj暂列金 TObj = Tdataitems[i];
                            obj.GroupName = TObj.GroupName;
                            obj.TUnit = TObj.Unit;
                            obj.TName = TObj.Name;
                            obj.TProvisionalSums = TObj.ProvisionalSums;
                            CreateTreeDatas(obj.Children, TObj.Children, BObj.Children);
                        }
                    }
                    bool b = true;
                    if (obj.TUnit.Replace(" ", "") != obj.BUnit.Replace(" ", ""))
                        b = false;
                    if (obj.TName.Replace(" ", "") != obj.BName.Replace(" ", ""))
                        b = false;
                    if (obj.TProvisionalSums.Replace(" ", "") != obj.BProvisionalSums.Replace(" ", ""))
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

        private void CreateTreeDatas(ObservableCollection<TreeFeesObjTB暂列金> dataChilds, ObservableCollection<TreeFeesObj暂列金> Tdataitems, ObservableCollection<TreeFeesObj暂列金> Bdataitems)
        {
            if (Tdataitems.Count >= Bdataitems.Count)
            {
                for (int i = 0; i < Tdataitems.Count; i++)
                {
                    TreeFeesObjTB暂列金 obj = new TreeFeesObjTB暂列金();
                    TreeFeesObj暂列金 TObj = Tdataitems[i];
                    obj.GroupName = TObj.GroupName;
                    obj.TUnit = TObj.Unit;
                    obj.TName = TObj.Name;
                    obj.TProvisionalSums = TObj.ProvisionalSums;
                    if (Bdataitems != null && Bdataitems.Count != 0)
                    {
                        if (i >= Bdataitems.Count) continue;
                        if (Bdataitems[i] != null)
                        {
                            TreeFeesObj暂列金 BObj = Bdataitems[i];
                            obj.BUnit = BObj.Unit;
                            obj.BName = BObj.Name;
                            obj.BProvisionalSums = BObj.ProvisionalSums;
                            CreateTreeDatas(obj.Children, TObj.Children, BObj.Children);
                        }

                    }
                    bool b = true;
                    if (obj.TUnit.Replace(" ", "") != obj.BUnit.Replace(" ", ""))
                        b = false;
                    if (obj.TName.Replace(" ", "") != obj.BName.Replace(" ", ""))
                        b = false;
                    if (obj.TProvisionalSums.Replace(" ", "") != obj.BProvisionalSums.Replace(" ", ""))
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
                    TreeFeesObjTB暂列金 obj = new TreeFeesObjTB暂列金();
                    TreeFeesObj暂列金 BObj = Bdataitems[i];
                    obj.BUnit = BObj.Unit;
                    obj.BName = BObj.Name;
                    obj.BProvisionalSums = BObj.ProvisionalSums;
                    if (Tdataitems != null && Tdataitems.Count != 0)
                    {
                        if (i >= Tdataitems.Count) continue;
                        if (Tdataitems[i] != null)
                        {
                            TreeFeesObj暂列金 TObj = Tdataitems[i];
                            obj.GroupName = TObj.GroupName;
                            obj.TUnit = TObj.Unit;
                            obj.TName = TObj.Name;
                            obj.TProvisionalSums = TObj.ProvisionalSums;
                            CreateTreeDatas(obj.Children, TObj.Children, BObj.Children);
                        }
                    }
                    bool b = true;
                    if (obj.TUnit.Replace(" ", "") != obj.BUnit.Replace(" ", ""))
                        b = false;
                    if (obj.TName.Replace(" ", "") != obj.BName.Replace(" ", ""))
                        b = false;
                    if (obj.TProvisionalSums.Replace(" ", "") != obj.BProvisionalSums.Replace(" ", ""))
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

        private List<TreeFeesObj暂列金> AnalysisData(Dictionary<XMLNodeItem, Dictionary<XMLNodeItem, List<XMLNodeItem>>> dicNodes, bool isT)
        {
            List<TreeFeesObj暂列金> list = new List<TreeFeesObj暂列金>();
            if (isT)
            {
                if (dicNodes == null || dicNodes.Count == 0) return list;
                foreach (KeyValuePair<XMLNodeItem, Dictionary<XMLNodeItem, List<XMLNodeItem>>> item in dicNodes)
                {
                    TreeFeesObj暂列金 rootTree = new TreeFeesObj暂列金();
                    string root = "";
                    for (int i = 0; i < item.Key.Attributes.Count; i++)
                    {
                        if (item.Key.Attributes[i].Name == "名称")
                            root = item.Key.Attributes[i].Value;

                    }
                    rootTree.GroupName = root;
                    foreach (KeyValuePair<XMLNodeItem, List<XMLNodeItem>> child in item.Value)
                    {
                        TreeFeesObj暂列金 secondTree = new TreeFeesObj暂列金();
                        string second = "";
                        for (int i = 0; i < child.Key.Attributes.Count; i++)
                        {
                            if (child.Key.Attributes[i].Name == "名称")
                                second = child.Key.Attributes[i].Value;
                        }
                        secondTree.GroupName = second;
                        foreach (XMLNodeItem node in child.Value)
                        {
                            TreeFeesObj暂列金 obj = new TreeFeesObj暂列金();
                            for (int i = 0; i < node.Attributes.Count; i++)
                            {
                                if (node.Attributes[i].Name == "项目名称")
                                    obj.Name = node.Attributes[i].Value;
                                if (node.Attributes[i].Name == "计量单位")
                                    obj.Unit = node.Attributes[i].Value;
                                if (node.Attributes[i].Name == "暂定金额")
                                    obj.ProvisionalSums = node.Attributes[i].Value;
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
                    TreeFeesObj暂列金 rootTree = new TreeFeesObj暂列金();
                    string root = "";
                    for (int i = 0; i < item.Key.Attributes.Count; i++)
                    {
                        if (item.Key.Attributes[i].Name == "名称")
                            root = item.Key.Attributes[i].Value;

                    }
                    rootTree.GroupName = root;
                    foreach (KeyValuePair<XMLNodeItem, List<XMLNodeItem>> child in item.Value)
                    {
                        TreeFeesObj暂列金 secondTree = new TreeFeesObj暂列金();
                        string second = "";
                        for (int i = 0; i < child.Key.Attributes.Count; i++)
                        {
                            if (child.Key.Attributes[i].Name == "名称")
                                second = child.Key.Attributes[i].Value;
                        }
                        secondTree.GroupName = second;
                        foreach (XMLNodeItem node in child.Value)
                        {
                            TreeFeesObj暂列金 obj = new TreeFeesObj暂列金();
                            for (int i = 0; i < node.Attributes.Count; i++)
                            {
                                if (node.Attributes[i].Name == "计量单位")
                                    obj.Unit = node.Attributes[i].Value;
                                if (node.Attributes[i].Name == "项目名称")
                                    obj.Name = node.Attributes[i].Value;
                                if (node.Attributes[i].Name == "暂定金额")
                                    obj.ProvisionalSums = node.Attributes[i].Value;
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
    public class TreeFeesObj暂列金
    {
        public TreeFeesObj暂列金() { _children = new ObservableCollection<TreeFeesObj暂列金>(); }
        private string _Unit = string.Empty;
        private string _Name = string.Empty;
        private string _ProvisionalSums = string.Empty;
        private string _GroupName = string.Empty;

        public string Unit { get { return _Unit; } set { _Unit = value; } }
        public string Name { get { return _Name; } set { _Name = value; } }
        public string ProvisionalSums { get { return _ProvisionalSums; } set { _ProvisionalSums = value; } }
        public string GroupName { get { return _GroupName; } set { _GroupName = value; } }
        private ObservableCollection<TreeFeesObj暂列金> _children;
        public ObservableCollection<TreeFeesObj暂列金> Children
        {
            get { return _children; }
            set { _children = value; }
        }
    }
    public class TreeFeesObjTB暂列金
    {
        public TreeFeesObjTB暂列金() { _children = new ObservableCollection<TreeFeesObjTB暂列金>(); }
        private string _TUnit = string.Empty;
        private string _TName = string.Empty;
        private string _TProvisionalSums = string.Empty;
        private string _BUnit = string.Empty;
        private string _BName = string.Empty;
        private string _BProvisionalSums = string.Empty;
        private Brush _Foreground = Brushes.Black;
        private string _ItemIcon = "/XMLContrast;component/Resources/gc.ico";
        private Thickness _Margin = new Thickness(2);

        public string TUnit { get { return _TUnit; } set { _TUnit = value; } }
        public string TName { get { return _TName; } set { _TName = value; } }
        public string TProvisionalSums { get { return _TProvisionalSums; } set { _TProvisionalSums = value; } }
        public string BUnit { get { return _BUnit; } set { _BUnit = value; } }
        public string BName { get { return _BName; } set { _BName = value; } }
        public string BProvisionalSums { get { return _BProvisionalSums; } set { _BProvisionalSums = value; } }
        public string CheckResult { get; set; }
        public string GroupName { get; set; }
        public string ItemIcon { get { return _ItemIcon; } set { _ItemIcon = value; } }
        public Brush Foreground { get { return _Foreground; } set { _Foreground = value; } }
        public Thickness Margin { get { return _Margin; } set { _Margin = value; } }
        private bool _IsExpanded = false;
        public bool IsExpanded { get { return _IsExpanded; } set { _IsExpanded = value; } }
        private ObservableCollection<TreeFeesObjTB暂列金> _children;
        public ObservableCollection<TreeFeesObjTB暂列金> Children
        {
            get { return _children; }
            set { _children = value; }
        }
    }
}
