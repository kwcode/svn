using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class uploadhandler : System.Web.UI.Page
{
    int _res = 0;//0失败 1成功
    object _msg = "上传失败！";//消息
    string _path = "/";//相对路径
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            HttpPostedFile file = Request.Files["Filedata"];
            string ext = System.IO.Path.GetExtension(file.FileName);//后缀名 如：.jpg
            string name = GetCodeString(8);//随机8位
            string folder = Request["folder"] ?? "~/uploads/" + DateTime.Now.ToString("yyyy/MM/dd"); //新版取消了folder 不安全属性所以 存放路径自己定义 
            string savepath = HttpContext.Current.Server.MapPath(folder);
            string path = Path.Combine(savepath, name + ext);
            if (file != null)
            {
                if (!Directory.Exists(savepath))
                    Directory.CreateDirectory(savepath);
                file.SaveAs(path);//文件保存
                _path = (folder + "/" + name + ext).Replace("~", "");
                _res = 1;
                _msg = "上传成功！";
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
        catch (Exception ex)
        {
            Response.Clear();
            Response.ContentType = "application/json";
            string json = "{\"res\": " + _res + ", \"path\":\"" + _path + "\", \"msg\": \"" + ex.Message + "\"}";
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

    public void ResponseWrite(HttpResponse _Response, int res, string path, object msg)
    {
        _Response.Clear();
        _Response.ContentType = "application/json";
        string json = "{\"res\": " + res + ", \"path\":\"" + path + "\", \"msg\": \"" + msg + "\"}";
        _Response.Write(json);
        _Response.End();
    }

}