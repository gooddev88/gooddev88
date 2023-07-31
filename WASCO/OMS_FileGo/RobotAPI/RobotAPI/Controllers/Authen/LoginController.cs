using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RobotAPI.Data.DA.Login;
using RobotAPI.Models;
using RobotAPI.Models.Shared.Jwt;
using RobotAPI.Services.Jwt;

namespace RobotAPI.Controllers {
    
    [Route("api/authen/[controller]")]
    [ApiController]
    public class LoginController : BaseApiController {
        private readonly IJwtUserService userService; 
        public LoginController(IJwtUserService userService ) {
            this.userService = userService; 
        }


        [AllowAnonymous]
        [HttpPost]
        [Route("jwtApilogin")]
        public async Task<IActionResult> jwtApilogin(LoginAPIRequest login) {

            var loginResponse = await userService.LoginApiAsync(login);
            if (!loginResponse.Success) {
                return Unauthorized(new { 
                    loginResponse.ErrorCode,
                    loginResponse.Error
                }); 
            }
            return Ok(loginResponse);
        }




        [AllowAnonymous]
        [HttpPost]
        [Route("jwtApplogin")]
        public async Task<IActionResult> jwtApplogin(LoginAPIRequest login) {

            var loginResponse = await userService.LoginAppAsync(login);
            if (!loginResponse.Success) {
                return Unauthorized(new {
                    loginResponse.ErrorCode,
                    loginResponse.Error
                });
            }
            return Ok(loginResponse);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        [Route("appLoginWithPermission")]
        public async Task<IActionResult> appLoginWithPermission(LoginAPIRequest login) {
            var loginInfo = await LogInService.Login(login.UserName, login.Password, "");
            return Ok(loginInfo);
        }

        [Authorize]
        [HttpGet]
        [Route("info")]
        public   IActionResult Info() {
            var userResponse = "Test Value";
            return Ok(userResponse);

        }
    }
}
