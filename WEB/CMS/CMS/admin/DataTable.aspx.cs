using CMS.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CMS.admin
{
    public partial class DataTable : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            rpt_showListHoliday.DataSource = UserDataList();
            rpt_showListHoliday.DataBind();
            PaginationResult.InnerHtml = new Pagination(2, 1,1000).GetHtmlResult(DisplayEdgeCount: 1);
        }
        protected List<UserInfo> UserDataList()
        {
            System.Threading.Thread.Sleep(4000);
            List<UserInfo> ulist = new List<UserInfo>();
            ulist.Add(new UserInfo() { ID = 1, UserID = "1", Name = "tkw", NickName = "纯粹是糖", Password = "123", Email = "353328333@qq.com", LoginName = "tkw", SmallPhoto = "/images/smallphoto/blue_bird_16.png" });
            ulist.Add(new UserInfo() { ID = 2, UserID = "2", Name = "csx", NickName = "AAAAAAAAAAAAAAAAAAAAAAAA", Password = "123", Email = "453352038@qq.com", LoginName = "tkw", SmallPhoto = "/images/smallphoto/ie_16.png" });
            ulist.Add(new UserInfo() { ID = 3, UserID = "3", Name = "tkw", NickName = "纯粹是糖", Password = "123", Email = "353328333@qq.com", LoginName = "tkw", SmallPhoto = "/images/smallphoto/o_16.png" });
            ulist.Add(new UserInfo() { ID = 4, UserID = "4", Name = "tkw", NickName = "纯粹是糖", Password = "123", Email = "353328333@qq.com", LoginName = "tkw", SmallPhoto = "/images/smallphoto/pin_suger_16.png" });
            ulist.Add(new UserInfo() { ID = 5, UserID = "5", Name = "tkw", NickName = "纯粹是糖", Password = "123", Email = "353328333@qq.com", LoginName = "tkw", SmallPhoto = "/images/smallphoto/blue_bird_16.png" });
            return ulist;
        }
        protected void ListPager_PageChanged(object sender, EventArgs e)
        {
            // PageInit(ListPager.CurrentPageIndex);
        }
        protected int reg = 0;
        /// <summary>
        /// 获取套餐类型信息
        /// </summary>
        /// <param name="CurrentPageIndex">当前页数</param>
        private void PageInit(int CurrentPageIndex)
        {
            rpt_showListHoliday.DataSource = UserDataList();
            rpt_showListHoliday.DataBind();
            //string totalCount = string.Empty;
            //DataTable dt = hightravel.GetAgentCooperationPager(reg, CurrentPageIndex, 10, true, out totalCount);
            //rptList.DataSource = dt;
            //rptList.DataBind();
            //ListPager.RecordCount = Convert.ToInt32(totalCount);
        }
    }
}