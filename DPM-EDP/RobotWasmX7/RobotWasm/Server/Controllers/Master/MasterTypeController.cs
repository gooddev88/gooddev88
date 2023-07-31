using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RobotWasm.Server.Data.DA.Master;
using RobotWasm.Shared.Data.DimsDB;
using RobotWasm.Shared.Data.GaDB;
using RobotWasm.Shared.Data.ML.Master.MasterType;
using System.Text.Json;
using System.Text.Json.Serialization;
using static RobotWasm.Shared.Data.ML.Shared.I_Result;

namespace RobotWasm.Server.Controllers.Master
{
    [Route("api/[controller]")]
    [ApiController]
    public class MasterTypeController : ControllerBase {


        [HttpGet("GetDocSet")]
        public I_MasterTypeSet GetDocSet(string docid) {
            return MastertypeService.GetDocSet(docid);
        }

        [HttpPost("SaveMaster")]
        public I_BasicResult SaveMaster([FromBody] string data, bool action) {

            var head = JsonSerializer.Deserialize<I_MasterTypeSet>(data, new JsonSerializerOptions {
                PropertyNameCaseInsensitive = true,
                ReferenceHandler = ReferenceHandler.Preserve
            });
            I_BasicResult r = MastertypeService.Save(head);
            return r;
        }

        [HttpGet("ListType")]
        public List<master_type_line> ListType(string? masid,bool isShowBlank) {
            masid = masid == null ? "" : masid;
            return MastertypeService.ListType(masid, isShowBlank);
        }

        [HttpGet("DeleteDoc")]
        public I_BasicResult DeleteDoc(string docid) {
            return MastertypeService.DeleteDoc(docid);
        }

        [HttpGet("GenSort")]
        public short GenSort() {
            return MastertypeService.GenSort();
        }

        [HttpGet("ListDoc")]
        public List<master_type_line> ListDoc(string? search) {
            search = search == null ? "" : search;
            return MastertypeService.ListDoc(search);
        }

    }
}
