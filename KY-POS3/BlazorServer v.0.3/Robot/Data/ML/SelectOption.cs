using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Robot.Data.ML {
    public class SelectOption {
        public bool IsSelect { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
        public bool IsVisible { get; set; }
        public int Sort { get; set; }
    }
}
