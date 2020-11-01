using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonitorDataSys.UtilTool.DataConvert
{
    public class HcDataConvert
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="areaCode"></param>
        /// <param name="csId"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static List<JObject> DataTableConvertList(string tableName,string areaCode, string csId, DataTable dt)
        {
            List<JObject> list = new List<JObject>();
            if (dt != null && dt.Rows.Count > 0)
            {
                list = Utility.DataTableToListJObject(tableName,dt, areaCode, csId);
            }
            return list;
        }
    }
}
