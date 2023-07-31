namespace RobotAPI.Data.ML.Portal {
    public class EST006DataSet {
                     
        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);

        public class DocSet {
            public int status { get; set; }
            public object message { get; set; }
            public List<DataRow> rows { get; set; }
        }

        public class DataRow {
            public string DISASTER_ID { get; set; }
            public string AMPHUR_ID { get; set; }
            public string AMPHUR_NAME { get; set; }
            public string TAMBON_ID { get; set; }
            public string TAMBON_NAME { get; set; }
            public string MOOBAN_ID { get; set; }
            public string MOOBAN_NAME { get; set; }
        }
    }
}
