using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Controls;
using System.Windows;

namespace WPF.CustomControl
{  /// <summary>
    /// 设置进度条的委托
    /// </summary>
    /// <param name="maxValue"></param>
    /// <param name="currentValue"></param>
    /// <param name="textValue"></param>
    public delegate void SetProgessBarEventHandler(int maxValue, int currentValue, string textValue);
    /// 响应的控制进度条事件的委托
    /// </summary>
    /// <param name="e"></param>
    public delegate void BindProgessBarEventHandler(SetProgessBarEventHandler e);
    public class ProgressBarControl
    {
        /// <summary>
        /// 进度条窗口
        /// </summary>
        private ProgressBar myProcessBar = null;
        SetProgessBarEventHandler mSetProgressBar;
        private BindProgessBarEventHandler mBindProgressBar = null;

        #region 绑定响应进度条的事件
        /// <summary>
        /// 绑定响应进度条的事件
        /// </summary>
        public BindProgessBarEventHandler BindProgressBar
        {
            get
            {
                return mBindProgressBar;
            }
            set
            {
                mBindProgressBar = value;
            }
        }
        #endregion
        /// 显示进度条窗口
        /// </summary>
        public void ShowProcessBar(IAsyncResult result)
        {


        }

    }
}