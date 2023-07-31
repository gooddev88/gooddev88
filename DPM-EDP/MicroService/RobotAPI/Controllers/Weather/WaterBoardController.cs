using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RobotAPI.Data.DA.Weather.Waterboard;
using RobotAPI.Data.ML.Portal;
using RobotAPI.Data.ML.Weather.WaterBoard;
using RobotAPI.Models.Shared.Jwt;

namespace RobotAPI.Controllers.Weather {
    [Route("api/weather/[controller]")]
    [ApiController]
    public class WaterBoardController : ControllerBase {
        WaterService _waterService;
        public WaterBoardController(WaterService waterService) {
            _waterService = waterService;
        }

        [AllowAnonymous]
        [HttpGet("GetBasicInfo")]
        async public Task<BasicInfo> GetBasicInfo() {
            return await Task.Run(() => _waterService.GetBasicInfo());
        }

        [AllowAnonymous]
        [HttpGet("GetDamInfo")]
        public DamInfo GetDamInfo() {
            return WaterService.GetDamInfo();
        }

        [AllowAnonymous]
        [HttpGet("GetRainFallInfo")]
        async public Task<RainFallInfo> GetRainFallInfo() {
            return await Task.Run(() => _waterService.GetRainFallInfo());
        }
        [AllowAnonymous]
        [HttpGet("GetPMInfo")]
        async public Task<PMInfo> GetPMInfo() {
            return await Task.Run(() => _waterService.GetPMInfo());
        }

        [AllowAnonymous]
        [HttpGet("GetStormInfo")]
        public StormInfo GetStormInfo() {
            return WaterService.GetStormInfo();
        }

        [AllowAnonymous]
        [HttpGet("GetRainFallX")]
        async public Task<List<DPM020DataSet.Water>> GetRainFallX() {
            return await Task.Run(() => _waterService.GetRainFallInfo_Extend());
        }
        [AllowAnonymous]
        [HttpGet("GetDam")]
        async public Task<List<DPM020DataSet.Dam>> GetDam() {
            return await Task.Run(() => _waterService.GetWater_Dam());
        }

        [AllowAnonymous]
        [HttpGet("DisasterSummary")]
        async public Task<DiasterSummary?> DisasterSummary(string? province) {
            //get all disaster summary
            return await Task.Run(() => _waterService.DisasterSummary(province));
        }
        [AllowAnonymous]
        [HttpGet("DisasterSummaryV2")]
        async public Task<DiasterSummary?> DisasterSummaryV2(string? province) {
            //get all disaster summary
            return await Task.Run(() => _waterService.DisasterSummary2(province));
        }

    }
}
