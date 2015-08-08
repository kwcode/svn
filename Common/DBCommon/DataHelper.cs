using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DBCommon
{
    public class DataHelper
    {
        private string _ConnectionString;
        private SqlConnection conn = new SqlConnection();

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="connString">数据库连接字符串</param>
        public DataHelper(string connString)
        {
            _ConnectionString = connString;
            if (conn.State != ConnectionState.Open)
            {
                try
                {
                    this.conn.ConnectionString = this._ConnectionString;
                    this.conn.Open();
                }
                catch (Exception ex)
                {

                }
            }
        }


    }
}
