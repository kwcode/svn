using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Collections;

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

    #region 新闻相关==============================================
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
    public static int UpdateNews(int id, int showindex, string title, string summary, string details)
    {
        return DataConnect.Data.ExecuteSP("p_admin_updatenew", new object[] { id, showindex, title, summary, details });

    }

    public static DataTable GetNews(int pageindex, int pagesize, out int total)
    {
        Hashtable ht = new Hashtable();
        total = 0;
        DataTable dt = DataConnect.Data.ExecuteDataTable("p_news_getnews", out ht, new object[] { pageindex, pagesize, total });
        if (ht.Count > 0)
        {
            total = Convert.ToInt32(ht["total"] ?? "0");
        }
        return dt;
    }

    public static DataTable GetNewsByID(int id)
    {
        DataTable dt = DataConnect.Data.ExecuteDataTable("p_news_getnewsbyid", new object[] { id });

        return dt;
    }
    #endregion

    #region 公司简介==============================================
    public static int SaveAboutMe(string summary, string details)
    {
        DataTable dt = DataConnect.Data.ExecuteDataTable("p_admin_saveaboutme", new object[] { summary, details });
        if (dt == null || dt.Rows.Count == 0)
        {
            return 0;
        }
        else
        {
            return 1;
        }
    }

    public static DataTable GetAboutMe()
    {
        DataTable dt = DataConnect.Data.ExecuteDataTable("p_admin_getaboutme", new object[] { });
        return dt;
    }
    #endregion

    #region 联系我们==============================================
    public static int SaveRelation(string details)
    {
        DataTable dt = DataConnect.Data.ExecuteDataTable("p_admin_saverelation", new object[] { details });
        if (dt == null || dt.Rows.Count == 0)
        {
            return 0;
        }
        else
        {
            return 1;
        }
    }

    public static DataTable GetRelation()
    {
        DataTable dt = DataConnect.Data.ExecuteDataTable("p_admin_getrelation", new object[] { });
        return dt;
    }
    #endregion

    #region 轮播图片==============================================
    /// <summary>
    /// 获取轮播图片
    /// </summary>
    /// <param name="pagesize"></param>
    /// <returns></returns>
    public static DataTable GetBanner(int pagesize)
    {
        DataTable dt = DataConnect.Data.ExecuteDataTable("p_comm_getbannner", new object[] { pagesize });
        return dt;
    }
    public static int AddBanner(string title, string imgaddress, int showindex)
    {
        DataTable dt = DataConnect.Data.ExecuteDataTable("p_comm_addbannner", new object[] { title, imgaddress, showindex });
        if (dt == null || dt.Rows.Count == 0)
        {
            return 0;
        }
        else
        {
            return 1;
        }
    }
    public static int UpdateBanner(int id, string title, string imgaddress, int showindex)
    {
        return DataConnect.Data.ExecuteSP("p_comm_updatebannner", new object[] { id, title, imgaddress, showindex });

    }

    #endregion
}