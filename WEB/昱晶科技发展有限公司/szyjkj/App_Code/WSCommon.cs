using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
///QTCommon 的摘要说明
/// </summary>
public class WSCommon
{
    public WSCommon()
    {

    }
    #region 首页相关==============================================
    /// <summary>
    /// 获取首页新闻
    /// </summary>
    /// <returns></returns>
    public static DataTable GetHomeNews(int pagesize)
    {
        DataTable dt = DataConnect.Data.ExecuteDataTable("p_home_getnews", new object[] { pagesize });
        return dt;
    }
    public static DataTable GetHomeProducts(int pagesize)
    {
        DataTable dt = DataConnect.Data.ExecuteDataTable("p_home_getproducts", new object[] { pagesize });
        return dt;
    }
    #endregion

    #region 新闻相关
    public static int AddNews(int showindex, string title, string summary, string details)
    {
        DataTable dt = DataConnect.Data.ExecuteDataTable("p_admin_addnew", new object[] { showindex, title, summary, details, 0 });
        if (dt == null || dt.Rows.Count == 0)
        {
            return 0;
        }
        else
        {
            return 1;
        }

    }

    #endregion
}