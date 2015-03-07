using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class newsdetails : System.Web.UI.Page
{
    public DataTable DtNews { get; set; }
    protected void Page_Load(object sender, EventArgs e)
    {
        int id = Convert.ToInt32(Request["id"] ?? "0");
        DtNews = WSCommon.GetNewsByID(id);
    }
}