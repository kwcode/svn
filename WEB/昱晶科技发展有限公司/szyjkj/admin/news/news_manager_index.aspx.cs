using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class admin_news_news_manager_index : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        DataTable dt = WSCommon.GetHomeNews(0);
        repeaterlist.DataSource = dt;
        repeaterlist.DataBind();
    }
}