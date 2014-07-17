using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS
{
    /// <summary>
    /// 菜单
    /// </summary>
    public class MenuEntity : BaseEntity
    {
        /// <summary>
        /// 级别
        /// </summary>
        public string Level { get; set; }
        /// <summary>
        /// 是否显示
        /// </summary>
        public bool IsDisplay { get; set; }
        /// <summary>
        /// 上级ID
        /// </summary>
        public int ParentID { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// URL
        /// </summary>
        public string URL { get; set; }
    }
}
