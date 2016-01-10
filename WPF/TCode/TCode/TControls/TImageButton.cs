using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace T.WPF.TControls
{
    [TemplatePart(Name = "TimgIcon", Type = typeof(Image))]
    public class TImageButton : Button
    {
        public TImageButton()
            : base()
        {
            DefaultStyleKey = typeof(TImageButton);
        }
        Image _imgIcon;
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _imgIcon = Template.FindName("TimgIcon", this) as Image;
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
    }
}
