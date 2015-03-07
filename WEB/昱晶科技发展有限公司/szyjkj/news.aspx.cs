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
    public int PageIndex { get; set; }
    public int PageSize = 20;
    public int TotalCount { get; set; }
    protected void Page_Load(object sender, EventArgs e)
    {
        int total = 0;
        DtNews = WSCommon.GetNews(PageIndex, PageSize, out total);
        TotalCount = total;
    }
}