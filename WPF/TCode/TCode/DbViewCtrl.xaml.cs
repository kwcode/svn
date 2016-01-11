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
        private void AddTree(SqlConnectResult sqlConInfo)
        {
            string dbName = sqlConInfo.DbName;
            string service = sqlConInfo.ServiceName;
            bool isAll = sqlConInfo.IsAll;
            string sqlCon = "Integrated Security=SSPI;Data Source=" + sqlConInfo.ServiceName + ";Initial Catalog=" + dbName;
            if (sqlConInfo.IsSqlService)
            {
                sqlCon = "user id=" + sqlConInfo.UserID + ";password=" + sqlConInfo.Password + ";initial catalog=" + dbName + ";data source=" + sqlConInfo.ServiceName;
            }

            ObservableCollection<TreeItem> treelist = new ObservableCollection<TreeItem>();
            string sqlString = "";

            dc.ConnectionString = sqlCon;
            dc.OpenConn(true);
            if (isAll)//加载全部数据库
            {
                tree.Add(new TreeItem() { NodeText = service + "(" + (sqlConInfo.IsAll == true ? sqlConInfo.DbName : "全部") + "-" + sqlConInfo.UserID + ")", Children = treelist, IsExpanded = true, NodeType = NodeType.服务器, NodeTag = service, SqlConnectResult = sqlConInfo });
                sqlString = "select name from sysdatabases order by name";
                DataTable dt = dc.GetDataTable(sqlString);
                dc.CloseConn();
                foreach (DataRow item in dt.Rows)
                {
                    ObservableCollection<TreeItem> tableChildrenTree = new ObservableCollection<TreeItem>();
                    treelist.Add(new TreeItem() { NodeText = item["name"].ToString(), NodeType = NodeType.数据库, SqlConnectResult = sqlConInfo, Children = tableChildrenTree });
                    string sqlChildCon = "Integrated Security=SSPI;Data Source=" + sqlConInfo.ServiceName + ";Initial Catalog=" + item["name"].ToString();

                    if (sqlConInfo.IsSqlService)
                    {
                        sqlChildCon = "user id=" + sqlConInfo.UserID + ";password=" + sqlConInfo.Password + ";initial catalog=" + item["name"].ToString() + ";data source=" + sqlConInfo.ServiceName;
                    }
                    //  DoTheadAddTable(tableChildrenTree, sqlChildCon, dbName);
                    new Thread(o => { DoTheadAddTable(tableChildrenTree, sqlChildCon, dbName, sqlConInfo); }).Start();
                }
            }
            else
            {
                tree.Add(new TreeItem() { NodeText = service + "(" + dbName + "-" + sqlConInfo.UserID + ")", Children = treelist, IsExpanded = true, NodeType = NodeType.数据库, NodeTag = service, SqlConnectResult = sqlConInfo });

                ObservableCollection<TreeItem> tableChildrenTree = new ObservableCollection<TreeItem>();
                treelist.Add(new TreeItem() { NodeText = dbName, NodeType = NodeType.数据库, SqlConnectResult = sqlConInfo, Children = tableChildrenTree });
                string sqlChildCon = "Integrated Security=SSPI;Data Source=" + sqlConInfo.ServiceName + ";Initial Catalog=" + dbName;
                if (sqlConInfo.IsSqlService)
                {
                    sqlChildCon = "user id=" + sqlConInfo.UserID + ";password=" + sqlConInfo.Password + ";initial catalog=" + dbName + ";data source=" + sqlConInfo.ServiceName;
                }
                new Thread(o => { DoTheadAddTable(tableChildrenTree, sqlChildCon, dbName, sqlConInfo); }).Start();
                //DoTheadAddTable(tableChildrenTree, sqlChildCon, dbName);
            }
        }
        /// <summary>
        /// 线程增加表名
        /// </summary>
        public void DoTheadAddTable(ObservableCollection<TreeItem> tableChildrenTree, string sqlCon, string dbName, SqlConnectResult sqlConInfo)
        {
            dc.ConnectionString = sqlCon;
            dc.OpenConn(true);
            string sqlstr = "SELECT Name FROM " + dbName + "..SysObjects Where XType='U' ORDER BY Name";
            DataTable dt = dc.GetDataTable(sqlstr);
            dc.CloseConn();

            //tableChildrenTree.Add(new TreeItem() { NodeText = item["Name"].ToString() });
            Global.SysContext.Send(o =>
            {
                foreach (DataRow item in dt.Rows)
                {
                    tableChildrenTree.Add(new TreeItem() { NodeText = item["Name"].ToString(), NodeType = NodeType.用户表, SqlConnectResult = sqlConInfo });
                }
            }, null);


        }

        private void btn_OpenDb_Click(object sender, RoutedEventArgs e)
        {
            LoginSQL win = new LoginSQL();
            bool? b = win.ShowDialog();
            if (b.Value)
            {
                AddTree(win.SqlConnectResult);
            }
        }

        private void btn_CloseDb_Click(object sender, RoutedEventArgs e)
        {

        }

        public TreeItem SelectedItem
        {
            get
            {
                return this.tree_data.SelectedItem as TreeItem;
            }
        }
    }
}
