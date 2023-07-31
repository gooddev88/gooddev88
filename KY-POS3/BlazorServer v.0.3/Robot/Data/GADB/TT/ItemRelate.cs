using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Robot.Data.GADB.TT {
    public class ItemRelate {
        public int ID { get; set; }
        public string RCompanyID { get; set; }
        public string CompanyID { get; set; }
        public string MasterItemID { get; set; }
        public string RelateItemID { get; set; }
        public string RelateType { get; set; }
        public string Remark { get; set; }
        public bool IsActive { get; set; }
    }

}
