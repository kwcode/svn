using CMS.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CMS.admin.action
{
    public partial class DataTableHandler : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Request["Action"] != null)
            {
                string action = this.Request["Action"].ToString();
                if (action == "GetDataTable")
                {
                    string jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(GetUserInfo());
                    this.Response.Write(jsonstr);
                }
            }

        }

        private List<UserInfo> GetUserInfo()
        {
            System.Threading.Thread.Sleep(1000);
            List<UserInfo> ulist = new List<UserInfo>();
            ulist.Add(new UserInfo() { ID = 1, UserID = "1", Name = "tkw", NickName = "纯粹是糖", Password = "123", Email = "353328333@qq.com", LoginName = "tkw", SmallPhoto = "/images/logo.png" });
            ulist.Add(new UserInfo() { ID = 2, UserID = "2", Name = "csx", NickName = "", Password = "123", Email = "453352038@qq.com", LoginName = "tkw", SmallPhoto = "/images/logo.png" });
            ulist.Add(new UserInfo() { ID = 3, UserID = "3", Name = "tkw", NickName = "纯粹是糖", Password = "123", Email = "353328333@qq.com", LoginName = "tkw", SmallPhoto = "/images/logo.png" });
            ulist.Add(new UserInfo() { ID = 4, UserID = "4", Name = "tkw", NickName = "纯粹是糖", Password = "123", Email = "353328333@qq.com", LoginName = "tkw", SmallPhoto = "/images/logo.png" });
            ulist.Add(new UserInfo() { ID = 5, UserID = "5", Name = "tkw", NickName = "纯粹是糖", Password = "123", Email = "353328333@qq.com", LoginName = "tkw", SmallPhoto = "/images/logo.png" });
            return ulist;
        }
    }

}