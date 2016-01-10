using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace TCode
{
    /// <summary>
    /// 菜单项
    /// </summary>
    [TemplatePart(Name = "CloseMark", Type = typeof(Image))]
    public class HLMenuItem : MenuItem, IDisposable
    {
        private bool _disposed = false;
        private Image _CloseMark;

        /// <summary>
        /// 关闭句柄。
        /// </summary>
        /// <param name="sender"></param>
        public delegate void CloseHandler(object sender);

        /// <summary>
        /// 关闭事件。
        /// </summary>
        public CloseHandler CloseEvent;

        /// <summary>
        /// 菜单点击的处理程序如果未捕获异常，菜单项会通过该委托来处理异常
        /// </summary>
        /// <param name="sender">触发事件的菜单项</param>
        /// <param name="e">发生的异常</param>
        public delegate void DispatchExceptionHandler(object sender, Exception e);

        /// <summary>
        /// 菜单项默认的异常处理句柄，如果菜单项里面没有处理异常的委托，则使用这个默认处理句柄处理异常；
        /// 如果默认处理句柄也未赋值，则会将原异常直接抛出
        /// </summary>
        public static DispatchExceptionHandler DefaultDispatchingException { get; set; }   //统一处理未捕获异常

        static HLMenuItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(HLMenuItem), new FrameworkPropertyMetadata(typeof(HLMenuItem)));
        }

        /// <summary>
        /// 构造函数。
        /// </summary>
        public HLMenuItem()
        {
            //ResourcesHelper.LanguageChanged += new EventHandler<LanaguageChangedEventArgs>(ResourcesHelper_LanguageChanged);
            //Loaded += new RoutedEventHandler(HLMenuItem_Loaded);
        }

        //void HLMenuItem_Loaded(object sender, RoutedEventArgs e)
        //{
        //      OnLanguageChanged(ResourcesHelper.CurrentLanguageID);
        //}

        #region Dependency Properties

        //public static readonly DependencyProperty HLPopupMenuItemBackgroundProperty =
        //    DependencyProperty.Register("HLPopupMenuItemBackground", typeof(Brush), typeof(HLMenuItem), new FrameworkPropertyMetadata(new SolidColorBrush(Colors.White)));//Color.FromRgb(0xdb, 0xe0, 0xe7)

        ///// <summary>
        ///// 弹出菜单的背景。
        ///// </summary>
        //public Brush HLPopupMenuItemBackground
        //{
        //    get
        //    {
        //        return GetValue(HLPopupMenuItemBackgroundProperty) as Brush;
        //    }
        //    set
        //    {
        //        SetValue(HLPopupMenuItemBackgroundProperty, value);
        //    }
        //}

        //public static readonly DependencyProperty HLPopupMenuItemBorderBrushProperty =
        //    DependencyProperty.Register("HLPopupMenuItemBorderBrush", typeof(Brush), typeof(HLMenuItem), new FrameworkPropertyMetadata(new SolidColorBrush(Colors.Gray)));

        ///// <summary>
        ///// 弹出菜单的边框颜色。
        ///// </summary>
        //public Brush HLPopupMenuItemBorderBrush
        //{
        //    get
        //    {
        //        return GetValue(HLPopupMenuItemBorderBrushProperty) as Brush;
        //    }
        //    set
        //    {
        //        SetValue(HLPopupMenuItemBorderBrushProperty, value);
        //    }
        //}

        /// <summary>
        /// 依赖属性。选中菜单项的背景。
        /// </summary>
        public static readonly DependencyProperty HLSelectedMenuItemBackgroundProperty =
            DependencyProperty.Register("HLSelectedMenuItemBackground", typeof(Brush), typeof(HLMenuItem), new FrameworkPropertyMetadata(new SolidColorBrush(Colors.Bisque)));

        /// <summary>
        /// 选中菜单项的背景。
        /// </summary>
        public Brush HLSelectedMenuItemBackground
        {
            get
            {
                return GetValue(HLSelectedMenuItemBackgroundProperty) as Brush;
            }
            set
            {
                SetValue(HLSelectedMenuItemBackgroundProperty, value);
            }
        }

        public static readonly DependencyProperty HLSelectedMenuItemBorderBrushProperty =
            DependencyProperty.Register("HLSelectedMenuItemBorderBrush", typeof(Brush), typeof(HLMenuItem), new FrameworkPropertyMetadata(new SolidColorBrush(Colors.Blue)));

        /// <summary>
        /// 选中菜单项的边框颜色。
        /// </summary>
        public Brush HLSelectedMenuItemBorderBrush
        {
            get
            {
                return GetValue(HLSelectedMenuItemBorderBrushProperty) as Brush;
            }
            set
            {
                SetValue(HLSelectedMenuItemBorderBrushProperty, value);
            }
        }

        /// <summary>
        /// 依赖属性。可关闭。
        /// </summary>
        public static readonly DependencyProperty IsClosableProperty =
            DependencyProperty.Register("IsClosable", typeof(bool), typeof(HLMenuItem), new FrameworkPropertyMetadata(false));

        /// <summary>
        /// 可关闭菜单。
        /// </summary>
        public bool IsClosable
        {
            get
            {
                return (bool)GetValue(IsClosableProperty);
            }
            set
            {
                SetValue(IsClosableProperty, value);
            }
        }

        #endregion

        #region 保护和私有方法。

        //private void ResourcesHelper_LanguageChanged(object sender, LanaguageChangedEventArgs e)
        //{
        //    OnLanguageChanged(e.LanguageID);
        //}

        //protected virtual void OnLanguageChanged(string langid)
        //{
        //    if ((!string.IsNullOrEmpty(ResourceFileName)) && (!string.IsNullOrEmpty(ResourceID)))
        //    {
        //        string tmp = ResourcesHelper.GetResource(ResourceFileName, ResourceID);
        //        if (!string.IsNullOrEmpty(tmp))
        //        {
        //            if (Header is string)
        //                Header = tmp;
        //        }
        //    }
        //}

        protected virtual void Dispose(bool disposing)
        {
            //ResourcesHelper.LanguageChanged -= new EventHandler<LanaguageChangedEventArgs>(ResourcesHelper_LanguageChanged);
        }

        protected override void OnClick()
        {
            try
            {
                base.OnClick();
            }
            catch (Exception ex)
            {
                if (ExceptionDispatcher != null) ExceptionDispatcher(this, ex);
                else if (DefaultDispatchingException != null) DefaultDispatchingException(this, ex);
                else throw ex;
            }
        }
        #endregion

        #region 公有方法。

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _CloseMark = Template.FindName("CloseMark", this) as Image;
            if (_CloseMark != null)
            {
                _CloseMark.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(_CloseMark_MouseLeftButtonDown);
            }
        }

        void _CloseMark_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (CloseEvent != null) CloseEvent(this);

            if (Parent is Menu)
            {
                (Parent as Menu).Items.Remove(this);
            }
            else if (Parent is MenuItem)
            {
                (Parent as MenuItem).Items.Remove(this);
            }
        }

        /// <summary>
        /// 卸载资源。
        /// </summary>
        public void Dispose()
        {
            if (!_disposed)
            {
                Dispose(true);
                _disposed = true;
            }
        }
        #endregion

        #region 属性。
        /// <summary>
        /// 资源文件名
        /// </summary>
        public string ResourceFileName { get; set; }
        /// <summary>
        /// 资源文件号
        /// </summary>
        public string ResourceID { get; set; }
        /// <summary>
        /// 菜单点击后的处理程序中如果没有处理异常，则使用本委托处理异常
        /// 如果本委托未赋值，则使用菜单项默认的异常处理委托
        /// 如果本委托与菜单项的默认异常处理委托都没有赋值，则将异常原样抛出
        /// </summary>
        public DispatchExceptionHandler ExceptionDispatcher { get; set; }

        #endregion
    }
}
