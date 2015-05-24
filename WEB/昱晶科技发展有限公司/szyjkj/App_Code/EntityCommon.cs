using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
    public string ShowIndex { get; set; }
}