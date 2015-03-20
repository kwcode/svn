using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class admin_action_actionupdateimage : System.Web.UI.Page
{
    int res = 0;
    string desc = "操作失败！";
    string message = "aa";
    protected void Page_Load(object sender, EventArgs e)
    {
        string action = Request["action"] ?? "";
        switch (action)
        {
            case "ajaxuploadfile":
                Ajaxuploadfile();
                break;
        }
        string json = Newtonsoft.Json.JsonConvert.SerializeObject(new { res = res, desc = desc });
        Response.Clear();
        Response.Write(json);
        Response.End();
    }
    private void Ajaxuploadfile()
    {
        if (Request.Files.Count > 0)
        {
            HttpPostedFile ajaxFile = Request.Files[0];//runat="server"
            if (ajaxFile.ContentLength < 10485760) //判断文件是否小于10Mb  
            {
                string extension = System.IO.Path.GetExtension(ajaxFile.FileName);//判断扩展名字
                string type = ajaxFile.ContentType;
                if (extension.ToLower() == ".jpg" || extension.ToLower() == ".png" || extension.ToLower() == ".gif" || extension.ToLower() == ".bmp" || extension.ToLower() == ".jpeg")
                {
                    Stream stream = ajaxFile.InputStream;
                    if (stream != null && stream.Length > 0)
                    {
                        stream.Position = 0;
                        using (BinaryReader br = new BinaryReader(stream))
                        {
                            byte[] buff = br.ReadBytes((int)stream.Length);
                            res = 1;
                            desc = "data:" + type + ";base64," + Convert.ToBase64String(buff);
                        }
                    }
                    else
                    {
                        res = 0;
                        desc = "上传失败！";
                    }
                }
                else
                {
                    res = 0;
                    desc = "请上传图片格式";
                }
            }
            else
            {
                res = 0;
                desc = "图片不能超过10MB";
            }
        }
    }
}