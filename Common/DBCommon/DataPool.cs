using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Data;
using System.Collections;
using System.Data.Common;

namespace DBCommon
{

    /// <summary>
    /// 数据连接池
    /// </summary>
    public class DataPool
    {
        /// <summary>
        ///  连接池等待时间(ms), 默认值为 20000
        /// </summary>
        public static int WaitFor = 20000;
        /// <summary>
        /// 信号量的初始请求数
        /// </summary>
        private Semaphore _semap;
        /// <summary>
        /// 数据连接池
        /// </summary>
        private Stack<DataClass> dataConnPool;


        #region 私有方法
        /// <summary>
        /// 将数据连接对象存入池
        /// </summary>
        /// <param name="dc">数据连接对象</param>
        public void Push(DataClass dc)
        {
            lock (dataConnPool)
            {
                this.dataConnPool.Push(dc);
            }
            this._semap.Release();
        }
        /// <summary>
        /// 得到一个 DataClass 对象
        /// </summary>
        /// <returns></returns>
        private DataClass GetDataClass()
        {
            if (!this._semap.WaitOne(WaitFor))
            {
                throw new Exception("数据连接池的数据连接耗尽！");
                //throw new Exception("Data connection in the data pool is run out!");
            }
            DataClass result;
            lock (this.dataConnPool)
            {
                result = this.dataConnPool.Pop();
            }
            return result;
        }

        #endregion

        #region 公共方法

        #region 执行储存过程

        /// <summary>
        /// 初始化数据池对象
        /// </summary>
        /// <param name="capacity">池大小</param>
        /// <param name="connString">数据库连接字符串</param>
        public DataPool(int capacity, string connString)
        {
            _semap = new Semaphore(capacity, capacity);
            this.dataConnPool = new Stack<DataClass>(capacity);
            int i = 0;
            while (i < capacity)
            {
                DataClass dc = new DataClass();
                //dc.CmdTimeOut = 20;
                this.dataConnPool.Push(dc);
                try
                {
                    dc.ConnectionString = connString;
                }
                catch (Exception ex)
                {
                    //     LogsRecord.write("sqlConnErr", ex.ToString());
                    //LogsRecord.write("sqlConnErr", "ConnectionString:" + connString);
                    throw new Exception(ex.Message);
                }
                dc.OpenConn(true);
                i++;
                continue;
            }
        }

        /// <summary>
        /// 执行存储过程,
        /// 
        /// </summary>
        /// <param name="storedProcedureName">存储过程</param>
        /// <param name="paraValues">参数</param>
        /// <returns>返回 System.Data.DataTable。
        /// (TableName = "yes" 表示执行成功，
        /// 否则 TableName 为错误信息)</returns>
        public DataTable ExecuteDataTable(string storedProcedureName, params object[] paraValues)
        {
            DataTable dt = null;
            DataClass dc = this.GetDataClass();
            try
            {
                dt = dc.ExecuteDataTable(storedProcedureName, paraValues);
            }
            catch (Exception ex)
            {
                return new DataTable(ex.Message);
            }
            finally
            {
                this.Push(dc);
            }
            return dt;
        }
        public DataTable ExecuteDataTable(string storedProcedureName, out Hashtable output, params object[] paraValues)
        {
            DataTable dt = null;
            DataClass dc = this.GetDataClass();
            try
            {
                dt = dc.ExecuteDataTable(storedProcedureName, out output, paraValues);
            }
            catch (Exception ex)
            {
                output = new Hashtable();
                return new DataTable(ex.Message);
            }
            finally
            {
                this.Push(dc);
            }
            return dt;
        }
        /// <summary>
        /// 执行存储过程
        /// </summary> 
        /// <returns>影响行数</returns>
        public int ExecuteSP(string storedProcedureName, params object[] paraValues)
        {
            int rows = -1;
            DataClass dc = this.GetDataClass();
            try
            {
                rows = dc.ExecuteNonQuery(storedProcedureName, paraValues);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.Push(dc);
            }
            return rows;
        }
        /// <summary>
        /// 执行存储过程
        /// </summary> 
        /// <returns>影响行数</returns>
        public int ExecuteSP(string storedProcedureName, out Hashtable output, params object[] paraValues)
        {
            int rows = -1;
            DataClass dc = this.GetDataClass();
            try
            {
                rows = dc.ExecuteNonQuery(storedProcedureName, out output, paraValues);
            }
            catch (Exception ex)
            {
                output = new Hashtable();
                throw ex;
            }
            finally
            {
                this.Push(dc);
            }
            return rows;
        }

        /// <summary>
        /// 执行存储过程，返回 System.Data.DataTable[]
        /// </summary>
        /// <param name="storedProcedureName">SP name</param>
        /// <param name="paraValues">Params</param>
        /// <returns></returns>
        public DataTable[] ExecuteDataTables(string storedProcedureName, params object[] paraValues)
        {
            DataClass dc = this.GetDataClass();
            DataTable[] dt;
            try
            {
                dt = dc.ExecuteDataTables(storedProcedureName, paraValues);
            }
            catch (Exception ex)
            {
                return new DataTable[]
				{
					new DataTable(ex.Message)
				};
            }
            finally
            {
                this.Push(dc);
            }
            return dt;
        }
        /// <summary>
        /// 执行存储过程，返回 System.Data.DataTable[]
        /// </summary>
        /// <param name="storedProcedureName">SP name</param>
        /// <param name="paraValues">Params</param>
        /// <returns></returns>
        public DataTable[] ExecuteDataTables(string storedProcedureName, out Hashtable outputValues, params object[] paraValues)
        {
            DataClass dc = this.GetDataClass();
            DataTable[] dt;
            try
            {
                dt = dc.ExecuteDataTables(storedProcedureName, out outputValues, paraValues);
            }
            catch (Exception ex)
            {
                outputValues = new Hashtable();
                return new DataTable[]
				{
					new DataTable(ex.Message)
				};
            }
            finally
            {
                this.Push(dc);
            }
            return dt;
        }

        #endregion

        #region 执行Sql语句

        public DataTable GetDataTable(string sqlstr, params object[] values)
        {
            DataTable dt = null;
            DataClass dc = this.GetDataClass();
            try
            {
                dt = dc.GetDataTable(sqlstr, values);
            }
            catch (Exception ex)
            {
                return new DataTable(ex.Message);
            }
            finally
            {
                this.Push(dc);
            }
            return dt;
        }
        /// <summary>
        /// 执行sql语句返回影响行数
        /// </summary>
        /// <param name="sqlstr"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public int ExecuteSQL(string sqlstr, params object[] values)
        {
            int rows = -1;
            DataClass dc = this.GetDataClass();
            try
            {
                rows = dc.ExecuteSQL(sqlstr, values);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.Push(dc);
            }
            return rows;
        }
        #endregion
        #endregion
    }
}
