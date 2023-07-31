 
namespace RobotWasm.Client.Data.ML.HighChart {
    public partial class PlotLine {

  
        public string Color { get; set; } = "red";
 
        public double? Value { get; set; }
   
        public int? Width { get; set; } = 1;
 
        public int? ZIndex { get; set; } = 5;
 
        public Label Label { get; set; }
 
        public string Id { get; set; }


        public PlotLine() {

        }
    }
}
