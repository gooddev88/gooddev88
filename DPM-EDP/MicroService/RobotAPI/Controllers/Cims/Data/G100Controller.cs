using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RobotAPI.Data.DA.Data;
using RobotAPI.Data.ML.Data;
using RobotAPI.Services.Api;

namespace RobotAPI.Controllers.Cims.Data {
    [Route("api/[controller]")]
    [ApiController]
    public class G100Controller : ControllerBase {


        ClientService _clientService;
        Data100Service _data100Service;

        public G100Controller(ClientService clientService, Data100Service data100Service) {
            _clientService = clientService;
            _data100Service = data100Service;
        }

        [AllowAnonymous]
        [HttpGet("data101")]
        async public Task<ActionResult> data101(string? datefrom, string? dateto) {
            //สถิติเวลาการใช้ห้องประชุม
            Data101Set.DocSet output = new Data101Set.DocSet { message = "ok", rows = new List<Data101Set.DataRow>(), status = 1 };
            try {

                output.rows = await Task.Run(() => _data100Service.Data101(datefrom, dateto));
                Ok(output);

            } catch (Exception ex) {
                output.status = 0;
                output.message = ex.Message;
                BadRequest(output);
            }
            return Ok(output);
        }

        [AllowAnonymous]
        [HttpGet("data102")]
        async public Task<ActionResult> data102(string? datefrom, string? dateto) {
            Data102Set.DocSet output = new Data102Set.DocSet { message = "ok", rows = new List<Data102Set.DataRow>(), status = 1 };
            try {
                //5 อันดับห้องประชุมที่ถูกใช้งานมากที่สุด    
                output.rows = await Task.Run(() => _data100Service.Data102(datefrom, dateto));
                Ok(output);

            } catch (Exception ex) {
                output.status = 0;
                output.message = ex.Message;
                BadRequest(output);
            }
            return Ok(output);
        }
        [AllowAnonymous]
        [HttpGet("data103")]
        async public Task<ActionResult> data103(string? datefrom, string? dateto) {
            //สถิติจำนวนครั้งการใช้ห้องประชุม
            Data103Set.DocSet output = new Data103Set.DocSet { message = "ok", rows = new List<Data103Set.DataRow>(), status = 1 };
            try {

                output.rows = await Task.Run(() => _data100Service.Data103(datefrom,dateto));
                Ok(output);

            } catch (Exception ex) {
                output.status = 0;
                output.message = ex.Message;
                BadRequest(output);
            }
            return Ok(output);
        }
        [AllowAnonymous]
        [HttpGet("data104")]
        async public Task<ActionResult> data104(string? datefrom, string? dateto) {
            ////สถิติจำนวนครั้งการใช้ห้องประชุม
            Data104Set.DocSet output = new Data104Set.DocSet { message = "ok", rows = new List<Data104Set.DataRow>(), status = 1 };
            try {

                output.rows = await Task.Run(() => _data100Service.Data104(datefrom, dateto));
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
