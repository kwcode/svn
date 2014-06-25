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

using System.IO;
using System.Collections.ObjectModel;
using System.Collections;
namespace WPF.CustomControl
{
    /// <summary>
    /// 2023-7-23
    /// 树控件， 呈现数据
    ///  右键事件 隐藏复选框  隐藏和呈现右键状态 
    /// </summary>
    public partial class TreeViwDataCtrl : UserControl
    {
        public ContextMenu _ContextMenuItems;//右键菜单
        public TreeViwDataCtrl()
        {
            InitializeComponent();
            RegistrationEvent();
        }

        private void RegistrationEvent()
        {
             
        }

        #region 属性
        /// <summary>
        /// 树对象
        /// </summary>
        public TreeView TreeViewData { get { return tree_DataView; } }
        /// <summary>
        /// 当前选中项
        /// </summary>
        public object SelectedItem { get { return tree_DataView.SelectedItem; } }
        /// <summary>
        /// 整颗树 数据源 （ObservableCollection）
        /// </summary>
        public IEnumerable ItemsSource { get { return tree_DataView.ItemsSource; } }

        #endregion

        #region 开放的初始方法
        /// <summary>
        /// 绑定数据源
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="datasouce">数据源</param>
        public void BindDataToTree<T>(ObservableCollection<T> datasouce)
        {
            tree_DataView.SelectedItemChanged -= new RoutedPropertyChangedEventHandler<object>(tree_DataView_SelectedItemChanged);
            tree_DataView.ItemsSource = datasouce;
            tree_DataView.SelectedItemChanged += new RoutedPropertyChangedEventHandler<object>(tree_DataView_SelectedItemChanged);
        }

        #region 重载创建绑定列
        /// <summary>
        /// 创建树的绑定列（只能创建一次）
        /// </summary>
        /// <param name="nodeText">显示的节点值</param>
        /// <param name="children">子节点</param>
        public void CreateTreeViewDataBindColumns(string nodeText, string children)
        {
            tree_DataView.CreateTreeViewDataBindColumns(nodeText, children);
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
            tree_DataView.CreateTreeViewDataBindColumns(nodeText, children, ImageIco, thickness);
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
            tree_DataView.CreateTreeViewDataBindColumns(nodeText, children, IsChecked, thickness, CheckClick);
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
            tree_DataView.CreateTreeViewDataBindColumns( nodeText,  children,  IsChecked,  ImageIco,  thickness,  CheckClick);
        }
        /// <summary>
        /// 创建树的绑定列（只能创建一次）
        /// </summary>
        /// <param name="NodeText">显示的节点值</param>
        public void CreateTreeViewDataBindColumns(string NodeText)
        {
            tree_DataView.CreateTreeViewDataBindColumns(NodeText);
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
            tree_DataView.AddRightContexMenu(header, tag, mc);
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
            tree_DataView.AddRightContexMenu(header, tag, imageIco, mc);

        }
        /// <summary>
        /// 注册 选中项 右键 事件
        /// </summary>
        /// <param name="values"></param>
        public void AddRightContexMenu(params MenuItem[] values)
        {
            tree_DataView.AddRightContexMenu(values);
        }

        /// <summary>
        /// 添加右键菜单分隔符
        /// </summary>
        public void AddRightMenuSeparator()
        {
            tree_DataView.AddRightMenuSeparator();
        }

        #region 设置右键
        /// <summary>
        /// 重置右键菜单初始值
        /// </summary>
        public void ResetContextMenuItems()
        {
            tree_DataView.ResetContextMenuItems();
        }
        #region 右键
        /// <summary>
        /// 设置右键菜单的状态 设置后下次需要使用时，必须设置回来
        /// </summary>
        /// <param name="isEnabled">状态（false 不可用， true 可用）</param>
        /// <param name="values">需要设置的菜单项 的标识号集合    string[] values = new string[] { "tag" }; </param>
        public void SetRightButtonStatus(bool isEnabled, params object[] values)
        {
            tree_DataView.SetRightButtonStatus(isEnabled, values);

        }

        /// <summary>
        /// 设置右键菜单的状态 设置后下次需要使用时，必须设置回来
        /// </summary>
        /// <param name="isVisibility">状态（Visible 显示,Hidden 隐藏、预留空间布局中的元素， Collapsed 隐藏、不预留空间布局中的元素）</param>
        /// <param name="values">需要设置的菜单项 的标识号集合    string[] values = new string[] { "tag" }; </param>
        /// TODO:用法： string[] values = new string[] { "tag1", "tag2", "tag3", "tag4" };  SetRightButtonClick(Visibility.Collapsed, values);
        public void SetRightButtonStatus(Visibility isVisibility, params object[] values)
        {
            tree_DataView.SetRightButtonStatus(isVisibility, values);
        }
        #endregion

        #endregion
        #endregion

        #endregion

        #region 私有方法
         
        protected void HandleDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.RightButton == MouseButtonState.Pressed) return;
            if (DoubleHandleClick != null)
                DoubleHandleClick(sender, e);
        }
        void tree_DataView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (DoSelectedItemChanged != null)
                DoSelectedItemChanged(sender, e);
        }
        #endregion

        #region 事件
        /// <summary>
        /// listview 双击左键事件
        /// </summary>
        public RoutedEventHandler DoubleHandleClick { get; set; }
        public RoutedPropertyChangedEventHandler<object> DoSelectedItemChanged { get; set; }

        #endregion
    }

}
