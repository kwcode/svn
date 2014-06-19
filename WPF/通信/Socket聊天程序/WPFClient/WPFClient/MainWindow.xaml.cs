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
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Threading;
using System.Runtime.InteropServices;

namespace WPFClient
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        SynchronizationContext SysContext = DispatcherSynchronizationContext.Current;
        public MainWindow()
        {
            InitializeComponent();
            txt_servicemsg.Text = "";
            //定义一个IPV4，TCP模式的Socket
            ClientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            MsgBuffer = new Byte[1024];
            //允许子线程刷新数据
            txt_servicemsg.Text += "计算机名称：" + Environment.MachineName + "\n";
            txt_servicemsg.Text += "用户：" + Environment.UserName + "\n";
            txt_servicemsg.Text += "域名：" + Environment.UserDomainName + "\n";
            this.Closed += MainWindow_Closed;
        }


        private Socket ClientSocket;
        //信息接收缓存
        private Byte[] MsgBuffer;

        private static object _LockObj = new object();
        private void btn_ConnectService_Click(object sender, RoutedEventArgs e)
        {
            string port = txt_port.Text.Trim();
            //服务端IP和端口信息设定,这里的IP可以是127.0.0.1，可以是本机局域网IP，也可以是本机网络IP
            IPAddress ip = GetLocalIP();
            IPEndPoint ServerInfo = new IPEndPoint(ip, Convert.ToInt32(port));
            try
            {
                //客户端连接服务端指定IP端口，Sockket
                ClientSocket.Connect(ServerInfo);
                //将用户登录信息发送至服务器，由此可以让其他客户端获知
                ClientSocket.Send(Encoding.UTF8.GetBytes("系统消息： 【" + txt_name.Text + "】进入系统！\n"));
                //开始从连接的Socket异步读取数据。接收来自服务器，其他客户端转发来的信息
                //AsyncCallback引用在异步操作完成时调用的回调方法
                ClientSocket.BeginReceive(MsgBuffer, 0, MsgBuffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallBack), null);
                txt_servicemsg.Text += "成功登录服务器！\n";
            }
            catch
            {
                MessageBox.Show("登录服务器失败，请确认服务器是否正常工作！");
            }
        }
        //结束挂起的异步读取，返回接收到的字节数。 AR，它存储此异步操作的状态信息以及所有用户定义数据
        private void ReceiveCallBack(IAsyncResult AR)
        {
            try
            {
                int REnd = ClientSocket.EndReceive(AR);
                lock (_LockObj)
                {
                    SysContext.Send(o =>
                    {
                        string str = Encoding.UTF8.GetString(MsgBuffer, 0, REnd);
                        txt_msg.Text += str + "\n";
                    }, null);
                }
                ClientSocket.BeginReceive(MsgBuffer, 0, MsgBuffer.Length, 0, new AsyncCallback(ReceiveCallBack), null);
            }
            catch
            {
                MessageBox.Show("已经与服务器断开连接！");
            }
        }

        //发消息
        private void btn_Send_Click(object sender, RoutedEventArgs e)
        {
            //发消息
            Byte[] MsgSend = Encoding.UTF8.GetBytes("[" + DateTime.Now.ToLongTimeString() + "]" + txt_name.Text + ":  " + txt_sendmsg.Text);
            Byte[] sendmsg = new byte[65535];
            if (ClientSocket.Connected)
            {
                //将数据发送到连接的 System.Net.Sockets.Socket。
                ClientSocket.Send(MsgSend);
                txt_sendmsg.Text = "";
            }
            else
            {
                MessageBox.Show("当前与服务器断开连接，无法发送信息！");
            }
        }

        //退出
        private void btn_Out_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        void MainWindow_Closed(object sender, EventArgs e)
        {
            if (ClientSocket.Connected)
            {
                ClientSocket.Send(Encoding.UTF8.GetBytes(txt_name.Text + "离开了房间！\n"));
                //禁用发送和接受
                ClientSocket.Shutdown(SocketShutdown.Both);
                //关闭套接字，不允许重用
                ClientSocket.Disconnect(false);
            }
            ClientSocket.Close();
        }
        #region 方法
        /// <summary>
        /// 获取本地IP 获取失败返回127.0.0.1
        /// </summary>
        /// <returns></returns>
        public IPAddress GetLocalIP()
        {
            IPAddress localip = IPAddress.Parse("127.0.0.1");
            IPAddress[] ips = Dns.GetHostAddresses(Dns.GetHostName());
            foreach (IPAddress ip in ips)
            {
                if (ip.AddressFamily.Equals(AddressFamily.InterNetwork))
                {
                    localip = ip;
                    break;
                }
            }
            return localip;
        }

        #endregion

    }
}
