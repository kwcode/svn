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
using System.Windows.Shapes;
using System.ComponentModel;
using System.Data;
using System.Globalization;

namespace XMLContrast
{
    /// <summary>
    /// Window1.xaml 的交互逻辑
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
            Button_Click();
        }

        private void Button_Click()
        {
            //DataTable dt = new DataTable();
            //dt.Columns.Add("Name");
            //dt.Columns.Add("Foreground");
            //dt.Columns.Add("ItemIcon");
            //dt.Columns.Add("姓名");
            //DataRow dr = dt.NewRow();
            //dr["Name"] = "唐开文";
            //dr["Foreground"] = "#08246B";
            //dr["姓名"] = "唐开文";
            //dt.Rows.Add(dr);
            //dr = dt.NewRow();
            //dr["Name"] = "仙仙";
            //dr["Foreground"] = "#08246B";
            //dt.Rows.Add(dr);
            //dr = dt.NewRow();
            //dr["Name"] = "唐晨";
            //dr["Foreground"] = "#08246B";
            //dr["ItemIcon"] = "/XMLContrast;component/Resources/gc.ico";

            // dt.Rows.Add(dr);
            //List<DataItem> data = new List<DataItem>();
            //data.Add(new DataItem() { Name = "唐开文" });
            //lv_Data.ItemsSource = data;

            //lv_Data.ItemsSource = dt.DefaultView;
            //ListCollectionView dataList = new ListCollectionView(list);
            //dataList.GroupDescriptions.Add(new PropertyGroupDescription("GroupName"));
            List<FeesRates> _datalist = new List<FeesRates>();
            _datalist.Add(new FeesRates()
            {
                ItemIcon = "/XMLContrast;component/Resources/gc.ico",
                GroupName = "唐开文",
                TProjectName = "ttt",
            });
            _datalist.Add(new FeesRates()
            {
                ItemIcon = "/XMLContrast;component/Resources/gc.ico",
                GroupName = "AAA",
                TProjectName = "123124",
            });
            _datalist.Add(new FeesRates()
            {
                ItemIcon = "/XMLContrast;component/Resources/gc.ico",
                GroupName = "AAA",
                TProjectName = "ABXCFD",
            });
            ListCollectionView dataList = new ListCollectionView(_datalist);
            dataList.GroupDescriptions.Add(new PropertyGroupDescription("GroupName"));
            lv_DataView.ItemsSource = dataList;
        }

        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            //DataRowView dr = lv_Data.Items[0] as DataRowView;
            //var row = dataGrid.ItemContainerGenerator.ContainerFromItem(dataGrid.Items[0]) as DataGridRow;
            //row.Background = new SolidColorBrush(Colors.Red);

            //dr = lv_Data.Items[1] as DataRowView;
            //row = dataGrid.ItemContainerGenerator.ContainerFromItem(dataGrid.Items[1]) as DataGridRow;
            //row.Foreground = Brushes.Blue;
            ////  row.Background = new SolidColorBrush(Colors.Red);

            //(dataGrid.Columns[1].GetCellContent(dataGrid.Items[2]) as TextBlock).Foreground = Brushes.Blue;
            ////(dataGrid.Columns[2].GetCellContent(dataGrid.Items[2]) as TextBlock).Background = Brushes.Blue;
            //TextBlock tblock = (dataGrid.Columns[2].GetCellContent(dataGrid.Items[2]) as TextBlock);
            //tblock.Background = Brushes.Blue;

            ////DataGridTextColumn dcolum = dataGrid.Columns[0] as DataGridTextColumn;

            //dcolum.Foreground = Brushes.Red;

        }

        private void dg_LoadingRow(object sender, DataGridRowEventArgs e)
        {

        }
    }
    [ValueConversion(typeof(object), typeof(int))]

    public class NumberToPolarValueConverter : IValueConverter
    {

        public object Convert(

        object value, Type targetType,

        object parameter, CultureInfo culture)
        {

            double number = (double)System.Convert.ChangeType(value, typeof(double));



            if (number < 0.0)

                return -1;



            if (number == 0.0)

                return 0;



            return +1;

        }



        public object ConvertBack(

        object value, Type targetType,

        object parameter, CultureInfo culture)
        {

            throw new NotSupportedException("ConvertBack not supported");

        }

    }

}
