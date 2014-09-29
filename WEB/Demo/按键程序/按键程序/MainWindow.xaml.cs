using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Runtime.InteropServices;
using System.Windows.Threading;

namespace 按键程序
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            txt_msg.ScrollToEnd();
        }
        DispatcherTimer timer = new DispatcherTimer();
        //此处用于取得计算器窗口的句柄
        [System.Runtime.InteropServices.DllImport("user32.dll", EntryPoint = "FindWindow")]
        public static extern int FindWindow(
                   string lpClassName,
                   string lpWindowName
        );
        //此处用于向窗口发送消息
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        //此处用于将窗口设置在最前
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool SetWindowPos(IntPtr hWnd,
        int hWndInsertAfter,
        int X,
        int Y,
        int cx,
        int cy,
        int uFlags
        );
        //   窗口置前   
        public static void SetWindowPos(IntPtr hWnd)
        {
            //SetWindowPos(hWnd, -1, 0, 0, 0, 0, 0x4000 | 0x0001 | 0x0002);
            SetWindowPos(hWnd, -1, 0, 0, 0, 0, 3);
        }

        private void btn_start_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                txt_msg.Text = "清空";
                count = 1;
                int.TryParse(txt_sum.Text, out maxsum);
                int k = FindWindow(null, txt_title.Text);//余海刚
                string k16 = System.Convert.ToString(k, 16);
                //MessageBox.Show(k16.ToString()); 
                IntPtr iptr = new IntPtr(k);
                SetWindowPos(iptr);
                timer.Interval = TimeSpan.FromSeconds(3);   //设置刷新的间隔时间
                timer.Tick += new EventHandler(timer_Tick);
                timer.Start();
                msg("程序开始");

            }
            catch (Exception ex)
            {
                msg(ex.Message);
            }
        }
        int count = 1;
        int maxsum = 10;
        void timer_Tick(object sender, EventArgs e)
        {
            System.Windows.Forms.SendKeys.SendWait("{UP}");
            System.Windows.Forms.SendKeys.SendWait("{ENTER}");
            count++;
            msg(" " + count.ToString() + "次");
            if (count > maxsum)
            {
                timer.Stop();
            }
        }
        private void msg(string msg)
        {
            txt_msg.Text += DateTime.Now.ToString("MM-dd hh:mm:ss") + ": " + msg + "\n";
        }

        private void btn_stop_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            msg("程序开始！");
        }
    }
}
