namespace RobotWasm.Shared.Data.ML.DPMBaord.DpmPortal {
    public class CVL004DataSet {
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
