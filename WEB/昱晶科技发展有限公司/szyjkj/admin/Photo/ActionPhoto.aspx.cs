using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class admin_Photo_ActionPhoto : AjaxBase
{
    int res = 0;
    object desc = "异常错误代码 -1";
    protected override object loadAjaxData()
    {
        if (Request.HttpMethod == "POST")
        {
            string action = Request["action"] ?? "";
            switch (action)
            {
                case "SavePhotoBook":
                    SavePhotoBook();
                    break;
                case "DelPhotoBook":
                    DelPhotoBook();
                    break;
                case "DelPhoto":
                    DelPhoto();
                    break;
            }
        }
        else
        {
            desc = "提交方式需要POST";
        }
        return new { res = res, desc = desc };
    }

    private void DelPhoto()
    {
        int id = 0;
        int.TryParse(Request["id"] ?? "0", out id);
        res = WSCommon.DelPhoto(id);
        if (res > 0)
            desc = "删除成功";
        else
            desc = "删除失败";
    }

    private void DelPhotoBook()
    {
        int id = 0;
        int.TryParse(Request["id"] ?? "0", out id);
        res = WSCommon.DelPhotoBook(id);
        if (res > 0)
            desc = "删除成功";
        else
            desc = "删除失败";
    }

    private void SavePhotoBook()
    {
        int id = 0;
        int.TryParse(Request["id"] ?? "0", out id);
        string Name = Request["Name"] ?? "";
        int IsPublic = 0;
        int.TryParse(Request["IsPublic"] ?? "0", out IsPublic);
        int ShowIndex = 0;
        int.TryParse(Request["ShowIndex"] ?? "0", out ShowIndex);
        string Remark = Request["Remark"] ?? "";
        res = WSCommon.SavePhotoBook(id, UserID, Name, IsPublic, ShowIndex, Remark);
        if (res > 0)
            desc = "保存成功";
        else
            desc = "保存失败";
    }
}