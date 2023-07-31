using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Robot.Data.GADB.TT {
    public class POS_SaleRefresh {
        public int ID { get; set; }
        public string RComID { get; set; }
        public string ComID { get; set; }
        public string BillID { get; set; }
        public string MacID { get; set; }
        public DateTime LatestModifiedDate { get; set; }
    }

}
