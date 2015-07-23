using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class admin_JianJie_JianjieEdit : System.Web.UI.Page
{
    protected int ID { get; set; }
    protected void Page_Load(object sender, EventArgs e)
    {
        int id = 0;
        int.TryParse(Request["id"] ?? "0", out id);
        ID = id;

        DataTable dt = WSCommon.GetAboutByID(id);
        string json = string.Format("var jsonjj={0};", Newtonsoft.Json.JsonConvert.SerializeObject(dt));  
        this.Page.ClientScript.RegisterStartupScript(this.GetType(), "script", json, true);
    }
}