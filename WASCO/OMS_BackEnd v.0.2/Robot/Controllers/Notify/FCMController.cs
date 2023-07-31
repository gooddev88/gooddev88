using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Robot.Data.DA;
using Robot.Helper.FCM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Robot.Data.ML.I_Result;

namespace Robot.Controllers {
    [Route("api/Notify/[controller]")]
    [ApiController]
    public class FCMController : ControllerBase {
        public FireBaseService _fireBaseService;
  
        public FCMController(  FireBaseService fireBaseService) { 
            _fireBaseService = fireBaseService;
        }
        [HttpPost("FCMTokenValidate")]
        public string FCMTokenValidate(List<string> token) {
            //token = "dwyq2hsFtU:APA91bGF5pEyccccGRdHZd0PgxVhTHXKaN5a4DcXNbtAYf9pPFqXOKJ4O_zybR8gyBLjvUOtB9G-WvUXc5KjCLXag8IEas3hWwjRbk8fX2umiPTVPaGlOhihHRwRCwd2X2o9rXDHgnGLzRzW";
            string statuscode = "fail";
            try {
                statuscode = _fireBaseService.ValidateToken(token.FirstOrDefault());
            } catch (Exception) {
                statuscode = "fail";
            } 
            return statuscode;
        }
        [HttpGet("Sendnotify")]
        public I_BasicResult Sendnotify(string rcom,string com) { 
               return FireBaseService.Sendnotify(rcom,com); 
        }

    }
}
