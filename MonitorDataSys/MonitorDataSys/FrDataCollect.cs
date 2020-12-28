using MonitorDataSys.Models;
using MonitorDataSys.Repository.local;
using MonitorDataSys.UtilTool;
using MonitorDataSys.UtilTool.IJobTool;
using Newtonsoft.Json.Linq;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using Env.CnemcPublish.DAL;
using Env.CnemcPublish.RiaServices;
using Com.Hzexe.Air.OpenAirLibrary;
using OpenRiaServices.DomainServices.Client;
using System.Linq;
using MonitorDataSys.Repository.bzk;
using System.Net;
using System.Text;
using NSoup.Select;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using MonitorDataSys.Models.NationPredict;
using MonitorDataSys.Models.NationPrediction;
using System.IO;

namespace MonitorDataSys
{
    public partial class FrDataCollect : Form
    {

        private readonly string XAP_URL = ConfigurationManager.AppSettings["dataUrl"];

        public static FrDataCollect frDataCollect;

        private readonly AirStationRepository asr = new AirStationRepository();
        private readonly DataConfigRepository dcr = new DataConfigRepository();
        private readonly CollectStaticRepository cst = new CollectStaticRepository();
        private readonly LogRepository lr = new LogRepository();
        private readonly NationAreaPreditionRepository napr = new NationAreaPreditionRepository();
        private readonly WeatherStationRepository wst = new WeatherStationRepository();
        private CityMonitorHourRepository cmhr = null;
        private StationMonitorHourRepository smhr = null;
        private CityMonitorDayRepository cmdr = null;
        private StationMonitorDayRepository smdr = null;

        private AreaPredictionRepository apr = null;
        private ProvincePredictionRepository ppr = null;
        private CityPredictionRepository cpr = null;

        private WeatherStationHourRepository wshr = null;
        private WeatherStationDayRepository wsdr = null;
        private WeatherCityHourRepository wchr = null;
        private WeatherCityDayRepository wcdr = null;

        private readonly string hourCity = ConfigurationManager.AppSettings["hourCity"];
        private readonly string hourStation = ConfigurationManager.AppSettings["hourStation"];
        private readonly string dayCity = ConfigurationManager.AppSettings["dayCity"];
        private readonly string dayStation = ConfigurationManager.AppSettings["dayStation"];

        private readonly string noDataValue = ConfigurationManager.AppSettings["noDataValue"];

        private IDictionary hourCityTableFiledDic = ConfigurationManager.GetSection("hourCitySettings") as IDictionary;
        private IDictionary hourStationTableFiledDic = ConfigurationManager.GetSection("hourStationSettings") as IDictionary;
        private IDictionary dayCityTableFiledDic = ConfigurationManager.GetSection("dayCitySettings") as IDictionary;
        private IDictionary dayStationTableFiledDic = ConfigurationManager.GetSection("dayStationSettings") as IDictionary;

        private readonly string areaPredictionUrl = ConfigurationManager.AppSettings["areaPredictionUrl"];
        private readonly string provincePredictionUrl = ConfigurationManager.AppSettings["provincePredictionUrl"];
        private readonly string cityPredictionUrl = ConfigurationManager.AppSettings["cityPredictionUrl"];

        private readonly string areaPredictionTableName = ConfigurationManager.AppSettings["areaPredictionTable"];
        private readonly string provincePredictionTableName = ConfigurationManager.AppSettings["provincePredictionTable"];
        private readonly string cityPredictionTableName = ConfigurationManager.AppSettings["cityPredictionTable"];

        private readonly string weatherServerUrl = ConfigurationManager.AppSettings["weatherServerUrl"];
        private readonly string weatherStationHourTableName = ConfigurationManager.AppSettings["weatherStationHourTable"];
        private readonly string weatherStationDayTableName = ConfigurationManager.AppSettings["weatherStationDayTable"];
        private readonly string weatherCityHourTableName = ConfigurationManager.AppSettings["weatherCityHourTable"];
        private readonly string weatherCityDayTableName = ConfigurationManager.AppSettings["weatherCityDayTable"];

        private delegate void InvokeCallback(RichTextBox rtb, string msg, ColorEnum color = ColorEnum.Green);

        private List<RichTextBox> richLogs = new List<RichTextBox>();

        //从工厂中获取一个调度器实例化
        IScheduler scheduler;
        //string jobName = "jobname";
        //string tigerName = "tigername";
        string groupName = "groupname";

        public FrDataCollect()
        {
            InitDBConnectInfo();
            InitializeComponent();
            System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = false;
            frDataCollect = this;
        }

        private void FrDataCollect_Load(object sender, EventArgs e)
        {
            initRichLogs();
            refreshStaticInfo();
        }

        /// <summary>
        /// 清除任务和触发器
        /// </summary>
        public void ClearJobTrigger()
        {
            try
            {
                if (scheduler != null && scheduler.IsStarted)
                {
                    scheduler.Shutdown();
                }

                //TriggerKey triggerKey = new TriggerKey(tigerName, groupName);
                //JobKey jobKey = new JobKey(jobName, groupName);
                //if (scheduler != null)
                //{
                //    scheduler.PauseTrigger(triggerKey);// 停止触发器  
                //    scheduler.UnscheduleJob(triggerKey);// 移除触发器  
                //    scheduler.DeleteJob(jobKey);// 删除任务
                //}
            }
            catch (Exception ex)
            {
                //日志处理
                lr.AddLogInfo(ex.ToString(), "捕获异常信息", "捕获异常信息", "Error");
                Loghelper.WriteErrorLog("捕获异常信息", ex);
            }
        }

        /// <summary>
        /// 设置控件是否可用
        /// </summary>
        /// <param name="status"></param>
        private void setControlStatus(bool status)
        {
            try
            {
                this.btn_Start.Enabled = status;
                this.btn_Stop.Enabled = !status;
                this.nuD_min_Hour.Enabled = status;
                this.nuD_day_Day.Enabled = status;
            }
            catch (Exception ex)
            {
                //日志处理
                lr.AddLogInfo(ex.ToString(), "捕获异常信息", "捕获异常信息", "Error");
                Loghelper.WriteErrorLog("捕获异常信息", ex);
            }
        }

        /// <summary>
        /// 启动定时采集
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Start_Click(object sender, EventArgs e)
        {
            #region 测试用例
            //Task.Run(() =>
            //{
            //    for (var i = 0; i < 100000; i++)
            //    {
            //        //本次集采时间
            //        DateTime collectTime = DateTime.Now;
            //        //本次采集数据条数
            //        int collectTotal = 0;

            //        setControlStatus(false);

            //        this.rtb_Log.Clear();
            //        writeLog("开始采集数据", ColorEnum.Blue);

            //        //1.读取标准库中对应设备表Sb（设备编号DeviceId、设备类型Sblx、厂商编号CsId、区域编号AreaCode）
            //        //2.通过厂商编号CsId、区域编号AreaCode就可以找到具体的数据库
            //        //  通过设备类型Sblx就可以找到具体的数据库表
            //        //  通过设备编号DeviceId就可以找到具体的数据
            //        //3.查询标准库中最新数据对应的时间，然后把厂商库中时间大于最新数据时间的数据全部查询出来录入到标准库中
            //        try
            //        {
            //            DataTable sbTable = sr.GetSb();
            //            DataTable sblxTable = slr.GetSblx();

            //            if (sbTable != null && sbTable.Rows.Count > 0)
            //            {
            //                for (int a = 0; a < sbTable.Rows.Count; a++)
            //                {
            //                    try
            //                    {
            //                        string areaCode = sbTable.Rows[a]["AreaCode"].ToString();
            //                        //设备编号后面处理后有可能存在多个，中间用英文逗号分隔
            //                        string deviceId = sbTable.Rows[a]["DeviceId"].ToString();
            //                        string sblxId = sbTable.Rows[a]["Sblx"].ToString();
            //                        string csId = sbTable.Rows[a]["CsId"].ToString();
            //                        string sbName = sbTable.Rows[a]["Name"].ToString();
            //                        string bzTableName = "";
            //                        string bzInnerTableName = "";

            //                        writeLog("正在采集<" + areaName + "-" + sbCsName + "-" + sbName + ">设备数据...", ColorEnum.Blue);

            //                        if (sblxTable != null && sblxTable.Rows.Count > 0)
            //                        {
            //                            for (int b = 0; b < sblxTable.Rows.Count; b++)
            //                            {
            //                                string sblcId = sblxTable.Rows[b]["SblcId"].ToString();
            //                                if (sblcId == sblxId)
            //                                {
            //                                    bzTableName = sblxTable.Rows[b]["DataTbName"].ToString();
            //                                    bzInnerTableName = sblxTable.Rows[b]["InnnerTbName"].ToString();
            //                                    break;
            //                                }

            //                            }
            //                        }
            //                        if (!String.IsNullOrEmpty(bzInnerTableName))
            //                        {
            //                            //一种设备对应多种监测
            //                            DataTable innerTable = dir.GetDeviceInnerInfos(bzInnerTableName, deviceId);
            //                            if (innerTable != null && innerTable.Rows.Count > 0)
            //                            {
            //                                List<string> deviceInnerIDList = new List<string>();
            //                                for (int c = 0; c < innerTable.Rows.Count; c++)
            //                                {
            //                                    string deviceInnerID = innerTable.Rows[c]["DeviceInnerID"].ToString();
            //                                    deviceInnerIDList.Add(deviceInnerID);
            //                                }
            //                                if (deviceInnerIDList.Count > 0)
            //                                {
            //                                    deviceId = String.Join(",", deviceInnerIDList.ToArray());
            //                                }
            //                            }
            //                        }
            //                        if (!String.IsNullOrEmpty(bzTableName))
            //                        {
            //                            string relationTableNames = csTableDic[csId].ToString();
            //                            IDictionary relationTableDic = ConfigurationManager.GetSection(relationTableNames) as IDictionary;
            //                            string monitorTableName = relationTableDic[bzTableName].ToString();
            //                            DateTime lastTime = bdr.GetDataLastTime(bzTableName);
            //                            ConnectionStringSettings conn = dcr.ConnnectConfigQuery(areaCode, csId);
            //                            if (conn != null)
            //                            {
            //                                List<JObject> bzDataResutList = new List<JObject>();
            //                                switch (relationTableNames)
            //                                {
            //                                    case "hcTableSettings":
            //                                        HcDataRepository hdr = new HcDataRepository(conn);
            //                                        DataTable hcMonitorTable = hdr.HcMonitorDataQuery(monitorTableName, deviceId, lastTime, collectTime);
            //                                        if (hcMonitorTable != null && hcMonitorTable.Rows.Count > 0)
            //                                        {
            //                                            bzDataResutList = HcDataConvert.DataTableConvertList(areaCode, csId, hcMonitorTable);
            //                                        }
            //                                        break;
            //                                    case "jkTableSettings":
            //                                        JkDataRepository jdr = new JkDataRepository(conn);
            //                                        DataTable jkMonitorTable = jdr.JkMonitorDataQuery(monitorTableName, deviceId, lastTime, collectTime);
            //                                        if (jkMonitorTable != null && jkMonitorTable.Rows.Count > 0)
            //                                        {
            //                                            bzDataResutList = JkDataConvert.DataTableConvertList(areaCode, csId, jkMonitorTable);
            //                                        }
            //                                        break;
            //                                }
            //                                if (bzDataResutList != null && bzDataResutList.Count > 0)
            //                                {
            //                                    bool addResult = bdr.AddDataConfigInfo(bzTableName, bzDataResutList);
            //                                    if (addResult)
            //                                    {
            //                                        collectTotal += bzDataResutList.Count;
            //                                        writeLog("<" + areaName + "-" + sbCsName + "-" + sbName + ">设备数据采集成功，本次采集" + bzDataResutList.Count + "条数据", ColorEnum.Green);
            //                                    }
            //                                    else
            //                                    {
            //                                        writeLog("<" + areaName + "-" + sbCsName + "-" + sbName + ">设备数据采集失败", ColorEnum.Red);
            //                                    }
            //                                }
            //                                else
            //                                {
            //                                    writeLog("<" + areaName + "-" + sbCsName + "-" + sbName + ">设备暂无数据，无法采集", ColorEnum.Orange);
            //                                }
            //                            }
            //                            else
            //                            {
            //                                //请配置数据源信息
            //                                writeLog("<" + areaName + "-" + sbCsName + "-" + sbName + ">设备对应的数据源信息未配置，无法进行数据采集", ColorEnum.Red);
            //                            }
            //                        }
            //                        else
            //                        {
            //                            writeLog("<" + areaName + "-" + sbCsName + "-" + sbName + ">设备在设备类型表中没有配置对应数据表关系，无法采集", ColorEnum.Orange);
            //                        }
            //                    }
            //                    catch (Exception ex2)
            //                    {
            //                        //日志处理
            //                        lr.AddLogInfo(ex2.StackTrace.ToString(), "", "", "Error");
            //                        throw ex2;
            //                    }
            //                }
            //            }

            //            //保存本次采集信息
            //            cst.AddStaticInfo(collectTotal, collectTime);
            //            writeLog("本次定时爬取完成", ColorEnum.Blue);
            //        }
            //        catch (Exception ex)
            //        {
            //            //日志处理
            //            lr.AddLogInfo(ex.StackTrace.ToString(), "", "", "Error");
            //        }
            //    }
            //});
            #endregion
            try
            {
                //Stream s = ZipHelper.FileToStream("E:/万维/Code/rublish/202012210800-wind-surface-level-gfs-0.25.json");
                //byte[] s1 = ZipHelper.StreamToBytes(s);
                //string str = ZipHelper.DeflateAndEncodeBase64(s1);
                //ZipHelper.GZipDeCompress(s, s1);
                //Stream s2 = ZipHelper.Inflate(s);
                //ZipHelper.StreamToFile(s2, "E:/万维/Code/rublish/gfs.json");
                setControlStatus(false);
                for (int i = 0; i < richLogs.Count; i++)
                {
                    if (richLogs[i] != null)
                    {
                        richLogs[i].Clear();
                        writeLog(richLogs[i], "已启动定时采集...", ColorEnum.Blue);
                    }
                }

                //清除任务和触发器
                ClearJobTrigger();

                scheduler = StdSchedulerFactory.GetDefaultScheduler();

                //先清除任务和触发器
                ClearJobTrigger();

                //开启调度器
                scheduler.Start();

                ////触发器
                //ITrigger trigger = TriggerBuilder.Create()
                //                            .WithIdentity(tigerName, groupName)
                //                            .StartNow()
                //                            .WithCronSchedule("0 0/" + nuD_min_Hour.Value + " * * * ?")
                //                            //.WithCronSchedule("0 0/5 * * * ?")
                //                            .Build();
                ////任务
                //IJobDetail job = JobBuilder.Create<LoadHourJob>()
                //                .WithIdentity(jobName, groupName)
                //                .UsingJobData("key", "value")
                //                .Build();

                //启动
                //scheduler.ScheduleJob(job, trigger);

                var dictionary = new Dictionary<IJobDetail, Quartz.Collection.ISet<ITrigger>>();

                #region 小时数据定时器
                Quartz.Collection.ISet<ITrigger> hourTriggerList = new Quartz.Collection.HashSet<ITrigger>();
                IJobDetail hourJob = JobBuilder.Create<LoadHourJob>().WithIdentity("hourJob", groupName).UsingJobData("key", "value").Build();
                ITrigger hourTrigger = TriggerBuilder.Create()
                                            .WithIdentity("hourTiger", groupName)
                                            .StartNow()
                                            .WithCronSchedule("0 0/" + nuD_min_Hour.Value + " * * * ?")
                                            //.WithCronSchedule("0 0/5 * * * ?")
                                            .Build();
                hourTriggerList.Add(hourTrigger);
                dictionary.Add(hourJob, hourTriggerList);
                #endregion

                #region 日均数据定时器
                Quartz.Collection.ISet<ITrigger> dayTriggerList = new Quartz.Collection.HashSet<ITrigger>();
                IJobDetail dayJob = JobBuilder.Create<LoadDayJob>().WithIdentity("dayJob", groupName).UsingJobData("key", "value").Build();
                ITrigger dayTrigger = TriggerBuilder.Create()
                                            .WithIdentity("dayTiger", groupName)
                                            .StartNow()
                                             .WithCronSchedule("0 0 0/" + nuD_day_Day.Value + " * * ? *")
                                            //.WithCronSchedule("0 0/" + nuD_day_Day.Value + " * * * ?")
                                            .Build();
                dayTriggerList.Add(dayTrigger);
                dictionary.Add(dayJob, dayTriggerList);
                #endregion

                #region 国家预报数据定时器
                Quartz.Collection.ISet<ITrigger> areaPredictionTriggerList = new Quartz.Collection.HashSet<ITrigger>();
                IJobDetail areaPredictionJob = JobBuilder.Create<LoadAreaPredictionJob>().WithIdentity("areaPredictionJob", groupName).UsingJobData("key", "value").Build();
                ITrigger areaPredictionTrigger = TriggerBuilder.Create()
                                            .WithIdentity("areaPredictionTiger", groupName)
                                            .StartNow()
                                           .WithCronSchedule("0 0 0/" + nuD_day_Day.Value + " * * ? *")
                                            //.WithCronSchedule("0 0/" + nuD_day_Day.Value + " * * * ?")
                                            .Build();
                areaPredictionTriggerList.Add(areaPredictionTrigger);
                dictionary.Add(areaPredictionJob, areaPredictionTriggerList);

                #endregion

                if (dictionary.Count > 0)
                {
                    scheduler.ScheduleJobs(dictionary, true);
                }
            }
            catch (Exception ex)
            {
                Loghelper.WriteErrorLog("启动定时任务", ex);
                lr.AddLogInfo(ex.ToString(), "启动定时任务", "启动定时任务", "Error");
            }
        }

