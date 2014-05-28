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
    public partial class 规费子目 : HLUserControlExt
    {
        public 规费子目()
        {
            InitializeComponent();

        }
        public 规费子目(List<XMLNodeItem> tenderDataNodes, List<XMLNodeItem> bidDataNodes, object owner)
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

                Dictionary<XMLNodeItem, Dictionary<XMLNodeItem, List<XMLNodeItem>>> dicTNodes = XMLOperat.GetTreeListNodes("单项工程", "单位工程", "规费子目", tenderDataNodes);
                Dictionary<XMLNodeItem, Dictionary<XMLNodeItem, List<XMLNodeItem>>> dicBNodes = XMLOperat.GetTreeListNodes("单项工程", "单位工程", "规费子目", bidDataNodes);
                List<TreeFeesObj规费子目> Tdataitems = AnalysisData(dicTNodes, true);//招标
                List<TreeFeesObj规费子目> Bdataitems = AnalysisData(dicBNodes, false);//招标
                List<TreeFeesObjTB规费子目> dataList = ContrastDatas(Tdataitems, Bdataitems);
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

        private List<TreeFeesObjTB规费子目> ContrastDatas(List<TreeFeesObj规费子目> Tdataitems, List<TreeFeesObj规费子目> Bdataitems)
        {
            List<TreeFeesObjTB规费子目> list = new List<TreeFeesObjTB规费子目>();
            if (Tdataitems.Count >= Bdataitems.Count)
            {
                for (int i = 0; i < Tdataitems.Count; i++)
                {
                    TreeFeesObjTB规费子目 obj = new TreeFeesObjTB规费子目();
                    TreeFeesObj规费子目 TObj = Tdataitems[i];
                    obj.GroupName = TObj.GroupName;
                    obj.TUnit = TObj.Unit;
                    obj.TName = TObj.Name;
                    obj.TNumber = TObj.Number;
                    obj.TRate = TObj.Rate;
                    if (Bdataitems != null && Bdataitems.Count != 0)
                    {
                        if (i >= Bdataitems.Count) continue;
                        if (Bdataitems[i] != null)
                        {
                            TreeFeesObj规费子目 BObj = Bdataitems[i];
                            obj.BUnit = BObj.Unit;
                            obj.BName = BObj.Name;
                            obj.BNumber = BObj.Number;
                            obj.BRate = BObj.Rate;
                            CreateTreeDatas(obj.Children, TObj.Children, BObj.Children);
                        }
                    }
                    bool b = true;
                    if (obj.TUnit.Replace(" ", "") != obj.BUnit.Replace(" ", ""))
                        b = false;
                    if (obj.TName.Replace(" ", "") != obj.BName.Replace(" ", ""))
                        b = false;
                    if (obj.TNumber.Replace(" ", "") != obj.BNumber.Replace(" ", ""))
                        b = false;
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
                    TreeFeesObjTB规费子目 obj = new TreeFeesObjTB规费子目();
                    TreeFeesObj规费子目 BObj = Bdataitems[i];
                    obj.BUnit = BObj.Unit;
                    obj.BName = BObj.Name;
                    obj.BNumber = BObj.Number;
                    obj.BRate = BObj.Rate;
                    if (Tdataitems != null && Tdataitems.Count != 0)
                    {
                        if (i >= Tdataitems.Count) continue;
                        if (Tdataitems[i] != null)
                        {
                            TreeFeesObj规费子目 TObj = Tdataitems[i];
                            obj.GroupName = TObj.GroupName;
                            obj.TUnit = TObj.Unit;
                            obj.TName = TObj.Name;
                            obj.TNumber = TObj.Number;
                            obj.TNumber = BObj.Number;
                            obj.TRate = BObj.Rate;
                            CreateTreeDatas(obj.Children, TObj.Children, BObj.Children);
                        }
                    }
                    bool b = true;
                    if (obj.TUnit.Replace(" ", "") != obj.BUnit.Replace(" ", ""))
                        b = false;
                    if (obj.TName.Replace(" ", "") != obj.BName.Replace(" ", ""))
                        b = false;
                    if (obj.TNumber.Replace(" ", "") != obj.BNumber.Replace(" ", ""))
                        b = false;
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

        private void CreateTreeDatas(ObservableCollection<TreeFeesObjTB规费子目> dataChilds, ObservableCollection<TreeFeesObj规费子目> Tdataitems, ObservableCollection<TreeFeesObj规费子目> Bdataitems)
        {
            if (Tdataitems.Count >= Bdataitems.Count)
            {
                for (int i = 0; i < Tdataitems.Count; i++)
                {
                    TreeFeesObjTB规费子目 obj = new TreeFeesObjTB规费子目();
                    TreeFeesObj规费子目 TObj = Tdataitems[i];
                    obj.GroupName = TObj.GroupName;
                    obj.TUnit = TObj.Unit;
                    obj.TName = TObj.Name;
                    obj.TNumber = TObj.Number;
                    obj.TRate = TObj.Rate;
                    if (Bdataitems != null && Bdataitems.Count != 0)
                    {
                        if (i >= Bdataitems.Count) continue;
                        if (Bdataitems[i] != null)
                        {
                            TreeFeesObj规费子目 BObj = Bdataitems[i];
                            obj.BUnit = BObj.Unit;
                            obj.BName = BObj.Name;
                            obj.BNumber = BObj.Number;
                            obj.BRate = BObj.Rate;
                            CreateTreeDatas(obj.Children, TObj.Children, BObj.Children);
                        }

                    }
                    bool b = true;
                    if (obj.TUnit.Replace(" ", "") != obj.BUnit.Replace(" ", ""))
                        b = false;
                    if (obj.TName.Replace(" ", "") != obj.BName.Replace(" ", ""))
                        b = false;
                    if (obj.TNumber.Replace(" ", "") != obj.BNumber.Replace(" ", ""))
                        b = false;
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
                    TreeFeesObjTB规费子目 obj = new TreeFeesObjTB规费子目();
                    TreeFeesObj规费子目 BObj = Bdataitems[i];
                    obj.BUnit = BObj.Unit;
                    obj.BName = BObj.Name;
                    obj.BNumber = BObj.Number;
                    obj.BRate = BObj.Rate;
                    if (Tdataitems != null && Tdataitems.Count != 0)
                    {
                        if (i >= Tdataitems.Count) continue;
                        if (Tdataitems[i] != null)
                        {
                            TreeFeesObj规费子目 TObj = Tdataitems[i];
                            obj.GroupName = TObj.GroupName;
                            obj.TUnit = TObj.Unit;
                            obj.TName = TObj.Name;
                            obj.TNumber = TObj.Number;
                            obj.TRate = TObj.Rate;
                            CreateTreeDatas(obj.Children, TObj.Children, BObj.Children);
                        }
                    }
                    bool b = true;
                    if (obj.TUnit.Replace(" ", "") != obj.BUnit.Replace(" ", ""))
                        b = false;
                    if (obj.TName.Replace(" ", "") != obj.BName.Replace(" ", ""))
                        b = false;
                    if (obj.TNumber.Replace(" ", "") != obj.BNumber.Replace(" ", ""))
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

        private List<TreeFeesObj规费子目> AnalysisData(Dictionary<XMLNodeItem, Dictionary<XMLNodeItem, List<XMLNodeItem>>> dicNodes, bool isT)
        {
            List<TreeFeesObj规费子目> list = new List<TreeFeesObj规费子目>();
            if (isT)
            {
                if (dicNodes == null || dicNodes.Count == 0) return list;
                foreach (KeyValuePair<XMLNodeItem, Dictionary<XMLNodeItem, List<XMLNodeItem>>> item in dicNodes)
                {
                    TreeFeesObj规费子目 rootTree = new TreeFeesObj规费子目();
                    string root = "";
                    for (int i = 0; i < item.Key.Attributes.Count; i++)
                    {
                        if (item.Key.Attributes[i].Name == "名称")
                            root = item.Key.Attributes[i].Value;

                    }
                    rootTree.GroupName = root;
                    foreach (KeyValuePair<XMLNodeItem, List<XMLNodeItem>> child in item.Value)
                    {
                        TreeFeesObj规费子目 secondTree = new TreeFeesObj规费子目();
                        string second = "";
                        for (int i = 0; i < child.Key.Attributes.Count; i++)
                        {
                            if (child.Key.Attributes[i].Name == "名称")
                                second = child.Key.Attributes[i].Value;
                        }
                        secondTree.GroupName = second;
                        foreach (XMLNodeItem node in child.Value)
                        {
                            TreeFeesObj规费子目 obj = new TreeFeesObj规费子目();
                            for (int i = 0; i < node.Attributes.Count; i++)
                            {
                                if (node.Attributes[i].Name == "计算基础")
                                    obj.Unit = node.Attributes[i].Value;
                                if (node.Attributes[i].Name == "项目名称")
                                    obj.Name = node.Attributes[i].Value;
                                if (node.Attributes[i].Name == "费率")
                                    obj.Rate = node.Attributes[i].Value;
                                if (node.Attributes[i].Name == "序号")
                                    obj.Number = node.Attributes[i].Value;
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
                    TreeFeesObj规费子目 rootTree = new TreeFeesObj规费子目();
                    string root = "";
                    for (int i = 0; i < item.Key.Attributes.Count; i++)
                    {
                        if (item.Key.Attributes[i].Name == "名称")
                            root = item.Key.Attributes[i].Value;

                    }
                    rootTree.GroupName = root;
                    foreach (KeyValuePair<XMLNodeItem, List<XMLNodeItem>> child in item.Value)
                    {
                        TreeFeesObj规费子目 secondTree = new TreeFeesObj规费子目();
                        string second = "";
                        for (int i = 0; i < child.Key.Attributes.Count; i++)
                        {
                            if (child.Key.Attributes[i].Name == "名称")
                                second = child.Key.Attributes[i].Value;
                        }
                        secondTree.GroupName = second;
                        foreach (XMLNodeItem node in child.Value)
                        {
                            TreeFeesObj规费子目 obj = new TreeFeesObj规费子目();
                            for (int i = 0; i < node.Attributes.Count; i++)
                            {
                                if (node.Attributes[i].Name == "计算基础")
                                    obj.Unit = node.Attributes[i].Value;
                                if (node.Attributes[i].Name == "项目名称")
                                    obj.Name = node.Attributes[i].Value;
                                if (node.Attributes[i].Name == "费率")
                                    obj.Rate = node.Attributes[i].Value;
                                if (node.Attributes[i].Name == "序号")
                                    obj.Number = node.Attributes[i].Value;
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
    public class TreeFeesObj规费子目
    {
        public TreeFeesObj规费子目() { _children = new ObservableCollection<TreeFeesObj规费子目>(); }
        private string _Unit = string.Empty;
        private string _Name = string.Empty;
        private string _Number = string.Empty;
        private string _Rate = string.Empty;
        private string _GroupName = string.Empty;
        public string Unit { get { return _Unit; } set { _Unit = value; } }
        public string Name { get { return _Name; } set { _Name = value; } }
        public string GroupName { get { return _GroupName; } set { _GroupName = value; } }
        public string Number { get { return _Number; } set { _Number = value; } }
        public string Rate { get { return _Rate; } set { _Rate = value; } }

        private ObservableCollection<TreeFeesObj规费子目> _children;
        public ObservableCollection<TreeFeesObj规费子目> Children
        {
            get { return _children; }
            set { _children = value; }
        }
    }
    public class TreeFeesObjTB规费子目
    {
        public TreeFeesObjTB规费子目() { _children = new ObservableCollection<TreeFeesObjTB规费子目>(); }
        private string _TUnit = string.Empty;
        private string _TName = string.Empty;
        private string _TNumber = string.Empty;
        private string _TRate = string.Empty;

        private string _BUnit = string.Empty;
        private string _BName = string.Empty;
        private string _BNumber = string.Empty;
        private string _BRate = string.Empty;
        private Brush _Foreground = Brushes.Black;
        private string _ItemIcon = "/XMLContrast;component/Resources/gc.ico";
        private Thickness _Margin = new Thickness(2);

        public string TUnit { get { return _TUnit; } set { _TUnit = value; } }
        public string TName { get { return _TName; } set { _TName = value; } }
        public string TNumber { get { return _TNumber; } set { _TNumber = value; } }
        public string TRate { get { return _TRate; } set { _TRate = value; } }

        public string BUnit { get { return _BUnit; } set { _BUnit = value; } }
        public string BName { get { return _BName; } set { _BName = value; } }
        public string BNumber { get { return _BNumber; } set { _BNumber = value; } }
        public string BRate { get { return _BRate; } set { _BRate = value; } }

        public string CheckResult { get; set; }
        public string GroupName { get; set; }
        public string ItemIcon { get { return _ItemIcon; } set { _ItemIcon = value; } }
        public Brush Foreground { get { return _Foreground; } set { _Foreground = value; } }
        public Thickness Margin { get { return _Margin; } set { _Margin = value; } }
        private bool _IsExpanded = false;
        public bool IsExpanded { get { return _IsExpanded; } set { _IsExpanded = value; } }
        private ObservableCollection<TreeFeesObjTB规费子目> _children;
        public ObservableCollection<TreeFeesObjTB规费子目> Children
        {
            get { return _children; }
            set { _children = value; }
        }
    }
}
