using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Input;
using System.Windows.Data;

namespace TCode
{
    /// <summary>
    /// 支持图形和文本共同显示的按钮。
    /// </summary> 
    [TemplatePart(Name = "imgIcon", Type = typeof(Image))]
    [TemplatePart(Name = "btButton", Type = typeof(HLMultiSelectButton))]
    public class HLImageButton : Button
    {
        Image _imgIcon;
        HLMultiSelectButton _btButton;

        /// <summary>
        /// 构造函数。
        /// </summary>
        public HLImageButton()
            : base()
        {
            DefaultStyleKey = typeof(HLImageButton);
            //Loaded += new RoutedEventHandler(HLImageButton_Loaded);
            LayoutUpdated += new EventHandler(HLImageButton_LayoutUpdated);
            //ResourcesHelper.LanguageChanged += new EventHandler<LanaguageChangedEventArgs>(ResourcesHelper_LanguageChanged);
        }

        //void HLImageButton_Loaded(object sender, RoutedEventArgs e)
        //{
        //    OnLanguageChanged(ResourcesHelper.CurrentLanguageID);
        //}

        void HLImageButton_LayoutUpdated(object sender, EventArgs e)
        {
            if (_imgIcon != null)
            {
                if (HLOrientation == Orientation.Horizontal)
                {
                    _imgIcon.Margin = new Thickness(0, 0, 2, 0);
                }
                else
                {
                    _imgIcon.Margin = new Thickness(0, 0, 0, 2);
                }
            }
            else
            {
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
 

        /// <summary>
        /// 重载模版函数。
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _imgIcon = Template.FindName("imgIcon", this) as Image;
            _btButton = Template.FindName("btButton", this) as HLMultiSelectButton;
            if (_btButton != null)
            {
                _btButton.SetBinding(HLMultiSelectButton.HLButtonBackgroundProperty, new Binding(HLButtonBackgroundProperty.Name) { Source = this });
                _btButton.SetBinding(HLMultiSelectButton.HLHotButtonBackgroundProperty, new Binding(HLHotButtonBackgroundProperty.Name) { Source = this });
                _btButton.SetBinding(HLMultiSelectButton.HLMarginProperty, new Binding(HLMarginProperty.Name) { Source = this });
                _btButton.SetBinding(HLMultiSelectButton.HLMenuItemSelectedBackgroundProperty, new Binding(HLMenuItemSelectedBackgroundProperty.Name) { Source = this });
                _btButton.SetBinding(HLMultiSelectButton.HLMenuItemSelectedBorderBrushProperty, new Binding(HLMenuItemSelectedBorderBrushProperty.Name) { Source = this });

            }
        }

        /// <summary>
        /// 添加菜单项。
        /// </summary>
        /// <param name="hlMI"></param>
        public void AddMenuItem(HLMenuItem hlMI)
        {
            if (_btButton != null && hlMI != null)
            {
                _btButton.AddMenuItem(hlMI);
            }
        }

        #region 依赖属性。

        /// <summary>
        /// 图形按钮里面的多选按钮。
        /// </summary>
        public HLMultiSelectButton MultiSelectButton
        {
            get { return _btButton; }
        }

        /// <summary>
        /// 依赖属性。图形的源。
        /// </summary>
        public static DependencyProperty HLInformationIconSourceProperty =
            DependencyProperty.Register("HLInformationIconSource", typeof(ImageSource), typeof(HLImageButton), null);
        /// <summary>
        /// 图形的源。
        /// </summary>
        public ImageSource HLInformationIconSource
        {
            get
            {
                return GetValue(HLInformationIconSourceProperty) as ImageSource;
            }
            set
            {
                try
                {
                    SetValue(HLInformationIconSourceProperty, value);
                }
                catch { }
            }
        }

        /// <summary>
        /// 依赖属性。图形和文本的摆放方式。
        /// </summary>
        public static DependencyProperty HLOrientationProperty =
            DependencyProperty.Register("HLOrientation", typeof(Orientation), typeof(HLImageButton), new PropertyMetadata(Orientation.Horizontal));
        /// <summary>
        /// 图形和文本的摆放方式。
        /// </summary>
        public Orientation HLOrientation
        {
            get { return (Orientation)GetValue(HLOrientationProperty); }
            set { SetValue(HLOrientationProperty, value); }
        }

        /// <summary>
        /// 依赖属性。按钮背景。
        /// </summary>
        public static readonly DependencyProperty HLButtonBackgroundProperty =
            DependencyProperty.Register("HLButtonBackground", typeof(Brush), typeof(HLImageButton), new FrameworkPropertyMetadata(Brushes.Transparent));
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
            DependencyProperty.Register("HLHotButtonBackground", typeof(Brush), typeof(HLImageButton), new FrameworkPropertyMetadata(GetHotBackGround()));

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

        /// <summary>
        /// 按钮的边距。
        /// </summary>
        public static readonly DependencyProperty HLMarginProperty =
            DependencyProperty.Register("HLMargin", typeof(Thickness), typeof(HLImageButton), new PropertyMetadata(new Thickness(2)));
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
            DependencyProperty.Register("HLMenuItemSelectedBackground", typeof(Brush), typeof(HLImageButton), new FrameworkPropertyMetadata(Brushes.SkyBlue));
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
            DependencyProperty.Register("HLMenuItemSelectedBorderBrush", typeof(Brush), typeof(HLImageButton), new FrameworkPropertyMetadata(GetHotBackGround()));
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
