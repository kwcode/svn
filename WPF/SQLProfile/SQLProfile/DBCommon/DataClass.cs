using System;
using System.Collections.Generic;
  
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
        /// 获取 output 值, 本属性在执行非常频繁时存在线程污染错误, 请使用 out 重载代替
        /// </summary>
        [Obsolete]
        public Hashtable OutputValues = new Hashtable();

        /// <summary>
        /// 存储过程参数缓存
        /// </summary>
        private Dictionary<string, SqlParameter[]> _SPParametersCache = new Dictionary<string, SqlParameter[]>();
        #region 重载
        public DataClass()
        {

        }
        #endregion




        #region 公共方法
        /// <summary>
        /// 执行存储过程，返回DataTable。
        /// (TableName = "yes" 表示执行成功，
        /// 否则 TableName 为错误信息)
        /// </summary>
        /// <param name="storedProcedureName">存储过程名字</param>
        /// <param name="paraValues">参数</param>
        /// <returns></returns>
        public DataTable ExecuteDataTable(string storedProcedureName, params object[] paraValues)
        {
            DataTable result;
            lock (this.conn)
            {
                using (SqlCommand sqlCommand = this.CreateSqlCommand(this.conn, storedProcedureName))
                {
                    try
                    {
                        sqlCommand.CommandTimeout = this.ExecTimeOut;
                        // 检索sqlCommand 中的参数添加到sqlCommand.Parameters 中
                        this.DeriveParameters(sqlCommand, storedProcedureName);
                        /// 1、检查参数
                        /// 2、填充对应的参数
                        /// 3、判断存储过程返回值参数
                        bool outExist = this.AssignParameterValues(sqlCommand, storedProcedureName, paraValues);
                        SqlDataAdapter adapter = new SqlDataAdapter(sqlCommand);
                        DataTable dataTable = new DataTable("yes");
                        adapter.Fill(dataTable);//填充
                        adapter.Dispose();
                        if (outExist)//如果out值
                        {
                            this.SetOutputValues(sqlCommand);
                        }
                        result = dataTable;
                    }
                    catch (Exception ex)
                    {
                        //this.errLog(storedProcedureName, ex, paraValues);
                        //base.msg(storedProcedureName + " execute error:" + ex.Message);
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
        /// <summary>
        /// 执行存储过程，返回DataTable。
        /// (TableName = "yes" 表示执行成功，
        /// 否则 TableName 为错误信息)
        /// </summary>
        /// <param name="storedProcedureName">存储过程名字</param>
        /// <param name="outputValues">out 集合</param>
        /// <param name="paraValues">参数</param>
        /// <returns></returns>
        public DataTable ExecuteDataTable(string storedProcedureName, out Hashtable output, params object[] paraValues)
        {
            DataTable result;
            lock (this.conn)
            {
                using (SqlCommand sqlCommand = this.CreateSqlCommand(this.conn, storedProcedureName))
                {
                    try
                    {
                        sqlCommand.CommandTimeout = this.ExecTimeOut;
                        this.DeriveParameters(sqlCommand, storedProcedureName);
                        this.AssignParameterValues(sqlCommand, storedProcedureName, paraValues);
                        SqlDataAdapter adapter = new SqlDataAdapter(sqlCommand);
                        DataTable dataTable = new DataTable("yes");
                        adapter.Fill(dataTable);
                        output = this.SetOutputValues(sqlCommand);
                        adapter.Dispose();
                        result = dataTable;
                    }
                    catch (Exception ex)
                    {
                        output = new Hashtable();
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
        /// <summary>
        ///  执行存储过程 
        /// </summary> 
        /// <returns>影响行数</returns>
        public int ExecuteNonQuery(string storedProcedureName, object[] paraValues)
        {
            int result;
            lock (this.conn)
            {
                using (SqlCommand sqlCommand = this.CreateSqlCommand(this.conn, storedProcedureName))
                {
                    sqlCommand.CommandTimeout = this.ExecTimeOut;
                    try
                    {
                        this.DeriveParameters(sqlCommand, storedProcedureName);
                        bool outExist = this.AssignParameterValues(sqlCommand, storedProcedureName, paraValues);
                        int affectedRowsCount = sqlCommand.ExecuteNonQuery();
                        //获取存储过程返回值。
                        // returnValue = (int)sqlCommand.Parameters["@RETURN_VALUE"].Value;

                        if (outExist)
                        {
                            this.SetOutputValues(sqlCommand);
                        }
                        result = affectedRowsCount;
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
        public int ExecuteNonQuery(string storedProcedureName, out Hashtable output, object[] paraValues)
        {
            int result;
            lock (this.conn)
            {
                using (SqlCommand sqlCommand = this.CreateSqlCommand(this.conn, storedProcedureName))
                {
                    sqlCommand.CommandTimeout = this.ExecTimeOut;
                    try
                    {
                        this.DeriveParameters(sqlCommand, storedProcedureName);
                        int affectedRowsCount = sqlCommand.ExecuteNonQuery();
                        //获取存储过程返回值。
                        // this.returnValue = (int)sqlCommand.Parameters["@RETURN_VALUE"].Value; 
                        this.AssignParameterValues(sqlCommand, storedProcedureName, paraValues);
                        output = SetOutputValues(sqlCommand);
                        result = affectedRowsCount;
                    }
                    catch (Exception ex)
                    {
                        output = new Hashtable();
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

        internal DataTable[] ExecuteDataTables(string storedProcedureName, object[] paraValues)
        {
            DataTable[] result;
            lock (this.conn)
            {
                using (SqlCommand sqlCommand = this.CreateSqlCommand(this.conn, storedProcedureName))
                {
                    try
                    {
                        sqlCommand.CommandTimeout = this.ExecTimeOut;
                        this.DeriveParameters(sqlCommand, storedProcedureName);
                        bool outExist = this.AssignParameterValues(sqlCommand, storedProcedureName, paraValues);
                        SqlDataAdapter adapter = new SqlDataAdapter(sqlCommand);
                        DataSet ds = new DataSet("yes");
                        adapter.Fill(ds);
                        DataTable[] dataTable = new DataTable[ds.Tables.Count];
                        for (int i = 0; i < dataTable.Length; i++)
                        {
                            dataTable[i] = ds.Tables[i];
                            if (i == 0)
                            {
                                dataTable[0].TableName = "yes";
                            }
                        }
                        adapter.Dispose();
                        if (outExist)
                        {
                            this.SetOutputValues(sqlCommand);
                        }
                        result = dataTable;
                    }
                    catch (Exception ex)
                    {
                        result = new DataTable[]
						{
							new DataTable(ex.Message)
						};
                    }
                    finally
                    {
                        sqlCommand.Parameters.Clear();
                    }
                }
            }
            return result;
        }
        internal DataTable[] ExecuteDataTables(string storedProcedureName, out Hashtable outputValues, object[] paraValues)
        {
            DataTable[] result;
            lock (this.conn)
            {
                using (SqlCommand sqlCommand = this.CreateSqlCommand(this.conn, storedProcedureName))
                {
                    try
                    {
                        sqlCommand.CommandTimeout = this.ExecTimeOut;
                        this.DeriveParameters(sqlCommand, storedProcedureName);
                        this.AssignParameterValues(sqlCommand, storedProcedureName, paraValues);
                        SqlDataAdapter adapter = new SqlDataAdapter(sqlCommand);
                        DataSet ds = new DataSet("yes");
                        adapter.Fill(ds);
                        DataTable[] dataTable = new DataTable[ds.Tables.Count];
                        for (int i = 0; i < dataTable.Length; i++)
                        {
                            dataTable[i] = ds.Tables[i];
                            if (i == 0)
                            {
                                dataTable[0].TableName = "yes";
                            }
                        }
                        outputValues = SetOutputValues(sqlCommand);
                        adapter.Dispose();
                        result = dataTable;
                    }
                    catch (Exception ex)
                    {
                        outputValues = new Hashtable();

                        result = new DataTable[]
						{
							new DataTable(ex.Message)
                        };
                    }
                    finally
                    {
                        sqlCommand.Parameters.Clear();
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 打开数据连接连接
        /// </summary>
        /// <param name="needReConn"></param>
        /// <returns></returns>
        public bool OpenConn(bool needReConn)
        {
            if (string.IsNullOrEmpty(_ConnectionString))
            {
                return false;
            }
            this.needReConnect = needReConn;
            if (this.conn.State != ConnectionState.Open)
            {
                try
                {
                    if (string.IsNullOrEmpty(this.conn.ConnectionString))
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

                catch (Exception ex)
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
        #endregion

        #region 私有方法
        /// <summary>
        /// 创建对Sql-执行对象
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="storedProcedureName"></param>
        /// <returns></returns>
        private SqlCommand CreateSqlCommand(SqlConnection connection, string storedProcedureName)
        {
            return new SqlCommand(storedProcedureName, connection)
            {
                //需要执行的类型
                CommandType = CommandType.StoredProcedure,//存储过程的名称
                CommandTimeout = this.ExecTimeOut
            };
        }

        /// <summary>
        /// 检索sqlCommand 中的参数添加到sqlCommand.Parameters 中
        /// </summary>
        /// <param name="sqlCommand"></param>
        /// <param name="storedProcedureName">储存过程名字</param>
        private void DeriveParameters(SqlCommand sqlCommand, string storedProcedureName)
        {
            string key = storedProcedureName.ToLower();//存储过程转小写
            SqlParameter[] paramerters = null;//参数列表
            //这里用一个列表来缓存 参数 下一次先从缓存里面找
            if (this._SPParametersCache.TryGetValue(key, out paramerters))
            {
                //如果存在 在sqlCommand 加参数 并直接返回
                sqlCommand.Parameters.AddRange(paramerters);
                return;
            }

            #region 如果不存在
            //将sqlCommand的参数sqlCommand 到sqlCommand.Parameters 集合里面
            SqlCommandBuilder.DeriveParameters(sqlCommand);
            //
            int length = sqlCommand.Parameters.Count;
            paramerters = new SqlParameter[length];//初始化列表
            for (int i = 0; i < length; i++)
            {
                SqlParameter item = sqlCommand.Parameters[i];
                paramerters[i] = item;
                //paramerters[i] = new SqlParameter(item.ParameterName, item.SqlDbType, item.Size);
                //paramerters[i].Precision = item.Precision;
                //paramerters[i].Direction = item.Direction;
                //paramerters[i].IsNullable = item.IsNullable;
                //paramerters[i].Scale = item.Scale;
            }
            lock (this._SPParametersCache)
            {
                this._SPParametersCache[key] = paramerters;//加入缓存
            }
            #endregion
        }

        /// <summary>
        /// 1、检查参数
        /// 2、填充对应的参数
        /// 3、判断存储过程返回值参数
        /// 
        /// </summary>
        /// <param name="sqlCommand"></param>
        /// <param name="storedProcedureName"></param>
        /// <param name="paraValues"></param>
        /// <returns>存储过程是否有返回值参数</returns>
        private bool AssignParameterValues(SqlCommand sqlCommand, string storedProcedureName, object[] paraValues)
        {
            bool outExist = false;
            if (paraValues != null)
            {
                //sqlCommand.Parameters 默认会有一个@RETURN_VALUE
                int pCont = sqlCommand.Parameters.Count - 1;//获取参数的个数
                #region 参数不匹配
                if (pCont != paraValues.Length)
                {
                    string str = string.Empty;
                    for (int i = 0; i < paraValues.Length; i++)
                    {
                        if (i == 0)
                        {
                            str += paraValues[i].ToString();
                        }
                        else
                        {
                            str = str + "," + paraValues[i].ToString();
                        }
                    }
                    string errStr = string.Concat(new string[]{
                    "执行存储过程【",
                    storedProcedureName,
                    "】错误信息：传递的参数与存储过程参数不匹配。\r\n",
                    "正确数量是：",
                    pCont.ToString(),
                    "\r\n 当前传入的参数：(",
                    paraValues.Length.ToString(),
                    ")",
                    str
                    });
                    throw new Exception(errStr);
                }
                #endregion

                #region 匹配
                for (int i = 0; i < paraValues.Length; i++)
                {
                    object v = paraValues[i];//得到参数
                    SqlParameter parameter = sqlCommand.Parameters[i + 1];
                    //指定表值参数中包含的构造数据的特殊数据类型
                    if (parameter.SqlDbType == SqlDbType.Structured)
                    {
                        //Q sqlCommand.Parameters[i + 1] = new SqlParameter(parameter.ParameterName,);
                        //我的
                        sqlCommand.Parameters[i + 1].SqlValue = v;
                    }
                    else
                    {
                        //参数是只可输入、只可输出、双向还是存储过程返回值参数
                        if (parameter.Direction == ParameterDirection.Output || parameter.Direction == ParameterDirection.InputOutput)
                        {
                            outExist = true;//该存储过程有返回的参数 如：Out @Count
                        }
                        if (v == null)
                        {
                            sqlCommand.Parameters[i + 1].Value = DBNull.Value;
                        }
                        else
                        {
                            if (v is string)//字符串
                            {
                                string temp = v as string;
                                #region 输出参数 如：out @Count
                                if ((parameter.Direction == ParameterDirection.Output || parameter.Direction == ParameterDirection.InputOutput) && temp == "OUTPUT")
                                {
                                    parameter.Value = DBNull.Value;
                                }
                                #endregion

                                #region 正常的参数 @ID
                                else
                                {
                                    #region bool
                                    if (parameter.SqlDbType == SqlDbType.Bit)
                                    {
                                        bool b;
                                        if (bool.TryParse(temp, out b))
                                            v = b;
                                        else
                                            v = !temp.Equals("0", StringComparison.OrdinalIgnoreCase);//false
                                    }
                                    #endregion
                                    #region GUID 全局唯一标识符
                                    else
                                    {
                                        if (parameter.SqlDbType == SqlDbType.UniqueIdentifier)
                                            v = new Guid(temp);
                                    }
                                    #endregion
                                }
                                #endregion
                                //赋值
                                sqlCommand.Parameters[i + 1].Value = v;
                            }
                            else
                            {     //赋值
                                sqlCommand.Parameters[i + 1].Value = v;
                            }
                        }
                    }
                }
                #endregion
            }
            return outExist;
        }

        /// <summary>
        /// 设置 out 参数返回集合
        /// </summary>
        /// <param name="sqlCommand"></param>
        /// <returns></returns>
        private Hashtable SetOutputValues(SqlCommand sqlCommand)
        {
            Hashtable outputTable = new Hashtable();
            foreach (SqlParameter para in sqlCommand.Parameters)
            {
                if (para.Direction == ParameterDirection.Output || para.Direction == ParameterDirection.InputOutput)
                {
                    outputTable.Add(para.ParameterName, para.Value);
                }
            }
            this.OutputValues = outputTable;
            return outputTable;
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
        #endregion

        public DataTable GetDataTable(string sqlstr, object[] values)
        {
            DataTable result;
            lock (this.conn)
            {
                string cmd = GetSqlCmdStr(sqlstr, values);
                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    try
                    {
                        sqlCommand.CommandText = cmd;
                        sqlCommand.Connection = this.conn;
                        sqlCommand.CommandTimeout = this.ExecTimeOut;
                        SqlDataAdapter adapter = new SqlDataAdapter(sqlCommand);
                        DataTable dataTable = new DataTable("yes");
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
