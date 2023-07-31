using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using RobotAPI.Data.DA.Cims;
using RobotAPI.Data.DA.Data;
using RobotAPI.Data.ML.Eoc;
using RobotAPI.Services.Api;

namespace RobotAPI.Controllers.Cims.Eoc {
    [Route("api/[controller]")]
    [ApiController]
    public class EocController : ControllerBase {
        ClientService _clientService;
        EOCService _eOCService;
        public EocController(ClientService clientService, EOCService eOCService) {
            _clientService = clientService;
            _eOCService = eOCService;
        }
        [AllowAnonymous]
        [HttpGet("WarningPublish")]
        async public Task<ActionResult> WarningPublish(string? search) {
            //ข้อมูลประกาศแจ้งเตือนภัย (Warning Publish)
            List<WarningPublish> output = new List<WarningPublish>();
            try {
                output = await Task.Run(() => _eOCService.WarningPublish(search));
            } catch (Exception ex) {
                return BadRequest(output);
            }
            return Ok(output);
        }
        [AllowAnonymous]
        [HttpGet("WarningEvent")]
        async public Task<ActionResult> WarningEvent(string? search = "") {
            //ข้อมูเหตุการณ์การแจ้งเตือนภัย (Warning Event)
            List<WarningEvent> output = new List<WarningEvent>();
            try {
                output = await Task.Run(() => _eOCService.WarningEvent(search));
            } catch (Exception ex) {
                return BadRequest(output);
            }
            return Ok(output);
        }

        [AllowAnonymous]
        [HttpGet("Expense")]
        async public Task<ActionResult> Expense(string? search = "") {
            //ข้อมูลค่าใช้จ่ายสาธารณูปโภคของหน่วยงาน (Expense)
            List<Expense> output = new List<Expense>();
            try {
                output = await Task.Run(() => _eOCService.Expense(search));
            } catch (Exception ex) {
                return BadRequest(output);
            }
            return Ok(output);
        }

        [AllowAnonymous]
        [HttpGet("Indicator")]
        async public Task<ActionResult> Indicator(string? search = "") {
            //ข้อมูลผลการดำเนินงานของหน่วยงานตามตัวชี้วัดการประเมินหน่วยงาน (Indicator)
            List<Indicator> output = new List<Indicator>();
            try {
                output = await Task.Run(() => _eOCService.Indicator(search)); 
            } catch (Exception ex) {
              return  BadRequest(output);
            }
            return Ok(output);
        }


        [AllowAnonymous]
        [HttpGet("IDP")]
        async public Task<ActionResult> IDP(string? search = "") {
            //ข้อมูลแผนพัฒนาบุคลากร รายบุคคล (IDP)
            List<IDP> output = new List<IDP>();
            try {
                output = await Task.Run(() => _eOCService.IDP(search));
            } catch (Exception ex) {
                return BadRequest(output);
            }
            return Ok(output);
        }
        [AllowAnonymous]
        [HttpGet("Help")]
        async public Task<ActionResult> Help(string? province = "") {
            //ข้อมูลการให้ความช่วยเหลือ/บรรเทาทุกข์ ระดับหมู่บ้าน
            List<Help> output = new List<Help>();
            try {
                output = await Task.Run(() => _eOCService.Help(province));
            } catch (Exception ex) {
                return BadRequest(output);
            }
            return Ok(output);
        }
        [AllowAnonymous]
        [HttpGet("Shelter")]
        async public Task<ActionResult> Shelter(string? province = "") {
            // ข้อมูลศูนย์พักพิง Shelter Information.
                        List<Shelter> output = new List<Shelter>();
            try {
                output = await Task.Run(() => _eOCService.Shelter(province));
            } catch (Exception ex) {
                return BadRequest(output);
            }
            return Ok(output);
        }
        [AllowAnonymous]
        [HttpGet("Hospital")]
        async public Task<ActionResult> Hospital(string? province = "") {
            //ข้อมูลสถานพยาบาล Hospital Info.
            List<Hospital> output = new List<Hospital>();
            try {
                output = await Task.Run(() => _eOCService.Hospital(province));
            } catch (Exception ex) {
                return BadRequest(output);
            }
            return Ok(output);
        }
        [AllowAnonymous]
        [HttpGet("Contact")]
        async public Task<ActionResult> Contact(string? province = "") {
            //ข้อมูลการติดต่อ Contact Information.
            List<Contact> output = new List<Contact>();
            try {
                output = await Task.Run(() => _eOCService.Contact(province));
            } catch (Exception ex) {
                return BadRequest(output);
            }
            return Ok(output);
        }
    }
}