        public async Task AAA()
        {
            DateTime startTime = DateTime.Parse("2020-01-01 00:00:00");
            DateTime endTime = DateTime.Parse("2020-01-05 00:00:00");
            EnvCnemcPublishDomainContext publishCtx = new EnvCnemcPublishDomainContext(XAP_URL);

            //城市日均
            //EntityQuery<CityDayAQIPublishHistory> cityAQILiveData = publishCtx.GetCityDayAQIPublishHistoriesQuery()
            //                                               .Where(x => x.Area == "兰州市")
            //                                               .Where(x => x.TimePoint >= startTime && x.TimePoint <= endTime)
            //                                               .OrderBy(x => x.TimePoint);
            //IEnumerable<CityDayAQIPublishHistory> stationAQIHistoryDataIEB = await publishCtx.Load(cityAQILiveData).ResultAsync();

            //EntityQuery<CityAQIPublishHistory> aa = publishCtx.GetCityAQIPublishHistoriesQuery()
            //     .Where(x => x.Area == "兰州市")
            //     .Where(x => x.TimePoint >= startTime && x.TimePoint <= endTime)
            //     .OrderBy(x => x.TimePoint);
            //IEnumerable<CityAQIPublishHistory> stationAQIHistoryDataIEB = await publishCtx.Load(aa).ResultAsync();

            //站点小时
            //EntityQuery<IAQIDataPublishHistory> cityAQILiveData = publishCtx.GetIAQIDataPublishHistoriesQuery()
            //                                               .Where(x => x.Area == "兰州市")
            //                                               .Where(x => x.TimePoint >= startTime && x.TimePoint <= endTime)
            //                                               .OrderBy(x => x.TimePoint); ;
            //IEnumerable<IAQIDataPublishHistory> stationAQIHistoryDataIEB = await publishCtx.Load(cityAQILiveData).ResultAsync();


            //站点日均
            //EntityQuery<AQIDataPublishHistory> stationAQIHistoryQuery = publishCtx.GetAQIDataPublishHistoriesQuery()
            //                                              .Where(x => x.Area == cityName)
            //                                              .Where(x => x.TimePoint >= dayTime && x.TimePoint <= dayTime)
            //                                              .OrderBy(x => x.TimePoint);

        }

        /// <summary>
        /// 停止定时采集
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Stop_Click(object sender, EventArgs e)
        {
            try
            {
                setControlStatus(true);

                for (int i = 0; i < richLogs.Count; i++)
                {
                    if (richLogs[i] != null)
                    {
                        this.writeLog(richLogs[i], "已停止采集...", ColorEnum.Blue);
                    }
                }
                if (!scheduler.IsShutdown)
                {
                    scheduler.Shutdown();
                }
            }
            catch (Exception ex)
            {
                //日志处理
                Loghelper.WriteErrorLog("捕获异常信息", ex);
                lr.AddLogInfo(ex.ToString(), "捕获异常信息", "捕获异常信息", "Error");
            }
        }

        /// <summary>
        /// 初始化数据库连接信息
        /// </summary>
        public void InitDBConnectInfo()
        {
            try
            {
                DataTable configTable = dcr.DataConfigInfoQuery();
                if (configTable != null && configTable.Rows.Count > 0)
                {
                    string ipStr = configTable.Rows[0]["DBIPAddress"].ToString();
                    string portStr = configTable.Rows[0]["DBPort"].ToString();
                    string userNameStr = configTable.Rows[0]["DBUserName"].ToString();
                    string passwordStr = MD5Helper.Md5Decrypt(configTable.Rows[0]["DBPassword"].ToString());
                    string dbTypeStr = configTable.Rows[0]["DBType"].ToString();
                    string dbNameStr = configTable.Rows[0]["DBName"].ToString();

                    cmhr = new CityMonitorHourRepository(ipStr, portStr, userNameStr, passwordStr, dbTypeStr, dbNameStr);
                    smhr = new StationMonitorHourRepository(ipStr, portStr, userNameStr, passwordStr, dbTypeStr, dbNameStr);
                    cmdr = new CityMonitorDayRepository(ipStr, portStr, userNameStr, passwordStr, dbTypeStr, dbNameStr);
                    smdr = new StationMonitorDayRepository(ipStr, portStr, userNameStr, passwordStr, dbTypeStr, dbNameStr);

                    apr = new AreaPredictionRepository(ipStr, portStr, userNameStr, passwordStr, dbTypeStr, dbNameStr);
                    ppr = new ProvincePredictionRepository(ipStr, portStr, userNameStr, passwordStr, dbTypeStr, dbNameStr);
                    cpr = new CityPredictionRepository(ipStr, portStr, userNameStr, passwordStr, dbTypeStr, dbNameStr);

                    wshr = new WeatherStationHourRepository(ipStr, portStr, userNameStr, passwordStr, dbTypeStr, dbNameStr);
                    wsdr = new WeatherStationDayRepository(ipStr, portStr, userNameStr, passwordStr, dbTypeStr, dbNameStr);
                    wchr = new WeatherCityHourRepository(ipStr, portStr, userNameStr, passwordStr, dbTypeStr, dbNameStr);
                    wcdr = new WeatherCityDayRepository(ipStr, portStr, userNameStr, passwordStr, dbTypeStr, dbNameStr);
                }
            }
            catch (Exception e)
            {
                //日志处理
                Loghelper.WriteErrorLog("捕获异常信息", e);
                lr.AddLogInfo(e.ToString(), "捕获异常信息", "捕获异常信息", "Error");
            }

        }

        public void initRichLogs()
        {
            richLogs = new List<RichTextBox>();
            richLogs.Add(rtb_Hour_Log);
            richLogs.Add(rtb_Day_Log);
            richLogs.Add(rtb_AreaPrediction_Log);
            richLogs.Add(rtb_WeatherHour_Log);
            richLogs.Add(rtb_WeatherDay_Log);
        }

        /// <summary>
        /// 界面打印日志
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="color"></param>
        public void writeLog(RichTextBox rtb, string msg, ColorEnum color = ColorEnum.Green)
        {
            try
            {
                //    //这时后台线程已经完成，并返回了主线程，所以可以直接使用UI控件了 
                if (rtb != null)
                {
                    if (rtb.InvokeRequired)
                    {
                        InvokeCallback msgCallback = new InvokeCallback(writeLog);
                        rtb.Invoke(msgCallback, new object[] { rtb, msg, color });
                    }
                    else
                    {
                        rtb.Focus();
                        rtb.Select(rtb.Text.Length, 0);
                        switch (color)
                        {
                            case ColorEnum.Blue:
                                rtb.SelectionColor = Color.Blue;
                                break;
                            case ColorEnum.Red:
                                rtb.SelectionColor = Color.Red;
                                break;
                            case ColorEnum.Green:
                                rtb.SelectionColor = Color.Green;
                                break;
                            case ColorEnum.Black:
                                rtb.SelectionColor = Color.Black;
                                break;
                            case ColorEnum.Orange:
                                rtb.SelectionColor = Color.Orange;
                                break;
                        }
                        string time = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                        rtb.AppendText(time + "->" + msg);
                        rtb.AppendText("\r\n");
                    }
                }
            }
            catch (Exception ex)
            {
                //日志处理
                Loghelper.WriteErrorLog("捕获异常信息", ex);
                lr.AddLogInfo(ex.ToString(), "捕获异常信息", "捕获异常信息", "Error");
            }
        }

