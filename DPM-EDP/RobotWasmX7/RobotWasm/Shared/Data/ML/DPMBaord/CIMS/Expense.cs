namespace RobotWasm.Shared.Data.ML.DPMBaord.CIMS {
    public class Expense {

        public class DataRow {
            public string dataid { get; set; }
            public string originator_realname { get; set; }
            public string originator_user { get; set; }
            public string year_data { get; set; }
            public string month_data { get; set; }
            public string total_elec { get; set; }
            public string total_water { get; set; }
            public string total_tel { get; set; }
            public string total_net { get; set; }
            public string total_post { get; set; }
            public string entrydate { get; set; }
        }

    }
}
