﻿using MonitorDataSys.Repository.local;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonitorDataSys.UtilTool.IJobTool
{
    
    public class LoadAreaPredictionJob : IJob
    {
        private readonly LogRepository lr = new LogRepository();

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="context"></param>
        public void Execute(IJobExecutionContext context)
        {
            _ = Task.Run(async () =>
              {
                  try
                  {
                      JobDataMap dataMap = context.JobDetail.JobDataMap;
                      string key = dataMap.GetString("key");//获取参数
                }
                  catch (Exception e)
                  {
                      Loghelper.WriteErrorLog("定时任务调用窗体函数采集数据失败", e);
                      lr.AddLogInfo(e.ToString(), "定时任务调用窗体函数采集数据失败", "定时任务调用窗体函数采集数据失败", "Error");
                    //throw e;
                }
              });
        }
    }
}