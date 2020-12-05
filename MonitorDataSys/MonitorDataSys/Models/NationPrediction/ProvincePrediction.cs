using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonitorDataSys.Models.NationPrediction
{
    /// <summary>
    /// 省域预报
    /// </summary>
    public class ProvincePrediction
    {
        /// <summary>
        /// 省份编码
        /// </summary>
        public string ProvinceCode { get; set; }

        /// <summary>
        /// 省份名称
        /// </summary>
        public string ProvinceName { get; set; }

        /// <summary>
        /// 发布时间
        /// </summary>
        public DateTime PublishDate { get; set; }

        /// <summary>
        /// 预警信息
        /// </summary>
        public string WarningInfo { get; set; }

        /// <summary>
        /// 其他描述
        /// </summary>
        public string OtherDescription { get; set; }

        /// <summary>
        /// 趋势图
        /// </summary>
        public string TrendImage { get; set; }

        /// <summary>
        ///预警信息描述 
        /// </summary>
        public string ForecastDescription { get; set; }

        /// <summary>
        /// 健康提示
        /// </summary>
        public string HealthTips { get; set; }

        /// <summary>
        /// 全景图地址
        /// </summary>
        public string SceneryImagesPath { get; set; }
    }
}
