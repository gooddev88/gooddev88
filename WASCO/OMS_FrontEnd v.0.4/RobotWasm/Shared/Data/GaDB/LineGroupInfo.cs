using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotWasm.Shared.Data.GaDB {
    public partial class LineGroupInfo {
        public int ID { get; set; }
        public string RComID { get; set; }
        public string ComID { get; set; }
        public string LineGroupID { get; set; }
        public string GroupName { get; set; }
        public string GroupRef1 { get; set; }
        public string GroupRef2 { get; set; }
        public string LineTokenID { get; set; }
        public int? CountMember { get; set; }
        public string Status { get; set; }
        public bool IsActive { get; set; }
    }
}
