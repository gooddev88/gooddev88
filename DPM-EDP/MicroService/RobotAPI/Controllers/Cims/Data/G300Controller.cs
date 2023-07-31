using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RobotAPI.Data.DA.Data;
using RobotAPI.Data.ML.Data;
using RobotAPI.Services.Api;

namespace RobotAPI.Controllers.Cims.Data {
    [Route("api/[controller]")]
    [ApiController]
    public class G300Controller : ControllerBase {


        ClientService _clientService;
        Data300Service _data300Service;

        public G300Controller(ClientService clientService, Data300Service data300Service) {
            _clientService = clientService;
            _data300Service = data300Service;
        }
        [AllowAnonymous]
        [HttpGet("data301")]
        async public Task<ActionResult> data301(string? datefrom, string? dateto)   {
            //อันดับหมวดหมู่ข่าวเตือนภัย ปภ. Today
            Data301Set.DocSet output = new Data301Set.DocSet { message = "ok", rows = new List<Data301Set.DataRow>(), status = 1 };
            try { 
                output = await Task.Run(() => _data300Service.Data301(datefrom,dateto));
                Ok(output);

            } catch (Exception ex) {
                output.status = 0;
                output.message = ex.Message;
                BadRequest(output);
            }
            return Ok(output);
        }

        [AllowAnonymous]
        [HttpGet("data302")]
        async public Task<ActionResult> data302(string? datefrom, string? dateto) {
            //5 อันดับข่าว ปภ. Today
            Data302Set.DocSet output = new Data302Set.DocSet { message = "ok", rows = new List<Data302Set.DataRow>(), status = 1 };
            try {
                output = await Task.Run(() => _data300Service.Data302(datefrom, dateto));
                Ok(output);

            } catch (Exception ex) {
                output.status = 0;
                output.message = ex.Message;
                BadRequest(output);
            }
            return Ok(output);
        }


        [AllowAnonymous]
        [HttpGet("data303")]
        async public Task<ActionResult> data303(string? datefrom, string? dateto) {
            //จำนวนผู้เข้าชมตามช่วงเวลา
            Data303Set.DocSet output = new Data303Set.DocSet { message = "ok", rows = new List<Data303Set.DataRow>(), status = 1 };
            try {
                output = await Task.Run(() => _data300Service.Data303(datefrom, dateto));
                Ok(output);

            } catch (Exception ex) {
                output.status = 0;
                output.message = ex.Message;
                BadRequest(output);
            }
            return Ok(output);
        }


        [AllowAnonymous]
        [HttpGet("data304")]
        async public Task<ActionResult> data304(string? datefrom, string? dateto) {
            //จำนวนผู้เข้าชมประจำวัน
            Data304Set.DocSet output = new Data304Set.DocSet { message = "ok", rows = new List<Data304Set.DataRow>(), status = 1 };
            try {
                output = await Task.Run(() => _data300Service.Data304(datefrom, dateto));
                Ok(output); 
            } catch (Exception ex) {
                output.status = 0;
                output.message = ex.Message;
                BadRequest(output);
            }
            return Ok(output);
        }


        [AllowAnonymous]
        [HttpGet("data305")]
        async public Task<ActionResult> data305(string? datefrom, string? dateto) {
            //5 อันดับบราวเซอร์ที่ใช้งาน
            Data305Set.DocSet output = new Data305Set.DocSet { message = "ok", rows = new List<Data305Set.DataRow>(), status = 1 };
            try {
                output = await Task.Run(() => _data300Service.Data305(datefrom, dateto));
                Ok(output);

            } catch (Exception ex) {
                output.status = 0;
                output.message = ex.Message;
                BadRequest(output);
            }
            return Ok(output);
        }

         [AllowAnonymous]
        [HttpGet("data306")]
        async public Task<ActionResult> data306( ) {
            //ข่าวล่าสุด 100 ลำดับแรก
            Data306Set.DocSet output = new Data306Set.DocSet { message = "ok", rows = new List<Data306Set.DataRow>(), status = 1 };
            try {
                output = await Task.Run(() => _data300Service.Data306( ));
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
