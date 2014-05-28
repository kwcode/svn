using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Xml.Serialization;

namespace BaseApiCommon
{
    /// <summary>
    /// 序列化
    ///  [Serializable]//表示一个类可以被序列化。这个类不能被继承。
    ///  [XmlIgnore]//忽略 XML序列号
    ///   [NonSerialized]//表示一个序列化的类的一个字段不应序列。  
    /// </summary>
    public class SerializationCommon
    {
        /// <summary>
        /// 序列化对象(生成XML文件为乱码) 必须标记 [Serializable]
        /// </summary>
        /// <param name="o">反序列化对象</param>
        /// <param name="path">存放完整地址(默认当前程序下面)</param>
        public static void SerializeObject(object o, string path)
        {
            try
            {
                BinaryFormatter binFormatter = new BinaryFormatter();
                using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.Read))
                {
                    binFormatter.Serialize(fs, o);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        /// <summary>
        /// 序列化对象（生成可视化的XML文件）  必须标记 [Serializable]
        /// </summary>
        /// <param name="o">反序列化对象</param>
        /// <param name="path">存放地址</param>
        public static void SerializeXML(object o, string path)
        {
            try
            {
                Type type = o.GetType();
                XmlSerializer xs = new XmlSerializer(type);
                using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.Read))
                {
                    xs.Serialize(fs, o);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        /// <summary>
        /// 反序列化对象
        /// </summary>
        /// <param name="path">还原序列化的对象</param>
        /// <returns></returns>
        public static object DeserializeObject(string path)
        {
            try
            {
                BinaryFormatter binFormatter = new BinaryFormatter();
                object o;
                using (FileStream fs = new FileStream(path, FileMode.Open))
                {
                    o = binFormatter.Deserialize(fs);
                }
                return o;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }

        /// <summary>
        /// 反序列化对象
        /// </summary>
        /// <param name="path">还原序列化的对象</param>
        /// <returns></returns>
        //public static object DeserializeXML(string path)
        //{
        //    try
        //    {
        //      //  XmlSerializer binFormatter = new XmlSerializer();
        //        object o;
        //        using (FileStream fs = new FileStream(path, FileMode.Open))
        //        {
        //            o = binFormatter.Deserialize(fs);
        //        }
        //        return o;
        //    }
        //    catch (Exception ex)
        //    {

        //        throw new Exception(ex.Message);
        //    }

        //}
    }
}
