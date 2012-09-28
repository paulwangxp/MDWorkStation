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
        public Form1()
        {
            this.WindowState = FormWindowState.Maximized;

            InitializeComponent();

            InitControlPos();//设置各控件的位置，用于不同分辨率的情况

            string []drivers = getUsbDeviceName();//获得usb的盘符

            //initDevice();
        }

       


        private void InitControlPos()
        {
            //label1.Parent = pictureBox1;
            //label1.BringToFront();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }
    }


}
