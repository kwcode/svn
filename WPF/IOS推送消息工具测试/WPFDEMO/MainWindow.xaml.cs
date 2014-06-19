using JHSoft.IOS.pushNotification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPFDEMO
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        Dictionary<int, string> dicSocket = new Dictionary<int, string>();
        public MainWindow()
        {
            InitializeComponent();
            dicSocket.Add(0, "123");
            dicSocket.Add(1, "456");
            txt_stoken.Text = "07616e25dae46b84839a48a597c08912351fdbf961eca061a0c548b34f0f85c4";
            this.KeyDown += MainWindow_KeyDown;
        }

        void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                DoSend();
            }
        }
        private void btn_Send_Click(object sender, RoutedEventArgs e)
        {
            DoSend();
        }
        private void DoSend()
        {
            if (string.IsNullOrWhiteSpace(txt_stoken.Text))
            {
                txt_MSG.Text = "[" + DateTime.Now + "]    设备令牌为空！";
                return;
            }
            if (string.IsNullOrWhiteSpace(txt_action.Text))
            {
                txt_MSG.Text = "[" + DateTime.Now + "]    发送消息为空！";
                return;
            }
            try
            {
                IOSPushNotification.pushNotifications(txt_stoken.Text.Trim(), txt_action.Text.Trim());
                txt_MSG.Text = "[" + DateTime.Now + "]    发送成功！{" + txt_action.Text.Trim() + "}";
            }
            catch (Exception ex)
            {
                txt_MSG.Text = "[" + DateTime.Now + "] " + ex.Message;
            }

        }
        private void btn_Reset_Click(object sender, RoutedEventArgs e)
        {
            txt_stoken.Text = "07616e25dae46b84839a48a597c08912351fdbf961eca061a0c548b34f0f85c4";
        }
    }
}
