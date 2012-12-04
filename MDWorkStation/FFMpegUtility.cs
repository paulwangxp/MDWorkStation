using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;

namespace MDWorkStation
{
    //ffmpeg.exe控制命令的包装，通过调用cmd将输出结果使用出来
    class FFMpegUtility
    {
        public static long getMediaDruation(string mediaName)
        {

            long m_Duration = 0;
            string bitrate = "";


            ProcessStartInfo start = new ProcessStartInfo(System.Environment.CurrentDirectory + "\\ffmpeg.exe");//设置运行的命令行文件问ping.exe文件，这个文件系统会自己找到
            //如果是其它exe文件，则有可能需要指定详细路径，如运行winRar.exe
            start.Arguments = " - i " + mediaName;//设置命令参数
            start.CreateNoWindow = true;//不显示dos命令行窗口
            start.RedirectStandardOutput = true;//
            start.RedirectStandardInput = true;//
            start.UseShellExecute = false;//是否指定操作系统外壳进程启动程序
            Process p = Process.Start(start);
            StreamReader reader = p.StandardOutput;//截取输出流
            string line = reader.ReadLine();//每次读取一行
            while (!reader.EndOfStream)
            {
                line = reader.ReadLine();
                if (line.Contains("Duration"))
                {
                    int pos1 = line.IndexOf(":");
                    int pos2 = line.IndexOf(",");

                    line.Substring(pos1+1,pos2-pos1);
                    break;
                }
            }
            p.WaitForExit();//等待程序执行完退出进程
            p.Close();//关闭进程
            reader.Close();//关闭流


            return m_Duration;
        }
    }
}
