using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace MDWorkStation
{
    public partial class Form1 : Form
    {
        
        Queue <MDUsb>usbQueue = new Queue<MDUsb>();//USB对象队列，结束一个pop up一个

        public Form1()
        {
            this.WindowState = FormWindowState.Maximized;

            InitializeComponent();

            InitControlPos();//设置各控件的位置，用于不同分辨率的情况

            StartIdle();//进入自动上传循环中

            string []drivers = getUsbDeviceName();//获得usb的盘符

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
            //2.开启定时器，查找U盘变化
            writeMsg("系统等待中...");
            //发现有设备插入，读取盘符，压入队列
            writeMsg("G 盘插入，开始自动上传...");
            //根据USB端口顺序，设置插入设备为蓝色
            //进行队列U盘的批量文件上传工作
            writeMsg("G 盘插自动上传结束...");
            

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

        public static void RunWhenStart(bool Started, string name, string path)
        {
            return;
            RegistryKey HKLM = Registry.LocalMachine;
            RegistryKey Run = HKLM.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run");
            if (Started == true)
            {
                try
                {
                    Run.SetValue(name, path);
                    HKLM.Close();
                }
                catch
                {

                }
            }
            else
            {
                try
                {
                    Run.DeleteValue(name);
                    HKLM.Close();
                }
                catch
                {

                }
            }
        }
    }


}
