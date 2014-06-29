using CMS.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;

namespace CMS.admin
{
    public partial class DataTable : System.Web.UI.Page
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
            if (Request["Action"] != null)
            {
                string action = Request["Action"].ToString();
                if (action == "DeleteData")
                {
                    string jsonstr = Request["uids"].ToString();
                    List<string> idlist = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(jsonstr);
                    AspNetPager3.RecordCount -= idlist.Count;
                    foreach (string item in idlist)
                    {
                        try
                        {
                            int id = Convert.ToInt32(item);
                            UserCommon.Remove(id);
                        }
                        catch { }
                    }
                    int startIndex = AspNetPager3.StartRecordIndex;
                    int endIndex = AspNetPager3.EndRecordIndex;
                    rpt_showListHoliday.DataSource = UserCommon.GetUser(startIndex, endIndex);
                    rpt_showListHoliday.DataBind();
                }
                if (action == "test")
                {
                    Response.Clear();
                    Response.Write(true);
                }
            }
            // PaginationResult.InnerHtml = new Pagination(2, 1,1000).GetHtmlResult(DisplayEdgeCount: 1);
        }
        protected void ListPager_PageChanged(object sender, EventArgs e)
        {
            // PageInit(ListPager.CurrentPageIndex);
            int startIndex = AspNetPager3.StartRecordIndex;
            int endIndex = AspNetPager3.EndRecordIndex;
            rpt_showListHoliday.DataSource = UserCommon.GetUser(startIndex, endIndex);
            rpt_showListHoliday.DataBind();
        }
        protected void rpt_ItemCommand(object sender, RepeaterCommandEventArgs e)
        {

        }
        protected int reg = 0;
        /// <summary>
        /// 获取套餐类型信息
        /// </summary>
        /// <param name="CurrentPageIndex">当前页数</param>
        private void PageInit(int CurrentPageIndex)
        {
            //int startIndex = AspNetPager3.StartRecordIndex;
            //int endIndex = AspNetPager3.EndRecordIndex;
            //rpt_showListHoliday.DataSource = GetUserDataList(startIndex, endIndex);
            //rpt_showListHoliday.DataBind();
            //string totalCount = string.Empty;
            //DataTable dt = hightravel.GetAgentCooperationPager(reg, CurrentPageIndex, 10, true, out totalCount);
            //rptList.DataSource = dt;
            //rptList.DataBind();
            //ListPager.RecordCount = Convert.ToInt32(totalCount);
        }

        private void D(List<string> uids)
        {
            List<string> idlist = uids;
            AspNetPager3.RecordCount -= idlist.Count;
            foreach (string item in idlist)
            {
                try
                {
                    int id = Convert.ToInt32(item);
                    UserCommon.Remove(id);
                }
                catch { }
            }
            int startIndex = AspNetPager3.StartRecordIndex;
            int endIndex = AspNetPager3.EndRecordIndex;
            rpt_showListHoliday.DataSource = UserCommon.GetUser(startIndex, endIndex);
            rpt_showListHoliday.DataBind();
        }
        [WebMethod]
        public static string SayHello(string arr)
        {
            return "Hello Ajax!";
        }

        [WebMethod]
        public static bool DeleteData(List<string> uids)
        {
            new DataTable().D(uids);
            return true;
        }
    }
}