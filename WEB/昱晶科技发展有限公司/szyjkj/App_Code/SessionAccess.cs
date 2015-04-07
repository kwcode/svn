using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

/// <summary>
/// SessionAccess 的摘要说明
/// </summary>
public class SessionAccess
{
    private class SessionKey
    {

        public const string UserID = "UserID";
        public const string NickName = "NickName";
        public const string UserType = "UserType";

    }
    private static HttpSessionState Session
    {
        get
        {
            return HttpContext.Current.Session;
        }
    }
    /// <summary>
    /// 当前登录的用户ID
    /// </summary>
    public static int UserId
    {
        get
        {
            return Convert.ToInt32(Session[SessionKey.UserID] ?? "0");
        }
        set
        {
            Session[SessionKey.UserID] = value;
        }
    }
    /// <summary>
    /// 当前登录的昵称
    /// </summary>
    public static string NickName
    {
        get
        {
            return (Session[SessionKey.NickName] ?? "").ToString();
        }
        set
        {
            Session[SessionKey.NickName] = value.ToString();
        }
    }

    public static string UserType
    {
        get
        {
            return (Session[SessionKey.UserType] ?? "").ToString();
        }
        set
        {
            Session[SessionKey.UserType] = value.ToString();
        }
    }

    public static bool IsLogin
    {
        get
        {
            return UserId > 0;
        }
    }
}