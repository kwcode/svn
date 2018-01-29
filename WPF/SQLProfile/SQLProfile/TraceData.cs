using System;
using System.Collections.Generic;
using System.Text;

namespace SQLProfile
{
    public class TraceData
    {
        /// <summary>
        /// 事件名称
        /// </summary>
        public string EventClass { get; set; }
        /// <summary>
        /// 执行内容
        /// </summary>
        public string TextData { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string LoginName { get; set; }
        /// <summary>
        /// Sql为客户端的相关进程分配的服务器进程ID
        /// </summary>
        public int SPID { get; set; }
        /// <summary>
        /// 事件时间
        /// 微秒计算
        /// </summary>
        public long Duration { get; set; }
        public string DurationStr { get; set; }
        /// <summary>
        /// 连接的客户端程序
        /// 程序名称
        /// </summary>
        public string ApplicationName { get; set; }
        /// <summary>
        /// 事件启动时间
        /// </summary>
        public DateTime? StartTime { get; set; }
        /// <summary>
        /// 时间结束的时间
        /// </summary>
        public DateTime? EndTime { get; set; }
        /// <summary>
        /// 事件使用的CUP时间（毫秒）
        /// </summary>
        public int CPU { get; set; }
        /// <summary>
        /// 调用SQL Service 的应用程序的进程ID
        /// </summary>
        public int ClientProcessID { get; set; }
        /// <summary>
        /// Windows 用户名
        /// </summary>
        public string NTUserName { get; set; }
        /// <summary>
        /// 读取逻辑磁盘的次数
        /// </summary>
        public long Reads { get; set; }
        /// <summary>
        /// 写入物理磁盘的次数
        /// </summary>
        public long Writes { get; set; }
        /// <summary>
        /// 正在执行用户语句的数据库名称
        /// </summary>
        public string DatabaseName { get; set; }
        /// <summary>
        /// 正在运行客户端的计算机
        /// </summary>
        public string HostName { get; set; }
        /// <summary>
        /// 系统分配的事务ID
        /// </summary>
        public long TransactionID { get; set; }
    }
}
