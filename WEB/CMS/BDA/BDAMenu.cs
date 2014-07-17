using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CMS
{
    public class BDAMenu
    {
        private List<MenuEntity> _list = new List<MenuEntity>();
        public BDAMenu()
        {
            _list.Add(new MenuEntity() { ID = 1, Code = "c_1", Name = "系统主页", ParentID = 0, IsDisplay = true, Sort = 1, URL = "" });
            _list.Add(new MenuEntity() { ID = 2, Code = "c_2", Name = "酒店预订", ParentID = 0, IsDisplay = true, Sort = 2, URL = "" });
            _list.Add(new MenuEntity() { ID = 3, Code = "c_3", Name = "攻略", ParentID = 0, IsDisplay = true, Sort = 3, URL = "" });
            _list.Add(new MenuEntity() { ID = 4, Code = "c_4", Name = "活动", ParentID = 0, IsDisplay = true, Sort = 4, URL = "" });
            _list.Add(new MenuEntity() { ID = 5, Code = "c_5", Name = "统计信息", ParentID = 1, IsDisplay = true, Sort = 5, URL = "" });
            _list.Add(new MenuEntity() { ID = 6, Code = "c_6", Name = "广告标幅", ParentID = 1, IsDisplay = true, Sort = 6, URL = "" });
            _list.Add(new MenuEntity() { ID = 7, Code = "c_7", Name = "客户反馈", ParentID = 1, IsDisplay = true, Sort = 7, URL = "" });
            _list.Add(new MenuEntity() { ID = 8, Code = "c_8", Name = "酒店管理", ParentID = 2, IsDisplay = true, Sort = 8, URL = "" });
            _list.Add(new MenuEntity() { ID = 9, Code = "c_9", Name = "地区位置管理", ParentID = 2, IsDisplay = true, Sort = 9, URL = "" });
            _list.Add(new MenuEntity() { ID = 10, Code = "c_10", Name = "信用卡管理", ParentID = 2, IsDisplay = true, Sort = 10, URL = "" });
            _list.Add(new MenuEntity() { ID = 11, Code = "c_11", Name = "攻略管理", ParentID = 3, IsDisplay = true, Sort = 11, URL = "" });
            _list.Add(new MenuEntity() { ID = 13, Code = "c_13", Name = "攻略申请", ParentID = 3, IsDisplay = true, Sort = 12, URL = "" });
            _list.Add(new MenuEntity() { ID = 14, Code = "c_14", Name = "主题管理", ParentID = 3, IsDisplay = true, Sort = 13, URL = "" });
        }
        /// <summary>
        /// 获取菜单
        /// </summary>
        /// <returns></returns>
        public List<MenuEntity> GetMenuList()
        {
            List<MenuEntity> list = new List<MenuEntity>();
            list = _list;
            return list;
        }

        /// <summary>
        /// 增加菜单
        /// </summary>
        /// <param name="Code"></param>
        /// <param name="Name"></param>
        /// <param name="IsDisplay"></param>
        /// <param name="ParentID"></param>
        /// <param name="Description"></param>
        public void AddMenu(string Code, string Name, bool IsDisplay, int ParentID, string URL, string Description)
        {
            _list.Add(new MenuEntity() { ID = _list.Count + 2, Code = Code, Name = Name, ParentID = ParentID, IsDisplay = IsDisplay, Sort = _list.Count + 2, URL = "" });
        }

        public void DelMenu(int id)
        {

        }
    }
}
