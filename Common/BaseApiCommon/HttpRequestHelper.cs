using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace BaseApiCommon
{
    /// <summary>
    /// 提供 Get Post
    /// </summary>
    public class HttpRequestHelper
    {
        #region Get

        /// <summary>
        /// 使用Get方法获取字符串结果（没有加入Cookie）
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string HttpGet(string url, Encoding encoding = null)
        {
            WebClient wc = new WebClient();
            wc.Encoding = encoding ?? Encoding.UTF8;
            return wc.DownloadString(url);
        }
        /// <summary>
        /// 使用Get方法获取字符串结果（加入Cookie）
        /// </summary>
        /// <param name="url"></param>
        /// <param name="cookieContainer"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string HttpGet(string url, CookieContainer cookieContainer = null, Encoding encoding = null)
        {
            Stream stream = GetStream(url, cookieContainer);
            StreamReader streamReader = new StreamReader(stream, encoding ?? Encoding.UTF8);
            return streamReader.ReadToEnd();
        }
        /// <summary>
        /// 获取字符流
        /// </summary>
        /// <param name="url"></param>
        /// <param name="cookieContainer"></param>
        /// <returns></returns>
        public static Stream GetStream(string url, CookieContainer cookieContainer)
        {
            HttpWebRequest httpWebRequest = null;
            HttpWebResponse httpWebResponse = null;
            try
            {
                httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(url);
                httpWebRequest.CookieContainer = cookieContainer;
                httpWebRequest.Method = "GET";
                httpWebRequest.ServicePoint.ConnectionLimit = int.MaxValue;
                httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                Stream responseStream = httpWebResponse.GetResponseStream();
                return responseStream;
            }
            catch (Exception)
            {
                return null;
            }
        }
        #endregion

        #region Post
        public static string HttpPost(string url, Dictionary<string, string> parameters = null, CookieContainer cookieContainer = null, Encoding encoding = null)
        {
            if (cookieContainer == null)
                cookieContainer = new CookieContainer();
            Stream stream = PostStream(url, cookieContainer, parameters);
            StreamReader streamReader = new StreamReader(stream, encoding ?? Encoding.UTF8);
            return streamReader.ReadToEnd();
        }
         
        public static Stream PostStream(string url, CookieContainer cookieContainer, Dictionary<string, string> parameters = null)
        {
            HttpWebRequest httpWebRequest = null;
            HttpWebResponse httpWebResponse = null;
            try
            {
                httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(url);
                httpWebRequest.CookieContainer = cookieContainer;
                httpWebRequest.Method = "POST";
                httpWebRequest.ContentType = "application/x-www-form-urlencoded";
                // httpWebRequest.ServicePoint.ConnectionLimit = int.MaxValue;
                if (!(parameters == null || parameters.Count == 0))
                {
                    StringBuilder buffer = new StringBuilder();
                    int i = 0;
                    foreach (string key in parameters.Keys)
                    {
                        if (i > 0)
                        {
                            buffer.AppendFormat("&{0}={1}", key, parameters[key]);
                        }
                        else
                        {
                            buffer.AppendFormat("{0}={1}", key, parameters[key]);
                        }
                        i++;
                    }
                    byte[] data = Encoding.Default.GetBytes(buffer.ToString());
                    //using (Stream stream = request.GetRequestStream()) 
                    using (Stream stream = httpWebRequest.GetRequestStream())
                    {
                        stream.Write(data, 0, data.Length);
                    }
                }
                httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                Stream responseStream = httpWebResponse.GetResponseStream();
                return responseStream;
            }
            catch
            {
                return null;
            }
        }
        #endregion
    }
}
