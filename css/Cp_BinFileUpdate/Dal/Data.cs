using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;
using System.Reflection;
using System.Configuration;
using System.Data.SqlClient;

namespace DataBack.Dal
{
    class Data
    {


        public static readonly string conn_Default =ConnDate();  //系统默认数据库连接串

        public static string ConnDate()
        {
            string txtConn="";

            string txtConnPath = System.Windows.Forms.Application.StartupPath + "\\Conn.txt";

            int counter = 0;

            string line;

            // Read the file and display it line by line.
            System.IO.StreamReader file = new System.IO.StreamReader(txtConnPath);

            while ((line = file.ReadLine()) != null)
            {
                txtConn=line;
                counter++;
            }

            file.Close();

            return txtConn;
        }

        #region 存储过程 数据源DataSet,可返回一条或多条记录
        /// <param name="SelectIF">选择字段</param>
        /// <param name="Table">表名</param>
        /// <param name="sql_Where">条件,条件要自带where </param>
        /// <returns></returns>
        public static DataSet proc_GetSource_GroupBy(string SelectIF, string Table, string sql_Where, int CurrentPageIndex, int PageSize, string txtID, string txtOrder, string txtGroup)
        {
            using (SqlConnection MyConn = new SqlConnection(conn_Default))
            using (SqlCommand myCommand = new SqlCommand())
            using (SqlDataAdapter da = new SqlDataAdapter())
            using (DataSet ds = new DataSet())
            {
                try
                {
                    if (SelectIF == null || SelectIF.Trim() == "")
                        SelectIF = " * ";

                    //以下为存储过程部分------------------

                    myCommand.Connection = MyConn;
                    myCommand.CommandText = "MultiPage";
                    myCommand.CommandType = CommandType.StoredProcedure;

                    myCommand.Parameters.Add("@Tables", SqlDbType.VarChar).Value = Table;//表名称,视图

                    myCommand.Parameters.Add("@PrimaryKey", SqlDbType.VarChar).Value = txtID;//主关键字

                    myCommand.Parameters.Add("@Fields", SqlDbType.VarChar).Value = SelectIF;//不带Select,比如 txtTitle,Content 

                    myCommand.Parameters.Add("@Sort", SqlDbType.VarChar).Value = txtOrder;//排序语句，不带Order By 比如：NewsID Desc,OrderRows Asc

                    myCommand.Parameters.Add("@CurrentPage", SqlDbType.Int).Value = CurrentPageIndex;//当前页码

                    myCommand.Parameters.Add("@PageSize", SqlDbType.Int).Value = PageSize;//每页显示记录数

                    myCommand.Parameters.Add("@Filter", SqlDbType.VarChar).Value = sql_Where;//条件语句，不带Where 比如： a=b and c=d 

                    myCommand.Parameters.Add("@Group", SqlDbType.VarChar).Value = txtGroup;//Group语句,不带Group By

                    //--------------------下面是填写数据

                    //System.Web.HttpContext.Current.Response.Write(txtGroup+"<br>");			

                    da.SelectCommand = myCommand;         ///取得存储过程返回记录数

                    da.Fill(ds);
                    da.Dispose();
                    MyConn.Close();
                    MyConn.Dispose();

                    return ds;
                }
                catch 
                {
                   // System.Web.HttpContext.Current.Response.Write(err);
                    return null;
                }
            }
        }
        #endregion

        DataBack.Components.Config Config = new DataBack.Components.Config();



        #region 可按任意字段排序的分页存储过程(不用临时表的方法)
        /// <param name="SelectIF">选择字段</param>
        /// <param name="Table">表名</param>
        /// <param name="sql_Where">条件,条件要自带where </param>
        /// <returns></returns>
        public static DataSet proc_MoreOrder_GetSource(string SelectIF, string Table, string sql_Where, int CurrentPageIndex, int PageSize, string txtID, string txtOrder, int OrderType, string orderFldType)
        {
            using (SqlConnection MyConn = new SqlConnection(conn_Default))
            using (SqlCommand myCommand = new SqlCommand())
            using (SqlDataAdapter da = new SqlDataAdapter())
            using (DataSet ds = new DataSet())
            {

                if (SelectIF == null || SelectIF.Trim() == "")
                    SelectIF = " * ";

                //以下为存储过程部分------------------
                myCommand.Connection = MyConn;

                myCommand.CommandText = "GetRecordFromPage";

                myCommand.CommandType = CommandType.StoredProcedure;

                myCommand.Parameters.Add("@tblName", SqlDbType.VarChar).Value = Table;//表名

                myCommand.Parameters.Add("@fldName", SqlDbType.VarChar).Value = txtID;//主键ID

                myCommand.Parameters.Add("@listFldName", SqlDbType.VarChar).Value = SelectIF;//不带Select,比如 txtTitle,Content 

                myCommand.Parameters.Add("@orderFldName", SqlDbType.VarChar).Value = txtOrder;//排序字段名 

                myCommand.Parameters.Add("@orderFldType", SqlDbType.VarChar).Value = orderFldType;//需要排序的字段的类型。   因为price这段类型为float，所以我们这里设置值为"float" 

                myCommand.Parameters.Add("@PageSize", SqlDbType.Int).Value = PageSize;//页尺寸

                myCommand.Parameters.Add("@PageIndex", SqlDbType.Int).Value = CurrentPageIndex;//页码 

                myCommand.Parameters.Add("@OrderType", SqlDbType.Bit).Value = OrderType;//排序类型, 0 - 升序, 1 - 降序

                myCommand.Parameters.Add("@strWhere", SqlDbType.VarChar).Value = sql_Where;//查询条件 (注意: 不要加 where)             

                //--------------------下面是填写数据			
                da.SelectCommand = myCommand;         //取得存储过程返回记录数
                da.Fill(ds);
                da.Dispose();
                MyConn.Close();
                MyConn.Dispose();

                //System.Web.HttpContext.Current.Response.Write(sql_Where+"<br><br>");

                return ds;

            }
        }
        #endregion

