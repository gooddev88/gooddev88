using Microsoft.AspNetCore.Mvc;
using RobotWasm.Server.Data.DA.Master;
using RobotWasm.Shared.Data.GaDB;
using RobotWasm.Shared.Data.ML.Master.Company;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace RobotWasm.Server.Controllers.AssetItem {
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase {


        [HttpGet("GetComInfoByComID")]
        public CompanyInfo GetComInfoByComID(string? rcom,string? comId)
        {
            rcom = rcom == null ? "" : rcom;
            comId = comId == null ? "" : comId;
            return CompanyService.GetComInfoByComID(rcom, comId);
        }

        [HttpGet("GetComInfoByRComID")]
        public CompanyInfo GetComInfoByRComID(string? comId) {
            comId = comId == null ? "" : comId;
            return CompanyService.GetComInfoByRComID(comId);
        }

        [HttpGet("ListCompanyInfo")]
        public List<CompanyInfoList> ListCompanyInfo(string type, bool addShowAll) {
            return CompanyService.ListCompanyInfo(type,addShowAll);
        }


        [HttpPost("ListCompanyInfoByComID")]
        public List<CompanyInfoList> ListCompanyInfoByComID([FromBody] string? data) {
  
            var param = JsonSerializer.Deserialize<List<string>>(data, new JsonSerializerOptions {
                PropertyNameCaseInsensitive = true,
                ReferenceHandler = ReferenceHandler.Preserve
            });
            var r = CompanyService.ListCompanyInfoByComID(param);
            return r;
        }

    }
}
