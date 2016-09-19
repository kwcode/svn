using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Microsoft.Win32;
using System.Diagnostics;

using System.Globalization;
using System.Collections;
using System.Text.RegularExpressions;
using FileBackup;

namespace DataBack
{
    public partial class index : Form
    {

        DataBack.Components.Config Config = new DataBack.Components.Config();

        public index()
        {
            InitializeComponent();
        }

        private void index_Load(object sender, EventArgs e)
        {
            this.SourceBinTs.Text = "源文件目录:"+Util.SourceBin();//Bin源文件目录
            this.TargetBinTs.Text = "要更新文件目录:" + Util.TargetBin();//Bin要更新文件目录
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string SourceBin = Util.SourceBin();//Bin源文件目录
            string TargetBin = Util.TargetBin();//Bin要更新文件目录

            if (TargetBin.ToLower().IndexOf("tc") == -1)
            {
                Log.Text = "失败，路径中必须含有tc";
                return;
            }

            //检测源路径是否存在。
            if (Directory.Exists(TargetBin))
            {
                #region 遍历目标文件夹
                string[] dirs = Directory.GetDirectories(TargetBin);
                foreach (string dir in dirs)
                {
                    string TargetUrl = dir + "\\Bin\\";

                    Config.CopyDirectory(SourceBin, TargetUrl);

                    Util.Log(TargetUrl);//日志提示;
                }
                #endregion 遍历目标文件夹
            }

            Log.Text = "更新bin成功，已输出日志到Log.txt中，请查看！";                

        }
    }
}
