using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
public partial class ActionPage : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string action = Request["action"];
        object re = null;
        switch (action)
        {
            case "list":
                {
                    int pageNow = int.Parse(Request["pageNow"]);
                    int pageSize = int.Parse(Request["pageSize"]);
                    re = Newtonsoft.Json.JsonConvert.SerializeObject(GetPages(pageNow, pageSize));
                    Response.Write(re);
                    break;
                }

            default:
                break;
        }
    }
    public Pages GetPages(int pageNow, int pageSize)
    {
        Pages pages = new Pages();
        List<PageEntity> list = new List<PageEntity>();
        if (pageNow == 1)
        {
            for (int i = 0; i < pageSize; i++)
            {
                list.Add(new PageEntity() { ID = i.ToString(), Name = "第" + i + "个", Time = "2013-9", Desc = "AAA" + i });
            }

        }
        else if (pageNow == 2)
        {
            for (int i = pageNow * pageSize; i < pageSize + pageNow * pageSize; i++)
            {
                list.Add(new PageEntity() { ID = i.ToString(), Name = "第" + i + "个", Time = "2013-9", Desc = "AAA" + i });
            }
        }
        else
        {
            for (int i = pageNow * pageSize; i < pageSize + pageNow * pageSize; i++)
            {
                list.Add(new PageEntity() { ID = i.ToString(), Name = "第" + i + "个", Time = "2013-9", Desc = "AAA" + i });
            }
        }
        pages.pageNow = pageNow;
        pages.pageSize = pageSize;
        pages.total = 20;
        pages.dataList = list.ToArray();
        return pages;
    }
}
public class Pages
{
    public PageEntity[] dataList { get; set; }
    public int pageNow { get; set; }
    public int pageSize { get; set; }
    public int total { get; set; }
}
public class PageEntity
{
    public string ID { get; set; }
    public string Name { get; set; }
    public string Time { get; set; }
    public string Desc { get; set; }

}