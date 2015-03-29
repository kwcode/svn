using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class productdesc : System.Web.UI.Page
{
    public DataTable DtProductdesc { get; set; }
    protected void Page_Load(object sender, EventArgs e)
    {
        int id = 0;
        int.TryParse(Request["id"] ?? "0", out id);
        DtProductdesc = WSCommon.GetProductImgsByid(id);
    }
}