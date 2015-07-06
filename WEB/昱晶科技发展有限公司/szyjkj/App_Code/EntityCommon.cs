using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

/// <summary>
/// EntityCommon 的摘要说明
/// </summary>
public class EntityCommon
{
    public EntityCommon()
    {
        //
        // TODO: 在此处添加构造函数逻辑
        //
    }
    public static List<TreeBaseDataCL> ConvertDtToTree(DataTable dt, int pid = 0)
    {
        List<TreeBaseDataCL> list = new List<TreeBaseDataCL>();
        if (dt != null && dt.Rows.Count > 0)
        {
            foreach (DataRow item in dt.Rows)
            {
                int _Pid = Convert.ToInt32(item["PID"]);
                if (_Pid == pid)
                {
                    int ID = Convert.ToInt32(item["ID"]);
                    TreeBaseDataCL treeitem = new TreeBaseDataCL();
                    treeitem.ID = ID;
                    treeitem.ICO = item["ICO"].ToString();
                    treeitem.ShowIndex = Convert.ToInt32(item["ShowIndex"]);
                    treeitem.Text = item["Name"].ToString();
                    treeitem.Url = item["Url"].ToString();
                    treeitem.PID = Convert.ToInt32(item["PID"]);
                    treeitem.Children = GetTreeChildren(dt, ID);
                    list.Add(treeitem);
                }
            }
        }
        return list;
    }
    private static List<TreeBaseDataCL> GetTreeChildren(DataTable dt, int pid)
    {
        List<TreeBaseDataCL> list = new List<TreeBaseDataCL>();
        if (dt != null && dt.Rows.Count > 0)
        {
            foreach (DataRow item in dt.Rows)
            {
                int _Pid = Convert.ToInt32(item["PID"]);
                if (_Pid == pid)
                {
                    int ID = Convert.ToInt32(item["ID"]);
                    TreeBaseDataCL treeitem = new TreeBaseDataCL();
                    treeitem.ID = ID;
                    treeitem.ICO = item["ICO"].ToString();
                    treeitem.ShowIndex = Convert.ToInt32(item["ShowIndex"]);
                    treeitem.Text = item["Name"].ToString();
                    treeitem.Url = item["Url"].ToString();
                    treeitem.PID = Convert.ToInt32(item["PID"]);
                    treeitem.Children = GetTreeChildren(dt, ID);
                    list.Add(treeitem);
                }
            }
        }
        return list;
    }

}
public class LeftMenuCL
{
    //public LeftMenuCL()
    //{
    //    //
    //    // TODO: 在此处添加构造函数逻辑
    //    //
    //}
    public int ID { get; set; }
    public string Url { get; set; }
    public string Name { get; set; }
    public MenuICO Ico { get; set; }
}
public enum MenuICO
{
    icon1,
    icon2,
    icon3,
    icon4,
    icon5,
    icon6,
    icon7,
    icon8,
    icon9,
}

/// <summary>
/// 树的基本类
/// </summary>
public class TreeBaseDataCL
{
    /// <summary>
    ///  绑定到节点的标识值.
    /// </summary>
    [JsonProperty("id")]
    public int ID { get; set; }
    /// <summary>
    /// 显示文本
    /// </summary>
    [JsonProperty("text")]
    public string Text { get; set; }
    /// <summary>
    /// open  closed
    /// </summary>
    [JsonProperty("state")]
    public string State { get; set; }
    [JsonProperty("children")]
    public List<TreeBaseDataCL> Children { get; set; }
    [JsonProperty("iconCls")]
    public string ICO { get; set; }
    [JsonProperty("checked")]
    public bool Checked { get; set; }

    [JsonProperty("showindex")]
    public int ShowIndex { get; set; }
    [JsonProperty("url")]
    public string Url { get; set; }
    [JsonProperty("pid")]
    public int PID { get; set; }
}
