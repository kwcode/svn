using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class index : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.HttpMethod == "POST")
        {
            if (Request.Files.Count > 0)
            {
                HttpPostedFile file = Request.Files[0];//runat="server"
                string fileName = file.FileName;
                file.SaveAs(Server.MapPath("file/" + fileName));
                //FileUpload fuImage = new FileUpload(); 
            }
        }
    }
}