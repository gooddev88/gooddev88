using Microsoft.AspNetCore.Mvc;
using RobotWasm.Server.Data.DA.Master;
using RobotWasm.Shared.Data.GaDB;
using RobotWasm.Shared.Data.ML.Login;
using RobotWasm.Shared.Data.ML.Master.Company;
using RobotWasm.Shared.Data.ML.Param;
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

        [HttpGet("GetLocationInfoByLocID")]
        public LocationInfo GetLocationInfoByLocID(string? rcom,string? comId,string? locid) {
            rcom = rcom == null ? "" : rcom;
            comId = comId == null ? "" : comId;
            locid = locid == null ? "" : locid;
            return CompanyService.GetLocationInfoByLocID(rcom,comId,locid);
        }
        [HttpPost("ListLocationsByUser")]
        public List<LocationInfo> ListLocationsByUser([FromBody]  string data) {
            var doc = JsonSerializer.Deserialize<LocParam>(data, new JsonSerializerOptions {
                PropertyNameCaseInsensitive = true,
                ReferenceHandler = ReferenceHandler.Preserve
            });
            return CompanyService.ListLocationsByUser(doc.RCom, doc.Com, doc.User);
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
        

        [HttpPost("ListCompanyInfoUIC")]
        public List<CompanyInfoList> ListCompanyInfoUIC([FromBody] string? data, string? type, bool addShowAll) {
            type = type == null ? "" : type;
            var doc = JsonSerializer.Deserialize<LoginSet>(data, new JsonSerializerOptions {
                PropertyNameCaseInsensitive = true,
                ReferenceHandler = ReferenceHandler.Preserve
            });
            var r = CompanyService.ListCompanyInfoUIC(doc,type,addShowAll);
            return r;
        }




        [HttpGet("GetCompanyInfo")]
        public CompanyInfo GetCompanyInfo(string? rcom, string? comId) {
            rcom = rcom == null ? "" : rcom;
            comId = comId == null ? "" : comId;
            return CompanyService.GetCompanyInfo(rcom, comId);
        }

    }
}
