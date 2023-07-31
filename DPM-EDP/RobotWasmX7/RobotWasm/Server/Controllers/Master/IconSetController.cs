using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RobotWasm.Shared.Data.GaDB;
using System.Text.Json;
using System.Text.Json.Serialization;
using RobotWasm.Shared.Data.ML.Master.IconSet;
using static RobotWasm.Shared.Data.ML.Shared.I_Result;
using RobotWasm.Shared.Data.DimsDB;
using RobotWasm.Server.Data.DA.Master;

namespace RobotWasm.Server.Controllers.Master {
    [Route("api/[controller]")]
    [ApiController]
    public class IconSetController : ControllerBase {

        [HttpGet("GetDocSet")]
        public I_IconSet GetDocSet(string docid) {
            return IconSetService.GetDocSet(docid);
        }

        [HttpPost("SaveIconSet")]
        public I_BasicResult SaveIconSet([FromBody] string data, bool action) {
            var dataset = JsonSerializer.Deserialize<I_IconSet>(data, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                ReferenceHandler = ReferenceHandler.Preserve
            });
            I_BasicResult r = IconSetService.Save(dataset, action);
            return r;
        }

        [HttpPost("GetLatestFiles")]
        public I_IconSet GetLatestFiles([FromBody] I_IconSet data)
        {
            I_IconSet r = IconSetService.GetLatestFiles(data);
            return r;
        }

        [HttpGet("DeleteDoc")]
        public I_BasicResult DeleteDoc(string docid) {
            return IconSetService.DeleteDoc(docid);
        }

        [HttpGet("GenSort")]
        public short GenSort() {
            return IconSetService.GenSort();
        }

        [HttpGet("ListDoc")]
        public List<vw_icon_set> ListDoc(string? search)
        {
            search = search == null ? "" : search;
            return IconSetService.ListDoc(search);
        }

    }
}