        #region 存储过程 返回所有记录数
        /// <param name="sqlstr">sql连接字符串</param>
        public int proc_RecordCount(string SelectIF, string Table, string sql_Where)
        {
            using (SqlConnection MyConn = new SqlConnection(conn_Default))
            using (SqlCommand myCommand = new SqlCommand())
            {
                try
                {

                    MyConn.Open();

                    //以下为存储过程部分------------------
                    myCommand.Connection = MyConn;
                    myCommand.CommandText = "proc_RecordCount";
                    myCommand.CommandType = CommandType.StoredProcedure;

                    myCommand.Parameters.Add("@Tables", SqlDbType.VarChar).Value = Table;//表名称,视图
                    myCommand.Parameters.Add("@Fields", SqlDbType.VarChar).Value = SelectIF;//不带Select,比如 txtTitle,Content 
                    myCommand.Parameters.Add("@Filter", SqlDbType.VarChar).Value = sql_Where;//带Where 比如：where a=b and c=d 

                    MyConn.Close();
                    MyConn.Dispose();

                    return 0;
                }
                catch 
                {

                   // System.Web.HttpContext.Current.Response.Write(err);
                    return 0;
                }
            }

        }
        #endregion

        #region 存储过程 数据源DataSet,可返回一条或多条记录
        /// <param name="SelectIF">选择字段</param>
        /// <param name="Table">表名</param>
        /// <param name="sql_Where">条件,条件要自带where </param>
        /// <returns></returns>
        public static DataSet proc_GetSource(string SelectIF, string Table, string sql_Where, int CurrentPageIndex, int PageSize, string txtID, string txtOrder)
        {
            using (SqlConnection MyConn = new SqlConnection(conn_Default))
            using (SqlCommand myCommand = new SqlCommand())
            using (SqlDataAdapter da = new SqlDataAdapter())
            using (DataSet ds = new DataSet())
            {
                try
                {
                    if (SelectIF == null || SelectIF.Trim() == "")
                        SelectIF = " * ";

                    //以下为存储过程部分------------------

                    myCommand.Connection = MyConn;

                    myCommand.CommandText = "MultiPage";

                    myCommand.CommandType = CommandType.StoredProcedure;

                    myCommand.Parameters.Add("@Tables", SqlDbType.VarChar).Value = Table;//表名称,视图

                    myCommand.Parameters.Add("@PrimaryKey", SqlDbType.VarChar).Value = txtID;//主关键字

                    myCommand.Parameters.Add("@Fields", SqlDbType.VarChar).Value = SelectIF;//不带Select,比如 txtTitle,Content 

                    myCommand.Parameters.Add("@Sort", SqlDbType.VarChar).Value = txtOrder;//排序语句，不带Order By 比如：NewsID Desc,OrderRows Asc

                    myCommand.Parameters.Add("@CurrentPage", SqlDbType.Int).Value = CurrentPageIndex;//当前页码

                    myCommand.Parameters.Add("@PageSize", SqlDbType.Int).Value = PageSize;//每页显示记录数

                    myCommand.Parameters.Add("@Filter", SqlDbType.VarChar).Value = sql_Where;//条件语句，不带Where 比如： a=b and c=d 

                    myCommand.Parameters.Add("@Group", SqlDbType.VarChar).Value = "";//Group语句,不带Group By

                    //--------------------下面是填写数据

                    //System.Web.HttpContext.Current.Response.Write(sql_Where+"<br>");			

                    da.SelectCommand = myCommand;         ///取得存储过程返回记录数

                    da.Fill(ds);
                    da.Dispose();
                    MyConn.Close();
                    MyConn.Dispose();


                    //System.Web.HttpContext.Current.Response.Write(sql_Where+"<br><br>");

                    return ds;
                }
                catch
                {
                    //System.Web.HttpContext.Current.Response.Write(err);
                    return null;
                }
            }
        }
        #endregion

