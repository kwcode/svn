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
using System.Windows.Forms;
using System.Windows.Interop;

namespace AutomaticSound
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
                    int ts = 3;
                    int.TryParse(txt_ts.Text, out ts);
                    timer.Interval = TimeSpan.FromSeconds(ts);   //设置刷新的间隔时间
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
        //ShowWindow参数
        private const int SW_SHOWNORMAL = 1;
        private const int SW_RESTORE = 9;
        private const int SW_SHOWNOACTIVATE = 4;
        //SendMessage参数
        private const int WM_KEYDOWN = 0X100;
        private const int WM_KEYUP = 0X101;
        private const int WM_SYSCHAR = 0X106;
        private const int WM_SYSKEYUP = 0X105;
        private const int WM_SYSKEYDOWN = 0X104;
        private const int WM_CHAR = 0X102;
        void timer_Tick(object sender, EventArgs e)
        {
            //判断窗体是否在最前面
            IntPtr b = WinApi.GetForegroundWindow();
            msg("最前面的句柄为: " + b);
            if (b == CurrentTask.hWnd)
            {
                //System.Windows.Forms.SendKeys.SendWait("{UP}");
                //System.Windows.Forms.SendKeys.SendWait("{ENTER}");


                int i = Convert.ToInt32(txt_DA.Text, 16);
                msg(i.ToString());

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

        private void btn_test_Click(object sender, RoutedEventArgs e)
        {
            CurrentTask = comb_title.SelectedItem as Task;
            IntPtr hWnd = WinApi.FindWindow("Notepad", "梦幻.txt - 记事本");

            ////   IntPtr childHwnd = WinApi.FindWindowEx(hWnd, IntPtr.Zero, "Edit", null);   //获得按钮的句柄
            //const int WM_SETTEXT = 0x0C;
            //string s100 = "aa";
            //IntPtr hWnd2 = WinApi.FindWindow("Eidt", null);

            //IntPtr childHwnd = WinApi.FindWindowEx(hWnd, IntPtr.Zero, "Edit", CurrentTask.Title);   //获得按钮的句柄  


            //const int EM_SETSEL = 0x00B1;
            //const int EM_REPLACESEL = 0x00C2;
            //int handle = new WindowInteropHelper(this).Handle.ToInt32();
            //IntPtr hWnd = WinApi.FindWindow("WindowsForms10.Window.8.app.0.b7ab7b_r11_ad1", "Form1");
            //IntPtr childHwnd = WinApi.FindWindowEx(hWnd, IntPtr.Zero, "WindowsForms10.EDIT.app.0.b7ab7b_r11_ad1", null);   //获得按钮的句柄
            //   WinApi.SendMessage((IntPtr)i, 0x00C2, 0, "内容");
            int i = Convert.ToInt32(txt_DA.Text, 16);
            msg(i.ToString());
            const int WM_KEYDOWN = 0x0100;
            const int WM_KEYUP = 0x0101; 
            WinApi.SendMessage((IntPtr)i, WM_KEYDOWN, 32, 0);
            WinApi.SendMessage((IntPtr)i, WM_KEYUP, 32, 0);
        }
    }
}
