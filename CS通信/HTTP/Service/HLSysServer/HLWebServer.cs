using System;
using System.Web;
using System.Text;
using System.IO;
using BaseApiCommon;

namespace HL.ServiceModel
{
    public class HLWebServer : IHttpHandler
    {
        #region IHttpHandler Members

        public bool IsReusable
        {
            // Return false in case your Managed Handler cannot be reused for another request.
            // Usually this would be false in case you have some state information preserved per request.
            get { return true; }
        }
        public void ProcessRequest(HttpContext context)
        {
            if (context.Request["Type"] == "0")
            {
                long len = context.Request.InputStream.Length;
                byte[] buff = new byte[len];
                context.Request.InputStream.Read(buff, 0, (int)len);
                InvokeParam obj = (InvokeParam)BaseApiCommon.SerializationCommon.Deserilize(buff);
                if (obj == null) return;
                string bapath = System.AppDomain.CurrentDomain.BaseDirectory;
                string dll = bapath + @"Bin\ServiceComponent.DLL";
                object objs = BaseApiCommon.AssemblyCommon.InvokeMember(dll, obj.Interface, obj.MethodName, obj.Parameters);
                buff = BaseApiCommon.SerializationCommon.Serilize(objs);
                context.Response.OutputStream.Write(buff, 0, buff.Length);
            }
            else
            {

            }
        }
        #endregion
    }
}
