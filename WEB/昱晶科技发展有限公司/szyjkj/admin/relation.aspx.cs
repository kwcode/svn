using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class admin_relation : System.Web.UI.Page
{

    //public string Details { get; set; }
    protected void Page_Load(object sender, EventArgs e)
    {
        DataTable dt = WSCommon.GetRelation();

        if (dt != null && dt.Rows.Count > 0)
        {
            string json = string.Format("var jsonrelation = {0};", Newtonsoft.Json.JsonConvert.SerializeObject(dt));
            this.Page.ClientScript.RegisterStartupScript(this.GetType(), "script", json, true);
        }
    }
}