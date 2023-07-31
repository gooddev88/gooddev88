using Microsoft.AspNetCore.Mvc;
using RobotWasm.Server.Data.DA.GenDoc;
using RobotWasm.Shared.Data.ML.Param;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace RobotWasm.Server.Controllers.GenDoc {
    [Route("api/[controller]")]
    [ApiController]
    public class RuunerIDController : ControllerBase {

        [HttpPost("GenPOSSaleID")]
        public List<string> GenPOSSaleID([FromBody] string data){
            var doc = JsonSerializer.Deserialize<GenPOSSaleIDParam>(data, new JsonSerializerOptions {
                PropertyNameCaseInsensitive = true,
                ReferenceHandler = ReferenceHandler.Preserve
            });
            return IDRuunerService.GenPOSSaleID(doc.DocType,doc.Rcom,doc.StoreId,doc.Macno,doc.ShiptoId,doc.Isrun_next,doc.TransDate);
        }

        [HttpPost("GetNewIDV2")]
        public List<string> GetNewIDV2([FromBody] string data) {
            var doc = JsonSerializer.Deserialize<GenIDParam>(data, new JsonSerializerOptions {
                PropertyNameCaseInsensitive = true,
                ReferenceHandler = ReferenceHandler.Preserve
            });
            return IDRuunerService.GetNewIDV2(doc.DocType, doc.RComID,doc.ComID,doc.DocDate,doc.Isrun_next,doc.Year_culture);
        }

    }
}
