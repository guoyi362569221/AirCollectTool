using MonitorDataSys.UtilTool;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonitorDataSys.Repository.local
{
    public class AreaRepository
    {
        private readonly string dataBaseName = ConfigurationManager.AppSettings["configDataBaseName"];
        private readonly LogRepository lr = new LogRepository();

        /// <summary>
        /// 查询区域信息
        /// </summary>
        /// <param name="areaCode"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        public DataTable AreaInfoQuery(string areaCode = "*", string level = "*")
        {
            DataTable dt = new DataTable();
            string areaCodeFilter = "";
            if (!string.IsNullOrEmpty(areaCode) && areaCode != "*")
            {
                areaCodeFilter = " and \"AreaCode\" like '" + areaCode + "%'";
            }
            string levelFilter = "";
            if (!string.IsNullOrEmpty(level) && level != "*")
            {
                levelFilter = " and \"Level\" in(" + Utility.ConvertFieldValue(level) + ")";
            }
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("select * from \"t_area\" where 1=1 {0} {1} " +
                            " order by \"AreaCode\" asc", areaCodeFilter, levelFilter);

                SQLiteConnection conn = SQLiteHelper.GetConnection(dataBaseName);
                DataSet datasetTemp = SQLiteHelper.ExecuteDataSet(conn, sb.ToString(), null);
                if (datasetTemp != null)
                {
                    dt = datasetTemp.Tables[0];
                }
            }
            catch (Exception e)
            {
                //日志处理
                Loghelper.WriteErrorLog("查询区域信息失败", e);
                lr.AddLogInfo(e.ToString(), "查询区域信息失败", "t_area", "Error");
                //throw e;
            }
            return dt;
        }
    }
}
