using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Robot.Data.DA.API;
using Robot.Data.GADB.TT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Robot.Data.ML.I_Result;

namespace Robot.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class LogAPIController : ControllerBase {


        public I_BasicResult AddAPILog(LogAPIRequest data) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                result = LogAPIService.AddLogRequest(data);
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
