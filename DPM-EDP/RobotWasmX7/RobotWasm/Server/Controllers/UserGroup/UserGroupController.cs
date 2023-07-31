using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RobotWasm.Server.Data.DA.Master;
using RobotWasm.Shared.Data.DimsDB;
using RobotWasm.Shared.Data.ML.Master.UserGroup;
using System.Text.Json;
using System.Text.Json.Serialization;
using static RobotWasm.Shared.Data.ML.Shared.I_Result;

namespace RobotWasm.Server.Controllers.UserGroup {
    [Route("api/[controller]")]
    [ApiController]
    public class UserGroupController : ControllerBase {



        [HttpGet("GetDocSet")]
        public I_userGroupSet GetDocSet(string groupid) {

            return UserGroupService.GetDocSet(groupid);
        }

        [HttpGet("GenSort")]
        public short GenSort() {

            return UserGroupService.GenSort();
        }

        [HttpPost("Save")]
        public I_BasicResult Save([FromBody] string data) {

            var usergroup = JsonSerializer.Deserialize<I_userGroupSet>(data, new JsonSerializerOptions {
                PropertyNameCaseInsensitive = true,
                ReferenceHandler = ReferenceHandler.Preserve
            });
            I_BasicResult r = UserGroupService.Save(usergroup);
            return r;
        }

        [HttpGet("ListDoc")]
        public List<usergroup_info> ListDoc(string? search) {
            search = search == null ? "" : search;
            return UserGroupService.ListDoc(search);
        }

        [HttpGet("DeleteUserGroup")]
        public I_BasicResult DeleteUserGroup(string usergroup)
        {
            return UserGroupService.DeleteUserGroup(usergroup);
        }

    }
}
