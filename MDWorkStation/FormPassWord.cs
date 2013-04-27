using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MDWorkStation
{
    public partial class FormPassWord : Form
    {
        public FormPassWord()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, EventArgs e)
        {
            switch (((Button)sender).Text)
            {
                case "1":
                case "2":
                case "3":
                case "4":
                case "5":
                case "6":
                case "7":
                case "8":
                case "9":
                    if (textBox1.Text.Length < 6)
                        textBox1.Text += ((Button)sender).Text;
                    if (textBox1.Text == "999999")//密码验证
                    {
                        this.DialogResult = System.Windows.Forms.DialogResult.OK;
                        this.Close();
                    }
                    break;
                case "清空":
                    textBox1.Text = "";
                    break;
                case "关闭":
                    this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                    break;
            }
        }
    }
}
