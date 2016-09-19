using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.IO;

namespace FileBackup
{
    public  class Util
    {
        #region 取得Bin源文件目录
        public static string SourceBin()
        {
            return ConfigurationSettings.AppSettings["SourceBin"].ToString();
        }
        #endregion

        #region 取得Bin要更新文件目录
        public static string TargetBin()
        {
            return ConfigurationSettings.AppSettings["TargetBin"].ToString();
        }
        #endregion

        public static void Log(string str)
        {
            string txtConnPath = System.Windows.Forms.Application.StartupPath + "\\Log.txt";
            FileStream fs = new FileStream(txtConnPath, FileMode.Append);
            StreamWriter StWrite = new StreamWriter(fs);
            StWrite.WriteLine(str+"--"+System.DateTime.Now.ToShortDateString()+" 已更新成功");
            StWrite.Flush();
            fs.Close();
        }
    }
}
