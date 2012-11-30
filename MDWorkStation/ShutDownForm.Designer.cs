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
            this.button1.Location = new System.Drawing.Point(50, 177);
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
            this.Button2.Location = new System.Drawing.Point(245, 177);
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
            this.button3.Location = new System.Drawing.Point(448, 177);
            this.button3.Name = "button3";
            this.button3.NormalImage = global::MDWorkStation.Properties.Resources.c1;
            this.button3.Size = new System.Drawing.Size(148, 60);
            this.button3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.button3.TabIndex = 13;
            this.button3.TabStop = false;
            this.button3.Text = "取消";
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // ShutDownForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::MDWorkStation.Properties.Resources._1_376;
            this.ClientSize = new System.Drawing.Size(660, 298);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.Button2);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "ShutDownForm";
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
    }
}