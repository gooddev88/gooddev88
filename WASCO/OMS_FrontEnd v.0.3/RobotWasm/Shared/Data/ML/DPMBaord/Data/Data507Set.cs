namespace RobotWasm.Shared.Data.ML.DPMBaord.Data {
    public class Data507Set {



        public class DocSet {
            public int status { get; set; }
            public object message { get; set; }
            public List<DataRow> rows { get; set; }
        }

        public class DataRow {
            public string id { get; set; }
            public string dpm_id { get; set; }
            public string province { get; set; }
            public string disaster_type { get; set; }
            public DateTime? start_date { get; set; }
            public DateTime? announcedate { get; set; }
            public DateTime? helpannouncedate { get; set; }
            public DateTime? end_date { get; set; }
            public string amphur { get; set; }
            public string tambon_code { get; set; }
            public string tambon { get; set; }
            public int muban_count { get; set; }
            public DateTime? last_upd_date { get; set; }
            public DateTime? dpm_timestamp { get; set; }
            public string PID { get; set; }
            public string AID { get; set; }
            public string org { get; set; }
            public int community_count { get; set; }
            public string status_office { get; set; }
            public string flood_type { get; set; }
            public string TID { get; set; }
            public string org_id { get; set; }
            public string key_check { get; set; }

        }
        public class Param {
            public string search { get; set; }
            public string province { get; set; }
            public string distype { get; set; }

            public DateTime begin { get; set; }
            public DateTime end { get; set; }



        }

    }


}
