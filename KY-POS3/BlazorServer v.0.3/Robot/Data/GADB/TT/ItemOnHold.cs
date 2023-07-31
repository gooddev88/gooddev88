using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Robot.Data.GADB.TT {
    public   class ItemOnHold {
    
        public int ID { get; set; }
        public string ItemID { get; set; }
        public string RCompanyID { get; set; }
        public string CompanyID { get; set; }
        public string ItemName { get; set; }
        public bool IsHold { get; set; }
    }
}
