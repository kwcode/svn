using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using trip;
using trip.CommunalClass;

namespace DBCommonTest
{
    public partial class Form1 : Form
    {
        static int capacity = 5;
        static string connString = "Data Source=119.147.24.179,61433;Initial Catalog=trip;User ID=sa;Password=i20sll9e0slI2T0fm08bz7;";
        // @"Data Source=(LocalDB)\v11.0;AttachDbFilename=E:\svn\网上Git\EasyEF\Backup\EasyEF.WCFService\App_data\MyDb.mdf;Integrated Security=True;Connect Timeout=30";
        //  connString = "Data Source=112.74.104.180;Initial Catalog=SZYJKJ;User ID=sa;Password=Abc123456;";

        public SynchronizationContext SysContext { get; set; }
        MessageDisplay md = null;
        public Form1()
        {
            InitializeComponent();
            SysContext = SynchronizationContext.Current;
            md = new MessageDisplay(txt_msg);
        }

        public static DataPool dataCon = new DataPool(capacity, connString);
        private void button1_Click(object sender, EventArgs e)
        {

            new Thread(DoOpenTask).Start();

        }

        private void DoOpenTask(object obj)
        {
        }
        int maxThread = 50;
        private void button2_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < maxThread; i++)
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(DoTask), i);
            }

            //new Thread(DoTask).Start();

        }
        private void DoTask(object obj)
        {
            Stopwatch sw2 = new Stopwatch();
            sw2.Start();
            for (int i = 0; i < 100; i++)
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                DataTable dt = dataCon.ExecuteDataTable("TKW_TEST");
                sw.Stop();
                TimeSpan ts2 = sw.Elapsed;
                if (ts2.TotalSeconds > 1)
                {
                  //  msg("第" + i + "个，花费" + ts2.TotalSeconds);
                }
            }
            TimeSpan ts3 = sw2.Elapsed;
            msg("第-" + obj + "总花费的时间：" + ts3.TotalSeconds);
        }
        public void msg(object msg)
        {
            SysContext.Send(o =>
            {
                txt_msg.Text += DateTime.Now.ToString("yyyy-MM-dd HH:mm") + ":" + msg + "\r\n";
                txt_msg.SelectionStart = txt_msg.Text.Length;
                txt_msg.ScrollToCaret();
            }, null);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            txt_msg.Text = "";
            int i = 0;
            foreach (DataClass item in dataCon.dataConnPool)
            {
                i++;
                msg("第" + i + "个，" + item.UseredCount);
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            //new Thread(DoThreadTask2).Start();
            for (int i = 0; i < maxThread; i++)
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(DoThreadTask2), i);
            }

        }

        private void DoThreadTask2(object obj)
        {
            Stopwatch sw2 = new Stopwatch();
            sw2.Start();
            for (int i = 0; i < 100; i++)
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                DataTable dt = Get_1();
                sw.Stop();
                TimeSpan ts2 = sw.Elapsed;
                if (ts2.TotalSeconds > 1)
                {
                   // msg("第" + i + "个，花费" + ts2.TotalSeconds);
                }
            }
            TimeSpan ts3 = sw2.Elapsed;
            msg("第-" + obj + "有关闭的链接-总花费的时间：" + ts3.TotalSeconds);
        }
        private DataTable Get_1()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();
                    // Invoke RegionUpdate Procedure
                    SqlCommand cmd = new SqlCommand("TKW_TEST", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dataTable = new DataTable("yes");
                    adapter.Fill(dataTable);
                    SqlDataReader reader = cmd.ExecuteReader();
                    return dataTable;
                }
            }
            catch (SqlException ex)
            {
                msg(ex.Message);
                return null;
            }
        }

    }
}
