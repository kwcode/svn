using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net.Mail;
using System.Text.RegularExpressions;
using BaseApiCommon.Mail;
using BaseApiCommon;
using System.IO;
using System.Xml.Serialization;

namespace BulkMail
{
    /// <summary>
    /// 邮件群发器 静态公共类
    /// </summary>
    public class BMCommon
    {
        /// <summary>
        ///  为 TCP 网络服务提供客户端连接。
        /// </summary>
        public static TcpClient TCPClient;
        /// <summary>
        /// 发送邮件协议
        /// </summary>
        public static SmtpClient SMTPClient;
        /// <summary>
        /// 当前登录邮箱信息
        /// </summary>
        public static EMail Email;

        /// <summary> 
        /// 判断邮箱格式是否正确 
        /// </summary> 
        /// <param name="email">邮箱地址</param> 
        public static bool IsEmail(string email)
        {
            string paterner = @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";
            if (!Regex.IsMatch(email, paterner))
            { return false; }
            else
            { return true; }
        }
        /// <summary>
        /// 是否存在该文件，不存在创建
        /// </summary>
        /// <param name="path"></param>
        public static void ExistsPath(string path)
        {
            bool isExist = File.Exists(path);
            if (isExist == false)
            {
                path = System.AppDomain.CurrentDomain.BaseDirectory + @"Resources";
                DirectoryInfo dir = new DirectoryInfo(path);
                if (dir.Exists == false)//如果不存在则创建
                {
                    Directory.CreateDirectory(path);
                }
                path = path + @"\ToAddress.xml";
                List<ToAddress> toList = new List<ToAddress>();
                BMCommon.SaveToAddressXML(toList, path);
            }
        }
        public static ToAddress[] GetToaddress(string path)
        {
            List<ToAddress> toList = new List<ToAddress>();
            if (path == null) return toList.ToArray();
            //toList = SerializationCommon.XMLDesrialize<List<ToAddress>>(path) ;
            //if (toList == null) return toList.ToArray();
            return toList.ToArray();
        }
        /// <summary>
        /// 可视化XML
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="path"></param>
        public static void SaveToAddressXML(object obj, string path)
        {
            SerializationCommon.SerializeXML(obj, path);
        }
        /// <summary>
        /// 乱码
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="path"></param>
        public static void SaveToAddressObject(object obj, string path)
        {
            SerializationCommon.SerializeObject(obj, path);
        }
    }
}
