using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RobotWasm.Server.Data.DA.GenDoc;
using RobotWasm.Server.Data.DA.Master;
using RobotWasm.Shared.Data.DimsDB;
using System.Text.Json;
using System.Text.Json.Serialization;
using static RobotWasm.Shared.Data.ML.Shared.I_Result;

namespace RobotWasm.Server.Controllers.GenDoc
{
    [Route("api/[controller]")]
    [ApiController]
    public class RuunerIDController : ControllerBase {

        [HttpGet("GetNewIDV2")]
        public List<string> GetNewIDV2(string docTypeId, string? rcom, string? comId, DateTime docdate, bool isrun_next, string year_culture) {
            rcom = rcom == null ? "" : rcom;
            comId = comId == null ? "" : comId;
            return IDRuunerService.GetNewIDV2(docTypeId,rcom,comId,docdate, isrun_next,year_culture);
        }

    }
}
