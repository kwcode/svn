using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class master_uc_n_uc_banner : System.Web.UI.UserControl
{
    public DataTable DtBanner { get; set; }
    protected void Page_Load(object sender, EventArgs e)
    {
        int total = 0;
        DtBanner = WSCommon.GetBanner(1, 10, "", out total);
        string json = string.Format("var jsonbanner = {0};", Newtonsoft.Json.JsonConvert.SerializeObject(DtBanner));
        this.Page.ClientScript.RegisterStartupScript(this.GetType(), "script", json, true);
    }
}