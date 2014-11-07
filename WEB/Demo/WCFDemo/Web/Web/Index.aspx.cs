using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Index : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        WF.User.WFUserClient uclient = new WF.User.WFUserClient();
        string u1 = uclient.GetUserList();
        int row = uclient.Save();
        Response.Write(u1 + row);
    }
}