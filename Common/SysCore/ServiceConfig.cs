using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.IO;

namespace SysCore
{
    public class ServiceConfig
    {
        private static XmlDocument _Doc = null;
        private static object _Sycobj = new object();

        private static XmlDocument Doc
        {
            get
            {
                if (_Doc == null)
                {
                    lock (_Sycobj)
                    {
                        if (_Doc == null)
                        {
                            _Doc = new XmlDocument();
                            _Doc.Load(ConfigFile);
                        }
                    }
                }
                return _Doc;
            }
        }
        public static readonly string ConfigFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Config\HLServer.xml");

        public static string GetServiceComponent(string intfname)
        {
            string str = null;
            XmlNodeList list = Doc.SelectNodes("/root/services/mapping");
            if (list != null && list.Count > 0)
            {
                foreach (XmlNode node in list)
                {
                    string Interface = node.Attributes["Interface"].Value.Trim();
                    string[] inters = Interface.Split(',');
                    string name = inters[0];
                    if (name == intfname)
                    {
                        str = node.Attributes["component"].Value.Trim().Split(',')[1];

                    }
                }
            }
            return str;
        }

    }
}