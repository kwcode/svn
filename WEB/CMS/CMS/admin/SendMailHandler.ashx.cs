using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMS.admin
{
    /// <summary>
    /// SendMailHandler 的摘要说明
    /// </summary>
    public class SendMailHandler : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            //context.Response.ContentType = "text/plain";
            //context.Response.Write("Hello World");
            if (context.Request["Action"] != null)
            {
                string action = context.Request["Action"].ToString();
                if (action == "SendMail")
                {
                    string data = context.Request["list"].ToString();
                    context.Response.Write("SendMail");
                }
                if (action == "GetData")
                {
                    string jsonst = "{[{\"name\": \"tkw\"}, { \"name\": \"qy\"}  ]}";
                    context.Response.Write(jsonst);
                }
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}