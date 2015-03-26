using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class product : System.Web.UI.Page
{
    public DataTable DtProductType { get; set; }
    public DataTable DtproductImgs { get; set; }
    protected void Page_Load(object sender, EventArgs e)
    {

        int page = 0;
        int.TryParse(Request["page"] ?? "0", out page);
        int pid = 0;
        int.TryParse(Request["pid"] ?? "0", out pid);
        DtProductType = WSCommon.GetProduct(100);
        DtproductImgs = WSCommon.GetProductImgs(pid, 20);
    }
}