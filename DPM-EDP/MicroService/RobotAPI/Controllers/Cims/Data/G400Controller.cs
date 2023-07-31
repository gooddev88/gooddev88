using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RobotAPI.Data.DA.Data;
using RobotAPI.Data.ML.Data;
using RobotAPI.Services.Api;

namespace RobotAPI.Controllers.Cims.Data {
    [Route("api/[controller]")]
    [ApiController]
    public class G400Controller : ControllerBase {


        ClientService _clientService;
        Data400Service _data400Service;

        public G400Controller(ClientService clientService, Data400Service data400Service) {
            _clientService = clientService;
            _data400Service = data400Service;
        }
        [AllowAnonymous]
        [HttpGet("data401")]
        async public Task<ActionResult> data401(string? department, string? supdepartment, int? year) 
            {
            //สถิติการลา
            Data401Set.DocSet output = new Data401Set.DocSet { message = "ok", rows = new List<Data401Set.DataRow>(), status = 1 };
            try { 
                output = await Task.Run(() => _data400Service.Data401(department,supdepartment,year));
                Ok(output);

            } catch (Exception ex) {
                output.status = 0;
                output.message = ex.Message;
                BadRequest(output);
            }
            return Ok(output);
        }
        [AllowAnonymous]
        [HttpGet("data402")]
        async public Task<ActionResult> data402(int? year, int? month) {
            //สถิติวันลาของหน่วยงาน
            Data402Set.DocSet output = new Data402Set.DocSet { message = "ok", rows = new List<Data402Set.DataRow>(), status = 1 };
            try {
                output = await Task.Run(() => _data400Service.Data402(year,month));
                Ok(output);

            } catch (Exception ex) {
                output.status = 0;
                output.message = ex.Message;
                BadRequest(output);
            }
            return Ok(output);
        }

        [AllowAnonymous]
        [HttpGet("data403")]
        async public Task<ActionResult> data403(int? year, int? month) {
            //สถิติวันลาของส่วนงาน
            Data403Set.DocSet output = new Data403Set.DocSet { message = "ok", rows = new List<Data403Set.DataRow>(), status = 1 };
            try {
                output = await Task.Run(() => _data400Service.Data403(year, month));
                Ok(output);

            } catch (Exception ex) {
                output.status = 0;
                output.message = ex.Message;
                BadRequest(output);
            }
            return Ok(output);
        }

        [AllowAnonymous]
        [HttpGet("data404")]
        async public Task<ActionResult> data404(int? year, string? leavetype) {
            //สถิติการลาประจำวัน (ประเภทวันลา)
            Data404Set.DocSet output = new Data404Set.DocSet { message = "ok", rows = new List<Data404Set.DataRow>(), status = 1 };
            try {
                output = await Task.Run(() => _data400Service.Data404(year, leavetype));
                Ok(output);

            } catch (Exception ex) {
                output.status = 0;
                output.message = ex.Message;
                BadRequest(output);
            }
            return Ok(output);
        }

        [AllowAnonymous]
        [HttpGet("data405")]
        async public Task<ActionResult> data405(int? year, string? leavetype) {
            //สถิติการลาแต่ละประเภท (ประเภทพนักงาน)
            Data405Set.DocSet output = new Data405Set.DocSet { message = "ok", rows = new List<Data405Set.DataRow>(), status = 1 };
            try {
                output = await Task.Run(() => _data400Service.Data405(year, leavetype));
                Ok(output);

            } catch (Exception ex) {
                output.status = 0;
                output.message = ex.Message;
                BadRequest(output);
            }
            return Ok(output);
        }
        [AllowAnonymous]
        [HttpGet("data406")]
        async public Task<ActionResult> data406(int? year, string? leavetype) {
            //สถิติการลาแบ่งตามเพศ
            Data406Set.DocSet output = new Data406Set.DocSet { message = "ok", rows = new List<Data406Set.DataRow>(), status = 1 };
            try {
                output = await Task.Run(() => _data400Service.Data406(year, leavetype));
                Ok(output);

            } catch (Exception ex) {
                output.status = 0;
                output.message = ex.Message;
                BadRequest(output);
            }
            return Ok(output);
        }
    }
}
