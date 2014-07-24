using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using trip.CommunalClass;
namespace trip
{
    public class DataClass : MessageProc, IDisposable
    {
        public SPClass sp = new SPClass();
        private bool needReConnect;
        public int CmdTimeOut = 20;
        private SqlConnection conn = new SqlConnection();
        private string _ConnectionString;
        private static byte[] Keys = new byte[]
		{
			149, 
			62, 
			68, 
			162, 
			182, 
			246, 
			233, 
			169
		};
        private static string encryptKey = "0b9x8b2v";
        private Timer autoConnTimer;
        public SqlConnection conncetion
        {
            get
            {
                return this.conn;
            }
        }
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
        public DataClass()
        {
            this.sp.sqlConnection = this.conn;
        }
        /// <summary>
        ///
        /// </summary>
        /// <param name="connString">数据库连接字符串</param>
        /// <param name="isConnStrEncrypted">数据库连接字符串是否加密过</param>
        public DataClass(string connString)
        {
            this.sp.sqlConnection = this.conn;
            this.ConnectionString = connString;
        }
        public void connClose()
        {
            this.needReConnect = false;
            if (this.conn.State != ConnectionState.Closed)
            {
                this.conn.StateChange -= new StateChangeEventHandler(this.autoConn);
                if (this.autoConnTimer != null)
                {
                    this.autoConnTimer.Dispose();
                }
                this.conn.Close();
                this.conn.Dispose();
            }
        }
        public bool connOpen(bool needReConn)
        {
            if (string.IsNullOrEmpty(this._ConnectionString))
            {
                this.writeErr("connect string is empty");
                return false;
            }
            this.needReConnect = needReConn;
            if (this.conn.State != ConnectionState.Open)
            {
                try
                {
                    if (string.IsNullOrEmpty(this.conn.ConnectionString))
                    {
                        Assembly asm = Assembly.GetEntryAssembly();
                        if (asm == null)
                        {
                            asm = Assembly.GetCallingAssembly();
                        }
                        this.conn.ConnectionString = this._ConnectionString;
                    }
                    this.conn.Open();
                    this.conn.StateChange += new StateChangeEventHandler(this.autoConn);
                    base.msg("Database is connected successfully.");
                    bool result = true;
                    return result;
                }
                catch
                {
                    string msg = string.Format("connect DB Failed : {0}", this._ConnectionString);
                    this.writeErr(msg);
                    if (needReConn)
                    {
                        this.autoConn(null, null);
                    }
                    bool result = false;
                    return result;
                }
                return true;
            }
            return true;
        }
        private bool InsertDataTable(string TableName, DataTable dt)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("select * from " + TableName, this.conn))
                {
                    cmd.CommandTimeout = this.CmdTimeOut;
                    foreach (DataRow dr in dt.Rows)
                    {
                        dr.SetAdded();
                    }
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    sda.Update(dt);
                }
            }
            catch (Exception ex)
            {
                LogsRecord.write("Data", ex.ToString());
            }
            return true;
        }
        /// <summary>
        /// Make insert into string data
        /// </summary>
        /// <param name="sql">SQL statement</param>
        /// <param name="tableName">Data table name</param>
        /// <returns></returns>
        private string GetInsertSql(string sql, string tableName, bool isInsertStruct)
        {
            if (this.conn.State != ConnectionState.Open)
            {
                return null;
            }
            try
            {
                using (SqlCommand cmd = new SqlCommand(sql, this.conn))
                {
                    cmd.CommandTimeout = this.CmdTimeOut;
                    StringBuilder sb = new StringBuilder();
                    if (!isInsertStruct)
                    {
                        sb.Append(tableName);
                        sb.Append("|");
                    }
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        if (isInsertStruct)
                        {
                            sb.AppendLine(string.Concat(new string[]
							{
								"insert into ", 
								tableName, 
								" values (", 
								this.getJoin(dr, isInsertStruct), 
								")"
							}));
                        }
                        else
                        {
                            sb.AppendLine(this.getJoin(dr, isInsertStruct));
                        }
                    }
                    dr.Dispose();
                    string result;
                    if (sb.Length > 0)
                    {
                        result = sb.ToString();
                        return result;
                    }
                    result = null;
                    return result;
                }
            }
            catch (Exception ex)
            {
                base.msg(ex.Message);
                this.writeErr(sql + "\r\n" + ex.ToString());
            }
            return null;
        }
        /// <summary>
        /// Get DataTable from sql statement (return null shows that the connection isn't open)
        /// </summary>
        /// <param name="sql">被执行的SQL语句</param>
        /// <returns>返回DataSet</returns>
        private DataTable myDt(string sql)
        {
            if (this.conn.State != ConnectionState.Open)
            {
                return null;
            }
            DataTable dt = new DataTable();
            try
            {
                using (SqlDataAdapter da = new SqlDataAdapter(sql, this.conn))
                {
                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    da.Dispose();
                    dt = ds.Tables[0];
                }
            }
            catch (Exception ex)
            {
                base.msg(ex.Message);
                this.writeErr(sql + "\r\n" + ex.ToString());
            }
            return dt;
        }
        /// <summary>
        /// Get DataSet from sql statement (return null shows that the connection isn't open)
        /// </summary>
        /// <param name="sql">被执行的SQL语句</param>
        /// <returns>返回DataSet</returns>
        private DataSet myDs(string sql)
        {
            if (this.conn.State != ConnectionState.Open)
            {
                return null;
            }
            DataSet ds = new DataSet();
            try
            {
                using (SqlDataAdapter da = new SqlDataAdapter(sql, this.conn))
                {
                    da.Fill(ds);
                    da.Dispose();
                }
            }
            catch (Exception ex)
            {
                base.msg(ex.Message);
                this.writeErr(sql + "\r\n" + ex.ToString());
            }
            return ds;
        }
        /// <summary>
        /// Get SqlDataReader from sql statement (return null shows that the connection isn't open)
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        private SqlDataReader myDr(string sql)
        {
            if (this.conn.State != ConnectionState.Open)
            {
                return null;
            }
            SqlDataReader dr = null;
            try
            {
                dr = new SqlCommand(sql, this.conn)
                {
                    CommandTimeout = this.CmdTimeOut
                }.ExecuteReader();
            }
            catch (Exception ex)
            {
                base.msg(ex.Message);
                this.writeErr(sql + "\r\n" + ex.ToString());
            }
            return dr;
        }
        private string getSystemConfig(string para_name)
        {
            string sql = "select para_value from f_config where para_name = '" + para_name + "'";
            SqlDataReader dr = this.myDr(sql);
            string tmp = "";
            if (dr.Read())
            {
                tmp = dr[0].ToString();
            }
            dr.Dispose();
            return tmp;
        }
        private void setSystemConfig(string para_name, string para_value)
        {
            string sql = string.Concat(new string[]
			{
				"update f_config set para_value='", 
				para_value, 
				"' where para_name = '", 
				para_name, 
				"'"
			});
            this.myCmd(sql);
        }
        /// <summary>
        /// DES加密字符串
        /// </summary>
        /// <param name="encryptString">待加密的字符串</param>
        /// <param name="encryptKey">加密密钥,要求为8位</param>
        /// <returns>加密成功返回加密后的流，失败返回源串</returns>
        public static Stream EncryptDES(string encryptString)
        {
            Stream result;
            try
            {
                byte[] rgbKey = Encoding.Default.GetBytes(DataClass.encryptKey.Substring(0, 8));
                byte[] rgbIV = DataClass.Keys;
                byte[] inputByteArray = Encoding.Default.GetBytes(encryptString);
                DESCryptoServiceProvider dCSP = new DESCryptoServiceProvider();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, dCSP.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                result = cStream;
            }
            catch
            {
                result = new MemoryStream(Encoding.Default.GetBytes(encryptString));
            }
            return result;
        }
        /// <summary>
        /// DES加密字符串
        /// </summary>
        /// <param name="encryptString">待加密的byte[]</param>
        /// <param name="encryptKey">加密密钥,要求为8位</param>
        /// <returns>加密成功返回加密后的字符串，失败返回源串</returns>
        public static Stream EncryptDES(byte[] inputByteArray)
        {
            Stream result;
            try
            {
                byte[] rgbKey = Encoding.Default.GetBytes(DataClass.encryptKey.Substring(0, 8));
                byte[] rgbIV = DataClass.Keys;
                DESCryptoServiceProvider dCSP = new DESCryptoServiceProvider();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, dCSP.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                result = cStream;
            }
            catch
            {
                result = new MemoryStream(inputByteArray);
            }
            return result;
        }
        /// <summary>
        /// DES加密字符串
        /// </summary>
        /// <param name="encryptString">待加密的流</param>
        /// <param name="encryptKey">加密密钥,要求为8位</param>
        /// <returns>加密成功返回加密后的字符串，失败返回源串</returns>
        public static Stream EncryptDES(Stream inputStream)
        {
            Stream result;
            try
            {
                byte[] inputByteArray = DataClass.StreamToBytes(inputStream);
                byte[] rgbKey = Encoding.Default.GetBytes(DataClass.encryptKey.Substring(0, 8));
                byte[] rgbIV = DataClass.Keys;
                DESCryptoServiceProvider dCSP = new DESCryptoServiceProvider();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, dCSP.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                result = cStream;
            }
            catch
            {
                result = inputStream;
            }
            return result;
        }
        /// <summary>
        /// DES解密字符串
        /// </summary>
        /// <param name="decryptString">待解密的字符串</param>
        /// <param name="decryptKey">解密密钥,要求为8位,和加密密钥相同</param>
        /// <returns>解密成功返回解密后的字符串，失败返源串</returns>
        public static string DecryptDES(string decryptString)
        {
            string result;
            try
            {
                byte[] rgbKey = Encoding.Default.GetBytes(DataClass.encryptKey);
                byte[] rgbIV = DataClass.Keys;
                byte[] inputByteArray = Convert.FromBase64String(decryptString);
                DESCryptoServiceProvider DCSP = new DESCryptoServiceProvider();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, DCSP.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                result = Encoding.Default.GetString(mStream.ToArray());
            }
            catch
            {
                result = decryptString;
            }
            return result;
        }
        /// <summary> 
        /// 将 Stream 转成 byte[] 
        /// </summary> 
        private static byte[] StreamToBytes(Stream stream)
        {
            byte[] bytes = new byte[(int)((object)((IntPtr)stream.Length))];
            stream.Read(bytes, 0, bytes.Length);
            stream.Seek(0L, SeekOrigin.Begin);
            return bytes;
        }
        /// <summary>
        /// Write log file
        /// </summary>
        /// <param name="err"></param>
        private void writeErr(string err)
        {
            LogsRecord.write("DataClass", err);
        }
        protected override string msgType()
        {
            return "DataClass";
        }
        private string getJoin(SqlDataReader o, bool isInsert)
        {
            StringBuilder sb = new StringBuilder();
            int count = o.FieldCount;
            for (int i = 0; i < count; i++)
            {
                string v = string.Empty;
                if (isInsert)
                {
                    v = "'" + o[i].ToString() + "'";
                }
                else
                {
                    if (o[i] is bool)
                    {
                        if ((bool)o[i])
                        {
                            v = "1";
                        }
                        else
                        {
                            v = "0";
                        }
                    }
                    else
                    {
                        v = o[i].ToString();
                        v = v.Replace('\t', ' ');
                    }
                }
                if (i == 0)
                {
                    sb.Append(v);
                }
                else
                {
                    sb.Append("\t" + v);
                }
            }
            return sb.ToString();
        }
        private void autoConn(object sender, StateChangeEventArgs e)
        {
            if (!this.needReConnect)
            {
                return;
            }
            if (this.conn.State != ConnectionState.Open && this.conn.State != ConnectionState.Connecting)
            {
                base.msg("Database disconnected, system will reconnect.");
                this.autoConnTimer = new Timer(new TimerCallback(this.TimerCallback), null, 1000, 5000);
            }
        }
        private void TimerCallback(object sender)
        {
            if (!this.connOpen(this.needReConnect))
            {
                base.msg("Reconnect to DB failed.");
                return;
            }
            Thread a = new Thread(new ThreadStart(this.dropTimer));
            a.Start();
        }
        private void dropTimer()
        {
            this.autoConnTimer.Dispose();
        }
        /// <summary>
        /// Submit sql statement (Return the number of rows affected.)
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        internal int myCmd(string sql)
        {
            if (this.conn.State != ConnectionState.Open)
            {
                return -2;
            }
            int result;
            using (SqlCommand cmd = new SqlCommand(sql, this.conn))
            {
                cmd.CommandTimeout = this.CmdTimeOut;
                int b = -1;
                try
                {
                    b = cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    this.writeErr("SQL执行错误：" + sql + "\r\n" + ex.ToString());
                    result = -3;
                    return result;
                }
                result = b;
            }
            return result;
        }
        /// <summary>
        /// 获得系统配置值
        /// </summary>
        /// <param name="v_name">配置值名称</param>
        /// <returns>代理用户名</returns>
        public string getParaValue(string v_name)
        {
            string sql = "select para_value from f_config where para_name = '" + v_name + "'";
            SqlDataReader dr = this.myDr(sql);
            string tmp = "";
            if (dr.Read())
            {
                tmp = dr[0].ToString();
            }
            dr.Dispose();
            return tmp;
        }
        public void Dispose()
        {
            if (this.conn != null && this.conn.State == ConnectionState.Open)
            {
                this.conn.Close();
            }
        }
    }
}
