using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;

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
                            this.timer1.Enabled = true;
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
                                }
                                this.label_device.Text = this.Text;
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
                                    this.Text = IO(ID) + "盘退出！\n";


                                }
                                this.label_device.Text += this.Text;
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
                throw new Exception(ex.Message);
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
            label_device.Text = sb.ToString();

            return deviceInfo;
        }

      
    }


}
