using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class PC_ActionCommon : AjaxBase
{
    protected override bool NeedLogin
    {
        get
        {
            return false;
        }
    }
    int res = 0;
    object desc = "操作失败！";
    protected override object loadAjaxData()
    {
        string action = Request["action"] ?? "";
        if (Request.HttpMethod == "POST")
        {
            switch (action)
            {
                case "AddLeaveComments":
                    AddLeaveComments();
                    break;
            }
        }
        else
        {
            desc = "请用POST方式提交";
        }
        return new { res = res, desc = desc };
    }

    private void AddLeaveComments()
    {
        string title = Request["title"] ?? "";
        int type = 0;
        int.TryParse(Request["type"] ?? "0", out type);
        string content = Request["content"] ?? "";
        string ip = HTTPS.GetIP;
        string Contacts = Request["contacts"] ?? "";
        res = WSCommon.AddLeaveComments(0, title, type, content, ip, Request.Browser.Browser, Request.Browser.Type, Contacts);
    }
}