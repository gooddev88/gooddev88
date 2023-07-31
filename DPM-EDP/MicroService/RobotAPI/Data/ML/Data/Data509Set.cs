using RobotAPI.Data.CimsDB.TT;

namespace RobotAPI.Data.ML.Data {
    public class Data509Set {
        //อุบัติเหตุ  / acd_accident_person
        public class DocSet {
            public int status { get; set; }
            public object message { get; set; }
            public List<acd_accident_person> rows { get; set; }
        } 

      

    } 
}
