using System;
using System.Collections.Generic;
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
                
            default:
                break;
        }
        return new { res = res, desc = desc };
    }

    private void saverelation()
    {
        
        string details = Server.UrlDecode(Request["details"] ?? "");

        int rows = WSCommon.SaveRelation( details);
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