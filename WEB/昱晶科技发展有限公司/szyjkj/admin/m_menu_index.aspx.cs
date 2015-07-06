using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class admin_m_menu_index : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        #region 测试数据
        List<TreeBaseDataCL> treelist = new List<TreeBaseDataCL>();
        List<TreeBaseDataCL> treechildred = new List<TreeBaseDataCL>();
        treechildred.Add(new TreeBaseDataCL() { ID = 11, Text = "子1" });
        treechildred.Add(new TreeBaseDataCL() { ID = 12, Text = "子2" });

        treelist.Add(new TreeBaseDataCL() { ID = 1, Text = "父1", ICO = "icon-pencil", Children = treechildred });
        treelist.Add(new TreeBaseDataCL() { ID = 2, Text = "父2", Children = treechildred });
        #endregion

        if (Request.HttpMethod == "POST")
        {
            int page = Convert.ToInt32(Request["page"] ?? "1");
            int pagesize = Convert.ToInt32(Request["rows"] ?? "1");
            string keywords = Request["keywords"] ?? "";
            int total = 0;
            DataTable dt = WSCommon.GetPmMenuList();
            List<TreeBaseDataCL> tree = EntityCommon.ConvertDtToTree(dt);
            ResponseJson(new DataGridJson() { total = total, rows = tree });
        }
        else
        {
            //获取admin 里面aspx页面
            string path = Server.MapPath("~/");
            List<string> str = Directory.GetFiles(path + "admin/", "*.aspx", SearchOption.AllDirectories).ToList();
            for (int i = 0; i < str.Count; i++)
            {
                str[i] = "/" + str[i].Substring(str[i].LastIndexOf("admin/"), str[i].Length - str[i].LastIndexOf("admin/")).Replace("\\", "/");
            }
            #region 这里读取icon.css文件 做一个缓存
            string CacheKey = "C_icos";//检索指定项，
            object objModel = Cache.Get(CacheKey);
            if (objModel == null)
            {
                FileStream Stream = File.Open(path + "style/icon.css", FileMode.Open);
                byte[] buff = new byte[Stream.Length];
                Stream.Read(buff, 0, (int)Stream.Length);
                string ss = System.Text.Encoding.Default.GetString(buff, 0, buff.Length);
                Stream.Close();
                MatchCollection mc = Regex.Matches(ss, @"icon-(\w+)");
                List<string> icolist = new List<string>();
                foreach (Match item in mc)
                {
                    icolist.Add(item.Value);
                }
                objModel = icolist;
                Cache.Insert(CacheKey, objModel, null, DateTime.Now.AddMinutes(120), System.Web.Caching.Cache.NoSlidingExpiration);
            }

            #endregion


            DataTable dt = WSCommon.GetPmMenuList();
            string json = string.Format("var jsonadminPage = {0};var jsonicos={1};var jsontree={2}"
                , Newtonsoft.Json.JsonConvert.SerializeObject(str),
                Newtonsoft.Json.JsonConvert.SerializeObject(objModel),
                  Newtonsoft.Json.JsonConvert.SerializeObject(dt)
                );
            this.Page.ClientScript.RegisterStartupScript(this.GetType(), "script", json, true);
        }


    }


}