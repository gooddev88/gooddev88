using RobotAPI.Data.CimsDB.TT;

namespace RobotAPI.Data.ML.Data {
    public class Data501Set {

        //จุดแผ่นดินไหว / etl.dss_earthquake   

        public class DocSet {
            public int status { get; set; }
            public object message { get; set; }
            public List<dss_earthquake> rows { get; set; }
        }


        public class Param {
            public string search { get; set; }
            public DateTime date_begin { get; set; }
            public DateTime date_end { get; set; }

        }
    }


}
