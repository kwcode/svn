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
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Windows.Threading;

namespace WPFService
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
            this.Closed += new EventHandler(MainWindow_Closed);
            txt_msg.Text = "";
            btn_StopService.IsEnabled = false;
        }

        void MainWindow_Closed(object sender, EventArgs e)
        {
            ServerThread.Abort();//线程终止
            ServerSocket.Close();//关闭SOCKET 
        }

        //声明将要用到的类
        private IPEndPoint ServerInfo;//存放服务器的IP和端口信息
        private Socket ServerSocket;//服务端运行的SOCKET
        private Thread ServerThread;//服务端运行的线程
        private Socket[] ClientSocket;//为客户端建立的SOCKET连接
        private int ClientNumb;//存放客户端数量
        private byte[] MsgBuffer;//存放消息数据
        private void btn_StartService_Click(object sender, RoutedEventArgs e)
        {
            //启动服务
            if (ServerSocket == null)
            {
                ServerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                //提供一个 IP 地址，指示服务器应侦听所有网络接口上的客户端活动
                IPAddress ip = IPAddress.Any;
                ServerInfo = new IPEndPoint(ip, 0619);
                ServerSocket.Bind(ServerInfo);//将SOCKET接口和IP端口绑定
                ServerSocket.Listen(10);//开始监听，并且挂起数为10
                ClientSocket = new Socket[65535];//为客户端提供连接个数
                MsgBuffer = new byte[65535];//消息数据大小
                ClientNumb = 0;//数量从0开始统计
                ServerThread = new Thread(new ThreadStart(RecieveAccept));//将接受客户端连接的方法委托给线程
                ServerThread.Start();//线程开始运行 
                txt_msg.Text += "服务器正在运行 ,运行的端口为0619。[" + DateTime.Now + "]\n\r";
            }
            btn_StartService.IsEnabled = false;
            btn_StopService.IsEnabled = true;
        }
        private static object _LockObj = new object();
        private void RecieveAccept()
        {
            while (true)
            {
                //Accept 以同步方式从侦听套接字的连接请求队列中提取第一个挂起的连接请求，然后创建并返回新的 Socket。
                //在阻止模式中，Accept 将一直处于阻止状态，直到传入的连接尝试排入队列。连接被接受后，原来的 Socket 继续将传入的连接请求排入队列，直到您关闭它。
                ClientSocket[ClientNumb] = ServerSocket.Accept();
                //接受数据
                ClientSocket[ClientNumb].BeginReceive(MsgBuffer, 0, MsgBuffer.Length, SocketFlags.None,
                    new AsyncCallback(RecieveCallBack), ClientSocket[ClientNumb]);
                lock (_LockObj)
                {
                    SysContext.Send(o =>
                    {
                        string str = ClientSocket[ClientNumb].RemoteEndPoint.ToString();
                        txt_msg.Text += str + "\n";
                    }, null);
                }
                ClientNumb++;
            }
        }

        //回发数据给客户端
        private void RecieveCallBack(IAsyncResult AR)
        {
            try
            {
                Socket RSocket = (Socket)AR.AsyncState;
                int REnd = RSocket.EndReceive(AR);
                //对每一个侦听的客户端端口信息进行接收和回发
                for (int i = 0; i < ClientNumb; i++)
                {
                    if (ClientSocket[i].Connected)
                    {
                        //回发数据到客户端
                        ClientSocket[i].Send(MsgBuffer, 0, REnd, SocketFlags.None);
                    }
                    //同时接收客户端回发的数据，用于回发
                    RSocket.BeginReceive(MsgBuffer, 0, MsgBuffer.Length, 0, new AsyncCallback(RecieveCallBack), RSocket);
                }
            }
            catch { }

        }

        private void btn_StopService_Click(object sender, RoutedEventArgs e)
        {
            if (ServerSocket != null)
            {
                ServerThread.Abort();//线程终止
                ServerSocket.Close();//关闭socket 
                ServerSocket = null;
                txt_msg.Text += "已经关闭服务器。[" + DateTime.Now + "]\n\r";
                btn_StartService.IsEnabled = true;
                btn_StopService.IsEnabled = false;
            }
        }

    }
}
