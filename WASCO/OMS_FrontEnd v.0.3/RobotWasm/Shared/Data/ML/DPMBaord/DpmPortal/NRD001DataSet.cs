namespace RobotWasm.Shared.Data.ML.DPMBaord.DpmPortal {
    public class NRD001DataSet {

        public class DocSet {
            public int status { get; set; }
            public object message { get; set; }
            public List<DataRow> rows { get; set; }
        }

        public class DataRow {
            public string TAMBON_ID { get; set; }
            public string TAMBON_NAME { get; set; }
            public string MOOBAN_ID { get; set; }
            public object MOOBAN_NAME { get; set; }
        }
    }
}
