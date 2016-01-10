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
            if (cb.SelectedIndex == 1)
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
                    this.cmbDBlist.ItemsSource = dt.AsDataView();
                    this.cmbDBlist.DisplayMemberPath = "name";
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
            string dbname = "";
            if (this.cmbDBlist.SelectedValue != null)
            {
                dbname = this.cmbDBlist.Text.Trim();
            }

            if (string.IsNullOrWhiteSpace(dbname))
            {
                MessageBox.Show("请先连接数据库，选择！");
                return;
            }
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
                    constr = "user id=" + user + ";initial catalog=" + dbname + ";data source=" + server;
                }
                else
                {
                    constr = "user id=" + user + ";password=" + pass + ";initial catalog=" + dbname + ";data source=" + server;
                }
            }
            else
            {
                constr = "Integrated Security=SSPI;Data Source=" + server + ";Initial Catalog=" + dbname;
            }

            try
            {
                this.Title = "正在连接服务器，请稍候...";
                dc.ConnectionString = constr;
                bool b = dc.OpenConn(true);
                if (b)
                {
                    this.Title = "连接数据库" + dbname + "成功！";
                    DialogResult = true;
                    SqlConnectionString = constr;
                    SqlDbName = dbname;
                }
                else
                {
                    this.Title = "连接数据库" + dbname + "失败！";
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
        public string SqlConnectionString { get; set; }
        /// <summary>
        /// 选择的数据库名称
        /// </summary>
        public string SqlDbName { get; set; }
        private void btn_Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
