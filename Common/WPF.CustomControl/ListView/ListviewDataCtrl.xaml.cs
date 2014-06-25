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

using System.Windows.Controls.Primitives;
using System.Collections.ObjectModel;
using System.Collections;
using System.IO;

namespace WPF.CustomControl
{
    /// <summary>
    /// listView控件 
    /// 功能自定义列 ：可创建复选框列（全选复选框） 右键菜单 带有标题图片的列
    /// </summary>
    public partial class ListviewDataCtrl : UserControl
    {
        #region 属性
        /// <summary>
        ///  绑定界面数据源 用法需要转换  ObservableCollection<T> tt = lv_DataCtrl.DataItem as ObservableCollection<T>;
        /// </summary>
        public IEnumerable DataItemSource { get { return lv_DataView.ItemsSource; } set { lv_DataView.ItemsSource = value; } }
        /// <summary>
        /// 选择的模式 
        /// </summary>
        public SelectionMode SelectionMode { set { lv_DataView.SelectionMode = value; } }
        /// <summary>
        /// 获取当前选定的项目。
        /// </summary>
        public IList SelectedItems { get { return lv_DataView.SelectedItems; } }

        /// <summary>
        ///获取或设置的第一个项目，在当前的选择，如果选择为空则返回null
        /// </summary>
        public object SelectedItem { get { return lv_DataView.SelectedItem; } set { lv_DataView.SelectedItem = value; } }

        /// <summary>
        /// 获取或设置的索引的第一个项目， 如果选择为空返回（-1）
        /// </summary>
        public int SelectedIndex { get { return lv_DataView.SelectedIndex; } set { lv_DataView.SelectedIndex = value; } }
        /// <summary>
        /// 单元格线的颜色
        /// </summary>
        public Brush GridLineBrush { get { return _GridLineBrush; } set { _GridLineBrush = value; } }
        private Brush _GridLineBrush = Brushes.Gray;

        /// <summary>
        /// 单元格线显隐
        /// </summary>
        public Visibility GridLineVisibility { get { return _GridLineVisibility; } set { _GridLineVisibility = value; } }
        private Visibility _GridLineVisibility = Visibility.Visible;
        /// <summary>
        /// 右键菜单 最小宽度
        /// </summary>
        public double MinWidthContextMenuItem { set { _ContextMenuItems.MinWidth = value; } }
        #endregion

        /// <summary>
        /// 右键菜单集合
        /// </summary>
        private ContextMenu _ContextMenuItems;//右键菜单
        private static object _lockedObj = new object();//锁

        public ListviewDataCtrl()
        {
            InitializeComponent();
            lgld_decorator.DataContext = this;
        }

