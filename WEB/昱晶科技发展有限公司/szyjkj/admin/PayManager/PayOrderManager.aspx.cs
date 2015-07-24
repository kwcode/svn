using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class admin_PayManager_PayOrderManager : System.Web.UI.Page
{
    public int PageSize = 20;
    public int TotalCount { get; set; }
    public DataTable dtOrder { get; set; }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            TripWS.WSCommunal comm = new TripWS.WSCommunal();
            object result = null;
            int pageindex = Convert.ToInt32(Request["page"] ?? "1");
            string billnum = (Request["billnum"] ?? "").Trim();
            string ordernum = (Request["ordernum"] ?? "").Trim();
            int type = Convert.ToInt32(Request["type"] ?? "-1");
            int status = Convert.ToInt32(Request["status"] ?? "-1");
            int paytype = Convert.ToInt32(Request["paytype"] ?? "-2");
            string keywords = (Request["keywords"] ?? "").Trim();
            int userid = 0;
            int.TryParse(Request["userid"] ?? "0", out userid);
            dtOrder = comm.ExecDataTableResultOut("p_Admin_GetPayOrderList", new object[] { pageindex, PageSize, billnum, userid, ordernum, type, status, paytype, keywords, 0 }, out result);
            TotalCount = Convert.ToInt32(result);

            txt_bilnum.Value = billnum;
            txt_ordernum.Value = ordernum;
            txt_keywords.Value = keywords;
            sel_paytype.Value = paytype.ToString();
            sel_status.Value = status.ToString();
            sel_type.Value = type.ToString();
            txt_userid.Value = userid.ToString();
        }
    }
    protected void btn_search_Click(object sender, EventArgs e)
    {
        Response.Redirect(string.Format("/admin/PayManager/PayOrderManager.aspx?page={0}&billnum={1}&ordernum={2}&type={3}&keywords={4}&userid={5}&status={6}&paytype={7}", 1, txt_bilnum.Value, txt_ordernum.Value, sel_type.Value, txt_keywords.Value, txt_userid.Value, sel_status.Value, sel_paytype.Value));
    }
    #region 转换

    public string ConvertToStatus(int status)
    {
        string str = "待支付";
        if (status == 1)
            str = "已支付";
        if (status == 2)
            str = "交易关闭";
        if (status == 4)
            str = "已退款";
        return str;
    }
    /// <summary>
    /// 支付方式
    /// </summary>
    /// <param name="paytype"></param>
    /// <returns></returns>
    public string ConvertToPayType(int paytype)
    {
        string str = "待选择";
        if (paytype == 0)
            str = "驼铃支付";
        if (paytype == 1)
            str = "财付通";
        if (paytype == 2)
            str = "支付宝";
        return str;
    }

    public string ConvertToType(int type)
    {
        string str = "其他";
        if (type == 0)
            str = "充值";
        if (type == 1)
            str = "驼铃购物";
        if (type == 2)
            str = "提现";
        if (type == 3)
            str = "驼铃奖励";
        if (type == 11)
            str = "保险";
        if (type == 22)
            str = "租车";
        if (type == 33)
            str = "活动";
        return str;
    }
    #endregion
}