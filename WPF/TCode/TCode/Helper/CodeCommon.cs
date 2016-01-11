using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TCode
{
    public class CodeCommon
    {
        public static Dictionary<string, string> dicCsType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="columnInfo"></param>
        /// <param name="nSpace"></param>
        /// <param name="modelName"></param>
        /// <returns></returns>
        public static string GenerateCode(List<ColumnInfo> columnInfo, string nSpace, string modelName)
        {
            StringPlus strclass = new StringPlus();
            strclass.AppendLine("using System;");
            strclass.AppendLine("using System.Collections.Generic;");
            strclass.AppendLine("using System.Linq;");
            strclass.AppendLine("using System.Text;");
            strclass.AppendLine("namespace " + nSpace);
            strclass.AppendLine("{");
            strclass.AppendSpaceLine(1, "/// <summary>");
            strclass.AppendSpaceLine(1, "/// 实体类" + modelName);

            strclass.AppendSpaceLine(1, "/// Class Name：" + modelName + "-对应实体类");
            strclass.AppendSpaceLine(1, "/// Depiction：" + modelName + "-对应实体类");
            strclass.AppendSpaceLine(1, "/// Create By :" + "TCode");
            strclass.AppendSpaceLine(1, "/// Create Date:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm"));

            strclass.AppendSpaceLine(1, "/// </summary>");
            //strclass.AppendSpaceLine(1, "[Serializable]");
            strclass.AppendSpaceLine(1, "public class " + modelName);
            strclass.AppendSpaceLine(1, "{");
            //strclass.AppendSpaceLine(2, "public " + modelName + "()");
            //strclass.AppendSpaceLine(2, "{}");
            strclass.AppendLine(CreatModelMethod(columnInfo));
            strclass.AppendSpaceLine(1, "}");
            strclass.AppendLine("}");
            strclass.AppendLine("");

            return strclass.ToString();
        }
        public static string CreatModelMethod(List<ColumnInfo> columnInfo)
        {
            StringPlus strclass = new StringPlus();
            StringPlus strclass1 = new StringPlus();
            StringPlus strclass2 = new StringPlus();
            strclass.AppendSpaceLine(2, "#region 原始字段");
            strclass.AppendLine();
            foreach (ColumnInfo field in columnInfo)
            {
                string columnName = field.ColumnName;
                string columnType = field.TypeName;
                bool IsIdentity = field.IsIdentity;
                bool ispk = field.IsPK;
                bool cisnull = field.cisNull;
                string deText = field.DeText;
                columnType = CodeCommon.DbTypeToCS(columnType);
                string isnull = "";
                if (isValueType(columnType))
                {
                    if ((!IsIdentity) && (!ispk) && (cisnull))
                    {
                        isnull = "?";//代表可空类型
                    }
                }

                //strclass1.AppendSpaceLine(2, "private " + columnType + isnull + " _" + columnName.ToLower() + ";");//私有变量
                if (!string.IsNullOrWhiteSpace(deText))
                {
                    strclass2.AppendSpaceLine(2, "/// <summary>");
                    strclass2.AppendSpaceLine(2, "/// " + deText);
                    strclass2.AppendSpaceLine(2, "/// </summary>");
                }

                strclass2.AppendSpaceLine(2, "public " + columnType + isnull + " " + columnName + " { get; set; }");//属性
                //strclass2.AppendSpaceLine(2, "{");
                //strclass2.AppendSpaceLine(3, "set{" + " _" + columnName.ToLower() + "=value;}");
                //strclass2.AppendSpaceLine(3, "get{return " + "_" + columnName.ToLower() + ";}");
                //strclass2.AppendSpaceLine(2, "}");
            }
            strclass.Append(strclass1.Value);
            strclass.Append(strclass2.Value);
            strclass.AppendLine();
            strclass.AppendSpaceLine(2, "#endregion");

            return strclass.ToString();
        }
        private static string DbTypeToCS(string columnType)
        {
            if (dicCsType == null)
            {
                dicCsType = new Dictionary<string, string>();
                dicCsType.Add("varchar", "string");
                dicCsType.Add("nvarchar", "string");
                dicCsType.Add("char", "string");
                dicCsType.Add("nchar", "string");
                dicCsType.Add("text", "string");
                dicCsType.Add("longtext", "string");
                dicCsType.Add("date", "DateTime");
                dicCsType.Add("datetime", "DateTime");
                dicCsType.Add("int", "int");
                dicCsType.Add("number", "int");

                dicCsType.Add("bigint", "long");
                dicCsType.Add("tinyint", "int");
                dicCsType.Add("float", "decimal");

                dicCsType.Add("numeric", "decimal");
                dicCsType.Add("decimal", "decimal");
                dicCsType.Add("money", "decimal");

                dicCsType.Add("bit", "bool");
            }
            string val = "string";
            if (dicCsType.ContainsKey(columnType))
            {
                val = dicCsType[columnType];
                if (string.IsNullOrEmpty(val))
                {
                    val = "string";
                }
            }

            return val;
        }
        public static Dictionary<string, string> dicCsValueType { get; set; }
        private static bool isValueType(string columnType)
        {
            bool isval = false;
            if (dicCsValueType == null)
            {
                dicCsValueType = new Dictionary<string, string>();
                dicCsValueType.Add("int", "true");
                dicCsValueType.Add("DateTime", "true");
                dicCsValueType.Add("decimal", "true");
                dicCsValueType.Add("Decimal", "string");

            }
            if (dicCsType.ContainsKey(columnType))
            {
                string val = dicCsType[columnType];
                if (val == "true")
                {
                    isval = true;
                }
            }
            return isval;
        }
    }
}
