using System.Data;
using static RobotWasm.Shared.Data.ML.Shared.I_Result;
using RobotWasm.Shared.Data.GaDB;
using RobotWasm.Server.Data.GaDB;
using RobotWasm.Shared.Data.ML.Login;

namespace RobotWasm.Server.Data.DA.Master {
    public class CustomerService {

        public CustomerService() {

        }

        #region Get List

        public static CustomerInfo GetCustomerInfoByCusID(string rcom, string comId,string cusid) {
            CustomerInfo result = new CustomerInfo();
            using (GAEntities db = new GAEntities()) {
                result = db.CustomerInfo.Where(o => o.CompanyID == comId && o.RCompanyID == rcom && o.CustomerID == cusid  ).FirstOrDefault();
            }
            return result;
        }

        public static List<CustomerInfo> ListCustomerInfo(string rcom, string comId,string  brand,string search) {
            search= search==null? "" : search.Trim(); 
            List<CustomerInfo> result = new List<CustomerInfo>();
            using (GAEntities db = new GAEntities()) {
                result = db.CustomerInfo.Where(o =>
                                                        o.CompanyID == comId && o.RCompanyID == rcom 
                                                        && o.GroupID== brand
                                                        && o.IsActive == true
                                                        && (
                                                        o.CustomerID.Contains(search)
                                                        || o.FullNameTh.Contains(search)
                                                        || search == ""
                                                        ) 
                                                        ).ToList();
            }
            return result;
        }

        public static CustomerInfo GetCustInfo(string custId, string rcom, string com) {
            CustomerInfo result = new CustomerInfo();
            using (GAEntities db = new GAEntities()) {
                result = db.CustomerInfo.Where(o => o.CustomerID == custId && o.RCompanyID == rcom && o.CompanyID == com).FirstOrDefault();
            }
            return result;
        }
        #endregion

        #region tax type
        public static decimal GetTaxRate(string rcom, string type, string taxid) {
            decimal rate = 0;
            using (GAEntities db = new GAEntities()) {
                var query = db.TaxInfo.Where(o => o.RComID == rcom && o.Type == type && o.TaxTypeID == taxid).FirstOrDefault();
                if (query!=null) {
                    rate = query.TaxValue;
                }
            }
            return rate;
        }
        #endregion

    }
}
