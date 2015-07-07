using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// HTTPS 的摘要说明
/// </summary>
public class HTTPS
{
    public HTTPS()
    {

    }
    /// <summary>
    /// 穿过代理服务器取远程用户真实IP地址
    /// </summary>
    public static string GetIP
    {
        get
        {
            if (System.Web.HttpContext.Current.Request.ServerVariables["HTTP_VIA"] != null)
            {
                if (System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].Length > 0)
                {
                    return System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].Split(new char[] { ',' })[0];
                }
                else
                {
                    return "";
                }
            }
            else
                return System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
        }
    }

}