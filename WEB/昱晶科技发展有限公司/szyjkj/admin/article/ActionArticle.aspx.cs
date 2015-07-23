using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class admin_article_ActionArticle : AjaxBase
{
    int res = 0;
    object desc = "操作失败！";
    protected override object loadAjaxData()
    {
        string action = Request["action"] ?? "";
        switch (action)
        {
            case "SaveArticleType":
                SaveArticleType();
                break;
            case "DelArticleType":
                DelArticleType();
                break;

            case "SaveArticle":
                SaveArticle();
                break;
            case "DelArticle":
                DelArticle();
                break;

            default:
                break;
        }
        return new { res = res, desc = desc };
    }

    private void DelArticle()
    {
        int id = 0;
        int.TryParse(Request["id"] ?? "0", out id);
        res = WSCommon.DelArticle(id);
        if (res > 0)
            desc = "删除成功";
        else
            desc = "删除失败";
    }

    private void SaveArticle()
    {
        int id = 0;
        int.TryParse(Request["id"] ?? "0", out id);
        int showindex = 0;
        int.TryParse(Request["showindex" ?? "0"], out  showindex);
        int typeid = 0;
        int.TryParse(Request["typeid"] ?? "0", out typeid);
        string title = Request["title"] ?? "";
        string content = Server.UrlDecode(Request["content"] ?? "");
        string imgurl = Request["imgurl"] ?? "";
        string summary = Request["summary"] ?? "";
        if (id > 0)
        {
            res = WSCommon.UpdateArticle(id, title, content, UserID, showindex, typeid, imgurl, summary);
            if (res > 0)
                desc = "修改成功";
            else
                desc = "修改失败";
        }
        else
        {
            res = WSCommon.AddArticle(title, content, UserID, showindex, typeid, imgurl, summary);
            if (res > 0)
                desc = "添加成功";
            else
                desc = "添加失败";
        }
    }

    private void DelArticleType()
    {
        int id = 0;
        int.TryParse(Request["id"] ?? "0", out id);
        res = WSCommon.DelArticleType(id);
        if (res > 0)
            desc = "删除成功";
        else
            desc = "删除失败";
    }

    private void SaveArticleType()
    {
        int id = 0;
        int.TryParse(Request["id"] ?? "0", out id);
        int showindex = 0;
        int.TryParse(Request["showindex" ?? "0"], out  showindex);
        string name = Request["name"] ?? "";
        if (id == 0)
        {
            res = WSCommon.SaveArticleType(id, name, showindex);
            if (res > 0)
                desc = "新增成功";
            else
                desc = "新增失败";
        }
        else//修改
        {
            res = WSCommon.SaveArticleType(id, name, showindex);
            if (res > 0)
                desc = "修改成功";

            else desc = "修改失败";
        }

    }
}