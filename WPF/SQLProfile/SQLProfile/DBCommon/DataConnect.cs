using DBCommon;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace SQLProfile.DBCommon
{
    public class DataConnect
    {
        public static DataPool Data = new DataPool(50, ConfigurationManager.AppSettings["ConnString"]);
    }
}
