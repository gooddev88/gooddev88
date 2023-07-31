 

namespace RobotWasm.Client.Data.ML.HighChart {
    public partial class Accessibility {
 
        public string RangeDescription { get; set; }
   
        public string Description { get; set; }

        public Accessibility() {

        }

        // I added another param here for area charts to keep one accessibility class
        // TODO find another solution here to simplify ctor.
        public Accessibility(string rangeDescription = null, string description = null) {
            RangeDescription = rangeDescription;
            Description = description;
        }
    }
}
