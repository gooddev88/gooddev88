using RobotAPI.Data.CimsDB.TT;

namespace RobotAPI.Data.ML.Data {
    public class Data502Set {
        //น้ำเขื่อน / etl.dss_rid_dam
        public class DocSet {
            public int status { get; set; }
            public object message { get; set; }
            public List<dss_rid_dam> rows { get; set; }
        }


        public class Param {
            public string search { get; set; }
            public DateTime date_begin { get; set; }
            public DateTime date_end { get; set; }

        }
    }


}
