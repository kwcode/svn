using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace T.WPF.TControls
{

    [TemplatePart(Name = "bdButton", Type = typeof(Border))]
    [TemplatePart(Name = "TimgIcon", Type = typeof(Image))]
    public class TImageButton : Button
    {
        public TImageButton()
            : base()
        {
            DefaultStyleKey = typeof(TImageButton);
        }
        Image _imgIcon;
        Border _border;
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _imgIcon = Template.FindName("TimgIcon", this) as Image;
            _border = Template.FindName("bdButton", this) as Border;
        }
        /// <summary>
        /// 依赖属性。图形的源。
        /// </summary>
        public static DependencyProperty HLInformationIconSourceProperty =
            DependencyProperty.Register("HLInformationIconSource", typeof(ImageSource), typeof(TImageButton), null);
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

        public Brush TButtonBackground
        {
            get
            {
                return GetValue(TButtonBackgroundProperty) as Brush;
            }
            set
            {
                try
                {
                    SetValue(TButtonBackgroundProperty, value);
                }
                catch { }
            }
        }
        /// <summary>
        /// 依赖属性。鼠标悬浮于按钮时的背景。
        /// </summary>
        public static DependencyProperty TButtonBackgroundProperty =
            DependencyProperty.Register("TButtonBackground", typeof(Brush), typeof(TImageButton), new FrameworkPropertyMetadata(Brushes.Transparent));


        public Brush THotButtonBackground
        {
            get
            {
                return GetValue(THotButtonBackgroundProperty) as Brush;
            }
            set
            {
                try
                {
                    SetValue(THotButtonBackgroundProperty, value);
                }
                catch { }
            }
        }
        /// <summary>
        /// 依赖属性。鼠标悬浮于按钮时的背景。
        /// </summary>
        public static DependencyProperty THotButtonBackgroundProperty =
            DependencyProperty.Register("THotButtonBackground", typeof(Brush), typeof(TImageButton), new FrameworkPropertyMetadata(GetHotBackGround()));

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


        //public double TButtonWidth
        //{
        //    get
        //    {
        //        return Convert.ToDouble(GetValue(TButtonWidthProperty));
        //    }
        //    set
        //    {
        //        try
        //        {
        //            SetValue(TButtonWidthProperty, value);

        //        }
        //        catch { }
        //    }
        //}

        //public static DependencyProperty TButtonWidthProperty =
        //    DependencyProperty.Register("TButtonWidth", typeof(double), typeof(Image), null);
        //public double TButtonHeight
        //{
        //    get
        //    {
        //        return Convert.ToDouble(GetValue(TButtonHeightProperty));
        //    }
        //    set
        //    {
        //        try
        //        {
        //            SetValue(TButtonHeightProperty, value);

        //        }
        //        catch { }
        //    }
        //}

        //public static DependencyProperty TButtonHeightProperty =
        //    DependencyProperty.Register("TButtonHeight", typeof(double), typeof(Image), null);

        public double TButtonWidth
        {
            get { return (double)GetValue(TButtonWidthProperty); }
            set { SetValue(TButtonWidthProperty, value); }
        }
         
        public static readonly DependencyProperty TButtonWidthProperty =
            DependencyProperty.Register("TButtonWidth", typeof(double), typeof(TImageButton), null);
    }
}
