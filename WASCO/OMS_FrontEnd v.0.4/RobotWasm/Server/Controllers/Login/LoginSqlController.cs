using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RobotWasm.Server.Data.DA.Login;
using RobotWasm.Shared.Data.ML.Login;
using RobotWasm.Shared.Data.ML.Param;
using System.Text.Json.Serialization;
using System.Text.Json;
using RobotWasm.Shared.Data.GaDB;

namespace RobotWasm.Server.Controllers.Login {
    [Route("api/[controller]")]
    [ApiController]
    public class LoginSqlController : ControllerBase {
        LogInSqlService _logInService;
        public LoginSqlController(LogInSqlService logInService) {
            _logInService = logInService;
        }




        #region login cross app
        

            [HttpPost("CreateCrossAppReq")]
            public LoginCrossReturn CreateCrossAppReq([FromBody] string data) {
            var doc = JsonSerializer.Deserialize<LoginCrossRequest>(data, new JsonSerializerOptions {
                PropertyNameCaseInsensitive = true,
                ReferenceHandler = ReferenceHandler.Preserve
            });
            return LogInSqlService.CreateCrossAppReq(doc);
            }

        [HttpGet("GetCrossAppReq")]
        public LogInCrossAppReq GetCrossAppReq(string reqid) {
        
            return LogInSqlService.GetCrossAppReq(reqid);
        }
        #endregion

    }
}
