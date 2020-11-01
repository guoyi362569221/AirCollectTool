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
    public class LogRepository
    {
        private readonly string dataBaseName = ConfigurationManager.AppSettings["configDataBaseName"];

        /// <summary>
        /// 添加日志信息
        /// </summary>
        /// <param name="content"></param>
        /// <param name="areaCode"></param>
        /// <param name="dataTableName"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool AddLogInfo(string content,string areaCode,string dataTableName,string type)
        {
            JObject jdata = new JObject();

            jdata["Content"] = content;
            jdata["CreateTime"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            jdata["AreaCode"] = areaCode;
            jdata["TableName"] = dataTableName;
            jdata["Type"] = type;

            string tableName = "t_log_info";
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
                                fieldvalue.AppendFormat("'{0}'", Convert.ToDateTime(itemProperties[i].Value).ToString("yyyy-MM-dd HH:mm:ss.fff"));
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
                    Loghelper.WriteErrorLog("添加日志信息失败", e);
                    //throw e;
                }
            }
            return false;
        }

        /// <summary>
        /// 查询日志信息
        /// </summary>
        /// <param name="startTimeStr"></param>
        /// <param name="endTimeStr"></param>
        /// <returns></returns>
        public DataTable LogInfoQuery(string startTimeStr = "*", string endTimeStr = "*")
        {
            DataTable dt = new DataTable();
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("select * from \"t_log_info\" where 1=1 and CreateTime between '{0}' and '{1}' " +
                            " order by \"CreateTime\" desc", DateTime.Parse(startTimeStr).ToString("yyyy-MM-dd 00:00:00.000"), Convert.ToDateTime(endTimeStr).ToString("yyyy-MM-dd 23:59:59.999"));

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
                Loghelper.WriteErrorLog("查询日志信息失败", e);
                //throw e;
            }
            return dt;
        }
    }
}
