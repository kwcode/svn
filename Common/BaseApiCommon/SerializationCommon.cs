using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Soap;

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
        #region 对象

        /// <summary>
        /// 序列化对象  对象必须允许序列化[Serializable]
        /// </summary> 
        public static byte[] Serilize(object obj)
        {
            if (obj == null)
            {
                return null;
            }
            BinaryFormatter formatter = new BinaryFormatter();
            using (MemoryStream stream = new MemoryStream())
            {
                formatter.Serialize(stream, obj);
                return stream.ToArray();
            }
        }
        /// <summary>
        /// 反序列化
        /// </summary> 
        public static object Deserilize(byte[] buff)
        {
            if ((buff == null) || (buff.Length == 0))
                return null;
            using (MemoryStream stream = new MemoryStream(buff, false))
            {
                return new BinaryFormatter().Deserialize(stream);
            }
        }

        #endregion

        #region XML序列化和反序列化[可视XML文件]

        /// <summary>
        /// 序列化对象（生成可视化的XML文件）  必须标记 [Serializable]
        /// </summary>
        /// <param name="o">反序列化对象</param>
        /// <param name="path">存放地址</param>
        public static byte[] SerializeXML(object o)
        {
            try
            {
                Type type = o.GetType();
                XmlSerializer xs = new XmlSerializer(type);
                using (MemoryStream stream = new MemoryStream())
                {
                    xs.Serialize(stream, o);
                    return stream.ToArray();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// 视化的XML文件反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <returns></returns>
        public static T DeserializeXML<T>(byte[] buff)
        {
            try
            {
                if ((buff == null) || (buff.Length == 0))
                    return default(T);
                using (MemoryStream stream = new MemoryStream(buff, false))
                {
                    XmlSerializer xs = new XmlSerializer(typeof(T));
                    T o = (T)xs.Deserialize(stream);
                    return o;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion

        #region Soap序列化

        /// <summary>
        /// 序列化Soap对象  对象必须允许序列化[Serializable]
        /// </summary> 
        public static Byte[] SerializeSoap(object obj)
        {
            try
            {
                SoapFormatter formatter = new SoapFormatter();
                using (MemoryStream stream = new MemoryStream())
                {
                    formatter.Serialize(stream, obj);
                    return stream.ToArray();
                }
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        /// <summary>
        /// 反序列化Soap
        /// </summary> 
        public static T DeSerializeSoap<T>(Byte[] buff)
        {
            if ((buff == null) || (buff.Length == 0))
                return default(T);
            using (MemoryStream stream = new MemoryStream(buff, false))
            {
                return (T)new SoapFormatter().Deserialize(stream);
            }
        }
        #endregion

         
    }
}
