using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonitorDataSys.UtilTool
{
    public class AQICalculateHelper
    {
        private static readonly string noDataValue = ConfigurationManager.AppSettings["noDataValue"];

        /// <summary>
        /// 空气质量分数和浓度界限
        /// </summary>
        public static readonly Dictionary<string, double[]> AirIAQI = new Dictionary<string, double[]>
        {
            {"iaqi", new double[]{0, 50, 100, 150, 200, 300, 400, 500}},
            {"so2_hour", new double[]{0.0, 150.0, 500.0, 650.0, 800.0, 1600.0, 2100.0, 2620.0}},
            {"so2_day", new double[]{0.0, 50.0, 150.0, 475.0, 800.0, 1600.0, 2100.0, 2620.0}},
            {"no2_hour", new double[]{0.0, 100.0, 200.0, 700.0, 1200.0, 2340.0, 3090.0, 3840.0}},
            {"no2_day", new double[]{0.0, 40.0, 80.0, 180.0, 280.0, 565.0, 750.0, 940.0}},
            {"pm10_hour", new double[]{0.0, 50.0, 150.0, 250.0, 350.0, 420.0, 500.0, 600.0}},
            {"pm10_day", new double[]{0.0, 50.0, 150.0, 250.0, 350.0, 420.0, 500.0, 600.0}},
            {"co_hour", new double[]{0.0, 5.0, 10.0, 35.0, 60.0, 90.0, 120.0, 150.0}},
            {"co_day", new double[]{0.0, 2.0, 4.0, 14.0, 24.0, 36.0, 48.0, 60.0}},
            {"o3_hour", new double[]{0.0, 160.0, 200.0, 300.0, 400.0, 800.0, 1000.0, 1200.0}},
            {"o3_day", new double[]{0.0, 100.0, 160.0, 215.0, 265.0, 800.0, 1000.0, 1200.0}},
            {"pm25_hour", new double[]{0.0, 35.0, 75.0, 115.0, 150.0, 250.0, 350.0, 500.0}},
            {"pm25_day", new double[]{0.0, 35.0, 75.0, 115.0, 150.0, 250.0, 350.0, 500.0}}
        };
        /// <summary>
        /// 计算综合指数
        /// </summary>
        /// <param name="pm25"></param>
        /// <param name="pm10"></param>
        /// <param name="so2"></param>
        /// <param name="no2"></param>
        /// <param name="co"></param>
        /// <param name="o3"></param>
        /// <returns></returns>
        public static string CalcuCompIndex(string pm25, string pm10, string so2, string no2, string co, string o3)
        {
            if (Utility.ConvertValueOrgin(pm25) != noDataValue && Utility.ConvertValueOrgin(pm10) != noDataValue && Utility.ConvertValueOrgin(so2) != noDataValue && Utility.ConvertValueOrgin(no2) != noDataValue && Utility.ConvertValueOrgin(co) != noDataValue && Utility.ConvertValueOrgin(o3) != noDataValue)
            {
                double result = Double.Parse(pm25) / 35 + Double.Parse(pm10) / 70 + Double.Parse(so2) / 60 + Double.Parse(no2) / 40 + Double.Parse(co) / 4 + Double.Parse(o3) / 160;
                string calcResult = Decimal.Round(Decimal.Parse(result.ToString()), 3).ToString();
                return calcResult;
            }
            else
            {
                return noDataValue;
            }
        }

        /// <summary>
        /// 计算某项污染物对应的IAQI
        /// </summary>
        /// <param name="data">污染物对应的浓度值</param>
        /// <param name="target">污染物对应的指标名称</param>
        /// <returns></returns>
        public static int CalcuIAQI(double data, string target)
        {
            double temp = -999;

            double[] iaqiRanges = AirIAQI[target];

            if (data < iaqiRanges[0])
            {
                temp = -999;
            }
            else if (iaqiRanges[0] <= data && data < iaqiRanges[0 + 1])
            {
                temp = ((50.0 - 0.0) / (iaqiRanges[0 + 1] - iaqiRanges[0])) * (data - iaqiRanges[0]) + 0.0;
            }
            else if (iaqiRanges[0 + 1] <= data && data < iaqiRanges[0 + 2])
            {
                temp = ((100.0 - 50.0) / (iaqiRanges[0 + 2] - iaqiRanges[0 + 1])) * (data - iaqiRanges[0 + 1]) + 50.0;
            }
            else if (iaqiRanges[0 + 2] <= data && data < iaqiRanges[0 + 3])
            {
                temp = ((150.0 - 100.0) / (iaqiRanges[0 + 3] - iaqiRanges[0 + 2])) * (data - iaqiRanges[0 + 2]) + 100.0;
            }
            else if (iaqiRanges[0 + 3] <= data && data < iaqiRanges[0 + 4])
            {
                temp = ((200.0 - 150.0) / (iaqiRanges[0 + 4] - iaqiRanges[0 + 3])) * (data - iaqiRanges[0 + 3]) + 150.0;
            }
            else if (iaqiRanges[0 + 4] <= data && data < iaqiRanges[0 + 5])
            {
                temp = ((300.0 - 200.0) / (iaqiRanges[0 + 5] - iaqiRanges[0 + 4])) * (data - iaqiRanges[0 + 4]) + 200.0;
            }
            else if (iaqiRanges[0 + 5] <= data && data < iaqiRanges[0 + 6])
            {
                temp = ((400.0 - 300.0) / (iaqiRanges[0 + 6] - iaqiRanges[0 + 5])) * (data - iaqiRanges[0 + 5]) + 300.0;
            }
            else if (iaqiRanges[0 + 6] <= data && data < iaqiRanges[0 + 7])
            {
                temp = ((500.0 - 400.0) / (iaqiRanges[0 + 7] - iaqiRanges[0 + 6])) * (data - iaqiRanges[0 + 6]) + 400.0;
            }
            else
            {
                temp = 500;
            }
            if (data > 800 && (target == "so2_hour" || target == "o3_day"))
            {
                temp = -999;
            }
            return Int32.Parse(Math.Ceiling(temp).ToString());
        }

        /// <summary>
        /// 统一首要污染物
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static string GetPrimaryPollutant(string target)
        {
            string result = "-";
            if (target.Contains("pm2.5") || target.Contains("pm25") || target.Contains("PM2.5") || target.Contains("PM25"))
            {
                result = "PM2.5";
            }
            else if (target.Contains("pm10") || target.Contains("PM10"))
            {
                result = "PM10";
            }
            else if (target.Contains("o3") || target.Contains("O3"))
            {
                result = "O3";
            }
            else if (target.Contains("so2") || target.Contains("SO2"))
            {
                result = "SO2";
            }
            else if (target.Contains("no2") || target.Contains("NO2"))
            {
                result = "NO2";
            }
            else if (target.Contains("co") || target.Contains("CO"))
            {
                result = "CO";
            }
            return result;
        }


        /// <summary>
        /// 通过六项污染物浓度值计算AQI
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="dateType">时间类型 小时（hour）、日均(day)</param>
        /// <returns></returns>
        public static JObject CalcuAQI(string so2, string no2, string pm10, string co, string o3, string pm25, string dateType)
        {
            JObject resultObj = new JObject();
            resultObj["so2"] = so2;
            resultObj["so2_iaqi"] = noDataValue;
            resultObj["no2"] = no2;
            resultObj["no2_iaqi"] = noDataValue;
            resultObj["pm10"] = pm10;
            resultObj["pm10_iaqi"] = noDataValue;
            resultObj["co"] = co;
            resultObj["co_iaqi"] = noDataValue;
            resultObj["o3"] = o3;
            resultObj["o3_iaqi"] = noDataValue;
            resultObj["pm25"] = pm25;
            resultObj["pm25_iaqi"] = noDataValue;
            resultObj["aqi"] = noDataValue;
            resultObj["primary_pollutant"] = "";
            resultObj["aqi_level"] = "";

            List<double> iaqiList = new List<double>();
            List<JObject> iaqiResultList = new List<JObject>();
            if (Utility.ConvertValueOrgin(so2) != noDataValue)
            {
                double so2Data = Double.Parse(so2);
                int iaqi_so2 = AQICalculateHelper.CalcuIAQI(so2Data, "so2_" + dateType);
                if (iaqi_so2 > 0)
                {
                    iaqiList.Add(iaqi_so2);
                    resultObj["so2_iaqi"] = iaqi_so2;
                    
                    JObject iaqiResult = new JObject();
                    iaqiResult["iaqi"] = iaqi_so2;
                    iaqiResult["target"] = GetPrimaryPollutant("so2");
                    iaqiResultList.Add(iaqiResult);
                }
            }
            if (Utility.ConvertValueOrgin(no2) != noDataValue)
            {
                double no2Data = Double.Parse(no2);
                int iaqi_no2 = AQICalculateHelper.CalcuIAQI(no2Data, "no2_" + dateType);
                if (iaqi_no2 > 0)
                {
                    iaqiList.Add(iaqi_no2);
                    resultObj["no2_iaqi"] = iaqi_no2;

                    JObject iaqiResult = new JObject();
                    iaqiResult["iaqi"] = iaqi_no2;
                    iaqiResult["target"] = GetPrimaryPollutant("no2");
                    iaqiResultList.Add(iaqiResult);
                }
            }
            if (Utility.ConvertValueOrgin(pm10) != noDataValue)
            {
                double pm10Data = Double.Parse(pm10);
                int iaqi_pm10 = AQICalculateHelper.CalcuIAQI(pm10Data, "pm10_" + dateType);
                if (iaqi_pm10 > 0)
                {
                    iaqiList.Add(iaqi_pm10);
                    resultObj["pm10_iaqi"] = iaqi_pm10;

                    JObject iaqiResult = new JObject();
                    iaqiResult["iaqi"] = iaqi_pm10;
                    iaqiResult["target"] = GetPrimaryPollutant("pm10");
                    iaqiResultList.Add(iaqiResult);
                }
            }
            if (Utility.ConvertValueOrgin(co) != noDataValue)
            {
                double coData = Double.Parse(co);
                int iaqi_co = AQICalculateHelper.CalcuIAQI(coData, "co_" + dateType);
                if (iaqi_co > 0)
                {
                    iaqiList.Add(iaqi_co);
                    resultObj["co_iaqi"] = iaqi_co;

                    JObject iaqiResult = new JObject();
                    iaqiResult["iaqi"] = iaqi_co;
                    iaqiResult["target"] = GetPrimaryPollutant("co");
                    iaqiResultList.Add(iaqiResult);
                }
            }
            if (Utility.ConvertValueOrgin(o3) != noDataValue)
            {
                double o3Data = Double.Parse(o3);
                int iaqi_o3 = AQICalculateHelper.CalcuIAQI(o3Data, "o3_" + dateType);
                if (iaqi_o3 > 0)
                {
                    iaqiList.Add(iaqi_o3);
                    resultObj["o3_iaqi"] = iaqi_o3;

                    JObject iaqiResult = new JObject();
                    iaqiResult["iaqi"] = iaqi_o3;
                    iaqiResult["target"] = GetPrimaryPollutant("o3");
                    iaqiResultList.Add(iaqiResult);
                }
            }
            if (Utility.ConvertValueOrgin(pm25) != noDataValue)
            {
                double pm25Data = Double.Parse(pm25);
                int iaqi_pm25 = AQICalculateHelper.CalcuIAQI(pm25Data, "pm25_" + dateType);
                if (iaqi_pm25 > 0)
                {
                    iaqiList.Add(iaqi_pm25);
                    resultObj["pm25_iaqi"] = iaqi_pm25;

                    JObject iaqiResult = new JObject();
                    iaqiResult["iaqi"] = iaqi_pm25;
                    iaqiResult["target"] = GetPrimaryPollutant("pm25");
                    iaqiResultList.Add(iaqiResult);
                }
            }
            if (iaqiList.Count > 0)
            {
                double aqi = iaqiList.Max();
                if (aqi != null)
                {
                    resultObj["aqi"] = aqi;
                    JObject aqiLevelObj = GetPollutantLevel(Int32.Parse(aqi.ToString()));
                    if (aqiLevelObj != null && aqiLevelObj["level"] != null)
                    {
                        resultObj["aqi_level"] = aqiLevelObj["level"].ToString();
                    }
                    if (aqi > 50)
                    {
                        //通过最大IAQI读取首要污染物
                        List<string> primaryPollutants = new List<string>();
                        List<string> primaryPollutantResults = new List<string>();
                        for (int v = 0; v < iaqiResultList.Count; v++)
                        {
                            if (Double.Parse(iaqiResultList[v]["iaqi"].ToString()) == aqi)
                            {
                                primaryPollutants.Add(iaqiResultList[v]["target"].ToString());
                            }
                        }
                        if (primaryPollutants.Count > 0)
                        {
                            string[] tempPrimaryPollutants = new string[] { "PM2.5", "PM10", "O3", "SO2", "NO2", "CO" };
                            for (int z = 0; z < tempPrimaryPollutants.Length; z++)
                            {
                                if (primaryPollutants.Contains(tempPrimaryPollutants[z]))
                                {
                                    primaryPollutantResults.Add(tempPrimaryPollutants[z]);
                                }
                            }
                        }
                        if (primaryPollutantResults.Count > 0)
                        {
                            resultObj["primary_pollutant"] = String.Join(",", primaryPollutantResults.ToArray());
                        }
                    }
                }
            }

            return resultObj;
        }


        /// <summary>
        /// 通过六项污染物浓度值计算AQI
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="dateType">时间类型 小时（hour）、日均(day)</param>
        /// <returns></returns>
        public static DataTable CalcuAQI(DataTable dt, string dateType)
        {
            DataTable tempCope = dt.Copy();
            string[] targets = new string[] { "so2", "no2", "pm10", "co", "o3", "pm25" };
            //day "so2_24h","no2_24h","pm10_24h","co_24h","o3_8h_max","pm25_24h"
            //hour "so2_1h","no2_1h","pm10_1h","co_1h","o3_1h","pm25_1h"

            if (tempCope != null && tempCope.Rows.Count > 0)
            {
                if (!tempCope.Columns.Contains("aqi"))
                {
                    tempCope.Columns.Add("aqi", typeof(System.Int32));
                }
                if (!tempCope.Columns.Contains("primary_pollutant"))
                {
                    tempCope.Columns.Add("primary_pollutant", typeof(System.String));
                }
                if (!tempCope.Columns.Contains("aqi_level"))
                {
                    tempCope.Columns.Add("aqi_level", typeof(System.String));
                }

                for (int l = 0; l < tempCope.Rows.Count; l++)
                {
                    tempCope.Rows[l]["aqi"] = -999;
                    tempCope.Rows[l]["aqi_level"] = "-";
                    tempCope.Rows[l]["primary_pollutant"] = "-";

                    List<double> iaqiList = new List<double>();
                    List<JObject> iaqiResultList = new List<JObject>();
                    for (int k = 0; k < targets.Length; k++)
                    {
                        string target = "";
                        switch (dateType)
                        {
                            case "hour":
                                target = targets[k] + "_1h";
                                break;
                            case "day":
                                if (targets[k] == "o3")
                                {
                                    target = targets[k] + "_8h_max";
                                }
                                else
                                {
                                    target = targets[k] + "_24h";
                                }
                                break;
                        }
                        if (tempCope.Rows[l][target] != null && tempCope.Rows[l][target].ToString() != "")
                        {
                            double targetData = Double.Parse(tempCope.Rows[l][target].ToString());
                            if (targets[k] == "co")
                            {
                                targetData = Math.Round(targetData, 1);
                            }
                            else
                            {
                                targetData = Math.Round(targetData, 0);
                            }

                            tempCope.Rows[l][target] = targetData;

                            int iaqi = AQICalculateHelper.CalcuIAQI(targetData, targets[k] + "_" + dateType);
                            if (iaqi > 0)
                            {
                                iaqiList.Add(iaqi);

                                JObject iaqiResult = new JObject();
                                iaqiResult["iaqi"] = iaqi;
                                iaqiResult["target"] = GetPrimaryPollutant(targets[k]);
                                iaqiResultList.Add(iaqiResult);
                            }
                        }
                    }
                    if (iaqiList.Count > 0)
                    {
                        double aqi = iaqiList.Max();
                        if (aqi != null)
                        {
                            tempCope.Rows[l]["aqi"] = aqi;
                            JObject aqiLevelObj = GetPollutantLevel(Int32.Parse(aqi.ToString()));
                            if (aqiLevelObj != null && aqiLevelObj["level"] != null)
                            {
                                tempCope.Rows[l]["aqi_level"] = aqiLevelObj["level"].ToString();
                            }
                            if (aqi > 50)
                            {
                                //通过最大IAQI读取首要污染物
                                List<string> primaryPollutants = new List<string>();
                                List<string> primaryPollutantResults = new List<string>();
                                for (int v = 0; v < iaqiResultList.Count; v++)
                                {
                                    if (Double.Parse(iaqiResultList[v]["iaqi"].ToString()) == aqi)
                                    {
                                        primaryPollutants.Add(iaqiResultList[v]["target"].ToString());
                                    }
                                }
                                if (primaryPollutants.Count > 0)
                                {
                                    string[] tempPrimaryPollutants = new string[] { "PM2.5", "PM10", "O3", "SO2", "NO2", "CO" };
                                    for (int z = 0; z < tempPrimaryPollutants.Length; z++)
                                    {
                                        if (primaryPollutants.Contains(tempPrimaryPollutants[z]))
                                        {
                                            primaryPollutantResults.Add(tempPrimaryPollutants[z]);
                                        }
                                    }
                                }
                                if (primaryPollutantResults.Count > 0)
                                {
                                    tempCope.Rows[l]["primary_pollutant"] = String.Join(",", primaryPollutantResults.ToArray());
                                }
                            }
                        }
                    }
                }
            }
            return tempCope;
        }

        /// <summary>
        /// 通过AQI值读取AQI的等级
        /// </summary>
        /// <param name="aqi">AQI值</param>
        /// <returns>grade，level</returns>
        public static JObject GetPollutantLevel(int aqi)
        {
            JObject obj = new JObject();

            string level = "-";
            int grade = 0;
            if (aqi == null || aqi == -999)
            {
                level = "-";
                grade = 0;
            }
            else if (aqi <= 50 && aqi >= 0)
            {
                level = "优";
                grade = 1;
            }
            else if (aqi > 50 && aqi <= 100)
            {
                level = "良";
                grade = 2;
            }
            else if (aqi > 100 && aqi <= 150)
            {
                level = "轻度污染";
                grade = 3;
            }
            else if (aqi > 150 && aqi <= 200)
            {
                level = "中度污染";
                grade = 4;
            }
            else if (aqi > 200 && aqi <= 300)
            {
                level = "重度污染";
                grade = 5;
            }
            else
            {
                level = "严重污染";
                grade = 6;
            }

            obj["level"] = level;
            obj["grade"] = grade;
            return obj;
        }
    }
}