        #region 登录检测用户密码,返回bool
        public bool LoginCheck(string TableName, string sql_Where)
        {
            string sqlstr = "Select null from " + TableName + sql_Where;
            using (SqlConnection MyConn = new SqlConnection(conn_Default))
            using (SqlCommand MyComm = new SqlCommand(sqlstr, MyConn))
            {
                try
                {
                    MyConn.Open();

                    using (SqlDataReader reader = MyComm.ExecuteReader())
                    {
                        bool i = false;
                        if (reader.Read())
                        {
                            i = true;
                        }
                        else
                        {
                            i = false;
                        }
                        MyConn.Close();
                        MyConn.Dispose();
                        return i;
                    }
                }
                catch
                {
                    //System.Web.HttpContext.Current.Response.Write(err);
                    return false;
                }
            }
        }
        #endregion

        #region 返回数据源DataSet,可返回一条或多条记录
        /// <summary>
        /// 
        /// </summary>
        /// <param name="SelectIF">选择字段</param>
        /// <param name="Table">表名</param>
        /// <param name="sql_Where">条件,条件要自带where </param>
        /// <returns></returns>
        public static DataSet GetSource(string SelectIF, string Table, string sql_Where)
        {
            if (SelectIF == null) SelectIF = " * ";
            string sqlstr = "Select " + SelectIF + " from " + Table;

            if (sql_Where != null || sql_Where != null)
            {
                sqlstr = sqlstr + sql_Where;
            }
            using (SqlConnection MyConn = new SqlConnection(conn_Default))
            using (SqlDataAdapter MyAdapter = new SqlDataAdapter(sqlstr, MyConn))




            using (DataSet ds = new DataSet())
            {

                MyConn.Open();

                MyAdapter.Fill(ds, "ListTable");
                MyAdapter.Dispose();

                MyConn.Close();
                MyConn.Dispose();
                return ds;

            }
        }
        #endregion

        #region 返回分页数据源DataSet,可返回一条或多条记录
        /// <param name="SelectIF">选择字段</param>
        /// <param name="Table">表名</param>
        /// <param name="sql_Where">条件,条件要自带where </param>
        /// <returns></returns>
        public static DataSet PageGetSource(string SelectIF, string Table, string sql_Where, int CurrentPageIndex, int PageSize)
        {
            if (SelectIF == null) SelectIF = " * ";
            string sqlstr = "Select " + SelectIF + " from " + Table;

            if (sql_Where != null || sql_Where != null)
            {
                sqlstr = sqlstr + sql_Where;
            }

            using (SqlConnection MyConn = new SqlConnection(conn_Default))
            using (SqlDataAdapter MyAdapter = new SqlDataAdapter(sqlstr, MyConn))
            using (DataSet ds = new DataSet())
            {
                try
                {
                    MyConn.Open();

                    MyAdapter.Fill(ds, CurrentPageIndex, PageSize, "News");
                    MyAdapter.Dispose();

                    MyConn.Close();
                    MyConn.Dispose();
                    return ds;
                }
                catch
                {
                    //System.Web.HttpContext.Current.Response.Write(err);
                    return null;
                }
            }
        }

        #endregion

        #region 返回sql字符串(更新/修改)
        /// <summary>
        /// 更新/修改SQL条件
        /// </summary>
        /// <param name="Table">数据库表名</param>
        /// <param name="ht">HashTable表值</param>
        /// <param name="ht_Where">sql条件</param>
        /// <returns></returns>
        public string Update_Sql(string Table, Hashtable ht, string ht_Where)
        {

            string str_Sql = "";
            int i = 0;
            int ht_Count = ht.Count;
            IDictionaryEnumerator myEnumerator = ht.GetEnumerator();
            while (myEnumerator.MoveNext())
            {
                if (i == 0)
                {
                    str_Sql = myEnumerator.Key + "='" + myEnumerator.Value + "'";

                }
                else
                {
                    str_Sql = str_Sql + "," + myEnumerator.Key + "='" + myEnumerator.Value + "'";
                }
                i = i + 1;
            }
            if (ht_Where == null || ht_Where.Replace(" ", "") == "")  // 更新时候没有条件
            {
                str_Sql = "update " + Table + " set " + str_Sql;
            }
            else
            {
                str_Sql = "update " + Table + " set " + str_Sql + "  " + ht_Where;
            }
            //str_Sql=str_Sql.Replace("set ,","set ").Replace("update ,","update ");
            //System.Web.HttpContext.Current.Response.Write(str_Sql);

            return str_Sql;
        }
        #endregion

