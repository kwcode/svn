using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using trip;

namespace WPFDB
{
    public class DataConnect
    {
        public static DataPool Data = new DataPool(10, ConfigurationManager.AppSettings["ConnString"]);
        public static DataPool Data2 = new DataPool(10, ConfigurationManager.AppSettings["ConnString2"]);
    }
}
