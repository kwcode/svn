using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace TCode
{
    public class Global
    {
        private static SynchronizationContext _SysContext;
        /// <summary>
        /// UI界面方法使用的主程序的UI主线程同步器
        /// </summary>
        public static SynchronizationContext SysContext { get { return _SysContext; } set { _SysContext = value; } }
    }
}
