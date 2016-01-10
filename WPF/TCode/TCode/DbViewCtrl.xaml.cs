using DBCommon;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
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
            //tree.Add(new TreeItem() { NodeText = "服务器", NodeTag = "0", IsExpanded = true }); 
            tree_data.ItemsSource = tree;

        }

        private void cb_checkOne_Click(object sender, RoutedEventArgs e)
        {

        }
        private void AddTree(string sqlCon, string service, bool isAll, string dbName)
        {
            ObservableCollection<TreeItem> treelist = new ObservableCollection<TreeItem>();
            tree.Add(new TreeItem() { NodeText = service, Children = treelist, IsExpanded = true, NodeType = NodeType.服务器, NodeTag = service, SqlConnectionString = sqlCon });
            string sqlString = "";

            dc.ConnectionString = sqlCon;
            dc.OpenConn(true);
            if (isAll)//加载全部数据库
            {
                sqlString = "select name from sysdatabases order by name";
                DataTable dt = dc.GetDataTable(sqlString);
                dc.CloseConn();
                foreach (DataRow item in dt.Rows)
                {
                    treelist.Add(new TreeItem() { NodeText = item["name"].ToString(), NodeType = NodeType.数据库 });
                }
            }
            else
            {
                string sqlstr = "SELECT Name FROM " + dbName + "..SysObjects Where XType='U' ORDER BY Name";
                DataTable dt = dc.GetDataTable(sqlstr);
                dc.CloseConn();
                foreach (DataRow item in dt.Rows)
                {
                    treelist.Add(new TreeItem() { NodeText = item["Name"].ToString() });
                }

            }
        }

        private void btn_OpenDb_Click(object sender, RoutedEventArgs e)
        {
            LoginSQL win = new LoginSQL();
            bool? b = win.ShowDialog();
            if (b.Value)
            {
                AddTree(win.SqlConnectInfo.ServiceName, win.SqlServiceName, win.SqlDbInfo.IsAll, win.SqlDbInfo.Name);
            }
        }

        private void btn_CloseDb_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
