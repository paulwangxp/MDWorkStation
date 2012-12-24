namespace MDWorkStation
{
    partial class ShutDownForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.ImageButton();
            this.Button2 = new System.Windows.Forms.ImageButton();
            this.button3 = new System.Windows.Forms.ImageButton();
            this.label1 = new System.Windows.Forms.Label();
            this.checkBox_AllDay = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.dateTimePicker2 = new System.Windows.Forms.DateTimePicker();
            ((System.ComponentModel.ISupportInitialize)(this.button1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Button2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.button3)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.Transparent;
            this.button1.DialogResult = System.Windows.Forms.DialogResult.None;
            this.button1.DownImage = global::MDWorkStation.Properties.Resources.c3;
            this.button1.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button1.HoverImage = global::MDWorkStation.Properties.Resources.c2;
            this.button1.Location = new System.Drawing.Point(74, 282);
            this.button1.Name = "button1";
            this.button1.NormalImage = global::MDWorkStation.Properties.Resources.c1;
            this.button1.Size = new System.Drawing.Size(148, 60);
            this.button1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.button1.TabIndex = 11;
            this.button1.TabStop = false;
            this.button1.Text = "关闭电脑";
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Button2
            // 
            this.Button2.BackColor = System.Drawing.Color.Transparent;
            this.Button2.DialogResult = System.Windows.Forms.DialogResult.None;
            this.Button2.DownImage = global::MDWorkStation.Properties.Resources.c3;
            this.Button2.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Bold);
            this.Button2.HoverImage = global::MDWorkStation.Properties.Resources.c2;
            this.Button2.Location = new System.Drawing.Point(269, 282);
            this.Button2.Name = "Button2";
            this.Button2.NormalImage = global::MDWorkStation.Properties.Resources.c1;
            this.Button2.Size = new System.Drawing.Size(148, 60);
            this.Button2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.Button2.TabIndex = 12;
            this.Button2.TabStop = false;
            this.Button2.Text = "重启电脑";
            this.Button2.Click += new System.EventHandler(this.Button2_Click);
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.Color.Transparent;
            this.button3.DialogResult = System.Windows.Forms.DialogResult.None;
            this.button3.DownImage = global::MDWorkStation.Properties.Resources.c3;
            this.button3.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Bold);
            this.button3.HoverImage = global::MDWorkStation.Properties.Resources.c2;
            this.button3.Location = new System.Drawing.Point(472, 282);
            this.button3.Name = "button3";
            this.button3.NormalImage = global::MDWorkStation.Properties.Resources.c1;
            this.button3.Size = new System.Drawing.Size(148, 60);
            this.button3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.button3.TabIndex = 13;
            this.button3.TabStop = false;
            this.button3.Text = "取消";
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.Color.DarkOrange;
            this.label1.Location = new System.Drawing.Point(72, 65);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(220, 21);
            this.label1.TabIndex = 14;
            this.label1.Text = "选择上传服务器时间：";
            // 
            // checkBox_AllDay
            // 
            this.checkBox_AllDay.AutoSize = true;
            this.checkBox_AllDay.BackColor = System.Drawing.Color.Transparent;
            this.checkBox_AllDay.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.checkBox_AllDay.Location = new System.Drawing.Point(307, 65);
            this.checkBox_AllDay.Name = "checkBox_AllDay";
            this.checkBox_AllDay.Size = new System.Drawing.Size(142, 23);
            this.checkBox_AllDay.TabIndex = 15;
            this.checkBox_AllDay.Text = "全天自动上传";
            this.checkBox_AllDay.UseVisualStyleBackColor = false;
            this.checkBox_AllDay.CheckedChanged += new System.EventHandler(this.checkBox_AllDay_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.ForeColor = System.Drawing.Color.DarkOrange;
            this.label2.Location = new System.Drawing.Point(70, 120);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(115, 21);
            this.label2.TabIndex = 17;
            this.label2.Text = "开始时间：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.ForeColor = System.Drawing.Color.DarkOrange;
            this.label3.Location = new System.Drawing.Point(72, 164);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(115, 21);
            this.label3.TabIndex = 19;
            this.label3.Text = "结束时间：";
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.CustomFormat = "HH:mm:ss";
            this.dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker1.Location = new System.Drawing.Point(191, 120);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.ShowUpDown = true;
            this.dateTimePicker1.Size = new System.Drawing.Size(200, 21);
            this.dateTimePicker1.TabIndex = 21;
            // 
            // dateTimePicker2
            // 
            this.dateTimePicker2.CustomFormat = "HH:mm:ss";
            this.dateTimePicker2.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker2.Location = new System.Drawing.Point(191, 166);
            this.dateTimePicker2.Name = "dateTimePicker2";
            this.dateTimePicker2.ShowUpDown = true;
            this.dateTimePicker2.Size = new System.Drawing.Size(200, 21);
            this.dateTimePicker2.TabIndex = 21;
            // 
            // ShutDownForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::MDWorkStation.Properties.Resources._1_376;
            this.ClientSize = new System.Drawing.Size(741, 400);
            this.Controls.Add(this.dateTimePicker2);
            this.Controls.Add(this.dateTimePicker1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.checkBox_AllDay);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.Button2);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "ShutDownForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "请选择对应的功能";
            ((System.ComponentModel.ISupportInitialize)(this.button1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Button2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.button3)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ImageButton button1;
        private System.Windows.Forms.ImageButton Button2;
        private System.Windows.Forms.ImageButton button3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox checkBox_AllDay;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.DateTimePicker dateTimePicker2;
    }
}