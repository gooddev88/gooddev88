using System.Data;
using static RobotWasm.Shared.Data.ML.Shared.I_Result;
using RobotWasm.Shared.Data.GaDB;
using RobotWasm.Server.Data.GaDB;
using static RobotWasm.Client.Data.DA.Master.CompanyService;
using RobotWasm.Shared.Data.ML.Master.Company;
using RobotWasm.Shared.Data.ML.Login;
using RobotWasm.Server.Data.DA.Login;

namespace RobotWasm.Server.Data.DA.Master {
    public class CompanyService {

        public CompanyService() {

        }

        #region Get List

        public static CompanyInfo GetComInfoByComID(string rcom, string comId) {
            CompanyInfo com = new CompanyInfo();
            using (GAEntities db = new GAEntities()) {
                com = db.CompanyInfo.Where(o => o.CompanyID == comId && o.RCompanyID == rcom).FirstOrDefault();
            }

            return com;
        }

        public static CompanyInfo GetComInfoByRComID(string comId) {
            CompanyInfo com = new CompanyInfo();
            using (GAEntities db = new GAEntities()) {
                com = db.CompanyInfo.Where(o => o.CompanyID == comId).FirstOrDefault();
            }

            return com;
        }

        public static List<CompanyInfoList> ListCompanyInfo(string type, bool addShowAll) {
            List<CompanyInfoList> result = new List<CompanyInfoList>();
            using (GAEntities db = new GAEntities()) {
                var query = db.vw_CompanyInfo.Where(o =>
                                                    (o.TypeID == type || type == "")
                                                     && o.IsActive
                                                ).ToList();
                foreach (var q in query) {
                    CompanyInfoList n = new CompanyInfoList();
                    n.RCompanyID = q.RCompanyID;
                    n.CompanyID = q.CompanyID;
                    n.Name = q.Name1 + " " + q.Name2 + " (" + q.CompanyID + ")";
                    n.TypeName = q.TypeName;
                    n.TypeID = q.TypeID;
                    n.FullAddr = q.AddrFull;
                    n.TaxID = q.TaxID;
                    n.IsWH = Convert.ToBoolean(q.IsWH);
                    result.Add(n);
                }
                if (addShowAll) {
                    CompanyInfoList blank = new CompanyInfoList { ComCode = "", CompanyID = "", Name = "ทุกสาขา", TypeID = "", TaxID = "", FullAddr = "" };
                    result.Insert(0, blank);
                }

            }
            return result;
        }


        public static List<CompanyInfoList> ListCompanyInfoByComID(List<string> param) {


            List<CompanyInfoList> result = new List<CompanyInfoList>();
            using (GAEntities db = new GAEntities()) {
                var query = db.vw_CompanyInfo.Where(o =>
                                                       param.Contains(o.CompanyID)
                                                     && o.IsActive
                                                ).ToList();
                foreach (var q in query) {
                    CompanyInfoList n = new CompanyInfoList();
                    n.RCompanyID = q.RCompanyID;
                    n.CompanyID = q.CompanyID;
                    n.Name = q.Name1 + " " + q.Name2;//q.Name1 + " " + q.Name2 + " (" + q.CompanyID + ")";
                    n.TypeName = q.TypeName;
                    n.TypeID = q.TypeID;
                    n.FullAddr = q.AddrFull;
                    n.TaxID = q.TaxID;
                    n.IsWH = Convert.ToBoolean(q.IsWH);
                    result.Add(n);
                }
                //if (addShowAll) {
                //    CompanyInfoList blank = new CompanyInfoList { ComCode = "", CompanyID = "", Name = "ทุกสาขา", TypeID = "", TaxID = "", FullAddr = "" };
                //    result.Insert(0, blank);
                //}

            }
            return result;
        }

        #endregion

    }
}
