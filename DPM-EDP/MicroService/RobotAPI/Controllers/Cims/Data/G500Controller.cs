using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RobotAPI.Data.CimsDB.TT;
using RobotAPI.Data.DA.Data;
using RobotAPI.Data.ML.Data;
using RobotAPI.Services.Api;

namespace RobotAPI.Controllers.Cims.Data {
    [Route("api/[controller]")]
    [ApiController]
    public class G500Controller : ControllerBase {


        ClientService _clientService;
        Data500Service _data500Service;

        public G500Controller(ClientService clientService, Data500Service data500Service) {
            _clientService = clientService;
            _data500Service = data500Service;
        }
        //[AllowAnonymous]
        //[HttpPost("data507")]
        //async public Task<ActionResult> data507([FromBody] Data507Set.Param input) {
        //    Data507Set.DocSet output = new Data507Set.DocSet { message = "ok", rows = new List<Data507Set.DataRow>(), status = 1 };
        //    try {
        //        input.search = input.search == null ? "" : input.search.ToLower();
        //        input.province = input.province == null ? "" : input.province.ToLower();

        //        //รายงาน 7 ภัยตามภัยและวันที่เลือก
        //        output.rows = await Task.Run(() => _data500Service.Data507(input));
        //        Ok(output);

        //    } catch (Exception ex) {
        //        output.status = 0;
        //        output.message = ex.Message;
        //        BadRequest(output);
        //    }
        //    return Ok(output);
        //}

        [AllowAnonymous]
        [HttpGet("data501")]
        async public Task<ActionResult> data501(string? datefrom, string? dateto) {
            Data501Set.DocSet output = new Data501Set.DocSet();
            try {
                //จุดแผ่นดินไหว / etl.dss_earthquake
                output = await Task.Run(() => _data500Service.Data501(datefrom, dateto));
                Ok(output);

            } catch (Exception ex) {
                output.status = 0;
                output.message = ex.Message;
                BadRequest(output);
            }
            return Ok(output);

        }
        [AllowAnonymous]
        [HttpGet("data502")]
        async public Task<ActionResult> data502(string? datefrom, string? dateto) {
            Data502Set.DocSet output = new Data502Set.DocSet();
            try {
                //น้ำเขื่อน / etl.dss_rid_dam
                output = await Task.Run(() => _data500Service.Data502(datefrom, dateto));
                Ok(output);

            } catch (Exception ex) {
                output.status = 0;
                output.message = ex.Message;
                BadRequest(output);
            }
            return Ok(output);
        }
        [AllowAnonymous]
        [HttpGet("data503")]
        async public Task<ActionResult> data503(string? datefrom, string? dateto) {
            Data503Set.DocSet output = new Data503Set.DocSet();
            try {
                //ลำน้ำสำคัญ / etl.dss_water
                output = await Task.Run(() => _data500Service.Data503(datefrom, dateto));
                Ok(output);
            } catch (Exception ex) {
                output.status = 0;
                output.message = ex.Message;
                BadRequest(output);
            }
            return Ok(output);
        }

        [AllowAnonymous]
        [HttpGet("data504")]
        async public Task<ActionResult> data504(string? datefrom, string? dateto) {
            //ปริมาณน้ำฝน  / etl.dss_water
            Data504Set.DocSet output = new Data504Set.DocSet();
            try {
                output = await Task.Run(() => _data500Service.Data504(datefrom, dateto));
                Ok(output);
            } catch (Exception ex) {
                output.status = 0;
                output.message = ex.Message;
                BadRequest(output);
            }
            return Ok(output);
        }
        [AllowAnonymous]
        [HttpGet("data505")]
        async public Task<ActionResult> data505(string? datefrom, string? dateto) {
            ////คุณภาพอากาศ / etl.dss_weather_quality
            Data505Set.DocSet output = new Data505Set.DocSet();
            try {
                output = await Task.Run(() => _data500Service.Data505(datefrom, dateto));
                Ok(output);
            } catch (Exception ex) {
                output.status = 0;
                output.message = ex.Message;
                BadRequest(output);
            }
            return Ok(output);
        }
        [AllowAnonymous]
        [HttpGet("data506")]
        async public Task<ActionResult> data506(string? datefrom, string? dateto) {
            // เสี่ยงแล้ง / etl.dpm_drought_risk
            Data506Set.DocSet output = new Data506Set.DocSet();
            try {
                output = await Task.Run(() => _data500Service.Data506(datefrom, dateto));
                Ok(output);
            } catch (Exception ex) {
                output.status = 0;
                output.message = ex.Message;
                BadRequest(output);
            }
            return Ok(output);
        }
        [AllowAnonymous]
        [HttpPost("data507")]
       
