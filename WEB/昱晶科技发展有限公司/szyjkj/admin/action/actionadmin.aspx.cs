using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class admin_action_actionadmin : AjaxBase
{
    int res = 0;
    object desc = "操作失败！";
    protected override object loadAjaxData()
    {
        string action = Request["action"] ?? "";
        switch (action)
        {
            case "saveaboutme":
                saveaboutme();
                break;
            case "saverelation":
                saverelation();
                break;
            case "saverebanner":
                saverebanner();
                break;
            default:
                break;
        }
        return new { res = res, desc = desc };
    }

    private void saverebanner()
    {
        string imgaddress = Server.UrlDecode(Request["img"] ?? "");
        int id = 0;
        int.TryParse(Request["id"] ?? "0", out id);
        int imgtype = 0;
        int.TryParse(Request["imgtype"] ?? "0", out imgtype);
        string title = Request["title"] ?? "";
        int showindex = 0;
        int.TryParse(Request["showindex"] ?? "0", out showindex);
        string imgpath = "/images/nophoto1.jpg";
        if (imgtype == 64)
        {
            imgaddress = Server.UrlDecode(imgaddress);
            string[] paths = imgaddress.Split(',');
            System.Drawing.Image img = Base64ToImage(paths[1]);

            string imgspath = SingleFileUpload.GetServiceUrl(SingleFileUpload.fileNameIndex.Blogroll);
            string fileName = stringCoding.RndNum(8);
            #region 获取后缀名
            string extension = ".jpg";
            if (paths[0].Contains("jpeg"))
                extension = ".jpg";
            else if ((paths[0].Contains("png")))
                extension = ".png";
            else if ((paths[0].Contains("gif")))
                extension = ".gif";
            #endregion
            img.Save(Server.MapPath(imgspath + fileName + extension));
            imgpath = imgspath + fileName + extension;

        }
        else
        {
            imgpath = imgaddress;
        }
        if (id > 0)
        {
            res = WSCommon.UpdateBanner(id, title, imgpath, showindex);
            if (res > 0)
                desc = "修改成功！";
            else
                desc = "修改失败！";
        }
        else
        {
            res = WSCommon.AddBanner(title, imgpath, showindex);
            if (res > 0)
                desc = "新增成功！";
            else
                desc = "新增失败！";
        }

    }

    public System.Drawing.Image Base64ToImage(string base64String)
    {
        byte[] imageBytes = Convert.FromBase64String(base64String);
        MemoryStream ms = new MemoryStream(imageBytes, 0,
          imageBytes.Length);
        ms.Write(imageBytes, 0, imageBytes.Length);
        System.Drawing.Image image = System.Drawing.Image.FromStream(ms, true);
        return image;
    }
    private void saverelation()
    {

        string details = Server.UrlDecode(Request["details"] ?? "");

        int rows = WSCommon.SaveRelation(details);
        if (rows == 0)
        {
            desc = "修改失败！";
        }
        else { res = 1; desc = "修改成功"; }
    }

    private void saveaboutme()
    {
        string summary = Request["summary"] ?? "";
        string details = Server.UrlDecode(Request["details"] ?? "");

        int rows = WSCommon.SaveAboutMe(summary, details);
        if (rows == 0)
        {
            desc = "修改失败！";
        }
        else { res = 1; desc = "修改成功"; }

    }
}