using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class admin_products_m_procductimg_index : PageBase
{
    public DataTable DtProcdut { get; set; }
    protected void Page_Load(object sender, EventArgs e)
    { 
        if (Request.HttpMethod == "POST")
        {
            int pid = 0; int.TryParse(Request["pid"] ?? "0", out pid);
            int page = Convert.ToInt32(Request["page"] ?? "1");
            int pagesize = Convert.ToInt32(Request["rows"] ?? "1");
            string keywords = Request["keywords"] ?? "";
            int total = 0;
            DataTable dt = WSCommon.GetAdminProductImgs(pid, page, pagesize, keywords, out total);
            ResponseJson(new DataGridJson() { total = total, rows = dt });
        }

    }
    public int ID
    {
        get
        {
            int id = 0;
            int.TryParse(Request["pid"] ?? "0", out id);
            return id;
        }
    }
}