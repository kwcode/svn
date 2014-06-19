using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace WPFService
{
    /// <summary>
    /// Window1.xaml 的交互逻辑
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
            btn_StopService.IsEnabled = false;
        }
        void SocketService_ClientStatusEvent(int type, string mg)
        {
            txt_msg.Text += mg;
        }

        void SocketService_NoticeEvent(int type, string mg)
        {
            txt_newsmsg.Text += mg;
        }

        private void btn_StartService_Click(object sender, RoutedEventArgs e)
        {
            SocketService.NoticeEvent -= SocketService_NoticeEvent;
            SocketService.ClientStatusEvent -= SocketService_ClientStatusEvent;
            SocketService.NoticeEvent += SocketService_NoticeEvent;
            SocketService.ClientStatusEvent += SocketService_ClientStatusEvent;
            SocketService.Start();
            btn_StartService.IsEnabled = false;
            btn_StopService.IsEnabled = true;
        }

        private void btn_StopService_Click(object sender, RoutedEventArgs e)
        {
            SocketService.Stop();
            btn_StartService.IsEnabled = true;
        }
    }
}
