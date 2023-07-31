using Robot.Data.GADB.TT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Robot.Data.ML.I_Result;

namespace Robot.Data.DA.API {
    public class LogAPIService {


        public static I_BasicResult AddLogRequest(LogAPIRequest data) {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {               
                using (GAEntities db = new GAEntities()) {
                    data.RequestDate = DateTime.Now;
                    data.IsActive = true;
                    db.LogAPIRequest.Add(data);
                    db.SaveChanges();
                }
            } catch (Exception ex) {
                r.Result = "fail";
                if (ex.InnerException!=null) {
                    r.Message1 = ex.InnerException.ToString();
                } else {
                    r.Message1 = ex.Message;
                }
            }
            return r;
        }

      
       
    }
}
