using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using trip.CommunalClass;
namespace trip
{
	/// <summary>
	/// 存储过程的调用类
	/// </summary>
	public class SPClass : MessageProc
	{
		private const string logName = "sqlSPerr";
		private int returnValue;
		private SqlConnection connection;
		public int ExecTimeOut;
		/// <summary>
		/// 获取 output 值, 本属性在执行非常频繁时存在线程污染错误, 请使用 out 重载代替
		/// </summary>
		[Obsolete]
		public Hashtable OutputValues = new Hashtable();
		/// <summary>
		/// 存储过程参数缓存
		/// </summary>
		private Dictionary<string, SqlParameter[]> SPParametersCache = new Dictionary<string, SqlParameter[]>();
		/// <summary>
		/// 获取或设置数据库连接。
		/// </summary>
		internal SqlConnection sqlConnection
		{
			get
			{
				return this.connection;
			}
			set
			{
				this.connection = value;
			}
		}
		/// <summary>
		/// 获取存储过程返回值。
		/// int ret = sp.ReturnValue;
		/// </summary>
		public int ReturnValue
		{
			get
			{
				return this.returnValue;
			}
		}
		/// <summary>
		/// 初始化  StoredProceduer 对象。
		/// </summary>
		/// <param name="connectionString">数据库连接字符串。</param>
		/// <example>
		/// string conStr = "@Data Source=server;Initial Catalog=database;Integrated Security=true";
		/// StoredProcedure sp = new StoredProcedure(conStr);
		/// </example>
		public SPClass(SqlConnection conn)
		{
			this.connection = conn;
		}
		/// <summary>
		/// 初始化  StoredProceduer 对象。
		/// </summary>
		/// <param name="connectionString">数据库连接字符串。</param>
		/// <example>
		/// string conStr = "@Data Source=server;Initial Catalog=database;Integrated Security=true";
		/// StoredProcedure sp = new StoredProcedure(conStr);
		/// </example>
		public SPClass()
		{
		}
		private SqlCommand CreateSqlCommand(SqlConnection connection, string storedProcedureName)
		{
			return new SqlCommand(storedProcedureName, connection)
			{
				CommandType = CommandType.StoredProcedure, 
				CommandTimeout = this.ExecTimeOut
			};
		}
		private Hashtable setOutputValues(SqlCommand sqlCommand)
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
		protected override string msgType()
		{
			return "SPProcess";
		}
		private void errLog(string spname, Exception ex, params object[] prms)
		{
			StringBuilder a = new StringBuilder();
			a.Append("null,");
			if (prms != null)
			{
				for (int i = 0; i < prms.Length; i++)
				{
					object o = prms[i];
					a.AppendFormat("{0},", o);
				}
			}
			if (a.Length > 0)
			{
				a = a.Remove(a.Length - 1, 1);
			}
			LogsRecord.write("sqlSPerr", string.Concat(new string[]
			{
				"SP name: ", 
				spname, 
				"\r\nParams: ", 
				a.ToString(), 
				"\r\n", 
				ex.ToString()
			}));
		}
		/// <summary>
		/// 清除存储过程参数缓存
		/// </summary>
		public void ClearSPParameterCache()
		{
			lock (this.SPParametersCache)
			{
				this.SPParametersCache.Clear();
			}
		}
		/// <summary>
		/// 从在 System.Data.SqlClient.SqlCommand 中指定的存储过程中检索参数信息并填充指定的 
		/// System.Data.SqlClient.SqlCommand 对象的 System.Data.SqlClient.SqlCommand.Parameters 集合。
		/// </summary>
		/// <param name="sqlCommand">将从其中导出参数信息的存储过程的 System.Data.SqlClient.SqlCommand 对象。</param>
		internal void DeriveParameters(SqlCommand sqlCommand, string storedProcedureName)
		{
			string key = storedProcedureName.ToLower();
			SqlParameter[] paramerters = null;
			if (this.SPParametersCache.TryGetValue(key, out paramerters))
			{
				sqlCommand.Parameters.AddRange(paramerters);
				return;
			}
			SqlCommandBuilder.DeriveParameters(sqlCommand);
			int length = sqlCommand.Parameters.Count;
			paramerters = new SqlParameter[length];
			for (int i = 0; i < length; i++)
			{
				SqlParameter item = sqlCommand.Parameters[i];
				paramerters[i] = new SqlParameter(item.ParameterName, item.SqlDbType, item.Size);
				paramerters[i].Precision = item.Precision;
				paramerters[i].Direction = item.Direction;
				paramerters[i].IsNullable = item.IsNullable;
				paramerters[i].Scale = item.Scale;
			}
			lock (this.SPParametersCache)
			{
				this.SPParametersCache[key] = paramerters;
			}
		}
		/// <summary>
		/// 用指定的参数值列表为存储过程参数赋值。
		/// </summary>
		/// <param name="sqlCommand"></param>
		/// <param name="storedProcedureName"></param>
		/// <param name="paraValues"></param>
		/// <returns>是否存在Output参数</returns>
		private bool AssignParameterValues(SqlCommand sqlCommand, string storedProcedureName, params object[] paraValues)
		{
			bool outExist = false;
			if (paraValues != null)
			{
				int pCount = sqlCommand.Parameters.Count - 1;
				if (sqlCommand.Parameters.Count - 1 != paraValues.Length)
				{
					string paraString = "";
					for (int i = 0; i < paraValues.Length; i++)
					{
						if (i == 0)
						{
							paraString += paraValues[i].ToString();
						}
						else
						{
							paraString = paraString + "," + paraValues[i].ToString();
						}
					}
					string errStr = string.Concat(new string[]
					{
						"Execute SP [", 
						storedProcedureName, 
						"] error, Parameters' count is not right\r\nNeed count: ", 
						pCount.ToString(), 
						"\r\nTransmission parameters:(", 
						paraValues.Length.ToString(), 
						")", 
						paraString
					});
					LogsRecord.write("ExecSP err", errStr);
					base.msg("传递的参数与存储过程参数不匹配。");
					throw new Exception(errStr);
				}
				for (int j = 0; j < paraValues.Length; j++)
				{
					object v = paraValues[j];
					SqlParameter parameter = sqlCommand.Parameters[j + 1];
					if (parameter.SqlDbType == SqlDbType.Structured)
					{
						sqlCommand.Parameters[j + 1] = new SqlParameter(parameter.ParameterName, v);
					}
					else
					{
						if (parameter.Direction == ParameterDirection.Output || parameter.Direction == ParameterDirection.InputOutput)
						{
							outExist = true;
						}
						if (v == null)
						{
							sqlCommand.Parameters[j + 1].Value = DBNull.Value;
						}
						else
						{
							if (v is string)
							{
								string temp = v as string;
								if ((parameter.Direction == ParameterDirection.Output || parameter.Direction == ParameterDirection.InputOutput) && temp == "OUTPUT")
								{
									parameter.Value = DBNull.Value;
								}
								else
								{
									if (parameter.SqlDbType == SqlDbType.Bit)
									{
										bool b;
										if (bool.TryParse(temp, out b))
										{
											v = b;
										}
										else
										{
											v = !temp.Equals("0", StringComparison.OrdinalIgnoreCase);
										}
									}
									else
									{
										if (parameter.SqlDbType == SqlDbType.UniqueIdentifier)
										{
											v = new Guid(temp);
										}
									}
									sqlCommand.Parameters[j + 1].Value = v;
								}
							}
							else
							{
								sqlCommand.Parameters[j + 1].Value = v;
							}
						}
					}
				}
			}
			return outExist;
		}
		/// <summary>
		/// 执行存储过程
		/// </summary>
		/// <param name="storedProcedureName">存储过程名称</param>
		/// <param name="paraValues">参数</param>
		/// <returns>影响行数</returns>
		public int ExecuteNonQuery(string storedProcedureName, params object[] paraValues)
		{
			int result;
			lock (this.connection)
			{
				using (SqlCommand sqlCommand = this.CreateSqlCommand(this.connection, storedProcedureName))
				{
					sqlCommand.CommandTimeout = this.ExecTimeOut;
					try
					{
						this.DeriveParameters(sqlCommand, storedProcedureName);
						bool outExist = this.AssignParameterValues(sqlCommand, storedProcedureName, paraValues);
						int affectedRowsCount = sqlCommand.ExecuteNonQuery();
						this.returnValue = (int)sqlCommand.Parameters["@RETURN_VALUE"].Value;
						if (outExist)
						{
							this.setOutputValues(sqlCommand);
						}
						result = affectedRowsCount;
					}
					catch (Exception ex)
					{
						this.errLog(storedProcedureName, ex, paraValues);
						base.msg(storedProcedureName + " execute error:" + ex.Message);
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
		/// <summary>
		/// 执行存储过程
		/// </summary>
		/// <param name="storedProcedureName">存储过程名称</param>
		/// <param name="paraValues">参数</param>
		/// <returns>影响行数</returns>
		public int ExecuteNonQuery(string storedProcedureName, out Hashtable output, params object[] paraValues)
		{
			int result;
			lock (this.connection)
			{
				using (SqlCommand sqlCommand = this.CreateSqlCommand(this.connection, storedProcedureName))
				{
					sqlCommand.CommandTimeout = this.ExecTimeOut;
					try
					{
						this.DeriveParameters(sqlCommand, storedProcedureName);
						this.AssignParameterValues(sqlCommand, storedProcedureName, paraValues);
						int affectedRowsCount = sqlCommand.ExecuteNonQuery();
						this.returnValue = (int)sqlCommand.Parameters["@RETURN_VALUE"].Value;
						output = this.setOutputValues(sqlCommand);
						result = affectedRowsCount;
					}
					catch (Exception ex)
					{
						this.errLog(storedProcedureName, ex, paraValues);
						base.msg(storedProcedureName + " execute error:" + ex.Message);
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
		/// <summary>
		/// 执行存储过程，返回 System.Data.DataTable。(TableName = "yes" 表示执行成功，否则 TableName 为错误信息)
		/// </summary>
		/// <param name="storedProcedureName">SP name</param>
		/// <param name="paraValues">Params</param>
		/// <returns></returns>
		public DataTable ExecuteDataTable(string storedProcedureName, params object[] paraValues)
		{
			DataTable result;
			lock (this.connection)
			{
				using (SqlCommand sqlCommand = this.CreateSqlCommand(this.connection, storedProcedureName))
				{
					try
					{
						sqlCommand.CommandTimeout = this.ExecTimeOut;
						this.DeriveParameters(sqlCommand, storedProcedureName);
						bool outExist = this.AssignParameterValues(sqlCommand, storedProcedureName, paraValues);
						SqlDataAdapter adapter = new SqlDataAdapter(sqlCommand);
						DataTable dataTable = new DataTable("yes");
						adapter.Fill(dataTable);
						adapter.Dispose();
						if (outExist)
						{
							this.setOutputValues(sqlCommand);
						}
						result = dataTable;
					}
					catch (Exception ex)
					{
						this.errLog(storedProcedureName, ex, paraValues);
						base.msg(storedProcedureName + " execute error:" + ex.Message);
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
		/// 执行存储过程，返回 System.Data.DataTable。(TableName = "yes" 表示执行成功，否则 TableName 为错误信息)
		/// </summary>
		/// <param name="storedProcedureName">SP name</param>
		/// <param name="paraValues">Params</param>
		/// <returns></returns>
		public DataTable ExecuteDataTable(string storedProcedureName, out Hashtable output, params object[] paraValues)
		{
			DataTable result;
			lock (this.connection)
			{
				using (SqlCommand sqlCommand = this.CreateSqlCommand(this.connection, storedProcedureName))
				{
					try
					{
						sqlCommand.CommandTimeout = this.ExecTimeOut;
						this.DeriveParameters(sqlCommand, storedProcedureName);
						this.AssignParameterValues(sqlCommand, storedProcedureName, paraValues);
						SqlDataAdapter adapter = new SqlDataAdapter(sqlCommand);
						DataTable dataTable = new DataTable("yes");
						adapter.Fill(dataTable);
						output = this.setOutputValues(sqlCommand);
						adapter.Dispose();
						result = dataTable;
					}
					catch (Exception ex)
					{
						this.errLog(storedProcedureName, ex, paraValues);
						base.msg(storedProcedureName + " execute error:" + ex.Message);
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
		/// 执行存储过程，返回 System.Data.DataTable[] (TableName = "yes" 表示执行成功，否则 TableName 为错误信息)
		/// </summary>
		/// <param name="storedProcedureName">SP name</param>
		/// <param name="paraValues">Params</param>
		/// <returns></returns>
		public DataTable[] ExecuteDataTables(string storedProcedureName, params object[] paraValues)
		{
			DataTable[] result;
			lock (this.connection)
			{
				using (SqlCommand sqlCommand = this.CreateSqlCommand(this.connection, storedProcedureName))
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
							this.setOutputValues(sqlCommand);
						}
						result = dataTable;
					}
					catch (Exception ex)
					{
						this.errLog(storedProcedureName, ex, paraValues);
						base.msg(storedProcedureName + " execute error:" + ex.Message);
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
		/// 执行存储过程，返回 System.Data.DataTable[] (TableName = "yes" 表示执行成功，否则 TableName 为错误信息)
		/// </summary>
		/// <param name="storedProcedureName">SP name</param>
		/// <param name="paraValues">Params</param>
		/// <returns></returns>
		public DataTable[] ExecuteDataTables(string storedProcedureName, out Hashtable output, params object[] paraValues)
		{
			DataTable[] result;
			lock (this.connection)
			{
				using (SqlCommand sqlCommand = this.CreateSqlCommand(this.connection, storedProcedureName))
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
						output = this.setOutputValues(sqlCommand);
						adapter.Dispose();
						result = dataTable;
					}
					catch (Exception ex)
					{
						this.errLog(storedProcedureName, ex, paraValues);
						base.msg(storedProcedureName + " execute error:" + ex.Message);
						output = new Hashtable();
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
		/// 执行存储过程，填充指定的 System.Data.DataTable。
		/// </summary>
		/// <param name="dataTable">用于填充查询结果的 System.Data.DataTable。</param>
		/// <param name="paraValues">传递给存储过程的参数值列表。</param>
		/// <example> 
		/// string conStr = "@Data Source=server;Initial Catalog=database;Integrated Security=true";
		/// string procName = "usp_Select";
		/// StoredProcedure sp = new StoredProcedure(conStr, procName);
		///
		/// int id = 1;
		/// DataTable dt = new DataTable();
		/// try{
		///     // 参数个数及顺序需要与存储过程中的一致。
		///     sp.ExecuteFillDataTable(dt, id);
		/// }catch(SqlException ex){
		/// }catch(Exception ex){
		/// }
		/// </example>
		public void ExecuteFillDataTable(DataTable dataTable, string storedProcedureName, params object[] paraValues)
		{
			using (SqlCommand sqlCommand = this.CreateSqlCommand(this.connection, storedProcedureName))
			{
				sqlCommand.CommandTimeout = this.ExecTimeOut;
				try
				{
					this.DeriveParameters(sqlCommand, storedProcedureName);
					bool outExist = this.AssignParameterValues(sqlCommand, storedProcedureName, paraValues);
					SqlDataAdapter adapter = new SqlDataAdapter(sqlCommand);
					adapter.Fill(dataTable);
					if (outExist)
					{
						this.setOutputValues(sqlCommand);
					}
				}
				catch (Exception ex)
				{
					this.errLog(storedProcedureName, ex, paraValues);
					base.msg(storedProcedureName + " execute error:" + ex.Message);
				}
				finally
				{
					sqlCommand.Parameters.Clear();
				}
			}
		}
		/// <summary>
		/// 执行存储过程返回 System.Data.SqlClient.SqlDataReader，
		/// 在 System.Data.SqlClient.SqlDataReader 对象关闭时，数据库连接自动关闭。
		/// </summary>
		/// <param name="paraValues">传递给存储过程的参数值列表。</param>
		/// <returns>包含查询结果的 System.Data.SqlClient.SqlDataReader 对象。</returns>
		/// <example>
		/// string conStr = "@Data Source=server;Initial Catalog=database;Integrated Security=true";
		/// string procName = "usp_Select";
		/// StoredProcedure sp = new StoredProcedure(conStr, procName); 
		///
		/// int id = 1;
		/// SqlDataReader sdr;
		/// try{
		///     // 参数个数及顺序需要与存储过程中的一致。
		///     sdr = p.ExecuteDataReader(id);
		/// }catch(SqlException ex){
		/// }catch(Exception ex){
		/// }
		/// </example>
		public SqlDataReader ExecuteDataReader(string storedProcedureName, params object[] paraValues)
		{
			SqlDataReader result;
			using (SqlCommand sqlCommand = this.CreateSqlCommand(this.connection, storedProcedureName))
			{
				sqlCommand.CommandTimeout = this.ExecTimeOut;
				try
				{
					this.DeriveParameters(sqlCommand, storedProcedureName);
					bool outExist = this.AssignParameterValues(sqlCommand, storedProcedureName, paraValues);
					SqlDataReader dr = sqlCommand.ExecuteReader();
					if (outExist)
					{
						this.setOutputValues(sqlCommand);
					}
					result = dr;
				}
				catch (Exception ex)
				{
					this.errLog(storedProcedureName, ex, paraValues);
					base.msg(storedProcedureName + " execute error:" + ex.Message);
					result = null;
				}
				finally
				{
					sqlCommand.Parameters.Clear();
				}
			}
			return result;
		}
		/// <summary>
		/// 执行查询，并返回查询所返回的结果集中第一行的第一列。忽略其他列或行。
		/// </summary>
		/// <param name="paraValues">传递给存储过程的参数值列表。</param>
		/// <returns>结果集中第一行的第一列或空引用（如果结果集为空）。</returns>
		/// <example> 
		/// string conStr = "@Data Source=server;Initial Catalog=database;Integrated Security=true";
		/// string procName = "usp_Select";
		/// StoredProcedure sp = new StoredProcedure(conStr, procName);
		///
		/// int id = 1;
		/// string name = "";
		/// try{
		///     // 参数个数及顺序需要与存储过程中的一致。
		///     name = sp.ExecuteScalar(id);
		/// }catch(SqlException ex){
		/// }catch(Exception ex){
		/// }
		/// </example>
		public object ExecuteScalar(string storedProcedureName, params object[] paraValues)
		{
			object result;
			using (SqlCommand sqlCommand = this.CreateSqlCommand(this.connection, storedProcedureName))
			{
				sqlCommand.CommandTimeout = this.ExecTimeOut;
				try
				{
					this.DeriveParameters(sqlCommand, storedProcedureName);
					bool outExist = this.AssignParameterValues(sqlCommand, storedProcedureName, paraValues);
					object retVale = sqlCommand.ExecuteScalar();
					if (outExist)
					{
						this.setOutputValues(sqlCommand);
					}
					result = retVale;
				}
				catch (Exception ex)
				{
					this.errLog(storedProcedureName, ex, paraValues);
					base.msg(storedProcedureName + " execute error:" + ex.Message);
					result = null;
				}
				finally
				{
					sqlCommand.Parameters.Clear();
				}
			}
			return result;
		}
	}
}
