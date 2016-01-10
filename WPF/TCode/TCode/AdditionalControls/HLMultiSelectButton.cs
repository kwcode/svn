using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Input;

namespace TCode
{
    /// <summary>
    /// 多选按钮。
    /// </summary>
    [TemplatePart(Name = "quickLinkList", Type = typeof(HLMenuItem))]
    [TemplatePart(Name = "bdButton", Type = typeof(Border))]
    public class HLMultiSelectButton : Button, IDisposable
    {
        private HLMenuItem _QuickLinkList;
        private Border _bdButton;
        private List<HLMenuItem> _menuitems = new List<HLMenuItem>();
        private bool _hasNewMenuItem = false;

        /// <summary>
        /// 构造函数。初始化样式。
        /// </summary>
        public HLMultiSelectButton()
        {
            DefaultStyleKey = typeof(HLMultiSelectButton);
            //ResourcesHelper.LanguageChanged += new EventHandler<LanaguageChangedEventArgs>(ResourcesHelper_LanguageChanged);
            LayoutUpdated += new EventHandler(HLMultiSelectButton_LayoutUpdated);
            //Loaded += new RoutedEventHandler(HLMultiSelectButton_Loaded);
        }

        //void HLMultiSelectButton_Loaded(object sender, RoutedEventArgs e)
        //{
        //    OnLanguageChanged(ResourcesHelper.CurrentLanguageID);
        //}

        void HLMultiSelectButton_LayoutUpdated(object sender, EventArgs e)
        {
            if (_QuickLinkList == null)
            {
                _QuickLinkList = Template.FindName("quickLinkList", this) as HLMenuItem;
            }

            if (_QuickLinkList != null)
            {
                if (_menuitems.Count <= 0)
                    MultiVisiblity = System.Windows.Visibility.Collapsed;
                else
                    MultiVisiblity = System.Windows.Visibility.Visible;

                if (_hasNewMenuItem)
                {
                    _hasNewMenuItem = false;
                    _QuickLinkList.Items.Clear();
                    foreach (HLMenuItem it in _menuitems)
                    {
                        _QuickLinkList.Items.Add(it);
                    }
                }
            }
        }

        /// <summary>
        /// 生成选中时的背景色。
        /// </summary>
        /// <returns></returns>
        private static Brush GetHotBackGround()
        {
            LinearGradientBrush lbrsh = new LinearGradientBrush();
            lbrsh.StartPoint = new Point(0.5, 0);
            lbrsh.EndPoint = new Point(0.5, 1);

            GradientStop gs = new GradientStop(Colors.White, 0);
            lbrsh.GradientStops.Add(gs);

            gs = new GradientStop(Colors.Orange, 1);
            lbrsh.GradientStops.Add(gs);

            Brush brsh = new SolidColorBrush(Colors.Bisque);

            return brsh;
        }

        #region Public Methods

        /// <summary>
        /// 重载模版函数。
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _QuickLinkList = Template.FindName("quickLinkList", this) as HLMenuItem;
            _bdButton = Template.FindName("bdButton", this) as Border;
        }

        /// <summary>
        /// 添加菜单项。
        /// </summary>
        /// <param name="hlMI"></param>
        public void AddMenuItem(HLMenuItem hlMI)
        {
            if (hlMI != null)
            {
                //if (_QuickLinkList != null)
                //{
                //    _QuickLinkList.Items.Add(hlMI);
                //}
                //else
                //{
                _menuitems.Add(hlMI);
                _hasNewMenuItem = true;
                //}
            }
        }

        /// <summary>
        /// 添加多个菜单项。
        /// </summary>
        /// <param name="hlMIs"></param>
        public void AddMenuItems(List<HLMenuItem> hlMIs)
        {
            if (hlMIs != null && hlMIs.Count > 0)
            {
                //if (_QuickLinkList != null)
                //{
                //    foreach (HLMenuItem it in hlMIs)
                //    {
                //        _QuickLinkList.Items.Add(it);
                //    }
                //}
                //else
                //{
                foreach (HLMenuItem it in hlMIs)
                {
                    _menuitems.Add(it);
                }
                _hasNewMenuItem = true;
                //}
            }
        }

        /// <summary>
        /// 清除菜单项。
        /// </summary>
        public void ClearMenuItems()
        {
            if (_QuickLinkList != null)
            {
                _QuickLinkList.Items.Clear();
            }
            _menuitems.Clear();
            _hasNewMenuItem = true;
        }
        #endregion

        #region 属性。
        /// <summary>
        /// 依赖属性。多选特性是否可见，默认可见。
        /// </summary>
        public static readonly DependencyProperty MultiVisiblityProperty =
            DependencyProperty.Register("MultiVisiblity", typeof(Visibility), typeof(HLMultiSelectButton), new FrameworkPropertyMetadata(Visibility.Visible));

        /// <summary>
        /// 多选特性是否可见，默认可见。
        /// </summary>
        public Visibility MultiVisiblity
        {
            get { return (Visibility)GetValue(MultiVisiblityProperty); }
            set { SetValue(MultiVisiblityProperty, value); }
        }

