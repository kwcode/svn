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
    #region 后台菜单==============================================
    /// <summary>
    /// 获取所有的菜单列
    /// </summary>
    /// <returns></returns>
    public static DataTable GetPmMenuList()
    {
        DataTable dt = DataConnect.Data.ExecuteDataTable("p_admin_getpmmenulist");
        return dt;
    }
    //新增菜单
    public static int AddPmMenu(string name, int pid, int showindex, string url, string ico)
    {
        int row = DataConnect.Data.ExecuteSP("p_admin_AddPmMenu", new object[] { name, pid, showindex, url, ico });
        return row;
    }
    //修改菜单
    public static int UpdatePmMenu(string name, int pid, int showindex, string url, string ico)
    {
        int row = DataConnect.Data.ExecuteSP("p_admin_UpdatePmMenu", new object[] { name, pid, showindex, url, ico });
        return row;
    }
    #endregion

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
    /// <summary>
    /// 删除一个新闻 字段删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public static int DelNews(int id)
    {
        DataTable dt = DataConnect.Data.ExecuteDataTable("p_pro_DelNews", id);
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
    public static DataTable GetBanner(int pageindex, int pagesize, string keywords, out int total)
    {
        Hashtable ht = new Hashtable();
        DataTable dt = DataConnect.Data.ExecuteDataTable("p_comm_getbannner", out ht, new object[] { pageindex, pagesize, keywords, 0 });
        if (ht.Count > 0)
        {
            total = Convert.ToInt32(ht["@Total"]);
        }
        else
        {
            total = 0;
        }
        return dt;
    }
    public static DataTable GetBannerByID(int id)
    {
        DataTable dt = DataConnect.Data.ExecuteDataTable("p_comm_GetBannerByID", new object[] { id });
        return dt;
    }
    public static int AddBanner(string title, string imgaddress, int showindex, string url)
    {
        DataTable dt = DataConnect.Data.ExecuteDataTable("p_comm_addbannner", new object[] { title, imgaddress, showindex, url });
        if (dt == null || dt.Rows.Count == 0)
        {
            return 0;
        }
        else
        {
            return 1;
        }
    }
    public static int UpdateBanner(int id, string title, string imgaddress, int showindex, string url)
    {
        DataTable dt = DataConnect.Data.ExecuteDataTable("p_comm_updatebannner", new object[] { id, title, imgaddress, showindex, url });
        if (dt == null || dt.Rows.Count == 0)
        {
            return 0;
        }
        else
        {
            return 1;
        }
    }
    public static int DelBanner(int id)
    {
        DataTable dt = DataConnect.Data.ExecuteDataTable("p_comm_DelBanner", id);
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

    #region 产品图片==============================================
    /// <summary>
    /// 获取产品图片
    /// </summary>
    /// <param name="pagesize"></param>
    /// <returns></returns>
    public static DataTable GetProduct(int pageindex, int pagesize, string keywords, out int total)
    {
        Hashtable ht = new Hashtable();
        DataTable dt = DataConnect.Data.ExecuteDataTable("p_pro_getprocduct", out ht, new object[] { pageindex, pagesize, keywords, 0 });
        if (ht.Count > 0)
        {
            total = Convert.ToInt32(ht["@Total"]);
        }
        else
        {
            total = 0;
        }
        return dt;
        //DataTable dt = DataConnect.Data.ExecuteDataTable("p_pro_getprocduct", new object[] { pagesize });
        //return dt;
    }
    public static DataTable GetProductClassByID(int id)
    {
        DataTable dt = DataConnect.Data.ExecuteDataTable("p_Pro_GetProductClassByID", new object[] { id });
        return dt;
    }
    public static int SaveProduct(int id, string title, string summary, int showindex, int userid)
    {
        DataTable dt = DataConnect.Data.ExecuteDataTable("p_admin_saveprocduct", new object[] { id, title, summary, showindex, userid });
        if (dt == null || dt.Rows.Count == 0)
        {
            return 0;
        }
        else
        {
            return 1;
        }
    }

    /// <summary>
    /// 获取产品图片
    /// </summary>
    /// <param name="pagesize"></param>
    /// <returns></returns>
    public static DataTable GetProductImgs(int pid, int pagesize)
    {
        DataTable dt = DataConnect.Data.ExecuteDataTable("p_pro_getprocductimgs", new object[] { pid, pagesize });

        return dt;
    }
    public static DataTable GetAdminProductImgs(int pid, int pageindex, int pagesize, string keywords, out int total)
    {
        Hashtable ht = new Hashtable();
        DataTable dt = DataConnect.Data.ExecuteDataTable("p_admin_getprocductimgs", out ht, new object[] { pid, pageindex, pagesize, keywords, 0 });
        if (ht.Count > 0)
        {
            total = Convert.ToInt32(ht["@Total"]);
        }
        else
        {
            total = 0;
        }
        return dt;
    }

    public static int SaveProductImg(int id, int pid, string imgurl, int userid, string title, int showindex)
    {
        DataTable dt = DataConnect.Data.ExecuteDataTable("p_admin_saveprocductimg", new object[] { id, pid, imgurl, userid, title, showindex });
        if (dt == null || dt.Rows.Count == 0)
        {
            return 0;
        }
        else
        {
            return 1;
        }
    }



    public static DataTable GetProductImgsByid(int id)
    {
        DataTable dt = DataConnect.Data.ExecuteDataTable("p_pro_GetProductImgsByid", new object[] { id });
        return dt;
    }

    public static int DelProcductImg(int id)
    {
        DataTable dt = DataConnect.Data.ExecuteDataTable("p_poc_delprocimg", id);
        if (dt == null || dt.Rows.Count == 0)
        {
            return 0;
        }
        else
        {
            return 1;
        }
    }

    public static int DelProduct(int id)
    {
        DataTable dt = DataConnect.Data.ExecuteDataTable("p_pro_DelProcduct", id);
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

    #region 用户相关==============================================

    public static int TryLogin(string name, string pwd, bool ischeck)
    {
        Hashtable ht = new Hashtable();
        string password = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(pwd, "md5").ToLower();
        DataTable dt = DataConnect.Data.ExecuteDataTable("p_comm_userlogin", out ht, new object[] { name, password, 0 });
        int login = 0;
        if (ht.Count > 0)
        {
            login = Convert.ToInt32(ht["@LoginType"]);
        }
        if (login == 1)//登录成功
        {
            if (ischeck)
            {
                int userId = Login(dt);
                CookieHelper.SaveUinfoCookie(userId);
            }
            else
            {
                Login(dt);
            }
        }
        return login;
    }
    private static int Login(DataTable dt)
    {
        registerSession(dt);   //登录成功时写入日志 
        string date = DateTime.Now.ToString();
        string ip = GetUserIP();
        // userCon.UserLoginRecord(SessionAccess.UserId, SessionAccess.NickName, date, ip, HttpContext.Current.Request.UserAgent, string.Empty, string.Empty, string.Empty); 
        return SessionAccess.UserId;

    }

    private static void registerSession(DataTable dt)
    {
        if (dt != null && dt.Rows.Count > 0)
        {
            DataRow row = dt.Rows[0];
            SessionAccess.UserId = Convert.ToInt32(row["UserID"]);
            SessionAccess.UserType = row["Role"].ToString();
            SessionAccess.NickName = row["NickName"].ToString();
            SessionAccess.LoginName = row["LoginName"].ToString();

        }

    }
    /// <summary>
    /// 获得用户IP
    /// </summary>
    public static string GetUserIP()
    {
        string ip;
        string[] temp;
        bool isErr = false;
        if (HttpContext.Current.Request.ServerVariables["HTTP_X_ForWARDED_For"] == null)
            ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
        else
            ip = HttpContext.Current.Request.ServerVariables["HTTP_X_ForWARDED_For"].ToString();
        if (ip.Length > 15)
            isErr = true;
        else
        {
            temp = ip.Split('.');
            if (temp.Length == 4)
            {
                for (int i = 0; i < temp.Length; i++)
                {
                    if (temp[i].Length > 3) isErr = true;
                }
            }
            else
                isErr = true;
        }

        if (isErr)
            return "1.1.1.1";
        else
            return ip;
    }

    public static int AddUser(string loginname, string pwd, string nickname, int role)
    {
        DataTable dt = DataConnect.Data.ExecuteDataTable("p_comm_adduser", new object[] { loginname, pwd, role, nickname });
        return dt.Rows.Count;
    }

    public static DataTable GetUserList(int pageindex, int pagesize, string quiry, out int total)
    {
        Hashtable ht = new Hashtable();
        DataTable dt = DataConnect.Data.ExecuteDataTable("p_comm_getuserlist", out ht, new object[] { pageindex, pagesize, quiry, 0 });
        if (ht.Count > 0)
        {
            total = Convert.ToInt32(ht["@Total"]);
        }
        else
        {
            total = 0;
        }
        return dt;
    }


    public static int DelUser(int userid)
    {
        DataTable dt = DataConnect.Data.ExecuteDataTable("p_comm_deluser", userid);
        if (dt == null || dt.Rows.Count == 0)
        {
            return 0;
        }
        else
        {
            return 1;
        }
    }

    public static int UpdateUser(int userid, string loginname, string password, string nickname, int role)
    {
        DataTable dt = DataConnect.Data.ExecuteDataTable("p_comm_updateuser", new object[] { userid, loginname, password, role, nickname });
        return dt.Rows.Count;
    }
    #endregion

    #region 网站其它==============================================
    /// <summary>
    /// 记录访问量
    /// </summary>
    public void Recordvisit()
    {
        try
        {
            DataConnect.Data.ExecuteDataTable("p_comm_addaccessrecord");
        }
        catch (Exception ex)
        {
        }
    }
    #endregion

}