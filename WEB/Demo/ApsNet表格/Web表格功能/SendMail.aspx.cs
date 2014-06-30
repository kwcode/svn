using CMS.entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Web表格功能
{
    public partial class SendMail : System.Web.UI.Page
    {
        private int pageSize = 5;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                PickUpManager.Init(GetData());
                pager.PageSize = pageSize;
                Button1.Attributes.Add("onclick", "loadlayer();");
                BingData();
            }
        }
        private DataTable GetData()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("UserID");
            dt.Columns.Add("NickName");
            dt.Columns.Add("LoginName");
            dt.Columns.Add("PasswordOrg");
            dt.Columns.Add("Email");
            for (int i = 0; i < 1000; i++)
            {
                DataRow dr = dt.NewRow();
                dr["UserID"] = i.ToString();
                dr["NickName"] = "纯粹是糖" + i;
                dr["LoginName"] = "tang" + i;
                dr["PasswordOrg"] = "tangkaiwne" + i;
                dr["Email"] = "353328" + i + "@qq.com";
                dt.Rows.Add(dr);
            }

            return dt;
        }
        protected void pager_PageChanged(object sender, EventArgs e)
        {
            BingData();
        }
        private void BingData()
        {
            pager.RecordCount = PickUpManager.ItemCount;
            int startIndex = pager.StartRecordIndex;
            int endIndex = pager.EndRecordIndex;
            rpt_user.DataSource = PickUpManager.GetUser(startIndex, endIndex);
            rpt_user.DataBind();
        }
        protected void Button1_Click(object sender, EventArgs e)
        {
            DelCheckBoxItem();
        }

        private void DelCheckBoxItem()
        {
            CheckBox cbox;
            HtmlInputHidden HtmlInputID;
            for (int i = 0; i <= rpt_user.Items.Count - 1; i++)
            {
                if (rpt_user.Items[i].ItemType == ListItemType.Item)
                {
                    cbox = (CheckBox)rpt_user.Items[i].FindControl("Ckb_Sel");
                    HtmlInputID = (HtmlInputHidden)rpt_user.Items[i].FindControl("txtID");

                    if (cbox.Checked == true)
                    {
                        string strID = HtmlInputID.Value.ToString();
                        PickUpManager.Remove(strID);
                    }
                }
                else if (rpt_user.Items[i].ItemType == ListItemType.AlternatingItem)
                {
                    cbox = (CheckBox)rpt_user.Items[i].FindControl("Ckb_Sel1");
                    HtmlInputID = (HtmlInputHidden)rpt_user.Items[i].FindControl("txtID1");

                    if (cbox.Checked == true)
                    {
                        string strID = HtmlInputID.Value.ToString();
                        PickUpManager.Remove(strID);
                    }
                }
            }
            BingData();
        }
    }
    public class PickUpManager
    {
        private static List<Pickup> _ulist = null;
        /// <summary>
        /// 初始化数据
        /// </summary>
        public static void Init(DataTable dt)
        {
            _ulist = new List<Pickup>();
            foreach (DataRow dr in dt.Rows)
            {
                _ulist.Add(new Pickup()
                {
                    UserID = dr["UserID"].ToString(),
                    NickName = dr["NickName"].ToString(),
                    LoginName = dr["LoginName"].ToString(),
                    PasswordOrg = dr["PasswordOrg"].ToString(),
                    Email = dr["Email"].ToString(),
                });
            }
        }
        public static List<Pickup> GetUser(int startIndex, int endIndex)
        {
            List<Pickup> ulist = new List<Pickup>();
            if (_ulist != null && _ulist.Count != 0)
            {
                if (startIndex < 0) startIndex = 1;
                if (endIndex < 0) startIndex = 1;

                for (int i = startIndex - 1; i < endIndex; i++)
                {
                    if (i < _ulist.Count)
                        ulist.Add(_ulist[i]);
                }
            }
            return ulist;
        }

        public static void Remove(Pickup user)
        {
            if (_ulist != null && _ulist.Count != 0)
            {
                _ulist.Remove(user);
            }
        }
        public static void Remove(string userid)
        {
            if (_ulist != null && _ulist.Count != 0)
            {
                Pickup user = _ulist.Find((delegate(Pickup u) { return u.UserID == userid; }));
                if (user != null)
                    Remove(user);
            }
        }
        public static int ItemCount
        {
            get
            {
                if (_ulist != null && _ulist.Count != 0)
                    return _ulist.Count;
                return 0;
            }
        }
    }
    public class Pickup
    {
        public string UserID { get; set; }
        public string LoginName { get; set; }
        public string PasswordOrg { get; set; }
        public string Email { get; set; }
        public string NickName { get; set; }
    }
}