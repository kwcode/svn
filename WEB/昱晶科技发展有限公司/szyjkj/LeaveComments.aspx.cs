using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class LeaveComments : System.Web.UI.Page
{
    public int PageSize = 20;
    public int TotalCount { get; set; }
    public DataTable dtComments { get; set; }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            List<LeftMenuCL> listmenu = new List<LeftMenuCL>();
            listmenu.Add(new LeftMenuCL() { ID = 0, Url = "/lianxi.html", Name = "联系我们", Ico = MenuICO.icon1 });
            listmenu.Add(new LeftMenuCL() { ID = 1, Url = "/liuyan.html", Name = "在线留言", Ico = MenuICO.icon2 });
            uc_leftmenu.MenuList = listmenu;

            int page = Convert.ToInt32(Request["page"] ?? "1");
            //
            int total = 0;
            dtComments = WSCommon.GetLeaveCommentsList(page, PageSize, out total);
            TotalCount = total;
        }

    }
    public string ConvertToLeaveType(int type)
    {
        string str = "";
        if (type == 0)
            str = "咨询：";
        if (type == 1)
            str = "建议：";
        if (type == 2)
            str = "投诉：";
        return str;
    }
}