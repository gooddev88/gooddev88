using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RobotWasm.Client;
using RobotWasm.Shared.Data.GaDB;
using RobotWasm.Shared.Data.ML.Param;

namespace RobotWasm.Server.Controllers.Config {
    [Route("api/[controller]")]
    [ApiController]
    public class ConfigController : ControllerBase {
        [HttpGet("GetConfig")]
        public ConfigParam GetConfig(string? type) {
            ConfigParam result= new ConfigParam();
            switch (type) {
                case "print":
                    result.Url = RobotWasm.Server.Data.Globals.ApiPrintMasterBaseUrl;
                    break;
                default:
                    break;
            }
            return result;
        }
    }
}
