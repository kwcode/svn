using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

/// <summary>
///DataConnect 的摘要说明
/// </summary>
public class DataConnect
{
    public static DBCommon.DataPool Data = new DBCommon.DataPool(1, ConfigurationManager.AppSettings["ConnString"]);

    public DataConnect()
    {
    }
}