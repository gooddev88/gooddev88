using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Robot.Data.DA;
using Robot.Data.DA.API.APP;
using Robot.Data.DA.LoginCrossApp;
using Robot.Data.GADB.TT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Robot.Data.ML.I_Result;
using static Robot.Data.DA.API.APP.AppLogInService;

namespace Robot.Controllers.APP
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogInController : ControllerBase
    {

        [HttpPost("LogInMobile")]
        public ILogInResult LogInMobile([FromBody] RequestLogin data)
        {
            return AppLogInService.Login(data);
        }

        [HttpPost("LogInCrossApp")]
        public LogInCrossAppReq LogInCrossApp([FromBody] RequestLogin data)
        {
            return LogInCrossAppService.CreateReqInfo(data.AppID, data.RComID, data.Username,data.ForwardUrl);
        }

        [HttpPost("RegisterMac")]
        public I_BasicResult RegisterMac([FromBody] MacRegister data)
        {
            return AppLogInService.RegisterMac(data);
        }

        [HttpGet("RemoveRegisterMac")]
        public I_BasicResult RemoveRegisterMac(string rcom, string com, string deviceId)
        {
            return AppLogInService.RemoveRegisterMac(rcom, com, deviceId);
        }

        [HttpGet("ListMacUseable")]
        public List<string> ListMacUseable(string rcom, string com)
        {
            return AppLogInService.ListMacUseable(rcom, com);

        }
    }
}
