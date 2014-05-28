using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Threading;
using System.Windows;
using System.Net.Sockets;
using System.IO;
using System.Text.RegularExpressions;
using BaseApiCommon.Mail;
using System.Net;

namespace BulkMail
{
    public class MailCommon
    {
        #region 验证登录邮箱

        /// <summary>
        /// 初始化TCP网络服务连接
        /// </summary>
        /// <param name="user">登录用户名（登录邮箱）</param>
        /// <returns></returns>
        public static TcpClient InitializeTCPClient(string user)
        {
            //TODO:解析 用户名 
            MailServece ms = GetAccordServer(user);
            TcpClient TCPClient = new TcpClient(ms.PopServerAddress, 110);
            return TCPClient;
        }
        /// <summary>
        /// 初始化TCP网络服务连接
        /// </summary>
        /// <param name="hostname">要连接到的远程主机的 DNS 名</param>
        /// <param name="port"> 要连接到的远程主机的端口号。</param>
        public static TcpClient InitializeTCPClient(string hostname, int port)
        {
            TcpClient TCPClient = new TcpClient(hostname, port);
            return TCPClient;
        }


        /// <summary>
        /// 验证PoP邮箱 登录名密码是否正确
        /// </summary>
        /// <param name="user">登录邮箱</param>
        /// <param name="pwd">密码</param>
        /// <param name="TCPClient">TCP 网络服务提供客户端连接</param>
        /// <returns></returns>
        public static bool CheckEmailUser(string user, string pwd, TcpClient TCPClient)
        {
            try
            {
                if (TCPClient == null)
                {
                    TCPClient = InitializeTCPClient(user);
                }
                NetworkStream ns = TCPClient.GetStream();//用于网络访问返回发送和接收信息
                StreamReader sr = new StreamReader(ns);//读取字符
                string ss01 = sr.ReadLine();
                // StreamWriter sw = new StreamWriter(ns);//写
                //StreamReader  sr = new StreamWriter(ns); //得到读对象，并查找字节顺序标记，防止显示乱码
                //检查用户名
                Byte[] userbytes = Encoding.ASCII.GetBytes("USER " + user + "\r\n");
                ns.Write(userbytes, 0, userbytes.Length);//写入
                string urlex = sr.ReadLine();//读取返回的信息d
                //检查密码  
                Byte[] pwdbytes = Encoding.ASCII.GetBytes("PASS " + pwd + "\r\n");
                ns.Write(pwdbytes, 0, pwdbytes.Length);
                string prlex = sr.ReadLine();
                if (urlex.Substring(0, 3) == "+OK" && prlex.Substring(0, 3) == "+OK")
                    return true;
                else
                    return false;
            }
            catch { return false; }

        }

        #endregion

        #region 发送邮件

        /// <summary>
        /// 初始化SMTP网络服务连接
        /// </summary> 
        /// <param name="host">SMTP 事务的主机的名称或 IP 地址</param>
        /// <param name="port">端口号</param>
        /// <returns></returns>
        public static SmtpClient InitializeSMTPClient(string host, int port)
        {
            SmtpClient SMTPClient = new SmtpClient(host, port); // 25 qq的端口   
            return SMTPClient;
        }
        /// <summary>
        /// 初始化SMTP网络服务连接
        /// </summary>
        /// <param name="user">登录用户名（登录邮箱）</param>
        /// <returns></returns>
        public static SmtpClient InitializeSMTPClient(string user)
        {
            //TODO:解析 用户名 
            MailServece ms = GetAccordServer(user);
            SmtpClient SMTPClient = new SmtpClient(ms.SmtpServerAddress, ms.SmtpPort); // 25 qq的端口   
            return SMTPClient;
        }

