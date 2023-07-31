using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Robot.Data.DA.Login;
using Robot.Data.ML;
using System;
using static Robot.Data.DA.Login.LogInService;

namespace Robot.Controllers.Login {
    [Route("api/[controller]")]
    [ApiController]
    public class AppsLoginController : ControllerBase {

        [HttpPost("Login")]
        public LoginSet Login(LoginRequest data) {
            LoginSet output = new LoginSet();
            try {
                output= LogInService.Login(data.Username, data.Password, data.AppsID, data.RComID);
            } catch (Exception ex) {
                output.LoginResult = "fail";
                output.LoginResultInfo =ex.Message;
            }
            return output;
        }
    }
}
