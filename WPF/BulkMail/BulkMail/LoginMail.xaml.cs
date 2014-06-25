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
using System.Net.Sockets;
using System.IO;
using System.Net.Mail;
using System.Net;
using System.Reflection;
using BaseApiCommon.Mail;
using System.Windows.Threading;
using BaseApiCommon;
using WPF.CustomControl;
using System.Threading;

namespace BulkMail
{
    /// <summary>
    /// 验证邮箱是否正确
    /// </summary>
    public partial class LoginMail : HLWindowExt
    {
        public LoginMail()
        {
            InitializeComponent();
            Global.SysContext = DispatcherSynchronizationContext.Current;
            btn_OK.Click += new RoutedEventHandler(btn_OK_Click);
            this.btn_Close.Click += new RoutedEventHandler(btn_Close_Click);
            this.KeyDown += new KeyEventHandler(LoginMail_KeyDown);
        }

        void LoginMail_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                string loginName = txt_LoginName.Text.Trim();
                string password = txt_Password.Password;
                StartProgress();
                new Thread(o => { DoLogin(loginName, password); }).Start();
            }
        }

        void btn_Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        void btn_OK_Click(object sender, RoutedEventArgs e)
        {
            string loginName = txt_LoginName.Text.Trim();
            string password = txt_Password.Password;
            StartProgress();
            new Thread(o => { DoLogin(loginName, password); }).Start();
        }

        void DoLogin(string loginName, string password)
        {
            try
            {
                bool isEmail = BMCommon.IsEmail(loginName);
                if (!isEmail)
                {
                    Global.SysContext.Send(o =>
                    {
                        MessageBox.Show(this, "帐号不是正确的邮箱！");
                        StopProgress();
                    }, null);
                    return;
                }
                MailCommon.InitializeTCPClient(loginName);
                #region 验证
                bool b = MailCommon.CheckEmailUser(loginName, password, BMCommon.TCPClient);
                if (b)
                {
                    Global.SysContext.Send(o => { LoginUI(loginName, password); }, null);
                }
                else
                {
                    Global.SysContext.Send(o =>
                    {
                        MessageBox.Show(this, "输入的登录用户名密码不匹配！");
                        StopProgress();
                    }, null);
                    return;
                }

                #endregion

            }
            catch (Exception ex)
            {
                Global.SysContext.Send(o =>
                {
                    MessageBox.Show(this, "登录失败！" + ex.Message);
                    StopProgress();
                }, null); return;

            }

        }
        void LoginUI(string loginName, string password)
        {
            MainWindow win = new MainWindow();
            this.Hide();
            BMCommon.Email = new EMail();
            BMCommon.Email.LoginName = BMCommon.Email.FromAddress = loginName;
            BMCommon.Email.Password = password;
            win.Title = "Send Mail[" + loginName + "]";
            win.Show();
            this.Close();
        }
    }
}
