using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace XMLContrast
{
    public class XMLOperat
    {
        /// <summary>
        /// 解析XML
        /// </summary>
        /// <param name="xmldoc"></param>
        /// <returns></returns>
        public static List<XMLNodeItem> ResolveXML(XmlDocument xmldoc)
        {
            List<XMLNodeItem> xmlNodes = new List<XMLNodeItem>();
            string rootNode = xmldoc.DocumentElement.Name;
            XMLNodeItem nodeItem = new XMLNodeItem();
            nodeItem.Name = rootNode;
            nodeItem.Attributes = GetAttributes(xmldoc.DocumentElement.Attributes);
            List<XMLNodeItem> xmlChildNodes = new List<XMLNodeItem>();
            foreach (XmlNode item in xmldoc.SelectSingleNode(rootNode).ChildNodes)
            {
                ResolveChildNodes(xmlChildNodes, item);
            }
            nodeItem.ChildNodes = xmlChildNodes;
            xmlNodes.Add(nodeItem);
            return xmlNodes;
        }
        /// <summary>
        /// 获取当前借点的属性
        /// </summary>
        /// <param name="xmlAttributeCollection"></param>
        /// <returns></returns>
        public static List<XMLAttributes> GetAttributes(XmlAttributeCollection xmlAttributeCollection)
        {
            List<XMLAttributes> attributes = new List<XMLAttributes>();
            if (xmlAttributeCollection != null)
            {
                foreach (XmlAttribute item in xmlAttributeCollection)
                {
                    attributes.Add(new XMLAttributes() { Name = item.Name, Value = item.Value, });
                }
            }
            return attributes;
        }

        public static void ResolveChildNodes(List<XMLNodeItem> xmlChildNodes, XmlNode childNodes)
        {
            List<XMLNodeItem> child = new List<XMLNodeItem>();
            XMLNodeItem nodeItem = new XMLNodeItem();
            nodeItem.Name = childNodes.Name;
            nodeItem.Attributes = GetAttributes(childNodes.Attributes);
            foreach (XmlNode item in childNodes.ChildNodes)
            {
                ResolveChildNodes(child, item);
            }
            nodeItem.ChildNodes = child;
            xmlChildNodes.Add(nodeItem);

        }


        #region 获取树中匹配的节点集合

        /// <summary>
        /// 获取树中匹配的节点集合
        /// </summary>
        /// <param name="name">匹配值</param>
        /// <param name="dataNodes">整个树</param>
        /// <returns></returns>
        public static List<XMLNodeItem> GetDataChildNodes(string name, List<XMLNodeItem> dataNodes)
        {
            List<XMLNodeItem> nodeItems = new List<XMLNodeItem>();
            foreach (XMLNodeItem item in dataNodes)
            {
                if (item.Name == name)
                    nodeItems.Add(item);

                if (item.ChildNodes != null && item.ChildNodes.Count != 0)
                {
                    SetDataChildNodes(nodeItems, item.ChildNodes, name);
                }
            }
            return nodeItems;
        }

        private static void SetDataChildNodes(List<XMLNodeItem> nodeItems, List<XMLNodeItem> childNodes, string name)
        {
            foreach (XMLNodeItem item in childNodes)
            {
                if (item.Name == name)
                    nodeItems.Add(item);
                if (item.ChildNodes != null && item.ChildNodes.Count != 0)
                {
                    SetDataChildNodes(nodeItems, item.ChildNodes, name);
                }
            }
        }
        #endregion

        /// <summary>
        /// 构造树
        /// </summary> 
        public static Dictionary<XMLNodeItem, Dictionary<XMLNodeItem, List<XMLNodeItem>>> GetTreeListNodes(string root, string second, string name, List<XMLNodeItem> dataNodes)
        {
            Dictionary<XMLNodeItem, Dictionary<XMLNodeItem, List<XMLNodeItem>>> diclist = new Dictionary<XMLNodeItem, Dictionary<XMLNodeItem, List<XMLNodeItem>>>();
            List<XMLNodeItem> keys = XMLOperat.GetDataChildNodes(root, dataNodes);
            foreach (XMLNodeItem item in keys)
            {
                Dictionary<XMLNodeItem, List<XMLNodeItem>> values = StructureTenderNodes(second, name, item.ChildNodes);
                // List<XMLNodeItem> values = XMLOperat.GetDataChildNodes(name, item.ChildNodes);
                diclist.Add(item, values);
            }
            return diclist;
        }
        public static Dictionary<XMLNodeItem, List<XMLNodeItem>> StructureTenderNodes(string groupName, string name, List<XMLNodeItem> dataNodes)
        {
            Dictionary<XMLNodeItem, List<XMLNodeItem>> diclist = new Dictionary<XMLNodeItem, List<XMLNodeItem>>();
            List<XMLNodeItem> keys = XMLOperat.GetDataChildNodes(groupName, dataNodes);
            foreach (XMLNodeItem item in keys)
            {
                List<XMLNodeItem> values = XMLOperat.GetDataChildNodes(name, item.ChildNodes);
                diclist.Add(item, values);
            }
            return diclist;
        }
    }
}
