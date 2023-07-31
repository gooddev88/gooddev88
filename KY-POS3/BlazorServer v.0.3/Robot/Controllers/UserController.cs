using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Robot.Data.DA;
using Robot.Data.GADB.TT;
using Robot.Helper.Hash;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Robot.Data.ML.I_Result;

namespace Robot.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase {


        [HttpGet("SetPassowrd")]
        public I_BasicResult SetPassowrd(string userid) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            if (userid==null) {
                userid = "";
            }
            try {
                using (GAEntities db = new GAEntities()) {
                    var user = db.UserInfo.Where(o =>
                                                         (o.Username == userid || userid=="")
                                                         && o.Email!=""
                                                        ).ToList();
                    foreach (var u in user) {
                        u.Password=   Hash.hashPassword("MD5", u.Email);
                        db.UserInfo.Update(u);
                    }
                    db.SaveChanges();
                }
                
            } catch (Exception ex) {
                result.Result = "fail";
                if (ex.InnerException!=null) {
                    result.Message1 = ex.InnerException.ToString();
                } else {
                    result.Message1 = ex.Message;
                }
            }
            return result;
        }

    }
}
