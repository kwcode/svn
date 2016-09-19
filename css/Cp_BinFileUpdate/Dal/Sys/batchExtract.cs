using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using DataBack.Model;

namespace DataBack.Dal.Sys
{
    class batchExtract
    {
        DataBack.Dal.Data Dal = new DataBack.Dal.Data();


        #region  添加
        public void Add(Hashtable ht)
        {
            Dal.Add("batchExtract", ht, null);
        }
        #endregion



        #region 修改
        public void Midify(Hashtable ht, string id)
        {

            string sql_Where = " where id=" + id;

            Dal.Update("[batchExtract]", ht, sql_Where);

        }
        #endregion


    }
}
