using MonitorDataSys.Repository;
using MonitorDataSys.Repository.local;
using MonitorDataSys.UtilTool;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MonitorDataSys
{
    public partial class Fr_Main : Form
    {
        private FrDataSourceConfig frDataSourceConfig = new FrDataSourceConfig();
        private FrDataConfig frDataConfig = new FrDataConfig();
        private FrDataCollect frDataCollect = new FrDataCollect();
        private FrLookLog frLookLog = new FrLookLog();
        private FrAboutAs frAboutAs = new FrAboutAs();
        private FrHistoryDataCollect frHistoryDataCollect = new FrHistoryDataCollect();

        private readonly LogRepository lr = new LogRepository();

        public Fr_Main()
        {
            InitializeComponent();

            try
            {
                Form[] forms = { frDataSourceConfig, frDataConfig, frDataCollect, frLookLog, frAboutAs, frHistoryDataCollect };
                for (int i = 0; i < forms.Length; i++)
                {
                    forms[i].TopLevel = false;
                    forms[i].Dock = DockStyle.Fill;//把子窗体设置为控件
                    forms[i].Hide();

                    forms[i].FormBorderStyle = FormBorderStyle.None;
                    groupBoxRightMenu.Controls.Add(forms[i]);
                }

                dataSourceConfigBtn_Click(null, null);
            }
            catch (Exception ex)
            {
                //日志处理
                Loghelper.WriteErrorLog("捕获异常信息", ex);
                lr.AddLogInfo(ex.ToString(), "捕获异常信息", "捕获异常信息", "Error");
            }
        }

        private void Fr_Main_Load(object sender, EventArgs e)
        {
            try
            {
                timer1.Start();
            }
            catch (Exception ex)
            {
                //日志处理
                Loghelper.WriteErrorLog("捕获异常信息", ex);
                lr.AddLogInfo(ex.ToString(), "捕获异常信息", "捕获异常信息", "Error");
            }
        }

        //切换到配置数据源
        private void dataSourceConfigBtn_Click(object sender, EventArgs e)
        {

            try
            {
                setMenuStyle("dataSourceConfigBtn");
                setTabContentShow(frDataSourceConfig);
            }
            catch (Exception ex)
            {
                //日志处理
                Loghelper.WriteErrorLog("捕获异常信息", ex);
                lr.AddLogInfo(ex.ToString(), "捕获异常信息", "捕获异常信息", "Error");
            }
        }

        //切换到同步基础
        private void dataConfigBtn_Click(object sender, EventArgs e)
        {
            try
            {
                setMenuStyle("dataConfigBtn");
                setTabContentShow(frDataConfig);
            }
            catch (Exception ex)
            {
                //日志处理
                Loghelper.WriteErrorLog("捕获异常信息", ex);
                lr.AddLogInfo(ex.ToString(), "捕获异常信息", "捕获异常信息", "Error");
            }
        }

        //切换到关于我们
        private void aboutUsBtn_Click(object sender, EventArgs e)
        {
            try
            {
                setMenuStyle("aboutUsBtn");
                setTabContentShow(frAboutAs);
            }
            catch (Exception ex)
            {
                //日志处理
                lr.AddLogInfo(ex.ToString(), "捕获异常信息", "捕获异常信息", "Error");
            }
        }

        //切换到日志查看
        private void lookLogBtn_Click(object sender, EventArgs e)
        {
            try
            {
                setMenuStyle("lookLogBtn");
                setTabContentShow(frLookLog);
            }
            catch (Exception ex)
            {
                //日志处理
                Loghelper.WriteErrorLog("捕获异常信息", ex);
                lr.AddLogInfo(ex.ToString(), "捕获异常信息", "捕获异常信息", "Error");
            }
        }

        //切换到数据采集
        private void dataCollectBtn_Click(object sender, EventArgs e)
        {
            try
            {
                setMenuStyle("dataCollectBtn");
                setTabContentShow(frDataCollect);
            }
            catch (Exception ex)
            {
                //日志处理
                Loghelper.WriteErrorLog("捕获异常信息", ex);
                lr.AddLogInfo(ex.ToString(), "捕获异常信息", "捕获异常信息", "Error");
            }
        }

        //计时器
        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                toolStripStatusLabel1.Text = "当前系统时间：" + System.DateTime.Now.ToString();
                int wt;
                int ct;
                ThreadPool.GetAvailableThreads(out wt, out ct);
                toolStripStatusLabel3.Text = "       线程情况：可用辅助线程" + wt + "个,异步I/O线程最大数" + ct+"个";
            }
            catch (Exception ex)
            {
                //日志处理
                Loghelper.WriteErrorLog("捕获异常信息", ex);
                lr.AddLogInfo(ex.ToString(), "捕获异常信息", "捕获异常信息", "Error");
            }
        }

        //切换菜单时修改菜单样式
        private void setMenuStyle(string selectedBtnName)
        {
            try
            {
                switch (selectedBtnName)
                {
                    case "dataSourceConfigBtn":
                        dataSourceConfigBtn.BackColor = Color.FromArgb(191, 205, 219);
                        dataConfigBtn.BackColor = Color.FromArgb(240, 240, 240);
                        dataCollectBtn.BackColor = Color.FromArgb(240, 240, 240);
                        lookLogBtn.BackColor = Color.FromArgb(240, 240, 240);
                        aboutUsBtn.BackColor = Color.FromArgb(240, 240, 240);
                        historyCollectBtn.BackColor = Color.FromArgb(240, 240, 240);
                        break;
                    case "dataConfigBtn":
                        dataSourceConfigBtn.BackColor = Color.FromArgb(240, 240, 240);
                        dataConfigBtn.BackColor = Color.FromArgb(191, 205, 219);
                        dataCollectBtn.BackColor = Color.FromArgb(240, 240, 240);
                        lookLogBtn.BackColor = Color.FromArgb(240, 240, 240);
                        aboutUsBtn.BackColor = Color.FromArgb(240, 240, 240);
                        historyCollectBtn.BackColor = Color.FromArgb(240, 240, 240);
                        break;
                    case "dataCollectBtn":
                        dataSourceConfigBtn.BackColor = Color.FromArgb(240, 240, 240);
                        dataCollectBtn.BackColor = Color.FromArgb(191, 205, 219);
                        dataConfigBtn.BackColor = Color.FromArgb(240, 240, 240);
                        lookLogBtn.BackColor = Color.FromArgb(240, 240, 240);
                        aboutUsBtn.BackColor = Color.FromArgb(240, 240, 240);
                        historyCollectBtn.BackColor = Color.FromArgb(240, 240, 240);
                        break;
                    case "historyCollectBtn":
                        dataSourceConfigBtn.BackColor = Color.FromArgb(240, 240, 240);
                        dataCollectBtn.BackColor = Color.FromArgb(240, 240, 240);
                        dataConfigBtn.BackColor = Color.FromArgb(240, 240, 240);
                        lookLogBtn.BackColor = Color.FromArgb(240, 240, 240);
                        aboutUsBtn.BackColor = Color.FromArgb(240, 240, 240);
                        historyCollectBtn.BackColor = Color.FromArgb(191, 205, 219);
                        break;
                    case "lookLogBtn":
                        dataSourceConfigBtn.BackColor = Color.FromArgb(240, 240, 240);
                        lookLogBtn.BackColor = Color.FromArgb(191, 205, 219);
                        dataConfigBtn.BackColor = Color.FromArgb(240, 240, 240);
                        dataCollectBtn.BackColor = Color.FromArgb(240, 240, 240);
                        aboutUsBtn.BackColor = Color.FromArgb(240, 240, 240);
                        historyCollectBtn.BackColor = Color.FromArgb(240, 240, 240);
                        break;
                    case "aboutUsBtn":
                        dataSourceConfigBtn.BackColor = Color.FromArgb(240, 240, 240);
                        aboutUsBtn.BackColor = Color.FromArgb(191, 205, 219);
                        dataConfigBtn.BackColor = Color.FromArgb(240, 240, 240);
                        dataCollectBtn.BackColor = Color.FromArgb(240, 240, 240);
                        lookLogBtn.BackColor = Color.FromArgb(240, 240, 240);
                        historyCollectBtn.BackColor = Color.FromArgb(240, 240, 240);
                        break;

                }
            }
            catch (Exception ex)
            {
                //日志处理
                Loghelper.WriteErrorLog("捕获异常信息", ex);
                lr.AddLogInfo(ex.ToString(), "捕获异常信息", "捕获异常信息", "Error");
            }
        }

        //设置菜单对应内容面板显示
        private void setTabContentShow(Form form)
        {
            try
            {
                if (groupBoxRightMenu.Controls != null)
                {
                    for (int i = 0; i < groupBoxRightMenu.Controls.Count; i++)
                    {
                        if (groupBoxRightMenu.Controls[i] == form)
                        {
                            groupBoxRightMenu.Controls[i].Show();
                        }
                        else
                        {
                            groupBoxRightMenu.Controls[i].Hide();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //日志处理
                Loghelper.WriteErrorLog("捕获异常信息", ex);
                lr.AddLogInfo(ex.ToString(), "捕获异常信息", "捕获异常信息", "Error");
            }
        }

        #region 退出最小到托盘

        private void Fr_Main_SizeChanged(object sender, EventArgs e)
        {
            try
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
            catch (Exception ex)
            {
                //日志处理
                Loghelper.WriteErrorLog("捕获异常信息", ex);
                lr.AddLogInfo(ex.ToString(), "捕获异常信息", "捕获异常信息", "Error");
            }
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
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
            catch (Exception ex)
            {
                //日志处理
                Loghelper.WriteErrorLog("捕获异常信息", ex);
                lr.AddLogInfo(ex.ToString(), "捕获异常信息", "捕获异常信息", "Error");
            }
        }

        private void Fr_Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (this.WindowState != FormWindowState.Minimized)
                {
                    e.Cancel = true;//不关闭程序

                    //最小化到托盘的时候显示图标提示信息，提示用户并未关闭程序
                    this.WindowState = FormWindowState.Minimized;
                    notifyIcon1.ShowBalloonTip(2000, "", "最小到托盘", ToolTipIcon.None);
                }
            }
            catch (Exception ex)
            {
                //日志处理
                Loghelper.WriteErrorLog("捕获异常信息", ex);
                lr.AddLogInfo(ex.ToString(), "捕获异常信息", "捕获异常信息", "Error");
            }
        }

        private void 显示ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
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
            catch (Exception ex)
            {
                //日志处理
                Loghelper.WriteErrorLog("捕获异常信息", ex);
                lr.AddLogInfo(ex.ToString(), "捕获异常信息", "捕获异常信息", "Error");
            }
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("是否确认退出程序，退出后将停止采集？", "退出", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    // 关闭所有的线程
                    this.Dispose();
                    this.Close();
                    if (frDataCollect != null)
                    {
                        frDataCollect.ClearJobTrigger();
                    }
                }
            }
            catch (Exception ex)
            {
                //日志处理
                Loghelper.WriteErrorLog("捕获异常信息", ex);
                lr.AddLogInfo(ex.ToString(), "捕获异常信息", "捕获异常信息", "Error");
            }
        }

        #endregion

        private void historyCollectBtn_Click(object sender, EventArgs e)
        {
            try
            {
                setMenuStyle("historyCollectBtn");
                setTabContentShow(frHistoryDataCollect);
            }
            catch (Exception ex)
            {
                //日志处理
                Loghelper.WriteErrorLog("捕获异常信息", ex);
                lr.AddLogInfo(ex.ToString(), "捕获异常信息", "捕获异常信息", "Error");
            }
        }
    }
}
