using DBF.Data.GADB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBF.Model {
    public class SODocSet {
        public vw_OSOHead Head { get; set; }
        public List<vw_OSOLine> Line { get; set; }
        
    }
}
