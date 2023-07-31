namespace RobotWasm.Shared.Data.ML.DPMBaord.DpmPortal {
    public class DPM005DataSet {

        public class DocSet {
            public int status { get; set; }
            public object message { get; set; }
            public List<DataRow> rows { get; set; }
        }

        public class DataRow {
            public string PROTECTEDZONEID { get; set; }
            public string PLACECODE { get; set; }
            public string PLACENAME { get; set; }
            public string SUBPLACECODE { get; set; }
            public string SUBPLACENAME { get; set; }
            public string NAME { get; set; }
            public string PROVINCECODE { get; set; }
            public string ADDRESS { get; set; }
            public string TAMBONCODE { get; set; }
            public string POSTALCODE { get; set; }
            public string LATITUDE { get; set; }
            public string LONGITUDE { get; set; }
            public string TEL { get; set; }
            public string FAX { get; set; }
        }

    }
}
