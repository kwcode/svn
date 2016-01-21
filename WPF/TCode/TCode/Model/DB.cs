using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TCode.Model
{
    [Serializable]
    public class DB
    {
        /// <summary>
        /// 服务器名称
        /// </summary>
        public string ServiceName { get; set; }
        /// <summary>
        /// 登录ID
        /// </summary>
        public string UserID { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
    }
}
