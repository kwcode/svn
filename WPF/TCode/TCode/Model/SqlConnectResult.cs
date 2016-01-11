using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TCode
{
    /// <summary> 
    /// </summary>
    public class SqlConnectResult
    {
        /// <summary>
        /// 是否就Sql Service 登陆
        /// true:"user id=" + user + ";password=" + pass + ";initial catalog=" + dbName + ";data source=" + server;
        /// false:"Integrated Security=SSPI;Data Source=" + server + ";Initial Catalog=" + dbName;
        /// </summary>
        public bool IsSqlService { get; set; }
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

        /// <summary>
        /// 全部的时候用master
        /// 数据库名称 
        /// </summary>
        public string DbName { get; set; }
        /// <summary>
        /// 是否所有数据库
        /// </summary>
        public bool IsAll { get; set; }
    }
}
