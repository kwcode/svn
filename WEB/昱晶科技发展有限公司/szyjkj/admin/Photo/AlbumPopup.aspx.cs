using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class admin_photo_AlbumPopup : PageBase
{
    protected override bool NeedLogin
    {
        get
        {
            return  false;
        }
    }
    public int BookId
    {
        get
        {
            int bookid = 0;
            int.TryParse(Request["bookid"] ?? "-1", out bookid);
            return bookid;
        }
    }
    public string SingleSelect
    {
        get
        {
            return Request["s"] ?? "0";
        }
    }

    //public DataTable dtPhotos { get; set; }
    //public int PageSize = 20;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.HttpMethod == "POST")
        {
            int page = Convert.ToInt32(Request["page"] ?? "1");
            int pagesize = Convert.ToInt32(Request["rows"] ?? "1");
            string keywords = Request["keywords"] ?? "";
            int total = 0;
            DataTable dt = WSCommon.GetPhotosList(page, pagesize, BookId, keywords, out total);
            ResponseJson(new DataGridJson() { total = total, rows = dt });
        }
        else
        {
            int total = 0;
            DataTable dtPhotoBook = WSCommon.GetPhotoBooksList(1, 1000, "", out total);
            string json = string.Format("var jsonphotobook = {0};", Newtonsoft.Json.JsonConvert.SerializeObject(dtPhotoBook));
            this.Page.ClientScript.RegisterStartupScript(this.GetType(), "script", json, true);
        }

    }
}