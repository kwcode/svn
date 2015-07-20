using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class news : System.Web.UI.Page
{
    public DataTable DtNews { get; set; }

    public int PageSize = 20;
    public int TotalCount { get; set; }
    protected void Page_Load(object sender, EventArgs e)
    {
        int total = 0;
        int page = Convert.ToInt32(Request["page"] ?? "1");
        DtNews = WSCommon.GetArticleList(page, PageSize, 0, "", out total);
        TotalCount = total;
    }
}