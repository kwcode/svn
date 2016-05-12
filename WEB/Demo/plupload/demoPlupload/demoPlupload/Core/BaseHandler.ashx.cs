using Newtonsoft.Json;
using System;
using System.Collections.Generic;
 
using System.Web;
using System.Web.SessionState;

namespace trip
{
    /// <summary>
    /// BaseHandler 的摘要说明
    /// </summary>
    public class BaseHandler : IHttpHandler, IRequiresSessionState
    {
        public HttpRequest Request
        {
            get
            {
                return HttpContext.Current.Request;
            }
        }
        public HttpResponse Response
        {
            get
            {
                return HttpContext.Current.Response;
            }
        }
        public void ProcessRequest(HttpContext context)
        {
            var data = ProcessResponse(context);
            var newData = new { code = data.code, data = data.data, msg = data.msg };
            context.Response.ContentType = "application/json";
            string jsonData = JsonConvert.SerializeObject(newData);
            context.Response.Write(jsonData);
        }
        /// <summary>
        /// 定义输出函数
        /// </summary>
        /// <returns></returns>
        protected virtual ResponseData ProcessResponse(HttpContext context)
        {
            ResponseData data = new ResponseData { code = ReturnCode.error, data = "" };
            return data;
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
    /// <summary>
    ///  返回值
    /// </summary>
    public struct ResponseData
    {
        /// <summary>
        /// 返回的状态码
        /// </summary>
        public ReturnCode code;
        /// <summary>
        /// 返回状态码对应的消息
        /// </summary>
        public string msg;
        /// <summary>
        /// 附加内容
        /// </summary>
        public object data;
    }
    /// <summary>
    /// 操作代码 返回给前台JS
    /// </summary>
    public enum ReturnCode
    {
        /// <summary>
        /// 失败 0
        /// </summary>
        error = 0,
        /// <summary>
        /// 成功 1
        /// </summary>
        success = 1,
        /// <summary>
        /// 此处一般用户 支付成功需要跳转和success同时存在
        /// 支付url 2
        /// </summary>
        url = 2,
        /// <summary>
        /// 没有更多数据 3
        /// </summary>
        nodata = 3,
        /// <summary>
        /// 其他 ，自定义状态 返回9
        /// </summary>
        other = 9
    }
}