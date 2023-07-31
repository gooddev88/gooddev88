
using RobotWasm.Shared.Data.GaDB;

namespace RobotWasm.Shared.Data.ML.DPMBaord.Data
{
    public class Data505Set {
        //คุณภาพอากาศ / etl.dss_weather_quality
        public class DocSet {
            public int status { get; set; }
            public object message { get; set; }
            public List<dss_weather_quality> rows { get; set; }
        }

        public partial class dss_weather_quality
        {
            public Guid? id { get; set; }
            public string station_id { get; set; }
            public DateTime? wq_date { get; set; }
            public string station_name_th { get; set; }
            public string station_name_en { get; set; }
            public string province_th { get; set; }
            public string province_en { get; set; }
            public string area_th { get; set; }
            public string area_en { get; set; }
            public string station_type { get; set; }
            public decimal? station_lat { get; set; }
            public decimal? station_long { get; set; }
            public decimal? pm25 { get; set; }
            public decimal? pm10 { get; set; }
            public decimal? o3 { get; set; }
            public decimal? co { get; set; }
            public decimal? no2 { get; set; }
            public decimal? so2 { get; set; }
            public decimal? aqi { get; set; }
            public DateTime? wq_timestamp { get; set; }
            public string source { get; set; }
        }

        public class Param {
            public string search { get; set; }
            public DateTime date_begin { get; set; }
            public DateTime date_end { get; set; }
        }

    } 
}
