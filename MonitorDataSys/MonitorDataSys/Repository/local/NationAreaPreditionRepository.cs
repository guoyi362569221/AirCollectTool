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
    public class NationAreaPreditionRepository
    {
        private readonly string dataBaseName = ConfigurationManager.AppSettings["configDataBaseName"];
        private readonly LogRepository lr = new LogRepository();

        /// <summary>
        /// 查询空气质量国家区域基础数据
        /// </summary>
        /// <param name="areaCode"></param>
        /// <returns></returns>
        public DataTable NationAreaInfoQuery(string areaCode = "*")
        {
            DataTable dt = new DataTable();
            string areaCodeFilter = "";
            if (!string.IsNullOrEmpty(areaCode) && areaCode != "*")
            {
                areaCodeFilter = " and \"AreaCode\" like '" + areaCode + "%'";
            }
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("select * from \"t_nation_prediction_area\" where 1=1 {0} " +
                            " order by \"AreaCode\" asc", areaCodeFilter);
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
                Loghelper.WriteErrorLog("查询查询空气质量国家区域基础数据失败", e);
                lr.AddLogInfo(e.ToString(), "查询查询空气质量国家区域基础数据失败", "t_nation_prediction_area", "Error");
                //throw e;
            }
            return dt;
        }

    }
}
