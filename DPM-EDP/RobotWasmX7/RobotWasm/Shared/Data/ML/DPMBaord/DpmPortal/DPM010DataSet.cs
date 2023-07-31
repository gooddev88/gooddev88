namespace RobotWasm.Shared.Data.ML.DPMBaord.DpmPortal {
    public class DPM010DataSet {

        public class DocSet {
            public int status { get; set; }
            public object message { get; set; }
            public List<DataRow> rows { get; set; }
        }

        public class DataRow {
            public string ANNOUNCECODE { get; set; }
            public string STARTDATE { get; set; }
            public string PROVINCE_ID { get; set; }
            public string PROVINCE_NAME { get; set; }
            public string AMPHUR_ID { get; set; }
            public string AMPHUR_NAME { get; set; }
            public string TAMBOL_ID { get; set; }
            public string TAMBOL_NAME { get; set; }
            public string STATUS_DATA { get; set; }
            public string STATUS_NAME { get; set; }
            public object ANNOUNCEDATE { get; set; }
            public object HELPANNOUNCEDATE { get; set; }
            public object ENDDATE { get; set; }
            public string CHK_UNWIND { get; set; }
            public string VILLAGE_AMT { get; set; }
        }
    }
}
