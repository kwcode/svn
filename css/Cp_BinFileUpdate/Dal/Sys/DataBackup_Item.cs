using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace DataBack.Dal.Sys
{
    class DataBackup_Item
    {
        DataBack.Dal.Data Dal = new DataBack.Dal.Data();

        #region  添加
        public void Add(Hashtable ht)
        {
            Dal.Add("DataBackup_Item", ht, null);
        }
        #endregion
    }
}
