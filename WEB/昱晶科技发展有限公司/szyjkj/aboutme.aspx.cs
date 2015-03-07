using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class aboutme : System.Web.UI.Page
{
    public string Details { get; set; }
    protected void Page_Load(object sender, EventArgs e)
    {
        DataTable dt = WSCommon.GetAboutMe();
        if (dt != null && dt.Rows.Count > 0)
        {
            Details = dt.Rows[0]["Details"].ToString();
        }
    }
}