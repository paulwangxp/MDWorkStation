﻿using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Drawing;

namespace MDWorkStation
{
    public class LogManager
    {
        private static string logPath = string.Empty;
        /// <summary>
        /// 保存日志的文件夹
        /// </summary>
        public static string LogPath
        {
            get
            {
                if (logPath == string.Empty)
                {
                    logPath = Path.GetDirectoryName(Application.ExecutablePath) + @"\Log\";
                }
                return logPath;
            }
            set { logPath = value; }
        }

        private static string logFielPrefix = string.Empty;
        /// <summary>
        /// 日志文件前缀
        /// </summary>
        public static string LogFielPrefix
        {
            get { return logFielPrefix; }
            set { logFielPrefix = value; }
        }

        /// <summary>
        /// 写日志
        /// </summary>
        public static void WriteLog(string logFile, string msg)
        {
            try
            {
                if (!Directory.Exists(LogPath))
                {
                    DirectoryInfo Dir = Directory.CreateDirectory(LogPath);
                }
                string filename = LogPath + LogFielPrefix + logFile + " " + DateTime.Now.ToString("yyyyMMdd") + ".Log";
                
                System.IO.StreamWriter sw = File.AppendText(filename);
                sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss: ") + msg);
                sw.Close();
            }
            catch
            { }
        }

        /// <summary>
        /// 写日志
        /// </summary>
        public static void WriteLog(LogFile logFile, string msg)
        {
            WriteLog(logFile.ToString(), msg);
        }

        public static void WriteLog(string msg)
        {
            WriteLog(LogFile.Trace.ToString(), msg);
        }

        public static void WriteErrorLog(string msg)
        {
            WriteLog(LogFile.Error.ToString(), msg);
        }


        public static void showErrorMsg(string msg)
        {
            WriteLog(msg);
            MessageBox.Show( msg,"错误",MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        

        //返回日期型字符串
        public static string getTimeString()
        {
            DateTime dt = DateTime.Now;
            return string.Format("{0:yyyyMMddHHmmss_ffff}", dt);
        }

        //根据文件后缀名，返回日期型文件名，返回 201212032110449999_.jpg
        public static string getTimeStringFileExtension(string fileName)
        {
            return LogManager.getTimeString() + Path.GetExtension(fileName);
        }
    }

    /// <summary>
    /// 日志类型
    /// </summary>
    public enum LogFile
    {
        Trace,
        Warning,
        Error,
        SQL
    }
}