        #region 更新/修改数据库中的表
        /// <summary>
        /// 修改/更新
        /// </summary>
        /// <param name="Table"></param>
        /// <param name="ht"></param>
        /// <param name="ht_Where"></param>
        public bool Update(string TableName, Hashtable ht, string ht_Where)
        {
            //==========================================sql语句模式
            //string sqlstr=Update_Sql(TableName,ht,ht_Where);
            //ExecuteNonQuery(sqlstr);
            //==========================================存储过程模式

            try
            {
                SqlParameter[] Parms = new SqlParameter[ht.Count];
                IDictionaryEnumerator et = ht.GetEnumerator();
                int i = 0;
                // 作哈希表循环
                while (et.MoveNext())
                {
                    System.Data.SqlClient.SqlParameter sp;

                    //内容超出限制
                    if (et.Key.ToString() == "txtContent" || et.Key.ToString() == "Content")
                    {
                        sp = Data.MakeParam("@" + et.Key.ToString(), SqlDbType.NText, System.Text.Encoding.Default.GetByteCount(et.Value.ToString()), et.Value.ToString());
                        //System.Web.HttpContext.Current.Response.Write(System.Text.Encoding.Default.GetByteCount(et.Value.ToString())+"<br>");
                    }
                    else
                    {
                        sp = Data.MakeParam("@" + et.Key.ToString(), et.Value.ToString());
                    }

                    Parms[i] = sp; // 添加SqlParameter对象
                    i = i + 1;
                }
                string str_Sql = GetUpdateSqlbyHt(TableName, ht_Where, ht); // 获得更新sql语句
                Data.ExecuteNonQuery(Data.conn_Default, CommandType.Text, str_Sql, Parms);
                return true;

            }
            catch 
            {
               // System.Web.HttpContext.Current.Response.Write(ex);
                //System.Web.HttpContext.Current.Response.End();
                return false;
            }


        }
        #endregion

        #region 执行sql语句
        public void ExecuteNonQuery(string sqlstr)
        {
            using (SqlConnection MyConn = new SqlConnection(conn_Default))
            using (SqlCommand MyComm = new SqlCommand(sqlstr, MyConn))
            {
                try
                {
                    MyConn.Open();
                    MyComm.ExecuteNonQuery();
                    MyConn.Close();
                    MyConn.Dispose();
                }
                catch
                {
                   // System.Web.HttpContext.Current.Response.Write(err);
                }
            }
        }
        #endregion

        #region 插入/添加(SQL语句)
        /// <summary>
        /// 插入/添加(SQL语句)
        /// </summary>
        /// <param name="Table"></param>
        /// <param name="ht"></param>
        /// <param name="ht_Where"></param>
        /// <returns></returns>
        public string Insert_Sql(string Table, Hashtable ht, string ht_Where)
        {
            string str_Sql = "";
            int i = 0;
            int ht_Count = ht.Count;
            string str_FieldName = "";
            string str_FieldValue = "";
            IDictionaryEnumerator myEnumerator = ht.GetEnumerator();
            while (myEnumerator.MoveNext())
            {
                if (i == 0)
                {
                    str_FieldName = myEnumerator.Key.ToString();  //字段名
                    str_FieldValue = "'" + myEnumerator.Value.ToString() + "'"; //字段值
                }
                else
                {
                    str_FieldName = str_FieldName + "," + myEnumerator.Key;
                    str_FieldValue = str_FieldValue + ",'" + myEnumerator.Value.ToString() + "'";
                }
                i = i + 1;
            }

            str_Sql = "Insert Into " + Table + "(" + str_FieldName + ") Values(" + str_FieldValue + ") " + ht_Where;

            //str_Sql=str_Sql.Replace("set ,","set ").Replace("update ,","update ");

            //System.Web.HttpContext.Current.Response.Write1(str_Sql);

            return str_Sql;
        }
        #endregion

        #region 添加/插入
        /// <summary>
        /// 添加/插入
        /// </summary>
        /// <param name="Table"></param>
        /// <param name="ht"></param>
        /// <param name="sql_Where"></param>
        public void Add(string TableName, Hashtable ht, string sql_Where)
        {
            //ExecuteNonQuery(Insert_Sql(Table,ht,sql_Where));	
            try
            {
                SqlParameter[] Parms = new SqlParameter[ht.Count];
                IDictionaryEnumerator et = ht.GetEnumerator();
                int i = 0;
                // 作哈希表循环
                while (et.MoveNext())
                {
                    System.Data.SqlClient.SqlParameter sp;

                    //内容超出限制
                    if (et.Key.ToString() == "txtContent" || et.Key.ToString() == "Content")
                    {
                        sp = Data.MakeParam("@" + et.Key.ToString(), SqlDbType.NText, System.Text.Encoding.Default.GetByteCount(et.Value.ToString()), et.Value.ToString());
                        //System.Web.HttpContext.Current.Response.Write(System.Text.Encoding.Default.GetByteCount(et.Value.ToString())+"<br>");
                    }
                    else
                    {
                        sp = Data.MakeParam("@" + et.Key.ToString(), et.Value.ToString());
                    }

                    Parms[i] = sp; // 添加SqlParameter对象
                    i = i + 1;
                }
                string str_Sql = GetInsertSqlbyHt(TableName, ht); // 获得插入sql语句

                //			System.Web.HttpContext.Current.Response.Write(str_Sql);

                Data.ExecuteNonQuery(Data.conn_Default, CommandType.Text, str_Sql, Parms);
            }
            catch 
            {
               // System.Web.HttpContext.Current.Response.Write(ex);
                //System.Web.HttpContext.Current.Response.End();
            }
        }
        #endregion

