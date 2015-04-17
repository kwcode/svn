using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class admin_m_banner_edit : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        int id = 0;
        int.TryParse(Request["id"] ?? "0", out id);
        if (id > 0)
        {
            DataTable dtProImg = WSCommon.GetBannerByID(id);
            string json = string.Format("var jsonbanner = {0};", Newtonsoft.Json.JsonConvert.SerializeObject(dtProImg));
            this.Page.ClientScript.RegisterStartupScript(this.GetType(), "script", json, true);
        }
        ID = id;
    }
    public int ID { get; set; }
}