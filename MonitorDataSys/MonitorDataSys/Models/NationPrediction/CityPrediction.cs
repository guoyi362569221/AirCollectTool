using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonitorDataSys.Models.NationPredict
{
    /// <summary>
    /// 城市预报
    /// </summary>
    public class CityPrediction
    {
        //public string CityCode { get; set; }
        //public string Name { get; set; }
        //public DateTime PredictionTime { get; set; }
        //public DateTime DataDate { get; set; }
        //public int PredictionInterval { get; set; }

        /// <summary>
        /// 城市编码
        /// </summary>
        public string CityCode { get; set; }

        /// <summary>
        /// 城市名称
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// 24小时预报 起始值
        /// </summary>
        public string AirIndex_From { get; set; }

        /// <summary>
        /// 24小时预报 终止值
        /// </summary>
        public string AirIndex_To { get; set; }

        /// <summary>
        /// 十天潜伏分析
        /// </summary>
        public string DetailInfo { get; set; }

        /// <summary>
        /// 城市经度
        /// </summary>
        public string Longitude { get; set; }
        
        /// <summary>
        /// 城市纬度
        /// </summary>
        public string Latitude { get; set; }

        /// <summary>
        /// 24小时首要污染物
        /// </summary>
        public string PrimaryPollutant { get; set; }

        /// <summary>
        /// 48小时预报 起始值
        /// </summary>
        public string Air48Index_From { get; set; }

        /// <summary>
        /// 48小时预报 终止值
        /// </summary>
        public string Air48Index_To { get; set; }

        /// <summary>
        /// 48小时首要污染物
        /// </summary>
        public string Primary48Pollutant { get; set; }

        /// <summary>
        /// 72小时预报 起始值
        /// </summary>
        public string Air72Index_From { get; set; }

        /// <summary>
        /// 72小时预报 终止值
        /// </summary>
        public string Air72Index_To { get; set; }

        /// <summary>
        /// 72小时首要污染物
        /// </summary>
        public string Primary72Pollutant { get; set; }

        /// <summary>
        /// 96小时预报 起始值
        /// </summary>
        public string Air96Index_From { get; set; }

        /// <summary>
        /// 96小时预报 终止值
        /// </summary>
        public string Air96Index_To { get; set; }

        /// <summary>
        /// 96小时首要污染物
        /// </summary>
        public string Primary96Pollutant { get; set; }

        /// <summary>
        /// 120小时预报 起始值
        /// </summary>
        public string Air120Index_From { get; set; }

        /// <summary>
        /// 120小时预报 终止值
        /// </summary>
        public string Air120Index_To { get; set; }

        /// <summary>
        /// 120小时首要污染物
        /// </summary>
        public string Primary120Pollutant { get; set; }

        /// <summary>
        /// 是否发布未来72小时
        /// </summary>
        public bool IsPublish_72Hour { get; set; }

    }
}