        #region 插入并取得当前ID号
        /// <summary>
        /// 添加/插入
        /// </summary>
        /// <param name="Table"></param>
        /// <param name="ht"></param>
        /// <param name="sql_Where"></param>
        public string GetInserCurrentID(string Table, Hashtable ht, string sql_Where)
        {
            string sqlstr = Insert_Sql(Table, ht, sql_Where);
            using (SqlConnection MyConn = new SqlConnection(conn_Default))
            using (SqlCommand MyComm = new SqlCommand(sqlstr, MyConn))
            {
                try
                {
                    MyConn.Open();
                    string dr = MyComm.ExecuteScalar().ToString();
                    MyConn.Close();
                    MyConn.Dispose();
                    return dr;
                }
                catch
                {
                    //System.Web.HttpContext.Current.Response.Write(err);
                    return "0";
                }
            }
        }
        #endregion

        #region   删除
        /// <summary>
        /// 删除一条或多少记录
        /// </summary>
        /// <param name="id">ID号</param>
        /// <param name="sqlstr">sql字符串</param>
        /// <returns></returns>

        public void Del(string Table, string sql_Where)
        {
            string sqlstr = "Delete from " + Table + " " + sql_Where;
            ExecuteNonQuery(sqlstr);
        }
        #endregion

        #region 检测用户或其它是否存在,返回bool
        public bool CheckOtherExists(string SelectIF, string Table, string sql_Where)
        {
            if (SelectIF == null) SelectIF = " * ";
            string sqlstr = "Select " + SelectIF + " from " + Table;

            if (sql_Where != null || sql_Where != null)
            {
                sqlstr = sqlstr + sql_Where;
            }


            using (SqlConnection MyConn = new SqlConnection(conn_Default))
            using (SqlCommand MyComm = new SqlCommand(sqlstr, MyConn))
            {
                try
                {
                    MyConn.Open();

                    using (SqlDataReader reader = MyComm.ExecuteReader())
                    {
                        bool i;
                        if (reader.Read())
                        {
                            i = true;//已经存在!
                        }
                        else
                        {
                            i = false;//不存在
                        }
                        MyConn.Close();
                        MyConn.Dispose();
                        return i;
                    }
                }
                catch
                {
                   // System.Web.HttpContext.Current.Response.Write(err);
                    return true;//已存在
                }
            }

        }
        #endregion

        #region 配合GetDetail使用获得sql条件
        /// <summary>
        /// 
        /// </summary>
        /// <param name="TableName"></param>
        /// <param name="sql_Where"></param>
        /// <returns></returns>
        public static string GetDetail_Sql(string TableName, string sql_Where)
        {
            string sqlstr = "Select * from " + TableName + " " + sql_Where;
            return sqlstr;
        }
        #endregion

        #region 返回只有一条记录
        /// <summary>
        /// 
        /// </summary>
        /// <param name="TableName"></param>
        /// <param name="sqlstr"></param>
        /// <param name="class_Name"></param>
        /// <returns></returns>
        public static Object GetDetail(string Table, string sql_Where, string class_Name)
        {

            Type myType = Type.GetType(class_Name);// 获得“类”类型
            Object o_Instance = System.Activator.CreateInstance(myType); // 实例化类

            using (DataSet ds = GetSource(null, Table, sql_Where))
            {
                try
                {
                    DataTable dt = ds.Tables[0];

                    // 获得类的所有属性数组
                    PropertyInfo[] myPropertyInfo1 = myType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

                    // 循环属性数组，并给数组属性赋值
                    for (int k = 0; k < myPropertyInfo1.Length; k++)
                    {
                        PropertyInfo myPropInfo = (PropertyInfo)myPropertyInfo1[k];
                        Object filed_Val = dt.Rows[0][myPropInfo.Name];
                        //System.Web.HttpContext.Current.Response.Write(dt.Rows[0]+myPropInfo.Name+"<Br>");
                        switch (myPropInfo.PropertyType.ToString())   //个人感觉可要可不要
                        {
                            case "System.String":
                                myPropInfo.SetValue(o_Instance, filed_Val.ToString(), null);
                                break;
                        }
                    }
                }
                catch 
                {
                   // System.Web.HttpContext.Current.Response.Write(err);
                    return false;
                }
            }
            // 把一行类记录赋值给ILst对象
            return o_Instance;
        }
        #endregion

