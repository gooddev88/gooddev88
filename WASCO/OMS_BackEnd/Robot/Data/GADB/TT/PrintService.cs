using System;
using static Robot.Data.ML.I_Result;

namespace Robot.Data.GADB.TT {
    public class PrintService {

        public static I_BasicResult CreatePrintData(PrintData data) {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                using (GAEntities db = new GAEntities()) {
                    data.PrintID = Guid.NewGuid().ToString();
                    data.PrintDate = DateTime.Now;
                    db.PrintData.Add(data);
                    db.SaveChanges();
                }
            } catch (Exception ex) {
                r.Result = "fail";
                r.Message1 = ex.Message;
            }

            return r;
        }
    }
}
