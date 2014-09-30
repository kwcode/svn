using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace 按键程序
{
    public class WinApi
    {
        /// <summary>
        /// 该函数将创建指定窗口的线程设置到前台，并且激活该窗
        /// </summary>
        /// <param name="hwnd">句柄</param>
        /// <returns></returns>
        [DllImport("user32")]
        public static extern int SetForegroundWindow(IntPtr hwnd);
        //   窗口置前   
        public static void SetWindowPos(IntPtr hWnd)
        {
            //SetWindowPos(hWnd, -1, 0, 0, 0, 0, 0x4000 | 0x0001 | 0x0002);
            //SetWindowPos(hWnd, -1, 0, 0, 0, 0, 3);
            SetWindowPos(hWnd, -2, 0, 0, 0, 0, 3);
        }
        //此处用于将窗口设置在最前
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool SetWindowPos(IntPtr hWnd, int hWndInsertAfter,
        int X,
        int Y,
        int cx,
        int cy,
        int uFlags
        );
        /// <summary>
        ///  窗体 最小化显示
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="nCmdShow"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);
        /// <summary>
        /// 判断窗体是否否是最小化
        /// </summary>
        /// <param name="hWnd">句柄哦</param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern bool IsIconic(IntPtr hWnd);
        [DllImport("user32.dll")]
        public static extern IntPtr GetTopWindow(IntPtr hWnd);
        //此处用于取得计算器窗口的句柄
        [System.Runtime.InteropServices.DllImport("user32.dll", EntryPoint = "FindWindow")]
        public static extern int FindWindow(
                   string lpClassName,
                   string lpWindowName
        );
        //此处用于向窗口发送消息
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        /// <summary>
        /// 获取一个前台窗口的句柄
        /// </summary>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();
    }
}
