using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using wox.serial;
using htmlHelper;
using System.Collections;

namespace HLXMLHelper
{
    public class XmlHelper
    {
        public static byte[] Load(byte[] buff)
        {
            // string name = Load<string>(buff);
            // person p = Load<person>(buff);
            ArrayList plist = Load<ArrayList>(buff);
          //  plist as List<person>;
            // plist.GetType();
            //  ArrayList list = new ArrayList() { new person(2, "qy", "桥园") };
            byte[] bb = Save(plist);
            return bb;
        }
        public static byte[] Save(object obj)
        {
            MemoryStream ms = new MemoryStream();
            XmlTextWriter writer = new XmlTextWriter(ms, null);
            ObjectWriter woxWriter = new SimpleWriter();
            woxWriter.write(obj, writer);
            writer.Close();
            return ms.ToArray();
        }
        public static T Load<T>(byte[] buff)
        {
            MemoryStream ms = new MemoryStream(buff);
            XmlTextReader xmlReader = new XmlTextReader(ms);
            ObjectReader woxReader = new SimpleReader();
            xmlReader.Read();
            Object ob = woxReader.read(xmlReader);
            xmlReader.Close();
            return (T)ob;
        }

    }
}
