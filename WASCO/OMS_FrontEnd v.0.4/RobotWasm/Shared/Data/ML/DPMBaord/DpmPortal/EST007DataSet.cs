namespace RobotWasm.Shared.Data.ML.DPMBaord.DpmPortal {
    public class EST007DataSet {

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
