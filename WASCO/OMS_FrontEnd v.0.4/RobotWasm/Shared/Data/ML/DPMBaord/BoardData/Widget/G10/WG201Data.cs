namespace RobotWasm.Shared.Data.ML.DPMBaord.BoardData.Widget.G10 {
    public class WG201Data {
        //จำนวนสิ่งของสำรองจ่าย แบ่งตามจังหวัด  EST008DataSet
        public class DocSet {
            public int status { get; set; }
            public object message { get; set; }
            public List<DataRow> rows { get; set; }
        }

        public class DataRow {
            public string PROVINCE_ID { get; set; }
            public string LOCATE_NAME { get; set; }
            public string STUFF_ID { get; set; }
            public string STUFF_NAME { get; set; }
            public string INOUT_REMAIN { get; set; }
        }


    }

}