        #region 私有方法
        /// <summary>
        /// 排序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void gvColumnHeader_Click(object sender, RoutedEventArgs e)
        {
            GridViewColumnHeader gcCol = sender as GridViewColumnHeader;
            WPFCommon.SortColumn(gcCol, lv_DataView);

        }
        /// <summary>
        /// 切换选择项
        /// </summary>
        void lv_DataView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_SelectionChanged != null)
                _SelectionChanged(sender, e);
        }
        //右击
        protected void HandleMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            object model = lv_DataView.SelectedItem;
            if (model == null) { lv_DataView.ContextMenu = null; return; }
            //右击时
            if (_ContextMenuItems == null) _ContextMenuItems = new System.Windows.Controls.ContextMenu();
            if (_ContextMenuItems.Items.Count > 0)
                lv_DataView.ContextMenu = _ContextMenuItems;
            else
                lv_DataView.ContextMenu = null;
        }

        private void HandleDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (_DoubleHandleClick != null)
                _DoubleHandleClick(sender, e);
        }

        #endregion

        #region 开放方法 事件

        #region 初始化

        /// <summary>
        /// 绑定数据到ListView
        /// </summary>
        /// <param name="dataListItemsSource"></param>
        public void BindDataToListViewSource<T>(List<T> dataListItemsSource)
        {
            lv_DataView.SelectionChanged -= new SelectionChangedEventHandler(lv_DataView_SelectionChanged);
            lv_DataView.ItemsSource = new ObservableCollection<T>(dataListItemsSource);
            lv_DataView.SelectedIndex = -1;
            lv_DataView.SelectionChanged += new SelectionChangedEventHandler(lv_DataView_SelectionChanged);
        }
        #region 重载创建 列 方法

        #region 创建含有列头

        private GridView _gvDataView;
        /// <summary>
        /// 创建带有列头的数据绑定列
        /// </summary>
        /// <param name="Header">显示的列头</param>
        /// <param name="bindHeader">绑定的列头</param>
        /// <param name="Width">该列的宽度</param>
        public void CreateListViewDataBindColumns(string Header, string bindHeader, double Width)//创建数据绑定列
        {
            if (_gvDataView == null) _gvDataView = new GridView();
            GridViewColumn gvc = new GridViewColumn();
            GridViewColumnHeader gvColumnHeader = new GridViewColumnHeader();
            gvColumnHeader.Tag = bindHeader;
            gvColumnHeader.Content = Header;
            gvc.Header = gvColumnHeader;
            gvc.DisplayMemberBinding = new Binding(bindHeader);

            gvColumnHeader.Click += new RoutedEventHandler(gvColumnHeader_Click);
            gvColumnHeader.HorizontalContentAlignment = HorizontalAlignment.Left;
            gvc.Width = Width;
            _gvDataView.Columns.Add(gvc);
            lv_DataView.View = _gvDataView;
        }
        /// <summary>
        /// 创建带有列头 且数据源含有图片的 列
        /// </summary>
        /// <param name="Header">显示的列头</param>
        /// <param name="bindHeader">绑定的列头</param>
        /// <param name="bindImage">该列绑定的标题图片</param>
        /// <param name="Width">该列的宽度</param>
        public void CreateListViewDataBindColumns(string Header, string bindHeader, string bindImage, double Width, Thickness thickness)//创建数据绑定列
        {
            if (_gvDataView == null) _gvDataView = new GridView();
            GridViewColumn gvc = new GridViewColumn();
            GridViewColumnHeader gvColumnHeader = new GridViewColumnHeader();
            StackPanel sp = new StackPanel();
            DataTemplate dTemplate = new DataTemplate();
            gvc.CellTemplate = dTemplate;
            FrameworkElementFactory FEFSP = new FrameworkElementFactory(typeof(StackPanel));
            FEFSP.SetValue(StackPanel.OrientationProperty, Orientation.Horizontal);
            FrameworkElementFactory FEFIMAGE = new FrameworkElementFactory(typeof(Image));
            Binding imgBind = new Binding();
            imgBind.IsAsync = true;
            imgBind.Path = new PropertyPath(bindImage);
            imgBind.Mode = BindingMode.TwoWay;
            FEFIMAGE.SetValue(Image.SourceProperty, imgBind);
            FEFIMAGE.SetValue(Image.MarginProperty, thickness);
            FrameworkElementFactory FEFTB = new FrameworkElementFactory(typeof(TextBlock) );
            Binding tbBind = new Binding();
            tbBind.Path = new PropertyPath(bindHeader);
            tbBind.Mode = BindingMode.TwoWay;
            FEFTB.SetValue(TextBlock.TextProperty, tbBind);
            FEFTB.SetValue(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center);
            FEFSP.AppendChild(FEFIMAGE);
            FEFSP.AppendChild(FEFTB);
            dTemplate.VisualTree = FEFSP;
            gvc.Header = gvColumnHeader;
            gvColumnHeader.Tag = bindHeader;
            gvColumnHeader.Content = Header;
            gvColumnHeader.Click += new RoutedEventHandler(gvColumnHeader_Click);
            gvColumnHeader.HorizontalContentAlignment = HorizontalAlignment.Left;
            gvc.Width = Width;
            _gvDataView.Columns.Add(gvc);
            lv_DataView.View = _gvDataView;

        }
        /// <summary>
        /// 创建带有列头为复选框的列
        /// </summary>
        /// <param name="SelectedAllClick">全选事件 参数说明：DoSelectedAllClick(object sender, RoutedEventArgs e)</param>
        /// <param name="SelectonedClick">单击事件</param>
        /// <param name="bindCheck">绑定列</param>
        /// <param name="width">列宽</param>
        /// <param name="isFixedWidth">是否固定列宽</param>
        public void CreateListViewDataBindColumns(RoutedEventHandler SelectedAllClick, RoutedEventHandler SelectonedClick, string bindCheck, double width, bool isFixedWidth)
        {
            if (_gvDataView == null) _gvDataView = new GridView();
            if (isFixedWidth)
            {
                xFixedWidthGridViewColumn gvc = new xFixedWidthGridViewColumn();
                GridViewColumnHeader gvColumnHeader = new GridViewColumnHeader();
                DataTemplate dTemplate = new DataTemplate();
                //创建绑定的复选框列
                FrameworkElementFactory FEFCB = new FrameworkElementFactory(typeof(CheckBox));
                FEFCB.AddHandler(CheckBox.ClickEvent, new RoutedEventHandler(SelectonedClick));
                Binding bindC = new Binding();
                bindC.Path = new PropertyPath(bindCheck);
                FEFCB.SetValue(CheckBox.IsCheckedProperty, bindC);
                bindC.Mode = BindingMode.TwoWay;
                dTemplate.VisualTree = FEFCB;
                gvc.CellTemplate = dTemplate;
                //全选框
                CheckBox cbAll = new CheckBox();
                cbAll.Click += new RoutedEventHandler(SelectedAllClick);
                gvColumnHeader.Content = cbAll;
                gvColumnHeader.Tag = bindCheck;
                gvc.Header = gvColumnHeader;
                gvColumnHeader.HorizontalContentAlignment = HorizontalAlignment.Left;

                gvc.FixedWidth = width;
                _gvDataView.Columns.Add(gvc);
            }
            else
            {
                GridViewColumn gvc = new GridViewColumn();
                GridViewColumnHeader gvColumnHeader = new GridViewColumnHeader();
                DataTemplate dTemplate = new DataTemplate();
                //创建绑定的复选框列
                FrameworkElementFactory FEFCB = new FrameworkElementFactory(typeof(CheckBox));
                FEFCB.AddHandler(CheckBox.ClickEvent, new RoutedEventHandler(SelectonedClick));
                Binding bindC = new Binding();
                bindC.Path = new PropertyPath(bindCheck);
                FEFCB.SetValue(CheckBox.IsCheckedProperty, bindC);
                bindC.Mode = BindingMode.TwoWay;
                dTemplate.VisualTree = FEFCB;

                gvc.CellTemplate = dTemplate;
                //全选框
                CheckBox cbAll = new CheckBox();
                cbAll.Click += new RoutedEventHandler(SelectedAllClick);
                gvColumnHeader.Content = cbAll;
                gvColumnHeader.Tag = bindCheck;
                gvc.Header = gvColumnHeader;
                gvColumnHeader.HorizontalContentAlignment = HorizontalAlignment.Left;

                gvc.Width = width;
                _gvDataView.Columns.Add(gvc);

            }

            lv_DataView.View = _gvDataView;
        }

        #endregion

        #region 不含有列头

        public ListView lvData { get { return lv_DataView; } }
        /// <summary>
        /// 创建不含列头的单列  
        /// </summary>
        /// <param name="bindHeader">绑定列</param>
        /// <param name="minWidth">最小列宽</param>
        public void CreateListViewDataBindColumns(string bindHeader, double minWidth)
        {
            if (lv_DataView.ItemTemplate != null) return;

            DataTemplate dTemplate = new DataTemplate();
            FrameworkElementFactory FEFSP = new FrameworkElementFactory(typeof(StackPanel));
            FEFSP.SetValue(StackPanel.OrientationProperty, Orientation.Horizontal);

            FrameworkElementFactory FEFTB = new FrameworkElementFactory(typeof(TextBlock));
            Binding tbBind = new Binding();
            tbBind.Path = new PropertyPath(bindHeader);
            FEFTB.SetValue(TextBlock.TextProperty, tbBind);
            FEFSP.AppendChild(FEFTB);
            dTemplate.VisualTree = FEFSP;
            lv_DataView.MinWidth = minWidth;
            lv_DataView.HorizontalAlignment = HorizontalAlignment.Left;
            lv_DataView.ItemTemplate = dTemplate;
        }
        /// <summary>
        /// 创建不含列头且含有图片的单列  
        /// </summary>
        /// <param name="bindHeader">绑定列</param>
        /// <param name="bindImage">绑定图片</param>
        /// <param name="minWidth">最小列宽</param>
        public void CreateListViewDataBindColumns(string bindHeader, string bindImage, double minWidth, double height, Thickness thickness)
        {
            if (lv_DataView.ItemTemplate != null) return;

            DataTemplate dTemplate = new DataTemplate();
            FrameworkElementFactory FEFSP = new FrameworkElementFactory(typeof(StackPanel));
            FEFSP.SetValue(StackPanel.OrientationProperty, Orientation.Horizontal);
            FrameworkElementFactory FEFTB = new FrameworkElementFactory(typeof(TextBlock));
            Binding tbBind = new Binding();
            tbBind.Path = new PropertyPath(bindHeader);
            FEFTB.SetValue(TextBlock.TextProperty, tbBind);

            FrameworkElementFactory FEFIMAGE = new FrameworkElementFactory(typeof(Image));
            Binding imgBind = new Binding();
            imgBind.Path = new PropertyPath(bindImage);
            FEFIMAGE.SetValue(Image.SourceProperty, imgBind);
            FEFIMAGE.SetValue(HeightProperty, height);
            FEFIMAGE.SetValue(Image.MarginProperty, thickness);
            FEFSP.AppendChild(FEFIMAGE);
            FEFSP.AppendChild(FEFTB);
            dTemplate.VisualTree = FEFSP;
            lv_DataView.MinWidth = minWidth;
            lv_DataView.HorizontalAlignment = HorizontalAlignment.Left;
            lv_DataView.ItemTemplate = dTemplate;
        }
        #endregion

        #endregion

        #region 注册右键菜单
        /// <summary>
        ///注册 选中项 右键 事件
        /// </summary>
        /// <param name="header">显示名称</param>
        /// <param name="tag">标识号</param>
        /// <param name="imageIco">图片 例如："/项目名;component/Resources/tick.png" 和"/Resources/tick.png" </param>
        /// <param name="mc">点击事件  如：DoClick (object sender, RoutedEventHandler e)  </param>
        public void AddRightContexMenu(string header, object tag, string imageIco, RoutedEventHandler mc)
        {
            MenuItem result = new MenuItem();
            result.Icon = new System.Windows.Controls.Image
            {
                Source = new BitmapImage(new Uri(imageIco, UriKind.Relative))
            };
            result.Header = header;
            result.Tag = tag;
            result.Margin = new Thickness(2);
            result.Click += new RoutedEventHandler(mc);
            if (_ContextMenuItems == null) _ContextMenuItems = new System.Windows.Controls.ContextMenu();
            _ContextMenuItems.Items.Add(result);
        }
        /// <summary>
        /// 注册 选中项 右键 事件
        /// </summary>
        /// <param name="values"></param>
        public void AddRightContexMenu(params MenuItem[] values)
        {
            if (values == null || values.Length == 0) return;
            foreach (MenuItem item in values)
            {
                if (_ContextMenuItems == null)
                    _ContextMenuItems = new System.Windows.Controls.ContextMenu();
                _ContextMenuItems.Items.Add(item);
            }
        }

        /// <summary>
        /// 添加右键菜单分隔符（隐藏分割线必须设置tag）
        /// </summary>
        public void AddRightMenuSeparator()
        {
            Separator separator = new Separator();
            separator.Margin = new Thickness(0);
            if (_ContextMenuItems == null) _ContextMenuItems = new ContextMenu();
            _ContextMenuItems.Items.Add(separator);
        }
        /// <summary>
        /// 添加右键菜单分隔符（隐藏分割线必须设置tag）
        /// </summary>
        /// <param name="tag">分隔符标识号 </param>
        public void AddRightMenuSeparator(object tag)
        {
            Separator separator = new Separator();
            separator.Tag = tag;
            separator.Margin = new Thickness(0);
            if (_ContextMenuItems == null) _ContextMenuItems = new ContextMenu();
            _ContextMenuItems.Items.Add(separator);
        }
        #endregion

        #endregion

        #region 设置右键
        /// <summary>
        /// 重置右键菜单初始值
        /// </summary>
        public void ResetContextMenuItems()
        {
            _ContextMenuItems = new ContextMenu();
        }
        #region 右键
        /// <summary>
        /// 设置右键菜单的可用状态
        /// </summary>
        /// <param name="isEnabled">状态（false 不可用， true 可用）</param>
        /// <param name="values">需要设置的菜单项 的标识号集合    string[] values = new string[] { "tag" }; </param>
        public void SetRightButtonStatus(bool isEnabled, params object[] values)
        {
            if (_ContextMenuItems == null) _ContextMenuItems = new System.Windows.Controls.ContextMenu();
            if (values != null && values.Length > 0)
            {
                foreach (object item in values)
                {
                    for (int i = 0; i < _ContextMenuItems.Items.Count; i++)
                    {
                        MenuItem hlMi = _ContextMenuItems.Items[i] as MenuItem;
                        if (hlMi == null) continue;
                        string tagstr = hlMi.Tag.ToString();
                        if (tagstr.Equals(item.ToString(), StringComparison.OrdinalIgnoreCase))
                        {
                            hlMi.IsEnabled = isEnabled;
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 设置右键菜单的显隐状态  
        /// </summary>
        /// <param name="isVisibility">状态（Visible 显示,Hidden 隐藏、预留空间布局中的元素， Collapsed 隐藏、不预留空间布局中的元素）</param>
        /// <param name="values">需要设置的菜单项 的标识号集合  如：  string[] values = new string[] { "tag" }; </param>
        /// TODO:用法： string[] values = new string[] { "tag1", "tag2", "tag3", "tag4" };  SetRightButtonClick(Visibility.Collapsed, values);
        public void SetRightButtonStatus(Visibility isVisibility, params object[] values)
        {
            if (_ContextMenuItems == null) _ContextMenuItems = new System.Windows.Controls.ContextMenu();
            if (values != null && values.Length > 0)
            {
                foreach (object item in values)
                {
                    for (int i = 0; i < _ContextMenuItems.Items.Count; i++)
                    {
                        MenuItem hlMi = _ContextMenuItems.Items[i] as MenuItem;
                        if (hlMi == null) continue;
                        string tagstr = hlMi.Tag.ToString();
                        if (tagstr.Equals(item.ToString(), StringComparison.OrdinalIgnoreCase))
                        {
                            hlMi.Visibility = isVisibility;
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 设置右键菜单中分割线的显隐状态
        /// </summary>
        /// <param name="isVisibility">状态（Visible 显示,Hidden 隐藏、预留空间布局中的元素， Collapsed 隐藏、不预留空间布局中的元素）</param>
        /// <param name="values">需要设置的菜单项 的标识号集合  如： string[] values = new string[] { "tag" }; </param>
        public void SetRightMenuSeparatorStatus(Visibility isVisibility, params object[] values)
        {
            if (_ContextMenuItems == null) return;
            if (values != null && values.Length > 0)
            {
                foreach (object item in values)
                {
                    for (int i = 0; i < _ContextMenuItems.Items.Count; i++)
                    {
                        Separator hlMi = _ContextMenuItems.Items[i] as Separator;
                        if (hlMi == null) continue;
                        string tagstr = hlMi.Tag.ToString();
                        if (tagstr.Equals(item.ToString(), StringComparison.OrdinalIgnoreCase))
                        {
                            hlMi.Visibility = isVisibility;
                            break;
                        }
                    }
                }
            }
        }

        #endregion

        #endregion

        #region 事件
        /// <summary>
        /// listview 双击左键事件
        /// </summary>
        public RoutedEventHandler DoubleHandleClick { get { return _DoubleHandleClick; } set { _DoubleHandleClick = value; } }
        public SelectionChangedEventHandler DoSelectionChanged { get { return _SelectionChanged; } set { _SelectionChanged = value; } }
        #endregion


        #endregion

        private SelectionChangedEventHandler _SelectionChanged;
        private RoutedEventHandler _DoubleHandleClick;

    }
}
