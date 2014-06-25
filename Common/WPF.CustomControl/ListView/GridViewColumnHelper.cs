using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Windows.Controls;
using System.Windows;

namespace WPF.CustomControl
{
    internal static class GridViewColumnHelper
    {
        private static PropertyInfo DesiredWidthProperty =
            typeof(GridViewColumn).GetProperty("DesiredWidth", BindingFlags.NonPublic | BindingFlags.Instance);

        public static double GetColumnWidth(this GridViewColumn column)
        {
            return (double.IsNaN(column.Width)) ? (double)DesiredWidthProperty.GetValue(column, null) : column.Width;
        }
    }
    /// <summary>
    /// 固定宽度的列
    /// </summary>
    public class xFixedWidthGridViewColumn : GridViewColumn
    {
        static xFixedWidthGridViewColumn()
        {
            WidthProperty.OverrideMetadata(typeof(xFixedWidthGridViewColumn),
                new FrameworkPropertyMetadata(null, new CoerceValueCallback(OnCoerceWidth)));
        }
        private static object OnCoerceWidth(DependencyObject o, object baseValue)
        {
            xFixedWidthGridViewColumn fwc = o as xFixedWidthGridViewColumn;
            if (fwc != null)
                return fwc.FixedWidth;
            return 0.0;
        }
        public double FixedWidth
        {
            get { return (double)GetValue(FixedWidthProperty); }
            set { SetValue(FixedWidthProperty, value); }
        }



        public static readonly DependencyProperty FixedWidthProperty =
          DependencyProperty.Register("FixedWidth", typeof(double), typeof(xFixedWidthGridViewColumn),
          new FrameworkPropertyMetadata(double.NaN, new PropertyChangedCallback(OnFixedWidthChanged)));

        private static void OnFixedWidthChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            xFixedWidthGridViewColumn fwc = o as xFixedWidthGridViewColumn;
            if (fwc != null)
                fwc.CoerceValue(WidthProperty);
        }
    }
}
