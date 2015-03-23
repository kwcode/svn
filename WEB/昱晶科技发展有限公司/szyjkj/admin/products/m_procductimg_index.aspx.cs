using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class admin_products_m_procductimg_index : System.Web.UI.Page
{
    public DataTable DtProcdut { get; set; }
    protected void Page_Load(object sender, EventArgs e)
    {
        DtProcdut = WSCommon.GetProductImgs(0, 20);
    }
}