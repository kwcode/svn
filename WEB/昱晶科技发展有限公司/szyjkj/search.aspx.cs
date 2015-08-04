using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class search : System.Web.UI.Page
{
    public DataTable dtSearch { get; set; }
    protected void Page_Load(object sender, EventArgs e)
    {
        dtSearch = DataConnect.Data.GetDataTable("SELECT top 20 * FROM v_search WHERE Title LIKE '%" + keywords + "%'");
    }
    public string keywords
    {
        get
        {
            return Server.UrlDecode(Request["keywords"] ?? "").Trim();
        }
    }
}