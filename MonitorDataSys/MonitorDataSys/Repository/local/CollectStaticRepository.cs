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
    public class CollectStaticRepository
    {
        private readonly string dataBaseName = ConfigurationManager.AppSettings["configDataBaseName"];

        private readonly LogRepository lr = new LogRepository();

        /// <summary>
        /// 添加统计信息
        /// </summary>
        /// <param name="collectTotal"></param>
        /// <param name="collectTime"></param>
        /// <returns></returns>
        public bool AddStaticInfo(int collectTotal, DateTime collectTime)
        {
            JObject jdata = new JObject();

            jdata["CollectTime"] = collectTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
            jdata["CollectTotal"] = collectTotal;

            string tableName = "t_collect_static";
            if (jdata != null)
            {
                try
                {
                    StringBuilder sb = new StringBuilder();
                    dynamic json = jdata;
                    StringBuilder fieldname = new StringBuilder();
                    StringBuilder fieldvalue = new StringBuilder();

                    //获得对象的所有字段名
                    var itemProperties = ((JObject)jdata).Properties().ToList();
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
                                fieldvalue.AppendFormat("'{0}'", Convert.ToDateTime(itemProperties[i].Value.ToString()).ToString("yyyy-MM-dd HH:mm:ss.fff"));
                                break;
                            case JTokenType.Boolean:
                                break;
                            case JTokenType.Array:
                                break;
                        }
                        fieldname.AppendFormat("\"" + itemProperties[i].Name + "\"");
                        if (i < itemProperties.Count - 1)
                        {
                            fieldname.AppendFormat(",");
                            fieldvalue.AppendFormat(",");
                        }
                    }
                    sb.AppendFormat("insert into \"{0}\" ({1}) values ({2});", tableName, fieldname.ToString(), fieldvalue.ToString());
                    //string sql = SQLUtils.genarateSQL(sb.ToString(), SQLConnectionType.PostgreSQL);
                    SQLiteConnection conn = SQLiteHelper.GetConnection(dataBaseName);
                    int count = SQLiteHelper.ExecuteNonQuery(conn, sb.ToString(), null);
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
                    Loghelper.WriteErrorLog("添加统计信息失败", e);
                    lr.AddLogInfo(e.ToString(), "添加统计信息失败", "t_collect_static", "Error");
                    //throw e;
                }
            }
            return false;
        }

        /// <summary>
        /// 查询统计信息
        /// </summary>
        /// <returns></returns>
        public JObject GetCollectInfo()
        {
            string startTimeStr = DateTime.Now.ToString("yyyy-MM-dd 00:00:00.000");
            string endTimeStr = DateTime.Now.ToString("yyyy-MM-dd 23:59:59.999");

            JObject objItem = new JObject();
            objItem["allTotal"] = 0;
            objItem["todayTotal"] = 0;
            objItem["lastTime"] = '-';
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("select * from ( " +
                                    "(select sum(CollectTotal) as allTotal,max(CollectTime) as lastTime from t_collect_static) a " +
                                    "left join " +
                                    "(select sum(CollectTotal) as todayTotal from t_collect_static where CollectTime between '{0}' and '{1}') b " +
                                    "on 1 = 1 " +
                                    ")"
                                , startTimeStr, endTimeStr);

                SQLiteConnection conn = SQLiteHelper.GetConnection(dataBaseName);
                DataSet datasetTemp = SQLiteHelper.ExecuteDataSet(conn, sb.ToString(), null);
                if (datasetTemp != null)
                {
                    DataTable dt = datasetTemp.Tables[0];
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        objItem["allTotal"] = (string.IsNullOrEmpty(dt.Rows[0]["allTotal"].ToString()) ? "0" : dt.Rows[0]["allTotal"].ToString());
                        objItem["todayTotal"] = (string.IsNullOrEmpty(dt.Rows[0]["todayTotal"].ToString()) ? "0" : dt.Rows[0]["todayTotal"].ToString());
                        objItem["lastTime"] = (dt.Rows[0]["lastTime"] == null || string.IsNullOrEmpty(dt.Rows[0]["lastTime"].ToString()) ? "-" : Convert.ToDateTime(dt.Rows[0]["lastTime"]).ToString("yyyy-MM-dd HH:mm:ss.fff"));
                    }
                }
            }
            catch (Exception e)
            {
                //日志处理
                Loghelper.WriteErrorLog("查询统计信息失败", e);
                lr.AddLogInfo(e.ToString(), "查询统计信息失败", "t_collect_static", "Error");
                //throw e;
            }
            return objItem;
        }
    }
}
