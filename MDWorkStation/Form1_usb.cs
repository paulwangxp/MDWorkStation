using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;
using Microsoft.Win32;

namespace MDWorkStation
{
    public partial class Form1 : Form
    {
        public const int WM_DEVICECHANGE = 0x219;
        public const int DBT_DEVICEARRIVAL = 0x8000;
        public const int DBT_CONFIGCHANGECANCELED = 0x0019;
        public const int DBT_CONFIGCHANGED = 0x0018;
        public const int DBT_CUSTOMEVENT = 0x8006;
        public const int DBT_DEVICEQUERYREMOVE = 0x8001;
        public const int DBT_DEVICEQUERYREMOVEFAILED = 0x8002;
        public const int DBT_DEVICEREMOVECOMPLETE = 0x8004;
        public const int DBT_DEVICEREMOVEPENDING = 0x8003;
        public const int DBT_DEVICETYPESPECIFIC = 0x8005;
        public const int DBT_DEVNODES_CHANGED = 0x0007;
        public const int DBT_QUERYCHANGECONFIG = 0x0017;
        public const int DBT_USERDEFINED = 0xFFFF;
        // 逻辑卷标
        public const int DBT_DEVTYP_VOLUME = 0x00000002;
        // private LockScreen Ls = new LockScreen();
        public string ID = "";
        public string Value;
        public string[] item;

        [StructLayout(LayoutKind.Sequential)]
        public struct DEV_BROADCAST_VOLUME
        {
            public int dbcv_size;
            public int dbcv_devicetype;
            public int dbcv_reserved;
            public int dbcv_unitmask;
        }

