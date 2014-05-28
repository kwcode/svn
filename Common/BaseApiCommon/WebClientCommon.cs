using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Cache;
using System.IO;

namespace BaseApiCommon
{
    public class WebClientCommon
    {
        #region Http WebClientPOST数据

        /// <summary>
        /// 以post的方式向服务器发送请求, 并获取服务器返回的数据
        /// </summary>
        /// <param name="url"></param>
        /// <param name="dataposttoserver"></param>
        /// <returns></returns>
        public static byte[] GetFromServer(string url, byte[] dataposttoserver)
        {
            int num = (int)((double)dataposttoserver.Length * 1.1 / 20.0);
            if (num < 100000)
            {
                num = 100000;
            }
            HttpWebRequest request = CreateRequest(url, num);
            HttpWebResponse response = GetResponse(request, dataposttoserver);
            byte[] result;
            try
            {
                Stream responseStream = response.GetResponseStream();
                byte[] array = ReadBytesFromStream(responseStream);
                response.Close();
                result = array;
            }
            catch (Exception ex)
            {
                response.Close();
                throw ex;
            }
            return result;
        }

        private static HttpWebRequest CreateRequest(string url, int timeOut)
        {
            HttpWebRequest result;
            try
            {
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                //httpWebRequest.UserAgent = HLWebClient.UserAgent;
                httpWebRequest.ContentType = "application/octet-stream";
                httpWebRequest.Method = "POST";
                httpWebRequest.CachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.NoCacheNoStore);
                if (timeOut >= 100000)
                {
                    httpWebRequest.Timeout = timeOut;
                }
                result = httpWebRequest;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        private static HttpWebResponse GetResponse(HttpWebRequest request, byte[] dataposttoserver)
        {
            HttpWebResponse httpWebResponse = null;
            try
            {
                if (dataposttoserver != null && dataposttoserver.Length > 0)
                {
                    request.ContentLength = (long)dataposttoserver.Length;
                    using (Stream requestStream = request.GetRequestStream())
                    {
                        requestStream.Write(dataposttoserver, 0, dataposttoserver.Length);
                        requestStream.Close();
                    }
                }
                httpWebResponse = (HttpWebResponse)request.GetResponse();
            }
            catch (WebException ex)
            {
                throw ex;
            }
            if (httpWebResponse.StatusCode != HttpStatusCode.OK)
            {
                httpWebResponse.Close();
                //CommunicationException ex4 = new CommunicationException(24u, httpWebResponse.StatusDescription, null);
                //if (httpWebResponse.StatusCode == HttpStatusCode.RequestTimeout)
                //{
                //    ex4.ErrorCode = 28u;
                //}
                //throw ex4;
            }
            return httpWebResponse;
        }
        public static byte[] ReadBytesFromStream(Stream stream)
        {
            using (MemoryStream stream2 = new MemoryStream())
            {
                byte[] buffer = new byte[0x400];
                int count = 0;
                do
                {
                    count = stream.Read(buffer, 0, 0x400);
                    if (count != 0)
                    {
                        stream2.Write(buffer, 0, count);
                    }
                }
                while (count != 0);
                stream2.Flush();
                stream2.Position = 0L;
                return stream2.ToArray();
            }
        }

        #endregion
    }
}
