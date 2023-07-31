using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RobotWasm.Server.Data.DA.Login;
using RobotWasm.Server.Data.DA.Master;
using RobotWasm.Shared.Data.DimsDB;
using RobotWasm.Shared.Data.ML.Master.User;
using System.Text.Json;
using System.Text.Json.Serialization;
using static RobotWasm.Shared.Data.ML.Shared.I_Result;

namespace RobotWasm.Server.Controllers.User {
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase {



        [HttpGet("GetDocSet")]
        public I_UserSet GetDocSet(string username) {

            return UserService.GetDocSet(username);
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
        public List<user_info> ListDoc(string? search) {
            search = search == null ? "" : search;
            return UserService.ListDoc(search);
        }

        [HttpGet("DeleteUser")]
        public I_BasicResult DeleteUser(string username)
        {
            return UserService.DeleteUser(username);
        }

        [HttpGet("UpdatePasswordNew")]
        public I_BasicResult UpdatePasswordNew(string? username,string password,string newPassword)
        {
            username = username == null ? "" : username;
            return LogInCIMSService.UpdatePasswordNew(username,password, newPassword);
        }

    }
}
