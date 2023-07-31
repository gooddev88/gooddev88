using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotWasm.Shared.Data.GaDB {
    public class MasterTypeHead
    {
        public string MasterTypeID { get; set; }
        public string UseFor { get; set; }
        public string Name { get; set; }
        public bool UserAddNew { get; set; }
        public string ParentID { get; set; }
        public string Remark { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool IsActive { get; set; }
    }
}
