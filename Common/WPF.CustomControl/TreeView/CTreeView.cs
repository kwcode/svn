using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using System.Windows.Input;

namespace WPF.CustomControl
{
    public class CTreeView : TreeView
    {
        private CTreeView _CTreeView;
        public CTreeView()
        {
            _CTreeView = this;
            _CTreeView.MouseRightButtonDown += new MouseButtonEventHandler(_CTreeView_MouseRightButtonDown);
        }
        /// <summary>
        /// 绑定数据源
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="datasouce">数据源</param>
        public void BindDataToTree<T>(ObservableCollection<T> datasouce)
        {
            _CTreeView.ItemsSource = datasouce;
        }

        #region 重载创建绑定列
        /// <summary>
        /// 创建树的绑定列（只能创建一次）
        /// </summary>
        /// <param name="nodeText">显示的节点值</param>
        /// <param name="children">子节点</param>
        public void CreateTreeViewDataBindColumns(string nodeText, string children)
        {
            if (_CTreeView.ItemTemplate != null) return;
            DataTemplate dTemplate = new DataTemplate();
            HierarchicalDataTemplate hDataTemplate = new HierarchicalDataTemplate();
            dTemplate = hDataTemplate;
            Binding hDataTemplateBind = new Binding();
            hDataTemplateBind.Mode = BindingMode.TwoWay;
            hDataTemplateBind.Path = new PropertyPath(children);
            hDataTemplate.ItemsSource = hDataTemplateBind;
            FrameworkElementFactory FEFTB = new FrameworkElementFactory(typeof(TextBlock));
            FEFTB.AddHandler(TextBlock.MouseRightButtonUpEvent, new MouseButtonEventHandler(DoItemMouseRightButtonDown));

            Binding tbBind = new Binding();
            tbBind.Mode = BindingMode.TwoWay;
            tbBind.Path = new PropertyPath(nodeText);
            FEFTB.SetBinding(TextBlock.TextProperty, tbBind);
            hDataTemplate.VisualTree = FEFTB;
            _CTreeView.ItemTemplate = dTemplate;
        }
        /// <summary>
        /// 创建带有图片的绑定列（只能创建一次）
        /// </summary>
        /// <param name="nodeText">显示的节点值</param>
        /// <param name="children">子节点</param>
        /// <param name="ImageIco">图片</param>
        /// <param name="thickness">间距</param>
        public void CreateTreeViewDataBindColumns(string nodeText, string children, string ImageIco, Thickness thickness)
        {
            if (_CTreeView.ItemTemplate != null) return;
            DataTemplate dTemplate = new DataTemplate();
            HierarchicalDataTemplate hDataTemplate = new HierarchicalDataTemplate();
            dTemplate = hDataTemplate;
            Binding hDataTemplateBind = new Binding();
            hDataTemplateBind.Mode = BindingMode.TwoWay;
            hDataTemplateBind.Path = new PropertyPath(children);
            hDataTemplate.ItemsSource = hDataTemplateBind;
            FrameworkElementFactory FEFSP = new FrameworkElementFactory(typeof(StackPanel));
            FEFSP.SetValue(StackPanel.OrientationProperty, Orientation.Horizontal);
            FEFSP.SetValue(StackPanel.VerticalAlignmentProperty, VerticalAlignment.Center);
            FrameworkElementFactory FEFIMAGE = new FrameworkElementFactory(typeof(Image));
            Binding imgBind = new Binding();
            imgBind.Path = new PropertyPath(ImageIco);
            FEFIMAGE.SetValue(Image.SourceProperty, imgBind);
            FEFIMAGE.SetValue(Image.MarginProperty, thickness);
            FrameworkElementFactory FEFTB = new FrameworkElementFactory(typeof(TextBlock));
            FEFSP.AddHandler(TextBlock.MouseRightButtonUpEvent, new MouseButtonEventHandler(DoItemMouseRightButtonDown));

            Binding tbBind = new Binding();
            tbBind.Mode = BindingMode.TwoWay;
            tbBind.Path = new PropertyPath(nodeText);
            FEFTB.SetBinding(TextBlock.TextProperty, tbBind);
            FEFTB.SetValue(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center);
            FEFSP.AppendChild(FEFIMAGE);
            FEFSP.AppendChild(FEFTB);

            hDataTemplate.VisualTree = FEFSP;
            _CTreeView.ItemTemplate = dTemplate;
        }

