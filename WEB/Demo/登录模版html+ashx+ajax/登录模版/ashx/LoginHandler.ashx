<%@ WebHandler Language="C#" Class="LoginHandler" %>

using System;
using System.Web;

public class LoginHandler : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/plain";
        //context.Response.Write("Hello World");
        string action = context.Request["Action"].ToString();
        if (action == "Login")
        {
            string uid = context.Request["username"];
            string pwd = context.Request["password"];
            LoginMsg msg = Login(uid, pwd);
            if (msg.issure == true)
            {
                string jsonstr = ConvertJson(msg);
                context.Response.Write(jsonstr);
            }
            else
            {
                string jsonstr = ConvertJson(msg);
                context.Response.Write(jsonstr);
            }
        }
    }
    public LoginMsg Login(string uid, string pwd)
    {
        LoginMsg msg = new LoginMsg();
        if (uid.Equals("admin", StringComparison.OrdinalIgnoreCase) && pwd.Equals("admin", StringComparison.OrdinalIgnoreCase))
        {
            msg.issure = true;
            msg.msg = "成功登录";
        }
        else
        {
            msg.issure = false;
            msg.msg = "登录失败，检查用户帐号";
        }
        return msg;
    }
    private string ConvertJson(LoginMsg msg)
    {
        string jsonstr = "";
        //jsonstr += "[";
        jsonstr += "{ \"issure\": " + msg.issure + ", \"msg\": \"" + msg.msg + "\"},";
        jsonstr = jsonstr.Remove(jsonstr.Length - 1, 1);
        // jsonstr += "]";
        jsonstr = jsonstr.ToLower();
        return jsonstr;
    }
    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}
public class LoginMsg
{
    public bool issure { get; set; }
    public string msg { get; set; }
}