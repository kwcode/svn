using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class admin_JianJie_Jianjiemanager : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.HttpMethod == "POST")
        {
            DataTable dt = WSCommon.GetAboutMe();
            ResponseJson(new DataGridJson() { rows = dt });
        }

    }
}