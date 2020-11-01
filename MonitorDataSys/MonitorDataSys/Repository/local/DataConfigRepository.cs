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
    public class DataConfigRepository
    {
        private readonly string dataBaseName = ConfigurationManager.AppSettings["configDataBaseName"];

        private readonly LogRepository lr = new LogRepository();

        /// <summary>
        /// 查询配置信息
        /// </summary>
        /// <param name="dbIP"></param>
        /// <param name="dbPort"></param>
        /// <returns></returns>
        public DataTable DataConfigInfoQuery(string dbIP = "*", string dbPort = "*", string dbType = "*")
        {
            DataTable dt = new DataTable();
            string dbIPFilter = "";
            if (!string.IsNullOrEmpty(dbIP) && dbIP != "*")
            {
                dbIPFilter = " and \"DBIPAddress\" in(" + Utility.ConvertFieldValue(dbIP) + ")";
            }
            string dbPortFilter = "";
            if (!string.IsNullOrEmpty(dbPort) && dbPort != "*")
            {
                dbPortFilter = " and \"DBPort\" in(" + Utility.ConvertFieldValue(dbPort) + ")";
            }
            string dbTypeFilter = "";
            if (!string.IsNullOrEmpty(dbType) && dbType != "*")
            {
                dbTypeFilter = " and \"DBType\" in(" + Utility.ConvertFieldValue(dbType) + ")";
            }
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("select * from \"t_data_config\" where 1=1 {0} {1} {2}" +
                            " order by \"CreateTime\" desc", dbIPFilter, dbPortFilter, dbTypeFilter);

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
                Loghelper.WriteErrorLog("查询配置信息失败", e);
                lr.AddLogInfo(e.ToString(), "查询配置信息失败", "t_data_config", "Error");
                //throw e;
            }
            return dt;
        }

        /// <summary>
        /// 删除配置信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteDataConfigInfo(string id)
        {
            bool result = false;
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("delete from \"t_data_config\" where 1=1 and Id ={0}", id);

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
                Loghelper.WriteErrorLog("删除配置信息失败", e);
                lr.AddLogInfo(e.ToString(), "删除配置信息失败", "t_data_config", "Error");
                //throw e;
            }
            return result;
        }

        /// <summary>
        /// 查询配置信息
        /// </summary>
        /// <param name="areaCode"></param>
        /// <param name="csCode"></param>
        /// <returns></returns>
        public ConnectionStringSettings ConnnectConfigQuery(string areaCode, string csCode)
        {
            ConnectionStringSettings conn = null;
            try
            {
                DataTable dt = DataConfigInfoQuery(areaCode, csCode);
                if (dt != null && dt.Rows.Count > 0)
                {
                    string bzkSQLServerDbServerIP = dt.Rows[0]["DBIPAddress"].ToString();
                    string bzkSQLServerDbServerPort = dt.Rows[0]["DBPort"].ToString();
                    string bzkSQLServerDbServerUserId = dt.Rows[0]["DBUserName"].ToString();
                    string bzkSQLServerDbServerUserPassword = dt.Rows[0]["DBPassword"].ToString();
                    string bzkSQLServerProviderName = dt.Rows[0]["DBType"].ToString();
                    string bzkSQLServerDbName = dt.Rows[0]["DBName"].ToString();

                    DBHelper dbHelper = new DBHelper();
                    //conn = dbHelper.GetCustomSQLServerConnection(bzkSQLServerDbServerIP, bzkSQLServerDbServerPort, bzkSQLServerDbServerUserId, bzkSQLServerDbServerUserPassword, bzkSQLServerProviderName, bzkSQLServerDbName);
                }

            }
            catch (Exception e)
            {
                //日志处理
                Loghelper.WriteErrorLog("查询配置信息失败", e);
                //throw e;
            }
            return conn;
        }

        /// <summary>
        /// 新增配置数据源
        /// </summary>
        /// <param name="jdata"></param>
        /// <returns></returns>
        public bool AddDataConfigInfo(JObject jdata)
        {
            string tableName = "t_data_config";
            if (jdata != null)
            {
                try
                {
                    jdata["CreateTime"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
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
                    Loghelper.WriteErrorLog("新增配置数据源失败", e);
                    //throw e;
                }
            }
            return false;
        }
    }
}
