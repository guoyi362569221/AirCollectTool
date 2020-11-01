using MonitorDataSys.Repository.local;
using MonitorDataSys.UtilTool;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonitorDataSys.Repository.bzk
{
    public class StationMonitorDayRepository
    {
        private readonly LogRepository lr = new LogRepository();

        private DBHelper dbHelper;

        private ConnectionStringSettings tempSQLCoonectStr = new ConnectionStringSettings();

        /// <summary>
        /// 构造函数用于定位当前数据库 和初始化数据库连接设置
        /// </summary>
        public StationMonitorDayRepository(string bzkSQLServerDbServerIP, string bzkSQLServerDbServerPort, string bzkSQLServerDbServerUserId, string bzkSQLServerDbServerUserPassword, string bzkSQLServerProviderName, string bzkSQLServerDbName)
        {
            this.dbHelper = new DBHelper();
            tempSQLCoonectStr = dbHelper.GetSQLConnection(bzkSQLServerDbServerIP, bzkSQLServerDbServerPort, bzkSQLServerDbServerUserId, bzkSQLServerDbServerUserPassword, bzkSQLServerProviderName, bzkSQLServerDbName);
        }

        /// <summary>
        /// 查询数据表中最新数据时间
        /// </summary>
        /// <returns></returns>
        public bool IsCompeletCollect(string tableName, string stationCode, DateTime monitorTime)
        {
            bool result = false;
            try
            {
                this.dbHelper = new DBHelper(tempSQLCoonectStr);
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("select * from {0} where 1=1 and SITE_CODE ='{1}' and MONITOR_TIME = '{2}' ", tableName, stationCode, monitorTime.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                string sql = SQLUtils.genarateSQL(sb.ToString(), this.dbHelper.sqlConnectionType);
                DataSet datasetTemp = dbHelper.DataAdapter(CommandType.Text, sql);
                if (datasetTemp != null)
                {
                    DataTable dt = datasetTemp.Tables[0];
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        result = true;
                    }
                }
            }
            catch (Exception e)
            {
                //日志处理
                Loghelper.WriteErrorLog("查询数据表中最新数据时间失败", e);
                lr.AddLogInfo(e.ToString(), "查询数据表中最新数据时间失败", tableName, "Error");
                //throw e;
            }
            return result;
        }

        /// <summary>
        /// 设备采集数据录入
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="jdatas"></param>
        /// <returns></returns>
        public bool AddDataInfo(string tableName, List<JObject> jdatas)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                for (int k = 0; k < jdatas.Count; k++)
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
                            //fieldname.AppendFormat("\"" + itemProperties[i].Name + "\"");
                            fieldname.AppendFormat(itemProperties[i].Name);
                            if (i < itemProperties.Count - 1)
                            {
                                fieldname.AppendFormat(",");
                                fieldvalue.AppendFormat(",");
                            }
                        }
                    }
                    sb.AppendFormat("insert into {0} ({1}) values ({2});", tableName, fieldname.ToString(), fieldvalue.ToString());
                }
                this.dbHelper = new DBHelper(tempSQLCoonectStr);
                string sql = SQLUtils.genarateSQL(sb.ToString(), this.dbHelper.sqlConnectionType);
                int count = dbHelper.ExecuteNonQuery(CommandType.Text, sql);
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
                Loghelper.WriteErrorLog("设备采集数据录入失败", e);
                lr.AddLogInfo(e.ToString(), "设备采集数据录入失败", tableName, "Error");
                //throw e;
            }
            return false;
        }
    }
}
