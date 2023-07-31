 

namespace RobotWasm.Client.Data.ML.HighChart {
    public class ChartData {


        public Title title { get; set; }
        
        public XAxis xaxis { get; set; }
        public List<SeriesElement> series { get; set; }
        public List<Object> datas { get; set; }
    }

    public class Title {
        public string text { get; set; }
    }
}
