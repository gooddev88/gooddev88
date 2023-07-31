using RobotAPI.Data.CimsDB.TT;

namespace RobotAPI.Data.ML.Data {
    public class Data505Set {
        //คุณภาพอากาศ / etl.dss_weather_quality
        public class DocSet {
            public int status { get; set; }
            public object message { get; set; }
            public List<dss_weather_quality> rows { get; set; }
        } 

        public class Param {
            public string search { get; set; }
            public DateTime date_begin { get; set; }
            public DateTime date_end { get; set; }
        }

    } 
}
