using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RobotAPI.Data.CimsDB.TT;
using RobotAPI.Data.DA.Front;

namespace RobotAPI.Controllers.Front {
    [Route("api/[controller]")]
    [ApiController]
    public class ApiStoreController : ControllerBase {
        [AllowAnonymous]
        [HttpGet("ListAllApi")]
        async public Task<IActionResult> ListAllApi( ) {
            List<api_store> output = new List<api_store>();
            try {
                output = ApiStoreService.ListAllApi();
            } catch (Exception) {
                BadRequest(output);
            } 
            return Ok(output);
        }
    }
}
