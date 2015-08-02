using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class admin_index : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //if (Request.HttpMethod == "POST")
        //{
        //    int page = Convert.ToInt32(Request["page"] ?? "1");
        //    int pagesize = Convert.ToInt32(Request["rows"] ?? "1");
        //    string keywords = Request["keywords"] ?? "";
        //    int total = 0;
        //    DataTable dt = WSCommon.GetPmMenuList();
        //    List<TreeBaseDataCL> tree = EntityCommon.ConvertDtToTree(dt);
        //    ResponseJson(tree);
        //}
        DataTable dt = WSCommon.GetPmMenuList();
        List<TreeBaseDataCL> tree = EntityCommon.ConvertDtToTree(dt);
        string json = string.Format("var jsontree={0};", Newtonsoft.Json.JsonConvert.SerializeObject(tree));
        this.Page.ClientScript.RegisterStartupScript(this.GetType(), "script", json, true);
    }
}