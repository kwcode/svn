using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
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
                //保存外链图片
                content = SaveOutLinkImg(content);
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
            string mydomain = HttpContext.Current.Request.Url.Host;
            HashSet<string> images = new HashSet<string>();
            Regex regImg = new Regex(@"<img\b[^<>]*?\bsrc[\s\t\r\n]*=[\s\t\r\n]*[""']?[\s\t\r\n]*(?<imgUrl>[^\s\t\r\n""'<>]*)[^<>]*?/?[\s\t\r\n]*>", RegexOptions.IgnoreCase);
            Regex regUrl = new Regex("(https?://[^\"]+)", RegexOptions.IgnoreCase);
            var ms = regImg.Matches(content);
            Match m = null;
            string url = null;
            foreach (Match item in ms)
            {
                if (item.Success)
                {
                    url = item.Groups[1].Value;
                    m = regUrl.Match(url);
                    if (m.Success && !m.Groups[1].Value.Equals(mydomain, StringComparison.OrdinalIgnoreCase) && !images.Contains(url))
                    {
                        images.Add(url);
                    }
                }
            }

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