        /// <summary>
        /// 创建带有复选框的列（只能创建一次）
        /// </summary>
        /// <param name="nodeText">显示的节点值</param>
        /// <param name="children">子节点</param>
        /// <param name="IsChecked">选中状态</param>
        /// <param name="thickness">间距</param>
        /// <param name="CheckClick">复选框点击事件</param>
        public void CreateTreeViewDataBindColumns(string nodeText, string children, string IsChecked, Thickness thickness, RoutedEventHandler CheckClick)
        {
            if (_CTreeView.ItemTemplate != null) return;
            DataTemplate dTemplate = new DataTemplate();
            HierarchicalDataTemplate hDataTemplate = new HierarchicalDataTemplate();
            dTemplate = hDataTemplate;
            Binding hDataTemplateBind = new Binding();
            hDataTemplateBind.Mode = BindingMode.TwoWay;
            hDataTemplateBind.Path = new PropertyPath(children);
            hDataTemplate.ItemsSource = hDataTemplateBind;
            FrameworkElementFactory FEFSP = new FrameworkElementFactory(typeof(StackPanel));
            FEFSP.SetValue(StackPanel.OrientationProperty, Orientation.Horizontal);
            FEFSP.SetValue(StackPanel.VerticalAlignmentProperty, VerticalAlignment.Center);
            FrameworkElementFactory FEFCB = new FrameworkElementFactory(typeof(CheckBox));
            Binding cbBind = new Binding();
            cbBind.Path = new PropertyPath(IsChecked);
            FEFCB.SetValue(CheckBox.IsCheckedProperty, cbBind);
            FEFCB.SetValue(CheckBox.MarginProperty, thickness);
            FEFCB.AddHandler(CheckBox.ClickEvent, new RoutedEventHandler(CheckClick));
            FEFCB.SetValue(CheckBox.VerticalAlignmentProperty, VerticalAlignment.Center);
            FrameworkElementFactory FEFTB = new FrameworkElementFactory(typeof(TextBlock));

            Binding tbBind = new Binding();
            tbBind.Mode = BindingMode.TwoWay;
            tbBind.Path = new PropertyPath(nodeText);
            FEFTB.SetBinding(TextBlock.TextProperty, tbBind);
            FEFTB.SetValue(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center);
            FEFSP.AppendChild(FEFCB);
            FEFSP.AppendChild(FEFTB);
            FEFSP.AddHandler(TextBlock.MouseRightButtonUpEvent, new MouseButtonEventHandler(DoItemMouseRightButtonDown));

            hDataTemplate.VisualTree = FEFSP;
            _CTreeView.ItemTemplate = dTemplate;
        }
        /// <summary>
        /// 创建带有复选框和图片的列（只能创建一次）
        /// </summary>
        /// <param name="nodeText">显示的节点值</param>
        /// <param name="children">子节点</param>
        /// <param name="IsChecked">选中状态</param>
        /// <param name="ImageIco">图片</param>
        /// <param name="thickness">间距</param>
        /// <param name="CheckClick">复选框点击事件</param>
        public void CreateTreeViewDataBindColumns(string nodeText, string children, string IsChecked, string ImageIco, Thickness thickness, RoutedEventHandler CheckClick)
        {
            if (_CTreeView.ItemTemplate != null) return;
            DataTemplate dTemplate = new DataTemplate();
            HierarchicalDataTemplate hDataTemplate = new HierarchicalDataTemplate();
            dTemplate = hDataTemplate;
            Binding hDataTemplateBind = new Binding();
            hDataTemplateBind.Mode = BindingMode.TwoWay;
            hDataTemplateBind.Path = new PropertyPath(children);
            hDataTemplate.ItemsSource = hDataTemplateBind;
            FrameworkElementFactory FEFSP = new FrameworkElementFactory(typeof(StackPanel));
            FEFSP.SetValue(StackPanel.OrientationProperty, Orientation.Horizontal);
            FEFSP.SetValue(StackPanel.VerticalAlignmentProperty, VerticalAlignment.Center);
            FrameworkElementFactory FEFIMAGE = new FrameworkElementFactory(typeof(Image));
            Binding imgBind = new Binding();
            imgBind.Path = new PropertyPath(ImageIco);
            FEFIMAGE.SetValue(Image.SourceProperty, imgBind);
            FEFIMAGE.SetValue(Image.MarginProperty, thickness);
            FrameworkElementFactory FEFCB = new FrameworkElementFactory(typeof(CheckBox));
            Binding cbBind = new Binding();
            cbBind.Path = new PropertyPath(IsChecked);
            FEFCB.SetValue(CheckBox.IsCheckedProperty, cbBind);
            FEFCB.SetValue(CheckBox.MarginProperty, thickness);
            FEFCB.SetValue(CheckBox.VerticalAlignmentProperty, VerticalAlignment.Center);
            FEFCB.AddHandler(CheckBox.ClickEvent, new RoutedEventHandler(CheckClick));
            FrameworkElementFactory FEFTB = new FrameworkElementFactory(typeof(TextBlock));

            Binding tbBind = new Binding();
            tbBind.Mode = BindingMode.TwoWay;
            tbBind.Path = new PropertyPath(nodeText);
            FEFTB.SetBinding(TextBlock.TextProperty, tbBind);
            FEFTB.SetValue(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center);
            FEFSP.AddHandler(TextBlock.MouseRightButtonUpEvent, new MouseButtonEventHandler(DoItemMouseRightButtonDown));
            FEFSP.AppendChild(FEFCB);
            FEFSP.AppendChild(FEFIMAGE);
            FEFSP.AppendChild(FEFTB);
            hDataTemplate.VisualTree = FEFSP;
            _CTreeView.ItemTemplate = dTemplate;
        }
        /// <summary>
        /// 创建树的绑定列（只能创建一次）
        /// </summary>
        /// <param name="NodeText">显示的节点值</param>
        public void CreateTreeViewDataBindColumns(string NodeText)
        {
            if (_CTreeView.ItemTemplate != null) return;
            DataTemplate dTemplate = new DataTemplate();
            HierarchicalDataTemplate hDataTemplate = new HierarchicalDataTemplate();
            dTemplate = hDataTemplate;
            FrameworkElementFactory FEFTB = new FrameworkElementFactory(typeof(TextBlock));
            FEFTB.AddHandler(TextBlock.MouseRightButtonUpEvent, new MouseButtonEventHandler(DoItemMouseRightButtonDown));
            Binding tbBind = new Binding();
            tbBind.Path = new PropertyPath(NodeText);
            FEFTB.SetBinding(TextBlock.TextProperty, tbBind);
            hDataTemplate.VisualTree = FEFTB;
            _CTreeView.ItemTemplate = dTemplate;
        }
        #endregion

