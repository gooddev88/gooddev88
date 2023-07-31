using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RobotWasm.Server.Data.DA.GenDoc;
using RobotWasm.Server.Data.DA.Master;
using RobotWasm.Server.Data.DA.Print;
using RobotWasm.Server.Service.FileGo;
using RobotWasm.Shared.Data.ML.FileGo;
using RobotWasm.Shared.Data.ML.Param;
using RobotWasm.Shared.Data.ML.Print;
using System.Text.Json;
using System.Text.Json.Serialization;
using static RobotWasm.Shared.Data.DA.POSFuncService;
using static RobotWasm.Shared.Data.ML.Print.SalePrintConverter;
using static RobotWasm.Shared.Data.ML.Shared.I_Result;

namespace RobotWasm.Server.Controllers.Print
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrintController : ControllerBase {

        [HttpPost("RunReport")]
        public async Task<I_BasicResult> RunReport([FromBody] string data, string printform_id) {
            var i = JsonSerializer.Deserialize<I_POSSaleSet>(data, new JsonSerializerOptions {
                PropertyNameCaseInsensitive = true,
                ReferenceHandler = ReferenceHandler.Preserve
            });

            I_BasicResult r = await Task.Run(() => PrintService.Convert2PrintData(i, printform_id)); ;
            return r;
        }

    }
}
