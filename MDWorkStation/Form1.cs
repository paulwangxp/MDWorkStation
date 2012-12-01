using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using System.IO;

namespace MDWorkStation
{
    public partial class Form1 : Form
    {

        Dictionary<string, MDUsb> usbDiskDic = new Dictionary<string, MDUsb> ();//USB对象队列，结束一个pop up一个

        bool m_hasDriver = false;//是否是有驱动版本
        string m_interfaceStr = "";//上传到服务器的接口地址
        string m_DataPathStr = "";//随时录数据保存路径

        public Form1()
        {
            this.WindowState = FormWindowState.Maximized;

            InitializeComponent();

            InitControlPos();//设置各控件的位置，用于不同分辨率的情况

            StartIdle();//进入自动上传循环中

            //string []drivers = getUsbDeviceName();//获得usb的盘符

            //initDevice();
        }

       


        private void InitControlPos()
        {
            //label1.Parent = pictureBox1;
            //label1.BringToFront();
            foreach (Control tbox in this.Controls)
            {
                if (tbox is Label)
                {
                    tbox.Visible = false;
                }
                else if (tbox is PictureBox && tbox.Name.Contains("pictureBox"))
                {
                    tbox.BackgroundImage = MDWorkStation.Properties.Resources.b2;
                }
            }
            writeMsg("系统初始化成功(Init)...");

            //设置开机启动注册表
            RunWhenStart(true, "MDWorkStation", Application.ExecutablePath);
        }

        private void StartIdle()
        {
 
            //1.读取当前程序配置
            writeMsg("读取配置...");
            INIFile iniObject = new INIFile();
            m_hasDriver = bool.Parse(iniObject.IniReadValue("config", "Driver"));
            m_interfaceStr = iniObject.IniReadValue("config", "Interface");
            m_DataPathStr = iniObject.IniReadValue("config", "Path");
            //2.开启windows消息处理，查找U盘变化
            //发现有设备插入，读取盘符，压入队列
            
            //3.启动定时器，读取队列磁盘
            timer_usbDiskCopy.Interval = 1000;
            timer_usbDiskCopy.Enabled = true;
            

        }


        private void writeMsg(string msg)
        {
            LogManager.WriteLog(msg);
            listBox1.Items.Add(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss: ") + msg);
            listBox1.SelectedIndex = listBox1.Items.Count - 1;
 
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            writeMsg("您选择了退出系统(exit)...");
            this.Close();
        }

        //U盘定时拷贝Timer
        private void timer_usbDiskCopy_Tick(object sender, EventArgs e)
        {
            timer_usbDiskCopy.Enabled = false;


            try
            {
                if (usbDiskDic.Count > 0)
                {
                    foreach (MDUsb usbItem in usbDiskDic.Values)//遍历整个usb队列
                    {
                        string sDir = "\\Data\\" + DateTime.Now.Year.ToString() + "\\" + 
                            DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString();
                        Directory.CreateDirectory(System.Environment.CurrentDirectory + sDir);//创建文件夹，\Data\2012\201211\
                    }
                    

                }
            }
            catch (Exception ex)
            {
                ;
            }





            timer_usbDiskCopy.Enabled = true;

        }

        
    }


}
