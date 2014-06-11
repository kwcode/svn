using System;
using System.Web;
using System.Text;
using System.IO;
using BaseApiCommon;
using UserServiceInterface;
using htmlHelper;

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
                string name = SysCore.ServiceConfig.GetServiceComponent(obj.Interface);
                if (name == null) throw new Exception("配置Config错误");
                string bapath = System.AppDomain.CurrentDomain.BaseDirectory;
                string dll = bapath + @"Bin\" + name + ".DLL";
                object objs = BaseApiCommon.AssemblyCommon.InvokeMember(dll, obj.Interface, obj.MethodName, obj.Parameters);
                buff = BaseApiCommon.SerializationCommon.Serilize(objs);
                context.Response.OutputStream.Write(buff, 0, buff.Length);
            }
            else if (context.Request["Type"] == "java")
            {
                long len = context.Request.InputStream.Length;
                byte[] buff = new byte[len];
                context.Request.InputStream.Read(buff, 0, (int)len);
                object obj = HLXMLHelper.XmlHelper.Load<object>(buff);
                javaInvokeParam param = obj as javaInvokeParam;
                if (param == null) return;
                string name = SysCore.ServiceConfig.GetServiceComponent(param.getInterface());
                if (name == null) throw new Exception("配置Config错误");
                string bapath = System.AppDomain.CurrentDomain.BaseDirectory;
                string dll = bapath + @"Bin\" + name + ".DLL";
                object objs = BaseApiCommon.AssemblyCommon.InvokeMember(dll, param.getInterface(), param.getMethodName(), null);
                javaUserInfo user = new javaUserInfo() { Id = "1", Code = "TKW" };
                byte[] bytes = HLXMLHelper.XmlHelper.Save(user);
                context.Response.OutputStream.Write(bytes, 0, bytes.Length);
            }
        }
        #endregion
    }
}
