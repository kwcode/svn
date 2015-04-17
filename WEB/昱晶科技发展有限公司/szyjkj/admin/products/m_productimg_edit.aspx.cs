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

        DataTable dtPC = WSCommon.GetProductClassByID(Pid);
        if (dtPC == null || dtPC.Rows.Count == 0)
        {
            Response.Redirect("/admin/procducts/m_product_index.aspx");
            return;
        }
        PName = dtPC.Rows[0]["Title"].ToString(); 
        //
        int id = 0;
        int.TryParse(Request["id"] ?? "0", out id);
        if (id > 0)
        {
            DataTable dtProImg = WSCommon.GetProductImgsByid(id);
            string json = string.Format("var jsonprocimg = {0};", Newtonsoft.Json.JsonConvert.SerializeObject(dtProImg));
            this.Page.ClientScript.RegisterStartupScript(this.GetType(), "script", json, true);   
        }
        ID = id;
    }
    public int ID { get; set; }
    public int Pid { get; set; }
    public string PName { get; set; }
}