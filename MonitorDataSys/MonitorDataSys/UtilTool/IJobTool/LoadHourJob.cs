using MonitorDataSys.Repository.local;
using Quartz;
using System;
using System.Threading.Tasks;

namespace MonitorDataSys.UtilTool.IJobTool
{
    public class LoadHourJob : IJob
    {
        private readonly LogRepository lr = new LogRepository();

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="context"></param>
        public void Execute(IJobExecutionContext context)
        {
            Task.Run(async () =>
            {
                try
                {
                    JobDataMap dataMap = context.JobDetail.JobDataMap;
                    string key = dataMap.GetString("key");//获取参数

                    //1.绑定需要执行的操作方法
                    //var act = new Action(FrDataCollect.frDataCollect.collectHourDataTool);
                    //act.BeginInvoke(ar => act.EndInvoke(ar), null);  //参数null可以作为回调函数的返回参数
                    await FrDataCollect.frDataCollect.collectHourDataTool();
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
