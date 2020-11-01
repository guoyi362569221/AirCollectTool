using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonitorDataSys.Models
{
    public class AirStation
    {
        public string UniqueCode { get; set; }
        public string Area { get; set; }
        public string CityCode { get; set; }
        public string StationCode { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string PositionName { get; set; }
    }
}
