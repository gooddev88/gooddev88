namespace RobotWasm.Shared.Data.ML.DPMBaord.DpmPortal {
    public class DPM003DataSet {

        public class DocSet {
            public int status { get; set; }
            public object message { get; set; }
            public List<DataRow> rows { get; set; }
        }

        public class DataRow {
            public string DPM_CENTER_ID { get; set; }
            public string CODE { get; set; }
            public string NAME { get; set; }
            public string TYPE { get; set; }
            public string ADDRESS { get; set; }
            public string PROVINCE_ID { get; set; }
            public string PROVINCE_NAME { get; set; }
            public string AMPHUR_ID { get; set; }
            public string AMPHUR_NAME { get; set; }
            public string DISTRICT_ID { get; set; }
            public string DISTRICT_NAME { get; set; }
            public string TEL { get; set; }
            public string FAX { get; set; }
            public string EMAIL { get; set; }
            public string WEBSITE { get; set; }
            public string LATITUDE { get; set; }
            public string LONGITUDE { get; set; }
            public string VOLUNTEER_AMT { get; set; }
        }

    }
}
