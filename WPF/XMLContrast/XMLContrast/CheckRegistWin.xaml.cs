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
using Microsoft.Win32;
using System.Threading;
using System.Windows.Threading;
using WPF.CustomControl;
using GenerateMachine;

namespace XMLContrast
{
    /// <summary>
    /// CheckRegistWin.xaml 的交互逻辑
    /// </summary>
    public partial class CheckRegistWin : HLWindowExt
    {
        public static SynchronizationContext SysContext = DispatcherSynchronizationContext.Current;
        public CheckRegistWin()
        {
            InitializeComponent();
            btn_Registration.Click += new RoutedEventHandler(btn_Registration_Click);
            btn_Out.Click += new RoutedEventHandler(btn_Out_Click);
            btn_Test.Click += new RoutedEventHandler(btn_Test_Click);
            this.txt_key.Focus();
            this.Loaded += new RoutedEventHandler(CheckRegistWin_Loaded);
        }

        void CheckRegistWin_Loaded(object sender, RoutedEventArgs e)
        {
            object obj = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\FRXMLCONSTRAST", "UseTimes", null);
            if (obj == null)
                Registry.SetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\FRXMLCONSTRAST", "UseTimes", DateTime.Now);
        }

        void btn_Test_Click(object sender, RoutedEventArgs e)
        {
            StartProgress();
            //Registry.SetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\FRXMLCONSTRAST", "UseTimes", DateTime.Now);
            //  object obj = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\FRXMLCONSTRAST", "UseTimes", DateTime.Now);
            object obj = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\FRXMLCONSTRAST", "UseTimes", null);
            if (obj != null)
            {
                DateTime dTime = Convert.ToDateTime(obj);
                TimeSpan ts = DateTime.Now - dTime;
                int days = ts.Days;

                if (DateTime.Now > dTime)
                {
                    if (days > 1)
                    {
                        MessageBox.Show(this, "尊敬的用户您好，您的试用期已结束，为了你能正常使用该软件程序，请联系重庆福软科技有限公司购买正版软件！");
                        StopProgress();
                        return;
                    }
                    else
                    {
                        MessageBox.Show(this, "试用剩余天数：" + (7 - days).ToString() + "天");
                        this.Hide();
                        ListAssistantWin win = new ListAssistantWin();
                        win.Show();
                        this.Close();
                        StopProgress();
                    }
                }
                else
                {
                    MessageBox.Show(this, "尊敬的用户您好，您的试用期已结束，为了你能正常使用该软件程序，请联系重庆福软科技有限公司购买正版软件！");
                    StopProgress();
                    return;
                }
            }

        }

        void btn_Out_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        void btn_Registration_Click(object sender, RoutedEventArgs e)
        {
            StartProgress();
            if (string.IsNullOrEmpty(txt_key.Text.Trim()))
            {
                MessageBox.Show("请输入密钥！", "警告", MessageBoxButton.OK, MessageBoxImage.Warning);
                StopProgress();
                return;
            }
            btn_Registration.IsEnabled = false;
            string key = txt_key.Text.Trim();
            bool b = KeyGenerator.IsExist(key);
            if (b)
            {
                Microsoft.Win32.Registry.CurrentUser.OpenSubKey("software", true).DeleteSubKeyTree("FR");
                Microsoft.Win32.Registry.CurrentUser.OpenSubKey("software", true).CreateSubKey("FR").CreateSubKey("FR.INI").CreateSubKey(key);
                MessageBox.Show(this, "注册成功！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Hide();
                ListAssistantWin win = new ListAssistantWin();
                win.Show();
                this.Close();
                StopProgress();
            }
            else
            {
                MessageBox.Show("密钥不正确！", "警告", MessageBoxButton.OK, MessageBoxImage.Warning);
                btn_Registration.IsEnabled = true;
                StopProgress();
                return;
            }
            //new Thread(o => { CheckRegist(key); }).Start();
        }
        /// <summary>
        /// 检查注册
        /// </summary>
        private void CheckRegist(string key)
        {
            bool b = GenerateMachine.KeyGenerator.IsExist(key);
            //if (!b)
            //{
            //    SysContext.Send(o =>
            //           {
            //               MessageBox.Show(this, "密钥不正确,注册失败！", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
            //               btn_Registration.IsEnabled = true;
            //               StopProgress();
            //           }, null);
            //    return;
            //}
            // string strRnumCode = GenerateMachine.KeyGenerator.getRNum();
            if (b)
            {
                Microsoft.Win32.Registry.CurrentUser.OpenSubKey("software", true).DeleteSubKeyTree("FR");
                Microsoft.Win32.Registry.CurrentUser.OpenSubKey("software", true).CreateSubKey("FR").CreateSubKey("FR.INI").CreateSubKey(key);
                RegistryKey retkey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("software", true).CreateSubKey("FR").CreateSubKey("FR.INI");
                foreach (string strRNum in retkey.GetSubKeyNames())//判断是否注册
                {
                    if (strRNum == key)
                    {
                        SysContext.Send(o =>
                        {
                            MessageBox.Show(this, "注册成功！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                            this.Hide();
                            ListAssistantWin win = new ListAssistantWin();
                            win.Show();
                            this.Close();
                            StopProgress();
                        }, null);
                        return;
                    }
                }
            }
            Thread th2 = new Thread(new ThreadStart(thCheckRegist2));
            th2.Start();

        }

        /// <summary>
        /// 验证试用次数
        /// </summary>
        private void thCheckRegist2()
        {
            string message = "密钥注册失败,您现在使用的是试用版，该软件可以免费试用30次！";
            Int32 tLong;
            try
            {
                tLong = (Int32)Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\FRXMLCONSTRAST", "UseTimes", 0);
                MessageBox.Show(message + "\n感谢您已使用了" + tLong + "次", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch
            {
                Registry.SetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\FRXMLCONSTRAST", "UseTimes", 0, RegistryValueKind.DWord);
                MessageBox.Show("欢迎新用户使用本软件", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            tLong = (Int32)Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\FRXMLCONSTRAST", "UseTimes", 0);
            if (tLong < 30)
            {
                int Times = tLong + 1;
                Registry.SetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\FRXMLCONSTRAST", "UseTimes", Times);
                SysContext.Send(o =>
                {
                    this.Hide();
                    ListAssistantWin win = new ListAssistantWin();
                    win.Show();
                    this.Close();
                    StopProgress();
                }, null);
                return;
            }
            else
            {
                MessageBox.Show("试用次数已到", "警告", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            SysContext.Send(o =>
            {
                btn_Registration.IsEnabled = true;
                StopProgress();
            }, null);
        }

    }
}

