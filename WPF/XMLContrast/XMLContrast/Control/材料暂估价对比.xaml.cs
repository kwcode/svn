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
    public partial class 材料暂估价对比 : HLUserControlExt
    {
        public 材料暂估价对比()
        {
            InitializeComponent();

        }
        public 材料暂估价对比(List<XMLNodeItem> tenderDataNodes, List<XMLNodeItem> bidDataNodes, object owner)
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

                Dictionary<XMLNodeItem, Dictionary<XMLNodeItem, List<XMLNodeItem>>> dicTNodes = XMLOperat.GetTreeListNodes("单项工程", "单位工程", "暂估材料子目", tenderDataNodes);
                Dictionary<XMLNodeItem, Dictionary<XMLNodeItem, List<XMLNodeItem>>> dicBNodes = XMLOperat.GetTreeListNodes("单项工程", "单位工程", "暂估材料子目", bidDataNodes);
                List<TreeFeesObj材料暂估价> Tdataitems = AnalysisData(dicTNodes, true);//招标
                List<TreeFeesObj材料暂估价> Bdataitems = AnalysisData(dicBNodes, false);//招标
                List<TreeFeesObjTB材料暂估价> dataList = ContrastDatas(Tdataitems, Bdataitems);
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

        private List<TreeFeesObjTB材料暂估价> ContrastDatas(List<TreeFeesObj材料暂估价> Tdataitems, List<TreeFeesObj材料暂估价> Bdataitems)
        {
            List<TreeFeesObjTB材料暂估价> list = new List<TreeFeesObjTB材料暂估价>();
            if (Tdataitems.Count >= Bdataitems.Count)
            {
                for (int i = 0; i < Tdataitems.Count; i++)
                {
                    TreeFeesObjTB材料暂估价 obj = new TreeFeesObjTB材料暂估价();
                    TreeFeesObj材料暂估价 TObj = Tdataitems[i];
                    obj.GroupName = TObj.GroupName;
                    obj.TUnit = TObj.Unit;
                    obj.TName = TObj.Name;
                    obj.TSpecification = TObj.Specification;
                    if (Bdataitems != null && Bdataitems.Count != 0)
                    {
                        if (i >= Bdataitems.Count) continue;
                        if (Bdataitems[i] != null)
                        {
                            TreeFeesObj材料暂估价 BObj = Bdataitems[i];
                            obj.BUnit = BObj.Unit;
                            obj.BName = BObj.Name;
                            obj.BSpecification = BObj.Specification;
                            CreateTreeDatas(obj.Children, TObj.Children, BObj.Children);
                        }
                    }
                    bool b = true;
                    if (obj.TUnit.Replace(" ", "") != obj.BUnit.Replace(" ", ""))
                        b = false;
                    if (obj.TName.Replace(" ", "") != obj.BName.Replace(" ", ""))
                        b = false;
                    if (obj.TSpecification.Replace(" ", "") != obj.BSpecification.Replace(" ", ""))
                        b = false;
                    if (obj.TModel.Replace(" ", "") != obj.BModel.Replace(" ", ""))
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
                    TreeFeesObjTB材料暂估价 obj = new TreeFeesObjTB材料暂估价();
                    TreeFeesObj材料暂估价 BObj = Bdataitems[i];
                    obj.BUnit = BObj.Unit;
                    obj.BName = BObj.Name;
                    obj.BSpecification = BObj.Specification;
                    if (Tdataitems != null && Tdataitems.Count != 0)
                    {
                        if (i >= Tdataitems.Count) continue;
                        if (Tdataitems[i] != null)
                        {
                            TreeFeesObj材料暂估价 TObj = Tdataitems[i];
                            obj.GroupName = TObj.GroupName;
                            obj.TUnit = TObj.Unit;
                            obj.TName = TObj.Name;
                            obj.TSpecification = TObj.Specification;
                            CreateTreeDatas(obj.Children, TObj.Children, BObj.Children);
                        }
                    }
                    bool b = true;
                    if (obj.TUnit.Replace(" ", "") != obj.BUnit.Replace(" ", ""))
                        b = false;
                    if (obj.TName.Replace(" ", "") != obj.BName.Replace(" ", ""))
                        b = false;
                    if (obj.TSpecification.Replace(" ", "") != obj.BSpecification.Replace(" ", ""))
                        b = false;
                    if (obj.TModel.Replace(" ", "") != obj.BModel.Replace(" ", ""))
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

        private void CreateTreeDatas(ObservableCollection<TreeFeesObjTB材料暂估价> dataChilds, ObservableCollection<TreeFeesObj材料暂估价> Tdataitems, ObservableCollection<TreeFeesObj材料暂估价> Bdataitems)
        {
            if (Tdataitems.Count >= Bdataitems.Count)
            {
                for (int i = 0; i < Tdataitems.Count; i++)
                {
                    TreeFeesObjTB材料暂估价 obj = new TreeFeesObjTB材料暂估价();
                    TreeFeesObj材料暂估价 TObj = Tdataitems[i];
                    obj.GroupName = TObj.GroupName;
                    obj.TUnit = TObj.Unit;
                    obj.TName = TObj.Name;
                    obj.TSpecification = TObj.Specification;
                    obj.TModel = TObj.Model;
                    obj.TPrice = TObj.Price;
                    if (Bdataitems != null && Bdataitems.Count != 0)
                    {
                        if (i >= Bdataitems.Count) continue;
                        if (Bdataitems[i] != null)
                        {
                            TreeFeesObj材料暂估价 BObj = Bdataitems[i];
                            obj.BUnit = BObj.Unit;
                            obj.BName = BObj.Name;
                            obj.BSpecification = BObj.Specification;
                            obj.BModel = BObj.Model;
                            obj.BPrice = BObj.Price;
                            CreateTreeDatas(obj.Children, TObj.Children, BObj.Children);
                        }

                    }
                    bool b = true;
                    if (obj.TUnit.Replace(" ", "") != obj.BUnit.Replace(" ", ""))
                        b = false;
                    if (obj.TName.Replace(" ", "") != obj.BName.Replace(" ", ""))
                        b = false;
                    if (obj.TSpecification.Replace(" ", "") != obj.BSpecification.Replace(" ", ""))
                        b = false;
                    if (obj.TModel.Replace(" ", "") != obj.BModel.Replace(" ", ""))
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
                    TreeFeesObjTB材料暂估价 obj = new TreeFeesObjTB材料暂估价();
                    TreeFeesObj材料暂估价 BObj = Bdataitems[i];
                    obj.BUnit = BObj.Unit;
                    obj.BName = BObj.Name;
                    obj.BSpecification = BObj.Specification;
                    obj.BModel = BObj.Model;
                    obj.BPrice = BObj.Price;
                    if (Tdataitems != null && Tdataitems.Count != 0)
                    {
                        if (i >= Tdataitems.Count) continue;
                        if (Tdataitems[i] != null)
                        {
                            TreeFeesObj材料暂估价 TObj = Tdataitems[i];
                            obj.GroupName = TObj.GroupName;
                            obj.TUnit = TObj.Unit;
                            obj.TName = TObj.Name;
                            obj.TSpecification = TObj.Specification;
                            obj.TModel = TObj.Model;
                            obj.TPrice = TObj.Price;
                            CreateTreeDatas(obj.Children, TObj.Children, BObj.Children);
                        }
                    }
                    bool b = true;
                    if (obj.TUnit.Replace(" ", "") != obj.BUnit.Replace(" ", ""))
                        b = false;
                    if (obj.TName.Replace(" ", "") != obj.BName.Replace(" ", ""))
                        b = false;
                    if (obj.TSpecification.Replace(" ", "") != obj.BSpecification.Replace(" ", ""))
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

        private List<TreeFeesObj材料暂估价> AnalysisData(Dictionary<XMLNodeItem, Dictionary<XMLNodeItem, List<XMLNodeItem>>> dicNodes, bool isT)
        {
            List<TreeFeesObj材料暂估价> list = new List<TreeFeesObj材料暂估价>();
            if (isT)
            {
                if (dicNodes == null || dicNodes.Count == 0) return list;
                foreach (KeyValuePair<XMLNodeItem, Dictionary<XMLNodeItem, List<XMLNodeItem>>> item in dicNodes)
                {
                    TreeFeesObj材料暂估价 rootTree = new TreeFeesObj材料暂估价();
                    string root = "";
                    for (int i = 0; i < item.Key.Attributes.Count; i++)
                    {
                        if (item.Key.Attributes[i].Name == "名称")
                            root = item.Key.Attributes[i].Value;

                    }
                    rootTree.GroupName = root;
                    foreach (KeyValuePair<XMLNodeItem, List<XMLNodeItem>> child in item.Value)
                    {
                        TreeFeesObj材料暂估价 secondTree = new TreeFeesObj材料暂估价();
                        string second = "";
                        for (int i = 0; i < child.Key.Attributes.Count; i++)
                        {
                            if (child.Key.Attributes[i].Name == "名称")
                                second = child.Key.Attributes[i].Value;
                        }
                        secondTree.GroupName = second;
                        foreach (XMLNodeItem node in child.Value)
                        {
                            TreeFeesObj材料暂估价 obj = new TreeFeesObj材料暂估价();
                            for (int i = 0; i < node.Attributes.Count; i++)
                            {
                                if (node.Attributes[i].Name == "计量单位")
                                    obj.Unit = node.Attributes[i].Value;
                                if (node.Attributes[i].Name == "材料名称")
                                    obj.Name = node.Attributes[i].Value;
                                if (node.Attributes[i].Name == "型号")
                                    obj.Model = node.Attributes[i].Value;
                                if (node.Attributes[i].Name == "规格")
                                    obj.Specification = node.Attributes[i].Value;
                                if (node.Attributes[i].Name == "单价")
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
                    TreeFeesObj材料暂估价 rootTree = new TreeFeesObj材料暂估价();
                    string root = "";
                    for (int i = 0; i < item.Key.Attributes.Count; i++)
                    {
                        if (item.Key.Attributes[i].Name == "名称")
                            root = item.Key.Attributes[i].Value;

                    }
                    rootTree.GroupName = root;
                    foreach (KeyValuePair<XMLNodeItem, List<XMLNodeItem>> child in item.Value)
                    {
                        TreeFeesObj材料暂估价 secondTree = new TreeFeesObj材料暂估价();
                        string second = "";
                        for (int i = 0; i < child.Key.Attributes.Count; i++)
                        {
                            if (child.Key.Attributes[i].Name == "名称")
                                second = child.Key.Attributes[i].Value;
                        }
                        secondTree.GroupName = second;
                        foreach (XMLNodeItem node in child.Value)
                        {
                            TreeFeesObj材料暂估价 obj = new TreeFeesObj材料暂估价();
                            for (int i = 0; i < node.Attributes.Count; i++)
                            {
                                if (node.Attributes[i].Name == "计量单位")
                                    obj.Unit = node.Attributes[i].Value;
                                if (node.Attributes[i].Name == "材料名称")
                                    obj.Name = node.Attributes[i].Value;
                                if (node.Attributes[i].Name == "型号")
                                    obj.Model = node.Attributes[i].Value;
                                if (node.Attributes[i].Name == "规格")
                                    obj.Specification = node.Attributes[i].Value;
                                if (node.Attributes[i].Name == "单价")
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
    public class TreeFeesObj材料暂估价
    {
        public TreeFeesObj材料暂估价() { _children = new ObservableCollection<TreeFeesObj材料暂估价>(); }
        private string _Unit = string.Empty;
        private string _Name = string.Empty;
        private string _Specification = string.Empty;
        private string _GroupName = string.Empty;

        private string _Model = string.Empty;
        private string _Price = string.Empty;

        public string Unit { get { return _Unit; } set { _Unit = value; } }
        public string Name { get { return _Name; } set { _Name = value; } }
        public string Specification { get { return _Specification; } set { _Specification = value; } }
        public string GroupName { get { return _GroupName; } set { _GroupName = value; } }

        public string Model { get { return _Model; } set { _Model = value; } }
        public string Price { get { return _Price; } set { _Price = value; } }

        private ObservableCollection<TreeFeesObj材料暂估价> _children;
        public ObservableCollection<TreeFeesObj材料暂估价> Children
        {
            get { return _children; }
            set { _children = value; }
        }
    }
    public class TreeFeesObjTB材料暂估价
    {
        public TreeFeesObjTB材料暂估价() { _children = new ObservableCollection<TreeFeesObjTB材料暂估价>(); }
        private string _TUnit = string.Empty;
        private string _TName = string.Empty;
        private string _TSpecification = string.Empty;
        private string _TModel = string.Empty;
        private string _TPrice = string.Empty;

        private string _BUnit = string.Empty;
        private string _BName = string.Empty;
        private string _BSpecification = string.Empty;
        private string _BModel = string.Empty;
        private string _BPrice = string.Empty;
        private Brush _Foreground = Brushes.Black;
        private string _ItemIcon = "/XMLContrast;component/Resources/gc.ico";
        private Thickness _Margin = new Thickness(2);

        public string TUnit { get { return _TUnit; } set { _TUnit = value; } }
        public string TName { get { return _TName; } set { _TName = value; } }
        public string TSpecification { get { return _TSpecification; } set { _TSpecification = value; } }
        public string TModel { get { return _TModel; } set { _TModel = value; } }
        public string TPrice { get { return _TPrice; } set { _TPrice = value; } }

        public string BUnit { get { return _BUnit; } set { _BUnit = value; } }
        public string BName { get { return _BName; } set { _BName = value; } }
        public string BSpecification { get { return _BSpecification; } set { _BSpecification = value; } }
        public string BModel { get { return _BModel; } set { _BModel = value; } }
        public string BPrice { get { return _BPrice; } set { _BPrice = value; } }
        private bool _IsExpanded = false;
        public bool IsExpanded { get { return _IsExpanded; } set { _IsExpanded = value; } }
        public string CheckResult { get; set; }
        public string GroupName { get; set; }
        public string ItemIcon { get { return _ItemIcon; } set { _ItemIcon = value; } }
        public Brush Foreground { get { return _Foreground; } set { _Foreground = value; } }
        public Thickness Margin { get { return _Margin; } set { _Margin = value; } }
        private ObservableCollection<TreeFeesObjTB材料暂估价> _children;
        public ObservableCollection<TreeFeesObjTB材料暂估价> Children
        {
            get { return _children; }
            set { _children = value; }
        }
    }
}
