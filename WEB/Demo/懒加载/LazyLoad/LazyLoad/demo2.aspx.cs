using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LazyLoad
{
    public partial class demo2 : System.Web.UI.Page
    {
        /// <summary>
        /// 图片集合
        /// </summary>
        public List<string> imglist = new List<string>();
        protected void Page_Load(object sender, EventArgs e)
        {
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(Server.MapPath("~/upload/images"));
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo item in files)
            {
                imglist.Add("/upload/images/" + item.Name);
            }
        }
    }
}