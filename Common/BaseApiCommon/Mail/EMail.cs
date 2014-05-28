using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BaseApiCommon.Mail
{
    /// <summary>
    /// 邮件管理相关属性
    /// </summary>
    public class EMail
    {
        /// <summary>
        /// 唯一标识号
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 登录帐号
        /// </summary>
        public string LoginName { get; set; }
        /// <summary>
        /// 登录密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 发送人（通常是登录帐号邮箱）
        /// </summary>
        public string FromAddress { get; set; }
        /// <summary>
        /// 接收者地址
        /// </summary>
        public string[] ToAddress { get; set; }
        /// <summary>
        /// 发送邮件内容
        /// </summary>
        public string Body { get; set; }
        /// <summary>
        /// 邮件主题
        /// </summary>
        public string Subject { get; set; }
        /// <summary>
        /// Smtp服务
        /// </summary>
        public string SmtpServerAddress { get; set; }
        /// <summary>
        /// Smtp端口
        /// </summary>
        public int SmtpPort { get; set; }
        /// <summary>
        /// Pop3服务地址
        /// </summary>
        public string PopServerAddress { get; set; }

    }
}
