namespace RobotAPI.Data.ML.Data {
    public class Data102Set {

         
        public class DocSet {
            public int status { get; set; }
            public object message { get; set; }
            public List<DataRow> rows { get; set; }
        }
        public class DataRow {
            public string room_id { get; set; }
            public string room_name { get; set; }
            public string count_use { get; set; }
          

        }
      

    }
}
