using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;
using VideoEncoder;

namespace MDWorkStation
{
    //ffmpeg.exe控制命令的包装，通过调用cmd将输出结果使用出来
    class FFMpegUtility
    {
        public static long getMediaDruation(string mediaName)//方法不使用
        {

            long m_Duration = 0;
            string bitrate = "";

            try
            {

                ProcessStartInfo start = new ProcessStartInfo(System.Environment.CurrentDirectory + "\\ffmpeg.exe");//设置运行的命令行文件问ping.exe文件，这个文件系统会自己找到
                //如果是其它exe文件，则有可能需要指定详细路径，如运行winRar.exe
                start.Arguments = " - i " + mediaName;//设置命令参数
                start.CreateNoWindow = true;//不显示dos命令行窗口
                start.RedirectStandardOutput = true;//
                start.RedirectStandardError = true;//
                start.UseShellExecute = false;//是否指定操作系统外壳进程启动程序
                Process p = Process.Start(start);
                StreamReader reader = p.StandardError;//截取输出流
                string line = reader.ReadLine();//每次读取一行
                while (!reader.EndOfStream)
                {
                    line = reader.ReadLine();
                    if (line.Contains("Duration"))
                    {
                        int pos1 = line.IndexOf(":");
                        int pos2 = line.IndexOf(",");

                        line.Substring(pos1 + 1, pos2 - pos1);
                        break;
                    }
                }
                p.WaitForExit();//等待程序执行完退出进程
                p.Close();//关闭进程
                reader.Close();//关闭流
            }
            catch( Exception)
            {
            }


            return m_Duration;
        }


        public static string getMediaInfo(string mediaName)
        {
            /**
              * 支持视频格式：mpeg，mpg，avi，dat，mkv，rmvb，rm，mov.
              *不支持：wmv
              * **/

            VideoEncoder.Encoder enc = new VideoEncoder.Encoder();
            //ffmpeg.exe的路径，程序会在执行目录（....FFmpeg测试\bin\Debug）下找此文件，
            enc.FFmpegPath = System.Environment.CurrentDirectory + "\\ffmpeg.exe";
            //视频路径
            VideoFile videoFile = new VideoFile(mediaName);

            enc.GetVideoInfo(videoFile);

            TimeSpan totaotp = videoFile.Duration;
            string totalTime = string.Format("{0:00}:{1:00}:{2:00}", (int)totaotp.TotalHours, totaotp.Minutes, totaotp.Seconds);

            Console.WriteLine("时间长度：{0}", totalTime);
            Console.WriteLine("高度：{0}", videoFile.Height);
            Console.WriteLine("宽度：{0}", videoFile.Width);
            Console.WriteLine("数据速率：{0}", videoFile.VideoBitRate);
            Console.WriteLine("数据格式：{0}", videoFile.VideoFormat);
            Console.WriteLine("比特率：{0}", videoFile.BitRate);
            Console.WriteLine("文件路径：{0}", videoFile.Path);

            return totalTime;
 
        }

        public static string getMediaPlayTime(string mediaName)
        {
            /**
              * 支持视频格式：mpeg，mpg，avi，dat，mkv，rmvb，rm，mov.
              *不支持：wmv
              * **/

            VideoEncoder.Encoder enc = new VideoEncoder.Encoder();
            //ffmpeg.exe的路径，程序会在执行目录（....FFmpeg测试\bin\Debug）下找此文件，
            enc.FFmpegPath = System.Environment.CurrentDirectory + "\\ffmpeg.exe";
            //视频路径
            VideoFile videoFile = new VideoFile(mediaName);

            enc.GetVideoInfo(videoFile);

            TimeSpan totaotp = videoFile.Duration;
            string totalTime = string.Format("{0:00}:{1:00}:{2:00}", (int)totaotp.TotalHours, totaotp.Minutes, totaotp.Seconds);

            //Console.WriteLine("时间长度：{0}", totalTime);
            //Console.WriteLine("高度：{0}", videoFile.Height);
            //Console.WriteLine("宽度：{0}", videoFile.Width);
            //Console.WriteLine("数据速率：{0}", videoFile.VideoBitRate);
            //Console.WriteLine("数据格式：{0}", videoFile.VideoFormat);
            //Console.WriteLine("比特率：{0}", videoFile.BitRate);
            //Console.WriteLine("文件路径：{0}", videoFile.Path);

            return totalTime;

        }
    }
}
