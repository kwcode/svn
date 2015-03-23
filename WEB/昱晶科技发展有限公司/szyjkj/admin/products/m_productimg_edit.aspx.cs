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

        if (dtProduct != null && dtProduct.Rows.Count > 0)
        {
            string json = string.Format("var jsonproduct = {0};", Newtonsoft.Json.JsonConvert.SerializeObject(dtProduct));
            this.Page.ClientScript.RegisterStartupScript(this.GetType(), "script", json, true);
        }
    }
}