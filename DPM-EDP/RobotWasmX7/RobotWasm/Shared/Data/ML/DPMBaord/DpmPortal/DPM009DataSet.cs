namespace RobotWasm.Shared.Data.ML.DPMBaord.DpmPortal {
    public class DPM009DataSet {

        public class DocSet {
            public int status { get; set; }
            public object message { get; set; }
            public List<DataRow> rows { get; set; }
        }

        public class DataRow {
            public string DISASTER_CODE { get; set; }
            public string CYEAR { get; set; }
            public string PROVINCE { get; set; }
            public string ISSUES { get; set; }
            public string STARTDATE { get; set; }
            public string TYPE_DISASTER { get; set; }
            public string AMPHUR_AMOUNT { get; set; }
            public string TAMBON_AMOUNT { get; set; }
            public string ORG_AMOUNT { get; set; }
            public string VILLAGE_AMOUNT { get; set; }
            public string COMMUNITY_AMOUNT { get; set; }
            public string STATUS { get; set; }
        }
    }
}
