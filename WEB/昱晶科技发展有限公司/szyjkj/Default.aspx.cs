using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Default : System.Web.UI.Page
{
    public DataTable DtScrollProcducts { get; set; }
    public DataTable dtNews;
    protected void Page_Load(object sender, EventArgs e)
    {
        dtNews = WSCommon.GetHomeArticles(10);
        DtScrollProcducts = WSCommon.GetHomeScrollProcducts(20);
        //简介
        dtJianJie = WSCommon.GetHomeJianJie();
    }

    public DataTable dtJianJie { get; set; }
}