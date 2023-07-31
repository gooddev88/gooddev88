using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace RobotAPI.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase {
        [HttpGet]
        [Route("GetData")]
        public IActionResult GetData() {
            
            return Ok("Fuck you");
        }
    }
}
