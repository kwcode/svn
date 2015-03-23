using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class master_uc_uc_banner : System.Web.UI.UserControl
{

    public DataTable DtBanner { get; set; }
    protected void Page_Load(object sender, EventArgs e)
    {
        DtBanner = WSCommon.GetBanner(10);
    }
}