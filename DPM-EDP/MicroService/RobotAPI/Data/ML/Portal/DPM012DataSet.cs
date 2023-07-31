namespace RobotAPI.Data.ML.Portal {

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class DPM012DataSet {

        public class DocSet {
            public int status { get; set; }
            public object message { get; set; }
            public List<DataRow> rows { get; set; }
        }

        public class DataRow {
            public string DISASTER_TYPE { get; set; }
            public string DISASTER_NAME { get; set; }
            public string FROM_DATE { get; set; }
            public string TO_DATE { get; set; }
            public string PROVINCE_CODE { get; set; }
            public string STATUS_ALERT { get; set; }
            public string STATUS_NAME { get; set; }
        }       
    }
}
