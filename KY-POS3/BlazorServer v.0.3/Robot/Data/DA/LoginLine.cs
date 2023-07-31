using Robot.Data.GADB.TT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Robot.Data.ML.I_Result;

namespace Robot.Data.DA
{
    public class LoginLine
    {

        public static I_BasicResult CreateLineLogin(  string lineId, string user) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                var n = NewLineLogIn();
                
                n.LineID = lineId;
                n.UserID = user;
                using (GAEntities db = new GAEntities()) {
                    var exist = db.LineLogIn.Where(o => o.LineID == lineId && o.IsActive==true).FirstOrDefault();
                    if (exist==null) {
                        db.LineLogIn.Add(n);
                        db.SaveChanges();
                    } else {
                        exist.LatestLogin = DateTime.Now;
                        db.LineLogIn.Update(exist);
                        db.SaveChanges();
                    }
                  
                }
            } catch (Exception ex) {
                result.Result = "fail";
                if (ex.InnerException==null) {
                    result.Message1 = ex.InnerException.ToString();
                } else {
                    result.Message1 = ex.Message;
                }
                
            }
            return result;
    
        }


        public static LineLogIn NewLineLogIn() {
            LineLogIn n = new LineLogIn();

            n.RComID = "";
            n.ComID = "";
            n.AppID = "KYPOS";
            n.FromReqID = "";
            n.UserType = "USER";
            n.LineID = "";
            n.UserID = "";
            n.ReqMemo = "";
            n.ApprovedMemo = "";
            n.RequestDate = DateTime.Now;
            n.ApprovedBy = "";
            n.ApprovedDate = DateTime.Now;
            n.LatestLogin = DateTime.Now;
            n.IsActive = true;
            return n;
        }
        //public static I_BasicResult ActionRequest(string action, string reqId, string memo,string currentUser) {
        //    I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
        //    try {
        //        using (GAEntities db = new GAEntities()) {
        //            var rq = db.LineLogInRequest.Where(o => o.ReqID == reqId).FirstOrDefault();
        //            rq.ActionStatus = action.ToUpper();
        //            rq.ActionBy = currentUser;
        //            rq.ActionDate = DateTime.Now;
        //            rq.ActionMemo = memo;
        //            db.SaveChanges();

        //            if (action == "ACCEPTED") {
        //                var login = NewLineLogIn(rq);
        //                db.LineLogIn.Add(login);
        //                db.SaveChanges();
        //            }


        //        }
        //    } catch (Exception ex) {
        //        result.Result = "fail";
        //        if (ex.InnerException != null) {
        //            result.Message1 = ex.InnerException.ToString();
        //        } else {
        //            result.Message1 = ex.Message;
        //        }
        //    }
        //    return result;

        //}
        //public static LineLogIn NewLineLogIn(LineLogInRequest rq) {
        //    LineLogIn n = new LineLogIn();

        //    n.RComID = rq.RComID;
        //    n.ComID = rq.ComID;
        //    n.AppID = rq.AppID;
        //    n.FromReqID = rq.ReqID;
        //    n.UserType = rq.UserType;
        //    n.LineID = rq.LineID;
        //    n.UserID = rq.UserID;
        //    n.ReqMemo = rq.ReqMemo;
        //    n.ApprovedMemo = rq.ActionMemo;
        //    n.RequestDate = rq.RequestDate;
        //    n.ApprovedBy = "";
        //    n.ApprovedDate = null;
        //    n.IsActive = true;
        //    return n;
        //}
    }
}
