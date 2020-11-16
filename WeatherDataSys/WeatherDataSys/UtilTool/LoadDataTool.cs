using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherDataSys.Model;
using WeatherDataSys.Repository;

namespace WeatherDataSys.UtilTool
{
    public class LoadDataTool
    {
        private StationInfoRepository sir = new StationInfoRepository();

        public void OnLineDataLoad()
        {
            int errorTotal = 0;

            From_Main.rtb.Clear();
            Utility.writeLog("开始抓取...", ColorEnum.Blue);

            DateTime now = DateTime.Now;
            string dateStr = now.AddHours(-1).ToString("yyyyMMddHH");
            string startTimeStr = now.AddHours(-1).ToString("yyyy-MM-dd HH:00:00");
            string endTimeStr = now.AddHours(-1).ToString("yyyy-MM-dd HH:00:00");
            try
            {
                DataTable dt = sir.StationInfoQuery();
                List<string> codes = new List<string>();
                bool isNoData = false;
                if (dt != null && dt.Rows.Count > 0)
                {
                    List<WeatherModel> addList = new List<WeatherModel>();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (From_Main.isStopRun)
                        {
                            break;
                        }
                        string code = dt.Rows[i]["区站号"].ToString();
                        codes.Add(code);
                    }

                    DataTable weatherDataTable = sir.WeatherDataQuery(startTimeStr, endTimeStr, string.Join(",", codes.ToArray()));
                    if (weatherDataTable != null && weatherDataTable.Rows.Count > 0)
                    {
                        isNoData = true; ;
                    }
                }

                if (dt != null && dt.Rows.Count > 0)
                {
                    List<WeatherModel> addList = new List<WeatherModel>();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (From_Main.isStopRun)
                        {
                            break;
                        }
                        string stationCode = dt.Rows[i]["区站号"].ToString();
                        string stationName = dt.Rows[i]["站名"].ToString();
                        if (stationCode != null && stationCode != "")
                        {
                            //2020012010/51378
                            if (!isNoData)
                            {
                                string resultStr = SendHelper.SendPost(From_Main.address.Text.Trim() + dateStr + "/" + stationCode);
                                if (String.IsNullOrEmpty(resultStr))
                                {
                                    errorTotal++;
                                }

                                if (errorTotal>100)
                                {
                                    string sendCredentials = ConfigurationManager.AppSettings["sendCredentials"];
                                    string sendAddress = ConfigurationManager.AppSettings["sendAddress"];
                                    string receiverAddress = ConfigurationManager.AppSettings["receiverAddress"];
                                    string projectName = ConfigurationManager.AppSettings["projectName"];
                                    bool enabledMailNotice = Boolean.Parse(ConfigurationManager.AppSettings["enabledMailNotice"]);

                                    MailHelper.SendMail(sendAddress, receiverAddress.Split(','), null, projectName, startTimeStr + "本次爬取数据超过100个站点没有数据，可能IP被拉黑，或者网络断开", sendCredentials);
                                    break;
                                }

                                List<WeatherModel> list = JsonConvert.DeserializeObject<List<WeatherModel>>(resultStr);
                                if (list != null && list.Count > 0 && !From_Main.isStopRun)
                                {
                                    WeatherModel wm = list[0];
                                    wm.stationcode = stationCode;
                                    addList.Add(wm);
                                    Utility.writeLog(stationName + "(" + stationCode + ")数据已下载，剩" + (dt.Rows.Count - i - 1) + "个站点。", ColorEnum.Green);
                                }

                                if (String.IsNullOrEmpty(resultStr))
                                {
                                    Utility.writeLog(stationName + "(" + stationCode + ")数据下载失败，剩" + (dt.Rows.Count - i - 1) + "个站点。", ColorEnum.Red);
                                }
                            }
                        }
                    }
                    if (!From_Main.isStopRun)
                    {
                        if (addList.Count > 0)
                        {
                            sir.AddWeatherDatas(addList);
                        }
                        Utility.writeLog("本次定时爬取完成,爬取时间：" + dateStr, ColorEnum.Blue);
                    }

                }
            }
            catch (Exception e)
            {
                Utility.writeLog("定时爬取失败,爬取时间：" + now, ColorEnum.Red);
            }
        }
    }
}
