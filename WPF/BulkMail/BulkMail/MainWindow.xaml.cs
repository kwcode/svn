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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net.Mail;
using System.Windows.Threading;
using System.Threading;
using BaseApiCommon;
using WPF.CustomControl;

namespace BulkMail
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : HLWindowExt
    {
        public MainWindow()
        {
            InitializeComponent();
            btn_Sendmail.Click += new RoutedEventHandler(btn_Sendmail_Click);
            btn_Close.Click += new RoutedEventHandler(btn_Close_Click);
            btn_Select.Click += new RoutedEventHandler(btn_Select_Click);
        }

        #region 事件

        void btn_Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        //  Gmail的smtp服务必须经过ssl加密，才可以验证成功
        void btn_Sendmail_Click(object sender, RoutedEventArgs e)
        {
            string toAddress = txt_ToAddress.Text.Trim();
            string subject = txt_Subject.Text.Trim();
            string body = txt_Body.Text.Trim();

            string[] address = ResolveAddress(toAddress);
            string[] toaddress = FiltersMail(address);//筛选正确的接收人
            if (toaddress == null || toaddress.Length == 0)
            {
                MessageBox.Show("接收人不能为空或者格式不正确！");
                return;
            }
            if (BMCommon.Email == null)
            {
                MessageBox.Show("请重启系统！");
                return;
            }
            BMCommon.Email.ToAddress = toaddress;
            BMCommon.Email.Subject = subject;
            BMCommon.Email.Body = body;

            // MailSendMessage     msm = MailCommon.SendMail(BMCommon.Email.LoginName,BMCommon.Email .Password,BMCommon.Email .LoginName,address,subject,body,SendType.QQ);
            StartProgress();
            new Thread(o => { DoThreadSendMail(); }).Start();
        }
        void DoThreadSendMail()
        {
            try
            {
                MailSendMessage msm = MailCommon.SendMail(BMCommon.Email, BMCommon.SMTPClient);
                Global.SysContext.Send(o =>
                {
                    MessageBox.Show(this, msm.ExMessageinfo + "用时：" + msm.MailTimeSpan);
                    StopProgress();
                }, null);
            }
            catch (Exception ex)
            {
                Global.SysContext.Send(o =>
                {
                    MessageBox.Show(this, ex.Message);
                    StopProgress();
                }, null);
                return;
            }

        }

        /// <summary>
        /// 筛选正确的邮件地址
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        private string[] FiltersMail(string[] address)
        {
            List<string> toList = new List<string>();
            if (address == null) return toList.ToArray();
            {
                foreach (string item in address)
                {
                    bool b = BMCommon.IsEmail(item);
                    if (b)
                    {
                        toList.Add(item);
                    }
                }
            }
            return toList.ToArray();
        }
        /// <summary>
        /// 解析接收人地址
        /// </summary>
        /// <param name="toAddress"></param>
        /// <returns></returns>
        private string[] ResolveAddress(string toAddress)
        {
            List<string> toList = new List<string>();
            string[] tostr = toAddress.Split(';');
            toList = tostr.ToList();
            return toList.ToArray();
        }
        private string[] GetToAddress()
        {
            List<string> toAddress = new List<string>();
            toAddress.Add("353328333@qq.com");
            toAddress.Add("761607380@qq.com");
            return toAddress.ToArray();
        }

        #endregion


        void btn_Select_Click(object sender, RoutedEventArgs e)
        {
            StartProgress();
            RecipientAddressWin win = new RecipientAddressWin();
            bool? b = win.ShowDialog();
            if (b.Value)
            {
                AddMailAddressText(win.ToAddress);
            }
            StopProgress();
        }

        private void AddMailAddressText(string[] toAddress)
        {
            if (toAddress == null || toAddress.Length == 0) return;
            string str = "";
            foreach (string item in toAddress)
            {
                str += item + ";";
            }
            txt_ToAddress.Text = str;
        }

      
    }
}
