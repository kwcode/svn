using DBCommon;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace TCode
{
    /// <summary>
    /// LoginSQL.xaml 的交互逻辑
    /// </summary>
    public partial class LoginSQL : Window
    {
        public LoginSQL()
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.ResizeMode = ResizeMode.NoResize;
            this.comboBox_Verified.SelectionChanged += comboBox_Verified_SelectionChanged;
        }

        void comboBox_Verified_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cb = sender as ComboBox;
            if (cb.SelectedIndex == 0)
            {
                txtUser.IsEnabled = true;
                txtPass.IsEnabled = true;
            }
            else
            {
                txtUser.IsEnabled = false;
                txtPass.IsEnabled = false;
            }
        }

        string constr = "";
        DataClass dc = new DataClass();
        private void btn_Test_Click(object sender, RoutedEventArgs e)
        {
            string server = this.comboBoxServer.Text.Trim();
            if (this.comboBox_Verified.SelectedIndex == 0)
            {
                string user = this.txtUser.Text.Trim();
                string pass = this.txtPass.Text.Trim();
                if (string.IsNullOrWhiteSpace(user) || string.IsNullOrWhiteSpace(pass))
                {
                    MessageBox.Show("服务器或用户名不能为空!");
                    return;
                }
                if (pass == "")
                {
                    constr = "user id=" + user + ";initial catalog=master;data source=" + server;
                }
                else
                {
                    constr = "user id=" + user + ";password=" + pass + ";initial catalog=master;data source=" + server;
                }
            }
            else
            {
                constr = "Integrated Security=SSPI;Data Source=" + server + ";Initial Catalog=master";
            }

            try
            {
                this.Title = "正在连接服务器，请稍候...";
                dc.ConnectionString = constr;
                bool b = dc.OpenConn(true);
                if (b)
                {
                    string strSql = "select name from sysdatabases order by name";
                    DataTable dt = dc.GetDataTable(strSql);
                    List<DbNameClass> dbList = new List<DbNameClass>();
                    dbList.Add(new DbNameClass() { Name = "全部", IsAll = true });
                    foreach (System.Data.DataRow item in dt.Rows)
                    {
                        dbList.Add(new DbNameClass() { Name = item["name"].ToString(), IsAll = false });
                    }
                    this.cmbDBlist.ItemsSource = dbList;
                    this.cmbDBlist.DisplayMemberPath = "Name";
                    this.Title = "连接服务器成功！";
                }
                else
                {
                    this.Title = "连接服务器失败！";
                }
            }
            catch (Exception ex)
            {
                this.Title = ex.Message;
            }
            finally
            {
                dc.CloseConn();
            }
        }

        private void btn_Ok_Click(object sender, RoutedEventArgs e)
        {
            string server = this.comboBoxServer.Text.Trim();

            DbNameClass dbClass = cmbDBlist.SelectedItem as DbNameClass;

            if (dbClass == null)
            {
                MessageBox.Show("请先连接数据库，选择！");
                return;
            }
            string dbName = dbClass.Name;
            if (dbClass.IsAll == true)
            {
                dbName = "master";
            }
            SqlConnectInfo = new DbSqlConnect();
            if (this.comboBox_Verified.SelectedIndex == 0)
            {
                string user = this.txtUser.Text.Trim();
                string pass = this.txtPass.Text.Trim();
                if (string.IsNullOrWhiteSpace(user) || string.IsNullOrWhiteSpace(pass))
                {
                    MessageBox.Show("服务器或用户名不能为空!");
                    return;
                }
                SqlConnectInfo.IsSqlService = true;
                SqlConnectInfo.ServiceName = server;
                SqlConnectInfo.DbName = dbName;
                SqlConnectInfo.UserID = user;
                if (pass == "")
                {
                    constr = "user id=" + user + ";initial catalog=" + dbName + ";data source=" + server;
                }
                else
                {
                    SqlConnectInfo.Password = pass;
                    constr = "user id=" + user + ";password=" + pass + ";initial catalog=" + dbName + ";data source=" + server;
                }
            }
            else
            {
                SqlConnectInfo.IsSqlService = false;
                SqlConnectInfo.ServiceName = server;
                SqlConnectInfo.DbName = dbName;
                constr = "Integrated Security=SSPI;Data Source=" + server + ";Initial Catalog=" + dbName;
            }

            try
            {
                this.Title = "正在连接服务器，请稍候...";
                dc.ConnectionString = constr;
                bool b = dc.OpenConn(true);
                if (b)
                {
                    this.Title = "连接数据库" + dbName + "成功！";
                    DialogResult = true;
                    SqlDbInfo = dbClass;
                    SqlServiceName = server;
                }
                else
                {
                    this.Title = "连接数据库" + dbClass.Name + "失败！";
                }
            }
            catch (Exception ex)
            {
                this.Title = ex.Message;
            }
            finally
            {
                dc.CloseConn();
            }
        }
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public DbSqlConnect SqlConnectInfo { get; set; }
        /// <summary>
        /// 选择的数据库名称
        /// </summary>
        public DbNameClass SqlDbInfo { get; set; }
        /// <summary>
        /// sql服务器名称
        /// </summary>
        public string SqlServiceName { get; set; }
        private void btn_Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
    public class DbNameClass
    {
        /// <summary>
        /// 是否所有数据库
        /// </summary>
        public bool IsAll { get; set; }
        /// <summary>
        /// 数据库表
        /// </summary>
        public string Name { get; set; }
    }
    /// <summary> 
    /// </summary>
    public class DbSqlConnect
    {
        /// <summary>
        /// 是否就Sql Service 登陆
        /// true:"user id=" + user + ";password=" + pass + ";initial catalog=" + dbName + ";data source=" + server;
        /// false:"Integrated Security=SSPI;Data Source=" + server + ";Initial Catalog=" + dbName;
        /// </summary>
        public bool IsSqlService { get; set; }

        public string ServiceName { get; set; }
        public string DbName { get; set; }
        public string UserID { get; set; }
        public string Password { get; set; }
    }
}
