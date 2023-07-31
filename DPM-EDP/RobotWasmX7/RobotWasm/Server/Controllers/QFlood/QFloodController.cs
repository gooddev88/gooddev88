using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RobotWasm.Server.Data.DA.Q;
using RobotWasm.Shared.Data.GaDB;
using System.Text.Json;
using System.Text.Json.Serialization;
using static RobotWasm.Shared.Data.ML.Q.QFloodServiceModel;
using static RobotWasm.Shared.Data.ML.Shared.I_Result;

namespace RobotWasm.Server.Controllers.QFlood {
    [Route("api/[controller]")]
    [ApiController]
    public class QFloodController : ControllerBase {



        [HttpGet("GetDocSet")]
        public QFloodDocSet GetDocSet(string mcode) {

            return QFloodService.GetDocSet(mcode);
        }

        [HttpGet("NewTransaction")]
        public QFloodDocSet NewTransaction(string mcode) {

            return QFloodService.NewTransaction(mcode);
        }

        [HttpGet("GetVillage")]
        public a_mm GetVillage(string mcode) {

            return QFloodService.GetVillage(mcode);
        }

        [HttpPost("SaveQFlood")]
        public I_BasicResult SaveQFlood([FromBody] string data,bool action) {

            var qflood = JsonSerializer.Deserialize<QFloodDocSet>(data, new JsonSerializerOptions {
                PropertyNameCaseInsensitive = true,
                ReferenceHandler = ReferenceHandler.Preserve
            });
            I_BasicResult r = QFloodService.Save(qflood, action);
            return r;

        }

        [HttpGet("ListVillage")]
        public List<a_mm> ListVillage(string? search) {
            search = search == null ? "" : search;
            return QFloodService.ListVillage(search);
        }

    }
}
