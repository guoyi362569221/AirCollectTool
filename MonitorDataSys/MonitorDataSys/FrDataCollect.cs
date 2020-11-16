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
using System.Threading;

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
        private CityMonitorHourRepository cmhr = null;
        private StationMonitorHourRepository smhr = null;
        private CityMonitorDayRepository cmdr = null;
        private StationMonitorDayRepository smdr = null;

        private readonly string hourCity = ConfigurationManager.AppSettings["hourCity"];
        private readonly string hourStation = ConfigurationManager.AppSettings["hourStation"];
        private readonly string dayCity = ConfigurationManager.AppSettings["dayCity"];
        private readonly string dayStation = ConfigurationManager.AppSettings["dayStation"];

        private readonly string noDataValue = ConfigurationManager.AppSettings["noDataValue"];

        private IDictionary hourCityTableFiledDic = ConfigurationManager.GetSection("hourCitySettings") as IDictionary;
        private IDictionary hourStationTableFiledDic = ConfigurationManager.GetSection("hourStationSettings") as IDictionary;
        private IDictionary dayCityTableFiledDic = ConfigurationManager.GetSection("dayCitySettings") as IDictionary;
        private IDictionary dayStationTableFiledDic = ConfigurationManager.GetSection("dayStationSettings") as IDictionary;


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
                setControlStatus(false);
                this.rtb_Log.Clear();
                writeLog("开始采集数据", ColorEnum.Blue);

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

        private void btn_Stop_Click(object sender, EventArgs e)
        {
            try
            {
                setControlStatus(true);
                this.writeLog("停止采集...", ColorEnum.Blue);
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
        /// 
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
                }
            }
            catch (Exception e) 
            {
                //日志处理
                Loghelper.WriteErrorLog("捕获异常信息", e);
                lr.AddLogInfo(e.ToString(), "捕获异常信息", "捕获异常信息", "Error");
            }
            
        }

        public void writeLog(string msg, ColorEnum color = ColorEnum.Green)
        {
            try
            {
                this.rtb_Log.Focus();
                this.rtb_Log.Select(this.rtb_Log.Text.Length, 0);
                switch (color)
                {
                    case ColorEnum.Blue:
                        this.rtb_Log.SelectionColor = Color.Blue;
                        break;
                    case ColorEnum.Red:
                        this.rtb_Log.SelectionColor = Color.Red;
                        break;
                    case ColorEnum.Green:
                        this.rtb_Log.SelectionColor = Color.Green;
                        break;
                    case ColorEnum.Black:
                        this.rtb_Log.SelectionColor = Color.Black;
                        break;
                    case ColorEnum.Orange:
                        this.rtb_Log.SelectionColor = Color.Orange;
                        break;
                }
                string time = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                this.rtb_Log.AppendText(time + "->" + msg);
                this.rtb_Log.AppendText("\r\n");
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

                this.rtb_Log.Clear();
                writeLog("开始采集数据", ColorEnum.Blue);

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
                        if (!String.IsNullOrEmpty(cityCode))
                        {
                            try
                            {
                                int wt = 0;
                                int ct = 0;
                                ThreadPool.GetAvailableThreads(out wt, out ct);
                                Loghelper.WriteLog("当前线程情况wt=" + wt + ",ct=" + ct);
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
                                    }
                                }
                            }
                            catch (Exception ex2)
                            {
                                //日志处理
                                Loghelper.WriteErrorLog(cityName + "小时数据采集异常", ex2);
                                lr.AddLogInfo(cityName + "小时数据采集异常", "", hourCity, "Error");
                            }
                        }
                    }
                    if (listCityHour.Count > 0)
                    {
                        bool cityHourInsertResult = cmhr.AddDataInfo(hourCity, listCityHour);
                        if (cityHourInsertResult)
                        {
                            collectTotal += listCityHour.Count;
                            //在SQLite表中录入当前采集条数
                            writeLog("<" + cityTable.Rows.Count + "个城市>小时数据采集成功，本次采集" + listCityHour.Count + "条数据", ColorEnum.Green);
                            lr.AddLogInfo(cityTable.Rows.Count + "个城市，小时数据采集成功，本次采集" + listCityHour.Count + "条数据", "", hourCity, "Info");
                        }
                        else
                        {
                            //在SQLite表中录入当前采集条数
                            writeLog("<" + cityTable.Rows.Count + "个城市>小时数据采集失败，应该采集" + listCityHour.Count + "条数据", ColorEnum.Red);
                            lr.AddLogInfo(cityTable.Rows.Count + "个城市，小时数据采集失败，应该采集" + listCityHour.Count + "条数据", "", hourCity, "Error");
                        }
                    }
                    else
                    {
                        //在SQLite表中录入当前采集条数
                        writeLog("<" + cityTable.Rows.Count + "个城市>暂无要采集小时数据", ColorEnum.Orange);
                    }
                }
                else
                {
                    //请先进性站点同步
                    writeLog("<请先进性站点同步,然后进行城市小时数据采集", ColorEnum.Orange);
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
                                int wt = 0;
                                int ct = 0;
                                ThreadPool.GetAvailableThreads(out wt, out ct);
                                Loghelper.WriteLog("当前线程情况wt=" + wt + ",ct=" + ct);
                                AQIDataPublishLive[] stationAQILiveData = await publishCtx.GetAreaAQIPublishLive(cityName).ResultAsync<AQIDataPublishLive[]>();
                                if (stationAQILiveData != null)
                                {
                                    IAQIDataPublishLive[] stationIAQILiveData = await publishCtx.GetAreaIaqiPublishLive(cityName).ResultAsync<IAQIDataPublishLive[]>();
                                    if (stationAQILiveData != null && stationAQILiveData.Length > 0)
                                    {
                                        for (int j = 0; j < stationAQILiveData.Count(); j++)
                                        {
                                            IAQIDataPublishLive tmpIAQIDate = null;
                                            string stationCode = stationAQILiveData[j].StationCode;
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
                                                bool isCompeletCollect = smhr.IsCompeletCollect(hourStation, stationCode, stationAQILiveData[j].TimePoint);
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
                        if (stationHourInsertResult)
                        {
                            collectTotal += listStationHour.Count;
                            //在SQLite表中录入当前采集条数
                            writeLog("<" + stationTable.Rows.Count + "个站点>小时数据采集成功，本次采集" + listStationHour.Count + "条数据", ColorEnum.Green);
                            lr.AddLogInfo(stationTable.Rows.Count + "个站点，小时数据采集成功，本次采集" + listStationHour.Count + "条数据", "", hourStation, "Info");
                        }
                        else
                        {
                            //在SQLite表中录入当前采集条数
                            writeLog("<" + cityTable.Rows.Count + "个站点>小时数据采集失败，应该采集" + listStationHour.Count + "条数据", ColorEnum.Red);
                            lr.AddLogInfo(cityTable.Rows.Count + "个站点，小时数据采集失败，应该采集" + listStationHour.Count + "条数据", "", hourCity, "Error");
                        }
                    }
                    else
                    {
                        //在SQLite表中录入当前采集条数
                        writeLog("<" + stationTable.Rows.Count + "个站点>暂无要采集小时数据", ColorEnum.Orange);
                    }
                }
                else
                {
                    //请先进性站点同步
                    writeLog("<请先进性站点同步,然后进行站点小时数据采集", ColorEnum.Orange);
                }
                #endregion
                //保存本次采集信息
                cst.AddStaticInfo(collectTotal, collectTime);
                writeLog("本次小时定时采集完成", ColorEnum.Blue);
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

                this.rtb_Log.Clear();
                writeLog("开始采集数据", ColorEnum.Blue);

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
                                int wt = 0;
                                int ct = 0;
                                ThreadPool.GetAvailableThreads(out wt, out ct);
                                Loghelper.WriteLog("当前线程情况wt=" + wt + ",ct=" + ct);
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
                            writeLog("<" + cityTable.Rows.Count + "个城市>日均数据采集成功，本次采集" + listCityDay.Count + "条数据", ColorEnum.Green);
                            lr.AddLogInfo(cityTable.Rows.Count + "个城市，日均数据采集成功，本次采集" + listCityDay.Count + "条数据", "", dayCity, "Info");
                        }
                        else
                        {
                            //在SQLite表中录入当前采集条数
                            writeLog("<" + cityTable.Rows.Count + "个城市>日均数据采集失败，应该采集" + listCityDay.Count + "条数据", ColorEnum.Red);
                            lr.AddLogInfo(cityTable.Rows.Count + "个城市，日均数据采集失败，应该采集" + listCityDay.Count + "条数据", "", hourCity, "Error");
                        }
                    }
                    else
                    {
                        //在SQLite表中录入当前采集条数
                        writeLog("<" + cityTable.Rows.Count + "个城市>暂无要采集日均数据", ColorEnum.Orange);
                    }
                }
                else
                {
                    //请先进性站点同步
                    writeLog("请先进性站点同步,然后进行城市日均数据采集", ColorEnum.Orange);
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
                                int wt = 0;
                                int ct = 0;
                                ThreadPool.GetAvailableThreads(out wt, out ct);
                                Loghelper.WriteLog("当前线程情况wt=" + wt + ",ct=" + ct);

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
                            writeLog("<" + stationTable.Rows.Count + "个站点>日均数据采集成功，本次采集" + listStationDay.Count + "条数据", ColorEnum.Green);
                            lr.AddLogInfo(stationTable.Rows.Count + "个站点，日均数据采集成功，本次采集" + listStationDay.Count + "条数据", "", dayStation, "Info");
                        }
                        else
                        {
                            //在SQLite表中录入当前采集条数
                            writeLog("<" + stationTable.Rows.Count + "个站点>日均数据采集失败，应该采集" + listStationDay.Count + "条数据", ColorEnum.Red);
                            lr.AddLogInfo(stationTable.Rows.Count + "个站点，日均数据采集失败，应该采集" + listStationDay.Count + "条数据", "", hourCity, "Error");
                        }
                    }
                    else
                    {
                        //在SQLite表中录入当前采集条数
                        writeLog("<" + stationTable.Rows.Count + "个站点>暂无要采集日均数据", ColorEnum.Orange);
                    }
                }
                else
                {
                    //请先进性站点同步
                    writeLog("请先进性站点同步,然后进行站点日均数据采集", ColorEnum.Orange);
                }
                #endregion
                //保存本次采集信息
                cst.AddStaticInfo(collectTotal, collectTime);
                writeLog("本次日均定时采集完成", ColorEnum.Blue);
                refreshStaticInfo();
            }
            catch (Exception ex)
            {
                //日志处理
                Loghelper.WriteErrorLog("数据采集异常", ex);
                lr.AddLogInfo(ex.ToString(), "", "", "Error");
            }
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
