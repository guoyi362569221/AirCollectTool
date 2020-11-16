using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherDataSys.UtilTool
{
    public class Loghelper
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger("LogHelper");

        public static void WriteLog(string msg)
        {
            logger.Info(msg);
        }

        public static void WriteErrorLog(string msg, Exception ex)
        {
            logger.Error(msg, ex);
        }

    }
}