        /// <summary>
        /// 依赖属性。按钮背景。
        /// </summary>
        public static readonly DependencyProperty HLButtonBackgroundProperty =
            DependencyProperty.Register("HLButtonBackground", typeof(Brush), typeof(HLMultiSelectButton), new FrameworkPropertyMetadata(Brushes.Transparent));
        /// <summary>
        /// 按钮背景。
        /// </summary>
        public Brush HLButtonBackground
        {
            get { return GetValue(HLButtonBackgroundProperty) as Brush; }
            set { SetValue(HLButtonBackgroundProperty, value); }
        }

        /// <summary>
        /// 依赖属性。鼠标悬浮于按钮时的背景。
        /// </summary>
        public static readonly DependencyProperty HLHotButtonBackgroundProperty =
            DependencyProperty.Register("HLHotButtonBackground", typeof(Brush), typeof(HLMultiSelectButton), new FrameworkPropertyMetadata(GetHotBackGround()));

        /// <summary>
        /// 鼠标悬浮于按钮时的背景。
        /// </summary>
        public Brush HLHotButtonBackground
        {
            get
            {
                return GetValue(HLHotButtonBackgroundProperty) as Brush;
            }
            set
            {
                SetValue(HLHotButtonBackgroundProperty, value);
            }
        }

        public static readonly DependencyProperty HLHotBorderBrushProperty =
            DependencyProperty.Register("HLHotBorderBrush", typeof(Brush), typeof(HLMultiSelectButton), new FrameworkPropertyMetadata(Brushes.LightGray));

        /// <summary>
        /// 选中菜单项的背景。
        /// </summary>
        public Brush HLHotBorderBrush
        {
            get
            {
                return GetValue(HLHotBorderBrushProperty) as Brush;
            }
            set
            {
                SetValue(HLHotBorderBrushProperty, value);
            }
        }


        /// <summary>
        /// 按钮的边距。
        /// </summary>
        public static readonly DependencyProperty HLMarginProperty =
            DependencyProperty.Register("HLMargin", typeof(Thickness), typeof(HLMultiSelectButton), new PropertyMetadata(new Thickness(2)));
        /// <summary>
        /// 内容部分与整体的边距。
        /// </summary>
        public Thickness HLMargin
        {
            get
            {
                return (Thickness)GetValue(HLMarginProperty);
            }
            set
            {
                SetValue(HLMarginProperty, value);
            }
        }

        /// <summary>
        /// 依赖属性。菜单项选中背景。
        /// </summary>
        public static readonly DependencyProperty HLMenuItemSelectedBackgroundProperty =
            DependencyProperty.Register("HLMenuItemSelectedBackground", typeof(Brush), typeof(HLMultiSelectButton), new FrameworkPropertyMetadata(GetHotBackGround()));
        /// <summary>
        /// 菜单项选中背景。
        /// </summary>
        public Brush HLMenuItemSelectedBackground
        {
            get { return GetValue(HLMenuItemSelectedBackgroundProperty) as Brush; }
            set { SetValue(HLButtonBackgroundProperty, value); }
        }

        /// <summary>
        /// 依赖属性。菜单项选中边框颜色。
        /// </summary>
        public static readonly DependencyProperty HLMenuItemSelectedBorderBrushProperty =
            DependencyProperty.Register("HLMenuItemSelectedBorderBrush", typeof(Brush), typeof(HLMultiSelectButton), new FrameworkPropertyMetadata(Brushes.RoyalBlue));
        /// <summary>
        /// 菜单项选中边框颜色。
        /// </summary>
        public Brush HLMenuItemSelectedBorderBrush
        {
            get { return GetValue(HLMenuItemSelectedBorderBrushProperty) as Brush; }
            set { SetValue(HLMenuItemSelectedBorderBrushProperty, value); }
        }

        #endregion

        #region 多语言部分。

        /// <summary>
        /// 资源文件名称。
        /// </summary>
        public string ResourceFileName { get; set; }
        /// <summary>
        /// 资源编号。
        /// </summary>
        public string ResourceID { get; set; }

        //private void ResourcesHelper_LanguageChanged(object sender, LanaguageChangedEventArgs e)
        //{
        //    OnLanguageChanged(e.LanguageID);
        //}

        /// <summary>
        /// 多语言改变处理。
        /// </summary>
        /// <param name="langid"></param>
        //protected virtual void OnLanguageChanged(string langid)
        //{
        //    if ((!string.IsNullOrWhiteSpace(ResourceFileName)) && (!string.IsNullOrWhiteSpace(ResourceID)))
        //    {
        //        string tmp = ResourcesHelper.GetResource(ResourceFileName, ResourceID);
        //        if (!string.IsNullOrWhiteSpace(tmp))
        //        {
        //            this.Content = tmp;
        //        }
        //    }
        //}

        private bool _disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            //ResourcesHelper.LanguageChanged -= new EventHandler<LanaguageChangedEventArgs>(ResourcesHelper_LanguageChanged);
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                Dispose(true);
                _disposed = true;
            }
        }

        #endregion
    }
}
