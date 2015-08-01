using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class relation : System.Web.UI.Page
{
    public string Details { get; set; }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            List<LeftMenuCL> listmenu = new List<LeftMenuCL>();
            listmenu.Add(new LeftMenuCL() { ID = 0, Url = "/lianxi.html", Name = "联系我们", Ico = MenuICO.icon1 });
            listmenu.Add(new LeftMenuCL() { ID = 1, Url = "/liuyan.html", Name = "在线留言", Ico = MenuICO.icon2 });
            uc_leftmenu.MenuList = listmenu; 
            
        }
        DataTable dt = WSCommon.GetRelation();
        if (dt != null && dt.Rows.Count > 0)
        {
            Details = dt.Rows[0]["Details"].ToString();
        }
    }
}