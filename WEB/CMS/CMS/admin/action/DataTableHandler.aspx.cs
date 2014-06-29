using CMS.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CMS.admin.action
{
    public partial class DataTableHandler : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Request["Action"] != null)
            {
                string action = this.Request["Action"].ToString();
                if (action == "GetDataTable")
                {
                    string jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(UserCommon.GetAllUser());
                    this.Response.Write(jsonstr);
                }
            }

        }
    }

}