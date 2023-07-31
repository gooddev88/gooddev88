using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Robot.Data.GADB.TT {
    public partial class TempFilePrinter {
        public int ID { get; set; }
        public string FileUrl { get; set; }
  public string AppPrinterUrl { get; set; }
        public System.DateTime CreatedDate { get; set; }
    }
}
