using MonitorDataSys.Repository.local;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonitorDataSys.UtilTool.DataConvert
{
    public class JkDataConvert
    {
        private static readonly LogRepository lr = new LogRepository();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="areaCode"></param>
        /// <param name="csId"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static List<JObject> DataTableConvertList(string areaCode, string csId, DataTable dt)
        {
            List<JObject> list = new List<JObject>();
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    try
                    {
                        JObject item = new JObject();
                        item["DataID"] = dr["DID"].ToString();
                        item["AreaCode"] = areaCode;
                        item["CsId"] = csId;
                        item["DeviceInnerID"] = dr["SID"].ToString();
                        item["DacTime"] = Convert.ToDateTime(dr["DataTime"]);
                        item["V1"] = (dr["R1"] == null ? null : dr["R1"].ToString());
                        item["V2"] = (dr["R2"] == null ? null : dr["R2"].ToString());
                        list.Add(item);
                    }
                    catch (Exception e)
                    {
                        //日志处理
                        Loghelper.WriteErrorLog("基康应力应变数据转换失败", e);
                        lr.AddLogInfo(e.ToString(), areaCode, "基康应力应变数据转换失败", "Error");
                        //throw e;
                    }
                }
            }
            return list;
        }
    }
}