        async public Task<ActionResult> data507([FromBody] Data507Set.Param param) {
            //รายงาน 7 ภัย
            Data507Set.DocSet output = new Data507Set.DocSet();
            try {
                output = await Task.Run(() => _data500Service.Data507(param));
               
                Ok(output);
            } catch (Exception ex) {
                output.status = 0;
                output.message = ex.Message;
                BadRequest(output);
            }
            return Ok(output);
        }
        [AllowAnonymous]
        [HttpGet("data508")]
        async public Task<ActionResult> data508(string? datefrom, string? dateto) {
            //เสี่ยงอุทกภัย  / etl.dpm_flood_risk
            Data508Set.DocSet output = new Data508Set.DocSet();
            try {
                output = await Task.Run(() => _data500Service.Data508(datefrom, dateto));
                Ok(output);
            } catch (Exception ex) {
                output.status = 0;
                output.message = ex.Message;
                BadRequest(output);
            }
            return Ok(output);
        }
        [AllowAnonymous]
        [HttpGet("data509")]
        async public Task<ActionResult> data509(string? datefrom, string? dateto,string? province) {
            //อุบัติเหตุ  / acd_accident_person 
            Data509Set.DocSet output = new Data509Set.DocSet();
            try {
                output = await Task.Run(() => _data500Service.Data509(datefrom, dateto, province));
                Ok(output);
            } catch (Exception ex) {
                output.status = 0;
                output.message = ex.Message;
                BadRequest(output);
            }
            return Ok(output);
        }
        [AllowAnonymous]
        [HttpGet("data510")]
        async public Task<ActionResult> data510(string? pname, string? aname ) {
            //get pm2.5 ล่าสุดตามจังหวัด
            view_weather_quality? output = new view_weather_quality();
            try {
                output = await Task.Run(() => _data500Service.Data510(pname, aname));
                Ok(output);
            } catch (Exception ex) { 
                BadRequest(output);
            }
            return Ok(output);
        }

     [AllowAnonymous]
        [HttpGet("Data511")]
        async public Task<ActionResult> Data511(double? lat, double? lon) {
            //get pm2.5 ล่าสุดตาม geolcation
            view_weather_quality? output = new view_weather_quality();
            try {
                output = await Task.Run(() => _data500Service.Data511(lat, lon));
                Ok(output);
            } catch (Exception ex) { 
                BadRequest(output);
            }
            return Ok(output);
        }

        [AllowAnonymous]
        [HttpGet("data550")]
        async public Task<ActionResult> data550(string? search, string? province) {
            //จำนวนสิ่งของสำรองจ่าย แบ่งตามจังหวัด 
            Data550Set.DocSet output = new Data550Set.DocSet();
            try {
                output = await Task.Run(() => _data500Service.Data550(search, province));
                Ok(output);
            } catch (Exception ex) {
                output.status = 0;
                output.message = ex.Message;
                BadRequest(output);
            }
            return Ok(output);
        }

        [AllowAnonymous]
        [HttpGet("data551")]
        async public Task<ActionResult> data551(string? search, string? sectioncode) {
            //จำนวนสิ่งของสำรองจ่าย แบ่งตามศูนย์
            Data551Set.DocSet output = new Data551Set.DocSet();
            try {
                output = await Task.Run(() => _data500Service.Data551(search, sectioncode));
                Ok(output);
            } catch (Exception ex) {
                output.status = 0;
                output.message = ex.Message;
                BadRequest(output);
            }
            return Ok(output);
        }

        [AllowAnonymous]
        [HttpGet("data561")]
        async public Task<ActionResult> data561( string? search, string? province) {
            //อาสาสมัคร
            Data561Set.DocSet output = new Data561Set.DocSet();
            try {
                output = await Task.Run(() => _data500Service.Data561(search, province));
                Ok(output);
            } catch (Exception ex) {
                output.status = 0;
                output.message = ex.Message;
                BadRequest(output);
            }
            return Ok(output);
        }

        [AllowAnonymous]
        [HttpGet("data570")]
        async public Task<ActionResult> data570(string? search, string? province) {
            //สถานะเครื่องจักรกลสาธารณภัย แบ่งตามจังหวัด
            Data570Set.DocSet output = new Data570Set.DocSet();
            try {
                output = await Task.Run(() => _data500Service.Data570(search, province));
                Ok(output);
            } catch (Exception ex) {
                output.status = 0;
                output.message = ex.Message;
                BadRequest(output);
            }
            return Ok(output);
        }

        [AllowAnonymous]
        [HttpGet("data571")]
        async public Task<ActionResult> data571(string? search, string? sectioncode) {
            //สถานะเครื่องจักรกลสาธารณภัย แบ่งตามศูนย์
            Data571Set.DocSet output = new Data571Set.DocSet();
            try {
                output = await Task.Run(() => _data500Service.Data571(search, sectioncode));
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
