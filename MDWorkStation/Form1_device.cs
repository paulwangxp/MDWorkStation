using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace MDWorkStation
{
    public partial class Form1 : Form
    {
        public struct DV_TM
        {
            public int tm_sec; /* 秒–取值区间为[0,59] */
            public int tm_min; /* 分 - 取值区间为[0,59] */
            public int tm_hour; /* 时 - 取值区间为[0,23] */
            public int tm_mday; /* 一个月中的日期 - 取值区间为[1,31] */
            public int tm_mon; /* 月份（从一月开始，0代表一月） - 取值区间为[0,11] */
            public int tm_year; /* 年份，其值等于实际年份如:2010 */
            public int tm_wday; /* 星期–取值区间为[0,6]，其中0代表星期天，1代表星期一，以此类推 */
            public int tm_yday; /* 从每年的1月1日开始的天数–取值区间为[0,365]，其中0代表1月1日，1代表1月2日，以此类推, 可设置为0*/
            public int tm_isdst; /* 可设置为0*/
        }


        [DllImport("CNTDVDLL.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall, EntryPoint = "CXK_ConnectDV")]
        static extern int CXK_ConnectDV();
        [DllImport("CNTDVDLL.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall, EntryPoint = "CXK_DisconnectDV")]
        static extern int CXK_DisconnectDV();
        [DllImport("CNTDVDLL.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall, EntryPoint = "CXK_Login")]
        static extern int CXK_Login(System.Text.StringBuilder password);//StringBuilder
        [DllImport("CNTDVDLL.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall, EntryPoint = "CXK_IntoUDiskMode")]
        static extern int CXK_IntoUDiskMode();
        [DllImport("CNTDVDLL.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall, EntryPoint = "CXK_GetID")]
        static extern int CXK_GetID(System.Text.StringBuilder ID);
        [DllImport("CNTDVDLL.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall, EntryPoint = "CXK_SetID")]
        static extern int CXK_SetID(System.Text.StringBuilder ID);
        [DllImport("CNTDVDLL.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall, EntryPoint = "CXK_GetTime")]
        static extern int CXK_GetTime(ref DV_TM ptm);
        [DllImport("CNTDVDLL.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall, EntryPoint = "CXK_SetTime")]
        static extern int CXK_SetTime(ref DV_TM ptm);
        [DllImport("CNTDVDLL.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall, EntryPoint = "CXK_SetPassword")]
        static extern int CXK_SetPassword(System.Text.StringBuilder pass);



        private int initDevice()
        {
            int rtn;
            rtn = CXK_ConnectDV();
            if (rtn > 0)
                MessageBox.Show(" 连接成功 ", " 高清DV ");
            else
                MessageBox.Show(" 连接成功 ", " 无驱动版本高清DV ");

            return rtn;
        }

        
    }


}
