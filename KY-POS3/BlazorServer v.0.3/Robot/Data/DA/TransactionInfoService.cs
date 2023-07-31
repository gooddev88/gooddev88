using Robot.Data.ML;
using Robot.Data.GADB.TT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Robot.Data.ML.I_Result;

namespace Robot.Data.DA {
    public class TransactionService {

        #region Method GET

        public static List<TransactionLog> ListLogByDocID(string docId, string rcom, string com) {
            List<TransactionLog> result = new List<TransactionLog>();
            using (GAEntities db = new GAEntities()) {
                result = db.TransactionLog.Where(o => (o.TransactionID == docId && o.RCompanyID == rcom && o.CompanyID == com)).ToList();
            }
            return result;
        }
        #endregion

        public static I_BasicResult SaveLog(TransactionLog data, string xrom, string xcom, string xuser) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            var rcom = xrom;
            var com = xcom;
            var user = xuser;
            try {
                data.RCompanyID = rcom;
                data.CompanyID = com;
                data.TableID = data.TableID == null ? "Unknown table" : data.TableID;
                data.CreatedBy = user;
                data.CreatedDate = DateTime.Now;
                data.ChangeValue = data.ChangeValue == null ? "" : data.ChangeValue;

                data.ParentID = data.ParentID == null ? "" : data.ParentID;
                data.Action = data.Action == null ? "" : data.Action;

                data.ActionType = data.ActionType == null ? "" : data.ActionType;
                data.IsActive = true;
                using (GAEntities db = new GAEntities()) {
                    db.TransactionLog.Add(data);
                    var r = db.SaveChanges();
                }

            } catch (Exception ex) {
                result.Result = "fail";
                if (ex.InnerException != null) {
                    result.Message1 = ex.InnerException.ToString();
                } else {
                    result.Message1 = ex.Message;
                }


            }

            return result;
        }



    }
}
