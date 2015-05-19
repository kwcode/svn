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