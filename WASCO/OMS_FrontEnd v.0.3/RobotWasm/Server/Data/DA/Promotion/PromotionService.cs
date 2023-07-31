using RobotWasm.Server.Data.GaDB;
using static RobotWasm.Shared.Data.ML.Shared.I_Result;
using RobotWasm.Shared.Data.GaDB;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using RobotWasm.Shared.Data.ML.Shared; 
using RobotWasm.Server.Data.DA.Line;
using RobotWasm.Shared.Data.ML.Promotion;

namespace RobotWasm.Server.Data.DA.Promotion
{
    public class PromotionService {

        #region promotion
        public static List<Promotions> ListPromotion(string rcom, string com, DateTime tranDate) {
            List<Promotions> result = new List<Promotions>();
            try {
                using (GAEntities db = new GAEntities()) {
                    result = db.Promotions.Where(o => o.RCompanyID == rcom
                                                            && o.CompanyID == com
                                                            && o.IsActive == true
                                                            && (tranDate >= o.DateBegin && tranDate <= o.DateEnd)
                                                            ).OrderBy(o => o.ProID).ToList();
                }
            } catch (Exception ex) {
            }
            return result;
        }


        public static Promotions? GetPromotionInfo(string rcom, string com, string proId) {
            Promotions? result = new Promotions();
            try {
                using (GAEntities db = new GAEntities()) {
                    result = db.Promotions.Where(o => o.RCompanyID == rcom
                                                            && o.CompanyID == com
                                                            && o.ProID == proId 
                                                            ).FirstOrDefault(); 
                }
            } catch (Exception ex) {
            }
            return result;
        }

        #endregion




    }
}
