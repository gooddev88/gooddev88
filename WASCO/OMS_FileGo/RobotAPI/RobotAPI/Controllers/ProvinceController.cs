using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RobotAPI.Data.DA.Accident;
using RobotAPI.Models.Accident;

namespace RobotAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProvinceController : ControllerBase
    {
        [AllowAnonymous]
        [HttpGet("GetProvinceAll")]
        public List<string> GetProvinceAll()
        {
            List<string> result = new List<string>();
            try
            {
                result = AccidentDashboardService.GetProvince().ToList();
            }
            catch
            {
            }

            return result;
        }
    }
}
