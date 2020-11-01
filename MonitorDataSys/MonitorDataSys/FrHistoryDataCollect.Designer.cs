namespace MonitorDataSys
{
    partial class FrHistoryDataCollect
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
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.rtb_Log = new System.Windows.Forms.RichTextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.endTime = new System.Windows.Forms.DateTimePicker();
            this.startTime = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.dayRadio = new System.Windows.Forms.RadioButton();
            this.hourRadio = new System.Windows.Forms.RadioButton();
            this.btn_Stop = new System.Windows.Forms.Button();
            this.btn_Start = new System.Windows.Forms.Button();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.rtb_Log);
            this.groupBox4.Location = new System.Drawing.Point(5, 70);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(665, 276);
            this.groupBox4.TabIndex = 7;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "历史数据采集记录";
            // 
            // rtb_Log
            // 
            this.rtb_Log.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtb_Log.Location = new System.Drawing.Point(3, 17);
            this.rtb_Log.Name = "rtb_Log";
            this.rtb_Log.ReadOnly = true;
            this.rtb_Log.Size = new System.Drawing.Size(659, 256);
            this.rtb_Log.TabIndex = 0;
            this.rtb_Log.Text = "";
            // 
            // groupBox3
            // 
            this.groupBox3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.groupBox3.Controls.Add(this.endTime);
            this.groupBox3.Controls.Add(this.startTime);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.dayRadio);
            this.groupBox3.Controls.Add(this.hourRadio);
            this.groupBox3.Controls.Add(this.btn_Stop);
            this.groupBox3.Controls.Add(this.btn_Start);
            this.groupBox3.Location = new System.Drawing.Point(5, 1);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(665, 63);
            this.groupBox3.TabIndex = 6;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "历史采集操作";
            // 
            // endTime
            // 
            this.endTime.Location = new System.Drawing.Point(352, 24);
            this.endTime.Name = "endTime";
            this.endTime.Size = new System.Drawing.Size(104, 21);
            this.endTime.TabIndex = 3;
            // 
            // startTime
            // 
            this.startTime.Location = new System.Drawing.Point(179, 25);
            this.startTime.Name = "startTime";
            this.startTime.Size = new System.Drawing.Size(104, 21);
            this.startTime.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(293, 29);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 7;
            this.label2.Text = "终止时间：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(118, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 6;
            this.label1.Text = "起始时间：";
            // 
            // dayRadio
            // 
            this.dayRadio.AutoSize = true;
            this.dayRadio.Location = new System.Drawing.Point(60, 27);
            this.dayRadio.Name = "dayRadio";
            this.dayRadio.Size = new System.Drawing.Size(47, 16);
            this.dayRadio.TabIndex = 5;
            this.dayRadio.Text = "日均";
            this.dayRadio.UseVisualStyleBackColor = true;
            // 
            // hourRadio
            // 
            this.hourRadio.AutoSize = true;
            this.hourRadio.Checked = true;
            this.hourRadio.Location = new System.Drawing.Point(12, 26);
            this.hourRadio.Name = "hourRadio";
            this.hourRadio.Size = new System.Drawing.Size(47, 16);
            this.hourRadio.TabIndex = 4;
            this.hourRadio.TabStop = true;
            this.hourRadio.Text = "小时";
            this.hourRadio.UseVisualStyleBackColor = true;
            // 
            // btn_Stop
            // 
            this.btn_Stop.Enabled = false;
            this.btn_Stop.Image = global::MonitorDataSys.Properties.Resources.stop;
            this.btn_Stop.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_Stop.Location = new System.Drawing.Point(568, 17);
            this.btn_Stop.Name = "btn_Stop";
            this.btn_Stop.Size = new System.Drawing.Size(80, 36);
            this.btn_Stop.TabIndex = 1;
            this.btn_Stop.Text = "停止补集";
            this.btn_Stop.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btn_Stop.UseVisualStyleBackColor = true;
            this.btn_Stop.Click += new System.EventHandler(this.btn_Stop_Click);
            // 
            // btn_Start
            // 
            this.btn_Start.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btn_Start.Image = global::MonitorDataSys.Properties.Resources.start;
            this.btn_Start.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_Start.Location = new System.Drawing.Point(478, 17);
            this.btn_Start.Name = "btn_Start";
            this.btn_Start.Size = new System.Drawing.Size(81, 36);
            this.btn_Start.TabIndex = 0;
            this.btn_Start.Text = "历史补采";
            this.btn_Start.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btn_Start.UseVisualStyleBackColor = true;
            this.btn_Start.Click += new System.EventHandler(this.btn_Start_Click);
            // 
            // FrHistoryDataCollect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(674, 348);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FrHistoryDataCollect";
            this.Text = "FrDataCollect";
            this.Load += new System.EventHandler(this.FrDataCollect_Load);
            this.groupBox4.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.RichTextBox rtb_Log;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btn_Stop;
        private System.Windows.Forms.Button btn_Start;
        private System.Windows.Forms.RadioButton dayRadio;
        private System.Windows.Forms.RadioButton hourRadio;
        private System.Windows.Forms.DateTimePicker endTime;
        private System.Windows.Forms.DateTimePicker startTime;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
    }
}