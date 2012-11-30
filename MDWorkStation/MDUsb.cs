using System;
using System.Collections.Generic;
using System.Text;

namespace MDWorkStation
{
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
    }
}