        /// <summary>
        /// SMTP协议发送邮件
        /// </summary>
        /// <param name="mm">发送的电子邮件</param>
        /// <param name="SMTPClient">SMTP协议</param>
        /// <returns></returns>
        public static MailSendMessage SendMail(MailMessage mm, SmtpClient SMTPClient)
        {
            MailSendMessage msm = new MailSendMessage();
            try
            {
                SMTPClient.Send(mm);
                int time = SMTPClient.Timeout;

                msm.ExMessageinfo = "发送成功！";
            }
            catch (Exception ex)
            {
                msm.ExMessageinfo = "发送失败！" + ex.Message;
            }
            return msm;
        }

        /// <summary>
        /// SMTP协议发送邮件
        /// </summary>
        /// <param name="eMail">邮件信息</param>
        /// <param name="SMTPClient">SMTP协议</param>
        /// <returns></returns>
        public static MailSendMessage SendMail(EMail eMail, SmtpClient SMTPClient)
        {

            MailSendMessage msm = new MailSendMessage();
            double timeSpan;
            DateTime sendTime = DateTime.Now;
            DateTime endTime;
            try
            {
                if (SMTPClient == null)
                {
                    SMTPClient = InitializeSMTPClient(eMail.LoginName);
                }
                if (eMail == null)
                {
                    msm.ExMessageinfo = "邮件信息或者协议存在错误，不能发送邮件！";
                    return msm;
                }
                MailMessage mm = new MailMessage();
                mm.From = new MailAddress(eMail.FromAddress, eMail.Name);
                if (eMail.ToAddress != null)
                {
                    foreach (string item in eMail.ToAddress)
                    {
                        mm.To.Add(new MailAddress(item));
                    }
                }
                mm.IsBodyHtml = true;//取得或設定值，指出郵件訊息主體是否採用 Html 格式。
                mm.BodyEncoding = System.Text.Encoding.UTF8;//取得或設定用來對訊息主體進行編碼的編碼方式。
                mm.Body = eMail.Body;
                mm.Subject = eMail.Subject;
                mm.Priority = MailPriority.High;
                SMTPClient.Credentials = new NetworkCredential(eMail.LoginName, eMail.Password); //这里是申请的邮箱和密码
                sendTime = DateTime.Now;
                SMTPClient.Send(mm);
                endTime = DateTime.Now;
                timeSpan = GetTimeSpan(sendTime, endTime);
                msm.ExMessageinfo = "发送成功！";
                msm.MailTimeSpan = timeSpan;
            }
            catch (Exception ex)
            {
                endTime = DateTime.Now;
                timeSpan = GetTimeSpan(sendTime, endTime);
                msm.ExMessageinfo = "发送失败！" + ex.Message;
                msm.MailTimeSpan = timeSpan;
            }
            return msm;
        }

        static DateTime _StartTime;
        static DateTime _EndTime;
        public static MailSendMessage SendMail(string userName, string password, string fromAddress, string[] toAddress, string subject, string body, SendType type)
        {

            MailSendMessage msm = new MailSendMessage();
            _StartTime = DateTime.Now;
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(fromAddress, userName);//必须是提供smtp服务的邮件服务器 
            if (toAddress != null)
            {
                foreach (string item in toAddress)
                {
                    mail.To.Add(new MailAddress(item));
                }
            }
            mail.Subject = subject;
            //mail.CC.Add(new MailAddress("761607380@qq.com"));
            //mail.Bcc.Add(new MailAddress("761607380@qq.com"));
            mail.IsBodyHtml = true;//取得或設定值，指出郵件訊息主體是否採用 Html 格式。
            mail.BodyEncoding = System.Text.Encoding.UTF8;//取得或設定用來對訊息主體進行編碼的編碼方式。
            mail.Body = body;
            mail.Priority = System.Net.Mail.MailPriority.High;//取得或設定這個電子郵件訊息的優先權。 
            SmtpClient client;
            if (type == SendType.QQ)
            {
                client = new SmtpClient("smtp.qq.com", 25); // 25 qq的端口  
            }
            else if (type == SendType.Gmail)
            {
                client = new SmtpClient("smtp.Gmail.com", 587); //  587;//Gmail使用的端口 
                client.EnableSsl = true; //必须经过ssl加密 
            }
            else { client = new SmtpClient(); }
            client.Credentials = new System.Net.NetworkCredential(userName, password); //这里是申请的邮箱和密码

            try
            {
                client.Send(mail);//发送邮件
                _EndTime = DateTime.Now;
                double ts = GetTimeSpan(_StartTime, _EndTime);
                msm.MailTimeSpan = ts;
                msm.ExMessageinfo = "发送成功！";
            }
            catch (Exception ex)
            {
                msm.ExMessageinfo = "发送有失败的！" + ex.Message;
            }
            return msm;
        }

