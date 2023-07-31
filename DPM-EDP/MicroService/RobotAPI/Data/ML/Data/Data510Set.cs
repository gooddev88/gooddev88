using RobotAPI.Data.CimsDB.TT;

namespace RobotAPI.Data.ML.Data {
    public class Data510Set {
        //get pm2.5 ล่าสุด
        public class DocSet {
            public int status { get; set; }
            public object message { get; set; }
            public List<nda_aws_weather> rows { get; set; }
        } 

      

    } 
}
