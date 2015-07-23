using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class newsdetails : System.Web.UI.Page
{
    public DataTable DtNews { get; set; }
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
                    listmenu.Add(new LeftMenuCL() { CL = "", ID = _id, Url = "/news/a-" + _id + ".html", Name = item["Name"].ToString(), Ico = MenuICO.icon1 });
                }
            }
        }
        if (type == 0)
        {
            listmenu[0].CL = "active";
        }
        uc_leftmenu.MenuList = listmenu;

        int id = Convert.ToInt32(Request["id"] ?? "0");
        DtNews = WSCommon.GetArticleByID(id);
        WSCommon.ClickArticle(id);
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