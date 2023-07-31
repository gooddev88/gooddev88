namespace RobotWasm.Shared.Data.ML.DPMBaord.DpmPortal {
    public class DPM015DataSet {

        public class DocSet {
            public int status { get; set; }
            public object message { get; set; }
            public List<DataRow> rows { get; set; }
        }

        public class DataRow {
            public string ANNOUNCECODE { get; set; }
            public string TYPE_DISASTER { get; set; }
            public string TYPE_DISASTER_NAME { get; set; }
            public string STARTDATE { get; set; }
            public string PROVINCE { get; set; }
            public string PROVINCE_NAME { get; set; }
            public string AUMPER { get; set; }
            public string AMPHUR_NAME { get; set; }
            public string ORG_ID { get; set; }
            public string ORG_CODE { get; set; }
            public string ORG_TYPE { get; set; }
            public string ORG_FNAME { get; set; }
            public string STATUS_DATA { get; set; }
            public string STATUS_NAME { get; set; }
            public string ANNOUNCEDATE { get; set; }
            public string HELPANNOUNCEDATE { get; set; }
            public string ENDDATE { get; set; }
            public string CHK_UNWIND { get; set; }
            public string COMMUNITY_ID { get; set; }
            public string COMMUNITY_NAME { get; set; }
        }
    }
}
