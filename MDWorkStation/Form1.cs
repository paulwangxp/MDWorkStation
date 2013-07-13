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
using System.Threading;
using MDWorkStation.LightFTP;

namespace MDWorkStation
{
    public partial class Form1 : Form
    {

        Dictionary<string, MDUsb> usbDiskDic = new Dictionary<string, MDUsb>();//USB对象队列，结束一个pop up一个
        List<string> FtpList = new List<string>();//FTP待上传的队列

        Thread workThread, uploadThread;
        bool threadRunFlag = true;

        bool m_topMost = false;//界面是否在最上层
        bool m_hasDriver = false;//是否是有驱动版本
        string m_DVPwd = "";//记录仪有驱动版本的密码
        static string m_DataPathStr = "";//随时录数据保存路径

        bool m_UploadFlag = false;//是否要上传至平台
        string m_interfaceStr = "";//上传到服务器的接口地址
        string m_ftpSever = "";
        string m_ftpPort = "";
        string m_ftpUser = "";
        string m_ftpPwd = "";
        int m_ftpBuffer = 512;//ftp buffer size
        string m_WorkStationID = "";
        string m_AppCode = "njmd84588111";//接口调用的appCode，防止人们直接访问接口

        public static void setSavePath(string path)
        {
            m_DataPathStr = path;
        }



        public Form1()
        {
            //this.WindowState = FormWindowState.Maximized;//全屏
            this.TopMost = false;//界面是否永远在最上层

            InitializeComponent();
           
            InitControlPos();//设置各控件的位置，用于不同分辨率的情况

            readConfig();//读取配置文件

            //string []drivers = getUsbDeviceName();//获得usb的盘符

            //initDevice();

            CheckForIllegalCrossThreadCalls = false;

            //ShutDownForm f1 = new ShutDownForm();
            //f1.Show();




            workThread = new Thread(new ThreadStart(allOfWork));
            workThread.Start();

            if (m_UploadFlag)//需要上传到服务器
            {
                uploadThread = new Thread(new ThreadStart(UploadWork));
                uploadThread.Start();
            }

            
        }




        private void InitControlPos()
        {
            //label1.Parent = pictureBox1;
            //label1.BringToFront();

            //listBox1.Visible = false;
            foreach (Control tbox in this.Controls)
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
            System.Environment.CurrentDirectory = Path.GetDirectoryName(Application.ExecutablePath);
        }

        private void readConfig()
        {
            //1.读取当前程序配置
            writeMsg("读取配置...");
            INIFile iniObject = new INIFile();

            m_topMost = iniObject.IniReadValue("config", "TopMost", "0") == "0" ? false : true;
            m_hasDriver = iniObject.IniReadValue("config", "Driver", "0") == "0" ? false : true;
            m_DVPwd = iniObject.IniReadValue("config", "LoginPwd", "\\Data");//DV的登录密码
            m_DataPathStr = iniObject.IniReadValue("config", "SavePath", "\\Data");//目录不允许设置保存路径

            m_UploadFlag = iniObject.IniReadValue("config", "UploadFlag", "0") == "0" ? false : true;
            m_interfaceStr = iniObject.IniReadValue("config", "UploadInterface", "http://127.0.0.1//interfaceAction.do");
            m_ftpSever = iniObject.IniReadValue("config", "FtpSever", "127.0.0.1");
            m_ftpPort = iniObject.IniReadValue("config", "FtpPort", "21");
            m_ftpUser = iniObject.IniReadValue("config", "FtpUser", "test1");
            m_ftpPwd = iniObject.IniReadValue("config", "FtpPwd", "test1");
            m_ftpBuffer = int.Parse(iniObject.IniReadValue("config", "FtpBuffer", "512"));
            m_WorkStationID = iniObject.IniReadValue("config", "MachineID", "778899");

            //FtpClient ftpClient = new FtpClient(m_ftpSever, m_ftpUser, m_ftpPwd, 120, int.Parse(m_ftpPort));
            //ftpClient.Login();
            //ftpClient.ChangeDir("/test");
            //ftpClient.ChangeDir("/test1");
            //ftpClient.ChangeDir("/test");
            //ftpClient.ChangeDir("/test");
            //ftpClient.MakeDir("test1");
            //ftpClient.MakeDir("test");

            if (m_topMost)
                this.TopMost = true;

            if (m_hasDriver)
                timer_IntoUSBMode.Enabled = true;



        }