        protected override void WndProc(ref Message m)
        {
            try
            {
                if (m.Msg == WM_DEVICECHANGE)
                {
                    switch (m.WParam.ToInt32())
                    {
                        case WM_DEVICECHANGE:
                            break;
                        case DBT_DEVICEARRIVAL://U盘有插入
                            DriveInfo[] s = DriveInfo.GetDrives();
                            foreach (DriveInfo DriveI in s)
                            {
                                if (DriveI.DriveType == DriveType.Removable)
                                {
                                    // Ls.Show();
                                    // this.Hide();
                                    // MessageBox.Show("sss");

                                    

                                    break;
                                }
                                int devType = Marshal.ReadInt32(m.LParam, 4);
                                if (devType == DBT_DEVTYP_VOLUME)
                                {
                                    DEV_BROADCAST_VOLUME vol;
                                    vol = (DEV_BROADCAST_VOLUME)Marshal.PtrToStructure(m.LParam, typeof(DEV_BROADCAST_VOLUME));
                                    ID = vol.dbcv_unitmask.ToString("x");
                                    this.Text = IO(ID);
                                    this.Tag = IO(ID);
                                    //if (item.Length ==0||IO(ID)!=this.Tag.ToString ())
                                    //{

                                    //}

                                    //根据盘符获得磁盘信息，在屏幕上显示，并且将此对象放入队列待用
                                    setUsbInfoToScreen(this.Text+"\\", true);
                                    break;

                                }
                                
                            }
                            break;
                        case DBT_CONFIGCHANGECANCELED:
                            break;
                        case DBT_CONFIGCHANGED:
                            break;
                        case DBT_CUSTOMEVENT:
                            break;
                        case DBT_DEVICEQUERYREMOVE:
                            break;
                        case DBT_DEVICEQUERYREMOVEFAILED:
                            break;
                        case DBT_DEVICEREMOVECOMPLETE: //U盘卸载
                            DriveInfo[] I = DriveInfo.GetDrives();
                            foreach (DriveInfo DrInfo in I)
                            {
                                int devType = Marshal.ReadInt32(m.LParam, 4);
                                if (devType == DBT_DEVTYP_VOLUME)
                                {
                                    DEV_BROADCAST_VOLUME vol;
                                    vol = (DEV_BROADCAST_VOLUME)Marshal.PtrToStructure(m.LParam, typeof(DEV_BROADCAST_VOLUME));
                                    ID = vol.dbcv_unitmask.ToString("x");
                                    this.Text = IO(ID);

                                    //根据盘符获得磁盘信息，在屏幕上显示，并且将此对象放入队列待用
                                    setUsbInfoToScreen(this.Text + "\\", false);
                                    break;

                                }
                                
                                
                                // MessageBox.Show("U盘已经卸载", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            break;
                        case DBT_DEVICEREMOVEPENDING:
                            break;
                        case DBT_DEVICETYPESPECIFIC:
                            break;
                        case DBT_DEVNODES_CHANGED:
                            break;
                        case DBT_QUERYCHANGECONFIG:
                            break;
                        case DBT_USERDEFINED:
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                //throw new Exception(ex.Message);
                LogManager.showErrorMsg(ex.Message + "请检查插入的U盘");
            }
            base.WndProc(ref m);
        }

        public string IO(string ff)
        {
            switch (ff)
            {
                case "1":
                    Value = "A:";
                    break;
                case "2":
                    Value = "B:";
                    break;
                case "4":
                    Value = "C:";
                    break;
                case "8":
                    Value = "D:";
                    break;
                case "10":
                    Value = "E:";
                    break;
                case "20":
                    Value = "F:";
                    break;
                case "40":
                    Value = "G:";
                    break;
                case "80":
                    Value = "H:";
                    break;
                case "100":
                    Value = "I:";
                    break;
                case "200":
                    Value = "J:";
                    break;
                case "400":
                    Value = "K:";
                    break;
                case "800":
                    Value = "L:";
                    break;
                case "1000":
                    Value = "M:";
                    break;
                case "2000":
                    Value = "N:";
                    break;
                case "4000":
                    Value = "O:";
                    break;
                case "8000":
                    Value = "P:";
                    break;
                case "10000":
                    Value = "Q:";
                    break;
                case "20000":
                    Value = "R:";
                    break;
                case "40000":
                    Value = "S:";
                    break;
                case "80000":
                    Value = "T:";
                    break;
                case "100000":
                    Value = "U:";
                    break;
                case "200000":
                    Value = "V:";
                    break;
                case "400000":
                    Value = "W:";
                    break;
                case "800000":
                    Value = "X:";
                    break;
                case "1000000":
                    Value = "Y:";
                    break;
                case "2000000":
                    Value = "Z:";
                    break;
                default: break;
            }
            return Value;
        }

        //根据盘符获得磁盘信息
        private bool getDiskInfo(string diskName/*盘符*/ ,MDUsb usbDisk/*Out 返回一个对象*/)
        {

            DriveInfo[] s = DriveInfo.GetDrives();
            bool findDisk = false;
            foreach (DriveInfo myDrive in s)
            {
                if (myDrive.Name.Equals(diskName.ToUpper()))
                {
                    findDisk = true;

                    usbDisk.driverName = myDrive.Name;
                    usbDisk.driverLable = myDrive.VolumeLabel;
                    usbDisk.driverType = myDrive.DriveType.ToString();
                    usbDisk.driverFormat = myDrive.DriveFormat;
                    usbDisk.TotalSize = myDrive.TotalSize.ToString();
                    usbDisk.TotalFreeSpace = myDrive.TotalFreeSpace.ToString();
                    usbDisk.AvailableFreeSpace = myDrive.AvailableFreeSpace.ToString();
                    usbDisk.freePercent = (int)(myDrive.AvailableFreeSpace * 100 / myDrive.TotalSize);

                    break;
                    
                }
            }

            return findDisk;
        }


        //得到一个驱动器的剩余空间
        public long getDiskFreeSpace(string DiskName)
        {
            DriveInfo[] s = DriveInfo.GetDrives();
            foreach (DriveInfo myDrive in s)
            {
                if (myDrive.Name.Contains(DiskName))
                {
                    return myDrive.TotalFreeSpace;
                }
            }

            return 0;
        }

        private string[] getUsbDeviceName()
        {
            string []deviceInfo = new string[26];

            StringBuilder sb = new StringBuilder();

            int num = 0;


            DriveInfo[] s = DriveInfo.GetDrives();
            foreach (DriveInfo myDrive in s)
            {
                if (myDrive.IsReady)
                {
                    sb.Append("磁盘驱动器盘符：");
                    sb.AppendLine(myDrive.Name);
                    sb.Append("磁盘卷标：");
                    sb.AppendLine(myDrive.VolumeLabel);
                    sb.Append("磁盘类型：");
                    sb.AppendLine(myDrive.DriveType.ToString());
                    sb.Append("磁盘格式：");
                    sb.AppendLine(myDrive.DriveFormat);
                    sb.Append("磁盘大小：");
                    sb.AppendLine(myDrive.TotalSize.ToString());
                    sb.Append("剩余空间：");
                    sb.AppendLine(myDrive.AvailableFreeSpace.ToString());
                    sb.Append("总剩余空间（含磁盘配额）：");
                    sb.AppendLine(myDrive.TotalFreeSpace.ToString());
                    sb.Append("剩余空间（百分比）：");
                    int freePercent = (int)(myDrive.AvailableFreeSpace * 100 / myDrive.TotalSize);
                    sb.AppendLine(freePercent.ToString()+" %");
                    sb.AppendLine("-------------------------------------");
                }
                if (myDrive.DriveType == DriveType.Removable)//usb drivers
                {
                    deviceInfo[num++] = myDrive.Name;
                }
                
            }
            //label_device.Text = sb.ToString();

            return deviceInfo;
        }


        //利用注册表开机启动
        public static void RunWhenStart(bool Started, string name, string path)
        {

            //如果是vista及以上版本，不支持直接写注册表，所以退出
            if (Environment.OSVersion.Platform == PlatformID.Win32NT &&
                Environment.OSVersion.Version.Major >= 6)
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

        private int getUsbPos(string driverName)
        {
            int i = 0;
            foreach (MDUsb usbItem in usbDiskDic.Values)
            {
                i++;
                if (usbItem.driverName.Equals(driverName))
                    return i;
            }


            return -1;
        }

        private void setUsbInfoToScreen(string diskName, bool isShow)
        {
            string currentDriverName = diskName;
            int pos = -1;

            //根据盘符获得磁盘信息，并且将此对象放入队列待用
            MDUsb usbDiskObject = new MDUsb();
            if (isShow)//插入U盘
            {
                if (getDiskInfo(currentDriverName, usbDiskObject))
                {

                    usbDiskDic.Add(currentDriverName, usbDiskObject);//将插入U盘对象放入容器
                    writeMsg(usbDiskObject.getDiskInfoStrirng());
                    writeMsg(diskName + " 盘 成功载入");
                }
                else
                {
                    writeMsg(diskName + " 盘 内容读取失败！");
                    return;
                }


                //  获得当前U盘对应的屏幕位置
                pos = MDUsbPos.getUsbPos(currentDriverName);

            }
            else//U盘拔出
            {
                if( usbDiskDic.ContainsKey(currentDriverName) )
                {
                    usbDiskObject = usbDiskDic[currentDriverName];//取出对应的元素
                    //  获得当前U盘对应的屏幕位置
                    pos = MDUsbPos.getUsbPos(currentDriverName);
                    MDUsbPos.setDisconnect(currentDriverName);//拔出时清空数据                    
                    usbDiskDic.Remove(currentDriverName);
                    
                }

                if (pos < 0)
                    return;

                writeMsg(diskName + " 盘已经拔出");

            }


            //修改屏幕上的lable信息-------------------------------------------------------

            

            //根据Name获得对应的控件对象
            string sID = "label_id_" + pos.ToString(); ;
            string sCount = "label_count_" + pos.ToString();
            string sPic = "pictureBox" + pos.ToString();
            System.Reflection.FieldInfo label1 = this.GetType().GetField(sID);//反射
            System.Reflection.FieldInfo label2 = this.GetType().GetField(sCount);//反射
            System.Reflection.FieldInfo pic1 = this.GetType().GetField(sPic);//反射

            Label lableID = (Label)label1.GetValue(this);
            Label lableCount = (Label)label2.GetValue(this);
            PictureBox pic = (PictureBox)pic1.GetValue(this);


            if (isShow)
            {

                

                //  获得当前U盘的文件个数
                int count = usbDiskObject.getFileCount();


                
                lableCount.Text = "0 / " + count.ToString();// 0 / 999
                lableCount.Visible = true;
                lableCount.Refresh();


                //  获得当前U盘的设备ID
                
                //lableID.Text = usbDiskObject.getDeviceID();// 设备ID： 99999
                //lableID.Text = "编号： " + usbDiskObject.getPoliceID();// 警员编号ID： 99999
                lableID.Text = "编号 " + usbDiskObject.getDeviceID();// 设备编号ID： A000022
                string id1 = usbDiskObject.getPoliceID();
                string id2 = usbDiskObject.getDataTime();
                lableID.Visible = true;
                lableID.Refresh();


                //设置设备颜色为蓝色
                pic.BackgroundImage = MDWorkStation.Properties.Resources.b3;
                pic.Refresh();
            }
            else
            {
                lableCount.Visible = lableID.Visible = false;
                pic.BackgroundImage = MDWorkStation.Properties.Resources.b2;//灰色
                pic.Refresh();

            }

            //修改屏幕上的lable信息 end-------------------------------------------------------

            return;

            /*

            if (bFirstRun)//只运行一次
                StartIdle();

            return;


            if (bFirstRun)
            {


                worker.DoWork += new DoWorkEventHandler(worker_DoWork);
                worker.ProgressChanged += new ProgressChangedEventHandler(worker_ProgressChanged);
                worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_RunWorkerCompleted);

                bFirstRun = false;

                return;

            }

            return;
             * */

            
        }
    }
       
      


}
