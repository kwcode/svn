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
    #region 后台其他
    /// <summary>
    /// 获取网站基础信息
    /// 如果没有则新增
    /// 
    /// </summary>
    /// <returns></returns>
    public static DataTable GetSiteBaseSettings()
    {
        return DataConnect.Data.ExecuteDataTable("p_Comm_GetSiteBaseSettings");
    }
    /// <summary>
    /// 修改
    /// </summary>
    /// <param name="SiteName">网站名称</param>
    /// <param name="SiteTitle">网站标题</param>
    /// <param name="KeyWords">网站关键字</param>
    /// <param name="Description">网站描述</param>
    /// <param name="TopContent">网站顶部内容</param>
    /// <param name="BottomContent">网站底部内容</param>
    /// <returns></returns>
    public static int UpdateSiteBaseSettings(string SiteName, string SiteTitle, string KeyWords, string Description, string TopContent, string BottomContent)
    {
        int row = DataConnect.Data.ExecuteSP("p_Comm_UpdateSiteBaseSettings", SiteName, SiteTitle, KeyWords, Description, TopContent, BottomContent);
        return row;
    }
    #endregion

    #region 后台导航条
    /// <summary>
    ///  
    /// </summary>
    /// <returns></returns>
    public static DataTable GetNvaBarList()
    {
        DataTable dt = DataConnect.Data.ExecuteDataTable("p_Admin_GetNvaBarList");
        return dt;
    }
    public static DataTable GetNvaBarByID()
    {
        DataTable dt = DataConnect.Data.ExecuteDataTable("p_Admin_GetNvaBarByID");
        return dt;
    }
    public static int SaveNavBar(int ID, string Name, string Url, int ShowIndex)
    {
        int row = DataConnect.Data.ExecuteSP("p_Admin_SaveNavBar", new object[] { ID, Name, Url, ShowIndex });
        return row;
    }
    public static int DelNavBar(int id)
    {
        int row = DataConnect.Data.ExecuteSP("p_Admin_DelNavBar", new object[] { id });
        return row;
    }

    #endregion

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
    public static int UpdatePmMenu(int id, string name, int pid, int showindex, string url, string ico)
    {
        int row = DataConnect.Data.ExecuteSP("p_admin_UpdatePmMenu", new object[] { id, name, pid, showindex, url, ico });
        return row;
    }
    public static int DelPmMenu(int id)
    {
        int row = DataConnect.Data.ExecuteSP("p_Admin_DelPmMenu", new object[] { id });
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
    public static DataTable GetHomeNvaBar()
    {
        DataTable dt = DataConnect.Data.ExecuteDataTable("p_Home_GetHomeNvaBar");
        return dt;
    }

    #endregion

    #region 文章相关==============================================

    /// <summary>
    /// 新增一篇新文章
    /// </summary>
    /// <param name="title">文章标题</param>
    /// <param name="content">文章内容</param>
    /// <param name="userid">创建者</param>
    /// <param name="showindex">排序</param>
    /// <param name="articletypeid">类型ID</param>
    /// <returns></returns>
    public static int AddArticle(string title, string content, int userid, int showindex, int articletypeid, string imgurl, string Summary)
    {
        int row = DataConnect.Data.ExecuteSP("p_admin_AddArticle", new object[] { title, content, userid, showindex, articletypeid, imgurl, Summary });
        return row;
    }
    /// <summary>
    /// 修改一篇文章
    /// </summary>
    /// <param name="id">文章标识ID</param>
    /// <param name="title">文章标题</param>
    /// <param name="content">文章内容</param>
    /// <param name="userid">创建者</param>
    /// <param name="showindex">排序</param>
    /// <param name="articletypeid">类型ID</param>
    /// <returns></returns>
    public static int UpdateArticle(int id, string title, string content, int userid, int showindex, int articletypeid, string imgurl, string Summary)
    {
        int row = DataConnect.Data.ExecuteSP("p_admin_UpdateArticle", new object[] { id, title, content, userid, showindex, articletypeid, imgurl, Summary });
        return row;
    }

    /// <summary>
    /// 删除指定文章
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public static int DelArticle(int id)
    {
        int row = DataConnect.Data.ExecuteSP("p_admin_DelArticle", new object[] { id });
        return row;
    }

    /// <summary>
    /// 保存文章分类
    /// </summary>
    /// <param name="id">标识号 0：新增 >0修改</param>
    /// <param name="name"></param>
    /// <param name="showindex"></param>
    /// <returns></returns>
    public static int SaveArticleType(int id, string name, int showindex)
    {
        int row = DataConnect.Data.ExecuteSP("p_admin_SaveArticleType", new object[] { id, name, showindex });
        return row;
    }
    /// <summary>
    /// 删除文章类型
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public static int DelArticleType(int id)
    {
        int row = DataConnect.Data.ExecuteSP("p_admin_DelArticleType", new object[] { id });
        return row;
    }
    /// <summary>
    /// 获取文章类型集合
    /// </summary>
    /// <param name="PageIndex"></param>
    /// <param name="PageSize"></param>
    /// <param name="keywords"></param>
    /// <param name="total"></param>
    /// <returns></returns>
    public static DataTable GetArticleTypeList(int PageIndex, int PageSize, string keywords, out int total)
    {
        Hashtable ht = new Hashtable();
        total = 0;
        DataTable dt = DataConnect.Data.ExecuteDataTable("p_Article_GetArticleTypeList", out ht, PageIndex, PageSize, keywords, total);
        if (ht.Count > 0)
        {
            total = Convert.ToInt32(ht["@TotalCount"] ?? "0");
        }
        return dt;
    }
    /// <summary>
    /// 获取文章列表
    /// </summary>
    /// <param name="PageIndex"></param>
    /// <param name="PageSize"></param>
    /// <param name="typeid">0 所有的文章</param>
    /// <param name="keywords"></param>
    /// <param name="total"></param>
    /// <returns></returns>
    public static DataTable GetArticleList(int PageIndex, int PageSize, int typeid, string keywords, out int total)
    {
        Hashtable ht = new Hashtable();
        total = 0;
        DataTable dt = DataConnect.Data.ExecuteDataTable("p_Article_GetArticleList", out ht, PageIndex, PageSize, typeid, keywords, total);
        if (ht.Count > 0)
        {
            total = Convert.ToInt32(ht["@TotalCount"] ?? "0");
        }
        return dt;
    }
    /// <summary>
    /// 获取文章信息
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public static DataTable GetArticleByID(int id)
    {
        DataTable dt = DataConnect.Data.ExecuteDataTable("p_Article_GetArticleByID", id);
        return dt;
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
    public static int SaveAboutMe(int id, string typename, string summary, string details, string imgurl, int showindex)
    {
        return DataConnect.Data.ExecuteSP("p_admin_saveaboutme", id, typename, summary, details, imgurl, showindex);
    }

    public static DataTable GetAboutMe()
    {
        DataTable dt = DataConnect.Data.ExecuteDataTable("p_admin_getaboutme", new object[] { });
        return dt;
    }
    public static DataTable GetAboutByID(int id)
    {
        DataTable dt = DataConnect.Data.GetDataTable("SELECT * FROM  dbo.t_comm_jianjie WHERE ID=" + id);
        return dt;
    }
    public static int DelJianJie(int id)
    {
        int row = DataConnect.Data.ExecuteSQL("DELETE dbo.t_comm_jianjie WHERE ID=" + id);
        return row;
    }
    public static int SetJianjieIsHomeTop(int id)
    {
        int row = DataConnect.Data.ExecuteSQL(" UPDATE dbo.t_comm_jianjie SET IsHome=0 UPDATE t_comm_jianjie SET IsHome=1 WHERE ID=" + id);
        return row;

    }
    public static DataTable GetHomeJianJie()
    {
        return DataConnect.Data.GetDataTable("SELECT * FROM dbo.t_comm_jianjie WHERE IsHome=1");
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
    /// 获取产品分类
    /// </summary>
    /// <param name="pagesize"></param>
    /// <returns></returns>
    public static DataTable GetProductTypeList(int pageindex, int pagesize, string keywords, out int total)
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

    /// <summary>
    /// 设置产品滚动显示
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public static int SetIsScroll(int id)
    {
        return DataConnect.Data.ExecuteSP("p_admin_SetIsScroll", id);
    }
    /// <summary>
    /// 设置产品置顶
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public static int SetIsHomeTop(int id)
    {
        return DataConnect.Data.ExecuteSP("p_admin_SetIsHomeTop", id);
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
            SessionAccess.Session.Timeout = 30;
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
    /// <summary>
    /// 点击文章 记录
    /// </summary>
    public static void ClickArticle(int ArticleID)
    {
        try
        {
            DataConnect.Data.ExecuteSP("p_Article_Click", ArticleID);
        }
        catch (Exception ex)
        {

        }
    }

    #endregion

    #region 相册管理==============================================

    /// <summary>
    /// 获取相册
    /// </summary>
    /// <param name="PageIndex"></param>
    /// <param name="PageSize"></param>
    /// <param name="KeyWords"></param>
    /// <param name="TotalCount"></param>
    /// <returns></returns>
    public static DataTable GetPhotoBooksList(int PageIndex, int PageSize, string KeyWords, out int TotalCount)
    {
        Hashtable ht = new Hashtable();
        TotalCount = 0;
        DataTable dt = DataConnect.Data.ExecuteDataTable("p_Admin_GetPhotoBooksList", out ht, PageIndex, PageSize, KeyWords, TotalCount);
        if (ht.Count > 0)
        {
            TotalCount = Convert.ToInt32(ht["@TotalCount"] ?? "0");
        }
        return dt;
    }
    /// <summary>
    /// 获取图片
    /// </summary>
    /// <param name="PageIndex"></param>
    /// <param name="PageSize"></param>
    /// <param name="BookID"></param>
    /// <param name="KeyWords"></param>
    /// <param name="TotalCount"></param>
    /// <returns></returns>
    public static DataTable GetPhotosList(int PageIndex, int PageSize, int BookID, string KeyWords, out int TotalCount)
    {
        Hashtable ht = new Hashtable();
        TotalCount = 0;
        DataTable dt = DataConnect.Data.ExecuteDataTable("p_Admin_GetPhotosList", out ht, PageIndex, PageSize, BookID, KeyWords, TotalCount);
        if (ht.Count > 0)
        {
            TotalCount = Convert.ToInt32(ht["@TotalCount"] ?? "0");
        }
        return dt;
    }

    /// <summary>
    /// 保存相册
    /// </summary>
    /// <param name="id">id 新增为0</param>
    /// <param name="UserID">用户ID</param>
    /// <param name="Name">相册名称</param>
    /// <param name="IsPublic">是否公开</param>
    /// <param name="ShowIndex">排序 默认0</param>
    /// <param name="Remark">相关描述</param>
    /// <returns></returns>
    public static int SavePhotoBook(int id, int UserID, string Name, int IsPublic, int ShowIndex, string Remark)
    {
        int row = DataConnect.Data.ExecuteSP("p_Admin_SavePhotoBook", id, UserID, Name, IsPublic, ShowIndex, Remark);
        return row;
    }
    /// <summary>
    /// 增加图片
    /// </summary>
    /// <param name="UserID">用户ID</param>
    /// <param name="BookID">相册ID 默认0</param>

    /// <param name="FileName">文件名称</param>
    /// <param name="Size">大小</param>
    /// <param name="Extension">后缀名</param>
    /// <param name="Tn">微缩图</param>
    /// <param name="Show">显示图</param>
    /// <param name="Orig">原图</param>
    /// <param name="Source">来源</param>
    /// <returns></returns>
    public static int AddPhoto(int UserID, int BookID, string FileName, double Size, string Extension, string Tn, string Show, string Orig, string Source)
    {
        int row = DataConnect.Data.ExecuteSP("p_Admin_AddPhoto", UserID, BookID, FileName, Size, Extension, Tn, Show, Orig, Source);
        return row;
    }
    /// <summary>
    /// 修改图片
    /// </summary>
    ///  <param name="ID">图片ID</param>
    /// <param name="BookID">相册ID 默认0</param>
    /// <param name="UserID">用户ID</param>
    /// <param name="FileName">文件名称</param>
    /// <param name="Size">大小</param>
    /// <param name="Extension">后缀名</param>
    /// <param name="Tn">微缩图</param>
    /// <param name="Show">显示图</param>
    /// <param name="Orig">原图</param>
    /// <param name="Source">来源</param>
    /// <returns></returns>
    public static int UpdatePhoto(int ID, int BookID, int UserID, string FileName, double Size, string Extension, string Tn, string Show, string Orig, string Source)
    {
        int row = DataConnect.Data.ExecuteSP("p_Admin_UpdatePhoto", ID, BookID, UserID, FileName, Size, Extension, Tn, Show, Orig, Source);
        return row;
    }
    /// <summary>
    /// 移动
    /// </summary>
    /// <param name="id">图片ID</param>
    /// <param name="BookID">目标相册</param>
    /// <returns></returns>
    public static int MovePhoto(int id, int BookID)
    {
        int row = DataConnect.Data.ExecuteSP("p_Admin_MovePhotoToBook", id, BookID);
        return row;
    }

    /// <summary>
    /// 删除相册
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public static int DelPhotoBook(int id)
    {
        int row = DataConnect.Data.ExecuteSP("p_Admin_DelPhotoBook", id);
        return row;
    }
    /// <summary>
    /// 删除图片
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public static int DelPhoto(int id)
    {
        int row = DataConnect.Data.ExecuteSP("p_Admin_DelPhoto", id);
        return row;
    }
    #endregion

    #region 前台相关

    public static DataTable GetHomeScrollProcducts(int size)
    {
        return DataConnect.Data.ExecuteDataTable("p_PC_GetHomeScrollProcducts", size);
    }

    public static DataTable GetHomeTopProcducts(int size)
    {
        return DataConnect.Data.ExecuteDataTable("p_PC_GetHomeTopProcducts", size);
    }

    public static DataTable GetHomeArticles(int size)
    {
        return DataConnect.Data.ExecuteDataTable("p_PC_GetHomeArticles", size);
    }
    /// <summary>
    /// 获取 留言列表
    /// </summary>
    /// <param name="page"></param>
    /// <param name="size"></param>
    /// <param name="total">总数</param>
    /// <returns></returns>
    public static DataTable GetLeaveCommentsList(int page, int size, out int total)
    {
        Hashtable ht = new Hashtable();
        total = 0;
        DataTable dt = DataConnect.Data.ExecuteDataTable("p_PC_GetLeaveCommentsList", out ht, page, size, 0);
        if (ht.Count > 0)
        {
            total = Convert.ToInt32(ht["@TotalCount"]);
        }
        return dt;
    }

    /// <summary>
    /// 留言
    /// </summary>
    /// <param name="UserID"></param>
    /// <param name="Title"></param>
    /// <param name="Type"></param>
    /// <param name="Content"></param>
    /// <param name="IP"></param>
    /// <param name="Browser"></param>
    /// <param name="BrowserType"></param>
    /// <param name="Contacts"></param>
    /// <returns></returns>
    public static int AddLeaveComments(int UserID, string Title, int Type, string Content, string IP, string Browser, string BrowserType, string Contacts)
    {
        return DataConnect.Data.ExecuteSP("p_PC_AddLeaveComments", UserID, Title, Type, Content, IP, Browser, BrowserType, Contacts);
    }
    #endregion






  
}