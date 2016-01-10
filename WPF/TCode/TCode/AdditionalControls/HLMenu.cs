using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace TCode
{
    public class HLMenu : System.Windows.Controls.Menu
    {
        static HLMenu()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(HLMenu), new FrameworkPropertyMetadata(typeof(HLMenu)));
        }

        public HLMenu()
        {            
        }

        #region Dependency Properties

        public static readonly DependencyProperty HLPopupMenuBackgroundProperty =
            DependencyProperty.Register("HLPopupMenuBackground", typeof(Brush), typeof(HLMenu), new FrameworkPropertyMetadata(Brushes.Transparent));//

        /// <summary>
        /// 弹出菜单的背景。
        /// </summary>
        public Brush HLPopupMenuBackground
        {
            get
            {
                return GetValue(HLPopupMenuBackgroundProperty) as Brush;
            }
            set
            {
                SetValue(HLPopupMenuBackgroundProperty, value);
            }
        }

        #endregion
    }
}
