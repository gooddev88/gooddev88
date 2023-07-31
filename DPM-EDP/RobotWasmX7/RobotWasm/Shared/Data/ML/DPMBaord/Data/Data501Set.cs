
using RobotWasm.Shared.Data.GaDB;

namespace RobotWasm.Shared.Data.ML.DPMBaord.Data
{
    public class Data501Set {

        //จุดแผ่นดินไหว / etl.dss_earthquake   

        public class DocSet {
            public int status { get; set; }
            public object message { get; set; }
            public List<dss_earthquake> rows { get; set; }
        }

        public partial class dss_earthquake
        {
            public string earthquake_key { get; set; }
            public DateTime? earthquake_date { get; set; }
            public decimal? earthquake_mag { get; set; }
            public decimal? earthquake_lat { get; set; }
            public decimal? earthquake_long { get; set; }
            public decimal? earthquake_depth { get; set; }
            public string earthquake_region { get; set; }
            public DateTime? earthquake_timestamp { get; set; }
            public string earthquake_source { get; set; }
            public string status_land { get; set; }
            public decimal? distance_th { get; set; }
            public string status_inth { get; set; }
        }

        public class Param {
            public string search { get; set; }
            public DateTime date_begin { get; set; }
            public DateTime date_end { get; set; }

        }
    }


}
