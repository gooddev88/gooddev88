using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RobotWasm.Server.Data.DA.Login;
using RobotWasm.Shared.Data.ML.Login;

namespace RobotWasm.Server.Controllers.Login {
    [Route("api/[controller]")]
    [ApiController]
    public class LoginSqlController : ControllerBase {
        LogInSqlService _logInService;
        public LoginSqlController(LogInSqlService logInService) {
            _logInService = logInService;
        }

        [HttpPost("Login")]
        public LoginSet Login(LoginRequest input) {
            return LogInSqlService.Login (input.UserName, input.Password, "", "");
        }


    }
}
