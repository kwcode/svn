using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Markup;
using System.Xml;

namespace TCode
{
    [TemplatePart(Name="host_part", Type=typeof(StackPanel))]
    public class HLContextMenu : ContextMenu
    {
        static HLContextMenu()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(HLContextMenu), new FrameworkPropertyMetadata(typeof(HLContextMenu)));
        }

        #region Dependency Properties

        public static readonly DependencyProperty HLPopupMenuBackgroundProperty =
            DependencyProperty.Register("HLPopupMenuBackground", typeof(Brush), typeof(HLContextMenu), new FrameworkPropertyMetadata(new SolidColorBrush(Colors.White)));

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

        public static readonly DependencyProperty HLPopupMenuBorderBrushProperty =
            DependencyProperty.Register("HLPopupMenuBorderBrush", typeof(Brush), typeof(HLContextMenu), new FrameworkPropertyMetadata(new SolidColorBrush(Colors.Gray)));

        /// <summary>
        /// 弹出菜单的边框颜色。
        /// </summary>
        public Brush HLPopupMenuBorderBrush
        {
            get
            {
                return GetValue(HLPopupMenuBorderBrushProperty) as Brush;
            }
            set
            {
                SetValue(HLPopupMenuBorderBrushProperty, value);
            }
        }

        #endregion

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }

        protected override Size MeasureOverride(Size constraint)
        {
            return base.MeasureOverride(constraint);
        }

        protected override Size ArrangeOverride(Size arrangeBounds)
        {
            return base.ArrangeOverride(arrangeBounds);
        }
    }
}
