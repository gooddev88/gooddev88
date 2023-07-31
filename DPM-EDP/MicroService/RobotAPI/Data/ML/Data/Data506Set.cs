using RobotAPI.Data.CimsDB.TT;

namespace RobotAPI.Data.ML.Data {
    public class Data506Set {
        //เสี่ยงแล้ง / etl.dpm_drought_risk
        public class DocSet {
            public int status { get; set; }
            public object message { get; set; }
            public List<dpm_drought_risk> rows { get; set; }
        } 

        public class Param {
            public string search { get; set; }
            public DateTime date_begin { get; set; }
            public DateTime date_end { get; set; }
        }

    } 
}
