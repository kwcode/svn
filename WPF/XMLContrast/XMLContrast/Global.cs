using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;

namespace XMLContrast
{
    public class Global
    {
        /// <summary>
        ///   UI界面方法使用的主程序的UI主线程同步器
        /// </summary>
        public static SynchronizationContext SysContext { get { return _SysContext; } set { _SysContext = value; } }
        private static SynchronizationContext _SysContext;
        /// <summary>
        /// 查找控件的上级元素，找出所属的Window
        /// </summary>
        /// <param name="ctrl">指定的控件</param>
        /// <returns>如果找到，返回找到的Window,否则返回null</returns>
        public static Window FindParentWindow(FrameworkElement ctrl)
        {
            FrameworkElement parent = ctrl.Parent as FrameworkElement;
            while (parent != null)
            {
                if (parent is Window) return parent as Window;
                if (parent.Parent == null) break;
                parent = parent.Parent as FrameworkElement;
            }
            return null;
        }

    }
}
