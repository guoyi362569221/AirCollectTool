using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeatherDataSys.UtilTool;
using WeatherDataSys.UtilTool.IJobTool;

namespace WeatherDataSys
{
    public partial class From_Main : Form
    {
        public static RichTextBox rtb;

        public static TextBox address;

        public static TextBox dbServerIP;
        public static TextBox dbServerPort ;
        public static TextBox dbServerUserId ;
        public static TextBox dbServerUserPassword ;
        public static string providerName ;
        public static TextBox dbName ;

        public static bool isStopRun;


        //从工厂中获取一个调度器实例化
        IScheduler scheduler ;

        string jobName = "jobname";
        string tigerName = "tigername";
        string groupName = "groupname";

        public From_Main()
        {
            isStopRun = true;
            InitializeComponent();
            rtb = this.rtb_Log;

            address = this.txt_Address;

            dbServerIP = this.txt_DB_IP;
            dbServerPort = this.txt_DB_Port;
            dbServerUserId = this.txt_DB_UserName;
            dbServerUserPassword = this.txt_DB_Password;
            providerName = "PostgreSQL";
            dbName = this.txt_DB_DbName;

            

            notifyIcon1.Visible = true;
        }

        /// <summary>
        /// 开始爬取
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Start_Click(object sender, EventArgs e)
        {
            From_Main.CheckForIllegalCrossThreadCalls = false;

            this.rtb_Log.Clear();

            #region 条件判断
            if (this.txt_Address.Text.Trim() == "")
            {
                Utility.writeLog("⚠数据源地址不能为空",ColorEnum.Red);
                return;
            }
            if (this.txt_DB_IP.Text.Trim() == "")
            {
                Utility.writeLog("⚠数据库地址不能为空", ColorEnum.Red);
                return;
            }
            if (this.txt_DB_Port.Text.Trim() == "")
            {
                Utility.writeLog("⚠端口不能为空", ColorEnum.Red);
                return;
            }
            if (this.txt_DB_DbName.Text.Trim() == "")
            {
                Utility.writeLog("⚠数据库名称不能为空", ColorEnum.Red);
                return;
            }
            if (this.txt_DB_UserName.Text.Trim() == "")
            {
                Utility.writeLog("⚠用户名不能为空", ColorEnum.Red);
                return;
            }
            if (this.txt_DB_Password.Text.Trim() == "")
            {
                Utility.writeLog("⚠密码不能为空", ColorEnum.Red);
                return;
            }
            #endregion

            //读取站点信息表信息

            setControlStatus(false);
            Utility.writeLog("开始抓取...", ColorEnum.Blue);

            try
            {
                scheduler = StdSchedulerFactory.GetDefaultScheduler().Result;

                //开启调度器
                scheduler.Start();

                isStopRun = false;

                //先清除任务和触发器
                ClearJobTrigger();

                //触发器
                ITrigger trigger = TriggerBuilder.Create()
                                            .WithIdentity(tigerName, groupName)
                                            .StartNow()
                                            .WithCronSchedule("0 0/"+ nuD_min.Value + " * * * ?")
                                            //.WithCronSchedule("0 0/5 * * * ?")
                                            .Build();
                //任务
                IJobDetail job = JobBuilder.Create<LoadJob>()
                                .WithIdentity(jobName, groupName)
                                .UsingJobData("key", "value")
                                .Build();

                //启动
                scheduler.ScheduleJob(job, trigger);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 清除任务和触发器
        /// </summary>
        private void ClearJobTrigger()
        {
            TriggerKey triggerKey = new TriggerKey(tigerName, groupName);
            JobKey jobKey = new JobKey(jobName, groupName);
            scheduler.PauseTrigger(triggerKey);// 停止触发器  
            scheduler.UnscheduleJob(triggerKey);// 移除触发器  
            scheduler.DeleteJob(jobKey);// 删除任务  
        }

        /// <summary>
        /// 停止爬取
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Stop_Click(object sender, EventArgs e)
        {
            isStopRun = true;

            setControlStatus(true);

            Utility.writeLog("停止抓取...", ColorEnum.Blue);

            if (!scheduler.IsShutdown)
            {
                scheduler.Shutdown();
            }
        }

        /// <summary>
        /// 设置控件是否可用
        /// </summary>
        /// <param name="status"></param>
        private void setControlStatus(bool status)
        {
            this.txt_Address.Enabled = status;
            this.txt_DB_IP.Enabled = status;
            this.txt_DB_Port.Enabled = status;
            this.txt_DB_DbName.Enabled = status;
            this.txt_DB_UserName.Enabled = status;
            this.txt_DB_Password.Enabled = status;
            this.btn_Start.Enabled = status;
            this.btn_Stop.Enabled = !status;
            this.nuD_min.Enabled = status;
        }

        private void From_Main_SizeChanged(object sender, EventArgs e)
        {
            // 判断是否选择的是最小化按钮
            if (WindowState == FormWindowState.Minimized)
            {
                //隐藏任务栏区图标
                this.ShowInTaskbar = false;
                //图标显示在托盘区
                notifyIcon1.Visible = true;
            }
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                //还原窗体显示    
                WindowState = FormWindowState.Normal;
                //激活窗体并给予它焦点
                this.Activate();
                //任务栏区显示图标
                this.ShowInTaskbar = true;
                //托盘区图标隐藏
                notifyIcon1.Visible = false;
            }
        }

        private void From_Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.WindowState != FormWindowState.Minimized)
            {
                e.Cancel = true;//不关闭程序

                //最小化到托盘的时候显示图标提示信息，提示用户并未关闭程序
                this.WindowState = FormWindowState.Minimized;
                notifyIcon1.ShowBalloonTip(2000, "","最小到托盘",ToolTipIcon.None);
            }
        }

        private void 显示ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //还原窗体显示    
            WindowState = FormWindowState.Normal;
            //激活窗体并给予它焦点
            this.Activate();
            //任务栏区显示图标
            this.ShowInTaskbar = true;
            //托盘区图标隐藏
            notifyIcon1.Visible = false;
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("是否确认退出程序？", "退出", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                // 关闭所有的线程
                this.Dispose();
                this.Close();
            }
        }
    }
}
