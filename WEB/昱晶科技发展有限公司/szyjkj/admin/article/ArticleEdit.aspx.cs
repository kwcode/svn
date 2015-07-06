using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class admin_article_ArticleEdit : PageBase
{
    protected int ID { get; set; }
    protected void Page_Load(object sender, EventArgs e)
    {
        int id = 0;
        int.TryParse(Request["id"] ?? "0", out id);
        ID = id;
        DataTable dt = WSCommon.GetArticleByID(ID);
        int total = 0;
        DataTable dtArticleType = WSCommon.GetArticleTypeList(1, 100, "", out total);
        string json = string.Format("var jsonarticletype={0};", Newtonsoft.Json.JsonConvert.SerializeObject(dtArticleType));
        if (dt != null && dt.Rows.Count > 0)
        {
            json += string.Format("var jsonarticle = {0};", Newtonsoft.Json.JsonConvert.SerializeObject(dt));
        }

        this.Page.ClientScript.RegisterStartupScript(this.GetType(), "script", json, true);
    }
}