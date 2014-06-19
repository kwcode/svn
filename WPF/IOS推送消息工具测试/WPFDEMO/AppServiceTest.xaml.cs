using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WPFDEMO
{
    /// <summary>
    /// AppServiceTest.xaml 的交互逻辑
    /// </summary>
    public partial class AppServiceTest : Window
    {
        public AppServiceTest()
        {
            InitializeComponent();
        }
        private Socket listener = null;
        private bool isListening = false;
        private const int bufferSize = 1024;
        private byte[] buffer = new byte[bufferSize];
        private void btn_Invoke_Click(object sender, RoutedEventArgs e)
        {
            Socket sockets = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            sockets.Connect("192.168.1.200", 5220);
            string jsonstri = "{Action:\"UserLogin\",LoginName:\"test\",Password:\"202cb962ac59075b964b07152d234b70\",DeviceModel:\"\",DeviceSystem:\"\",DeviceSystemType:\"\",Status:\"0\",Ver:\"v1\"}";

            // jsonstri = "{Action:\"UserDeviceToken\",SessionID:\"u36CKV8eckcuucDgj0HzTX0qR11cDRykGgDtCw\",DeviceToken:\"07616e25dae46b84839a48a597c08912351fdbf961eca061a0c548b34f0f85c4\"}";

            byte[] buff = System.Text.Encoding.Default.GetBytes(jsonstri);
            sockets.Send(buff);

        }
        //private void Listening()
        //{
        //    if (listener == null)
        //    {
        //        IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Any, 5221);
        //        listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        //        listener.Bind(localEndPoint);
        //    }
        //    isListening = true;
        //    try
        //    {
        //        listener.Listen(10);//监听
        //        while (isListening)
        //        {
        //            listener.BeginAccept(delegate(IAsyncResult ar)
        //                {
        //                    if (isListening)
        //                    {
        //                        Socket newSock = listener.EndAccept(ar);
        //                        newSock.BeginReceive(buffer, 0, buffer.Length, 0, new AsyncCallback(ReadDataCallback), "");
        //                    }

        //                }, null);
        //        }
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //} 
    }
}
