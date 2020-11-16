using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherDataSys.Model;
using WeatherDataSys.UtilTool;
using static WeatherDataSys.From_Main;

namespace WeatherDataSys.Repository
{
    public class StationInfoRepository
    {
        private DBHelper dbHelper;

        /// <summary>
        /// 构造函数用于定位当前数据库 和初始化数据库连接设置
        /// </summary>
        public StationInfoRepository()
        {
            this.dbHelper = new DBHelper(From_Main.dbServerIP.Text, From_Main.dbServerPort.Text, From_Main.dbServerUserId.Text, From_Main.dbServerUserPassword.Text, From_Main.providerName, From_Main.dbName.Text);
        }
        /// <summary>
        /// 查询站点相关信息
        /// </summary>
        /// <param name="code">站点code代码，以逗号分隔</param>
        /// <returns></returns>
        public DataTable StationInfoQuery(string code = "*")
        {
            DataTable dt = new DataTable();
            string codeFilter = "";
            if (!string.IsNullOrEmpty(code) && code != "*")
            {
                codeFilter = " where \"区站号\" in(" + Utility.ConvertFieldValue(code) + ")";
            }
            try
            {
                this.dbHelper = new DBHelper(From_Main.dbServerIP.Text, From_Main.dbServerPort.Text, From_Main.dbServerUserId.Text, From_Main.dbServerUserPassword.Text, From_Main.providerName, From_Main.dbName.Text);
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("SELECT * from \"t_station\" {0}" +
                            " order by \"区站号\" asc", codeFilter);
                string sql = SQLUtils.genarateSQL(sb.ToString(), this.dbHelper.sqlConnectionType);
                DataSet datasetTemp = dbHelper.DataAdapter(CommandType.Text, sql);
                if (datasetTemp != null)
                {
                    dt = datasetTemp.Tables[0];
                }
                return dt;
            }
            catch (Exception e)
            {
                Loghelper.WriteErrorLog("查询站点关联信息失败", e);
                throw e;
            }
        }

        /// <summary>
        /// 气象数据
        /// </summary>
        /// <param name="startTimeStr">起始时间 格式：2018-11-27 00:00:00，默认*</param>
        /// <param name="endTimeStr">终止时间 格式：2018-11-27 00:00:00，默认*</param>
        /// <param name="code">产品唯一编号，支持单个、多个、全部，默认是全部</param>
        /// <returns></returns>
        public DataTable WeatherDataQuery(string startTimeStr, string endTimeStr, string code = "*")
        {
            string tableName = "t_weather";
            DataTable dt = new DataTable();
            try
            {
                string codeFilter = "";
                if (!String.IsNullOrEmpty(code) && code != "*")
                {
                    codeFilter = " and  \"stationcode\" in(" + Utility.ConvertFieldValue(code) + ")";
                }

                DateTime startTime = DateTime.Parse(startTimeStr);
                DateTime endTime = DateTime.Parse(endTimeStr);

                this.dbHelper = new DBHelper(From_Main.dbServerIP.Text, From_Main.dbServerPort.Text, From_Main.dbServerUserId.Text, From_Main.dbServerUserPassword.Text, From_Main.providerName, From_Main.dbName.Text);
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat(
                    "select * from \"{0}\" where 1=1 {1} and \"time\" between '{2}' and '{3}' order by \"stationcode\",\"time\" asc", tableName, codeFilter, startTime, endTime);
                string sql = SQLUtils.genarateSQL(sb.ToString(), this.dbHelper.sqlConnectionType);
                DataSet datasetTemp2 = dbHelper.DataAdapter(CommandType.Text, sql);
                if (datasetTemp2 != null)
                {
                    dt = datasetTemp2.Tables[0];
                }
            }
            catch (Exception e)
            {
                Loghelper.WriteErrorLog("数据查询失败", e);
                throw e;
            }
            return dt;
        }

        /// <summary>
        /// 添加气象数据
        /// </summary>
        /// <param name="weather">[]</param>
        /// <returns></returns>
        public void AddWeatherDatas(List<WeatherModel> weathers)
        {
            string tableName = "t_weather";
            try
            {
                this.dbHelper = new DBHelper(From_Main.dbServerIP.Text, From_Main.dbServerPort.Text, From_Main.dbServerUserId.Text, From_Main.dbServerUserPassword.Text, From_Main.providerName, From_Main.dbName.Text);
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < weathers.Count; i++)
                {
                    sb.AppendFormat("insert into \"{0}\"(time, stationcode, rain1h, rain24h, rain12h, rain6h, temperature, humidity, pressure, winddirection, windspeed) VALUES('{1}', '{2}', {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11});", tableName, weathers[i].time, weathers[i].stationcode, weathers[i].rain1h, weathers[i].rain24h, weathers[i].rain12h, weathers[i].rain6h, weathers[i].temperature, weathers[i].humidity, weathers[i].pressure, weathers[i].windDirection, weathers[i].windSpeed);
                }
                string sql = SQLUtils.genarateSQL(sb.ToString(), this.dbHelper.sqlConnectionType);
                int count = dbHelper.ExecuteNonQuery(CommandType.Text, sql);
            }
            catch (Exception e)
            {
                Loghelper.WriteErrorLog("添加气象数据失败", e);
                throw e;
            }
        }
    }
}