        #region 返回只有一条记录SelectIF,Table,sql_Where,class_Name
        /// <summary>
        /// 
        /// </summary>
        /// <param name="TableName"></param>
        /// <param name="sqlstr"></param>
        /// <param name="class_Name"></param>
        /// <returns></returns>
        public static Object GetDetail(string SelectIF, string Table, string sql_Where, string class_Name)
        {
            Type myType = Type.GetType(class_Name);// 获得“类”类型
            Object o_Instance = System.Activator.CreateInstance(myType); // 实例化类
            using (DataSet ds = GetSource(SelectIF, Table, sql_Where))
            {
                try
                {
                    DataTable dt = ds.Tables[0];

                    // 获得类的所有属性数组
                    PropertyInfo[] myPropertyInfo1 = myType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

                    // 循环属性数组，并给数组属性赋值
                    for (int k = 0; k < myPropertyInfo1.Length; k++)
                    {
                        PropertyInfo myPropInfo = (PropertyInfo)myPropertyInfo1[k];
                        Object filed_Val = dt.Rows[0][myPropInfo.Name];
                        //System.Web.HttpContext.Current.Response.Write(dt.Rows[0]+myPropInfo.Name+"<Br>");
                        switch (myPropInfo.PropertyType.ToString())   //个人感觉可要可不要
                        {
                            case "System.String":
                                myPropInfo.SetValue(o_Instance, filed_Val.ToString(), null);
                                break;
                        }
                    }
                    // 把一行类记录赋值给ILst对象
                }
                catch
                {
                   // System.Web.HttpContext.Current.Response.Write(err);
                    return false;
                }
            }
            return o_Instance;
        }
        #endregion

        #region 返回所有记录数
        /// <param name="sqlstr">sql连接字符串</param>
        public int RecordCount(string SelectIF, string Table, string sql_Where)
        {
            string sqlstr = "Select " + SelectIF + " from " + Table + " " + sql_Where;
            using (SqlConnection MyConn = new SqlConnection(conn_Default))
            using (SqlCommand MyComm = new SqlCommand(sqlstr, MyConn))
            {
                try
                {

                    int intCount = 0;
                    MyConn.Open();


                    //计算共有多少记录

                    intCount = (int)MyComm.ExecuteScalar();

                    MyComm.Dispose();

                    MyConn.Close();
                    MyConn.Dispose();//结束
                    //	System.Web.HttpContext.Current.Response.Write(sql_Where+"<br>");
                    return intCount;
                }
                catch 
                {
                    //System.Web.HttpContext.Current.Response.Write(err);
                    return 0;
                }
            }

        }
        #endregion

        #region  返回一个SqlParameter实例
        /// </summary>
        /// <param name="ParamName">字段名</param>
        /// <param name="stype">字段类型</param>
        /// <param name="size">范围</param>
        /// <param name="Value">赋值</param>
        /// <returns>返回一个SqlParameter实例</returns>
        public static SqlParameter MakeParam(string ParamName, System.Data.SqlDbType stype, int size, Object Value)
        {
            SqlParameter para = new SqlParameter(ParamName, Value);
            para.SqlDbType = stype;
            para.Size = size;
            return para;
        }
        #endregion

        #region 获得SqlParameter实例
        /// </summary>
        /// <param name="ParamName">字段名</param>
        /// <param name="Value">赋值</param>
        /// <returns>返回一个SqlParameter实例</returns>
        public static SqlParameter MakeParam(string ParamName, string Value)
        {
            return new SqlParameter(ParamName, Value);
        }
        #endregion

        #region 获得插入Sql语句
        /// </summary>
        /// <param name="TableName">数据库表名</param>
        /// <param name="ht">表示层传递过来的哈希表对象</param>
        /// <returns>返回插入Sql语句</returns>
        public static string GetInsertSqlbyHt(string TableName, Hashtable ht)
        {
            string str_Sql = "";
            int i = 0;

            int ht_Count = ht.Count; // 哈希表个数
            IDictionaryEnumerator myEnumerator = ht.GetEnumerator();
            string before = "";
            string behide = "";
            while (myEnumerator.MoveNext())
            {
                if (i == 0)
                {
                    before = "(" + myEnumerator.Key;
                }
                else if (i + 1 == ht_Count)
                {
                    before = before + "," + myEnumerator.Key + ")";
                }
                else
                {
                    before = before + "," + myEnumerator.Key;
                }
                i = i + 1;
            }

            if (ht_Count == 1)
            {
                before = before + ")";
            }

            behide = " Values" + before.Replace(",", ",@").Replace("(", "(@");
            str_Sql = " Insert into " + TableName + before + behide;

            //str_Sql=" set identity_insert "+TableName+" on "+" Insert into "+TableName+before+behide+" set identity_insert "+TableName+" off ";
            //System.Web.HttpContext.Current.Response.Write("<br>"+ht_Count.ToString());
            return str_Sql;
        }
        #endregion

        #region 执行ExecuteNonQuery
        /// </summary>
        /// <param name="connString">数据库连接</param>
        /// <param name="cmdType">Sql语句类型</param>
        /// <param name="cmdText">Sql语句</param>
        /// <param name="cmdParms">Parm数组</param>
        /// <returns>返回影响行数</returns>
        public static int ExecuteNonQuery(string connString, CommandType cmdType, string cmdText, params SqlParameter[] cmdParms)
        {

            using (SqlConnection conn = new SqlConnection(connString))
            using (SqlCommand cmd = new SqlCommand())
            {
                try
                {
                    conn.Open();
                    cmd.Connection = conn;
                    cmd.CommandText = cmdText;
                    if (cmdParms != null)
                    {
                        foreach (SqlParameter parm in cmdParms)
                            cmd.Parameters.Add(parm);
                    }

                    int val = cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                    conn.Close();
                    conn.Dispose();
                    return val;
                }
                catch
                {
                    //System.Web.HttpContext.Current.Response.Write(err);
                    return 0;
                }
            }
        }
        #endregion