        public MailSendMessage SendMail(EMail mail)
        {
            MailSendMessage msm = new MailSendMessage();
            MailMessage mm = ConvertMail(mail);
            SendMail(mm);
            return msm;
        }

        private MailMessage ConvertMail(EMail mail)
        {
            throw new NotImplementedException();
        }

        public MailSendMessage SendMail(MailMessage mail)
        {
            MailSendMessage msm = new MailSendMessage();
            return msm;

        }

        /// <summary>
        /// 获取时间跨度 秒
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        private static double GetTimeSpan(DateTime startTime, DateTime endTime)
        {
            long ticks = endTime.Ticks - startTime.Ticks;
            TimeSpan timespan = new TimeSpan(ticks);
            double ts = timespan.TotalSeconds;
            return ts;
        }

        #endregion

        #region 其他方法
        /// <summary>
        /// 获取 邮箱服务器的信息 STMP 和POP3
        /// </summary>
        /// <returns></returns>
        public static MailServece[] GetMailServer()
        {
            List<MailServece> smtpServer = new List<MailServece>();
            smtpServer.Add(new MailServece() { Name = "网易163邮箱", SmtpServerAddress = "smtp.163.com", SmtpPort = 25, PopServerAddress = "pop3.163.com" });
            smtpServer.Add(new MailServece() { Name = "网易vip.163邮箱", SmtpServerAddress = "smtp.vip.163.com", SmtpPort = 25, PopServerAddress = "pop3.vip.163.com" });
            smtpServer.Add(new MailServece() { Name = "网易126邮箱", SmtpServerAddress = "smtp.126.com", SmtpPort = 25, PopServerAddress = "pop3.126.com" });
            smtpServer.Add(new MailServece() { Name = "网易188邮箱", SmtpServerAddress = "smtp.188.com", SmtpPort = 25, PopServerAddress = "pop3.188.com" });
            smtpServer.Add(new MailServece() { Name = "新浪邮箱", SmtpServerAddress = "smtp.sina.com", SmtpPort = 25, PopServerAddress = "pop3.sina.com" });

            smtpServer.Add(new MailServece() { Name = "雅虎邮箱", SmtpServerAddress = "smtp.mail.yahoo.com", SmtpPort = 25, PopServerAddress = "pop3.mail.yahoo.com" });
            smtpServer.Add(new MailServece() { Name = "搜狐邮箱", SmtpServerAddress = "smtp.sohu.com", SmtpPort = 25, PopServerAddress = "pop3.sohu.com" });
            smtpServer.Add(new MailServece() { Name = "TOM邮箱", SmtpServerAddress = "smtp.tom.com", SmtpPort = 25, PopServerAddress = "pop.tom.com" });
            smtpServer.Add(new MailServece() { Name = "Gmail邮箱", SmtpServerAddress = "smtp.gmail.com", SmtpPort = 587, PopServerAddress = "pop.gmail.com" });

            smtpServer.Add(new MailServece() { Name = "QQ邮箱", SmtpServerAddress = "smtp.qq.com", SmtpPort = 25, PopServerAddress = "pop.qq.com" });
            smtpServer.Add(new MailServece() { Name = "QQ企业邮箱", SmtpServerAddress = "smtp.biz.mail.qq.com", SmtpPort = 25, PopServerAddress = "pop.biz.mail.qq.com" });
            smtpServer.Add(new MailServece() { Name = "139邮箱", SmtpServerAddress = "smtp.139.com", SmtpPort = 25, PopServerAddress = "pop.139.com" });
            smtpServer.Add(new MailServece() { Name = "263邮箱", SmtpServerAddress = "smtp.263.com", SmtpPort = 25, PopServerAddress = "pop.263.com" });

            return smtpServer.ToArray();
        }
        /// <summary>
        /// 根据邮箱地址获取 对应的服务器地址
        /// </summary>
        /// <param name="mailAddress"></param>
        /// <returns></returns>
        public static MailServece GetAccordServer(string mailAddress)
        {
            MailServece mService = new MailServece();
            int index = mailAddress.IndexOf("@") + 1;
            string ms = mailAddress.Substring(index, mailAddress.Length - index);
            MailServece[] msArray = GetMailServer();
            if (msArray != null)
            {
                foreach (MailServece item in msArray)
                {
                    if (item.PopServerAddress.Contains(ms))
                    {
                        mService = item;
                        break;
                    }
                }
            }

            //msArray.ToList().Find(delegate(string s) { });
            return mService;
        }
        #endregion

