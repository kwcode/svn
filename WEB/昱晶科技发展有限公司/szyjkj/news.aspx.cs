using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class news : System.Web.UI.Page
{
    public DataTable DtNews { get; set; }

    public int PageSize = 20;
    public int TotalCount { get; set; }
    protected void Page_Load(object sender, EventArgs e)
    {
        int tt = 0;
        DataTable dtActicleType = WSCommon.GetArticleTypeList(1, 100, "", out tt);
        List<LeftMenuCL> listmenu = new List<LeftMenuCL>();

        if (dtActicleType != null && dtActicleType.Rows.Count > 0)
        {
            foreach (DataRow item in dtActicleType.Rows)
            {
                int _id = Convert.ToInt32(item["ID"]);
                if (type == _id)
                {
                    listmenu.Add(new LeftMenuCL() { CL = "active", ID = _id, Url = "/news/a-" + _id + ".html", Name = item["Name"].ToString(), Ico = MenuICO.icon1 });
                }
                else
                {
                    listmenu.Add(new LeftMenuCL() { ID = _id, Url = "/news/a-" + _id + ".html", Name = item["Name"].ToString(), Ico = MenuICO.icon1 });
                }

            }
        }
        if (type == 0)
        {
            listmenu[0].CL = "active";
        }
        uc_leftmenu.MenuList = listmenu;

        int total = 0;
        int page = Convert.ToInt32(Request["page"] ?? "1");
        DtNews = WSCommon.GetArticleList(page, PageSize, type, "", out total);
        TotalCount = total;
    }
    public int type
    {
        get
        {
            int type = 0;
            int.TryParse(Request["type"] ?? "0", out type);
            return type;

        }
    }
}