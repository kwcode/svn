using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CMS.entity;
using Newtonsoft.Json;

namespace CMS.admin
{
    public partial class GridTable_JSON : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    int page = Convert.ToInt32(Request["page"]);
                    int count = Convert.ToInt32(Request["count"]);
                    int RowCount = UserCommon.GetUserCount();
                    if (page <= 0)
                    {
                        page = RowCount / count;
                    }
                    int StartIndex = (page - 1) * count;
                    int EndIndex = page * count;

                    if (RowCount <= StartIndex)
                        return;
                    if (RowCount <= EndIndex)
                        EndIndex = RowCount;

                    List<UserInfo> rows = UserCommon.GetUser(startIndex: StartIndex, endIndex: EndIndex);
                    DataEntity data = new DataEntity();
                    data.rowcount = RowCount;
                    data.page = page;
                    data.count = count;
                    data.rows = rows;
                    string strJson = JsonConvert.SerializeObject(data);
                    Response.Write(strJson);
                }
            }
            catch(Exception err)
            {

            }
        }
        class DataEntity
        {
            public int rowcount { get; set; }
            public int page { get; set; }
            public int count { get; set; }
            public List<UserInfo> rows { get; set; }
        }
    }
}