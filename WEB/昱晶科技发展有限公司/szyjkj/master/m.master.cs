using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class master_m : System.Web.UI.MasterPage
{
    public DataTable dtBase { get; set; }
    protected void Page_Load(object sender, EventArgs e)
    {
        dtBase = WSCommon.GetSiteBaseSettings(); 
    }
}
