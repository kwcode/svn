using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class admin_news_actionnews : AjaxBase
{
    int res = 0;
    object desc = "操作失败！";
    protected override object loadAjaxData()
    {
        string action = Request["action"] ?? "";
        switch (action)
        {
            case "savenew":
                savenew();
                break;
            default:
                break;
        }
        return new { res = res, desc = desc };
    }

    private void savenew()
    {
        int id = 0;
        int.TryParse(Request["id"] ?? "0", out id);
        int showindex = 0;
        int.TryParse(Request["showindex" ?? "0"], out  showindex);
        string title = Request["title"] ?? "";
        string summary = Request["summary"] ?? "";
        string details = Server.UrlDecode(Request["details"] ?? "");
        if (id == 0)
        {
            int rows = WSCommon.AddNews(showindex, title, summary, details);
            if (rows == 0)
            {
                desc = "新增失败！";
            }
            else { res = 1; desc = "新增成功"; }
        }
        else//修改
        {

        }



    }
}