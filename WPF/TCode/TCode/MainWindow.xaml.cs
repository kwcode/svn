using DBCommon;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TCode
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            initUI();
            //CodeViewCtrl ctrl = new CodeViewCtrl();
            //AddTabControl("代码", ctrl);

        }

        private void initUI()
        {
            this.WindowState = WindowState.Maximized; 
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        private void AddTabControl(string header, object itemContent)
        {

            TabItem tItem = new TabItem();
            tItem.Header = header;
            tItem.Content = itemContent;
            tabControl.Items.Add(tItem);
        }

        private void btn_GenerateCode_Click(object sender, RoutedEventArgs e)
        {
            TreeItem treeItem = this.dbview.SelectedItem as TreeItem;
            if (treeItem == null || treeItem.NodeType != NodeType.用户表)
            {
                MessageBox.Show("请选择表");
                return;
            }
            List<ColumnInfo> columnList = GetColumnInfoList(treeItem.SqlConnectResult, treeItem.NodeText);
            string nSpace = codeView.NSpace;
            if (string.IsNullOrWhiteSpace(nSpace))
            {
                nSpace = "TEST";
            }
            string code = CodeCommon.GenerateCode(columnList, nSpace, treeItem.NodeText);
            codeView.Text = code;
        }
        DataClass dc = new DataClass();
        /// <summary>
        /// 得到数据库里表或视图的列的详细信息
        /// </summary>
        /// <param name="DbName">库</param>
        /// <param name="TableName">表</param>
        /// <returns></returns>
        public List<ColumnInfo> GetColumnInfoList(SqlConnectResult sqlConInfo, string TableName)
        {
            string dbName = sqlConInfo.DbName;
            string service = sqlConInfo.ServiceName;
            bool isAll = sqlConInfo.IsAll;
            string sqlCon = "Integrated Security=SSPI;Data Source=" + sqlConInfo.ServiceName + ";Initial Catalog=" + dbName;
            if (sqlConInfo.IsSqlService)
            {
                sqlCon = "user id=" + sqlConInfo.UserID + ";password=" + sqlConInfo.Password + ";initial catalog=" + dbName + ";data source=" + sqlConInfo.ServiceName;
            }

            dc.ConnectionString = sqlCon;
            dc.OpenConn(true);

            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT ");
            strSql.Append("colorder=C.column_id,");
            strSql.Append("ColumnName=C.name,");
            strSql.Append("TypeName=T.name, ");
            //strSql.Append("Length=C.max_length, ");
            strSql.Append("Length=CASE WHEN T.name='nchar' THEN C.max_length/2 WHEN T.name='nvarchar' THEN C.max_length/2 ELSE C.max_length END,");
            strSql.Append("Preci=C.precision, ");
            strSql.Append("Scale=C.scale, ");
            strSql.Append("IsIdentity=CASE WHEN C.is_identity=1 THEN N'√'ELSE N'' END,");
            strSql.Append("isPK=ISNULL(IDX.PrimaryKey,N''),");

            strSql.Append("Computed=CASE WHEN C.is_computed=1 THEN N'√'ELSE N'' END, ");
            strSql.Append("IndexName=ISNULL(IDX.IndexName,N''), ");
            strSql.Append("IndexSort=ISNULL(IDX.Sort,N''), ");
            strSql.Append("Create_Date=O.Create_Date, ");
            strSql.Append("Modify_Date=O.Modify_date, ");

            strSql.Append("cisNull=CASE WHEN C.is_nullable=1 THEN N'√'ELSE N'' END, ");
            strSql.Append("defaultVal=ISNULL(D.definition,N''), ");
            strSql.Append("deText=ISNULL(PFD.[value],N'') ");

            strSql.Append("FROM sys.columns C ");
            strSql.Append("INNER JOIN sys.objects O ");
            strSql.Append("ON C.[object_id]=O.[object_id] ");
            strSql.Append("AND (O.type='U' or O.type='V') ");
            strSql.Append("AND O.is_ms_shipped=0 ");
            strSql.Append("INNER JOIN sys.types T ");
            strSql.Append("ON C.user_type_id=T.user_type_id ");
            strSql.Append("LEFT JOIN sys.default_constraints D ");
            strSql.Append("ON C.[object_id]=D.parent_object_id ");
            strSql.Append("AND C.column_id=D.parent_column_id ");
            strSql.Append("AND C.default_object_id=D.[object_id] ");
            strSql.Append("LEFT JOIN sys.extended_properties PFD ");
            strSql.Append("ON PFD.class=1  ");
            strSql.Append("AND C.[object_id]=PFD.major_id  ");
            strSql.Append("AND C.column_id=PFD.minor_id ");
            //			strSql.Append("--AND PFD.name='Caption'  -- 字段说明对应的描述名称(一个字段可以添加多个不同name的描述) ");
            strSql.Append("LEFT JOIN sys.extended_properties PTB ");
            strSql.Append("ON PTB.class=1 ");
            strSql.Append("AND PTB.minor_id=0  ");
            strSql.Append("AND C.[object_id]=PTB.major_id ");
            //			strSql.Append("-- AND PFD.name='Caption'  -- 表说明对应的描述名称(一个表可以添加多个不同name的描述)   ");
            strSql.Append("LEFT JOIN ");// -- 索引及主键信息
            strSql.Append("( ");
            strSql.Append("SELECT  ");
            strSql.Append("IDXC.[object_id], ");
            strSql.Append("IDXC.column_id, ");
            strSql.Append("Sort=CASE INDEXKEY_PROPERTY(IDXC.[object_id],IDXC.index_id,IDXC.index_column_id,'IsDescending') ");
            strSql.Append("WHEN 1 THEN 'DESC' WHEN 0 THEN 'ASC' ELSE '' END, ");
            strSql.Append("PrimaryKey=CASE WHEN IDX.is_primary_key=1 THEN N'√'ELSE N'' END, ");
            strSql.Append("IndexName=IDX.Name ");
            strSql.Append("FROM sys.indexes IDX ");
            strSql.Append("INNER JOIN sys.index_columns IDXC ");
            strSql.Append("ON IDX.[object_id]=IDXC.[object_id] ");
            strSql.Append("AND IDX.index_id=IDXC.index_id ");
            strSql.Append("LEFT JOIN sys.key_constraints KC ");
            strSql.Append("ON IDX.[object_id]=KC.[parent_object_id] ");
            strSql.Append("AND IDX.index_id=KC.unique_index_id ");
            strSql.Append("INNER JOIN  ");// 对于一个列包含多个索引的情况,只显示第1个索引信息
            strSql.Append("( ");
            strSql.Append("SELECT [object_id], Column_id, index_id=MIN(index_id) ");
            strSql.Append("FROM sys.index_columns ");
            strSql.Append("GROUP BY [object_id], Column_id ");
            strSql.Append(") IDXCUQ ");
            strSql.Append("ON IDXC.[object_id]=IDXCUQ.[object_id] ");
            strSql.Append("AND IDXC.Column_id=IDXCUQ.Column_id ");
            strSql.Append("AND IDXC.index_id=IDXCUQ.index_id ");
            strSql.Append(") IDX ");
            strSql.Append("ON C.[object_id]=IDX.[object_id] ");
            strSql.Append("AND C.column_id=IDX.column_id  ");

            strSql.Append("WHERE O.name=N'" + TableName + "' ");
            strSql.Append("ORDER BY O.name,C.column_id  ");


            //return Query(DbName,strSql.ToString()).Tables[0];

            ArrayList colexist = new ArrayList();//是否已经存在

            List<ColumnInfo> collist = new List<ColumnInfo>();
            ColumnInfo col;
            DataTable dt = dc.GetDataTable(strSql.ToString());
            foreach (System.Data.DataRow item in dt.Rows)
            {
                col = new ColumnInfo();
                col.Colorder = item["Colorder"].ToString();
                col.ColumnName = item["ColumnName"].ToString();
                col.TypeName = item["TypeName"].ToString();
                col.Length = item["Length"].ToString();
                col.Preci = item["Preci"].ToString();
                col.Scale = item["Scale"].ToString();
                col.IsIdentity = (item["IsIdentity"].ToString() == "√") ? true : false;
                col.IsPK = (item["IsPK"].ToString() == "√") ? true : false;
                col.cisNull = (item["cisNull"].ToString() == "√") ? true : false;
                col.DefaultVal = item["DefaultVal"].ToString();
                col.DeText = item["DeText"].ToString();
                if (!colexist.Contains(col.ColumnName))
                {
                    collist.Add(col);
                    colexist.Add(col.ColumnName);
                }
            }
            return collist;
        }

    }
}
