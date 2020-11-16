namespace MonitorDataSys
{
    partial class FrDataCollect
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
            this.nuD_day_Day = new System.Windows.Forms.NumericUpDown();
            this.nuD_min_Hour = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.btn_Stop = new System.Windows.Forms.Button();
            this.btn_Start = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lb_NextTime = new System.Windows.Forms.Label();
            this.lb_AllTotal = new System.Windows.Forms.Label();
            this.lb_TodayTotal = new System.Windows.Forms.Label();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nuD_day_Day)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nuD_min_Hour)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.rtb_Log);
            this.groupBox4.Location = new System.Drawing.Point(5, 88);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(665, 258);
            this.groupBox4.TabIndex = 7;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "数据记录";
            // 
            // rtb_Log
            // 
            this.rtb_Log.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtb_Log.Location = new System.Drawing.Point(3, 17);
            this.rtb_Log.Name = "rtb_Log";
            this.rtb_Log.ReadOnly = true;
            this.rtb_Log.Size = new System.Drawing.Size(659, 238);
            this.rtb_Log.TabIndex = 0;
            this.rtb_Log.Text = "";
            // 
            // groupBox3
            // 
            this.groupBox3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.groupBox3.Controls.Add(this.nuD_day_Day);
            this.groupBox3.Controls.Add(this.nuD_min_Hour);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.btn_Stop);
            this.groupBox3.Controls.Add(this.btn_Start);
            this.groupBox3.Location = new System.Drawing.Point(302, 3);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(368, 79);
            this.groupBox3.TabIndex = 6;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "操作";
            // 
            // nuD_day_Day
            // 
            this.nuD_day_Day.Location = new System.Drawing.Point(82, 48);
            this.nuD_day_Day.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nuD_day_Day.Name = "nuD_day_Day";
            this.nuD_day_Day.ReadOnly = true;
            this.nuD_day_Day.Size = new System.Drawing.Size(56, 21);
            this.nuD_day_Day.TabIndex = 5;
            this.nuD_day_Day.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // nuD_min_Hour
            // 
            this.nuD_min_Hour.Location = new System.Drawing.Point(82, 19);
            this.nuD_min_Hour.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nuD_min_Hour.Name = "nuD_min_Hour";
            this.nuD_min_Hour.ReadOnly = true;
            this.nuD_min_Hour.Size = new System.Drawing.Size(56, 21);
            this.nuD_min_Hour.TabIndex = 2;
            this.nuD_min_Hour.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(137, 50);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 12);
            this.label1.TabIndex = 7;
            this.label1.Text = "小时/次";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 50);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 12);
            this.label2.TabIndex = 6;
            this.label2.Text = "日均数据频率";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(136, 23);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(47, 12);
            this.label8.TabIndex = 4;
            this.label8.Text = "分钟/次";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(5, 24);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(77, 12);
            this.label7.TabIndex = 3;
            this.label7.Text = "小时数据频率";
            // 
            // btn_Stop
            // 
            this.btn_Stop.Enabled = false;
            this.btn_Stop.Image = global::MonitorDataSys.Properties.Resources.stop;
            this.btn_Stop.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_Stop.Location = new System.Drawing.Point(279, 24);
            this.btn_Stop.Name = "btn_Stop";
            this.btn_Stop.Size = new System.Drawing.Size(80, 36);
            this.btn_Stop.TabIndex = 1;
            this.btn_Stop.Text = "停止采集";
            this.btn_Stop.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btn_Stop.UseVisualStyleBackColor = true;
            this.btn_Stop.Click += new System.EventHandler(this.btn_Stop_Click);
            // 
            // btn_Start
            // 
            this.btn_Start.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btn_Start.Image = global::MonitorDataSys.Properties.Resources.start;
            this.btn_Start.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_Start.Location = new System.Drawing.Point(189, 24);
            this.btn_Start.Name = "btn_Start";
            this.btn_Start.Size = new System.Drawing.Size(81, 36);
            this.btn_Start.TabIndex = 0;
            this.btn_Start.Text = "开始采集";
            this.btn_Start.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btn_Start.UseVisualStyleBackColor = true;
            this.btn_Start.Click += new System.EventHandler(this.btn_Start_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.groupBox1.Controls.Add(this.lb_NextTime);
            this.groupBox1.Controls.Add(this.lb_AllTotal);
            this.groupBox1.Controls.Add(this.lb_TodayTotal);
            this.groupBox1.Location = new System.Drawing.Point(5, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(291, 79);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "采集数据统计";
            // 
            // lb_NextTime
            // 
            this.lb_NextTime.AutoSize = true;
            this.lb_NextTime.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lb_NextTime.ForeColor = System.Drawing.Color.Fuchsia;
            this.lb_NextTime.Location = new System.Drawing.Point(9, 52);
            this.lb_NextTime.Name = "lb_NextTime";
            this.lb_NextTime.Size = new System.Drawing.Size(103, 12);
            this.lb_NextTime.TabIndex = 2;
            this.lb_NextTime.Text = "最近采集时间：-";
            // 
            // lb_AllTotal
            // 
            this.lb_AllTotal.AutoSize = true;
            this.lb_AllTotal.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lb_AllTotal.ForeColor = System.Drawing.Color.Blue;
            this.lb_AllTotal.Location = new System.Drawing.Point(153, 24);
            this.lb_AllTotal.Name = "lb_AllTotal";
            this.lb_AllTotal.Size = new System.Drawing.Size(90, 12);
            this.lb_AllTotal.TabIndex = 1;
            this.lb_AllTotal.Text = "累计采集：0条";
            // 
            // lb_TodayTotal
            // 
            this.lb_TodayTotal.AutoSize = true;
            this.lb_TodayTotal.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lb_TodayTotal.ForeColor = System.Drawing.Color.Blue;
            this.lb_TodayTotal.Location = new System.Drawing.Point(9, 24);
            this.lb_TodayTotal.Name = "lb_TodayTotal";
            this.lb_TodayTotal.Size = new System.Drawing.Size(90, 12);
            this.lb_TodayTotal.TabIndex = 0;
            this.lb_TodayTotal.Text = "今日采集：0条";
            // 
            // FrDataCollect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(674, 348);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FrDataCollect";
            this.Text = "FrDataCollect";
            this.Load += new System.EventHandler(this.FrDataCollect_Load);
            this.groupBox4.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nuD_day_Day)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nuD_min_Hour)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.RichTextBox rtb_Log;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown nuD_min_Hour;
        private System.Windows.Forms.Button btn_Stop;
        private System.Windows.Forms.Button btn_Start;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lb_TodayTotal;
        private System.Windows.Forms.Label lb_NextTime;
        private System.Windows.Forms.Label lb_AllTotal;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown nuD_day_Day;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
    }
}