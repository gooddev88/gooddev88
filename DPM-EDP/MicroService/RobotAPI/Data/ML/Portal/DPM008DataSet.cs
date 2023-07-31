namespace RobotAPI.Data.ML.Portal {

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class DPM008DataSet {

        public class DocSet {
            public int status { get; set; }
            public object message { get; set; }
            public List<DataRow> rows { get; set; }
        }

        public class DataRow {
            public string PROVINCEID { get; set; }
            public string PROVINCENAME { get; set; }
            public string NAME { get; set; }
            public string TEL { get; set; }
            public string LAT { get; set; }
            public string LNG { get; set; }
            public string ADDRESS { get; set; }
        }       
    }
}
