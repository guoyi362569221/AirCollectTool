using MonitorDataSys.UtilTool;
using Newtonsoft.Json.Linq;
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
    public class WeatherStationRepository
    {
        private readonly string dataBaseName = ConfigurationManager.AppSettings["configDataBaseName"];
        private readonly LogRepository lr = new LogRepository();

        /// <summary>
        /// 查询气象站点数据
        /// </summary>
        /// <param name="cityCode"></param>
        /// <returns></returns>
        public DataTable StationInfoQuery(string cityCode = "*")
        {
            DataTable dt = new DataTable();
            string cityCodeFilter = "";
            if (!string.IsNullOrEmpty(cityCode) && cityCode != "*")
            {
                cityCodeFilter = " and \"CityCode\" like '" + cityCode + "%'";
            }
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("select * from \"t_weather_station\" where 1=1 {0} " +
                            " order by \"CityCode\" asc", cityCodeFilter);

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
                Loghelper.WriteErrorLog("查询气象站点数据失败", e);
                lr.AddLogInfo(e.ToString(), "查询气象站点数据失败", "t_weather_station", "Error");
                //throw e;
            }
            return dt;
        }
        
    }
}
