using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace MDWorkStation
{
    public partial class ShutDownForm : Form
    {
        public ShutDownForm()
        {
            InitializeComponent();
        }

        //关闭电脑
        private void button1_Click(object sender, EventArgs e)
        {
            LogManager.WriteLog("关闭电脑(shutdown)...");
            Process.Start("shutdown", "-s -t 60");
        }

        //重启电脑
        private void Button2_Click(object sender, EventArgs e)
        {
            LogManager.WriteLog("关闭电脑(reset)...");
            Process.Start("shutdown", "-s -t 60");
        }

        //关闭当前form
        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