        #region 获得数字字段最大值(注：当该表记录为空,返回0)
        /// </summary>
        /// <param name="connString">数据库连接</param>
        /// <param name="id">Key值字段名</param>
        /// <param name="table_name">数据库名</param>
        /// <returns>返回数字字段最大值</returns>
        public static int GetMaxId(string connString, string id, string table_name)
        {
            string str_Sql = "Select Max(" + id + ") from " + table_name;
            int int_MaxId = 0;
            object obj = Data.ExecuteScalar(connString, CommandType.Text, str_Sql, null);
            if (obj == System.DBNull.Value)
            {
                int_MaxId = 0;
            }
            else
            {
                int_MaxId = Convert.ToInt32(obj);
            }

            return int_MaxId;
        }
        #endregion

        #region 执行ExecuteScalar
        /// </summary>
        /// <param name="connString">数据库连接串</param>
        /// <param name="cmdType">Sql语句类型</param>
        /// <param name="cmdText">Sql语句</param>
        /// <param name="cmdParms">Parm数组</param>
        /// <returns>返回第一行第一列记录值</returns>
        public static object ExecuteScalar(string connString, CommandType cmdType, string cmdText, params SqlParameter[] cmdParms)
        {


            using (SqlConnection conn = new SqlConnection(connString))
            using (SqlCommand cmd = new SqlCommand())
            {
                try
                {
                    conn.Open();
                    cmd.Connection = conn;
                    cmd.CommandText = cmdText;
                    if (cmdParms != null)
                    {
                        foreach (SqlParameter parm in cmdParms)
                            cmd.Parameters.Add(parm);
                    }
                    object val = cmd.ExecuteScalar();
                    cmd.Parameters.Clear();
                    conn.Close();
                    conn.Dispose();
                    return val;
                }
                catch 
                {
                   // System.Web.HttpContext.Current.Response.Write(err);
                    return "0";
                }
            }
        }
        #endregion

        #region 通过传递哈希表参数，获得更新Sql语句
        /// </summary>
        /// <param name="Table">数据库表名</param>
        /// <param name="ht_Where">传递条件,比如Id=@Id</param>
        /// <param name="ht">表示层传递过来的哈希表对象</param>
        /// <returns></returns>
        public static string GetUpdateSqlbyHt(string Table, string ht_Where, Hashtable ht)
        {
            string str_Sql = "";
            int i = 0;

            int ht_Count = ht.Count; // 哈希表个数
            IDictionaryEnumerator myEnumerator = ht.GetEnumerator();
            while (myEnumerator.MoveNext())
            {
                if (i == 0)
                {
                    if (ht_Where.ToString().ToLower().IndexOf((myEnumerator.Key + "=@" + myEnumerator.Key).ToLower()) == -1)
                    {
                        str_Sql = myEnumerator.Key + "=@" + myEnumerator.Key;
                    }
                }
                else
                {

                    if (ht_Where.ToString().ToLower().IndexOf(("@" + myEnumerator.Key + " ").ToLower()) == -1)
                    {
                        str_Sql = str_Sql + "," + myEnumerator.Key + "=@" + myEnumerator.Key;
                    }

                }
                i = i + 1;
            }
            if (ht_Where == null || ht_Where.Replace(" ", "") == "")  // 更新时候没有条件
            {
                str_Sql = "update " + Table + " set " + str_Sql;
            }
            else
            {
                str_Sql = "update " + Table + " set " + str_Sql + ht_Where;
            }
            str_Sql = str_Sql.Replace("set ,", "set ").Replace("update ,", "update ");
            return str_Sql;
        }
        #endregion

        #region 对数值型字段记录合计
        /// <param name="sqlstr">sql连接字符串</param>
        public Decimal RecordSum(string SelectIF, string Table, string sql_Where)
        {
            string sqlstr = "Select " + SelectIF + " from " + Table + " " + sql_Where;


            using (SqlConnection MyConn = new SqlConnection(conn_Default))
            using (SqlCommand MyComm = new SqlCommand(sqlstr, MyConn))
            {
                try
                {
                    MyConn.Open();

                    Decimal intCount = 0;

                    try
                    {

                        //string txtCount=MyComm.ExecuteScalar().ToString();

                        intCount = Convert.ToDecimal(MyComm.ExecuteScalar().ToString());
                        //System.Web.HttpContext.Current.Response.Write("Data层"+MyComm.ExecuteScalar().ToString());

                    }
                    catch
                    {
                        intCount = 0;

                        //System.Web.HttpContext.Current.Response.Write("Data层"+MyComm.ExecuteScalar().ToString());
                        //System.Web.HttpContext.Current.Response.End();
                    }



                    MyConn.Close();
                    MyConn.Dispose();
                    return intCount;


                }
                catch 
                {
                    //System.Web.HttpContext.Current.Response.Write(err);
                    return 0;
                }
            }

        }
        #endregion

