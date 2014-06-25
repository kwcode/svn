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

namespace WPF.CustomControl
{
    /// <summary>
    /// Interaction logic for TGridBuilder.xaml
    /// </summary>
    public partial class TGridBuilder : UserControl
    {
        public TGridBuilder()
        {
            InitializeComponent();
        }

        public DateTime StartTime { get; set; }
        #region 初始化
        Grid gd_Content = new Grid();
        public void InitializeUI()
        {
            _BordGrid = new List<BordGrid>();
            gd_Content = new Grid();
            CreateLineBorder(4, 8);
            SetGuidDefault(4, 8);
            SetBordGridStyle(StartTime);
            sv_viewer.Content = gd_Content;
        }
        /// <summary>
        /// 根据时间 设置界面样式
        /// </summary>
        /// <param name="StartTime"></param>
        private void SetBordGridStyle(DateTime StaTime)
        {
            int days = (DateTime.Now - StartTime).Days;
            if (_BordGrid == null || _BordGrid.Count == 0) return;
            foreach (BordGrid item in _BordGrid)
            {
                if (item.rowIndex == 0) continue;
                if (item.colIndex == 0) continue;
                if (item.colIndex < 5)
                    item.BorderControl.ResourceType = StyleType.Yesterday;
                if (item.colIndex == 5)
                    item.BorderControl.ResourceType = StyleType.Today;
                if (item.colIndex > 5)
                {
                    item.BorderControl.ResourceType = StyleType.Tomorrow;
                    item.BorderControl.IsHaveDoubleClick = false;
                }
            }
        }
        /// <summary>
        /// 设置默认标题栏
        /// </summary>
        /// <param name="rowCount"></param>
        /// <param name="colCount"></param>
        private void SetGuidDefault(int rowCount, int colCount)
        {
            if (_BordGrid == null || _BordGrid.Count == 0) return;
            if (rowCount != 4 || colCount != 8) return;
            foreach (BordGrid item in _BordGrid)
            {
                if (item.rowIndex == 0)
                {

                    if (item.colIndex == 0)
                        item.BorderControl.VisibilityBorder = Visibility.Collapsed;
                    if (item.colIndex == 1)
                        item.BorderControl.TextContent = "星期日 [" + StartTime.Day + "]";
                    if (item.colIndex == 2)
                        item.BorderControl.TextContent = "星期一 [" + StartTime.AddDays(1).Day + "]";
                    if (item.colIndex == 3)
                        item.BorderControl.TextContent = "星期二 [" + StartTime.AddDays(2).Day + "]";
                    if (item.colIndex == 4)
                        item.BorderControl.TextContent = "星期三 [" + StartTime.AddDays(3).Day + "]";
                    if (item.colIndex == 5)
                        item.BorderControl.TextContent = "星期四 [" + StartTime.AddDays(4).Day + "]";
                    if (item.colIndex == 6)
                        item.BorderControl.TextContent = "星期五 [" + StartTime.AddDays(5).Day + "]";
                    if (item.colIndex == 7)
                        item.BorderControl.TextContent = "星期六 [" + StartTime.AddDays(6).Day + "]";
                    item.BorderControl.ResourceType = StyleType.RowTitle;
                    item.BorderControl.IsHaveDoubleClick = false;
                }
                if (item.colIndex == 0)
                {

                    if (item.rowIndex == 1)
                        item.BorderControl.TextContent = "上午（0-10）";
                    if (item.rowIndex == 2)
                        item.BorderControl.TextContent = "中午（10-12）";
                    if (item.rowIndex == 3)
                        item.BorderControl.TextContent = "下午（13-24）";
                    item.BorderControl.ResourceType = StyleType.ColTitle;
                    item.BorderControl.IsHaveDoubleClick = false;
                }
            }
        }
        private List<BordGrid> _BordGrid = new List<BordGrid>();
        /// <summary>
        /// 生成整个界面布局
        /// </summary>
        /// <param name="rowCount">总行数</param>
        /// <param name="colCount">总列数</param>
        private void CreateLineBorder(int rowCount, int colCount)
        {
            //double dwidth = GetProportionWidth(this.Width, colCount);
            //double dheitht = GetProportionHeight(this.Height, rowCount);
            #region 确定格子数
            for (int i = 0; i < rowCount; i++)
            {
                RowDefinition row = new RowDefinition();
                if (i == 0)
                    row.Height = GridLength.Auto;
                gd_Content.RowDefinitions.Add(row);
            }
            for (int j = 0; j < colCount; j++)
            {
                ColumnDefinition col = new ColumnDefinition();
                if (j == 0)
                    col.Width = GridLength.Auto;
                gd_Content.ColumnDefinitions.Add(col);

            }
            #endregion
            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < colCount; j++)
                {
                    Style style = Resources["LineBorder"] as Style;
                    Border lineBorder = new Border();
                    lineBorder.Style = style;
                    lineBorder.SetValue(Grid.RowProperty, i);
                    lineBorder.SetValue(Grid.ColumnProperty, j);
                    gd_Content.Children.Add(lineBorder);
                    TGridControl tControl = new TGridControl();
                    tControl.DOMouseButtonDoubleClick -= new MouseButtonHandler(tControl_DOMouseButtonDoubleClick);
                    tControl.DOMouseButtonDoubleClick += new MouseButtonHandler(tControl_DOMouseButtonDoubleClick);
                    tControl.RowIndex = i;
                    tControl.ColIndex = j;
                    lineBorder.Child = tControl;
                    _BordGrid.Add(new BordGrid() { rowIndex = i, colIndex = j, BorderControl = tControl });
                }
            }
        }

        void tControl_DOMouseButtonDoubleClick(object sender, MouseButtonEventArgs e, TGridControl tControl)
        {
            if (DOMouseButtonDoubleClick != null)
                DOMouseButtonDoubleClick(sender, e, tControl);
        }
        #endregion

        public event MouseButtonHandler DOMouseButtonDoubleClick;
        /// <summary>
        /// 设置单元格
        /// </summary>
        /// <param name="rowIndex">行</param>
        /// <param name="colIndex">列</param>
        /// <param name="bgControl">单元格内容</param>
        public void SetBorderControl(int rowIndex, int colIndex, TGridControl bgControl)
        {
            if (_BordGrid == null || _BordGrid.Count == 0) return;
            if (bgControl.ID == null) return;
            foreach (BordGrid item in _BordGrid)
            {
                if (item.colIndex == colIndex && item.rowIndex == rowIndex)
                {
                    item.BorderControl.TextContent = bgControl.TextContent;
                    item.BorderControl.ID = bgControl.ID;
                    break;
                }
            }
        }
    }

}
