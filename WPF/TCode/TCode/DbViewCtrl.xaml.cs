using DBCommon;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TCode
{
    /// <summary>
    /// DbViewCtrl.xaml 的交互逻辑
    /// </summary>
    public partial class DbViewCtrl : UserControl
    {
        public DbViewCtrl()
        {
            InitializeComponent();
            LoadTree();
        }
        public ObservableCollection<TreeItem> tree = new ObservableCollection<TreeItem>();
        DataClass dc = new DataClass();
        private void LoadTree()
        {

            //ObservableCollection<TreeItem> treelist = new ObservableCollection<TreeItem>();
            //string sqlstr = "SELECT Name FROM PoliceEduDB..SysObjects Where XType='U' ORDER BY Name";
            //dc.OpenConn(true);
            //DataTable dt = dc.GetDataTable(sqlstr);
            //foreach (DataRow item in dt.Rows)
            //{
            //    treelist.Add(new TreeItem() { NodeText = item["Name"].ToString() });
            //}
            tree.Add(new TreeItem() { NodeText = "服务器", NodeTag = "0", IsExpanded = true });

            tree_data.ItemsSource = tree;

        }

        private void cb_checkOne_Click(object sender, RoutedEventArgs e)
        {

        }
        private void AddTree(string sqlCon, string dbName)
        {
            ObservableCollection<TreeItem> treelist = new ObservableCollection<TreeItem>();

            dc.ConnectionString = sqlCon;
            dc.OpenConn(true);
            string sqlstr = "SELECT Name FROM " + dbName + "..SysObjects Where XType='U' ORDER BY Name";
            DataTable dt = dc.GetDataTable(sqlstr);
            foreach (DataRow item in dt.Rows)
            {
                treelist.Add(new TreeItem() { NodeText = item["Name"].ToString() });
            }
            tree.Add(new TreeItem() { NodeText = dbName, Children = treelist });
        }
        private void addDb_Click(object sender, RoutedEventArgs e)
        {
            LoginSQL win = new LoginSQL();
            bool? b = win.ShowDialog();
            if (b.Value)
            {
                AddTree(win.SqlConnectionString, win.SqlDbName);

            }
        }
    }
}
