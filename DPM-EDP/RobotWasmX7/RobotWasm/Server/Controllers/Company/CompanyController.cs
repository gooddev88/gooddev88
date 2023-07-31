using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RobotWasm.Server.Data.DA.Master;
using RobotWasm.Shared.Data.DimsDB;
using RobotWasm.Shared.Data.GaDB;
using RobotWasm.Shared.Data.ML.Master.Company;
using System.Text.Json;
using System.Text.Json.Serialization;
using static RobotWasm.Shared.Data.ML.Shared.I_Result;

namespace RobotWasm.Server.Controllers.Company
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase {


        [HttpGet("GetDocSet")]
        public I_CompanySet GetDocSet(string docid) {
            return CompanyService.GetDocSet(docid);
        }

        [HttpPost("SaveCompany")]
        public I_BasicResult SaveCompany([FromBody] string data, bool action) {
            var head = JsonSerializer.Deserialize<I_CompanySet>(data, new JsonSerializerOptions {
                PropertyNameCaseInsensitive = true,
                ReferenceHandler = ReferenceHandler.Preserve
            });
            I_BasicResult r = CompanyService.Save(head);
            return r;
        }

        [HttpGet("DeleteDoc")]
        public I_BasicResult DeleteDoc(string docid,string modifyby) {
            return CompanyService.DeleteDoc(docid, modifyby);
        }

        [HttpGet("Checkduplicate")]
        public company_info Checkduplicate(string comid)
        {
            return CompanyService.Checkduplicate(comid);
        }

        [HttpGet("ListDoc")]
        public List<company_info> ListDoc(string? search) {
            search = search == null ? "" : search;
            return CompanyService.ListDoc(search);
        }

        [HttpGet("ListProvince")]
        public List<mas_province> ListProvince()
        {
            return CompanyService.ListProvince();
        }

        [HttpGet("ListGroupCompany")]
        public List<company_group_info> ListGroupCompany()
        {
            return CompanyService.ListGroupCompany();
        }

        [HttpGet("ListCompany")]
        public List<company_info> ListCompany()
        {
            return CompanyService.ListCompany();
        }

    }
}
