using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class action_actionIndex : System.Web.UI.Page
{
    int res = 0;
    string desc = "提交失败！"; 

    protected void Page_Load(object sender, EventArgs e)
    {
        string action = Request["action"] ?? "";
        switch (action)
        {
            case "ajaxuploadfile":

                if (Request.Files.Count > 0)
                {

                    HttpPostedFile ajaxFile = Request.Files[0];//runat="server"
                    if (ajaxFile.ContentLength < 10485760) //判断文件是否小于10Mb  
                    {
                        string fileName = ajaxFile.FileName;
                        ajaxFile.SaveAs(FileUploadHelper.GetFullPath(fileName));
                        //FileUpload fuImage = new FileUpload(); 
                        res = 1;
                        desc = FileUploadHelper.GetPath(fileName);
                    }
                }
                break;
            default:
                break;
        }
        string json = "{\"res\":" + res + ",\"desc\":\"" + desc + "\"}";
        Response.Clear();
        Response.Write(json);
        Response.End();
    }
}