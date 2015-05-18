using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class index : System.Web.UI.Page
{
    public DataTable dtNews;
    public DataTable DtProductImgs { get; set; }
    protected void Page_Load(object sender, EventArgs e)
    {
        int total = 0;
        dtNews = WSCommon.GetNews(1, 10, out total);
        DtProductImgs = WSCommon.GetProductImgs(0, 6);
    }
}