using Robot.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static Robot.Data.BL.I_Result;

namespace Robot.Communication.DA {
    public static class LogInFromOuterService {
        #region class

        
        public class ILogInSet {
            public string InnerUserID { get; set; }
            public string Apps { get; set; }
            public string OuterUserID { get; set; }
            public DateTime LogInDate { get; set; }
            public I_BasicResult Output { get; set; } 
        }
        #endregion
        #region  Global Var 
        public static ILogInSet LogOuterInfo { get { return (ILogInSet)HttpContext.Current.Session["login_outer_info"]; } set { HttpContext.Current.Session["login_outer_info"] = value; } }
        #endregion

        #region Query
        public static ILogInSet Login(string outerUser,string uType ,string apps) {
            ILogInSet r = NewTransaction();
            using (GAEntities db = new GAEntities()) {
                switch (apps) {
                    case "LINE":
                        var lineUser = db.LineLogIn.Where(o => o.IsActive && o.LineID == outerUser && o.UserType==uType).FirstOrDefault();
                        if (lineUser==null) {
                            r.Output.Result = "fail";
                            r.Output.Message1 = "You are not register.";
                            return r;
                        }
                        var user = db.UserInfo.Where(o => o.IsActive && o.Username == lineUser.UserID).FirstOrDefault();
                        if (user==null) {
                            r.Output.Result = "fail";
                            r.Output.Message1 = "No user information found.";
                            return r;
                        }
                        r.InnerUserID = user.Username;
                        r.OuterUserID = outerUser;
                        r.Apps = apps;
                        LogOuterInfo = r;
                        break;
                    default:
                        break;
                }
           
            }
          
            return r;
        }
        #endregion

        #region Transaction
        public static ILogInSet NewTransaction() {
            ILogInSet r = new ILogInSet();
            r.InnerUserID = "";
            r.Apps = "";
            r.OuterUserID = "";
            r.LogInDate = DateTime.Now;
            r.Output = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            return r;
        }
        #endregion

    }
}