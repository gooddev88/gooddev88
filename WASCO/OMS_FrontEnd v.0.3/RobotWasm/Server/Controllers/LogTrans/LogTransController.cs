using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RobotWasm.Server.Data.DA.LogTran;
using RobotWasm.Shared.Data.TFEDBF;
using static RobotWasm.Shared.Data.ML.Shared.I_Result;

namespace RobotWasm.Server.Controllers.LogTrans {
    [Route("api/[controller]")]
    [ApiController]
    public class LogTransController : ControllerBase {
        [AllowAnonymous]
        [HttpPost("CreateLogs")]
        async public Task<I_BasicResult> CreateLogs(y_trans_logs input) {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            r = LogTranService.CreateLog(input);
            return r;
        }
        //[AllowAnonymous]
        //[HttpGet("ListLogs")]
        //async public Task<List<trans_logs>> ListLogs(string doc_id, string app_id, string module) {

        //    return LogTranService.ListLogs(doc_id, app_id, module);
        //}
        //[AllowAnonymous]
        //[HttpGet("ListLogsHistory")]
        //async public Task<List<trans_logs>> ListLogsHistory(string? app_id, string? search) {

        //    return LogTranService.ListLogsHistory(app_id, search);
        //}
    }
}
