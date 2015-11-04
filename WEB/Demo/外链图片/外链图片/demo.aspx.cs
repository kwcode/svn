using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OutImg
{
    public partial class demo : System.Web.UI.Page
    {
        public string Content { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            string path = Server.MapPath("~/demo.txt");
            using (FileStream fs = File.Open(path, FileMode.OpenOrCreate))
            {
                int fsLen = (int)fs.Length;
                byte[] heByte = new byte[fsLen];
                int r = fs.Read(heByte, 0, heByte.Length);
                Content = System.Text.Encoding.UTF8.GetString(heByte);
            }
        }
    }
}