        internal MailSendMessage SendMail(string LoginName, string password, string fromAddress, string[] toAddress, string subject, string body, MailServece ms)
        {
            MailSendMessage msm = new MailSendMessage();
            _StartTime = DateTime.Now;
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(fromAddress, LoginName);//必须是提供smtp服务的邮件服务器 
            if (toAddress != null)
            {
                foreach (string item in toAddress)
                {
                    mail.To.Add(new MailAddress(item));
                }
            }
            mail.Subject = subject;
            //mail.CC.Add(new MailAddress("761607380@qq.com"));
            //mail.Bcc.Add(new MailAddress("761607380@qq.com"));
            mail.IsBodyHtml = true;//取得或設定值，指出郵件訊息主體是否採用 Html 格式。
            mail.BodyEncoding = System.Text.Encoding.UTF8;//取得或設定用來對訊息主體進行編碼的編碼方式。
            mail.Body = body;
            mail.Priority = System.Net.Mail.MailPriority.High;//取得或設定這個電子郵件訊息的優先權。 
            SmtpClient client = new SmtpClient(ms.SmtpServerAddress, ms.SmtpPort); // 25 qq的端口  
            if (ms.SmtpServerAddress.Contains("gmail"))
            {
                client.EnableSsl = true; //必须经过ssl加密 
            }

            client.Credentials = new System.Net.NetworkCredential(LoginName, password); //这里是申请的邮箱和密码

            try
            {
                client.Send(mail);//发送邮件
                _EndTime = DateTime.Now;
                double ts = GetTimeSpan(_StartTime, _EndTime);
                msm.MailTimeSpan = ts;
                msm.ExMessageinfo = "发送成功！";
            }
            catch (Exception ex)
            {
                msm.ExMessageinfo = "发送有失败的！" + ex.Message;
            }
            return msm;
        }


    }
    /// <summary>
    /// 发送邮件过程中产生的信息
    /// </summary>
    public class MailSendMessage
    {
        public string ID { get; set; }
        /// <summary>
        /// 成功数量
        /// </summary>
        public string SuccessNumber { get; set; }
        /// <summary>
        /// 成功地址
        /// </summary>
        public string[] SuccessAdress { get; set; }
        /// <summary>
        /// 失败数量
        /// </summary>
        public string FailNumber { get; set; }
        /// <summary>
        /// 失败地址
        /// </summary>
        public string[] FailAdress { get; set; }
        /// <summary>
        /// 邮件发送时间间隔
        /// </summary>
        public double MailTimeSpan { get; set; }
        /// <summary>
        //错误信息
        /// </summary>
        public string ExMessageinfo { get; set; }
    }
    public enum SendType
    {
        QQ = 0,
        Gmail = 1,
    }

    public class MailServece
    {
        public string Name { get; set; }
        public string SmtpServerAddress { get; set; }
        public int SmtpPort { get; set; }
        public string PopServerAddress { get; set; }
    }


}
