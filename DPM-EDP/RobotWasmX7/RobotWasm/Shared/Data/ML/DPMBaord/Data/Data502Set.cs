

using RobotWasm.Shared.Data.GaDB;

namespace RobotWasm.Shared.Data.ML.DPMBaord.Data
{
    public class Data502Set {
        //น้ำเขื่อน / etl.dss_rid_dam
        public class DocSet {
            public int status { get; set; }
            public object message { get; set; }
            public List<dss_rid_dam> rows { get; set; }
        }

        public partial class dss_rid_dam
        {
            public Guid? id { get; set; }
            public string station_id { get; set; }
            public DateTime? wd_date { get; set; }
            public string station_name { get; set; }
            public string dam_type { get; set; }
            public decimal? lat { get; set; }
            public decimal? lng { get; set; }
            public string project_name { get; set; }
            public string province { get; set; }
            public string region { get; set; }
            public decimal? cap_resv { get; set; }
            public decimal? low_qdisc { get; set; }
            public decimal? qdisc_prev { get; set; }
            public decimal? perc_resv_prev { get; set; }
            public decimal? qdisc_curr { get; set; }
            public decimal? perc_resv_curr { get; set; }
            public decimal? jan_info { get; set; }
            public decimal? q_info { get; set; }
            public decimal? q_outfo { get; set; }
            public decimal? water_workable { get; set; }
            public DateTime? wd_timestamp { get; set; }
        }

        public class Param {
            public string search { get; set; }
            public DateTime date_begin { get; set; }
            public DateTime date_end { get; set; }

        }
    }


}
