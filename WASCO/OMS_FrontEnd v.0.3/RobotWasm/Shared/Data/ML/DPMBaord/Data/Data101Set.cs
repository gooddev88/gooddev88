namespace RobotWasm.Shared.Data.ML.DPMBaord.Data {
    public class Data101Set {

         
        public class DocSet {
            public int status { get; set; }
            public object message { get; set; }
            public List<DataRow> rows { get; set; }
        }
        public class DataRow {
            public DateTime meeting_date { get; set; }
            public double use_time { get; set; }
        }
      

    }
}
