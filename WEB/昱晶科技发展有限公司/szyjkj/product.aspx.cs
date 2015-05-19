using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class product : System.Web.UI.Page
{
    //public DataTable DtProductType { get; set; }
    public DataTable DtproductImgs { get; set; }
    protected void Page_Load(object sender, EventArgs e)
    {

        int page = 0;
        int.TryParse(Request["page"] ?? "0", out page);
        int pid = 0;
        int.TryParse(Request["pid"] ?? "0", out pid);
        int total = 0;
        List<LeftMenuCL> listmenu = new List<LeftMenuCL>();
        DataTable  DtProductType = WSCommon.GetProduct(1, 100, "", out  total);
        DtproductImgs = WSCommon.GetProductImgs(pid, 20);
        foreach (DataRow item in DtProductType.Rows)
        {
            listmenu.Add(new LeftMenuCL() { ID = Convert.ToInt32(item["ID"]), Url = "/product.aspx?pid=" + item["ID"], Name = Convert.ToString(item["Title"]), Ico = MenuICO.icon1 });
        }
        uc_leftmenu.MenuList = listmenu;
    }
}