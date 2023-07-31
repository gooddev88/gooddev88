using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RobotAPI.Data.DA.Cims;
using RobotAPI.Data.DA.Data;
using RobotAPI.Data.ML.Data;
using RobotAPI.Services.Api;

namespace RobotAPI.Controllers.Cims.Data {
    [Route("api/[controller]")]
    [ApiController]
    public class G200Controller : ControllerBase {


        ClientService _clientService;
        Data200Service _data200Service;

        public G200Controller(ClientService clientService, Data200Service data200Service) {
            _clientService = clientService;
            _data200Service = data200Service;
        }
        [AllowAnonymous]
        [HttpGet("data201")]
        async public Task<ActionResult> data201(string? datefrom, string? dateto) {
            Data201Set.DocSet output = new Data201Set.DocSet { message = "ok", rows = new List<Data201Set.DataRow>(), status = 1 };
            try {

                output = await Task.Run(() => _data200Service.Data201(datefrom,dateto));
                Ok(output);

            } catch (Exception ex) {
                output.status = 0;
                output.message = ex.Message;
                BadRequest(output);
            }
            return Ok(output);
        }

        [AllowAnonymous]
        [HttpGet("data202")]
        async public Task<ActionResult> data202(string? datefrom, string? dateto) {
            Data202Set.DocSet output = new Data202Set.DocSet { message = "ok", rows = new List<Data202Set.DataRow>(), status = 1 };
            try {

                output = await Task.Run(() => _data200Service.Data202(datefrom, dateto));
                Ok(output);

            } catch (Exception ex) {
                output.status = 0;
                output.message = ex.Message;
                BadRequest(output);
            }
            return Ok(output);
        }
        [AllowAnonymous]
        [HttpGet("data203")]
        async public Task<ActionResult> data203( ) {
            Data203Set.DocSet output = new Data203Set.DocSet { message = "ok", rows = new List<Data203Set.DataRow>(), status = 1 };
            try {

                output = await Task.Run(() => _data200Service.Data203( ));
                Ok(output);

            } catch (Exception ex) {
                output.status = 0;
                output.message = ex.Message;
                BadRequest(output);
            }
            return Ok(output);
        }
        [AllowAnonymous]
        [HttpGet("data204")]
        async public Task<ActionResult> data204() {
            Data204Set.DocSet output = new Data204Set.DocSet { message = "ok", rows = new List<Data204Set.DataRow>(), status = 1 };
            try {

                output = await Task.Run(() => _data200Service.Data204());
                Ok(output);

            } catch (Exception ex) {
                output.status = 0;
                output.message = ex.Message;
                BadRequest(output);
            }
            return Ok(output);
        }
        [AllowAnonymous]
        [HttpGet("data205")]
        async public Task<ActionResult> data205() {
            Data205Set.DocSet output = new Data205Set.DocSet { message = "ok", rows = new List<Data205Set.DataRow>(), status = 1 };
            try {

                output = await Task.Run(() => _data200Service.Data205());
                Ok(output);

            } catch (Exception ex) {
                output.status = 0;
                output.message = ex.Message;
                BadRequest(output);
            }
            return Ok(output);
        }
        [AllowAnonymous]
        [HttpGet("data206")]
        async public Task<ActionResult> data206(string lat, string lon) {
            //หา ตำบล อำเภอ จังหวัดจาก gps
            Data206Set.DocSet output = new Data206Set.DocSet { message = "ok", rows = new List<Data206Set.DataRow>(), status = 1 };
            try { 
                output = await Task.Run(() => _data200Service.Data206(lat,lon));
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
