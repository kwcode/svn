using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Ajax 请求
/// </summary>
public class AjaxBase : PageBase
{
    protected override bool NeedLogin
    {
        get
        {
            return false;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        var data = loadAjaxData();
        Response.Clear();
        Response.ContentType = "application/json";
        if (data == null)
            Response.Write("{\"data\":\"error\"}");
        else
            Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(data));
        Response.End();
    }

    /// <summary>
    /// 默认的返回值 { err = 0, msg = "操作成功" }
    /// </summary>
    /// <returns></returns>
    protected virtual object loadAjaxData()
    {
        BaseRespond data = new BaseRespond { err = 0, msg = "操作成功" };
        return data;
    }

    struct BaseRespond
    {
        public int err;

        public string msg;
    }
}
