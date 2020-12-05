using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonitorDataSys.Models.NationPrediction
{
    /// <summary>
    /// 区域预报
    /// </summary>
    public class AreaPrediction
    {
        /// <summary>
        /// 预警信息
        /// </summary>
        public string WarningInfo { get; set; }

        /// <summary>
        /// 趋势图
        /// </summary>
        public string TrendImage { get; set; }

        /// <summary>
        /// 全景图
        /// </summary>
        public string SceneryImagesPath { get; set; }

        /// <summary>
        /// 接收时间
        /// </summary>
        public DateTime ReceiveTime { get; set; }

        /// <summary>
        /// 发布时间
        /// </summary>
        public DateTime PublishDate { get; set; }

        /// <summary>
        /// 其他描述
        /// </summary>
        public string OtherDescription { get; set; }

        /// <summary>
        /// ID
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 健康提示
        /// </summary>
        public string HealthTips { get; set; }

        /// <summary>
        /// 预报类型
        /// </summary>
        public string ForecastType { get; set; }

        /// <summary>
        /// 预报员
        /// </summary>
        public string ForecastPerson { get; set; }

        /// <summary>
        /// 预报描述
        /// </summary>
        public string ForecastDescription { get; set; }

        /// <summary>
        /// 详细全景图
        /// </summary>
        public string DeatailSceneryImagePath { get; set; }

        /// <summary>
        /// 审核时间
        /// </summary>
        public string AuditDate { get; set; }

        /// <summary>
        /// 区域编码
        /// </summary>
        public string AreaCode { get; set; }

        /// <summary>
        /// 是否允许发布
        /// </summary>
        public bool AllowPublish { get; set; }
    }
}
