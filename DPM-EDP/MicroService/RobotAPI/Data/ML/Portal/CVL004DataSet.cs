namespace RobotAPI.Data.ML.Portal {
    public class CVL004DataSet {
                     
        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);

        public class DocSet {
            public int status { get; set; }
            public object message { get; set; }
            public List<DataRow> rows { get; set; }
        }

        public class DataRow {
            public string CENTER_ID { get; set; }
            public string CENTER_NAME { get; set; }
            public string CENTER_SHORT_NAME { get; set; }
            public string SUM_MEMBER { get; set; }
        }
    }
}
