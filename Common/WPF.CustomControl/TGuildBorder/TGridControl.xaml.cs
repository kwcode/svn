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

namespace WPF.CustomControl
{
    /// <summary>
    /// Interaction logic for TGridControl.xaml
    /// </summary>
    public partial class TGridControl : UserControl
    {
        /// <summary>
        /// 行坐标
        /// </summary>
        public int RowIndex { get; set; }
        /// <summary>
        /// 列坐标
        /// </summary>
        public int ColIndex { get; set; }
        public TGridControl()
        {
            InitializeComponent();
            borderControl.Style = (Style)FindResource("styleBorder");
            tb_Content.PreviewMouseDown += new MouseButtonEventHandler(tb_Content_PreviewMouseDown);
        }

        public event MouseButtonHandler DOMouseButtonDoubleClick;
        void tb_Content_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                if (_IsHaveDoubleClick)
                {
                    if (DOMouseButtonDoubleClick != null)
                        DOMouseButtonDoubleClick(sender, e, this);
                }
            }
        }
        public string TextContent
        {
            get { return tb_Content.Text; }
            set
            {
                tb_Content.Text = value;
                if (value != null && value != string.Empty)
                {
                    borderControl.Style = (Style)FindResource("styleBorderContent");
                }
            }
        }
        /// <summary>
        /// 唯一标识号
        /// </summary>
        public string ID { get; set; }
        public Visibility VisibilityBorder { set { borderControl.Visibility = value; } }

        public StyleType ResourceType
        {
            set
            {
                if (value == StyleType.RowTitle)
                    borderControl.Style = (Style)FindResource("styleBorderRowTitle");
                if (value == StyleType.ColTitle)
                    borderControl.Style = (Style)FindResource("styleBorderColTitle");
                if (value == StyleType.Today)
                    borderControl.Style = (Style)FindResource("styleBorderToday");
                if (value == StyleType.Yesterday)
                    borderControl.Style = (Style)FindResource("styleBorderYesterday");
                if (value == StyleType.Tomorrow)
                    borderControl.Style = (Style)FindResource("styleBorderTomorrow");

            }
        }
        /// <summary>
        /// 是否有双击事件
        /// </summary>
        public bool IsHaveDoubleClick { get { return _IsHaveDoubleClick; } set { _IsHaveDoubleClick = value; } }
        private bool _IsHaveDoubleClick = true;

    }
    public delegate void MouseButtonHandler(object sender, MouseButtonEventArgs e, TGridControl tControl);
    public enum StyleType
    {
        Default = 0,
        RowTitle = 1,
        ColTitle = 2,
        Today = 3,
        Yesterday = 4,
        Tomorrow = 5
    }
}
