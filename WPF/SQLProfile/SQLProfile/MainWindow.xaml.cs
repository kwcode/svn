using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Trace;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SQLProfile
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            InitBase();
            this.btn_Begin.Click += btn_Begin_Click;
            this.btn_Pause.Click += btn_Pause_Click;
            this.btn_Clear.Click += btn_Clear_Click;
            this.btn_Stop.Click += btn_Stop_Click;
            this.btn_Init.Click += btn_Init_Click;
        }

        void btn_Init_Click(object sender, RoutedEventArgs e)
        {
            InitBase();
        }

        void btn_Stop_Click(object sender, RoutedEventArgs e)
        {
            trcReader.Stop();
        }

        void btn_Clear_Click(object sender, RoutedEventArgs e)
        {
            list.Clear();
        }
        /// <summary>
        /// 暂停
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btn_Pause_Click(object sender, RoutedEventArgs e)
        {
            trcReader.Pause();
            SetStatus("暂停");
        }

        WSLog wslog = new WSLog();
        TraceServer trcReader = new TraceServer();
        SqlConnectionInfo connInfo = new SqlConnectionInfo();
        Thread trceThread = null;
        string profileFileName = "";
        List<string> MonitorDBList = new List<string>();
        private void InitBase()
        {
            string MonitorDB = System.Configuration.ConfigurationManager.AppSettings["MonitorDB"];

            if (!string.IsNullOrEmpty(MonitorDB))
            {
                string[] arry = MonitorDB.Split(',');
                foreach (string item in arry)
                {
                    MonitorDBList.Add(item);
                }
            }
            txt_ServiceName.Text = ConfigurationManager.AppSettings["ServiceName"];
            txt_DatabaseName.Text = ConfigurationManager.AppSettings["DatabaseName"];
            txt_UserName.Text = ConfigurationManager.AppSettings["UserName"];
            txt_Password.Text = ConfigurationManager.AppSettings["Password"];
            profileFileName = AppDomain.CurrentDomain.BaseDirectory + @"Templates/tkw.tdf";
        }
        double time = 500;
        void btn_Begin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                time = Convert.ToDouble(txt_Dur.Text.Trim());
                connInfo.ServerName = txt_ServiceName.Text;
                connInfo.UserName = txt_UserName.Text;
                connInfo.Password = txt_Password.Text;
                string _dbName = txt_DatabaseName.Text;
                if (!string.IsNullOrEmpty(_dbName))
                {
                    //似乎没什么用
                    connInfo.DatabaseName = _dbName;
                }
                connInfo.UseIntegratedSecurity = false;
                trcReader.InitializeAsReader(connInfo, profileFileName);
                trceThread = new Thread(new ThreadStart(DoThreadReadTrace));
                trceThread.Priority = ThreadPriority.AboveNormal;
                trceThread.Start();
                SetStatus("正在运行中");
                list.Add(new TraceData()
                {
                    DatabaseName = "aa",
                    EventClass = "bb"
                });
                lv_data.ItemsSource = list;

            }
            catch (Exception ex)
            {
                SetStatus("运行有问题");
                Msg(ex.ToString());
                //MessageBox.Show(ex.ToString());
            }

        }
        string[] strRow = new string[4];
        ObservableCollection<TraceData> list = new ObservableCollection<TraceData>();
        private void DoThreadReadTrace()
        {
            try
            {

                while (trcReader.Read())
                {
                    if (trcReader.FieldCount > 0)
                    {
                        //去掉当前的数据库
                        string _DatabaseName = (trcReader["DatabaseName"] ?? "").ToString();
                        //if (!MonitorDBList.Contains(_DatabaseName))
                        //{
                        //    continue;
                        //}
                        TraceData tdata = new TraceData();
                        tdata.EventClass = trcReader["EventClass"].ToString();
                        if (tdata.EventClass.ToLower().Contains("audit logout"))
                        {
                            continue;
                        }
                        tdata.TextData = ReplaceWrap((trcReader["TextData"] ?? "").ToString());
                        tdata.LoginName = (trcReader["LoginName"] ?? "").ToString();
                        tdata.SPID = Convert.ToInt32(trcReader["SPID"]);

                        double Duration = Convert.ToDouble(trcReader["Duration"]);
                         
                        double dur = Duration / 1000;
                        if (dur < time)
                        {
                            continue;
                        }
                        tdata.Duration = Convert.ToInt64(trcReader["Duration"]);
                        tdata.DurationStr = dur.ToString() + "秒";
                        tdata.ApplicationName = (trcReader["ApplicationName"] ?? "").ToString();
                        tdata.StartTime = Convert.ToDateTime(trcReader["StartTime"]);
                        DateTime? TimeNull = null;
                        tdata.EndTime = trcReader["EndTime"] == null ? TimeNull : Convert.ToDateTime(trcReader["EndTime"]);
                        tdata.CPU = Convert.ToInt32(trcReader["CPU"]);
                        tdata.ClientProcessID = Convert.ToInt32(trcReader["ClientProcessID"]);
                        tdata.NTUserName = (trcReader["NTUserName"] ?? "").ToString();
                        tdata.Reads = Convert.ToInt64(trcReader["Reads"]);
                        tdata.Writes = Convert.ToInt64(trcReader["Writes"]);
                        tdata.DatabaseName = _DatabaseName;
                        tdata.HostName = (trcReader["HostName"] ?? "").ToString();
                        tdata.TransactionID = Convert.ToInt32(trcReader["TransactionID"]);

                        if (tdata.Duration <= 0)
                        {
                            continue;
                        }
                        Global.SysContext.Send(o =>
                        {
                            list.Add(tdata);
                            //滚动到最后
                            lv_data.SelectedIndex = lv_data.Items.Count - 1;
                            lv_data.ScrollIntoView(lv_data.SelectedItem);

                            //#region 记录数据库
                            //wslog.AddLog(tdata.EventClass, tdata.TextData, tdata.LoginName, tdata.SPID, tdata.Duration, tdata.ApplicationName, tdata.StartTime, tdata.EndTime, tdata.CPU, tdata.ClientProcessID, tdata.NTUserName, tdata.Reads, tdata.Writes, tdata.DatabaseName, tdata.HostName, tdata.TransactionID);
                            //#endregion
                        }, null);
                    }

                }
            }
            catch (Exception ex)
            {
                trcReader.Stop();
                Global.SysContext.Send(o =>
                        {
                            SetStatus("错误停止");
                            Msg(ex.ToString());
                        }, null);
                throw ex;
            }
        }
        /// <summary>
        /// 去掉换行
        /// </summary>
        /// <returns></returns>
        private string ReplaceWrap(string replaceContent)
        {
            string str = "";
            str = replaceContent.Replace("\r\n", "-");
            str = str.Replace("\n", "-");
            return str;
        }
        #region 私有方法
        private void SetStatus(string status)
        {
            tb_status.Text = status;
        }

        private void Msg(string msg)
        {
            txt_msg.Text += "\n\r" + DateTime.Now.ToString("yyyy-MM-dd") + ":" + msg;
            txt_msg.ScrollToEnd();
        }
        #endregion
    }
}
