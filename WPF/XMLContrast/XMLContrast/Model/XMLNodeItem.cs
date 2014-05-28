using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XMLContrast
{
    /// <summary>
    /// XML节点
    /// </summary>
    public class XMLNodeItem
    {
        /// <summary>
        /// 节点名字
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 子节点集合 ObservableCollection
        /// </summary>
        public List<XMLNodeItem> ChildNodes { get; set; }
        /// <summary>
        /// 属性集合
        /// </summary>
        public List<XMLAttributes> Attributes { get; set; }
    }
}
