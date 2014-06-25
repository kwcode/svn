using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;

namespace WPF.CustomControl
{
    public class ChinaDatePicker : DatePicker
    {

        static ChinaDatePicker()
        {

        }

        protected override void OnSelectedDateChanged(SelectionChangedEventArgs e)
        {
            base.OnSelectedDateChanged(e);
            DateTime? date = this.SelectedDate;
            string dateStr = date.Value.Year.ToString() + "年" + date.Value.Month.ToString() + "月" + date.Value.Day.ToString() + "日";
            this.Text = dateStr;

        }

        #region Text
        public static readonly DependencyProperty ChinaDateTextProperty = DependencyProperty.Register("ChinaDateText", typeof(string), typeof(ChinaDatePicker),
            new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnChinaDatePickerTextChanged)));
        private static void OnChinaDatePickerTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ChinaDatePicker)d).OnChinaDatePickerTextChanged(e);
            d.SetCurrentValue(ChinaDateTextProperty, e.NewValue.ToString());
        }
        private void OnChinaDatePickerTextChanged(DependencyPropertyChangedEventArgs e)
        {
            this.Text = e.NewValue.ToString();

        }
        public string ChinaDateText
        {
            get { return (string)GetValue(ChinaDateTextProperty); }
            set { SetValue(ChinaDateTextProperty, value); }
        }
        #endregion


        private static object OnCoerceText(DependencyObject o, object baseValue)
        {
            ChinaDatePicker cdp = o as ChinaDatePicker;
            return "";
        }
    }
}
