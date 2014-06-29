using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CMS.entity;
using System.Web.UI.HtmlControls;

namespace CMS.admin
{
    public partial class WebDataOperat : System.Web.UI.Page
    {
        private int pageSize = 10;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                UserCommon.RefreshData();
                AspNetPager3.RecordCount = UserCommon.ItemCount;
                AspNetPager3.PageSize = pageSize;
                rpt_showListHoliday.DataSource = UserCommon.GetUser(1, pageSize);
                rpt_showListHoliday.DataBind();
            }
        }

        protected void ListPager_PageChanged(object sender, EventArgs e)
        {
            int startIndex = AspNetPager3.StartRecordIndex;
            int endIndex = AspNetPager3.EndRecordIndex;
            rpt_showListHoliday.DataSource = UserCommon.GetUser(startIndex, endIndex);
            rpt_showListHoliday.DataBind();
        }
        private void BingData()
        {
            AspNetPager3.RecordCount = UserCommon.ItemCount;
            int startIndex = AspNetPager3.StartRecordIndex;
            int endIndex = AspNetPager3.EndRecordIndex;

            rpt_showListHoliday.DataSource = UserCommon.GetUser(startIndex, endIndex);
            rpt_showListHoliday.DataBind();
        }
        protected void Button1_Click(object sender, EventArgs e)
        {
            DelCheckBoxItem();
        }

        private void DelCheckBoxItem()
        {
            CheckBox cbox;
            HtmlInputHidden HtmlInputID;
            for (int i = 0; i <= rpt_showListHoliday.Items.Count - 1; i++)
            {
                if (rpt_showListHoliday.Items[i].ItemType == ListItemType.Item)
                {
                    cbox = (CheckBox)rpt_showListHoliday.Items[i].FindControl("Ckb_Sel");
                    HtmlInputID = (HtmlInputHidden)rpt_showListHoliday.Items[i].FindControl("txtID");

                    if (cbox.Checked == true)
                    {
                        string strID = HtmlInputID.Value.ToString();
                        int nid = int.Parse(strID);
                        UserCommon.Remove(nid);
                        //string sql_Del = "delete PublicProject  where ID = '" + nid + "'";
                        //newdb.CommonExecuteNonQuery(sql_Del); 
                    }
                }
                else if (rpt_showListHoliday.Items[i].ItemType == ListItemType.AlternatingItem)
                {
                    cbox = (CheckBox)rpt_showListHoliday.Items[i].FindControl("Ckb_Sel1");
                    HtmlInputID = (HtmlInputHidden)rpt_showListHoliday.Items[i].FindControl("txtID1");

                    if (cbox.Checked == true)
                    {
                        string strID = HtmlInputID.Value.ToString();
                        int nid = int.Parse(strID);
                        UserCommon.Remove(nid);
                    }
                }
            }
            BingData();
        }
    }
}