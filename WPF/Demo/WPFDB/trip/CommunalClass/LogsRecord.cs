using System;
using System.IO;
using System.Text;
using System.Threading;
namespace trip.CommunalClass
{
	/// <summary>
	/// 日志记录
	/// </summary>
	public class LogsRecord
	{
		private static StreamWriter loseStreamWriter;
		private static string loseLogName = null;
		private static StreamWriter mySw;
		private AutoResetEvent IOARE = new AutoResetEvent(true);
		private StreamWriter writer;
		private Thread tr;
		private bool isRunning = true;
		private static string nowStreamName
		{
			get;
			set;
		}
		/// <summary>
		/// 写日志
		/// </summary>
		/// <param name="logName">日志名</param>
		/// <param name="content">内容</param>
		/// <param name="FileNameByDay">是否每天产生一个目录</param>
		public static void write(string logName, string content, bool FileNameByDay)
		{
			string arg_34_0;
			DateTime now;
			if (!FileNameByDay)
			{
				arg_34_0 = logName + ".txt";
			}
			else
			{
				now = DateTime.Now;
				arg_34_0 = now.ToString("dd-HH") + " " + logName + ".txt";
			}
			string fileName = arg_34_0;
			string logPath = AppDomain.CurrentDomain.BaseDirectory + "log";
			if (!Directory.Exists(logPath))
			{
				Directory.CreateDirectory(logPath);
			}
			string arg_73_0 = logPath;
			string arg_73_1 = "\\";
			now = DateTime.Now;
			logPath = arg_73_0 + arg_73_1 + now.ToString("yyyyMM");
			if (!Directory.Exists(logPath))
			{
				Directory.CreateDirectory(logPath);
			}
			LogsRecord.writeLog(logPath, fileName, content);
		}
		/// <summary>
		/// 写日志
		/// </summary>
		/// <param name="logName">日志名</param>
		/// <param name="content">内容</param>
		public static void write(string logName, string content)
		{
			LogsRecord.write(logName, content, true);
		}
		/// <summary>
		/// 写日志
		/// </summary>
		/// <param name="logName">日志名</param>
		/// <param name="content">内容</param>
		/// <param name="LogsPath">指定特定日志目录</param>
		public static void write(string logName, string content, string LogsPath)
		{
			DateTime now = DateTime.Now;
			string fileName = now.ToString("dd-HH") + " " + logName + ".txt";
			if (!Directory.Exists(LogsPath))
			{
				throw new Exception("Path [" + LogsPath + "] not exist!");
			}
			LogsPath = LogsPath.TrimEnd(new char[]
			{
				'\\'
			});
			string arg_71_0 = LogsPath;
			string arg_71_1 = "\\";
			now = DateTime.Now;
			LogsPath = arg_71_0 + arg_71_1 + now.ToString("yyyyMM");
			if (!Directory.Exists(LogsPath))
			{
				Directory.CreateDirectory(LogsPath);
			}
			LogsRecord.writeLog(LogsPath, fileName, content);
		}
		private static void writeLog(string path, string fileName, string content)
		{
			string fullPath = path + "\\" + fileName;
			string info = string.Concat(new string[]
			{
				"[", 
				DateTime.Now.ToString("MM-dd HH:mm:ss"), 
				"] ", 
				content, 
				"\r\n"
			});
			try
			{
				if (fullPath != LogsRecord.nowStreamName)
				{
					if (LogsRecord.nowStreamName != null)
					{
						LogsRecord.closeLogFile();
					}
					LogsRecord.mySw = new StreamWriter(fullPath, true, Encoding.UTF8);
					LogsRecord.nowStreamName = fullPath;
				}
				LogsRecord.mySw.Write(info);
				LogsRecord.mySw.Flush();
			}
			catch
			{
				LogsRecord.writeLostLog(path, content);
			}
		}
		private static void writeLostLog(string path, string content)
		{
			try
			{
				string arg_1F_1 = "\\";
				DateTime now = DateTime.Now;
				string fileName = path + arg_1F_1 + now.ToString("dd-HH") + "_lostLog.txt";
				if (fileName != LogsRecord.loseLogName)
				{
					LogsRecord.closeLoseLogFile();
					LogsRecord.loseStreamWriter = new StreamWriter(fileName, true, Encoding.Default);
				}
				LogsRecord.loseLogName = fileName;
				string[] array = new string[5];
				array[0] = "[";
				string[] arg_7C_0 = array;
				int arg_7C_1 = 1;
				now = DateTime.Now;
				arg_7C_0[arg_7C_1] = now.ToString("MM-dd HH:mm:ss");
				array[2] = "] ";
				array[3] = content;
				array[4] = "\r\n";
				string info = string.Concat(array);
				LogsRecord.loseStreamWriter.Write(info);
				LogsRecord.loseStreamWriter.Flush();
			}
			catch
			{
			}
		}
		/// <summary>
		/// 关闭丢失日志记录文件流
		/// </summary>
		public static void closeLoseLogFile()
		{
			if (LogsRecord.loseStreamWriter != null)
			{
				LogsRecord.loseLogName = null;
				LogsRecord.loseStreamWriter.Close();
				LogsRecord.loseStreamWriter.Dispose();
				LogsRecord.loseStreamWriter = null;
			}
		}
		/// <summary>
		/// 关闭最后打开的日志文件流
		/// </summary>
		public static void closeLogFile()
		{
			if (LogsRecord.mySw != null)
			{
				LogsRecord.nowStreamName = null;
				LogsRecord.mySw.Close();
				LogsRecord.mySw.Dispose();
				LogsRecord.mySw = null;
			}
		}
		/// <summary>
		/// 日志记录类
		/// </summary>
		/// <param name="logName">日志名</param>
		public LogsRecord(string logName)
		{
			DateTime now = DateTime.Now;
			string fileName = now.ToString("dd-HH") + " " + logName + ".txt";
			string logPath = AppDomain.CurrentDomain.BaseDirectory + "log";
			if (!Directory.Exists(logPath))
			{
				Directory.CreateDirectory(logPath);
			}
			string arg_7C_0 = logPath;
			string arg_7C_1 = "\\";
			now = DateTime.Now;
			logPath = arg_7C_0 + arg_7C_1 + now.ToString("yyyyMM");
			if (!Directory.Exists(logPath))
			{
				Directory.CreateDirectory(logPath);
			}
			try
			{
				this.writer = new StreamWriter(logPath + "\\" + fileName);
			}
			catch
			{
				throw new Exception("This logname is opened by other process.");
			}
			if (this.tr == null)
			{
				this.tr = new Thread(new ThreadStart(this.flushThread));
				this.tr.Start();
			}
		}
		/// <summary>
		/// 写日志(非静态)
		/// </summary>
		/// <param name="content">内容</param>
		public void write(string content)
		{
			string info = "[" + DateTime.Now.ToString("MM-dd HH:mm:ss") + "] " + content;
			this.IOARE.WaitOne(10000);
			this.writer.WriteLine(info);
			this.IOARE.Set();
		}
		private void flushThread()
		{
			while (this.isRunning)
			{
				if (this.writer != null)
				{
					this.IOARE.WaitOne(10000);
					this.writer.Flush();
					this.IOARE.Set();
				}
				Thread.Sleep(2000);
			}
		}
		/// <summary>
		/// 关闭当前日志流
		/// </summary>
		public void close()
		{
			this.isRunning = false;
			if (this.writer != null)
			{
				this.writer.Close();
				this.writer.Dispose();
			}
		}
	}
}
