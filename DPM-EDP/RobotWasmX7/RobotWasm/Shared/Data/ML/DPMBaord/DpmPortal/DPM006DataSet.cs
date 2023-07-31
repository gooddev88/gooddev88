namespace RobotWasm.Shared.Data.ML.DPMBaord.DpmPortal {
    public class DPM006DataSet {

        public class DocSet {
            public int status { get; set; }
            public object message { get; set; }
            public List<DataRow> rows { get; set; }
        }

        public class DataRow {
            public string ORGANIZATIONID { get; set; }
            public string ORGANIZATIONNAME { get; set; }
            public string PROVINCECODE { get; set; }
            public string LATITUDE { get; set; }
            public string LONGITUDE { get; set; }
            public string TEL { get; set; }
            public string FAX { get; set; }
            public object EMAIL { get; set; }
            public string RADIOWAVE { get; set; }
            public object EXPERTISETYPE { get; set; }
            public object EXPERTISTTYPENAME { get; set; }
            public object AMOUNT { get; set; }
        }

    }
}
