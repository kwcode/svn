using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using DataBack.Model;

namespace DataBack.Dal.Sys
{
    class DataBackup
    {
        DataBack.Dal.Data Dal = new DataBack.Dal.Data();


        #region  添加
        public void Add(Hashtable ht)
        {
            Dal.Add("DataBackup", ht, null);
        }
        #endregion


        #region 取得一条记录
        public DataBackup_Model get_1_Detail(string id)
        {
            
            string Sql_Where = " where id=" + id;

            return (DataBackup_Model)Data.GetDetail(null, " DataBackup ", Sql_Where, "DataBack.Model.DataBackup_Model");

        }
        #endregion


        #region 修改
        public void Midify(Hashtable ht, string id)
        {

            string sql_Where = " where id=" + id;

            Dal.Update("[DataBackup_Model]", ht, sql_Where);

        }
        #endregion


    }
}