        #region 检测用户或其它是否存在,返回bool 另一种检测方式
        public bool CheckOtherExistsOk(string SelectIF, string Table, string sql_Where)
        {

            if (SelectIF == null) SelectIF = " * ";
            string sqlstr = "Select " + SelectIF + " from " + Table;

            if (sql_Where != null || sql_Where != null)
            {
                sqlstr = sqlstr + sql_Where;
            }

            using (SqlConnection MyConn = new SqlConnection(conn_Default))
            using (SqlCommand MyComm = new SqlCommand(sqlstr, MyConn))
            {
                try
                {
                    MyConn.Open();

                    string intCount = MyComm.ExecuteScalar().ToString();

                    MyConn.Close();
                    MyConn.Dispose();

                    if (intCount == "0")      //未找到
                        return false;
                    return true;          //找到
                }
                catch 
                {
                    //System.Web.HttpContext.Current.Response.Write(err);
                    return true;//找到
                }
            }
        }
        #endregion

        #region 检测其它用户存在否
        public bool CheckOtherExistsYes(string SelectIF, string Table, string sql_Where)
        {
            if (SelectIF == null) SelectIF = " * ";
            string sqlstr = "Select " + SelectIF + " from " + Table;

            if (sql_Where != null || sql_Where != null)
            {
                sqlstr = sqlstr + sql_Where;
            }

            using (SqlConnection MyConn = new SqlConnection(conn_Default))
            using (SqlCommand command = new SqlCommand("sp_CheckOtherExists", MyConn))
            {
                try
                {

                    MyConn.Open();// 废置SqlCommand的属性为存储过程

                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add("@sqlstr", SqlDbType.NText);

                    command.Parameters["@sqlstr"].Value = sqlstr;

                    string Count = command.ExecuteScalar().ToString();

                    //System.Web.HttpContext.Current.Response.Write(newID);

                    MyConn.Close();
                    MyConn.Dispose();

                    if (Count == "0")      //未找到
                        return false;
                    return true;          //找到
                }
                catch 
                {
                   // System.Web.HttpContext.Current.Response.Write(err);
                    return true;
                }
            }
        }
        #endregion

        #region 返回数据源DataSet,可返回一条或多条记录
        /// <summary>
        /// 
        /// </summary>
        /// <param name="SelectIF">选择字段</param>
        /// <param name="Table">表名</param>
        /// <param name="sql_Where">条件,条件要自带where </param>
        /// <returns></returns>
        public static DataSet Search_GetSource(string sqlstr)
        {
            using (SqlConnection MyConn = new SqlConnection(conn_Default))
            using (SqlDataAdapter MyAdapter = new SqlDataAdapter(sqlstr, MyConn))
            {
                try
                {
                    MyConn.Open();

                    //System.Web.HttpContext.Current.Response.Write(sqlstr);

                    DataSet ds = new DataSet();
                    MyAdapter.Fill(ds, "ListTable");
                    MyAdapter.Dispose();

                    MyConn.Close();
                    MyConn.Dispose();
                    return ds;
                }
                catch 
                {
                    //System.Web.HttpContext.Current.Response.Write(err);
                    return null;
                }
            }
        }
        #endregion

        #region 更新/修改数据库中的表 没有过滤特殊字符
        /// <summary>
        /// 修改/更新
        /// </summary>
        /// <param name="Table"></param>
        /// <param name="ht"></param>
        /// <param name="ht_Where"></param>
        public void No_SqlReplace_Update(string TableName, Hashtable ht, string ht_Where)
        {
            //==========================================sql语句模式
            //string sqlstr=Update_Sql(TableName,ht,ht_Where);
            //ExecuteNonQuery(sqlstr);
            //==========================================存储过程模式
            SqlParameter[] Parms = new SqlParameter[ht.Count];
            IDictionaryEnumerator et = ht.GetEnumerator();
            int i = 0;
            // 作哈希表循环
            while (et.MoveNext())
            {
                System.Data.SqlClient.SqlParameter sp;

                //内容超出限制
                if (et.Key.ToString() == "txtContent" || et.Key.ToString() == "Content")
                {
                    sp = Data.MakeParam("@" + et.Key.ToString(), SqlDbType.NText, System.Text.Encoding.Default.GetByteCount(et.Value.ToString()), et.Value.ToString());
                    //System.Web.HttpContext.Current.Response.Write(System.Text.Encoding.Default.GetByteCount(et.Value.ToString())+"<br>");
                }
                else
                {
                    sp = Data.MakeParam("@" + et.Key.ToString(), et.Value.ToString());
                }

                Parms[i] = sp; // 添加SqlParameter对象
                i = i + 1;
            }
            string str_Sql = GetUpdateSqlbyHt(TableName, ht_Where, ht); // 获得更新sql语句
            Data.ExecuteNonQuery(Data.conn_Default, CommandType.Text, str_Sql, Parms);
            //==========================================

        }
        #endregion
    }
}