        private void writeDebugMsg(string msg)
        {
            writeMsg(msg);
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
            

            FormPassWord form2 = new FormPassWord();
            DialogResult result = form2.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.Cancel)
                return;


            writeMsg("您选择了退出系统(exit)...");
            threadRunFlag = false;
            this.Close();
        }

        private void allOfWork()
        {





            while (threadRunFlag)
            {
                Thread.Sleep(1000);
                string sDir = "";

                if (!Directory.Exists(m_DataPathStr))//如果设定的目录不存在,就使用当前程序工作路径进行保存
                {

                    sDir = System.Environment.CurrentDirectory + "\\Data\\" + DateTime.Now.ToString("yyyy") + "\\" +
                                DateTime.Now.ToString("yyyyMM") + "\\"
                                + DateTime.Today.Date.ToString("yyyyMMdd");
                }
                else {
                    sDir = m_DataPathStr + "\\" + DateTime.Now.ToString("yyyy") + "\\" +
                            DateTime.Now.ToString("yyyyMM") + "\\"
                            + DateTime.Today.Date.ToString("yyyyMMdd");

                }

                

                //writeMsg(sDir);



                if (!Directory.Exists(sDir))//如果文件夹不存在
                    Directory.CreateDirectory(sDir);//创建文件夹，\Data\2012\201211\20121101



                try
                {
                    if (usbDiskDic.Count >= 0)
                    {




                        foreach (MDUsb usbItem in usbDiskDic.Values)//遍历整个usb队列
                        {
                            if (MDUsbPos.isComplate(usbItem.driverName) || !threadRunFlag)//如果数据已经拷贝完成，就不要再查看了
                            {
                                continue;
                            }

                            int uploadSuccessFileNum = 1;



                            //从当前U盘获取一个文件
                            foreach (string usbFileName in usbItem.getFileList())
                            {
                                LogManager.WriteLog(usbItem.driverName + " 盘 开始数据拷贝： ");

                                //string f1 = FFMpegUtility.getMediaInfo(@"h:\99999920121203091619.mp4");//获取文件信息


                                //得到文件大小，并且判断当前拷贝磁盘是否够用
                                FileInfo fileInfo = new FileInfo(usbFileName);

                                //不够用，提示清理文件
                                if (fileInfo.Length > getDiskFreeSpace(System.Environment.CurrentDirectory.Substring(0, 2)))
                                {
                                    writeMsg("错误，磁盘空间不足，停止拷贝！");
                                    Thread.Sleep(1000 * 60 * 10);//停止10分钟
                                    //timer_usbDiskCopy.Enabled = true;

                                    break;
                                }



                                //够用，拷贝
                                string localFileName = sDir + usbFileName.Substring(usbFileName.LastIndexOf("\\"));
                                writeMsg("正在拷贝数据： (" + uploadSuccessFileNum.ToString() + ")" + usbFileName);
                                File.Copy(usbFileName, localFileName, true);



                                //删除USB源文件
                                writeMsg("正在删除数据： " + usbFileName);
                                File.Delete(usbFileName);

                                if (m_UploadFlag)//如果要上传到服务器，就不能删除本地文件
                                {
                                    FtpList.Add(localFileName);
                                }
                                //else//不应在此删除数据，应该在上传到服务器完成后删除
                                //{
                                //    writeMsg("正在删除数据： " + localFileName);
                                //    File.Delete(localFileName);
                                //}

                                //根据Name获得对应的控件对象,修改屏幕显示进度内容
                                int pos = MDUsbPos.getUsbPos(usbItem.driverName);
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
                                    MDUsbPos.setComplated(usbItem.driverName);//数据完成了，所以不应该再使用它
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
                    continue;
                }
            }
        }

        private void UploadWork()
        {
            //LogManager.showNotifaction("ereqrew");
            //string str1 = LogManager.getTimeString();
            //string str = LogManager.getTimeString() + Path.GetExtension("c:\\123\\345\\222\\12346.jpg");
            //str.Equals("");

            

            //启动时先查看当前目录下是否有未上传完成的文件
            if (Directory.Exists(System.Environment.CurrentDirectory + "\\Data\\"))
            {
                List<string> listUnUploadFiles = new List<string>();
                calcFileCountAndAdd(System.Environment.CurrentDirectory + "\\Data\\", listUnUploadFiles);
                FtpList.AddRange(listUnUploadFiles);
            }


            FtpClient ftpClient = new FtpClient(m_ftpSever, m_ftpUser, m_ftpPwd, 120, int.Parse(m_ftpPort), m_ftpBuffer);

            //for test
            //ftpClient.Upload(FtpList[0], true);//true,断点续传 使用工作站中的文件上传，防止U盘被拔掉
            //string remoteRanameFile1 = LogManager.getTimeStringFileExtension(FtpList[0]);
            //ftpClient.RenameFile(Path.GetFileName(FtpList[0]), remoteRanameFile1, true);//上传后立刻重命名 格式20120312100624_001

            while (threadRunFlag)
            {

                Thread.Sleep(1);
                for( int i = 0; i<FtpList.Count; i++)
                {

                    if (FtpList.Count <= 0 || !threadRunFlag)
                    {
                        Thread.Sleep(1000);
                        continue;
                    }

                    Thread.Sleep(1);

                    string localFileName = FtpList[i];

                    try
                    {
                        ftpClient.Login();

                        //ftpClient.Download("0218620121203103434.mp4", "123.mp4", true);//断点下载
                        //LogManager.WriteLog("正在上传文件: " + );
                        //ftpClient.Upload(@"E:\company\MD\自动上传软件\执法记录仪资料\mp4\0218620121203103434.mp4",true);//断点续传


                        //从接口获取文件FTP的上传路径
                        string interface_Ftp = m_interfaceStr + "?method=getFtpPath";
                        string interface_Upload = m_interfaceStr + "?method=uploadFile";
                        string responseText = "";
                        string removeDir = "";//服务器返回的FTP文件存放路径
                        string removeDir1 = "";
                        string removeFileName = "";//上传到FTP上的文件绝对路径 1/101/103/11.mp4
                        try
                        {
                            if (HttpWebResponseUtility.getFtpDirRequestStatusCode(interface_Ftp, m_WorkStationID, MDUsb.getPoliceIDFromFile(localFileName), m_AppCode, out removeDir1)
                            != System.Net.HttpStatusCode.OK)
                            {

                                //接口调用失败，停止下面的工作，直接退出
                                writeMsg("错误，路径接口调用失败，请检查系统设备配置、网络及服务器！");
                                continue;
                            }
                        }
                        catch (Exception ex)
                        {

                            writeMsg("错误，接口调用失败，请检查系统设备配置、网络及服务器！");
                            LogManager.WriteErrorLog(ex.Message);
                            continue;
                        }
                        
                        string[] list1 = removeDir1.Substring(removeDir1.LastIndexOf('\n') + 1).Split(';');
                        if (!list1[0].Equals("0"))
                        {
                            LogManager.showErrorMsg("错误，" + list1[1]);
                            continue;
                        }

                        removeDir = list1[1];



                        //切换到服务器接口返回的工作目录                            
                        ftpClient.MakeDirs(removeDir);
                        //ftpClient.ChangeDir(removeDir);

                        //上传文件
                        writeMsg("正在上传文件: " + localFileName);
                        ftpClient.Upload(localFileName, true);//true,断点续传 使用工作站中的文件上传，防止U盘被拔掉
                        string remoteRanameFile = LogManager.getTimeStringFileExtension(localFileName);
                        ftpClient.RenameFile(Path.GetFileName(localFileName), remoteRanameFile, true);//上传后立刻重命名 格式20120312100624_001


                        //获得文件的播放时间
                        string fileDuration = FFMpegUtility.getMediaPlayTime(localFileName);

                        removeFileName = removeDir + remoteRanameFile;// 包含全ftp路径，例：1/2/201202201223210044.jpg


                        FileInfo fileInfo = new FileInfo(localFileName);
                        try
                        {
                            //调用平台上传接口
                            if (HttpWebResponseUtility.getUrlRequestStatusCode(interface_Upload, m_WorkStationID, MDUsb.getPoliceIDFromFile(localFileName),
                                Path.GetFileName(localFileName), removeFileName, MDUsb.getDataTimeFromFile(localFileName), fileInfo.Length.ToString(), fileDuration,
                                m_AppCode, out responseText)
                                != System.Net.HttpStatusCode.OK)
                            {

                                //接口调用失败，停止下面的工作，直接退出
                                writeMsg("错误，上传接口调用失败，请检查系统设备配置、网络及服务器！");
                                continue;

                            }
                        }
                        catch (Exception ex)
                        {

                            writeMsg("错误，接口调用失败，请检查系统设备配置、网络及服务器！");
                            LogManager.WriteErrorLog(ex.Message);
                            continue;
                        }
                        string[] list2 = responseText.Substring(responseText.LastIndexOf('\n') + 1).Split(';');
                        if (!list2[0].Equals("0"))
                        {
                            LogManager.showErrorMsg("错误，" + list2[1]);
                            continue;
                        }


                        //上传成功删除队列及本地文件
                        FtpList.Remove(localFileName);
                        writeMsg("文件上传服务器成功" + localFileName);
                        File.Delete(localFileName);
                        writeMsg("删除已上传文件" + localFileName);
                        

                    }
                    catch (Exception ex)
                    {
                        LogManager.WriteLog(ex.Message);
                        Thread.Sleep(1000);
                        continue;
                    }                  



                }

            }



        }

        //有驱动模式的定时Timer
        private void timer_DriverVersion_Tick(object sender, EventArgs e)
        {
            timer_IntoUSBMode.Enabled = false;

            initDevice();            


            timer_IntoUSBMode.Enabled = true;

        }

        //进入关机等功能
        private void imageButton_Setup_Click(object sender, EventArgs e)
        {

            //从接口获取文件FTP的上传路径
            //string interface_Ftp = m_interfaceStr + "?method=getFtpPath";
            //string interface_Upload = m_interfaceStr + "?method=uploadFile";
            //string responseText = "";
            //string removeDir = "";//服务器返回的FTP文件存放路径
            ////if (HttpWebResponseUtility.getFtpDirRequestStatusCode(interface_Ftp, "010101", out responseText)
            ////    != System.Net.HttpStatusCode.OK)
            ////{

            ////    //接口调用失败，停止下面的工作，直接退出
            ////    writeMsg("错误，路径接口调用失败，请检查网络及服务器！");
            ////    return;
            ////}

            //if (HttpWebResponseUtility.getUrlRequestStatusCode(interface_Upload, "010101", "010101",
            //                            "1.wav", "1/101/103/22.mp4", "20121210232323", "128", "30",
            //                            out responseText)
            //                            != System.Net.HttpStatusCode.OK)
            //{

            //    //接口调用失败，停止下面的工作，直接退出
            //    writeMsg("错误，上传接口调用失败，请检查网络及服务器！");
            //    return;

            //}


            FormPassWord form2 = new FormPassWord();
            DialogResult result = form2.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.Cancel)
                return;




            ShutDownForm form1 = new ShutDownForm();
            form1.ShowDialog();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            threadRunFlag = false;
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            threadRunFlag = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 获得某个路径下的文件个数
        /// </summary>
        /// <param name="fileDir">文件路径</param>
        /// <returns></returns>
        public int calcFileCountAndAdd(string fileDir, List<string> unUploadFileList)
        {
            //目前只认A2打头的这几类文件
            unUploadFileList.AddRange(Directory.GetFiles(fileDir, "A2*.wav", SearchOption.AllDirectories));

            unUploadFileList.AddRange(Directory.GetFiles(fileDir, "A2*.mp4", SearchOption.AllDirectories));

            unUploadFileList.AddRange(Directory.GetFiles(fileDir, "A2*.jpg", SearchOption.AllDirectories));

            unUploadFileList.AddRange(Directory.GetFiles(fileDir, "A2*.avi", SearchOption.AllDirectories));

            return unUploadFileList.Count;
        }

        



    }


}
