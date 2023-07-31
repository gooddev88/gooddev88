using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RobotWasm.Server.Data.DA.Master;
using RobotWasm.Shared.Data.DimsDB;
using RobotWasm.Shared.Data.GaDB;
using RobotWasm.Shared.Data.ML.Master.ComGroup;
using System.Text.Json;
using System.Text.Json.Serialization;
using static RobotWasm.Shared.Data.ML.Shared.I_Result;

namespace RobotWasm.Server.Controllers.ComGroup
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComGroupController : ControllerBase {


        [HttpGet("GetDocSetComGroup")]
        public I_ComGroupSet GetDocSetComGroup(string docid) {
            return CompanyGroupService.GetDocSetComGroup(docid);
        }

        [HttpPost("SaveComGroup")]
        public I_BasicResult SaveComGroup([FromBody] string data, bool action) {
            var head = JsonSerializer.Deserialize<I_ComGroupSet>(data, new JsonSerializerOptions {
                PropertyNameCaseInsensitive = true,
                ReferenceHandler = ReferenceHandler.Preserve
            });
            I_BasicResult r = CompanyGroupService.Save(head);
            return r;
        }

        [HttpGet("DeleteDocComGroup")]
        public I_BasicResult DeleteDocComGroup(string docid,string modifyby) {
            return CompanyGroupService.DeleteDocComGroup(docid, modifyby);
        }

        [HttpGet("Checkduplicate")]
        public company_group_info Checkduplicate(string comgroupid)
        {
            return CompanyGroupService.Checkduplicate(comgroupid);
        }

        [HttpGet("ListDocHead")]
        public List<company_group_info> ListDocHead(string? search) {
            search = search == null ? "" : search;
            return CompanyGroupService.ListDocHead(search);
        }



    }
}
