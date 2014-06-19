using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Threading;

namespace WPFService
{
    public class SocketService
    {
        private static Socket _ServerSocket;
        private static List<Socket> _ClientSocket;//为客户端建立的SOCKET连接 
        private static byte[] buffer = new byte[1024];
        private static ManualResetEvent allDone = new ManualResetEvent(false);
        public static int Port = 1019;
        private static bool isListening = false;
        /// <summary>
        /// 默认为1019
        /// </summary>
        public static void Start()
        {
            BaseApiCommon.Global.SysContext = DispatcherSynchronizationContext.Current;
            _ClientSocket = new List<Socket>();
            new Thread(DoThreadListening).Start();
        }
        private static void DoThreadListening()
        {
            _ServerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //提供一个 IP 地址，指示服务器应侦听所有网络接口上的客户端活动 
            IPEndPoint ServerInfo = new IPEndPoint(IPAddress.Any, Port);
            _ServerSocket.Bind(ServerInfo);//将SOCKET接口和IP端口绑定  
            isListening = true;
            try
            {
                _ServerSocket.Listen(10);
                MsgStatus("[" + DateTime.Now + "] 服务器正在运行 ,运行的端口为 " + Port + "。\n");
                while (isListening)
                {
                    allDone.Reset();
                    _ServerSocket.BeginAccept(delegate(IAsyncResult ar)
                    {
                        if (isListening)
                        {
                            allDone.Set();
                            Socket newSock = _ServerSocket.EndAccept(ar);
                            Socket client = newSock;
                            MsgStatus("[" + DateTime.Now + "]" + client.RemoteEndPoint.ToString() + "已经连接！\n");
                            _ClientSocket.Add(client);
                            newSock.BeginReceive(buffer, 0, buffer.Length, 0, new AsyncCallback(ReadDataCallback), client);
                        }
                    }, null);
                    allDone.WaitOne();
                }
            }
            catch (Exception ex)
            {
                Msg(ex.Message);
            }
        }

        private static void ReadDataCallback(IAsyncResult ar)
        {
            if (!isListening) return;
            int bytesRead = 0;
            Socket client = (Socket)ar.AsyncState;
            if (client.Connected == false)
                return;
            bytesRead = client.EndReceive(ar);
            if (bytesRead > 0)
            {
                Msg(bytesRead + "字节");
                string dataContent = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Msg(dataContent + "\n");
                foreach (Socket item in _ClientSocket)
                {
                    item.Send(buffer, 0, bytesRead, SocketFlags.None);
                }
                try
                {
                    client.BeginReceive(buffer, 0, buffer.Length, 0, new AsyncCallback(ReadDataCallback), client);
                }
                catch { }
            }
            else
            {
                MsgStatus("[" + DateTime.Now + "]" + client.RemoteEndPoint.ToString() + "断开Socket\n");
                client.Dispose();
                for (int i = 0; i < _ClientSocket.Count; i++)
                {
                    if (_ClientSocket[i].Equals(client))
                    {
                        _ClientSocket.Remove(client);
                        break;
                    }
                }
            }
        }
        private static void Msg(string msg)
        {
            if (NoticeEvent != null)
                BaseApiCommon.Global.SysContext.Send(o =>
                {
                    NoticeEvent(0, msg);
                }, null);
        }
        private static void MsgStatus(string msg)
        {
            if (NoticeEvent != null)
                BaseApiCommon.Global.SysContext.Send(o =>
                {
                    ClientStatusEvent(1, msg);
                }, null);
        }
        public static event NoticeEventHandler ClientStatusEvent;
        /// <summary>
        /// 消息通知
        /// </summary>
        public static event NoticeEventHandler NoticeEvent;

        internal static void Stop()
        {
            if (!isListening) { return; }
            isListening = false;
            foreach (var item in _ClientSocket)
            {
                item.Dispose();
            }
            _ClientSocket = null;
            _ServerSocket.Close();
            _ServerSocket.Dispose();
            _ServerSocket = null;
            System.GC.Collect();
            MsgStatus("[" + DateTime.Now.ToLongTimeString() + "]" + "停止服务\n");
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="type">0 消息 1 为客户端连接状态信息</param>
    /// <param name="mg"></param>
    public delegate void NoticeEventHandler(int type, string mg);
}
