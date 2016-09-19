using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Data;

namespace DataBack.Dal.Page
{
    class index
    {
        DataBack.Dal.Data Dal = new DataBack.Dal.Data();

        #region 定单 数据源
        public DataSet batchExtract_GetSource()
        {
            string SelectIF = " batchExtract.* ";

            string sql_Where = " where intCompressionStatus=0 order by SystemDate ";

            string txtTable = " batchExtract ";

            return Data.GetSource(SelectIF, txtTable, sql_Where);

        }
        #endregion

    }
}
