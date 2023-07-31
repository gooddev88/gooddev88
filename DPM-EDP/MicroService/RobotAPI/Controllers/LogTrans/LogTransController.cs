using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RobotAPI.Data;
using RobotAPI.Data.CimsDB.TT;
using RobotAPI.Data.DA.FileGo;
using RobotAPI.Data.DA.Logs;
using static RobotAPI.Data.ML.Shared.I_Result;

namespace RobotAPI.Controllers.Today {
    [Route("api/[controller]")]
    [ApiController]
    public class LogTransController : ControllerBase {
        [AllowAnonymous]
        [HttpPost("CreateLogs")]
        async public Task<I_BasicResult> CreateLogs(trans_logs input) {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            r = LogTranService.CreateLog(input);
            return r;
        }
        [AllowAnonymous]
        [HttpGet("ListLogs")]
        async public Task<List<trans_logs>> ListLogs(string doc_id, string app_id, string module) {

            return LogTranService.ListLogs(doc_id, app_id, module);
        }
        [AllowAnonymous]
        [HttpGet("ListLogsHistory")]
        async public Task<List<trans_logs>> ListLogsHistory(string app_id,string search) {

            return LogTranService.ListLogsHistory(app_id, search);
        }
    }
}
