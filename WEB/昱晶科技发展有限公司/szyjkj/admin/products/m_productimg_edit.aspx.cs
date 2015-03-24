using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class admin_products_m_productimg_edit : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        DataTable dtProduct = WSCommon.GetProduct(100);
        string json = "";
        if (dtProduct != null && dtProduct.Rows.Count > 0)
        {
            json += string.Format("var jsonproduct = {0};", Newtonsoft.Json.JsonConvert.SerializeObject(dtProduct));
        }

        //
        int id = 0;
        int.TryParse(Request["id"] ?? "0", out id);
        if (id > 0)
        {
            DataTable dtProImg = WSCommon.GetProductImgsByid(id);
            json += string.Format("var jsonprocimg = {0};", Newtonsoft.Json.JsonConvert.SerializeObject(dtProImg));
        }
        ID = id;
        this.Page.ClientScript.RegisterStartupScript(this.GetType(), "script", json, true);
    }
    public int ID { get; set; }
}