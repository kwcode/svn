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
using System.Data;

namespace XMLContrast
{
    /// <summary>
    /// Window2.xaml 的交互逻辑
    /// </summary>
    public partial class Window2 : Window
    {
        public Window2()
        {
            InitializeComponent();

            DataTable dt = new DataTable();
            dt.Columns.Add("Name");
            dt.Columns.Add("Foreground");
            dt.Columns.Add("姓名");
            DataRow dr = dt.NewRow();
            dr["Name"] = "唐开文";
            dr["Foreground"] = "#08246B";
            dr["姓名"] = "唐开文";
            //dr["Name"] = "唐开文";
            //dr["Name"] = "唐开文";
            dt.Rows.Add(dr);
            dr = dt.NewRow();
            dr["Name"] = "仙仙";
            dr["Foreground"] = "#08246B";
            //dr["Name"] = "唐开文";
            //dr["Name"] = "唐开文";
            dt.Rows.Add(dr);
            dr = dt.NewRow();
            dr["Name"] = "唐晨";
            dr["Foreground"] = "#08246B";
            //dr["Name"] = "唐开文";
            //dr["Name"] = "唐开文";
            dt.Rows.Add(dr);
            //List<DataItem> data = new List<DataItem>();
            //data.Add(new DataItem() { Name = "唐开文" });
            //lv_Data.ItemsSource = data;

            dg.ItemsSource = dt.DefaultView;
            //dataGrid.ItemsSource = dt.DefaultView;
        }

        private void dg_LoadingRow(object sender, DataGridRowEventArgs e)
        {

        }
    }
}
