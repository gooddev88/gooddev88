using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RobotAPI.Data.DA.User;
using RobotAPI.Data.MainDB.TT;
using System.Security.Claims;

namespace RobotAPI.Controllers.User
{
    [Route("api/user/[controller]")]
    [ApiController]
    public class UserServiceController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public UserServiceController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [Route("ListUser")]
        public IActionResult ListUser(int skip, int take, string sort, string fillter)
        {
            string constr = _configuration["MainContext:ConnectionString"];
            var data = UserService.GetUserPaging(constr ,skip, take, sort, fillter);
            return Ok(data);
        }
    }
}
