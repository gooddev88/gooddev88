using Microsoft.AspNetCore.Mvc;
using RobotWasm.Server.Service.FileGo;
using RobotWasm.Shared.Data.ML.FileGo;
using static RobotWasm.Shared.Data.ML.Shared.I_Result;

namespace RobotWasm.Server.Controllers.FileGo {
    [Route("api/[controller]")]
    [ApiController]
    public class FileGoController : ControllerBase {
        private readonly FileGoService _filego;
        public FileGoController(FileGoService filego) {
            _filego = filego;   
        }
        
        [HttpGet("LoginApiFileGo")]
    async     public   Task<I_BasicResult>  LoginApiFileGo() { 
           return  await _filego.LoginApiFileGo(); 
        }

      
        

    }
}
