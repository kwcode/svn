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
using System.Windows.Shapes;
using WPF.CustomControl;
using Microsoft.Win32;
using System.Xml;

namespace XMLContrast
{
    /// <summary>
    /// ListAssistantWin.xaml 的交互逻辑
    /// </summary>
    public partial class ListAssistantWin : HLWindowExt
    {
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
        public ListAssistantWin()
        {
            InitializeComponent();
            lv_DataMenu.SelectionChanged += new SelectionChangedEventHandler(lv_DataMenu_SelectionChanged);
            // lv_DataMenu.SelectedIndex = 0;
            Init();
        }

        private void Init()
        {
            btn_ImportTender.Click += new RoutedEventHandler(btn_ImportTender_Click);
            btn_Analysis.Click += new RoutedEventHandler(btn_Analysis_Click);
            btn_ImportBid.Click += new RoutedEventHandler(btn_ImportBid_Click);

        }
        void lv_DataMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListViewItem lv = lv_DataMenu.SelectedItem as ListViewItem;
            if (lv == null) return;
            if (lv.Tag == null) return;
            if (tb_Tender.Text == string.Empty)
            {
                MessageBox.Show(this, "请导入招清单");
                return;
            }
            if (tb_Bid.Text == string.Empty)
            {
                MessageBox.Show(this, "请导入投清单");
                return;
            }
            if (!_isAnaly)
            {
                MessageBox.Show(this, "请检查分析");
                return;
            }
            ctrl_tabview.ItemHeader = lv.Content.ToString();
            ctrl_tabview.SelectedItem(lv.Tag.ToString(), this); 
        }
        bool _isAnaly = false;
        void btn_Analysis_Click(object sender, RoutedEventArgs e)
        {
            if (tb_Tender.Text == string.Empty)
            {
                MessageBox.Show(this, "请导入招清单");
                return;
            }
            if (tb_Bid.Text == string.Empty)
            {
                MessageBox.Show(this, "请导入投清单");
                return;
            }
            //检查分析
            _isAnaly = true;
            ctrl_tabview.Init(_TenderDataNodes, _BidDataNodes);
            ListViewItem lv = lv_DataMenu.SelectedItem as ListViewItem;
            if (lv == null) return;
            ctrl_tabview.SelectedItem(lv.Tag.ToString(), this); 
        }
        #region 导入、导出
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
            _isAnaly = false;
            tb_Bid.Text = "<=>投标地址：" + path;
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(path);
            List<XMLNodeItem> xmlNodes = XMLOperat.ResolveXML(xmldoc);
            // tree_Data.ItemsSource = 
            _BidDataNodes = xmlNodes;
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
            _isAnaly = false;
            tb_Tender.Text = "招标地址：" + path;
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(path);
            List<XMLNodeItem> xmlNodes = XMLOperat.ResolveXML(xmldoc);
            _TenderDataNodes = xmlNodes;
            StopProgress();
        }
        #endregion
    }
}
