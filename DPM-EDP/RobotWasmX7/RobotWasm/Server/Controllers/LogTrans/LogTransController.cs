using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RobotWasm.Server.Data.DA.LogTran;
using RobotWasm.Shared.Data.DimsDB;
using static RobotWasm.Shared.Data.ML.Shared.I_Result;

namespace RobotWasm.Server.Controllers.LogTrans {
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
        async public Task<List<vw_trans_logs>> ListLogsHistory(string? app_id, string? search,string? DateFrom, string? DateTo) {

            return LogTranService.ListLogsHistory(app_id,search,Convert.ToDateTime(DateFrom),Convert.ToDateTime(DateTo));
        }

        [AllowAnonymous]
        [HttpGet("CreateTransLog")]
        async public Task<I_BasicResult> CreateTransLog(string? docid, string? user, string? module,string? action)
        {
            docid = docid == null ? "" : docid;
            user = user == null ? "" : user;
            module = module == null ? "" : module;
            action = action == null ? "" : action;
            return LogTranService.CreateTransLog(docid,user,module,action);
        }

    }
}
