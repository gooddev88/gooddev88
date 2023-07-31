namespace RobotAPI.Data.ML.Portal {
    public class EST007DataSet {
                     
        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);

        public class DocSet {
            public int status { get; set; }
            public object message { get; set; }
            public List<DataRow> rows { get; set; }
        }

        public class DataRow {
            public string CENTER_ID { get; set; }
            public string LOCATE_NAME { get; set; }
            public string STUFF_ID { get; set; }
            public string STUFF_NAME { get; set; }
            public string INOUT_REMAIN { get; set; }
        }
    }
}