        /// <summary>
        /// 小时数据采集总入口
        /// </summary>
        /// <returns></returns>
        public async Task collectHourDataTool()
        {

            //1.读取城市基本信息
            //2.读取站点基本信息
            //3.通过城市编号查询城市小时数据，并且通过查询到的城市小时数据中的时间和城市编码去对应的城市小时表中判断是否已经有数据，如果有数据则跳过这次采集，如果没有数据则进行采集
            //4.通过站点编号查询站点小时数据，并且通过查询到的站点小时数据中的时间和站点编码去对应的站点小时表中判断是否已经有数据，如果有数据则跳过这次采集，如果没有数据则进行采集
            //5.采集完后需要把采集结果记录在表中，并且在页面上进行展示
            try
            {
                //本次集采时间
                DateTime collectTime = DateTime.Now;
                //本次采集数据条数
                int collectTotal = 0;

                if (this.rtb_Hour_Log != null)
                {
                    this.rtb_Hour_Log.Clear();
                }
                writeLog(rtb_Hour_Log, "开始采集数据", ColorEnum.Blue);

                //创建domain客户端
                EnvCnemcPublishDomainContext publishCtx = new EnvCnemcPublishDomainContext(XAP_URL);

                DataTable cityTable = asr.AirAreaInfoQuery();
                DataTable stationTable = asr.AirStationInfoQuery();

                #region 城市小时采集
                if (cityTable != null && cityTable.Rows.Count > 0)
                {
                    List<JObject> listCityHour = new List<JObject>();
                    for (int i = 0; i < cityTable.Rows.Count; i++)
                    {
                        string cityCode = cityTable.Rows[i]["CityCode"].ToString();
                        string cityName = cityTable.Rows[i]["Area"].ToString();
                        bool isLoadSucess = false;
                        if (!String.IsNullOrEmpty(cityCode))
                        {
                            try
                            {
                                EntityQuery<CityAQIPublishLive> cityAQILiveData = publishCtx.GetCityRealTimeAQIModelByCitycodeQuery(Int32.Parse(cityCode));
                                if (cityAQILiveData != null)
                                {
                                    IEnumerable<CityAQIPublishLive> cityAQILiveDataIEB = await publishCtx.Load(cityAQILiveData).ResultAsync();
                                    if (cityAQILiveDataIEB != null)
                                    {
                                        List<CityAQIPublishLive> cityAQILiveDataList = cityAQILiveDataIEB.ToList();
                                        if (cityAQILiveDataList != null)
                                        {
                                            for (int j = 0; j < cityAQILiveDataList.Count; j++)
                                            {
                                                if (!String.IsNullOrEmpty(cityAQILiveDataList[j].AQI) && cityAQILiveDataList[j].AQI != "—")
                                                {
                                                    bool isCompeletCollect = cmhr.IsCompeletCollect(hourCity, cityCode, cityAQILiveDataList[j].TimePoint);
                                                    //bool isCompeletCollect = false;
                                                    if (!isCompeletCollect)
                                                    {
                                                        JObject item = new JObject();

                                                        item[hourCityTableFiledDic["MONITORTIME"]] = cityAQILiveDataList[j].TimePoint;
                                                        item[hourCityTableFiledDic["CITYCODE"]] = cityAQILiveDataList[j].CityCode;
                                                        item[hourCityTableFiledDic["SO2_1H"]] = Utility.ConvertValueOrgin(cityAQILiveDataList[j].SO2);
                                                        item[hourCityTableFiledDic["SO2_1H_IAQI"]] = noDataValue;
                                                        item[hourCityTableFiledDic["NO2_1H"]] = Utility.ConvertValueOrgin(cityAQILiveDataList[j].NO2);
                                                        item[hourCityTableFiledDic["NO2_1H_IAQI"]] = noDataValue;
                                                        item[hourCityTableFiledDic["PM10_1H"]] = Utility.ConvertValueOrgin(cityAQILiveDataList[j].PM10);
                                                        item[hourCityTableFiledDic["PM10_1H_IAQI"]] = noDataValue;
                                                        item[hourCityTableFiledDic["CO_1H"]] = Utility.ConvertValueOrgin(cityAQILiveDataList[j].CO);
                                                        item[hourCityTableFiledDic["CO_1H_IAQI"]] = noDataValue;
                                                        item[hourCityTableFiledDic["O3_1H_MAX"]] = Utility.ConvertValueOrgin(cityAQILiveDataList[j].O3);
                                                        item[hourCityTableFiledDic["O3_1H_MAX_IAQI"]] = noDataValue;
                                                        item[hourCityTableFiledDic["PM25_1H"]] = Utility.ConvertValueOrgin(cityAQILiveDataList[j].PM2_5);
                                                        item[hourCityTableFiledDic["PM25_1H_IAQI"]] = noDataValue;
                                                        item[hourCityTableFiledDic["AQI"]] = Utility.ConvertValueOrgin(cityAQILiveDataList[j].AQI);
                                                        item[hourCityTableFiledDic["PRIMARY_POLLUTANT"]] = cityAQILiveDataList[j].PrimaryPollutant;
                                                        item[hourCityTableFiledDic["AQI_LEVEL"]] = Utility.AQILevelCovertInt(cityAQILiveDataList[j].Quality);
                                                        item["TYPE"] = 0;
                                                        if (!String.IsNullOrEmpty(hourCityTableFiledDic["MEASURE"].ToString()))
                                                        {
                                                            item[hourCityTableFiledDic["MEASURE"]] = cityAQILiveDataList[j].Measure;
                                                        }
                                                        listCityHour.Add(item);
                                                    }
                                                }
                                            }
                                        }
                                        isLoadSucess = true;
                                    }

                                }
                            }
                            catch (Exception ex2)
                            {
                                //日志处理
                                Loghelper.WriteErrorLog(cityName + "小时数据采集异常", ex2);
                                lr.AddLogInfo(cityName + "小时数据采集异常", "", hourCity, "Error");
                            }
                            if (isLoadSucess)
                            {
                                writeLog(rtb_Hour_Log, cityName + "(" + cityCode + ")数据已下载，剩" + (cityTable.Rows.Count - i - 1) + "个城市。", ColorEnum.Green);
                            }
                            else
                            {
                                writeLog(rtb_Hour_Log, cityName + "(" + cityCode + ")数据下载失败，剩" + (cityTable.Rows.Count - i - 1) + "个城市。", ColorEnum.Red);
                            }
                        }
                    }
                    if (listCityHour.Count > 0)
                    {
                        bool cityHourInsertResult = cmhr.AddDataInfo(hourCity, listCityHour);
                        //bool cityHourInsertResult = true;
                        if (cityHourInsertResult)
                        {
                            collectTotal += listCityHour.Count;
                            //在SQLite表中录入当前采集条数
                            writeLog(rtb_Hour_Log, "<" + cityTable.Rows.Count + "个城市>小时数据采集成功，本次采集" + listCityHour.Count + "条数据", ColorEnum.Green);
                            lr.AddLogInfo(cityTable.Rows.Count + "个城市，小时数据采集成功，本次采集" + listCityHour.Count + "条数据", "", hourCity, "Info");
                        }
                        else
                        {
                            //在SQLite表中录入当前采集条数
                            writeLog(rtb_Hour_Log, "<" + cityTable.Rows.Count + "个城市>小时数据采集失败，应该采集" + listCityHour.Count + "条数据", ColorEnum.Red);
                            lr.AddLogInfo(cityTable.Rows.Count + "个城市，小时数据采集失败，应该采集" + listCityHour.Count + "条数据", "", hourCity, "Error");
                        }
                    }
                    else
                    {
                        //在SQLite表中录入当前采集条数
                        writeLog(rtb_Hour_Log, "<" + cityTable.Rows.Count + "个城市>暂无要采集小时数据", ColorEnum.Orange);
                    }
                }
                else
                {
                    //请先进性站点同步
                    writeLog(rtb_Hour_Log, "<请先进性站点同步,然后进行城市小时数据采集", ColorEnum.Orange);
                }
                #endregion

                #region 站点小时采集
                if (cityTable != null && cityTable.Rows.Count > 0)
                {
                    List<JObject> listStationHour = new List<JObject>();
                    for (int i = 0; i < cityTable.Rows.Count; i++)
                    {
                        string cityCode = cityTable.Rows[i]["CityCode"].ToString();
                        string cityName = cityTable.Rows[i]["Area"].ToString();

                        if (!String.IsNullOrEmpty(cityCode))
                        {
                            try
                            {
                                AQIDataPublishLive[] stationAQILiveData = await publishCtx.GetAreaAQIPublishLive(cityName).ResultAsync<AQIDataPublishLive[]>();
                                if (stationAQILiveData != null)
                                {
                                    IAQIDataPublishLive[] stationIAQILiveData = await publishCtx.GetAreaIaqiPublishLive(cityName).ResultAsync<IAQIDataPublishLive[]>();
                                    if (stationAQILiveData != null && stationAQILiveData.Length > 0)
                                    {
                                        for (int j = 0; j < stationAQILiveData.Count(); j++)
                                        {
                                            bool isLoadSucess = false;
                                            IAQIDataPublishLive tmpIAQIDate = null;
                                            string stationCode = stationAQILiveData[j].StationCode;
                                            string stationName = stationAQILiveData[j].PositionName;
                                            if (stationIAQILiveData != null && stationIAQILiveData.Length > 0)
                                            {
                                                for (int v = 0; v < stationIAQILiveData.Length; v++)
                                                {
                                                    if (stationIAQILiveData[v].StationCode == stationCode)
                                                    {
                                                        tmpIAQIDate = stationIAQILiveData[v];
                                                        break;
                                                    }
                                                }
                                            }

                                            if (!String.IsNullOrEmpty(stationAQILiveData[j].AQI) && stationAQILiveData[j].AQI != "—")
                                            {
                                                isLoadSucess = true;
                                                bool isCompeletCollect = smhr.IsCompeletCollect(hourStation, stationCode, stationAQILiveData[j].TimePoint);
                                                //bool isCompeletCollect = false;
                                                if (!isCompeletCollect)
                                                {
                                                    JObject item = new JObject();
                                                    item[hourStationTableFiledDic["MONITORTIME"]] = stationAQILiveData[j].TimePoint;
                                                    item[hourStationTableFiledDic["CITYCODE"]] = cityCode;
                                                    item[hourStationTableFiledDic["STATIONCODE"]] = stationAQILiveData[j].StationCode;
                                                    item[hourStationTableFiledDic["SO2_1H"]] = Utility.ConvertValueOrgin(stationAQILiveData[j].SO2);
                                                    item[hourStationTableFiledDic["SO2_1H_IAQI"]] = (tmpIAQIDate == null ? noDataValue : Utility.ConvertValueOrgin(tmpIAQIDate.ISO2));
                                                    item[hourStationTableFiledDic["NO2_1H"]] = Utility.ConvertValueOrgin(stationAQILiveData[j].NO2);
                                                    item[hourStationTableFiledDic["NO2_1H_IAQI"]] = (tmpIAQIDate == null ? noDataValue : Utility.ConvertValueOrgin(tmpIAQIDate.INO2)); ;
                                                    item[hourStationTableFiledDic["PM10_1H"]] = Utility.ConvertValueOrgin(stationAQILiveData[j].PM10);
                                                    item[hourStationTableFiledDic["PM10_1H_IAQI"]] = (tmpIAQIDate == null ? noDataValue : Utility.ConvertValueOrgin(tmpIAQIDate.IPM10)); ;
                                                    item[hourStationTableFiledDic["CO_1H"]] = Utility.ConvertValueOrgin(stationAQILiveData[j].CO);
                                                    item[hourStationTableFiledDic["CO_1H_IAQI"]] = (tmpIAQIDate == null ? noDataValue : Utility.ConvertValueOrgin(tmpIAQIDate.ICO)); ;
                                                    item[hourStationTableFiledDic["O3_1H_MAX"]] = Utility.ConvertValueOrgin(stationAQILiveData[j].O3);
                                                    item[hourStationTableFiledDic["O3_1H_MAX_IAQI"]] = (tmpIAQIDate == null ? noDataValue : Utility.ConvertValueOrgin(tmpIAQIDate.IO3)); ;
                                                    item[hourStationTableFiledDic["PM25_1H"]] = Utility.ConvertValueOrgin(stationAQILiveData[j].PM2_5);
                                                    item[hourStationTableFiledDic["PM25_1H_IAQI"]] = (tmpIAQIDate == null ? noDataValue : Utility.ConvertValueOrgin(tmpIAQIDate.IPM2_5)); ;
                                                    item[hourStationTableFiledDic["AQI"]] = Utility.ConvertValueOrgin(stationAQILiveData[j].AQI);
                                                    item[hourStationTableFiledDic["PRIMARY_POLLUTANT"]] = stationAQILiveData[j].PrimaryPollutant;
                                                    item[hourStationTableFiledDic["AQI_LEVEL"]] = Utility.AQILevelCovertInt(stationAQILiveData[j].Quality);
                                                    item["TYPE"] = 0;
                                                    if (!String.IsNullOrEmpty(hourStationTableFiledDic["MEASURE"].ToString()))
                                                    {
                                                        item[hourStationTableFiledDic["MEASURE"]] = stationAQILiveData[j].Measure;
                                                    }
                                                    listStationHour.Add(item);
                                                }
                                            }
                                            if (isLoadSucess)
                                            {
                                                writeLog(rtb_Hour_Log, cityName + "(" + stationName + "-" + stationCode + ")数据已下载，剩" + (cityTable.Rows.Count - i - 1) + "个城市。", ColorEnum.Green);
                                            }
                                            else
                                            {
                                                writeLog(rtb_Hour_Log, cityName + "(" + stationName + "-" + stationCode + ")数据下载失败，剩" + (cityTable.Rows.Count - i - 1) + "个城市。", ColorEnum.Red);
                                            }
                                        }
                                    }
                                }
                            }
                            catch (Exception ex2)
                            {
                                //日志处理
                                Loghelper.WriteErrorLog("站点小时数据采集异常", ex2);
                                lr.AddLogInfo("站点小时数据采集异常", "", hourStation, "Error");
                            }

                        }
                    }
                    if (listStationHour.Count > 0)
                    {
                        bool stationHourInsertResult = smhr.AddDataInfo(hourStation, listStationHour);
                        //bool stationHourInsertResult = true;
                        if (stationHourInsertResult)
                        {
                            collectTotal += listStationHour.Count;
                            //在SQLite表中录入当前采集条数
                            writeLog(rtb_Hour_Log, "<" + stationTable.Rows.Count + "个站点>小时数据采集成功，本次采集" + listStationHour.Count + "条数据", ColorEnum.Green);
                            lr.AddLogInfo(stationTable.Rows.Count + "个站点，小时数据采集成功，本次采集" + listStationHour.Count + "条数据", "", hourStation, "Info");
                        }
                        else
                        {
                            //在SQLite表中录入当前采集条数
                            writeLog(rtb_Hour_Log, "<" + cityTable.Rows.Count + "个站点>小时数据采集失败，应该采集" + listStationHour.Count + "条数据", ColorEnum.Red);
                            lr.AddLogInfo(cityTable.Rows.Count + "个站点，小时数据采集失败，应该采集" + listStationHour.Count + "条数据", "", hourCity, "Error");
                        }
                    }
                    else
                    {
                        //在SQLite表中录入当前采集条数
                        writeLog(rtb_Hour_Log, "<" + stationTable.Rows.Count + "个站点>暂无要采集小时数据", ColorEnum.Orange);
                    }
                }
                else
                {
                    //请先进性站点同步
                    writeLog(rtb_Hour_Log, "<请先进性站点同步,然后进行站点小时数据采集", ColorEnum.Orange);
                }
                #endregion
                //保存本次采集信息
                cst.AddStaticInfo(collectTotal, collectTime);
                writeLog(rtb_Hour_Log, "本次小时定时采集完成", ColorEnum.Blue);
                refreshStaticInfo();
            }
            catch (Exception ex)
            {
                //日志处理
                Loghelper.WriteErrorLog("数据采集异常", ex);
                lr.AddLogInfo(ex.ToString(), "", "", "Error");
            }
        }

