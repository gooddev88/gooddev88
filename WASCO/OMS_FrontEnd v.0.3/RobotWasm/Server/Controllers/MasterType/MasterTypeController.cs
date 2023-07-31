using Microsoft.AspNetCore.Mvc;
using RobotWasm.Server.Data.DA.Master;
using RobotWasm.Shared.Data.ML.Master.MasterType;
using RobotWasm.Shared.Data.GaDB;
using System.Text.Json;
using System.Text.Json.Serialization;
using static RobotWasm.Shared.Data.ML.Shared.I_Result;

namespace RobotWasm.Server.Controllers.MasterType {
    [Route("api/[controller]")]
    [ApiController]
    public class MasterTypeController : ControllerBase {


        [HttpGet("GetDocSet")]
        public I_MasterTypeSet GetDocSet(string docid,string rcom) {
            return MastertypeService.GetDocSet(docid,rcom);
        }

        [HttpPost("SaveMaster")]
        public I_BasicResult SaveMaster([FromBody] string data) {

            var head = JsonSerializer.Deserialize<MasterTypeLine>(data, new JsonSerializerOptions {
                PropertyNameCaseInsensitive = true,
                ReferenceHandler = ReferenceHandler.Preserve
            });
            I_BasicResult r = MastertypeService.Save(head);
            return r;
        }

        [HttpPost("ReOrder")]
        public I_BasicResult ReOrder([FromBody] string data)
        {
            var doc = JsonSerializer.Deserialize<List<MasterTypeLine>>(data, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                ReferenceHandler = ReferenceHandler.Preserve
            });
            I_BasicResult r = MastertypeService.ReOrder(doc);
            return r;
        }

        [HttpGet("ListMasterType")]
        public List<MasterTypeLine> ListMasterType(string? rcom,string? masid,bool isShowBlank) {
            rcom = rcom == null ? "" : rcom;
            masid = masid == null ? "" : masid;
            return MastertypeService.ListType(rcom,masid,isShowBlank);
        }


        [HttpGet("ListViewMasterType")]
        public List<vw_MasterTypeLine> ListViewMasterType(string? rcom, string? masid, bool isShowBlank) {
            rcom = rcom == null ? "" : rcom;
            masid = masid == null ? "" : masid;
            return MastertypeService.ListViewType(rcom, masid, isShowBlank);
        }

        [HttpGet("ListDoc")]
        public List<MasterTypeHead> ListDoc(string? search) {
            search = search == null ? "" : search;
            return MastertypeService.ListDoc(search);
        }

    }
}
