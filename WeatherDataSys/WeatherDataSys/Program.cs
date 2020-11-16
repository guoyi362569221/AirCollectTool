using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WeatherDataSys
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //檢測系統是否有“XXXXX.vshost.exe”這一進程存在，如果已有，則不允許再打開。
            if (System.Diagnostics.Process.GetProcessesByName(System.Diagnostics.Process.GetCurrentProcess().ProcessName).Length > 1)
            {
                MessageBox.Show("已经启动了一个气象爬取工具！", "提示信息");
            }
            else
            {
                Application.Run(new From_Main());
            }
        }
    }
}
