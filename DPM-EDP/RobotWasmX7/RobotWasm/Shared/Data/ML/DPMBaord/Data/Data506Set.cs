
using RobotWasm.Shared.Data.GaDB;

namespace RobotWasm.Shared.Data.ML.DPMBaord.Data
{
    public class Data506Set {
        //เสี่ยงแล้ง / etl.dpm_drought_risk
        public class DocSet {
            public int status { get; set; }
            public object message { get; set; }
            public List<dpm_drought_risk> rows { get; set; }
        }

        public partial class dpm_drought_risk
        {
            public Guid? id { get; set; }
            public string province { get; set; }
            public string amphor { get; set; }
            public string tumbon { get; set; }
            public decimal? moo_no { get; set; }
            public string village { get; set; }
            public string org { get; set; }
            public decimal? latitude { get; set; }
            public decimal? longitude { get; set; }
            public string risk_level { get; set; }
            public DateTime? data_update { get; set; }
        }

        public class Param {
            public string search { get; set; }
            public DateTime date_begin { get; set; }
            public DateTime date_end { get; set; }
        }

    } 
}
