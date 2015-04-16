using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class admin_user_usermanager : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // easyui datagrid 自身会通过 post 的形式传递 rows and page 
        if (Request.HttpMethod == "POST")
        {
            int page = Convert.ToInt32(Request["page"] ?? "1");
            int pagesize = Convert.ToInt32(Request["rows"] ?? "1");
            int total = 0;
            DataTable dt = WSCommon.GetUserList(page, pagesize, "", out total);
            Response.Clear();
            Response.ContentType = "application/json";
            // 返回到前台的值必须按照如下的格式包括 total and rows 
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(new DataGridJson() { total = total, rows = dt });
            Response.Write(json);
            Response.End();

        }
    }

    public class DataGridJson
    {
        public int total { get; set; }
        public DataTable rows { get; set; }
    }
}