namespace MonitorDataSys
{
    partial class Fr_Main
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Fr_Main));
            this.groupBoxLeftMenu = new System.Windows.Forms.GroupBox();
            this.historyCollectBtn = new System.Windows.Forms.Button();
            this.dataSourceConfigBtn = new System.Windows.Forms.Button();
            this.dataCollectBtn = new System.Windows.Forms.Button();
            this.aboutUsBtn = new System.Windows.Forms.Button();
            this.lookLogBtn = new System.Windows.Forms.Button();
            this.dataConfigBtn = new System.Windows.Forms.Button();
            this.groupBoxRightMenu = new System.Windows.Forms.GroupBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.NotifyMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.显示ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.退出ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.groupBoxLeftMenu.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.NotifyMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxLeftMenu
            // 
            this.groupBoxLeftMenu.Controls.Add(this.historyCollectBtn);
            this.groupBoxLeftMenu.Controls.Add(this.dataSourceConfigBtn);
            this.groupBoxLeftMenu.Controls.Add(this.dataCollectBtn);
            this.groupBoxLeftMenu.Controls.Add(this.aboutUsBtn);
            this.groupBoxLeftMenu.Controls.Add(this.lookLogBtn);
            this.groupBoxLeftMenu.Controls.Add(this.dataConfigBtn);
            this.groupBoxLeftMenu.Location = new System.Drawing.Point(13, 13);
            this.groupBoxLeftMenu.Name = "groupBoxLeftMenu";
            this.groupBoxLeftMenu.Size = new System.Drawing.Size(180, 371);
            this.groupBoxLeftMenu.TabIndex = 0;
            this.groupBoxLeftMenu.TabStop = false;
            this.groupBoxLeftMenu.Text = "菜单区";
            // 
            // historyCollectBtn
            // 
            this.historyCollectBtn.BackColor = System.Drawing.SystemColors.Control;
            this.historyCollectBtn.Image = global::MonitorDataSys.Properties.Resources.lscj1;
            this.historyCollectBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.historyCollectBtn.Location = new System.Drawing.Point(16, 251);
            this.historyCollectBtn.Name = "historyCollectBtn";
            this.historyCollectBtn.Size = new System.Drawing.Size(146, 54);
            this.historyCollectBtn.TabIndex = 5;
            this.historyCollectBtn.Text = "History历史采集";
            this.historyCollectBtn.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.historyCollectBtn.UseVisualStyleBackColor = false;
            this.historyCollectBtn.Click += new System.EventHandler(this.historyCollectBtn_Click);
            // 
            // dataSourceConfigBtn
            // 
            this.dataSourceConfigBtn.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.dataSourceConfigBtn.Image = global::MonitorDataSys.Properties.Resources.pzsjy;
            this.dataSourceConfigBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.dataSourceConfigBtn.Location = new System.Drawing.Point(16, 15);
            this.dataSourceConfigBtn.Name = "dataSourceConfigBtn";
            this.dataSourceConfigBtn.Size = new System.Drawing.Size(146, 52);
            this.dataSourceConfigBtn.TabIndex = 0;
            this.dataSourceConfigBtn.Text = "Set配置数据源";
            this.dataSourceConfigBtn.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.dataSourceConfigBtn.UseVisualStyleBackColor = false;
            this.dataSourceConfigBtn.Click += new System.EventHandler(this.dataSourceConfigBtn_Click);
            // 
            // dataCollectBtn
            // 
            this.dataCollectBtn.BackColor = System.Drawing.SystemColors.Control;
            this.dataCollectBtn.Image = global::MonitorDataSys.Properties.Resources.dqhj;
            this.dataCollectBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.dataCollectBtn.Location = new System.Drawing.Point(16, 133);
            this.dataCollectBtn.Name = "dataCollectBtn";
            this.dataCollectBtn.Size = new System.Drawing.Size(146, 54);
            this.dataCollectBtn.TabIndex = 2;
            this.dataCollectBtn.Text = "Do数据采集  ";
            this.dataCollectBtn.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.dataCollectBtn.UseVisualStyleBackColor = false;
            this.dataCollectBtn.Click += new System.EventHandler(this.dataCollectBtn_Click);
            // 
            // aboutUsBtn
            // 
            this.aboutUsBtn.BackColor = System.Drawing.SystemColors.Control;
            this.aboutUsBtn.Image = global::MonitorDataSys.Properties.Resources.lxwm;
            this.aboutUsBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.aboutUsBtn.Location = new System.Drawing.Point(16, 310);
            this.aboutUsBtn.Name = "aboutUsBtn";
            this.aboutUsBtn.Size = new System.Drawing.Size(146, 54);
            this.aboutUsBtn.TabIndex = 4;
            this.aboutUsBtn.Text = "About关于我们";
            this.aboutUsBtn.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.aboutUsBtn.UseVisualStyleBackColor = false;
            this.aboutUsBtn.Click += new System.EventHandler(this.aboutUsBtn_Click);
            // 
            // lookLogBtn
            // 
            this.lookLogBtn.BackColor = System.Drawing.SystemColors.Control;
            this.lookLogBtn.Image = global::MonitorDataSys.Properties.Resources.hjpz;
            this.lookLogBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lookLogBtn.Location = new System.Drawing.Point(16, 192);
            this.lookLogBtn.Name = "lookLogBtn";
            this.lookLogBtn.Size = new System.Drawing.Size(146, 54);
            this.lookLogBtn.TabIndex = 3;
            this.lookLogBtn.Text = "Look日志查看 ";
            this.lookLogBtn.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lookLogBtn.UseVisualStyleBackColor = false;
            this.lookLogBtn.Click += new System.EventHandler(this.lookLogBtn_Click);
            // 
            // dataConfigBtn
            // 
            this.dataConfigBtn.BackColor = System.Drawing.SystemColors.Control;
            this.dataConfigBtn.Image = global::MonitorDataSys.Properties.Resources.cssz;
            this.dataConfigBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.dataConfigBtn.Location = new System.Drawing.Point(16, 73);
            this.dataConfigBtn.Name = "dataConfigBtn";
            this.dataConfigBtn.Size = new System.Drawing.Size(146, 54);
            this.dataConfigBtn.TabIndex = 1;
            this.dataConfigBtn.Text = "Base站点同步 ";
            this.dataConfigBtn.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.dataConfigBtn.UseVisualStyleBackColor = false;
            this.dataConfigBtn.Click += new System.EventHandler(this.dataConfigBtn_Click);
            // 
            // groupBoxRightMenu
            // 
            this.groupBoxRightMenu.ForeColor = System.Drawing.SystemColors.ControlText;
            this.groupBoxRightMenu.Location = new System.Drawing.Point(199, 13);
            this.groupBoxRightMenu.Name = "groupBoxRightMenu";
            this.groupBoxRightMenu.Size = new System.Drawing.Size(678, 371);
            this.groupBoxRightMenu.TabIndex = 1;
            this.groupBoxRightMenu.TabStop = false;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel2,
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel3});
            this.statusStrip1.Location = new System.Drawing.Point(0, 388);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(889, 22);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(168, 17);
            this.toolStripStatusLabel2.Text = "版权所属：大气团队研制       ";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(210, 17);
            this.toolStripStatusLabel1.Text = "当前系统时间：2020-07-19 10:49:56";
            // 
            // toolStripStatusLabel3
            // 
            this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            this.toolStripStatusLabel3.Size = new System.Drawing.Size(312, 17);
            this.toolStripStatusLabel3.Text = "       线程情况：可用辅助线程0个,异步I/O线程最大数0个";
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // NotifyMenu
            // 
            this.NotifyMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.显示ToolStripMenuItem,
            this.退出ToolStripMenuItem});
            this.NotifyMenu.Name = "NotifyMenu";
            this.NotifyMenu.Size = new System.Drawing.Size(101, 48);
            // 
            // 显示ToolStripMenuItem
            // 
            this.显示ToolStripMenuItem.Name = "显示ToolStripMenuItem";
            this.显示ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.显示ToolStripMenuItem.Text = "显示";
            this.显示ToolStripMenuItem.Click += new System.EventHandler(this.显示ToolStripMenuItem_Click);
            // 
            // 退出ToolStripMenuItem
            // 
            this.退出ToolStripMenuItem.Name = "退出ToolStripMenuItem";
            this.退出ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.退出ToolStripMenuItem.Text = "退出";
            this.退出ToolStripMenuItem.Click += new System.EventHandler(this.退出ToolStripMenuItem_Click);
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.ContextMenuStrip = this.NotifyMenu;
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Tag = "";
            this.notifyIcon1.Text = "监测数据采集";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // timer2
            // 
            this.timer2.Interval = 2000;
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // Fr_Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(889, 410);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.groupBoxRightMenu);
            this.Controls.Add(this.groupBoxLeftMenu);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Fr_Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "空气质量监测数据实时采集";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Fr_Main_FormClosing);
            this.Load += new System.EventHandler(this.Fr_Main_Load);
            this.SizeChanged += new System.EventHandler(this.Fr_Main_SizeChanged);
            this.groupBoxLeftMenu.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.NotifyMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxLeftMenu;
        private System.Windows.Forms.GroupBox groupBoxRightMenu;
        private System.Windows.Forms.Button dataConfigBtn;
        private System.Windows.Forms.Button dataCollectBtn;
        private System.Windows.Forms.Button aboutUsBtn;
        private System.Windows.Forms.Button lookLogBtn;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ContextMenuStrip NotifyMenu;
        private System.Windows.Forms.ToolStripMenuItem 显示ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 退出ToolStripMenuItem;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.Button dataSourceConfigBtn;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.Button historyCollectBtn;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel3;
        private System.Windows.Forms.Timer timer2;
    }
}

