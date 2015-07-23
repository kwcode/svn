using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class admin_JianJie_ActionJianJie : AjaxBase
{
    int res = 0;
    object desc = "操作失败！";
    protected override object loadAjaxData()
    {
        string action = Request["action"] ?? "";
        switch (action)
        {
            case "SaveJianJie":
                SaveJianJie();
                break;
            case "SetIsHomeTop":
                SetIsHomeTop();
                break;
            case "delJianJie":
                delJianJie();
                break;
            default:
                break;
        }
        return new { res = res, desc = desc };
    }

    private void delJianJie()
    {
        int id = 0;
        int.TryParse(Request["id"] ?? "0", out id);
        res = WSCommon.DelJianJie(id);
    }

    private void SetIsHomeTop()
    {
        int id = 0;
        int.TryParse(Request["id"] ?? "0", out id);
        res = WSCommon.SetJianjieIsHomeTop(id);
    }

    private void SaveJianJie()
    {
        int id = 0;
        int.TryParse(Request["id"] ?? "0", out id);
        int showindex = 0;
        int.TryParse(Request["showindex" ?? "0"], out  showindex);

        string typename = Request["typename"] ?? "";
        string content = Server.UrlDecode(Request["content"] ?? "");
        string imgurl = Request["imgurl"] ?? "";
        string summary = Request["summary"] ?? "";
        res = WSCommon.SaveAboutMe(id, typename, summary, content, imgurl, showindex);

    }
}