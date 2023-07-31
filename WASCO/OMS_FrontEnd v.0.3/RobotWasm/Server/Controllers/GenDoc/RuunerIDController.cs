using Microsoft.AspNetCore.Mvc;
using RobotWasm.Server.Data.DA.GenDoc;
using RobotWasm.Shared.Data.ML.Param;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace RobotWasm.Server.Controllers.GenDoc {
    [Route("api/[controller]")]
    [ApiController]
    public class RuunerIDController : ControllerBase {

        //[HttpGet("GetNewIDV2")]
        //public List<string> GetNewIDV2(string docTypeId, string? rcom, string? comId, DateTime docdate, bool isrun_next, string year_culture) {
        //    rcom = rcom == null ? "" : rcom;
        //    comId = comId == null ? "" : comId;
        //    return IDRuunerService.GetNewIDV2(docTypeId, rcom, comId, docdate, isrun_next, year_culture);
        //}
        [HttpGet("GetNewIDV2")]
        public List<string> GetNewIDV2([FromBody] string data) {
            var doc = JsonSerializer.Deserialize<GenID>(data, new JsonSerializerOptions {
                PropertyNameCaseInsensitive = true,
                ReferenceHandler = ReferenceHandler.Preserve
            });
            return IDRuunerService.GetNewIDV2(doc.DocType, doc.RComID, doc.ComID, doc.DocDate, doc.Isrun_next, doc.year_culture);
           
        }
    }
}
