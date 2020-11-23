using MonitorDataSys.UtilTool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MonitorDataSys
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                //处理未捕获的异常
                Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
                //处理UI线程异常
                Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
                //处理非UI线程异常
                AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

                #region 应用程序的主入口点

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                //檢測系統是否有“XXXXX.vshost.exe”這一進程存在，如果已有，則不允許再打開。
                if (System.Diagnostics.Process.GetProcessesByName(System.Diagnostics.Process.GetCurrentProcess().ProcessName).Length > 1)
                {
                    MessageBox.Show("已经启动了一个数据采集工具！", "提示信息");
                }
                else
                {
                    Application.Run(new Fr_Main());
                }

                #endregion

            }
            catch (Exception ex)
            {
                string str = "";
                string strDateInfo = "出现应用程序未处理的异常：" + DateTime.Now.ToString() + "\r\n";

                if (ex != null)
                {
                    str = string.Format(strDateInfo + "异常类型：{0}\r\n异常消息：{1}\r\n异常信息：{2}\r\n",
                    ex.GetType().Name, ex.Message, ex.StackTrace);
                }
                else
                {
                    str = string.Format("应用程序线程错误:{0}", ex);
                }

                //MessageBox.Show(str, "系统错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            Loghelper.WriteErrorLog("Application_ThreadException", e.Exception);
            int wt;
            int ct;
            ThreadPool.GetAvailableThreads(out wt, out ct);
            while (true)
            {//循环处理，否则应用程序将会退出
                //if (glExitApp)
                //{//标志应用程序可以退出，否则程序退出后，进程仍然在运行
                //    LogHelper.Save("ExitApp");
                //    return;
                //}
                if (wt < 25)
                {
                    Thread.Sleep(5 * 60 * 1000);
                }
                else
                {
                    Thread.Sleep(30 * 1000);
                }
            };
            //MessageBox.Show(str, "系统错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Loghelper.WriteErrorLog("CurrentDomain_UnhandledException", e.ExceptionObject as Exception);
            int wt;
            int ct;
            ThreadPool.GetAvailableThreads(out wt, out ct);
            while (true)
            {//循环处理，否则应用程序将会退出
                //if (glExitApp)
                //{//标志应用程序可以退出，否则程序退出后，进程仍然在运行
                //    LogHelper.Save("ExitApp");
                //    return;
                //}
                if (wt < 25)
                {
                    Thread.Sleep(5 * 60 * 1000);
                }
                else
                {
                    Thread.Sleep(30 * 1000);
                }
            };
            //MessageBox.Show(str, "系统错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }


        ///// <summary>
        ///// 应用程序的主入口点。
        ///// </summary>
        //[STAThread]
        //static void Main()
        //{
        //    //设置应用程序处理异常方式：ThreadException处理
        //    Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
        //    //处理UI线程异常
        //    Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
        //    //处理非UI线程异常
        //    AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

        //    Application.EnableVisualStyles();
        //    Application.SetCompatibleTextRenderingDefault(false);

        //    //檢測系統是否有“XXXXX.vshost.exe”這一進程存在，如果已有，則不允許再打開。
        //    if (System.Diagnostics.Process.GetProcessesByName(System.Diagnostics.Process.GetCurrentProcess().ProcessName).Length > 1)
        //    {
        //        MessageBox.Show("已经启动了一个数据采集工具！", "提示信息");
        //    }
        //    else
        //    {
        //        Application.Run(new Fr_Main());
        //    }
        //}
    }
}
