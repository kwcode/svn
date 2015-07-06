using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class index : System.Web.UI.Page
{
    public DataTable dtNews;
    public DataTable DtProductImgs { get; set; }

    /// <summary>
    /// 滚动图片
    /// </summary>
    public DataTable DtScrollProcducts { get; set; }
    public DataTable DtHomeTopProcducts { get; set; }
    protected void Page_Load(object sender, EventArgs e)
    {
        int total = 0;
        dtNews = WSCommon.GetHomeArticles(10);
        // DtProductImgs = WSCommon.GetProductImgs(0, 8);

        DtScrollProcducts = WSCommon.GetHomeScrollProcducts(20);
        DtHomeTopProcducts = WSCommon.GetHomeTopProcducts(20);
    }
}