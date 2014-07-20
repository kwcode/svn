using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

namespace BaseApiCommon
{
    public class XmlCommon
    {
        private static bool _IsRefreshDoc = true;
        /// <summary>
        /// 操作之后是否自动刷新缓存文档 默认刷新
        /// </summary>
        public static bool IsRefreshDoc { get { return _IsRefreshDoc; } set { _IsRefreshDoc = value; } }
        private static string _DocName = "DBXml.xml";
        private static string _xmlPath = AppDomain.CurrentDomain.BaseDirectory;
        private static XmlDocument _xmlDocument;
        public static void SetPath(string path)
        {
            _xmlPath = path;
        }
        /// <summary>
        /// 初始化
        /// </summary>
        private static void Init()
        {
            string fileName = _xmlPath + _DocName;
            if (_xmlDocument == null)
            {
                _xmlDocument = new XmlDocument();
                bool isExist = File.Exists(fileName);
                if (isExist)
                    _xmlDocument.Load(fileName);
                else
                {
                    using (FileStream fileStream = File.Create(fileName))
                    {
                        fileStream.Close();
                        XmlDeclaration dec = _xmlDocument.CreateXmlDeclaration("1.0", "GB2312", null);
                        _xmlDocument.AppendChild(dec);
                        //创建根节点   
                        XmlElement roots = _xmlDocument.CreateElement("Roots");
                        XmlNode root = _xmlDocument.CreateElement("Root");
                        roots.AppendChild(root);
                        _xmlDocument.AppendChild(roots);
                        _xmlDocument.Save(fileName);
                    }
                }
            }
        }
        /// <summary>
        /// 刷新Doc
        /// </summary>
        public static void RefreshDoc()
        {
            string fileName = _xmlPath + _DocName;
            _xmlDocument = new XmlDocument();
            _xmlDocument.Load(fileName);
        }
        /// <summary>
        /// 初始化
        /// </summary>
        public static void InitializationXmlDoc()
        {
            string fileName = _xmlPath + _DocName;
            _xmlDocument = new XmlDocument();
            using (FileStream fileStream = File.Create(fileName))
            {
                fileStream.Close();
                XmlDeclaration dec = _xmlDocument.CreateXmlDeclaration("1.0", "GB2312", null);
                _xmlDocument.AppendChild(dec);
                //创建根节点   
                XmlElement root = _xmlDocument.CreateElement("Root");
                _xmlDocument.AppendChild(root);
                _xmlDocument.Save(fileName);
            }
            if (_IsRefreshDoc)
                RefreshDoc();
        }
        /// <summary>
        ///插入节点 
        /// </summary>
        /// <param name="Code">节点代码</param>
        /// <param name="Content">节点内容</param>
        /// <param name="parentCode">上级节点 默认Root</param>
        /// <param name="atts">属性集合mn</param>
        public static void InsetXml(string code, string content, List<XMLAttributes> atts, string parentCode = null)
        {
            string fileName = _xmlPath + _DocName;
            if (string.IsNullOrWhiteSpace(parentCode))
                parentCode = "Root";
            if (_xmlDocument == null)
                Init();
            if (IsExistNode(code, atts))
                return;
            //查找父级节点
            XmlNode root = _xmlDocument.SelectSingleNode(parentCode);
            XmlElement xel = _xmlDocument.CreateElement(code);
            if (atts != null && atts.Count > 0)
            {
                foreach (XMLAttributes item in atts)
                {
                    XmlAttribute att = _xmlDocument.CreateAttribute(item.Name);
                    att.InnerText = item.Value;
                    xel.Attributes.Append(att);
                }
            }
            xel.InnerText = content;
            root.AppendChild(xel);
            _xmlDocument.Save(fileName);
            if (_IsRefreshDoc)
                RefreshDoc();
        }
        /// <summary>
        /// 插入节点
        /// </summary>
        /// <param name="code">节点代码</param>
        /// <param name="content">节点内容</param>
        /// <param name="parentCode">上级节点 默认Root</param>
        public static void InsertXml(string code, string content = null, string parentCode = null)
        {
            string fileName = _xmlPath + _DocName;
            if (string.IsNullOrWhiteSpace(parentCode))
                parentCode = "Root";
            if (_xmlDocument == null)
                Init();
            //查找父级节点
            XmlNode root = _xmlDocument.SelectSingleNode(parentCode);
            XmlElement xel = _xmlDocument.CreateElement(code);
            xel.InnerText = content;
            root.AppendChild(xel);
            _xmlDocument.Save(fileName);
            if (_IsRefreshDoc)
                RefreshDoc();
        }
        /// <summary>
        /// 修改值
        /// </summary>
        /// <param name="code">节点值</param>
        /// <param name="content">内容</param>
        /// <param name="atts">属性集合mn</param>
        public static void UpdateNodeContent(string code, string content, List<XMLAttributes> atts)
        {
            string fileName = _xmlPath + _DocName;
            if (_xmlDocument == null)
                Init();
            XmlNode xmlNode = FindXmlSingleNode(code, atts);//查找节点
            xmlNode.InnerText = content;
            _xmlDocument.Save(fileName);
            if (_IsRefreshDoc)
                RefreshDoc();
        }

