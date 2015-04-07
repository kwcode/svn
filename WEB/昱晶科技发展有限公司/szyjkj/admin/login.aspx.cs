using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class admin_login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.HttpMethod == "POST")
        {
            string username = Request["loginname"] ?? "";
            string password = Request["pwd"] ?? "";
            bool ischeck = Convert.ToBoolean(Convert.ToInt32(Request["ischeck"] ?? "0"));
            int row = WSCommon.TryLogin(username, password, ischeck);
            Response.Clear();
            Response.ContentType = "application/json";
            Response.Write(row);
            Response.End();
        }

    }
}