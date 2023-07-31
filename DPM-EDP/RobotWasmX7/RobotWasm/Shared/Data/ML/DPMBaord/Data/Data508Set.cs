
using RobotWasm.Shared.Data.GaDB;

namespace RobotWasm.Shared.Data.ML.DPMBaord.Data
{
    public class Data508Set {
        //เสี่ยงอุทกภัย  / etl.dpm_flood_risk
        public class DocSet {
            public int status { get; set; }
            public object message { get; set; }
            public List<dpm_flood_risk> rows { get; set; }
        }

        public partial class dpm_flood_risk
        {
            public Guid? id { get; set; }
            public string risk_level { get; set; }
            public string province { get; set; }
            public string amphor { get; set; }
            public string tumbon { get; set; }
            public string type { get; set; }
            public decimal? moo_no { get; set; }
            public string moo_name { get; set; }
            public string moo_id { get; set; }
            public string village { get; set; }
            public decimal? risk_flood { get; set; }
            public decimal? risk_overflow { get; set; }
            public decimal? risk_flashflood { get; set; }
            public decimal? linklihood_1 { get; set; }
            public decimal? linklihood_2 { get; set; }
            public decimal? linklihood_3 { get; set; }
            public decimal? linklihood_4 { get; set; }
            public decimal? linklihood_10 { get; set; }
            public decimal? impact_hardness1 { get; set; }
            public decimal? impact_hardness2 { get; set; }
            public decimal? impact_hardness3 { get; set; }
            public string impactall_transport { get; set; }
            public string impactall_common { get; set; }
            public string impactall_agriculture { get; set; }
            public string impactall_fishing { get; set; }
            public string monthrisk_jan { get; set; }
            public string monthrisk_feb { get; set; }
            public string monthrisk_mar { get; set; }
            public string monthrisk_apr { get; set; }
            public string monthrisk_may { get; set; }
            public string monthrisk_jun { get; set; }
            public string monthrisk_jul { get; set; }
            public string monthrisk_aug { get; set; }
            public string monthrisk_sep { get; set; }
            public string monthrisk_oct { get; set; }
            public string monthrisk_nov { get; set; }
            public string monthrisk_dec { get; set; }
            public decimal? latitude { get; set; }
            public decimal? longitude { get; set; }
            public DateTime? data_update { get; set; }
        }

        public class Param {
            public string search { get; set; }
            public DateTime date_begin { get; set; }
            public DateTime date_end { get; set; }
        }

    } 
}
