using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Data.Common;

namespace DBCommon
{
    public class DataClass
    {
        /// <summary>
        /// 是否需要打开连接
        /// </summary>
        public bool needReConnect { get; set; }
        private SqlConnection conn = new SqlConnection();
        private string _ConnectionString;
        public string ConnectionString
        {
            get
            {
                return this._ConnectionString;
            }
            set
            {
                this._ConnectionString = value;
            }
        }
        /// <summary>
        ///  等待命令执行的时间（以秒为单位）。默认值为 30 秒。
        /// </summary>
        public int ExecTimeOut;

        /// <summary>
        /// 打开数据连接连接
        /// </summary>
        /// <param name="needReConn"></param>
        /// <returns></returns>
        public bool OpenConn(bool needReConn)
        {
            if (string.IsNullOrWhiteSpace(_ConnectionString))
            {
                return false;
            }
            this.needReConnect = needReConn;
            if (this.conn.State != ConnectionState.Open)
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(this.conn.ConnectionString))
                    {
                        //Assembly asm = Assembly.GetEntryAssembly();
                        //if (asm == null)
                        //{
                        //    asm = Assembly.GetCallingAssembly();
                        //}
                        this.conn.ConnectionString = this._ConnectionString;
                    }
                    this.conn.Open();
                    return true;
                }

                catch
                {
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// 关闭连接
        /// </summary>
        public void CloseConn()
        {
            this.needReConnect = false;
            if (this.conn.State != ConnectionState.Closed)
            {
                this.conn.Close();
                this.conn.Dispose();
            }
        }
        private string GetSqlCmdStr(string sqlstr, object[] values)
        {
            string sql = sqlstr;
            if (values != null)
            {
                int length = values.Length;
                string[] parms = new string[length];
                for (int i = 0; i < length; i++)
                {
                    parms[i] = "'" + values[i] + "'";
                }
                sql = string.Format(sqlstr, parms);
            }
            return sql;
        }
        public DataTable GetDataTable(string sqlstr, string dbName = "yes")
        {
            DataTable result;
            lock (this.conn)
            {
                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    try
                    {
                        sqlCommand.CommandText = sqlstr;
                        sqlCommand.Connection = this.conn;
                        sqlCommand.CommandTimeout = this.ExecTimeOut;
                        SqlDataAdapter adapter = new SqlDataAdapter(sqlCommand);
                        DataTable dataTable = new DataTable(dbName);
                        adapter.Fill(dataTable);
                        adapter.Dispose();
                        result = dataTable;
                    }
                    catch (Exception ex)
                    {
                        result = new DataTable(ex.Message);
                    }
                    finally
                    {
                        sqlCommand.Parameters.Clear();
                    }
                }
            }
            return result;
        }
        internal int ExecuteSQL(string sqlstr, object[] values)
        {
            int result;
            lock (this.conn)
            {
                string cmd = GetSqlCmdStr(sqlstr, values);
                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    sqlCommand.CommandTimeout = this.ExecTimeOut;
                    try
                    {
                        sqlCommand.CommandText = cmd;
                        sqlCommand.Connection = this.conn;
                        result = sqlCommand.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {

                        throw ex;
                    }
                    finally
                    {
                        sqlCommand.Parameters.Clear();
                    }
                }
            }
            return result;
        }
    }
}
