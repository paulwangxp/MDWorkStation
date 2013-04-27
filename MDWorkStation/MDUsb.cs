using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace MDWorkStation
{
    public static class MDUsbPos
    {
        private static string[] driverName;//驱动名
        private static int[] pos;//当前位置 1-16
        private static bool[] isConnected;//是否已经连接上电脑
        private static bool[] isCompated;
        private static int size = 16;//当前支持多少个设备

        static MDUsbPos()//静态构造函数，只被调用一次
        {
            driverName = new string[size];
            pos = new int[size];
            isConnected = new bool[size];
            isCompated = new bool[size];

            for (int i = 0; i < 16; i++)
            {
                driverName[i] = "";
                pos[i] = 0;
                isConnected[i] = false;
                isCompated[i] = false;
            }
        }


        public static int getUsbPos(string driverName1)
        {
            //先在数组中查找是否有这个驱动名，并且要isConnect = true
            int i = -1;
            foreach (string name in driverName)
            {
                i++;

                if (name.Contains(driverName1))
                {
                    if (isConnected[i])
                        return pos[i];
                    else
                        break;
                }

            }


            //如果没有，那就分配一个位置
            for (int j = 0; j < size; j++)
            {
                if (!isConnected[j])
                {
                    isConnected[j] = true;
                    driverName[j] = driverName1;
                    pos[j] = j + 1;//位置从1开始
                    return pos[j];
                }
            }

            return -1;
        }

        //磁盘拔下时必须调用，如果查找不到此盘符此返回false
        public static bool setDisconnect(string driverName1)
        {

            int i = -1;
            foreach (string name in driverName)
            {
                i++;

                if (name.Contains(driverName1))
                {
                    driverName[i] = "";
                    pos[i] = 0;
                    isConnected[i] = false;
                    isCompated[i] = false;

                    return true;
                }

            }

            return false;
        }

        

        public static bool isConnect(string driverName1)
        {

            int i = -1;
            foreach (string name in driverName)
            {
                i++;

                if (name.Contains(driverName1))
                {
                    return isConnected[i];
                }

            }

            return false;
        }

        public static bool isComplate(string driverName1)
        {

            int i = -1;
            foreach (string name in driverName)
            {
                i++;

                if (name.Contains(driverName1))
                {
                    return isCompated[i];
                }

            }

            return false;
        }

        public static void setComplated(string driverName1)
        {
            int i = -1;
            foreach (string name in driverName)
            {
                i++;

                if (name.Contains(driverName1))
                {
                    isCompated[i] = true;
                    return;

                }

            }


        }
    }
     
    public class MDUsb
    {

        

        public string  driverName/*磁盘驱动器盘符*/;
        public string driverLable/*磁盘卷标*/;
        public string driverType/*"磁盘类型：")*/;
        public string driverFormat/*磁盘格式*/;
        public string TotalSize/*磁盘大小*/;
        public string AvailableFreeSpace/*剩余空间*/;
        public string TotalFreeSpace/*总剩余空间（含磁盘配额）*/;
        public int freePercent/*剩余空间（百分比）*/;

        public long usbHubNuber/*对应的usb hub的顺序*/;
        public string diviceID/*设备编号*/;


        private List<string> m_FileList = new List<string>();


        public MDUsb()
        {

        }

        public string getDiskInfoStrirng()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("盘符：");
            sb.AppendLine(this.driverName);
            //sb.Append("磁盘卷标：");
            //sb.AppendLine(this.driverLable);
            //sb.Append("磁盘类型：");
            //sb.AppendLine(this.driverType);
            //sb.Append("磁盘格式：");
            //sb.AppendLine(this.driverFormat);
            //sb.Append("磁盘大小：");
            //sb.AppendLine(this.TotalSize);
            //sb.Append("剩余空间：");
            //sb.AppendLine(this.AvailableFreeSpace);
            //sb.Append("总剩余空间（含磁盘配额）：");
            //sb.AppendLine(this.TotalFreeSpace);
            sb.Append(" 剩余空间（百分比）：");
            this.freePercent = (int)(long.Parse(AvailableFreeSpace) * 100 / long.Parse(TotalSize));
            sb.AppendLine(freePercent.ToString() + " %");

            return sb.ToString();
        }

        public string getDeviceID()
        {
            if (m_FileList.Count > 0)
                return getDeviceIDFromFile(m_FileList[0]);

            return "******";

        }

        public string getPoliceID()
        {
            if (m_FileList.Count > 0)
                return getPoliceIDFromFile(m_FileList[0]);

            return "******";
        
        }


        public string getDataTime()
        {
            if (m_FileList.Count > 0)
                return getDataTimeFromFile(m_FileList[0]);

            return "00000000000000";
        }


        public static string getDeviceIDFromFile(string sFileName)
        {
            string sName = sFileName.Substring(sFileName.LastIndexOf("\\") + 1);
            //return sName.Substring(0, sName.Length - 18);//去掉后面14位日期的就是编号
            if (sName.Length < 26)//老文件格式
                return "******";//A000000

            return sName.Substring(0, sName.IndexOf("_"));//A000000_99999920120101231212.mp4
        }

        public static string getPoliceIDFromFile(string sFileName)
        {
            string sName = sFileName.Substring(sFileName.LastIndexOf("\\")+1);
            //return sName.Substring(0, sName.Length - 18);//去掉后面14位日期的就是编号
            if (sName.Length < 26)//老文件格式
                return sName.Substring(0, 6);//机器编号一定是6位的 99999920120101231212.mp4

            //return sName.Substring(8, 6);//A000000_99999920120101231212.mp4
            return sName.Substring(sName.IndexOf("_")+1, 6);//A000000_99999920120101231212.mp4
        }

        public static string getDataTimeFromFile(string sFileName)
        {
            string sName = sFileName.Substring(sFileName.LastIndexOf("\\") + 1);
            if (sName.Length < 26)//老文件格式
                return sName.Substring(6, 14);//去掉前面6位的编号，取后面14位

            return sName.Substring(sName.IndexOf("_") + 1 + 6, 14);//去掉前面6位的编号，取后面14位
        }

        public string[] getFileList()
        {
            return m_FileList.ToArray(); 
        }

        public int calcFileCount()
        {
            m_FileList.AddRange(Directory.GetFiles(driverName, "*.wav", SearchOption.AllDirectories));

            m_FileList.AddRange(Directory.GetFiles(driverName, "*.mp4", SearchOption.AllDirectories));

            m_FileList.AddRange(Directory.GetFiles(driverName, "*.jpg", SearchOption.AllDirectories));

            m_FileList.AddRange(Directory.GetFiles(driverName, "*.avi", SearchOption.AllDirectories));

            return m_FileList.Count;
        }

        public int getFileCount()
        {

            return m_FileList.Count;
        }

    }
}
