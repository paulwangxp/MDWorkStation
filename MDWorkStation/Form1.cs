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
        bool bFirstRun = true;

        bool m_hasDriver = false;//是否是有驱动版本
        string m_interfaceStr = "";//上传到服务器的接口地址
        string m_DataPathStr = "";//随时录数据保存路径
        string m_ftpSever = "";
        string m_ftpPort = "";
        string m_ftpUser = "";
        string m_ftpPwd = "";

        public Form1()
        {
            this.WindowState = FormWindowState.Maximized;

            InitializeComponent();

            InitControlPos();//设置各控件的位置，用于不同分辨率的情况

            //StartIdle();//进入自动上传循环中

            //string []drivers = getUsbDeviceName();//获得usb的盘符

            //initDevice();
        }

       


        private void InitControlPos()
        {
            //label1.Parent = pictureBox1;
            //label1.BringToFront();
            foreach (Control tbox in this.Controls )
            {
                if (tbox is Label && tbox.Name.Contains("label"))
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
            bFirstRun = false;
 
            //1.读取当前程序配置
            writeMsg("读取配置...");
            INIFile iniObject = new INIFile();
            m_hasDriver = bool.Parse(iniObject.IniReadValue("config", "Driver", "False"));
            m_interfaceStr = iniObject.IniReadValue("config", "UploadInterface", "http://127.0.0.1/");
            m_DataPathStr = iniObject.IniReadValue("config", "Path","\\Data");
            m_ftpSever = iniObject.IniReadValue("config", "FtpSever", "127.0.0.1");
            m_ftpPort = iniObject.IniReadValue("config", "FtpPort", "21");
            m_ftpUser = iniObject.IniReadValue("config", "FtpUser", "test1");
            m_ftpPwd = iniObject.IniReadValue("config", "FtpPwd", "test1");
            //2.开启windows消息处理，查找U盘变化
            //发现有设备插入，读取盘符，压入队列
            

            //3.启动定时器，读取队列磁盘
            timer_usbDiskCopy.Interval = 1000;
            timer_usbDiskCopy.Enabled = true;
            

        }


        private void writeMsg(string msg)
        {
            LogManager.WriteLog(msg);

            if (listBox1.Items.Count > 100)
                listBox1.Items.Clear();

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

            string sDir = System.Environment.CurrentDirectory +  "\\Data\\" + DateTime.Now.Year.ToString() + "\\" +
                            DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString();
            if(!Directory.Exists(sDir))//如果文件夹不存在
                Directory.CreateDirectory(sDir);//创建文件夹，\Data\2012\201211\

           


            try
            {
                if (usbDiskDic.Count > 0)
                {
                    foreach (MDUsb usbItem in usbDiskDic.Values)//遍历整个usb队列
                    {
                        if (!usbItem.isConnect(usbItem.driverName))//如果数据已经拷贝完成，就不要再查看了
                            break;

                        int uploadSuccessFileNum = 1;
                        foreach (string fileName in usbItem.getFileList())
                        {
                            LogManager.WriteLog(usbItem.driverName + " 盘 开始数据拷贝： ");

                            //从当前U盘获取一个文件

                            //得到文件大小，并且判断当前拷贝磁盘是否够用
                            FileInfo fileInfo = new FileInfo(fileName);

                            //不够用，提示清理文件
                            if (fileInfo.Length > getDiskFreeSpace(System.Environment.CurrentDirectory.Substring(0,2)) )
                            {
                                writeMsg("错误，磁盘空间不足，停止拷贝！");
                                timer_usbDiskCopy.Enabled = true;
                                return;
                            }

                            

                            //够用，拷贝
                            LogManager.WriteLog("正在拷贝数据： (" + uploadSuccessFileNum.ToString() + ")" + fileName);
                            File.Copy(fileName, sDir + fileName.Substring(fileName.LastIndexOf("\\")),true);


                            //通过FTP上传至平台服务器

                            //调用平台上传接口

                            //接口调用成功，删除U盘文件
                            LogManager.WriteLog("正在删除数据： " + fileName);
                            File.Delete(fileName);

                            //根据Name获得对应的控件对象
                            int pos = usbItem.getUsbPos();
                            string sCount = "label_count_" + pos.ToString();                            
                            System.Reflection.FieldInfo label2 = this.GetType().GetField(sCount);//反射
                            Label lableCount = (Label)label2.GetValue(this);
                            lableCount.Text = uploadSuccessFileNum.ToString() + " / " + usbItem.getFileList().Length;
                            lableCount.Refresh();

                            if (uploadSuccessFileNum >= usbItem.getFileList().Length)
                            {
                                string sPic = "pictureBox" + pos.ToString();
                                System.Reflection.FieldInfo pic1 = this.GetType().GetField(sPic);//反射
                                PictureBox pic = (PictureBox)pic1.GetValue(this);
                                pic.BackgroundImage = MDWorkStation.Properties.Resources.b1;//绿色，代表已经上传完成
                                writeMsg(usbItem.driverName + " 盘 数据拷贝完成");
                                writeMsg(usbItem.getPoliceID() + " 编号 数据拷贝完成");
                                usbItem.setDisconnect(usbItem.driverName);//数据完成了，所以不应该再使用它
                                break;
                            }

                            uploadSuccessFileNum++;

                            //调用不成功，提示错误，结束
                        }
                    }
                    

                }
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(ex.Message);
            }





            timer_usbDiskCopy.Enabled = true;

        }

        //进入关机等功能
        private void imageButton_Setup_Click(object sender, EventArgs e)
        {
            FormPassWord form2 = new FormPassWord();
            DialogResult result = form2.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.Cancel)
                return;




            ShutDownForm form1 = new ShutDownForm();
            form1.ShowDialog();
        }

        
    }


}
