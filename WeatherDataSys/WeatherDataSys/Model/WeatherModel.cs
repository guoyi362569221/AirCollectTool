using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherDataSys.Model
{
    public class WeatherModel
    {
        public DateTime time { get; set; }
        public string stationcode { get; set; }
        public string tempDiff { get; set; }
        public double rain1h { get; set; }
        public double rain24h { get; set; }
        public double rain12h { get; set; }
        public double rain6h { get; set; }
        public double temperature { get; set; }
        public double humidity { get; set; }
        public double pressure { get; set; }
        public double windDirection { get; set; }
        public double windSpeed { get; set; }
    }
}
