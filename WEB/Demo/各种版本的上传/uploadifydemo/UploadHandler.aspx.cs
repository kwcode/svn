using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UploadHandler : System.Web.UI.Page
{
    int _res = 0;//0失败 1成功
    object _msg = "上传失败！";//消息
    string _path = "/";//相对路径
    protected void Page_Load(object sender, EventArgs e)
    {
        //Response.ContentType = "text/plain";
        //Response.Charset = "utf-8";

        HttpPostedFile file = Request.Files["Filedata"];
        string ext = System.IO.Path.GetExtension(file.FileName);
        string folder = Request["folder"] ?? "uploads";
        string uploadPath = HttpContext.Current.Server.MapPath(folder);
        string codestr = GetCodeString(8);
        string path = Path.Combine(uploadPath, codestr + ext);

        if (file != null)
        {
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            file.SaveAs(path);
            _res = 1;
            _msg = "上传成功！";
            _path = "/" + folder + "/" + codestr + ext;
            Response.Clear();
            Response.ContentType = "application/json";
            string json = "{\"res\": " + _res + ", \"path\":\"" + _path + "\", \"msg\": \"" + _msg + "\"}";
            Response.Write(json);
            Response.End();
        }
        else
        {
            Response.Clear();
            Response.ContentType = "application/json";
            string json = "{\"res\": " + _res + ", \"path\":\"" + _path + "\", \"msg\": \"" + _msg + "\"}";
            Response.Write(json);
            Response.End();
        }

    }
    //获取随机数字
    //获取随机验证码字符
    public string GetCodeString(int length)
    {
        string[] verifycodeRange = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "a", "b", "c", "d", "e", "f", "g", "h", "j", "k", "m", "n", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y" };
        Random rnd = new Random();
        string result = "";
        for (int i = 0; i < length; i++)
        {
            int r = rnd.Next(0, verifycodeRange.Length);
            result += verifycodeRange[r];
        }
        return result;
    }
}