        /// <summary>
        /// 日均数据采集总入口
        /// </summary>
        /// <returns></returns>
        public async Task collectDayDataTool()
        {

            //1.读取城市基本信息
            //2.读取站点基本信息
            //3.通过城市编号查询城市小时数据，并且通过查询到的城市小时数据中的时间和城市编码去对应的城市小时表中判断是否已经有数据，如果有数据则跳过这次采集，如果没有数据则进行采集
            //4.通过站点编号查询站点小时数据，并且通过查询到的站点小时数据中的时间和站点编码去对应的站点小时表中判断是否已经有数据，如果有数据则跳过这次采集，如果没有数据则进行采集
            //5.采集完后需要把采集结果记录在表中，并且在页面上进行展示
            try
            {
                //本次集采时间
                DateTime collectTime = DateTime.Now;
                //本次采集数据条数
                int collectTotal = 0;

                if (this.rtb_Day_Log != null)
                {
                    this.rtb_Day_Log.Clear();
                }
                writeLog(rtb_Day_Log, "开始采集数据", ColorEnum.Blue);

                //创建domain客户端
                EnvCnemcPublishDomainContext publishCtx = new EnvCnemcPublishDomainContext(XAP_URL);

                DataTable cityTable = asr.AirAreaInfoQuery();
                DataTable stationTable = asr.AirStationInfoQuery();

                #region 城市日均采集
                if (cityTable != null && cityTable.Rows.Count > 0)
                {
                    List<JObject> listCityDay = new List<JObject>();
                    for (int i = 0; i < cityTable.Rows.Count; i++)
                    {
                        string cityCode = cityTable.Rows[i]["CityCode"].ToString();
                        string cityName = cityTable.Rows[i]["Area"].ToString();
                        if (!String.IsNullOrEmpty(cityCode))
                        {
                            try
                            {
                                EntityQuery<CityDayAQIPublishLive> cityAQILiveData = publishCtx.GetCityDayAQIModelByCitycodeQuery(Int32.Parse(cityCode));
                                if (cityAQILiveData != null)
                                {
                                    IEnumerable<CityDayAQIPublishLive> cityAQILiveDataIEB = await publishCtx.Load(cityAQILiveData).ResultAsync();
                                    if (cityAQILiveDataIEB != null)
                                    {
                                        List<CityDayAQIPublishLive> cityAQILiveDataList = cityAQILiveDataIEB.ToList();
                                        if (cityAQILiveDataList != null)
                                        {
                                            for (int j = 0; j < cityAQILiveDataList.Count; j++)
                                            {
                                                if (!String.IsNullOrEmpty(cityAQILiveDataList[j].AQI) && cityAQILiveDataList[j].AQI != "—")
                                                {
                                                    bool isCompeletCollect = cmdr.IsCompeletCollect(dayCity, cityCode, cityAQILiveDataList[j].TimePoint);
                                                    if (!isCompeletCollect)
                                                    {
                                                        JObject item = new JObject();
                                                        item[dayCityTableFiledDic["MONITORTIME"]] = cityAQILiveDataList[j].TimePoint;
                                                        item[dayCityTableFiledDic["CITYCODE"]] = cityAQILiveDataList[j].CityCode;
                                                        item[dayCityTableFiledDic["SO2_24H"]] = Utility.ConvertValueOrgin(cityAQILiveDataList[j].SO2_24h);
                                                        item[dayCityTableFiledDic["SO2_24H_IAQI"]] = noDataValue;
                                                        item[dayCityTableFiledDic["NO2_24H"]] = Utility.ConvertValueOrgin(cityAQILiveDataList[j].NO2_24h);
                                                        item[dayCityTableFiledDic["NO2_24H_IAQI"]] = noDataValue;
                                                        item[dayCityTableFiledDic["PM10_24H"]] = Utility.ConvertValueOrgin(cityAQILiveDataList[j].PM10_24h);
                                                        item[dayCityTableFiledDic["PM10_24H_IAQI"]] = noDataValue;
                                                        item[dayCityTableFiledDic["CO_24H"]] = Utility.ConvertValueOrgin(cityAQILiveDataList[j].CO_24h);
                                                        item[dayCityTableFiledDic["CO_24H_IAQI"]] = noDataValue;
                                                        item[dayCityTableFiledDic["O3_8H_MAX"]] = Utility.ConvertValueOrgin(cityAQILiveDataList[j].O3_8h_24h);
                                                        item[dayCityTableFiledDic["O3_8H_MAX_IAQI"]] = noDataValue;
                                                        item[dayCityTableFiledDic["PM25_24H"]] = Utility.ConvertValueOrgin(cityAQILiveDataList[j].PM2_5_24h);
                                                        item[dayCityTableFiledDic["PM25_24H_IAQI"]] = noDataValue;
                                                        item[dayCityTableFiledDic["AQI"]] = Utility.ConvertValueOrgin(cityAQILiveDataList[j].AQI);
                                                        item[dayCityTableFiledDic["PRIMARY_POLLUTANT"]] = cityAQILiveDataList[j].PrimaryPollutant;
                                                        item[dayCityTableFiledDic["AQI_LEVEL"]] = Utility.AQILevelCovertInt(cityAQILiveDataList[j].Quality);
                                                        item["TYPE"] = 0;
                                                        if (!String.IsNullOrEmpty(dayCityTableFiledDic["MEASURE"].ToString()))
                                                        {
                                                            item[dayCityTableFiledDic["MEASURE"]] = cityAQILiveDataList[j].Measure;
                                                        }
                                                        listCityDay.Add(item);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            catch (Exception ex2)
                            {
                                //日志处理
                                Loghelper.WriteErrorLog(cityName + "日均数据采集异常", ex2);
                                lr.AddLogInfo(cityName + "日均数据采集异常", "", dayCity, "Error");
                            }
                        }
                    }
                    if (listCityDay.Count > 0)
                    {
                        bool cityDayInsertResult = cmdr.AddDataInfo(dayCity, listCityDay);
                        if (cityDayInsertResult)
                        {
                            collectTotal += listCityDay.Count;
                            //在SQLite表中录入当前采集条数
                            writeLog(rtb_Day_Log, "<" + cityTable.Rows.Count + "个城市>日均数据采集成功，本次采集" + listCityDay.Count + "条数据", ColorEnum.Green);
                            lr.AddLogInfo(cityTable.Rows.Count + "个城市，日均数据采集成功，本次采集" + listCityDay.Count + "条数据", "", dayCity, "Info");
                        }
                        else
                        {
                            //在SQLite表中录入当前采集条数
                            writeLog(rtb_Day_Log, "<" + cityTable.Rows.Count + "个城市>日均数据采集失败，应该采集" + listCityDay.Count + "条数据", ColorEnum.Red);
                            lr.AddLogInfo(cityTable.Rows.Count + "个城市，日均数据采集失败，应该采集" + listCityDay.Count + "条数据", "", hourCity, "Error");
                        }
                    }
                    else
                    {
                        //在SQLite表中录入当前采集条数
                        writeLog(rtb_Day_Log, "<" + cityTable.Rows.Count + "个城市>暂无要采集日均数据", ColorEnum.Orange);
                    }
                }
                else
                {
                    //请先进性站点同步
                    writeLog(rtb_Day_Log, "请先进性站点同步,然后进行城市日均数据采集", ColorEnum.Orange);
                }
                #endregion

                #region 站点日均采集
                if (cityTable != null && cityTable.Rows.Count > 0)
                {
                    List<JObject> listStationDay = new List<JObject>();
                    for (int i = 0; i < cityTable.Rows.Count; i++)
                    {
                        string cityCode = cityTable.Rows[i]["CityCode"].ToString();
                        string cityName = cityTable.Rows[i]["Area"].ToString();

                        DateTime dayTime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd 00:00:00.00"));

                        if (!String.IsNullOrEmpty(cityCode))
                        {
                            try
                            {
                                EntityQuery<AQIDataPublishHistory> stationAQIHistoryQuery = publishCtx.GetAQIDataPublishHistoriesQuery()
                                                           .Where(x => x.Area == cityName)
                                                           .Where(x => x.TimePoint >= dayTime && x.TimePoint <= dayTime)
                                                           .OrderBy(x => x.TimePoint);
                                if (stationAQIHistoryQuery != null)
                                {
                                    IEnumerable<AQIDataPublishHistory> stationAQIHistoryDataIEB = await publishCtx.Load(stationAQIHistoryQuery).ResultAsync();
                                    if (stationAQIHistoryDataIEB != null)
                                    {
                                        List<AQIDataPublishHistory> stationAQIHistoryDataList = stationAQIHistoryDataIEB.ToList();
                                        if (stationAQIHistoryDataList != null)
                                        {
                                            EntityQuery<IAQIDataPublishHistory> stationIAQIHistoryQuery = publishCtx.GetIAQIDataPublishHistoriesQuery()
                                            .Where(x => x.Area == cityName)
                                            .Where(x => x.TimePoint >= dayTime && x.TimePoint <= dayTime)
                                            .OrderBy(x => x.TimePoint);
                                            IEnumerable<IAQIDataPublishHistory> stationIAQIHistoryDataIEB = null;
                                            List<IAQIDataPublishHistory> stationIAQIHistoryDataList = null;
                                            if (stationIAQIHistoryQuery != null)
                                            {
                                                stationIAQIHistoryDataIEB = await publishCtx.Load(stationIAQIHistoryQuery).ResultAsync();
                                                stationIAQIHistoryDataList = stationIAQIHistoryDataIEB.ToList();
                                            }
                                            if (stationAQIHistoryDataList != null && stationAQIHistoryDataList.Count > 0)
                                            {
                                                for (int j = 0; j < stationAQIHistoryDataList.Count(); j++)
                                                {
                                                    IAQIDataPublishHistory tmpIAQIDate = null;
                                                    string stationCode = stationAQIHistoryDataList[j].StationCode;
                                                    if (stationIAQIHistoryDataList != null && stationIAQIHistoryDataList.Count > 0)
                                                    {
                                                        for (int v = 0; v < stationIAQIHistoryDataList.Count; v++)
                                                        {
                                                            if (stationIAQIHistoryDataList[v].StationCode == stationCode)
                                                            {
                                                                tmpIAQIDate = stationIAQIHistoryDataList[v];
                                                                break;
                                                            }
                                                        }
                                                    }
                                                    if (!String.IsNullOrEmpty(stationAQIHistoryDataList[j].AQI) && stationAQIHistoryDataList[j].AQI != "—")
                                                    {
                                                        bool isCompeletCollect = smdr.IsCompeletCollect(dayStation, stationCode, stationAQIHistoryDataList[j].TimePoint.AddDays(-1));
                                                        if (!isCompeletCollect)
                                                        {
                                                            JObject item = new JObject();
                                                            item[dayStationTableFiledDic["MONITORTIME"]] = stationAQIHistoryDataList[j].TimePoint.AddDays(-1);
                                                            item[dayStationTableFiledDic["CITYCODE"]] = cityCode;
                                                            item[dayStationTableFiledDic["STATIONCODE"]] = stationAQIHistoryDataList[j].StationCode;
                                                            item[dayStationTableFiledDic["SO2_24H"]] = Utility.ConvertValueOrgin(stationAQIHistoryDataList[j].SO2);
                                                            item[dayStationTableFiledDic["SO2_24H_IAQI"]] = (tmpIAQIDate == null ? noDataValue : Utility.ConvertValueOrgin(tmpIAQIDate.ISO2));
                                                            item[dayStationTableFiledDic["NO2_24H"]] = Utility.ConvertValueOrgin(stationAQIHistoryDataList[j].NO2);
                                                            item[dayStationTableFiledDic["NO2_24H_IAQI"]] = (tmpIAQIDate == null ? noDataValue : Utility.ConvertValueOrgin(tmpIAQIDate.INO2)); ;
                                                            item[dayStationTableFiledDic["PM10_24H"]] = Utility.ConvertValueOrgin(stationAQIHistoryDataList[j].PM10);
                                                            item[dayStationTableFiledDic["PM10_24H_IAQI"]] = (tmpIAQIDate == null ? noDataValue : Utility.ConvertValueOrgin(tmpIAQIDate.IPM10)); ;
                                                            item[dayStationTableFiledDic["CO_24H"]] = Utility.ConvertValueOrgin(stationAQIHistoryDataList[j].CO);
                                                            item[dayStationTableFiledDic["CO_24H_IAQI"]] = (tmpIAQIDate == null ? noDataValue : Utility.ConvertValueOrgin(tmpIAQIDate.ICO)); ;
                                                            item[dayStationTableFiledDic["O3_8H_MAX"]] = Utility.ConvertValueOrgin(stationAQIHistoryDataList[j].O3);
                                                            item[dayStationTableFiledDic["O3_8H_MAX_IAQI"]] = (tmpIAQIDate == null ? noDataValue : Utility.ConvertValueOrgin(tmpIAQIDate.IO3)); ;
                                                            item[dayStationTableFiledDic["PM25_24H"]] = Utility.ConvertValueOrgin(stationAQIHistoryDataList[j].PM2_5);
                                                            item[dayStationTableFiledDic["PM25_24H_IAQI"]] = (tmpIAQIDate == null ? noDataValue : Utility.ConvertValueOrgin(tmpIAQIDate.IPM2_5)); ;
                                                            item[dayStationTableFiledDic["AQI"]] = Utility.ConvertValueOrgin(stationAQIHistoryDataList[j].AQI);
                                                            item[dayStationTableFiledDic["PRIMARY_POLLUTANT"]] = stationAQIHistoryDataList[j].PrimaryPollutant;
                                                            item[dayStationTableFiledDic["AQI_LEVEL"]] = Utility.AQILevelCovertInt(stationAQIHistoryDataList[j].Quality);
                                                            item["TYPE"] = 0;
                                                            if (!String.IsNullOrEmpty(dayStationTableFiledDic["MEASURE"].ToString()))
                                                            {
                                                                item[dayStationTableFiledDic["MEASURE"]] = stationAQIHistoryDataList[j].Measure;
                                                            }
                                                            listStationDay.Add(item);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            catch (Exception ex2)
                            {
                                //日志处理
                                Loghelper.WriteErrorLog("站点日均数据采集异常", ex2);
                                lr.AddLogInfo("站点日均数据采集异常", "", dayStation, "Error");
                            }
                        }
                    }
                    if (listStationDay.Count > 0)
                    {
                        bool stationDayInsertResult = smdr.AddDataInfo(dayStation, listStationDay);
                        if (stationDayInsertResult)
                        {
                            collectTotal += listStationDay.Count;

                            //在SQLite表中录入当前采集条数
                            writeLog(rtb_Day_Log, "<" + stationTable.Rows.Count + "个站点>日均数据采集成功，本次采集" + listStationDay.Count + "条数据", ColorEnum.Green);
                            lr.AddLogInfo(stationTable.Rows.Count + "个站点，日均数据采集成功，本次采集" + listStationDay.Count + "条数据", "", dayStation, "Info");
                        }
                        else
                        {
                            //在SQLite表中录入当前采集条数
                            writeLog(rtb_Day_Log, "<" + stationTable.Rows.Count + "个站点>日均数据采集失败，应该采集" + listStationDay.Count + "条数据", ColorEnum.Red);
                            lr.AddLogInfo(stationTable.Rows.Count + "个站点，日均数据采集失败，应该采集" + listStationDay.Count + "条数据", "", hourCity, "Error");
                        }
                    }
                    else
                    {
                        //在SQLite表中录入当前采集条数
                        writeLog(rtb_Day_Log, "<" + stationTable.Rows.Count + "个站点>暂无要采集日均数据", ColorEnum.Orange);
                    }
                }
                else
                {
                    //请先进性站点同步
                    writeLog(rtb_Day_Log, "请先进性站点同步,然后进行站点日均数据采集", ColorEnum.Orange);
                }
                #endregion
                //保存本次采集信息
                cst.AddStaticInfo(collectTotal, collectTime);
                writeLog(rtb_Day_Log, "本次日均定时采集完成", ColorEnum.Blue);
                refreshStaticInfo();
            }
            catch (Exception ex)
            {
                //日志处理
                Loghelper.WriteErrorLog("数据采集异常", ex);
                lr.AddLogInfo(ex.ToString(), "", "", "Error");
            }
        }

        /// <summary>
        /// 采集国家环境监测总站区域预报数据
        /// </summary>
        public async Task collectAreaPrediction()
        {
            #region 区域预报
            try
            {
                //本次集采时间
                DateTime collectTime = DateTime.Now;
                //本次采集数据条数
                int collectTotal = 0;
                if (this.rtb_AreaPrediction_Log != null)
                {
                    this.rtb_AreaPrediction_Log.Clear();
                }
                writeLog(rtb_AreaPrediction_Log, "开始采集数据", ColorEnum.Blue);

                DataTable areaTable = napr.NationAreaInfoQuery();
                if (areaTable != null && areaTable.Rows.Count > 0)
                {
                    List<JObject> list = new List<JObject>();
                    for (int i = 0; i < areaTable.Rows.Count; i++)
                    {
                        AreaPrediction areaPrediction = new AreaPrediction();
                        try
                        {
                            string areaCode = areaTable.Rows[i]["AreaCode"].ToString();
                            // 调用这个接口 
                            string areaUrl = areaPredictionUrl + "?areaCode=" + areaCode + "&strForecastType=1&_=" + new DateTime().Millisecond;//  预报未来7天的数据
                            string resultAreaStr = SendHelper.SendPost(areaUrl);
                            if (!String.IsNullOrEmpty(resultAreaStr))
                            {
                                areaPrediction = JsonConvert.DeserializeObject<AreaPrediction>(resultAreaStr);
                                bool isCompeletCollect = apr.IsCompeletCollect(areaPredictionTableName, areaCode, areaPrediction.PublishDate);
                                if (!isCompeletCollect)
                                {
                                    JObject item = new JObject();
                                    item["CONTENT"] = areaPrediction.ForecastDescription;
                                    item["REGION_CODE"] = areaPrediction.AreaCode;
                                    item["WARN_TIME"] = areaPrediction.PublishDate;
                                    list.Add(item);
                                }
                            }
                            else
                            {
                                //请先进性站点同步
                                writeLog(rtb_AreaPrediction_Log, "<请先完善全国区域基本信息，然后再进行采集。", ColorEnum.Orange);
                            }
                        }
                        catch (Exception ex)
                        {
                            //日志处理
                            Loghelper.WriteErrorLog("采集国家区域预报数据异常", ex);
                        }
                    }
                    if (list.Count > 0)
                    {

                        bool areaPredictionInsertResult = apr.AddDataInfo(areaPredictionTableName, list);
                        if (areaPredictionInsertResult)
                        {
                            collectTotal += list.Count;
                            //在SQLite表中录入当前采集条数
                            writeLog(rtb_AreaPrediction_Log, "<" + areaTable.Rows.Count + "个区域>区域预报数据采集成功，本次采集" + list.Count + "条数据", ColorEnum.Green);
                            lr.AddLogInfo(areaTable.Rows.Count + "个区域，区域预报数据采集成功，本次采集" + list.Count + "条数据", "", areaPredictionTableName, "Info");
                        }
                        else
                        {
                            //在SQLite表中录入当前采集条数
                            writeLog(rtb_AreaPrediction_Log, "<" + areaTable.Rows.Count + "个区域>区域预报数据采集失败，应该采集" + list.Count + "条数据", ColorEnum.Red);
                            lr.AddLogInfo(areaTable.Rows.Count + "个区域，区域预报数据采集失败，应该采集" + list.Count + "条数据", "", areaPredictionTableName, "Error");
                        }
                    }
                    else
                    {
                        //在SQLite表中录入当前采集条数
                        writeLog(rtb_AreaPrediction_Log, "<" + areaTable.Rows.Count + "个区域>暂无要采集预报数据", ColorEnum.Orange);
                    }
                }
                else
                {
                    //请先进性站点同步
                    writeLog(rtb_AreaPrediction_Log, "<请先完善全国区域基本信息，然后再进行采集。", ColorEnum.Orange);
                }
                //保存本次采集信息
                cst.AddStaticInfo(collectTotal, collectTime);
                writeLog(rtb_AreaPrediction_Log, "本次国家区域预报定时采集完成", ColorEnum.Blue);
                refreshStaticInfo();
            }
            catch (Exception e)
            {
                //日志处理
                Loghelper.WriteErrorLog("采集国家区域预报数据异常", e);
                lr.AddLogInfo(e.ToString(), "", "", "Error");
            }
            #endregion
        }

        /// <summary>
        /// 采集国家环境监测总站省域预报数据
        /// </summary>
        public async Task collectProvincePrediction()
        {
            #region 省域预报
            try
            {
                //本次集采时间
                DateTime collectTime = DateTime.Now;
                //本次采集数据条数
                int collectTotal = 0;
                if (this.rtb_AreaPrediction_Log != null)
                {
                    this.rtb_AreaPrediction_Log.Clear();
                }
                writeLog(rtb_AreaPrediction_Log, "开始采集数据", ColorEnum.Blue);
                //调用这个接口 
                string provinceUrl = provincePredictionUrl;//  预报未来3天的数据
                string resultProvinceStr = SendHelper.SendPost(provinceUrl);
                if (!String.IsNullOrEmpty(resultProvinceStr))
                {
                    List<JObject> list = new List<JObject>();
                    List<ProvincePrediction> provinceList = JsonConvert.DeserializeObject<List<ProvincePrediction>>(resultProvinceStr);
                    for (int i = 0; i < provinceList.Count; i++)
                    {
                        bool isCompeletCollect = ppr.IsCompeletCollect(provincePredictionTableName, provinceList[i].ProvinceCode, provinceList[i].PublishDate);
                        if (!isCompeletCollect)
                        {
                            JObject item = new JObject();
                            item["AREA"] = provinceList[i].ProvinceCode;
                            item["PREDICTION_INFO"] = provinceList[i].ForecastDescription;
                            item["PREDICT_TIME"] = provinceList[i].PublishDate;
                            list.Add(item);
                        }
                    }
                    if (list.Count > 0)
                    {
                        bool provincePredictionInsertResult = apr.AddDataInfo(provincePredictionTableName, list);
                        if (provincePredictionInsertResult)
                        {
                            collectTotal += list.Count;
                            //在SQLite表中录入当前采集条数
                            writeLog(rtb_AreaPrediction_Log, "<" + provinceList.Count + "个省域>省域预报数据采集成功，本次采集" + list.Count + "条数据", ColorEnum.Green);
                            lr.AddLogInfo(provinceList.Count + "个省域，省域预报数据采集成功，本次采集" + list.Count + "条数据", "", provincePredictionTableName, "Info");
                        }
                        else
                        {
                            //在SQLite表中录入当前采集条数
                            writeLog(rtb_AreaPrediction_Log, "<" + provinceList.Count + "个省域>省域预报数据采集成功，应该采集" + list.Count + "条数据", ColorEnum.Red);
                            lr.AddLogInfo(provinceList.Count + "个省域，省域预报数据采集失败，应该采集" + list.Count + "条数据", "", provincePredictionTableName, "Error");
                        }
                    }
                    else
                    {
                        //在SQLite表中录入当前采集条数
                        writeLog(rtb_AreaPrediction_Log, "暂无要采集省域预报数据", ColorEnum.Orange);
                    }
                }
                else
                {
                    lr.AddLogInfo("采集省域预报数据失败", "", "", "Error");
                }
                //保存本次采集信息
                cst.AddStaticInfo(collectTotal, collectTime);
                writeLog(rtb_AreaPrediction_Log, "本次国家省域预报定时采集完成", ColorEnum.Blue);
                refreshStaticInfo();
            }
            catch (Exception e)
            {
                //日志处理
                Loghelper.WriteErrorLog("采集省域预报数据异常", e);
                lr.AddLogInfo(e.ToString(), "", "", "Error");
            }
            #endregion
        }

        /// <summary>
        /// 采集国家环境监测总站城市预报数据
        /// </summary>
        public async Task collectCityPrediction()
        {
            #region 城市预报
            try
            {
                //本次集采时间
                DateTime collectTime = DateTime.Now;
                //本次采集数据条数
                int collectTotal = 0;
                if (this.rtb_AreaPrediction_Log != null)
                {
                    this.rtb_AreaPrediction_Log.Clear();
                }
                writeLog(rtb_AreaPrediction_Log, "开始采集数据", ColorEnum.Blue);
                //预报未来5天的数据
                WebClient webClient = new WebClient();
                String url = cityPredictionUrl + "?_=" + new DateTime().Millisecond;//1565404428085
                String HtmlString = Encoding.GetEncoding("utf-8").GetString(webClient.DownloadData(url));
                NSoup.Nodes.Document doc = NSoup.NSoupClient.Parse(HtmlString);
                Elements ele = doc.GetElementsByTag("script").Eq(12);
                if (null != ele)
                {
                    string[] sArray = Regex.Split(ele.ElementAt(0).Data, "var", RegexOptions.IgnoreCase);
                    String cityData = sArray[3].Replace("\n", "").Replace("\t", "").Replace("\r", "");
                    cityData = cityData.Substring(cityData.IndexOf("[") + 1, cityData.IndexOf("]") + 1).Replace("][0],", "");
                    List<JObject> list = new List<JObject>();
                    List<CityPrediction> cityList = JsonConvert.DeserializeObject<List<CityPrediction>>(cityData);
                    for (int i = 0; i < cityList.Count; i++)
                    {
                        DateTime publihsTime = DateTime.Now;
                        if (publihsTime.Hour >= 15)
                        {
                            publihsTime = publihsTime.AddDays(1);
                        }
                        bool isCompeletCollect = cpr.IsCompeletCollect(cityPredictionTableName, cityList[i].CityCode, DateTime.Parse(publihsTime.AddDays(-1).ToString("yyyy-MM-dd 00:00:00.00")), DateTime.Parse(publihsTime.ToString("yyyy-MM-dd 00:00:00.00")));
                        if (!isCompeletCollect)
                        {
                            JObject item1 = new JObject();
                            item1["PUBLISH_TIME"] = publihsTime.AddDays(-1).ToString("yyyy-MM-dd 00:00:00.00");
                            item1["REGION_CODE"] = cityList[i].CityCode;
                            item1["REGION_NAME"] = cityList[i].Name;
                            item1["PREDICT_TIME"] = publihsTime.ToString("yyyy-MM-dd 00:00:00.00");
                            item1["AQI_MIN"] = (String.IsNullOrEmpty(cityList[i].AirIndex_From) ? "-1" : cityList[i].AirIndex_From);
                            item1["AQI_LEVEL_MIN"] = (String.IsNullOrEmpty(cityList[i].AirIndex_From) ? "-1" : Utility.AQILevelCovertInt(Utility.GetPollutantLevel(Int32.Parse(cityList[i].AirIndex_From))));
                            item1["PRIMARY_POLLUTE"] = cityList[i].PrimaryPollutant;
                            item1["AQI_MAX"] = (String.IsNullOrEmpty(cityList[i].AirIndex_To) ? "-1" : cityList[i].AirIndex_To);
                            item1["AQI_LEVEL_MAX"] = (String.IsNullOrEmpty(cityList[i].AirIndex_To) ? "-1" : Utility.AQILevelCovertInt(Utility.GetPollutantLevel(Int32.Parse(cityList[i].AirIndex_To))));
                            item1["PREDICTION_INTERVAL"] = 1;
                            item1["POTENTIAL_ANALYSIS"] = cityList[i].DetailInfo;
                            list.Add(item1);

                            JObject item2 = new JObject();
                            item2["PUBLISH_TIME"] = publihsTime.AddDays(-1).ToString("yyyy-MM-dd 00:00:00.00");
                            item2["REGION_CODE"] = cityList[i].CityCode;
                            item2["REGION_NAME"] = cityList[i].Name;
                            item2["PREDICT_TIME"] = publihsTime.AddDays(1).ToString("yyyy-MM-dd 00:00:00.00");
                            item2["AQI_MIN"] = (String.IsNullOrEmpty(cityList[i].Air48Index_From) ? "-1" : cityList[i].Air48Index_From);
                            item2["AQI_LEVEL_MIN"] = (String.IsNullOrEmpty(cityList[i].Air48Index_From) ? "-1" : Utility.AQILevelCovertInt(Utility.GetPollutantLevel(Int32.Parse(cityList[i].Air48Index_From))));
                            item2["PRIMARY_POLLUTE"] = cityList[i].Primary48Pollutant;
                            item2["AQI_MAX"] = (String.IsNullOrEmpty(cityList[i].Air48Index_To) ? "-1" : cityList[i].Air48Index_To);
                            item2["AQI_LEVEL_MAX"] = (String.IsNullOrEmpty(cityList[i].Air48Index_To) ? "-1" : Utility.AQILevelCovertInt(Utility.GetPollutantLevel(Int32.Parse(cityList[i].Air48Index_To))));
                            item2["PREDICTION_INTERVAL"] = 2;
                            item2["POTENTIAL_ANALYSIS"] = cityList[i].DetailInfo;
                            list.Add(item2);

                            JObject item3 = new JObject();
                            item3["PUBLISH_TIME"] = publihsTime.AddDays(-1).ToString("yyyy-MM-dd 00:00:00.00");
                            item3["REGION_CODE"] = cityList[i].CityCode;
                            item3["REGION_NAME"] = cityList[i].Name;
                            item3["PREDICT_TIME"] = publihsTime.AddDays(2).ToString("yyyy-MM-dd 00:00:00.00");
                            item3["AQI_MIN"] = (String.IsNullOrEmpty(cityList[i].Air72Index_From) ? "-1" : cityList[i].Air72Index_From);
                            item3["AQI_LEVEL_MIN"] = (String.IsNullOrEmpty(cityList[i].Air72Index_From) ? "-1" : Utility.AQILevelCovertInt(Utility.GetPollutantLevel(Int32.Parse(cityList[i].Air72Index_From))));
                            item3["PRIMARY_POLLUTE"] = cityList[i].Primary72Pollutant;
                            item3["AQI_MAX"] = (String.IsNullOrEmpty(cityList[i].Air72Index_To) ? "-1" : cityList[i].Air72Index_To);
                            item3["AQI_LEVEL_MAX"] = (String.IsNullOrEmpty(cityList[i].Air72Index_To) ? "-1" : Utility.AQILevelCovertInt(Utility.GetPollutantLevel(Int32.Parse(cityList[i].Air72Index_To))));
                            item3["PREDICTION_INTERVAL"] = 3;
                            item3["POTENTIAL_ANALYSIS"] = cityList[i].DetailInfo;
                            list.Add(item3);

                            JObject item4 = new JObject();
                            item4["PUBLISH_TIME"] = publihsTime.AddDays(-1).ToString("yyyy-MM-dd 00:00:00.00");
                            item4["REGION_CODE"] = cityList[i].CityCode;
                            item4["REGION_NAME"] = cityList[i].Name;
                            item4["PREDICT_TIME"] = publihsTime.AddDays(3).ToString("yyyy-MM-dd 00:00:00.00");
                            item4["AQI_MIN"] = (String.IsNullOrEmpty(cityList[i].Air96Index_From) ? "-1" : cityList[i].Air96Index_From);
                            item4["AQI_LEVEL_MIN"] = (String.IsNullOrEmpty(cityList[i].Air96Index_From) ? "-1" : Utility.AQILevelCovertInt(Utility.GetPollutantLevel(Int32.Parse(cityList[i].Air96Index_From))));
                            item4["PRIMARY_POLLUTE"] = cityList[i].Primary96Pollutant;
                            item4["AQI_MAX"] = (String.IsNullOrEmpty(cityList[i].Air96Index_To) ? "-1" : cityList[i].Air96Index_To);
                            item4["AQI_LEVEL_MAX"] = (String.IsNullOrEmpty(cityList[i].Air96Index_To) ? "-1" : Utility.AQILevelCovertInt(Utility.GetPollutantLevel(Int32.Parse(cityList[i].Air96Index_To))));
                            item4["PREDICTION_INTERVAL"] = 4;
                            item4["POTENTIAL_ANALYSIS"] = cityList[i].DetailInfo;
                            list.Add(item4);

                            JObject item5 = new JObject();
                            item5["PUBLISH_TIME"] = publihsTime.AddDays(-1).ToString("yyyy-MM-dd 00:00:00.00");
                            item5["REGION_CODE"] = cityList[i].CityCode;
                            item5["REGION_NAME"] = cityList[i].Name;
                            item5["PREDICT_TIME"] = publihsTime.AddDays(4).ToString("yyyy-MM-dd 00:00:00.00");
                            item5["AQI_MIN"] = (String.IsNullOrEmpty(cityList[i].Air120Index_From) ? "-1" : cityList[i].Air120Index_From);
                            item5["AQI_LEVEL_MIN"] = (String.IsNullOrEmpty(cityList[i].Air120Index_From) ? "-1" : Utility.AQILevelCovertInt(Utility.GetPollutantLevel(Int32.Parse(cityList[i].Air120Index_From))));
                            item5["PRIMARY_POLLUTE"] = cityList[i].Primary120Pollutant;
                            item5["AQI_MAX"] = (String.IsNullOrEmpty(cityList[i].Air120Index_To) ? "-1" : cityList[i].Air120Index_To);
                            item5["AQI_LEVEL_MAX"] = (String.IsNullOrEmpty(cityList[i].Air120Index_To) ? "-1" : Utility.AQILevelCovertInt(Utility.GetPollutantLevel(Int32.Parse(cityList[i].Air120Index_To))));
                            item5["PREDICTION_INTERVAL"] = 5;
                            item5["POTENTIAL_ANALYSIS"] = cityList[i].DetailInfo;
                            list.Add(item5);

                        }
                    }
                    if (list.Count > 0)
                    {
                        bool cityPredictionInsertResult = cpr.AddDataInfo(cityPredictionTableName, list);
                        if (cityPredictionInsertResult)
                        {
                            collectTotal += list.Count;
                            //在SQLite表中录入当前采集条数
                            writeLog(rtb_AreaPrediction_Log, "<" + list.Count + "个城市>城市预报数据采集成功，本次采集" + list.Count + "条数据", ColorEnum.Green);
                            lr.AddLogInfo(list.Count + "个城市，城市预报数据采集成功，本次采集" + list.Count + "条数据", "", cityPredictionTableName, "Info");
                        }
                        else
                        {
                            //在SQLite表中录入当前采集条数
                            writeLog(rtb_AreaPrediction_Log, "<" + list.Count + "个城市>城市预报数据采集失败，应该采集" + list.Count + "条数据", ColorEnum.Red);
                            lr.AddLogInfo(list.Count + "个城市，城市预报数据采集失败，应该采集" + list.Count + "条数据", "", cityPredictionTableName, "Error");
                        }
                    }
                    else
                    {
                        //在SQLite表中录入当前采集条数
                        writeLog(rtb_AreaPrediction_Log, "暂无要采集城市预报数据", ColorEnum.Orange);
                    }
                }
                else
                {

                }
                //保存本次采集信息
                cst.AddStaticInfo(collectTotal, collectTime);
                writeLog(rtb_AreaPrediction_Log, "本次国家城市预报定时采集完成", ColorEnum.Blue);
                refreshStaticInfo();
            }
            catch (Exception e)
            {
                //日志处理
                Loghelper.WriteErrorLog("采集国家城市预报数据异常", e);
                lr.AddLogInfo(e.ToString(), "", "", "Error");
            }
            #endregion
        }

        /// <summary>
        /// 采集中央气象台小时数据
        /// </summary>
        /// <returns></returns>
        public async Task collectHourWeatherDataTool()
        {
            #region 气象小时
            try
            {
                //本次集采时间
                DateTime collectTime = DateTime.Now;
                string dateStr = collectTime.ToString("yyyyMMddHH");
                DateTime monitorTime = DateTime.Parse(collectTime.ToString("yyyy-MM-dd HH:00:00"));
                //本次采集数据条数
                int collectTotal = 0;
                if (this.rtb_WeatherHour_Log != null)
                {
                    this.rtb_WeatherHour_Log.Clear();
                }
                DataTable dt = wst.StationInfoQuery();
                writeLog(rtb_WeatherHour_Log, "开始采集数据", ColorEnum.Blue);
                List<JObject> stationList = new List<JObject>();
                List<JObject> ciytList = new List<JObject>();
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string stationCode = dt.Rows[i]["UniqueCode"].ToString();
                        string cityCode = dt.Rows[i]["CityCode"].ToString();
                        string type = dt.Rows[i]["type"].ToString();
                        switch (type)
                        {
                            case "1":
                                bool isStationCompeletCollect = wshr.IsCompeletCollect(weatherStationHourTableName, stationCode, monitorTime);
                                if (!isStationCompeletCollect)
                                {
                                    string resultStr = SendHelper.SendPost(weatherServerUrl + dateStr + "/" + stationCode);
                                    List<JObject> listResult = JsonConvert.DeserializeObject<List<JObject>>(resultStr);
                                    if (listResult != null && listResult.Count > 0)
                                    {
                                        JObject item = new JObject();
                                        item["MONITOR_TIME"] = monitorTime;
                                        item["CITY_CODE"] = cityCode;
                                        item["SITE_CODE"] = stationCode;
                                        item["WIND_SPEED"] = Utility.ConvertValueOrgin(listResult[0]["windSpeed"] != null ? listResult[0]["windSpeed"].ToString() : "");
                                        item["WIND_DIRECT"] = Utility.ConvertValueOrgin(listResult[0]["windDirection"] != null ? listResult[0]["windDirection"].ToString() : "");
                                        item["TEMP"] = Utility.ConvertValueOrgin(listResult[0]["temperature"] != null ? listResult[0]["temperature"].ToString() : "");
                                        item["HUM"] = Utility.ConvertValueOrgin(listResult[0]["humidity"] != null ? listResult[0]["humidity"].ToString() : "");
                                        item["PRES"] = Utility.ConvertValueOrgin(listResult[0]["pressure"] != null ? listResult[0]["pressure"].ToString() : "");
                                        item["RAIN"] = Utility.ConvertValueOrgin(listResult[0]["rain1h"] != null ? listResult[0]["rain1h"].ToString() : "");
                                        stationList.Add(item);
                                    }
                                }
                                break;
                            case "2":

                                bool isCityCompeletCollect = wchr.IsCompeletCollect(weatherCityHourTableName, stationCode, monitorTime);
                                if (!isCityCompeletCollect)
                                {
                                    string resultStr = SendHelper.SendPost(weatherServerUrl + dateStr + "/" + stationCode);
                                    List<JObject> listResult = JsonConvert.DeserializeObject<List<JObject>>(resultStr);
                                    if (listResult != null && listResult.Count > 0)
                                    {
                                        JObject item = new JObject();
                                        item["MONITOR_TIME"] = monitorTime;
                                        item["CITY_CODE"] = cityCode;
                                        item["WIND_SPEED"] = Utility.ConvertValueOrgin(listResult[0]["windSpeed"] != null ? listResult[0]["windSpeed"].ToString() : "");
                                        item["WIND_DIRECT"] = Utility.ConvertValueOrgin(listResult[0]["windDirection"] != null ? listResult[0]["windDirection"].ToString() : "");
                                        item["TEMP"] = Utility.ConvertValueOrgin(listResult[0]["temperature"] != null ? listResult[0]["temperature"].ToString() : "");
                                        item["HUM"] = Utility.ConvertValueOrgin(listResult[0]["humidity"] != null ? listResult[0]["humidity"].ToString() : "");
                                        item["PRES"] = Utility.ConvertValueOrgin(listResult[0]["pressure"] != null ? listResult[0]["pressure"].ToString() : "");
                                        item["RAIN"] = Utility.ConvertValueOrgin(listResult[0]["rain1h"] != null ? listResult[0]["rain1h"].ToString() : "");
                                        ciytList.Add(item);
                                    }
                                }
                                break;
                        }
                    }
                    if (stationList.Count > 0)
                    {
                        bool stationHourInsertResult = wshr.AddDataInfo(weatherStationHourTableName, stationList);
                        if (stationHourInsertResult)
                        {
                            collectTotal += stationList.Count;
                            //在SQLite表中录入当前采集条数
                            writeLog(rtb_WeatherHour_Log, "<" + stationList.Count + "个站点>气象站点数据采集成功，本次采集" + stationList.Count + "条数据", ColorEnum.Green);
                            lr.AddLogInfo(stationList.Count + "个站点，气象站点数据采集成功，本次采集" + stationList.Count + "条数据", "", weatherStationHourTableName, "Info");
                        }
                        else
                        {
                            //在SQLite表中录入当前采集条数
                            writeLog(rtb_WeatherHour_Log, "<" + stationList.Count + "个站点>气象站点数据采集失败，应该采集" + stationList.Count + "条数据", ColorEnum.Red);
                            lr.AddLogInfo(stationList.Count + "个站点，气象站点数据采集失败，应该采集" + stationList.Count + "条数据", "", weatherStationHourTableName, "Error");
                        }
                    }
                    else
                    {
                        //在SQLite表中录入当前采集条数
                        writeLog(rtb_WeatherHour_Log, "暂无要采集站点小时数据", ColorEnum.Orange);
                    }

                    if (ciytList.Count > 0)
                    {
                        bool stationHourInsertResult = wchr.AddDataInfo(weatherCityHourTableName, ciytList);
                        if (stationHourInsertResult)
                        {
                            collectTotal += ciytList.Count;
                            //在SQLite表中录入当前采集条数
                            writeLog(rtb_WeatherHour_Log, "<" + ciytList.Count + "个城市>气象城市数据采集成功，本次采集" + ciytList.Count + "条数据", ColorEnum.Green);
                            lr.AddLogInfo(ciytList.Count + "个城市，气象城市数据采集成功，本次采集" + ciytList.Count + "条数据", "", weatherCityHourTableName, "Info");
                        }
                        else
                        {
                            //在SQLite表中录入当前采集条数
                            writeLog(rtb_WeatherHour_Log, "<" + ciytList.Count + "个城市>气象城市数据采集失败，应该采集" + ciytList.Count + "条数据", ColorEnum.Red);
                            lr.AddLogInfo(ciytList.Count + "个城市，气象城市数据采集失败，应该采集" + ciytList.Count + "条数据", "", weatherCityHourTableName, "Error");
                        }
                    }
                    else
                    {
                        //在SQLite表中录入当前采集条数
                        writeLog(rtb_WeatherHour_Log, "暂无要采集城市小时数据", ColorEnum.Orange);
                    }
                }
                else
                {
                    //在SQLite表中录入当前采集条数
                    writeLog(rtb_WeatherHour_Log, "站点基础数据为空，请先完善基础数据再采集", ColorEnum.Orange);
                }
                //保存本次采集信息
                cst.AddStaticInfo(collectTotal, collectTime);
                writeLog(rtb_WeatherHour_Log, "本次气象小时数据定时采集完成", ColorEnum.Blue);
                refreshStaticInfo();
            }
            catch (Exception e)
            {
                //日志处理
                Loghelper.WriteErrorLog("采集气象小时数据异常", e);
                lr.AddLogInfo(e.ToString(), "", "", "Error");
            }
            #endregion
        }

        /// <summary>
        /// 采集中央气象台日均数据
        /// </summary>
        /// <returns></returns>
        public async Task collectDayWeatherDataTool()
        {
            #region 气象日均
            try
            {
                //本次集采时间
                DateTime collectTime = DateTime.Now.AddDays(-3);
                string dateStr = collectTime.ToString("yyyyMMdd23");
                DateTime monitorTime = DateTime.Parse(collectTime.ToString("yyyy-MM-dd 00:00:00"));
                //本次采集数据条数
                int collectTotal = 0;
                if (this.rtb_WeatherDay_Log != null)
                {
                    this.rtb_WeatherDay_Log.Clear();
                }
                DataTable dt = wst.StationInfoQuery();
                writeLog(rtb_WeatherDay_Log, "开始采集数据", ColorEnum.Blue);
                List<JObject> stationList = new List<JObject>();
                List<JObject> cityList = new List<JObject>();
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string stationCode = dt.Rows[i]["UniqueCode"].ToString();
                        string cityCode = dt.Rows[i]["CityCode"].ToString();
                        string type = dt.Rows[i]["type"].ToString();
                        switch (type)
                        {
                            case "1":
                                bool isStationCompeletCollect = wsdr.IsCompeletCollect(weatherStationDayTableName, stationCode, monitorTime);
                                if (!isStationCompeletCollect)
                                {
                                    string resultStr = SendHelper.SendPost(weatherServerUrl + dateStr + "/" + stationCode);
                                    List<JObject> listResult = JsonConvert.DeserializeObject<List<JObject>>(resultStr);
                                    if (listResult != null && listResult.Count > 0 && listResult.Count == 24)
                                    {
                                        List<double> windSpeedList = new List<double>();
                                        List<double> windDirectionList = new List<double>();
                                        List<double> temperatureList = new List<double>();
                                        List<double> humidityList = new List<double>();
                                        List<double> pressureList = new List<double>();
                                        List<double> rain1hList = new List<double>();
                                        for (int j = 0; j < listResult.Count; j++)
                                        {
                                            string tempWindSpeed = Utility.ConvertValueOrgin(listResult[j]["windSpeed"] != null ? listResult[j]["windSpeed"].ToString() : "");
                                            if (tempWindSpeed != noDataValue)
                                            {
                                                windSpeedList.Add(Double.Parse(tempWindSpeed));
                                            }
                                            string temWindDirection = Utility.ConvertValueOrgin(listResult[j]["windDirection"] != null ? listResult[j]["windDirection"].ToString() : "");
                                            if (temWindDirection != noDataValue)
                                            {
                                                windDirectionList.Add(Double.Parse(temWindDirection));
                                            }
                                            string temTemperature = Utility.ConvertValueOrgin(listResult[j]["temperature"] != null ? listResult[j]["temperature"].ToString() : "");
                                            if (temTemperature != noDataValue)
                                            {
                                                temperatureList.Add(Double.Parse(temTemperature));
                                            }
                                            string temHumidity = Utility.ConvertValueOrgin(listResult[j]["humidity"] != null ? listResult[j]["humidity"].ToString() : "");
                                            if (temHumidity != noDataValue)
                                            {
                                                humidityList.Add(Double.Parse(temHumidity));
                                            }
                                            string temPressure = Utility.ConvertValueOrgin(listResult[j]["pressure"] != null ? listResult[j]["pressure"].ToString() : "");
                                            if (temPressure != noDataValue)
                                            {
                                                pressureList.Add(Double.Parse(temPressure));
                                            }
                                            string temRain1h = Utility.ConvertValueOrgin(listResult[j]["rain1h"] != null ? listResult[j]["rain1h"].ToString() : "");
                                            if (temRain1h != noDataValue)
                                            {
                                                rain1hList.Add(Double.Parse(temRain1h));
                                            }
                                        }
                                        JObject item = new JObject();
                                        item["MONITOR_TIME"] = monitorTime;
                                        item["CITY_CODE"] = cityCode;
                                        item["SITE_CODE"] = stationCode;
                                        item["WIND_SPEED"] = (windSpeedList.Count > 0 ? Decimal.Round(Decimal.Parse(windSpeedList.Average().ToString()), 1).ToString() : noDataValue);
                                        item["WIND_DIRECT"] = (windDirectionList.Count > 0 ? Decimal.Round(Decimal.Parse(windDirectionList.Average().ToString()), 0).ToString() : noDataValue);
                                        item["TEMP"] = (temperatureList.Count > 0 ? Decimal.Round(Decimal.Parse(temperatureList.Average().ToString()), 1).ToString() : noDataValue);
                                        item["MIN_TEMP"] = (temperatureList.Count > 0 ? Decimal.Round(Decimal.Parse(temperatureList.Min().ToString()), 1).ToString() : noDataValue);
                                        item["MAX_TEMP"] = (temperatureList.Count > 0 ? Decimal.Round(Decimal.Parse(temperatureList.Max().ToString()), 1).ToString() : noDataValue);
                                        item["HUM"] = (humidityList.Count > 0 ? Decimal.Round(Decimal.Parse(humidityList.Average().ToString()), 0).ToString() : noDataValue);
                                        item["PRES"] = (pressureList.Count > 0 ? Decimal.Round(Decimal.Parse(pressureList.Average().ToString()), 0).ToString() : noDataValue);
                                        item["RAIN"] = (rain1hList.Count > 0 ? Decimal.Round(Decimal.Parse(rain1hList.Average().ToString()), 1).ToString() : noDataValue);
                                        stationList.Add(item);
                                    }
                                }
                                break;
                            case "2":
                                bool isCityCompeletCollect = wcdr.IsCompeletCollect(weatherCityDayTableName, cityCode, monitorTime);
                                if (!isCityCompeletCollect)
                                {
                                    string resultStr = SendHelper.SendPost(weatherServerUrl + dateStr + "/" + stationCode);
                                    List<JObject> listResult = JsonConvert.DeserializeObject<List<JObject>>(resultStr);
                                    if (listResult != null && listResult.Count > 0 && listResult.Count == 24)
                                    {
                                        List<double> windSpeedList = new List<double>();
                                        List<double> windDirectionList = new List<double>();
                                        List<double> temperatureList = new List<double>();
                                        List<double> humidityList = new List<double>();
                                        List<double> pressureList = new List<double>();
                                        List<double> rain1hList = new List<double>();
                                        for (int j = 0; j < listResult.Count; j++)
                                        {
                                            string tempWindSpeed = Utility.ConvertValueOrgin(listResult[j]["windSpeed"] != null ? listResult[j]["windSpeed"].ToString() : "");
                                            if (tempWindSpeed != noDataValue)
                                            {
                                                windSpeedList.Add(Double.Parse(tempWindSpeed));
                                            }
                                            string temWindDirection = Utility.ConvertValueOrgin(listResult[j]["windDirection"] != null ? listResult[j]["windDirection"].ToString() : "");
                                            if (temWindDirection != noDataValue)
                                            {
                                                windDirectionList.Add(Double.Parse(temWindDirection));
                                            }
                                            string temTemperature = Utility.ConvertValueOrgin(listResult[j]["temperature"] != null ? listResult[j]["temperature"].ToString() : "");
                                            if (temTemperature != noDataValue)
                                            {
                                                temperatureList.Add(Double.Parse(temTemperature));
                                            }
                                            string temHumidity = Utility.ConvertValueOrgin(listResult[j]["humidity"] != null ? listResult[j]["humidity"].ToString() : "");
                                            if (temHumidity != noDataValue)
                                            {
                                                humidityList.Add(Double.Parse(temHumidity));
                                            }
                                            string temPressure = Utility.ConvertValueOrgin(listResult[j]["pressure"] != null ? listResult[j]["pressure"].ToString() : "");
                                            if (temPressure != noDataValue)
                                            {
                                                pressureList.Add(Double.Parse(temPressure));
                                            }
                                            string temRain1h = Utility.ConvertValueOrgin(listResult[j]["rain1h"] != null ? listResult[j]["rain1h"].ToString() : "");
                                            if (temRain1h != noDataValue)
                                            {
                                                rain1hList.Add(Double.Parse(temRain1h));
                                            }
                                        }
                                        JObject item = new JObject();
                                        item["MONITOR_TIME"] = monitorTime;
                                        item["CITY_CODE"] = cityCode;
                                        item["WIND_SPEED"] = (windSpeedList.Count > 0 ? Decimal.Round(Decimal.Parse(windSpeedList.Average().ToString()), 1).ToString() : noDataValue);
                                        item["WIND_DIRECT"] = (windDirectionList.Count > 0 ? Decimal.Round(Decimal.Parse(windDirectionList.Average().ToString()), 0).ToString() : noDataValue);
                                        item["TEMP"] = (temperatureList.Count > 0 ? Decimal.Round(Decimal.Parse(temperatureList.Average().ToString()), 1).ToString() : noDataValue);
                                        item["MIN_TEMP"] = (temperatureList.Count > 0 ? Decimal.Round(Decimal.Parse(temperatureList.Min().ToString()), 1).ToString() : noDataValue);
                                        item["MAX_TEMP"] = (temperatureList.Count > 0 ? Decimal.Round(Decimal.Parse(temperatureList.Max().ToString()), 1).ToString() : noDataValue);
                                        item["HUM"] = (humidityList.Count > 0 ? Decimal.Round(Decimal.Parse(humidityList.Average().ToString()), 0).ToString() : noDataValue);
                                        item["PRES"] = (pressureList.Count > 0 ? Decimal.Round(Decimal.Parse(pressureList.Average().ToString()), 0).ToString() : noDataValue);
                                        item["RAIN"] = (rain1hList.Count > 0 ? Decimal.Round(Decimal.Parse(rain1hList.Average().ToString()), 1).ToString() : noDataValue);
                                        cityList.Add(item);
                                    }
                                }
                                break;
                        }

                    }
                    if (stationList.Count > 0)
                    {
                        bool stationDayInsertResult = wsdr.AddDataInfo(weatherStationDayTableName, stationList);
                        if (stationDayInsertResult)
                        {
                            collectTotal += stationList.Count;
                            //在SQLite表中录入当前采集条数
                            writeLog(rtb_WeatherDay_Log, "<" + stationList.Count + "个站点>气象站点数据采集成功，本次采集" + stationList.Count + "条数据", ColorEnum.Green);
                            lr.AddLogInfo(stationList.Count + "个站点，气象站点数据采集成功，本次采集" + stationList.Count + "条数据", "", weatherStationDayTableName, "Info");
                        }
                        else
                        {
                            //在SQLite表中录入当前采集条数
                            writeLog(rtb_WeatherDay_Log, "<" + stationList.Count + "个站点>气象站点数据采集失败，应该采集" + stationList.Count + "条数据", ColorEnum.Red);
                            lr.AddLogInfo(stationList.Count + "个站点，气象站点数据采集失败，应该采集" + stationList.Count + "条数据", "", weatherStationDayTableName, "Error");
                        }
                    }
                    else
                    {
                        //在SQLite表中录入当前采集条数
                        writeLog(rtb_WeatherDay_Log, "暂无要采集站点小时数据", ColorEnum.Orange);
                    }

                    if (cityList.Count > 0)
                    {
                        bool cityDayInsertResult = wcdr.AddDataInfo(weatherCityDayTableName, cityList);
                        if (cityDayInsertResult)
                        {
                            collectTotal += cityList.Count;
                            //在SQLite表中录入当前采集条数
                            writeLog(rtb_WeatherDay_Log, "<" + cityList.Count + "个城市>气象城市数据采集成功，本次采集" + cityList.Count + "条数据", ColorEnum.Green);
                            lr.AddLogInfo(cityList.Count + "个城市，气象城市数据采集成功，本次采集" + cityList.Count + "条数据", "", weatherCityDayTableName, "Info");
                        }
                        else
                        {
                            //在SQLite表中录入当前采集条数
                            writeLog(rtb_WeatherDay_Log, "<" + cityList.Count + "个城市>气象城市数据采集失败，应该采集" + cityList.Count + "条数据", ColorEnum.Red);
                            lr.AddLogInfo(cityList.Count + "个城市，气象城市数据采集失败，应该采集" + cityList.Count + "条数据", "", weatherCityDayTableName, "Error");
                        }
                    }
                    else
                    {
                        //在SQLite表中录入当前采集条数
                        writeLog(rtb_WeatherDay_Log, "暂无要采集城市日均数据", ColorEnum.Orange);
                    }
                }
                else
                {
                    //在SQLite表中录入当前采集条数
                    writeLog(rtb_WeatherDay_Log, "城市基础数据为空，请先完善基础数据再采集", ColorEnum.Orange);
                }
                //保存本次采集信息
                cst.AddStaticInfo(collectTotal, collectTime);
                writeLog(rtb_WeatherDay_Log, "本次气象日均数据定时采集完成", ColorEnum.Blue);
                refreshStaticInfo();
            }
            catch (Exception e)
            {
                //日志处理
                Loghelper.WriteErrorLog("采集气象小时数据异常", e);
                lr.AddLogInfo(e.ToString(), "", "", "Error");
            }
            #endregion
        }

        public void refreshStaticInfo()
        {
            try
            {
                JObject objItem = cst.GetCollectInfo();
                lb_TodayTotal.Text = "累计采集：" + objItem["allTotal"].ToString() + "条";
                lb_AllTotal.Text = "今日采集：" + objItem["todayTotal"].ToString() + "条";
                lb_NextTime.Text = "最近采集时间：" + objItem["lastTime"].ToString();
            }
            catch (Exception ex)
            {
                //日志处理
                Loghelper.WriteErrorLog("刷新采集统计失败", ex);
                lr.AddLogInfo(ex.ToString(), "刷新采集统计失败", "刷新采集统计失败", "Error");
            }
        }
    }
}
