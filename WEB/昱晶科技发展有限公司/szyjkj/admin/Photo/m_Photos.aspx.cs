using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class admin_Photo_m_Photos : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.HttpMethod == "POST")
        {
            int page = Convert.ToInt32(Request["page"] ?? "1");
            int pagesize = Convert.ToInt32(Request["rows"] ?? "1");
            string keywords = Request["keywords"] ?? "";
            int total = 0;
            DataTable dt = WSCommon.GetPhotosList(page, pagesize, BookId, keywords, out total);
            ResponseJson(new DataGridJson() { total = total, rows = dt });
        }
    }
    public int BookId
    {
        get
        {
            int bookid = 0;
            int.TryParse(Request["bookid"] ?? "-1", out bookid);
            return bookid;
        }
    }
}