        #region 解析

        /// <summary>
        /// 解析XML文件 构造树结构
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


        #endregion

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
        /// 获取XML文档信息
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetXmlDoc(string path = null)
        {
            string docstr = "";
            try
            {
                if (path != null)
                    SetPath(path);
                if (_xmlDocument == null) Init();
                string fileName = _xmlPath + _DocName;
                _xmlDocument.Load(fileName);
                docstr = _xmlDocument.InnerXml;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return docstr;
        }

        public static XmlNodeList FindXmlNodeList(string code, List<XMLAttributes> atts = null)
        {
            string fileName = _xmlPath + _DocName;
            if (_xmlDocument == null)
            {
                _xmlDocument = new XmlDocument();
                _xmlDocument.Load(fileName);
            }
            string xPath = "//" + code;
            if (atts != null && atts.Count > 0)
            {
                string c = "";
                int num = 0;
                string split = "";
                foreach (XMLAttributes item in atts)
                {
                    if (atts.Count > 1)
                    {
                        if (num == atts.Count - 1)
                            split = "and";
                    }
                    if (!string.IsNullOrWhiteSpace(item.Name) && string.IsNullOrWhiteSpace(item.Value))
                        c += split + "@" + item.Name;

                    if (!string.IsNullOrWhiteSpace(item.Name) && !string.IsNullOrWhiteSpace(item.Value))
                        c += split + "@" + item.Name + "='" + item.Value + "'";
                    num++;
                }

                xPath += "[" + c + "]";
            }
            XmlNodeList nodeList = _xmlDocument.SelectNodes(xPath);
            return nodeList;
        }
        public static XmlNode FindXmlSingleNode(string code, List<XMLAttributes> atts = null)
        {
            string fileName = _xmlPath + _DocName;
            if (_xmlDocument == null)
            {
                _xmlDocument = new XmlDocument();
                _xmlDocument.Load(fileName);
            }
            string xPath = "//" + code;
            if (atts != null && atts.Count > 0)
            {
                string c = "";
                int num = 0;
                string split = "";
                foreach (XMLAttributes item in atts)
                {
                    if (atts.Count > 1)
                    {
                        if (num == atts.Count - 1)
                            split = "and";
                    }
                    if (!string.IsNullOrWhiteSpace(item.Name) && string.IsNullOrWhiteSpace(item.Value))
                        c += split + "@" + item.Name;

                    if (!string.IsNullOrWhiteSpace(item.Name) && !string.IsNullOrWhiteSpace(item.Value))
                        c += split + "@" + item.Name + "='" + item.Value + "'";
                    num++;
                }

                xPath += "[" + c + "]";
            }
            XmlNode node = _xmlDocument.SelectSingleNode(xPath);
            return node;
        }

        public static bool IsExistNode(string code, List<XMLAttributes> atts = null)
        {
            string fileName = _xmlPath + _DocName;
            if (_xmlDocument == null)
            {
                _xmlDocument = new XmlDocument();
                _xmlDocument.Load(fileName);
            }
            string xPath = "//" + code;
            if (atts != null && atts.Count > 0)
            {
                string c = "";
                int num = 0;
                string split = "";
                foreach (XMLAttributes item in atts)
                {
                    if (atts.Count > 1)
                    {
                        if (num == atts.Count - 1)
                            split = "and";
                    }
                    if (!string.IsNullOrWhiteSpace(item.Name) && string.IsNullOrWhiteSpace(item.Value))
                        c += split + "@" + item.Name;

                    if (!string.IsNullOrWhiteSpace(item.Name) && !string.IsNullOrWhiteSpace(item.Value))
                        c += split + "@" + item.Name + "='" + item.Value + "'";
                    num++;
                }

                xPath += "[" + c + "]";
            }
            XmlNodeList nodeList = _xmlDocument.SelectNodes(xPath);
            bool b = true;
            if (nodeList == null || nodeList.Count == 0)
                b = false;
            return b;
        }

        public static string FindNodeContent(string code, List<XMLAttributes> atts = null)
        {
            XmlNode node = FindXmlSingleNode(code, atts);
            string content = node.InnerText;
            return content;
        }
    }
    /// <summary>
    /// XML节点 属性
    /// </summary>
    public class XMLAttributes
    {
        /// <summary>
        /// 属性名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 属性值
        /// </summary>
        public string Value { get; set; }
    }
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
