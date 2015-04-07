using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// PageBase 的摘要说明
/// </summary>
public class PageBase : System.Web.UI.Page
{
    /// <summary>
    /// 是否需要登录，默认为需要
    /// </summary>
    /// <returns></returns>
    protected virtual bool NeedLogin
    {
        get
        {
            return true;
        }
    }
    protected override void OnLoad(EventArgs e)
    {

        base.OnLoad(e);
    }

    protected override void OnInit(EventArgs e)
    {
        if (!NeedLogin || !UserIsLogin)
        {
            RedirectToLoginPage();//跳转到登录页面
            return;
        }
        base.OnInit(e);

    }

    /// <summary>
    /// 跳转到登录页面(执行于服务端)
    /// </summary>
    public void RedirectToLoginPage()
    {
        Response.Clear();
        Response.Redirect("/admin/login.aspx", true);
        Response.End();
    }
    #region 用户登录相关
    /// <summary>
    /// 判断用户是否登录
    /// </summary>
    public bool UserIsLogin
    {
        get
        {
            return SessionAccess.IsLogin;
        }
    }
    #endregion
}