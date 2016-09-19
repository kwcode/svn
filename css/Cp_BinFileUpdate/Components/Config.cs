using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Win32;
using System.Diagnostics;

namespace DataBack.Components
{
    class Config
    {
        #region 复制文件夹和文件
        ///   <summary>     
        ///   复制文件夹     
        ///   </summary>     
        ///   <param   name="sourceDirName">源文件夹</param>     
        ///   <param   name="destDirName">目标文件夹</param> 
        
        public void CopyDirectory(string sourceDirName, string destDirName)
        {
            if (!Directory.Exists(destDirName))
            {
                try
                {
                    Directory.CreateDirectory(destDirName);
                    File.SetAttributes(destDirName, File.GetAttributes(sourceDirName));
                }
                catch
                {
                }
                //File.SetAttributes(destDirName,FileAttributes.Normal);     
            }

            if (destDirName[destDirName.Length - 1] != Path.DirectorySeparatorChar)
                destDirName = destDirName + Path.DirectorySeparatorChar;

            string[] files = Directory.GetFiles(sourceDirName);
            foreach (string file in files)
            {
                try
                {
                    File.Copy(file, destDirName + Path.GetFileName(file), true);
                    File.SetAttributes(destDirName + Path.GetFileName(file), FileAttributes.Normal);
                }
                catch
                {
                }
               
            }

            string[] dirs = Directory.GetDirectories(sourceDirName);
            foreach (string dir in dirs)
            {
                try
                {
                    CopyDirectory(dir, destDirName + Path.GetFileName(dir));
                }
                catch
                {
                }
            }

        }
        #endregion

        #region 根据文件名称 返回路径
        //txtFilePath 如：\OpportunitiesPic
        //txtFileName 如：today.gif

        public string GetFilePath(string txtFileName)
        {

            string txtPath = "";

            if (txtFileName.Length > 14)
            {

                //取得年起始位置
                int intFileLenth = txtFileName.Length - 14 - (txtFileName.Length - txtFileName.LastIndexOf("."));

                //年 
                txtPath += "\\" + txtFileName.Substring(intFileLenth, 4);

                //月 年4
                txtPath += "\\" + txtFileName.Substring(intFileLenth + 4, 2);

                //日 年4＋月2
                txtPath += "\\" + txtFileName.Substring(intFileLenth + 6, 2);

                //HttpContext.Current.Response.Write(.ToString()+"<br>");

            }

            return txtPath;

        }
        #endregion

        public void Copy(string sourceDirectory, string targetDirectory)
        {
            DirectoryInfo diSource = new DirectoryInfo(sourceDirectory);
            DirectoryInfo diTarget = new DirectoryInfo(targetDirectory);

            CopyAll(diSource, diTarget);//复制文件
        }

        public void CopyAll(DirectoryInfo source, DirectoryInfo target)
        {
            // Check if the target directory exists, if not ,create it

            Console.WriteLine(GetFilePath(target.ToString()));

            if (Directory.Exists(target.FullName) == false)
            {
                Directory.CreateDirectory(target.FullName);
            }

            // Copy each file into its new directory
            foreach (FileInfo fi in source.GetFiles())
            {

                try
                {
                    // 
                    fi.CopyTo(Path.Combine(target.ToString(), fi.Name), true);
                    //Console.WriteLine(fi.Name);
                }
                catch
                {

                }

            }

            // Copy each subdirectory using recursion
            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
            {
                try
                {
                    DirectoryInfo nextTargetSubDir = target.CreateSubdirectory(diSourceSubDir.Name);
                    CopyAll(diSourceSubDir, nextTargetSubDir);
                }
                catch
                {

                }

            }

        }

        //Rar打包
        //source 打包源文件或目录　
        //targetRAR 打包文件名称
        //打包保存目录
        public void CompressRAR(string source, string targetRAR, string SaveDir)
        {


            string the_rar;
            RegistryKey the_Reg;
            object the_Obj;
            string the_Info;
            ProcessStartInfo the_StartInfo;
            Process the_Process;

            try
            {

                the_Reg = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\WinRAR.exe");
                the_Obj = the_Reg.GetValue("");
                the_rar = the_Obj.ToString();

                //Console.WriteLine(the_rar);

                the_Reg.Close();
                //the_rar = the_rar.Substring(1, the_rar.Length - 7);

                //Console.WriteLine(the_rar);

                //Directory.CreateDirectory(source);

                //命令参数
                //the_Info = " a " + rarName + " " + @"C:Test?70821.txt"; //文件压缩
                the_Info = " a " + targetRAR + "  " + source + " -r ";

                Console.WriteLine(the_Info);

                the_StartInfo = new ProcessStartInfo();
                the_StartInfo.FileName = the_rar;
                the_StartInfo.Arguments = the_Info;
                the_StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                //打包文件存放目录
                the_StartInfo.WorkingDirectory = SaveDir;
                the_Process = new Process();
                the_Process.StartInfo = the_StartInfo;

                the_Process.Start();

                the_Process.WaitForExit();
                the_Process.Close();
            }
            catch
            {
            }

        }


        #region 返回当前\\年\\月\\日（整体路径）
        //txtDate 当前日期
        //txtPicName pic文件夹下文件名 如： CertificatePic CompanyLogo CompanyPic OpportunitiesPic Pic_M
        public string YMD_Path(string txtDate)
        {
            string y, m, d;

            y = Convert.ToDateTime(txtDate).Year.ToString();

            //--------月----开始

            m = Convert.ToDateTime(txtDate).Month.ToString();

            if (m.Length == 1) m = "0" + m;

            //--------月----结束

            //--------日----开始

            d = Convert.ToDateTime(txtDate).Day.ToString();

            if (d.Length == 1) d = "0" + d;

            //--------日----结束

            string txtPath = y + "\\" + m + "\\" + d;

            return txtPath;

        }
        #endregion

        #region 创建文件夹 并返回绝对路径
        public string CreateFile(string txtFileName)
        {

            //路径
            string txtPath = "H:\\Test\\M\\HY1\\Hy1\\pic\\CertificatePic" + "\\" + YMD("y");

            //年不存在
            if (!Directory.Exists(txtPath))
            {
                Directory.CreateDirectory(txtPath);
            }

            //月不存在
            txtPath = txtPath + "\\" + YMD("m");
            if (!Directory.Exists(txtPath))
            {
                Directory.CreateDirectory(txtPath);
            }

            //日不存在
            txtPath = txtPath + "\\" + YMD("d");
            if (!Directory.Exists(txtPath))
            {
                Directory.CreateDirectory(txtPath);
            }

            return txtPath;

        }
        #endregion

        #region 返回年月日（单独一个）
        public string YMD(string txtDate)
        {
            txtDate = txtDate.Trim();
            string y, m, d;

            y = System.DateTime.Now.Year.ToString();
            if (txtDate == "y")
                return y;

            //--------月----开始

            m = System.DateTime.Now.Month.ToString();

            if (m.Length == 1) m = "0" + m;

            if (txtDate == "m")
                return m;

            //--------月----结束


            //--------日----开始

            d = System.DateTime.Now.Day.ToString();

            if (d.Length == 1) d = "0" + d;

            if (txtDate == "d")
                return d;

            //--------日----结束

            return "";

        }
        #endregion
    }
}
