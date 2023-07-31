 
using RobotWasm.Shared.Data.GaDB;
using static RobotWasm.Shared.Data.ML.Shared.I_Result;

namespace RobotWasm.Shared.Data.ML.Master.Company {
    public class I_ComSet {
        public CompanyInfo Head { get; set; }
    }

    public class CompanyInfoList {
        public string RCompanyID { get; set; }
        public string CompanyID { get; set; }
        public string ComCode { get; set; }
        public string ParentID { get; set; }
        public string Name { get; set; }
        public string Company { get; set; }
        public string TypeName { get; set; }
        public string TypeID { get; set; }
        public string Phone { get; set; }
        public string TaxID { get; set; }
        public string FullAddr { get; set; }
        public bool IsWH { get; set; }
    }

}
