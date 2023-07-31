using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RobotAPI.Data.DA.Accident;
using RobotAPI.Data.DA.Cims.Master;

namespace RobotAPI.Controllers {
    [Route("api/cims/[controller]")]
    [ApiController]
    public class MasterController : ControllerBase {
        [AllowAnonymous]
        [HttpGet("GetProvince")]
        public List<string> GetProvince(string? search = "") {
            List<string> result = new List<string>();
            try {
                result = MasterService.GetProvince(search).ToList();
            } catch {
            }

            return result;
        }
    }
}