        #region 注册右键菜单

        /// <summary>
        ///注册 选中项 右键 事件
        /// </summary>
        /// <param name="header">显示名称</param>
        /// <param name="tag">标识号</param>
        /// <param name="imageIco">图片 例如："/项目名;component/Resources/tick.png" 和"/Resources/tick.png" </param>
        /// <param name="mc">点击事件 TreeDoubleClickHandler(object sender, MouseButtonEventArgs e)  </param>
        public void AddRightContexMenu(string header, object tag, RoutedEventHandler mc)
        {
            MenuItem result = new MenuItem();
            result.Header = header;
            result.Tag = tag;
            result.Margin = new Thickness(2);
            result.Click += new RoutedEventHandler(mc);
            if (_ContextMenuItems == null)
                _ContextMenuItems = new ContextMenu();
            _ContextMenuItems.Items.Add(result);

        }
        /// <summary>
        ///注册 选中项 右键 事件
        /// </summary>
        /// <param name="header">显示名称</param>
        /// <param name="tag">标识号</param>
        /// <param name="imageIco">图片 例如："/项目名;component/Resources/tick.png" 和"/Resources/tick.png" </param>
        /// <param name="mc">点击事件 TreeDoubleClickHandler(object sender, MouseButtonEventArgs e)  </param>
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
            if (_ContextMenuItems == null)
                _ContextMenuItems = new ContextMenu();
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
                    _ContextMenuItems = new ContextMenu();
                _ContextMenuItems.Items.Add(item);
            }
        }

        /// <summary>
        /// 添加右键菜单分隔符
        /// </summary>
        public void AddRightMenuSeparator()
        {
            Separator separator = new Separator();
            if (_ContextMenuItems == null)
                _ContextMenuItems = new ContextMenu();
            _ContextMenuItems.Items.Add(separator);
        }

        #region 设置右键

        void DoItemMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            AddRightButtonToTree();
        }
        void _CTreeView_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            _CTreeView.ContextMenu = null;
        }
        /// <summary>
        /// 增加右键菜单到树
        /// </summary>
        private void AddRightButtonToTree()
        {
            object model = _CTreeView.SelectedItem as object;
            if (model == null) return;
            //右击时
            if (_ContextMenuItems.Items.Count > 0)
                _CTreeView.ContextMenu = _ContextMenuItems;
            else
                _CTreeView.ContextMenu = null;
        }

        /// <summary>
        /// 重置右键菜单初始值
        /// </summary>
        public void ResetContextMenuItems()
        {
            _ContextMenuItems = null;
        }
        #region 右键
        /// <summary>
        /// 设置右键菜单的状态 设置后下次需要使用时，必须设置回来
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
        /// 设置右键菜单的状态 设置后下次需要使用时，必须设置回来
        /// </summary>
        /// <param name="isVisibility">状态（Visible 显示,Hidden 隐藏、预留空间布局中的元素， Collapsed 隐藏、不预留空间布局中的元素）</param>
        /// <param name="values">需要设置的菜单项 的标识号集合    string[] values = new string[] { "tag" }; </param>
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
        #endregion

        public ContextMenu _ContextMenuItems { get; set; }
    }
}
