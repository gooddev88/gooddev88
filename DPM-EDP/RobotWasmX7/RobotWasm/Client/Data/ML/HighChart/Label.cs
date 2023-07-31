 
namespace RobotWasm.Client.Data.ML.HighChart {
    public partial class Label {
         
        public bool? ConnectorAllowed { get; set; }
    
        public bool? Enabled { get; set; }
  
        public string Align { get; set; }
      
        public string TextAlign { get; set; }
       
        public string VerticalAlign { get; set; }
     
        public double? Y { get; set; }
       
        public double? X { get; set; }
 
        public string Text { get; set; }
       
        public Style Style { get; set; }

        public Label() {

        }

        public Label(bool? connectorAllowed = null) {
            ConnectorAllowed = connectorAllowed;
        }
    }
}
