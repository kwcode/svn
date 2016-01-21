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
using TCode.Model;

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
            this.Loaded += LoginSQL_Loaded;
            comboBoxServer.SelectionChanged += comboBoxServer_SelectionChanged;
        }

        void comboBoxServer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cb = sender as ComboBox;
            DB db = cb.SelectedItem as DB;
            if (db != null)
            {
                if (comboBox_Verified.SelectedIndex == 0)
                {
                    txtUser.Text = db.UserID;
                    txtPass.Text = db.Password;
                }
            }

        }

        void LoginSQL_Loaded(object sender, RoutedEventArgs e)
        {
            //初始化加载xml里面现有数据
            Helper.SerializationCommon.Init();
            List<DB> dbList = Helper.SerializationCommon.DeserializeXML<List<DB>>();
            comboBoxServer.ItemsSource = dbList;
            comboBoxServer.DisplayMemberPath = "ServiceName";
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
            SqlConnectResult = new SqlConnectResult();
            if (this.comboBox_Verified.SelectedIndex == 0)
            {
                string user = this.txtUser.Text.Trim();
                string pass = this.txtPass.Text.Trim();
                if (string.IsNullOrWhiteSpace(user) || string.IsNullOrWhiteSpace(pass))
                {
                    MessageBox.Show("服务器或用户名不能为空!");
                    return;
                }
                SqlConnectResult.IsSqlService = true;
                SqlConnectResult.ServiceName = server;
                SqlConnectResult.DbName = dbName;
                SqlConnectResult.UserID = user;
                if (pass == "")
                {
                    constr = "user id=" + user + ";initial catalog=" + dbName + ";data source=" + server;
                }
                else
                {
                    SqlConnectResult.Password = pass;
                    constr = "user id=" + user + ";password=" + pass + ";initial catalog=" + dbName + ";data source=" + server;
                }
            }
            else
            {
                SqlConnectResult.IsSqlService = false;
                SqlConnectResult.ServiceName = server;
                SqlConnectResult.DbName = dbName;
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

                    #region MyRegion
                    List<DB> dbList = comboBoxServer.ItemsSource as List<DB>;
                    bool isExist = dbList.Exists(o => { return o.ServiceName.Equals(server); });
                    if (!isExist)
                    {
                        dbList.Add(new DB()
                        {
                            ServiceName = server,
                            UserID = SqlConnectResult.UserID,
                            Password = SqlConnectResult.Password
                        });
                        Helper.SerializationCommon.SerializeXML(dbList);
                    }
                    #endregion

                    DialogResult = true;
                    SqlConnectResult.IsAll = dbClass.IsAll;
                    SqlConnectResult.DbName = dbName;
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
        /// 返回相关信息
        /// </summary>
        public SqlConnectResult SqlConnectResult { get; set; }
        private void btn_Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }


}
