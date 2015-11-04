using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace 外链图片
{
    /// <summary>
    /// demoHandler 的摘要说明
    /// </summary>
    public class demoHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            //context.Response.ContentType = "text/plain";
            //context.Response.Write("Hello World");
            string action = context.Request["action"] ?? "";
            if (action == "SaveDemo")
            {
                string content = context.Request["content"] ?? "";
                content = context.Server.UrlDecode(content);
                string path = context.Server.MapPath("~/demo.txt");
                using (FileStream fs = File.Open(path, FileMode.OpenOrCreate))
                {
                    byte[] myByte = System.Text.Encoding.UTF8.GetBytes(content);
                    fs.Write(myByte, 0, myByte.Length);
                }
                context.Response.Write("OK");
            }
        }
        public string SaveOutLinkImg(string content)
        {
            string outstr = "";
            return outstr;
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