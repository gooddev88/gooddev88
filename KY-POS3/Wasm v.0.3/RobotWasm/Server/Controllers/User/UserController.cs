using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RobotWasm.Server.Data.DA.Master;
using RobotWasm.Shared.Data.ML.Master.User;
using RobotWasm.Shared.Data.GaDB;
using System.Text.Json;
using System.Text.Json.Serialization;
using static RobotWasm.Shared.Data.ML.Shared.I_Result;

namespace RobotWasm.Server.Controllers.User {
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase {

        [HttpGet("GetDocSet")]
        public I_UserSet GetDocSet(string username,string rcom, string? userlogin) {
            userlogin = userlogin == null ? "" : userlogin;
            return UserService.GetDocSet(username,rcom,userlogin);
        }

        [HttpPost("Save")]
        public I_BasicResult Save([FromBody] string data) {

            var user = JsonSerializer.Deserialize<I_UserSet>(data, new JsonSerializerOptions {
                PropertyNameCaseInsensitive = true,
                ReferenceHandler = ReferenceHandler.Preserve
            });
            I_BasicResult r = UserService.Save(user);
            return r;
        }

        [HttpPost("ReSetPassword")]
        public I_BasicResult ReSetPassword([FromBody] string data)
        {
            var user = JsonSerializer.Deserialize<I_UserSet>(data, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                ReferenceHandler = ReferenceHandler.Preserve
            });
            I_BasicResult r = UserService.ReSetPassword(user);
            return r;
        }

        [HttpGet("ListDoc")]
        public List<vw_UserInfo> ListDoc(string? search,string? rcom) {
            search = search == null ? "" : search;
            rcom = rcom == null ? "" : rcom;
            return UserService.ListDoc(search,rcom);
        }

        [HttpGet("ListUserInfo")]
        public IEnumerable<UserInfo> ListUserInfo()
        {
            return UserService.ListUserInfo();
        }

        [HttpGet("GetUserInfo")]
        public UserInfo GetUserInfo(string? username)
        {
            username = username == null ? "" : username;
            return UserService.GetUserInfo(username);
        }

    }
}
