using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class admin_products_ActionProcduct : AjaxBase
{
    int res = 0;
    object desc = "操作失败！";
    protected override object loadAjaxData()
    {
        string action = Request["action"] ?? "";
        if (Request.HttpMethod == "POST")
        {
            switch (action)
            {
                case "SetIsScroll":
                    SetIsScroll();
                    break;
                case "SetIsHomeTop":
                    SetIsHomeTop();
                    break;
                default:
                    break;
            }
        }
        else
        {
            desc = "请用POST方式提交";
        }
        return new { res = res, desc = desc };
    }

    private void SetIsHomeTop()
    {
        int id = 0;
        int.TryParse(Request["id"], out id);
        res = WSCommon.SetIsHomeTop(id);
        desc = "操作成功";
    }

    private void SetIsScroll()
    {
        int id = 0;
        int.TryParse(Request["id"], out id);
        res = WSCommon.SetIsScroll(id);
        desc = "操作成功";
    }

}