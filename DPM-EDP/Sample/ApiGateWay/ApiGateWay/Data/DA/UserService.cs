using ApiGateWay.Data.CimsDB;
using ApiGateWay.Models;
using Npgsql;
using System.Data;
using System.Linq;
using System.Security.Policy;
using static ApiGateWay.Data.ML.I_Result;

namespace ApiGateWay.Data.DA {
    public class UserService {
        public static IDbConnection Connection {
            get {
                return new NpgsqlConnection(Globals.CimsConn);
            }
        }

        public static I_BasicResult CheckLogin(AuthUser user) {
            I_BasicResult r = new I_BasicResult { Result = "fail", Message1 = "", Message2 = "" };
            try {
                var hash_pass = Services.Hash.hashPassword("MD5", user.Password);
                using (CimsContext db = new CimsContext()) {
                    var query = db.api_user.Where(o => o.username.ToLower() == user.Username.ToLower() && o.password == hash_pass && o.isactive == 1).FirstOrDefault();
                    if (query!=null) {
                        r.Result = "ok";
                    }
                }
            } catch (System.Exception ex) {
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
