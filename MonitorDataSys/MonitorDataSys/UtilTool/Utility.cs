using MonitorDataSys.Repository.local;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Reflection;
using System.Runtime.InteropServices;

namespace MonitorDataSys.UtilTool
{
    public class Utility
    {
        private static readonly string noDataValue = ConfigurationManager.AppSettings["noDataValue"];
        private static readonly string aqiLevelFormat = ConfigurationManager.AppSettings["aqiLevelFormat"];


        public static string ConvertValueOrgin(string value)
        {
            string result = value;
            if (String.IsNullOrEmpty(value) || value == "—" || value == "-999" || value == "9999.0")
            {
                value = noDataValue;
            }
            return value;
        }

        public static string AQILevelCovertInt(string aqiLevel)
        {
            string aqlLevelInt = aqiLevel;
            if (aqiLevelFormat == "int")
            {
                switch (aqiLevel)
                {
                    case "优":
                        aqlLevelInt = "1";
                        break;
                    case "良":
                        aqlLevelInt = "2";
                        break;
                    case "轻度污染":
                        aqlLevelInt = "3";
                        break;
                    case "中度污染":
                        aqlLevelInt = "4";
                        break;
                    case "重度污染":
                        aqlLevelInt = "5";
                        break;
                    case "严重污染":
                        aqlLevelInt = "6";
                        break;
                }
            }
            return aqlLevelInt;
        }

        /// <summary>
        /// 通过AQI值读取AQI的等级
        /// </summary>
        /// <param name="aqi">AQI值</param>
        /// <returns>level</returns>
        public static string GetPollutantLevel(int aqi)
        {
            string level = "-";
            if (aqi == null || aqi == -999)
            {
                level = "-";
            }
            else if (aqi <= 50 && aqi >= 0)
            {
                level = "优";
            }
            else if (aqi > 50 && aqi <= 100)
            {
                level = "良";
            }
            else if (aqi > 100 && aqi <= 150)
            {
                level = "轻度污染";
            }
            else if (aqi > 150 && aqi <= 200)
            {
                level = "中度污染";
            }
            else if (aqi > 200 && aqi <= 300)
            {
                level = "重度污染";
            }
            else
            {
                level = "严重污染";
            }

            return level;
        }

        public static string ConvertFieldValue(string fieldNames, string str = "'")
        {
            if (fieldNames.Contains(","))
            {
                string[] strArr = fieldNames.Split(',');
                for (int i = 0; i < strArr.Length; i++)
                {
                    strArr[i] = str + strArr[i] + str;
                }
                return string.Join(",", strArr);
            }
            else
            {
                return "'" + fieldNames + "'";
            }
        }

