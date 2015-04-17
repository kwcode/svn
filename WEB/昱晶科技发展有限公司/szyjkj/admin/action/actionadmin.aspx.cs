using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class admin_action_actionadmin : AjaxBase
{
    int res = 0;
    object desc = "异常错误代码 -1";

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
            case "savereproduct":
                savereproduct();
                break;
            case "savereproductimg":
                savereproductimg();
                break;
            case "delproductimg":
                delproductimg();
                break;
            case "delproduct":
                delproduct();
                break;
            case "delnews":
                delnews();
                break;
            case "adduser":
                adduser();
                break;
            case "upduser":
                upduser();
                break;
            case "outuser":
                outuser();
                break;
            case "deluser":
                deluser();
                break;
            case "delbanner":
                delbanner();
                break;
            default:
                break;
        }
        return new { res = res, desc = desc };
    }

    private void delbanner()
    {
        int id = 0;
        int.TryParse(Request["id"] ?? "0", out id);
        res = WSCommon.DelBanner(id);
        if (res > 0)
            desc = "删除成功！";
        else
            desc = "删除失败！";
    }

    private void upduser()
    {
        int userid = 0;
        int.TryParse(Request["id"], out userid);
        string loginname = Request["loginname"] ?? "";
        string nickname = Request["nickname"] ?? "";
        string role = Request["role"] ?? "0";
        string pwd = Request["pwd"] ?? "";
        string password = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(pwd, "md5").ToLower();
        WSCommon.UpdateUser(userid, loginname, password, nickname, Convert.ToInt32(role));
        res = 1;
        desc = "修改成功！";
    }

    private void deluser()
    {
        string json = Request["jsonuserids"] ?? "";
        List<int> userids = Newtonsoft.Json.JsonConvert.DeserializeObject<List<int>>(json);
        foreach (int item in userids)
        {
            res = WSCommon.DelUser(item);
            if (res > 0)
                desc = "删除成功！";
            else
                desc = "删除失败！";
        }
    }

    private void outuser()
    {
        SessionAccess.UserId = 0;
        SessionAccess.UserType = "0";
        SessionAccess.NickName = "";
        SessionAccess.LoginName = "";
        res = 1;
        desc = "注销成功！";

    }

    private void adduser()
    {
        string loginname = Request["loginname"] ?? "";
        string nickname = Request["nickname"] ?? "";
        string role = Request["role"] ?? "0";
        string pwd = Request["pwd"] ?? "";
        string password = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(pwd, "md5").ToLower();
        WSCommon.AddUser(loginname, password, nickname, Convert.ToInt32(role));
        res = 1;
        desc = "新增成功！";
    }

    private void delnews()
    {
        int id = 0;
        int.TryParse(Request["id"] ?? "0", out id);
        res = WSCommon.DelNews(id);
        if (res > 0)
            desc = "删除成功！";
        else
            desc = "删除失败！";
    }

    private void delproduct()
    {
        int id = 0;
        int.TryParse(Request["id"] ?? "0", out id);
        res = WSCommon.DelProduct(id);
        if (res > 0)
            desc = "删除成功！";
        else
            desc = "删除失败！";
    }

    private void delproductimg()
    {
        int id = 0;
        int.TryParse(Request["id"] ?? "0", out id);
        res = WSCommon.DelProcductImg(id);
        if (res > 0)
            desc = "删除成功！";
        else
            desc = "删除失败！";
    }

    private void savereproductimg()
    {
        string imgaddress = Server.UrlDecode(Request["img"] ?? "");
        int id = 0;
        int.TryParse(Request["id"] ?? "0", out id);
        int imgtype = 0;
        int.TryParse(Request["imgtype"] ?? "0", out imgtype);
        string title = Request["title"] ?? "";
        int showindex = 0;
        int.TryParse(Request["showindex"] ?? "0", out showindex);
        int pid = 0;
        int.TryParse(Request["pid"] ?? "0", out pid);
        string imgpath = "/images/nophoto1.jpg";
        if (imgtype == 64)
        {
            string[] paths = imgaddress.Split(',');
            System.Drawing.Image img = Base64ToImage(paths[1]);

            string imgspath = SingleFileUpload.GetServiceUrl(SingleFileUpload.fileNameIndex.Banner);
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
        res = WSCommon.SaveProductImg(id, pid, imgpath, 0, title, showindex);
        if (res > 0)
        {
            desc = "操作成功！";
        }
        else
        {
            desc = "操作失败！";
        }
    }

    private void savereproduct()
    {
        int id = 0;
        int.TryParse(Request["id"] ?? "0", out id);
        string title = Request["title"] ?? "";
        int showindex = 0;
        int.TryParse(Request["showindex"] ?? "0", out showindex);
        string summary = Request["summary"] ?? "";

        res = WSCommon.SaveProduct(id, title, summary, showindex, 0);
        if (res > 0)
        {
            desc = "操作成功！";
        }
        else
        {
            desc = "操作失败！";
        }
    }

    private void saverebanner()
    {
        string imgaddress = Server.UrlDecode(Request["img"] ?? "");
        int id = 0;
        int.TryParse(Request["id"] ?? "0", out id);
        int imgtype = 0;
        int.TryParse(Request["imgtype"] ?? "0", out imgtype);
        string title = Request["title"] ?? "";
        string url = Request["url"] ?? "";
        int showindex = 0;
        int.TryParse(Request["showindex"] ?? "0", out showindex);
        string imgpath = "/images/nophoto1.jpg";
        if (imgtype == 64)
        {
            string[] paths = imgaddress.Split(',');
            System.Drawing.Image img = Base64ToImage(paths[1]);

            string imgspath = SingleFileUpload.GetServiceUrl(SingleFileUpload.fileNameIndex.Banner);
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
            res = WSCommon.UpdateBanner(id, title, imgpath, showindex, url);
            if (res > 0)
                desc = "修改成功！";
            else
                desc = "修改失败！";
        }
        else
        {
            res = WSCommon.AddBanner(title, imgpath, showindex, url);
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