using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using TCode.Model;

namespace TCode.Helper
{
    public class SerializationCommon
    {
        public static string Path = AppDomain.CurrentDomain.BaseDirectory + @"DB.xml";
        /// <summary>
        /// 序列化对象（生成可视化的XML文件）  必须标记 [Serializable]
        /// </summary>
        /// <param name="o">反序列化对象</param>
        /// <param name="path">存放地址</param>
        public static void SerializeXML(object o)
        {
            try
            {
                FileStream fileStream = new FileStream(Path, FileMode.Create);
                Type type = o.GetType();
                XmlSerializer xs = new XmlSerializer(type);
                using (MemoryStream stream = new MemoryStream())
                {
                    xs.Serialize(stream, o);
                    fileStream.Write(stream.ToArray(), 0, (int)stream.Length);
                }
                //BinaryFormatter b = new BinaryFormatter();
                //b.Serialize(fileStream, o);
                fileStream.Close();
                fileStream.Dispose();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
        /// <summary>
        /// 视化的XML文件反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <returns></returns>
        public static T DeserializeXML<T>()
        {
            try
            {
                using (FileStream fs = new FileStream(Path, FileMode.Open))
                {
                    int len = (int)fs.Length;
                    byte[] buff = new byte[len];
                    fs.Read(buff, 0, len);
                    MemoryStream stream = new MemoryStream(buff, false);
                    XmlSerializer xs = new XmlSerializer(typeof(T));
                    T o = (T)xs.Deserialize(stream);
                    return o;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void Init()
        {
            try
            {
                bool b = System.IO.File.Exists(Path);
                if (!b)
                {
                    List<DB> dblist = new List<DB>();
                    dblist.Add(new DB() { ServiceName = ".", UserID = "sa", Password = "123" });
                    SerializeXML(dblist);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
