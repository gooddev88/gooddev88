using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RobotWasm.Server.Data.DA.Login;
using RobotWasm.Shared.Data.ML.Login;

namespace RobotWasm.Server.Controllers.Login {
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase {
        LogInService _logInService;
        public LoginController(LogInService logInService) {
            _logInService = logInService;
        }

        [HttpPost("Login")]
        public LoginSet Login(LoginRequest input) {
        
            return LogInService.Login (input.UserName, input.Password, "", "");

        }


    }
}
