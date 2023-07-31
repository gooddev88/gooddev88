using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Robot.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UrlMapperController : ControllerBase
    {

        [AllowAnonymous]
        //[ActionName("FuckNoAuth")]
        [HttpGet("FuckNoAuth")]
        public string FuckNoAuth()
        {
            return "No Authorize";
        }


        [Authorize]
        [HttpGet("FuckAuth")]
        public string FuckAuth()
        {
            return "Authorize บักหำน้อย";
        }
    }
}
