using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RobotWasm.Server.Data.DA.DataQuality;
using RobotWasm.Shared.Data.DimsDB;
using static RobotWasm.Shared.Data.ML.Shared.I_Result;

namespace RobotWasm.Server.Controllers.DataQuality {
    [Route("api/[controller]")]
    [ApiController]
    public class DataQualityController : ControllerBase {

        [AllowAnonymous]
        [HttpGet("ListDataQuality")]
        async public Task<List<dqt_data_logs>> ListDataQuality(string? search,string? datebegin, string? dateend) {

            return DataQualityService.ListDataQuality(search,Convert.ToDateTime(datebegin),Convert.ToDateTime(dateend));
        }
    }
}
