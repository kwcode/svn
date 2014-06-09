using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SysCore
{
    /// <summary>
    ///  一个标识,用于指明接口是一个服务接口
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface)]
    public sealed class ServiceContractAttribute : Attribute
    {

    }
}
