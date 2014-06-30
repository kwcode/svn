using CMS.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Web表格功能
{
    public partial class index : System.Web.UI.Page
    {
        private int pageSize = 5;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                UserCommon.Init();
                AspNetPager3.PageSize = pageSize;
                Button1.Attributes.Add("onclick", "loadlayer();");
                BingData();
            }
        }

        protected void ListPager_PageChanged(object sender, EventArgs e)
        {
            BingData();
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