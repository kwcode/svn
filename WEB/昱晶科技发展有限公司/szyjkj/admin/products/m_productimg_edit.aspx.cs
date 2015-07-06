using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class admin_products_m_productimg_edit : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Pid = Convert.ToInt32(Request["pid"] ?? "0");

        int total = 0;
        DataTable dtT = WSCommon.GetProductTypeList(1, 100, "", out total);
        if (dtT == null || dtT.Rows.Count == 0)
        {
            Response.Redirect("/admin/procducts/m_product_index.aspx");
            return;
        }
        string json = string.Format("var jsonproctype={0};", Newtonsoft.Json.JsonConvert.SerializeObject(dtT));
        //
        int id = 0;
        int.TryParse(Request["id"] ?? "0", out id);
        if (id > 0)
        {
            DataTable dtProImg = WSCommon.GetProductImgsByid(id);
            json += string.Format("var jsonprocimg = {0};", Newtonsoft.Json.JsonConvert.SerializeObject(dtProImg));

        }
        this.Page.ClientScript.RegisterStartupScript(this.GetType(), "script", json, true);
        ID = id;
    }
    public int ID { get; set; }
    public int Pid { get; set; }
}