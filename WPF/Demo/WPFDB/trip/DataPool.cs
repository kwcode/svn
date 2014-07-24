using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using trip.CommunalClass;
namespace trip
{
    public class DataPool : MessageProc
    {
        /// <summary>
        /// 连接池等待时间(ms), 默认值为 20000
        /// </summary>
        public static int waitFor = 20000;
        private Semaphore sema;
        private Stack<DataClass> dataConnPool;
        /// <summary>
        /// The count of DataPool
        /// </summary>
        public int Count
        {
            get
            {
                return this.dataConnPool.Count;
            }
        }
        protected override string msgType()
        {
            return "DataPool";
        }
        /// <summary>
        /// 初始化数据池对象
        /// </summary>
        /// <param name="capacity">池大小</param>
        /// <param name="connString">数据库连接字符串</param>
        public DataPool(int capacity, string connString)
        {
            this.sema = new Semaphore(capacity, capacity);
            this.dataConnPool = new Stack<DataClass>(capacity);
            int i = 0;
            while (i < capacity)
            {
                DataClass dc = new DataClass();
                dc.CmdTimeOut = 20;
                dc.noticeEvent += new MessageProc.myNoticeEvent(base.msg);
                if (dc.conncetion == null)
                {

                }
                if (dc.conncetion.State != ConnectionState.Open)
                {

                    //dc.conncetion.Open();
                }
                this.dataConnPool.Push(dc); 
                try
                {
                    dc.ConnectionString = connString;
                }
                catch (Exception ex)
                {
                    LogsRecord.write("sqlConnErr", ex.ToString());
                    LogsRecord.write("sqlConnErr", "ConnectionString:" + connString);
                }
                dc.connOpen(true);
                i++;
                continue;
            }
        }
        /// <summary>
        /// 得到一个 DataClass 对象
        /// </summary>
        /// <returns></returns>
        public DataClass getDataClass()
        {
            if (!this.sema.WaitOne(DataPool.waitFor))
            {
                throw new Exception("Data connection in the data pool is run out!");
            }
            DataClass result;
            lock (this.dataConnPool)
            {
                result = this.dataConnPool.Pop();
            }
            return result;
        }
        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        private int execSql(string sql)
        {
            DataClass dc = this.getDataClass();
            int res = -2;
            try
            {
                res = dc.myCmd(sql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.Push(dc);
            }
            return res;
        }
        /// <summary>
        /// 将对象存入池
        /// </summary>
        /// <param name="dc"></param>
        public void Push(DataClass dc)
        {
            lock (this.dataConnPool)
            {
                this.dataConnPool.Push(dc);
            }
            this.sema.Release();
        }
        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="SPName"></param>
        /// <param name="parm"></param>
        /// <returns>影响行数</returns>
        public int execSP(string SPName, params object[] parm)
        {
            int arows = -1;
            DataClass dc = this.getDataClass();
            try
            {
                arows = dc.sp.ExecuteNonQuery(SPName, parm);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.Push(dc);
            }
            return arows;
        }
        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="SPName"></param>
        /// <param name="parm"></param>
        /// <returns>影响行数</returns>
        public int execSP(string SPName, out Hashtable outputValues, params object[] parm)
        {
            int arows = -1;
            DataClass dc = this.getDataClass();
            try
            {
                arows = dc.sp.ExecuteNonQuery(SPName, out outputValues, parm);
            }
            catch (Exception ex)
            {
                outputValues = new Hashtable();
                throw ex;
            }
            finally
            {
                this.Push(dc);
            }
            return arows;
        }
        /// <summary>
        /// 执行存储过程，返回 System.Data.DataTable。(TableName = "yes" 表示执行成功，否则 TableName 为错误信息)
        /// </summary>
        /// <param name="storedProcedureName">SP name</param>
        /// <param name="paraValues">Params</param>
        /// <returns></returns>
        public DataTable ExecuteDataTable(string storedProcedureName, params object[] paraValues)
        {
            DataTable dt = null;
            DataClass dc = this.getDataClass();
            try
            {
                dt = dc.sp.ExecuteDataTable(storedProcedureName, paraValues);
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
        /// 执行存储过程，返回 System.Data.DataTable。(TableName = "yes" 表示执行成功，否则 TableName 为错误信息)
        /// </summary>
        /// <param name="storedProcedureName">SP name</param>
        /// <param name="outputValues"></param>
        /// <param name="paraValues">Params</param>
        /// <returns></returns>
        public DataTable ExecuteDataTable(string storedProcedureName, out Hashtable outputValues, params object[] paraValues)
        {
            DataTable dt = null;
            DataClass dc = this.getDataClass();
            try
            {
                dt = dc.sp.ExecuteDataTable(storedProcedureName, out outputValues, paraValues);
            }
            catch (Exception ex)
            {
                outputValues = new Hashtable();
                return new DataTable(ex.Message);
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
        public DataTable[] ExecuteDataTables(string storedProcedureName, params object[] paraValues)
        {
            DataClass dc = this.getDataClass();
            DataTable[] dt;
            try
            {
                dt = dc.sp.ExecuteDataTables(storedProcedureName, paraValues);
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
            DataClass dc = this.getDataClass();
            DataTable[] dt;
            try
            {
                dt = dc.sp.ExecuteDataTables(storedProcedureName, out outputValues, paraValues);
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
        /// <summary>
        /// 执行存储过程，返回 sqlDataReader
        /// </summary>
        /// <param name="storedProcedureName">SP name</param>
        /// <param name="paraValues">Params</param>
        /// <returns></returns>
        public SqlDataReaderCMD ExecuteDataReader(string storedProcedureName, params object[] paraValues)
        {
            SqlDataReader dt = null;
            DataClass dc = this.getDataClass();
            SqlDataReaderCMD result;
            try
            {
                dt = dc.sp.ExecuteDataReader(storedProcedureName, paraValues);
                SqlDataReaderCMD msd = new SqlDataReaderCMD(this, dc, dt);
                result = msd;
            }
            catch (Exception ex)
            {
                if (dt != null && !dt.IsClosed)
                {
                    dt.Close();
                }
                this.Push(dc);
                throw ex;
            }
            return result;
        }
    }
}
