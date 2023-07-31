

using RobotWasm.Shared.Data.GaDB;

namespace RobotWasm.Shared.Data.ML.DPMBaord.Data
{
    public class Data504Set {
        //ปริมาณน้ำฝน / etl.dss_rain
        public class DocSet {
            public int status { get; set; }
            public object message { get; set; }
            public List<dss_rain> rows { get; set; }
        }

        public partial class dss_rain
        {
            public Guid? id { get; set; }
            public string station_id { get; set; }
            public string station_name { get; set; }
            public DateTime? data_date { get; set; }
            public decimal? rn_24h { get; set; }
            public DateTime? rn_timestamp { get; set; }
        }

        public class Param {
            public string search { get; set; }
            public DateTime date_begin { get; set; }
            public DateTime date_end { get; set; }
        }

    } 
}
