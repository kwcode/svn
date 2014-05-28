using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows; 
using System.Windows.Media;
 

namespace WPF.CustomControl
{
    /// <summary>
    /// HLWindow的增强类，增加了进度条。默认隐藏WPF的默认图标。
    /// </summary>
    [TemplatePart(Name = "winProgressBar", Type = typeof(HLProgressBar))]
    public class HLWindowExt : Window
    {
        private HLProgressBar _winProgressBar;
        /// <summary>
        /// 构造函数。
        /// </summary>
        public HLWindowExt()
            : base()
        {

            ResourceDictionary rd = Application.LoadComponent(new Uri("/GenerateMachine;Component/Resources/ResHLWindow.xaml", UriKind.Relative)) as ResourceDictionary;
            Style s = rd["cusWin"] as Style;
            Style = s;
            Unloaded += new RoutedEventHandler(HLWindowExt_Unloaded);
            FontFamily = new FontFamily("宋体");
            FontSize = 12;
        }
        /// <summary>
        /// 模版函数。
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _winProgressBar = Template.FindName("winProgressBar", this) as HLProgressBar;
        }
        void HLWindowExt_Unloaded(object sender, RoutedEventArgs e)
        {
            StopProgress();
        }
        /// <summary>
        /// 启动进度条。
        /// </summary>
        public void StartProgress()
        {
            if (_winProgressBar != null)
            {
                _winProgressBar.Start();
            }
        }
        /// <summary>
        /// 停止进度条。
        /// </summary>
        public void StopProgress()
        {
            if (_winProgressBar != null)
                _winProgressBar.Stop();
        }
    }
}
