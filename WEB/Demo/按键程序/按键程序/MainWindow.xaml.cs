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
using System.Diagnostics;

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
            Init();
        }
        private List<Task> tasklist = new List<Task>();
        private Task CurrentTask;
        private void Init()
        {
            Process[] proArry = Process.GetProcesses();
            foreach (Process item in proArry)
            {
                if (item.MainWindowHandle != null)
                {
                    if (!string.IsNullOrEmpty(item.MainWindowTitle))
                    {
                        if (item.MainWindowTitle.Contains("梦幻"))
                        {
                            tasklist.Add(new Task() { ID = item.Id, hWnd = item.MainWindowHandle, Title = item.MainWindowTitle });
                        }
                    }
                }
            }
            comb_title.ItemsSource = tasklist;
            comb_title.DisplayMemberPath = "Title";
            comb_title.SelectedIndex = 0;
            msg("程序已经初始化！");
        }
        DispatcherTimer timer = new DispatcherTimer();

        private void btn_start_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CurrentTask = comb_title.SelectedItem as Task;
                if (CurrentTask == null)
                {
                    msg("请选择执行的程序模块！");
                }
                else
                {
                    txt_msg.Text = "清空";
                    count = 1;
                    IntPtr hWnd = CurrentTask.hWnd;
                    if (WinApi.IsIconic(hWnd))
                    {
                        WinApi.ShowWindowAsync(hWnd, 9);
                    }
                    WinApi.SetForegroundWindow(hWnd);
                    //    WinApi.SetWindowPos(iptr);
                    msg("正在将" + CurrentTask.Title + "前置！");
                    timer.Interval = TimeSpan.FromSeconds(3);   //设置刷新的间隔时间
                    timer.Tick += new EventHandler(timer_Tick);
                    timer.Start();
                    showInfo(CurrentTask.hWnd, CurrentTask.Title);
                    msg("程序开始");
                }
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
            //判断窗体是否在最前面
            IntPtr b = WinApi.GetForegroundWindow();
            msg("最前面的句柄为: " + b);
            if (b == CurrentTask.hWnd)
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
            else
            {
                msg("窗体被隐藏，需要将窗体前置！");
            }
        }

        private void msg(string msg)
        {
            txt_msg.Text += DateTime.Now.ToString("MM-dd hh:mm:ss") + ": " + msg + "\n";
            txt_msg.ScrollToEnd();
        }
        private void showInfo(IntPtr hWnd, string title)
        {
            txt_info.Text = "运行的相关信息：窗体句柄" + hWnd + " ， 窗体名称" + title;
        }

        private void btn_stop_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            msg("程序开始！");
        }

        #region 查找所有应用程序标题

        private const int GW_HWNDFIRST = 0;
        private const int GW_HWNDNEXT = 2;
        private const int GWL_STYLE = (-16);
        private const int WS_VISIBLE = 268435456;
        private const int WS_BORDER = 8388608;

        #region AIP声明
        [DllImport("IpHlpApi.dll")]
        extern static public uint GetIfTable(byte[] pIfTable, ref uint pdwSize, bool bOrder);

        [DllImport("User32")]
        private extern static int GetWindow(int hWnd, int wCmd);

        [DllImport("User32")]
        private extern static int GetWindowLongA(int hWnd, int wIndx);

        [DllImport("user32.dll")]
        private static extern bool GetWindowText(int hWnd, StringBuilder title, int maxBufSize);

        [DllImport("user32", CharSet = CharSet.Auto)]
        private extern static int GetWindowTextLength(IntPtr hWnd);
        #endregion
        #endregion

        private void btn_Ref_Click(object sender, RoutedEventArgs e)
        {
            msg("正在刷新");
            Init();
        }
    }
}
