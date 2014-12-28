using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class action_actionIndex : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string action = Request["action"] ?? "";
        switch (action)
        {
            case "file":
                HttpPostedFile file = Request.Files[0];
                
                break;
            default:
                break;
        }
    }
}