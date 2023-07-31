

using RobotWasm.Shared.Data.GaDB;

namespace RobotWasm.Shared.Data.ML.DPMBaord.Data
{
    public class Data503Set {
        //ลำน้ำสำคัญ  etl.dss_water
        public class DocSet {
            public int status { get; set; }
            public object message { get; set; }
            public List<dss_water> rows { get; set; }
        }

        public partial class dss_water
        {
            public Guid? id { get; set; }
            public string station_id { get; set; }
            public DateTime? h_date { get; set; }
            public decimal? waterline_lv { get; set; }
            public decimal? waterline_vol { get; set; }
            public DateTime? h_timestamp { get; set; }
        }

        public class Param {
            public string search { get; set; }
            public DateTime date_begin { get; set; }
            public DateTime date_end { get; set; }

        }
    }


}
