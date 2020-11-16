using Quartz;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static WeatherDataSys.From_Main;

namespace WeatherDataSys.UtilTool.IJobTool
{
    public class LoadJob : IJob
    {
        /// <summary>
        ///  执行
        /// </summary>
        /// <param name="context"></param>
        Task IJob.Execute(IJobExecutionContext context)
        {
            LoadDataTool lt = new LoadDataTool();
            return Task.Run(() =>
            {
                JobDataMap dataMap = context.JobDetail.JobDataMap;
                string key = dataMap.GetString("key");//获取参数
                
                //1.绑定需要执行的操作方法
                var act = new Action(lt.OnLineDataLoad);
                act.BeginInvoke(ar => act.EndInvoke(ar), null);  //参数null可以作为回调函数的返回参数
            });
        }

       
    }
}
