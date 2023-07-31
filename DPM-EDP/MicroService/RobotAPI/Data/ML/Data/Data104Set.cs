namespace RobotAPI.Data.ML.Data {
    public class Data104Set {

         
        public class DocSet {
            public int status { get; set; }
            public object message { get; set; }
            public List<DataRow> rows { get; set; }
        }
        public class DataRow {
            public DateTime start_date { get; set; }
            public string count_use { get; set; }
          

        }
      

    }
}
