namespace RobotWasm.Shared.Data.ML.DPMBaord.BoardData.Widget.G10 {
    
    public class WG202Data {
        //จำนวนสิ่งของสำรองจ่าย แบ่งตามศูนย์
        public class DocSet {
            public int status { get; set; }
            public object message { get; set; }
            public List<DataRow> rows { get; set; }
            public List<Branch> branchs { get; set; }
        }


        public class DataRow {
            public string unit { get; set; }
            public string unit_code { get; set; }
            public string stuff_name { get; set; }
            public string remain { get; set; }
        }
        public class Branch {
            public string CENTER_ID { get; set; }
            public string LOCATE_NAME { get; set; }
        }

    }

}
