using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class admin_article_ArticleManager : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.HttpMethod == "POST")
        {
            int typeid = Convert.ToInt32(Request["typeid"] ?? "0");
            int page = Convert.ToInt32(Request["page"] ?? "1");
            int pagesize = Convert.ToInt32(Request["rows"] ?? "1");
            string keywords = Request["keywords"] ?? "";
            int total = 0;
            DataTable dt = WSCommon.GetArticleList(page, pagesize, 0, keywords, out total);
            ResponseJson(new DataGridJson() { total = total, rows = dt });
        }
    }
}