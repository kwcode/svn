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
using Microsoft.Win32;
using System.Xml;
using System.Reflection;
using System.Data;
using System.Dynamic;

namespace XMLContrast
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : HLWindowExt
    {
        public MainWindow()
        {
            InitializeComponent();
            Init();
            TextBlock tt = new TextBlock();
        }
        #region 属性 、缓存
        /// <summary>
        /// 招标信息
        /// </summary>
        private List<XMLNodeItem> _TenderDataNodes = new List<XMLNodeItem>();
        /// <summary>
        /// 导入投清单
        /// </summary>
        private List<XMLNodeItem> _BidDataNodes = new List<XMLNodeItem>();

        #endregion

        private void Init()
        {
            btn_ImportTender.Click += new RoutedEventHandler(btn_ImportTender_Click);
            btn_Analysis.Click += new RoutedEventHandler(btn_Analysis_Click);
            btn_ImportBid.Click += new RoutedEventHandler(btn_ImportBid_Click);
            tree_Data.SelectedItemChanged += new RoutedPropertyChangedEventHandler<object>(tree_Data_SelectedItemChanged);
            //  btn_Contrast.Click += new RoutedEventHandler(btn_Contrast_Click);
        }
        #region 对比

        void btn_Contrast_Click(object sender, RoutedEventArgs e)
        {
            DGrid_BidData.SelectedIndex = -1;
            ContrastDBGrid(DGrid_TenderData, DGrid_BidData, Colors.Red);

        }
        /// <summary>
        /// 对比2个数据表
        /// </summary>
        /// <param name="dGridT">对比数据列表1</param>
        /// <param name="dGridB">对比数据列表2</param>
        /// <param name="color">对比数据差颜色 显示在列表2</param>
        public void ContrastDBGrid(DataGrid dGridT, DataGrid dGridB, Color color)
        {
            int TRowCount = dGridT.Items.Count;
            int BRowCount = dGridB.Items.Count;
            if (TRowCount == 0 || BRowCount == 0) return;//没有 数据

            if (TRowCount >= BRowCount)//1>=2 表数据 
            {
                int TColCount = dGridT.Columns.Count;
                int BColCount = dGridB.Columns.Count;
                if (TColCount == 0 || BColCount == 0) return;//没有 数据
                for (int i = 0; i < BRowCount; i++)
                {
                    DataRowView TDataRowView = dGridT.Items[i] as DataRowView;
                    DataRowView BDataRowView = dGridB.Items[i] as DataRowView;
                    if (TDataRowView == null || BDataRowView == null) continue;
                    for (int j = 0; j < BColCount; j++)
                    {
                        string strT = TDataRowView.Row.ItemArray[j] as string;
                        string strB = BDataRowView.Row.ItemArray[j] as string;
                        if (!strT.Equals(strB))
                        {
                            //TODO:为null的时候需要修改
                            TextBlock TtbLock = dGridT.Columns[j].GetCellContent(dGridT.Items[i]) as TextBlock;
                            TextBlock BtbLock = dGridB.Columns[j].GetCellContent(dGridB.Items[i]) as TextBlock;
                            if (TtbLock != null)
                            {
                                if (!string.IsNullOrEmpty(TtbLock.Text.Trim()))
                                    TtbLock.Foreground = Brushes.Yellow;
                                else
                                    TtbLock.Background = Brushes.DodgerBlue;
                            }
                            if (BtbLock != null)
                            {
                                if (!string.IsNullOrEmpty(BtbLock.Text.Trim()))
                                    BtbLock.Foreground = Brushes.Red;
                                else
                                    BtbLock.Background = Brushes.Blue;
                            }
                        }
                    }
                }
            }
            else
            {

            }
        }

        #endregion
        //private List<XMLNodeItem> _xmlTenderNodeItems;
        //private List<XMLNodeItem> _xmlBidNodeItems;
        void tree_Data_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            XMLNodeItem selectItem = tree_Data.SelectedItem as XMLNodeItem;
            if (selectItem == null) return;
            StartProgress();
            List<XMLNodeItem> _xmlTenderNodeItems = GetDataChildNodes(selectItem.Name, _TenderDataNodes);
            List<XMLNodeItem> _xmlBidNodeItems = GetDataChildNodes(selectItem.Name, _BidDataNodes);
            DataShow(_xmlTenderNodeItems, _xmlBidNodeItems);
            StopProgress();
        }
        #region 获取树中匹配的节点集合

        /// <summary>
        /// 获取树中匹配的节点集合
        /// </summary>
        /// <param name="name">匹配值</param>
        /// <param name="dataNodes">整个树</param>
        /// <returns></returns>
        private List<XMLNodeItem> GetDataChildNodes(string name, List<XMLNodeItem> dataNodes)
        {
            List<XMLNodeItem> nodeItems = new List<XMLNodeItem>();
            foreach (XMLNodeItem item in dataNodes)
            {
                if (item.Name == name)
                    nodeItems.Add(item);

                if (item.ChildNodes != null && item.ChildNodes.Count != 0)
                {
                    SetDataChildNodes(nodeItems, item.ChildNodes, name);
                }
            }
            return nodeItems;
        }

        private void SetDataChildNodes(List<XMLNodeItem> nodeItems, List<XMLNodeItem> childNodes, string name)
        {
            foreach (XMLNodeItem item in childNodes)
            {
                if (item.Name == name)
                    nodeItems.Add(item);
                if (item.ChildNodes != null && item.ChildNodes.Count != 0)
                {
                    SetDataChildNodes(nodeItems, item.ChildNodes, name);
                }
            }
        }
        #endregion

        #region 数据分析
        //数据分析
        void btn_Analysis_Click(object sender, RoutedEventArgs e)
        {
            StartProgress();
            DGrid_BidData.SelectedIndex = -1;
            ContrastDBGrid(DGrid_TenderData, DGrid_BidData, Colors.Red);
            StopProgress();
        }
        /// <summary>
        /// 数据展示
        /// </summary>
        /// <param name="xmlTenderNodeItems"></param>
        /// <param name="xmlBidNodeItems"></param>
        private void DataShow(List<XMLNodeItem> xmlTenderNodeItems, List<XMLNodeItem> xmlBidNodeItems)
        {
            #region 导入招标清单

            if (xmlTenderNodeItems == null || xmlTenderNodeItems.Count == 0) return;
            //
            DataTable dt = GetDataTable(xmlTenderNodeItems);
            DGrid_TenderData.ItemsSource = dt.DefaultView;

            #endregion

            #region 导入投清单

            if (xmlBidNodeItems == null || xmlBidNodeItems.Count == 0) return;
            DataTable dt2 = GetDataTable(xmlBidNodeItems);
            DGrid_BidData.ItemsSource = dt2.DefaultView;

            #endregion

        }

        private DataTable GetDataTable(List<XMLNodeItem> _xmlNodeItems)
        {
            DataTable dt = new DataTable();
            List<XMLAttributes> attributes = _xmlNodeItems[0].Attributes;
            foreach (XMLAttributes item in attributes)
            {
                dt.Columns.Add(item.Name);
            }
            foreach (XMLNodeItem child in _xmlNodeItems)
            {
                if (child.Attributes == null || child.Attributes.Count == 0) continue;
                DataRow dr = dt.NewRow();
                foreach (XMLAttributes item in child.Attributes)
                {
                    dr[item.Name] = item.Value;
                }
                dt.Rows.Add(dr);
            }

            return dt;
        }

        #endregion

        #region 导入招标清单
        //导入招标清单
        void btn_ImportTender_Click(object sender, RoutedEventArgs e)
        {
            StartProgress();
            string path = GetFilePath();
            if (string.IsNullOrEmpty(path))
            {
                MessageBox.Show("请选择XML文件！");
                StopProgress();
                return;
            }
            tb_Tender.Text = path;
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(path);
            List<XMLNodeItem> xmlNodes = XMLOperat.ResolveXML(xmldoc);
            tree_Data.ItemsSource = _TenderDataNodes = xmlNodes;
            StopProgress();
        }
        /// <summary>
        /// 打开文件地址
        /// </summary>
        /// <returns></returns>
        private string GetFilePath()
        {
            string path = "";
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "XML|*xml";
            bool? b = ofd.ShowDialog();
            if (b.Value)
            {
                path = ofd.FileName;
            }
            return path;
        }
        #endregion

        #region 导入投清单
        void btn_ImportBid_Click(object sender, RoutedEventArgs e)
        {
            StartProgress();
            string path = GetFilePath();
            if (string.IsNullOrEmpty(path))
            {
                MessageBox.Show("请选择XML文件！");
                StopProgress();
                return;
            }
            tb_Bid.Text = path;
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(path);
            List<XMLNodeItem> xmlNodes = XMLOperat.ResolveXML(xmldoc);
            // tree_Data.ItemsSource = 
            _BidDataNodes = xmlNodes;
            StopProgress();
        }
        #endregion
    }

}
