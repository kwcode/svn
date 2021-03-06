﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class aboutme : System.Web.UI.Page
{
    public string Details { get; set; }
    public int ID { get; set; }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ID = Convert.ToInt32(Request["id"] ?? "0");
            List<LeftMenuCL> listmenu = new List<LeftMenuCL>();
            DataTable dt = WSCommon.GetAboutMe();
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow item in dt.Rows)
                {
                    int _id = Convert.ToInt32(item["ID"]);
                    if (ID == _id)
                    {
                        listmenu.Add(new LeftMenuCL() { CL = "active", ID = _id, Url = "/aboutme/a-" + _id + ".html", Name = item["TypeName"].ToString(), Ico = MenuICO.icon1 });
                    }
                    else
                    {
                        listmenu.Add(new LeftMenuCL() { ID = _id, Url = "/aboutme/a-" + _id + ".html", Name = item["TypeName"].ToString(), Ico = MenuICO.icon1 });
                    }

                }
            }
            if (ID == 0 && listmenu.Count > 0)
            {
                listmenu[0].CL = "active";
            }
            uc_leftmenu.MenuList = listmenu;

            if (ID == 0 && listmenu.Count > 0)
            {
                ID = listmenu[0].ID;
            }
            DataTable dtCurrentJJ = WSCommon.GetAboutByID(ID);
            if (dt != null && dt.Rows.Count > 0)
            {
                Details = dtCurrentJJ.Rows[0]["Content"].ToString();
            }
        }

    }
}