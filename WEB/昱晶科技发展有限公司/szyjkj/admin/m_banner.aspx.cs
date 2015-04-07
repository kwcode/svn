using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class admin_m_banner : PageBase
{
    public DataTable DtBanner { get; set; }
    protected void Page_Load(object sender, EventArgs e)
    {
        DtBanner = WSCommon.GetBanner(20);
    }
}