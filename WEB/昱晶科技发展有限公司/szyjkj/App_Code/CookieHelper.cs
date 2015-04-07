using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

/// <summary>
/// Summary description for CookieHelper
/// </summary>
public class CookieHelper
{
    public const string ASPSessionId = "ASP.NET_SessionId";

    public const string uinfo = "uinfo";

    public const string city = "city";

    /// <summary>
    /// 用于加密Cookie的字符串
    /// </summary>
    private static string EncryKey = ConfigurationManager.AppSettings["TripLoginKey"];

    private static HttpRequest Request
    {
        get
        {
            return HttpContext.Current.Request;
        }
    }

    private static HttpResponse Response
    {
        get
        {
            return HttpContext.Current.Response;
        }
    }

    /// <summary>
    /// 登出时清理Cookie
    /// </summary>
    public static void ClearCookie(bool setDomain)
    {
        string domain = GetCookieDomain();

        foreach (string item in Request.Cookies.AllKeys)
        {
            if (item.Equals(ASPSessionId) || item.Equals(uinfo))
            {
                HttpCookie cookie = Request.Cookies[item];

                cookie.Expires = DateTime.Now.AddDays(-1);

                if (setDomain)
                {
                    cookie.Domain = domain;
                }

                Response.Cookies.Add(cookie);
            }
        }
    }

    /// <summary>
    /// 保存用于自动登录的Cookie
    /// </summary>
    /// <param name="userId"></param>
    public static void SaveUinfoCookie(int userId)
    {
        //cookie 格式为 yyyy-MM-ddUserId
        string strTicket = userId.ToString();
        HttpCookie cookie = new HttpCookie("uinfo", strTicket);
        cookie.Expires = DateTime.Now.AddDays(3);
        cookie.HttpOnly = false;
        cookie.Domain = GetCookieDomain();

        HttpContext.Current.Response.Cookies.Add(cookie);
    }

    /// <summary>
    /// 从自动登录Cookie中读取UserId
    /// </summary>
    /// <returns></returns>
    public static int GetUserIdFromCookie()
    {//cookie 格式为 yyyy-MM-ddUserId
        HttpCookie cookie = Request.Cookies[uinfo];
        if (cookie != null && !string.IsNullOrEmpty(cookie.Value))
        {
            string _userId = cookie.Value;// trip.CommunalClass.stringCoding.DecryString(cookie.Value, EncryKey);

            int userId;
            if (_userId != null && _userId.Length > 10 && int.TryParse(_userId.Substring(10), out userId))
            {
                return userId;
            }
        }
        return 0;
    }

    //private static Regex rDomain = new Regex(@"^([a-z\d\-]+\.){0,1}(([a-z\d\-]+\.)+[a-z\d\-]+)$");

    private static string MainDomain = ConfigurationManager.AppSettings["MainDomain"];

    /// <summary>
    /// 获取Cookie应该放置在哪个域名下面
    /// </summary>
    /// <returns></returns>
    public static string GetCookieDomain(string domain = "")
    {
        domain = string.IsNullOrEmpty(domain) ? Request.Url.Host : domain;
        if (domain.Contains(MainDomain))
        {
            return (MainDomain.StartsWith(".") ? string.Empty : ".") + MainDomain;
        }
        return domain;
    }

    public static void SetSessionCookie()
    {
        string domain = Request.Url.Host;
        if (domain.Contains(MainDomain))
        {
            Request.Cookies[ASPSessionId].Domain = MainDomain;
        }
    }

    /// <summary>
    /// 检查 Cookie 中是否有城市信息, 没有的话读取 IP 并写入
    /// </summary>
    //public static void setCookieCity()
    //{
    //    HttpCookie cookie = Request.Cookies[city];
    //    if (!Trip.HTTPS.IsLocalIP())
    //    {
    //        cookie = new HttpCookie(city);
    //        cookie.Value = HttpUtility.UrlEncodeUnicode(string.Format("{0}|{1}", CurrentCity.CityCode, CurrentCity.CityName));
    //        cookie.Domain = GetCookieDomain();
    //        cookie.Expires = DateTime.Now.AddDays(30);
    //        cookie.HttpOnly = false;
    //        cookie.Path = "/";
    //        Response.Cookies.Add(cookie);
    //    }
    //}
    /// <summary>
    /// 检查 Cookie 中是否有城市信息, 没有的话读取 IP 并写入
    /// </summary>
    public static void setCookieCity(string key, string value)
    {
        HttpCookie cookie = new HttpCookie(key);
        cookie.Value = HttpUtility.UrlEncode(value);
        cookie.Domain = GetCookieDomain();
        cookie.Expires = DateTime.Now.AddDays(1);
        cookie.Path = "/";
        Response.Cookies.Add(cookie);
    }

}