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
    public class AirStationRepository
    {
        private readonly string dataBaseName = ConfigurationManager.AppSettings["configDataBaseName"];
        private readonly LogRepository lr = new LogRepository();

        /// <summary>
        /// 查询空气质量站点数据
        /// </summary>
        /// <param name="cityCode"></param>
        /// <returns></returns>
        public DataTable AirStationInfoQuery(string cityCode = "*")
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
                sb.AppendFormat("select * from \"t_air_station\" where 1=1 {0} " +
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
                Loghelper.WriteErrorLog("查询空气质量站点数据失败", e);
                lr.AddLogInfo(e.ToString(), "查询空气质量站点数据失败", "t_area", "Error");
                //throw e;
            }
            return dt;
        }

        /// <summary>
        /// 查询空气质量站点对应区域数据
        /// </summary>
        /// <param name="cityCode"></param>
        /// <returns></returns>
        public DataTable AirAreaInfoQuery(string cityCode = "*") 
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
                sb.AppendFormat("select distinct(CityCode),Area from \"t_air_station\" where 1=1 {0} " +
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
                Loghelper.WriteErrorLog("查询空气质量站点对应区域数据失败", e);
                lr.AddLogInfo(e.ToString(), "查询空气质量站点对应区域数据失败", "t_area", "Error");
                //throw e;
            }
            return dt;
        }

        /// <summary>
        /// 空气质量站点数据录入
        /// </summary>
        /// <param name="jdatas"></param>
        /// <returns></returns>
        public bool AddAirStationInfo(List<JObject> jdatas)
        {
            string tableName = "t_air_station";
            try
            {
                int size = 500;
                int count = 0;
                int totalTimes = (jdatas.Count + size - 1) / size;
                for (int t = 0; t < totalTimes; t++)
                {
                    int startIndex = t * size;
                    int endIndex = ((t + 1) * size - 1) < (jdatas.Count-1) ? ((t + 1) * size - 1) : (jdatas.Count - 1);

                    StringBuilder sb = new StringBuilder();
                    for (int k = startIndex; k <= endIndex; k++)
                    {
                        StringBuilder fieldname = new StringBuilder();
                        StringBuilder fieldvalue = new StringBuilder();

                        //获得对象的所有字段名
                        var itemProperties = (jdatas[k]).Properties().ToList();
                        for (int i = 0; i < itemProperties.Count; i++)
                        {
                            //var v = item.Name + ":" + item.Value;
                            switch (itemProperties[i].Value.Type)
                            {
                                case JTokenType.Integer:
                                    fieldvalue.AppendFormat(itemProperties[i].Value.ToString());
                                    break;
                                case JTokenType.Float:
                                    fieldvalue.AppendFormat(itemProperties[i].Value.ToString());
                                    break;
                                case JTokenType.String:
                                    fieldvalue.AppendFormat("'" + itemProperties[i].Value.ToString() + "'");
                                    break;
                                case JTokenType.Date:
                                    fieldvalue.AppendFormat("'{0}'", Convert.ToDateTime(itemProperties[i].Value).ToString("yyyy-MM-dd HH:mm:ss.fff"));
                                    break;
                                case JTokenType.Null:
                                    //fieldvalue.AppendFormat(null);
                                    break;
                                default:
                                    fieldvalue.AppendFormat("'" + itemProperties[i].Value.ToString() + "'");
                                    break;
                            }

                            if (itemProperties[i].Value.Type != JTokenType.Null)
                            {
                                fieldname.AppendFormat("\"" + itemProperties[i].Name + "\"");
                                if (i < itemProperties.Count - 1)
                                {
                                    fieldname.AppendFormat(",");
                                    fieldvalue.AppendFormat(",");
                                }
                            }
                        }
                        sb.AppendFormat("insert into \"{0}\" ({1}) values ({2});", tableName, fieldname.ToString(), fieldvalue.ToString());
                    }

                    SQLiteConnection conn = SQLiteHelper.GetConnection(dataBaseName);
                    count += SQLiteHelper.ExecuteNonQuery(conn, sb.ToString(), null);
                }

                if (count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                //日志处理
                Loghelper.WriteErrorLog("空气质量站点数据录入失败", e);
                lr.AddLogInfo(e.ToString(), "空气质量站点数据录入失败", tableName, "Error");
                //throw e;
            }
            return false;
        }

        /// <summary>
        /// 删除空气质量站点数据
        /// </summary>
        /// <returns></returns>
        public bool DeleteAirStationInfo()
        {
            bool result = false;
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("delete from \"t_air_station\" where 1=1 ");

                SQLiteConnection conn = SQLiteHelper.GetConnection(dataBaseName);
                int count = SQLiteHelper.ExecuteNonQuery(conn, sb.ToString(), null);
                if (count > 0)
                {
                    result = true;
                }
            }
            catch (Exception e)
            {
                //日志处理
                Loghelper.WriteErrorLog("删除空气质量站点数据失败", e);
                lr.AddLogInfo(e.ToString(), "删除空气质量站点数据失败", "t_air_station", "Error");
                //throw e;
            }
            return result;
        }
    }
}
