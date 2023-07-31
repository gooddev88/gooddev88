using Microsoft.AspNetCore.Mvc;
using Robot.Data.DA;
using Robot.Data.GADB.TT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Robot.Controllers.APP {
    [Route("api/[controller]")]
    [ApiController]
    public class TempFileController : ControllerBase {
        [HttpGet("GetTempFileByID")]
        public TempFilePrinter GetTempFileByID(int id) {
            return TempFileService.GetTempFIleByID(id);

        }
        [HttpPost("CreateTempFile")]
        public TempFilePrinter CreateTempFile([FromBody] TempFilePrinter data) {
            return TempFileService.CreateTempFile(data);

        }
    }
}