        /// <summary>
        /// 把Model转换为DataRow
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="mod"></param>
        /// <returns></returns>
        public static T ParseDictionaryToModel<T>(Dictionary<string, string> dict)
        {
            T obj = default(T);
            obj = Activator.CreateInstance<T>();

            //根据Key值设定 Columns
            foreach (KeyValuePair<string, string> item in dict)
            {
                PropertyInfo prop = obj.GetType().GetProperty(item.Key);
                if (prop != null && !string.IsNullOrEmpty(item.Value))
                {
                    object value = item.Value;
                    //Nullable 获取Model类字段的真实类型
                    Type itemType = Nullable.GetUnderlyingType(prop.PropertyType) == null ? prop.PropertyType : Nullable.GetUnderlyingType(prop.PropertyType);
                    //根据Model类字段的真实类型进行转换
                    prop.SetValue(obj, Convert.ChangeType(value, itemType), null);
                }

            }
            return obj;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static List<Dictionary<string, string>> DataTableToList(DataTable dt)
        {
            List<Dictionary<string, string>> result = new List<Dictionary<string, string>>();
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    try
                    {
                        Dictionary<string, string> dic = new Dictionary<string, string>();
                        for (int i = 0; i < dr.Table.Columns.Count; i++)
                        {
                            dic.Add(dr.Table.Columns[i].ColumnName.ToString(), dr[dr.Table.Columns[i].ColumnName].ToString());
                        }
                        result.Add(dic);
                    }
                    catch (Exception e)
                    {
                        //日志处理
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="areaCode"></param>
        /// <param name="csId"></param>
        /// <returns></returns>
        public static List<JObject> DataTableToListJObject(string tableName, DataTable dt, string areaCode, string csId)
        {
            List<JObject> result = new List<JObject>();
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    try
                    {
                        JObject item = new JObject();
                        for (int i = 0; i < dr.Table.Columns.Count; i++)
                        {
                            Type dataType = dr[dr.Table.Columns[i].ColumnName].GetType();
                            switch (dataType.Name)
                            {
                                case "String":
                                    item[dr.Table.Columns[i].ColumnName] = dr[dr.Table.Columns[i].ColumnName].ToString();
                                    break;
                                case "Int32":
                                    item[dr.Table.Columns[i].ColumnName] = Int32.Parse(dr[dr.Table.Columns[i].ColumnName].ToString());
                                    break;
                                case "Double":
                                    item[dr.Table.Columns[i].ColumnName] = Double.Parse(dr[dr.Table.Columns[i].ColumnName].ToString());
                                    break;
                                case "Float":
                                    item[dr.Table.Columns[i].ColumnName] = float.Parse(dr[dr.Table.Columns[i].ColumnName].ToString());
                                    break;
                                case "DateTime":
                                    item[dr.Table.Columns[i].ColumnName] = Convert.ToDateTime(dr[dr.Table.Columns[i].ColumnName]);
                                    break;
                                case "Decimal":
                                    item[dr.Table.Columns[i].ColumnName] = Decimal.Parse(dr[dr.Table.Columns[i].ColumnName].ToString());
                                    break;
                                case "DBNull":
                                    item[dr.Table.Columns[i].ColumnName] = null;
                                    break;
                                default:
                                    item[dr.Table.Columns[i].ColumnName] = dr[dr.Table.Columns[i].ColumnName].ToString();
                                    break;
                            }

                        }
                        item["AreaCode"] = areaCode;
                        item["CsId"] = csId;
                        switch (tableName)
                        {
                            case "YB_DeviceData":
                            case "PCJC_DeviceData":
                                break;
                            default:
                                if (item.Property("DataID") != null)
                                {
                                    item.Remove("DataID");
                                }
                                break;
                        }

                        if (item.Property("CreateUserID") != null)
                        {
                            item.Remove("CreateUserID");
                        }
                        if (item.Property("CreateTime") != null)
                        {
                            item.Remove("CreateTime");
                        }
                        if (item.Property("ModifyUserID") != null)
                        {
                            item.Remove("ModifyUserID");
                        }
                        if (item.Property("ModifyTime") != null)
                        {
                            item.Remove("ModifyTime");
                        }
                        if (item.Property("DV1") != null)
                        {
                            item.Remove("DV1");
                        }
                        if (item.Property("DV2") != null)
                        {
                            item.Remove("DV2");
                        }
                        if (item.Property("DV3") != null)
                        {
                            item.Remove("DV3");
                        }
                        if (item.Property("Id") != null)
                        {
                            item.Remove("Id");
                        }
                        result.Add(item);
                    }
                    catch (Exception ex)
                    {
                        LogRepository lr = new LogRepository();
                        //日志处理
                        Loghelper.WriteErrorLog("数据转换失败，此过程发生在读取设备数据后，录入标准库过程中", ex);
                        lr.AddLogInfo(ex.ToString(), "数据转换失败，此过程发生在读取设备数据后，录入标准库过程中", "数据转换失败，此过程发生在读取设备数据后，录入标准库过程中", "Error");
                    }
                }
            }
            return result;
        }

        #region 

        /// <summary>
        /// 内存回收
        /// </summary>
        /// <param name="process"></param>
        /// <param name="minSize"></param>
        /// <param name="maxSize"></param>
        /// <returns></returns>
        [DllImport("kernel32.dll", EntryPoint = "SetProcessWorkingSetSize")]
        public static extern int SetProcessWorkingSetSize(IntPtr process, int minSize, int maxSize);

        /// <summary>
        /// 释放内存
        /// </summary>
        public static void ClearMemory()
        {
            try
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                {
                    SetProcessWorkingSetSize(System.Diagnostics.Process.GetCurrentProcess().Handle, -1, -1);
                }
            }
            catch (Exception e)
            {
                Loghelper.WriteErrorLog("内存回收，释放内存失败", e);
            }
        }

        #endregion
    }

}
