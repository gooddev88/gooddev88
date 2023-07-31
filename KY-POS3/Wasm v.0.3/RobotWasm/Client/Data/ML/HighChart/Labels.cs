 

namespace RobotWasm.Client.Data.ML.HighChart {
    public partial class Labels {
 
        public string Formatter { get; set; }
    
        public string Format { get; set; }
      
        public bool? Enabled { get; set; }
  
        public Style Style { get; set; }
 
        public int? Step { get; set; }
      
        public string Overflow { get; set; }

        public Labels() {

        }
        public Labels(bool? enabled = null) {
            Enabled = enabled;
        }

        public Labels(string formatter = null) {
            Formatter = formatter;
            // So the temporary workaround to pass actual javascript functions
            // is to write them as function() {}
            // the reviver on javascript side will look at this style and then convert
            // it into a new Function(replaceTextFunction);
            // Don't love this workaround, will investigate for something better.
            // The reasoning behind this is dotnet is already reviving any object 
            // passed as an argument to blazor as JSON string and then casting to a object again.
        }
    }
}
