using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class admin_m_BaseSettings : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        DataTable dtBase = WSCommon.GetSiteBaseSettings();
        string json = string.Format("var jsonbase={0};", Newtonsoft.Json.JsonConvert.SerializeObject(dtBase));
        this.Page.ClientScript.RegisterStartupScript(this.GetType(), "script", json, true);
    }
}