using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RobotWasm.Server.Data.DA.Master;
using RobotWasm.Shared.Data.DimsDB;
using RobotWasm.Shared.Data.GaDB;
using RobotWasm.Shared.Data.ML.Master.ApiCate;
using System.Text.Json;
using System.Text.Json.Serialization;
using static RobotWasm.Shared.Data.ML.Shared.I_Result;

namespace RobotWasm.Server.Controllers.Master {
    [Route("api/[controller]")]
    [ApiController]
    public class ApiCateController : ControllerBase {


        [HttpGet("GetDocSet")]
        public I_ApiCateSet GetDocSet(string docid) {
            return ApiCateService.GetDocSet(docid);
        }

        [HttpPost("SaveApiCate")]
        public I_BasicResult SaveApiCate([FromBody] string data, bool action) {

            var head = JsonSerializer.Deserialize<I_ApiCateSet>(data, new JsonSerializerOptions {
                PropertyNameCaseInsensitive = true,
                ReferenceHandler = ReferenceHandler.Preserve
            });
            I_BasicResult r = ApiCateService.Save(head);
            return r;
        }

        [HttpPost("GetLatestFiles")]
        public I_ApiCateSet GetLatestFiles([FromBody] I_ApiCateSet data) {
            I_ApiCateSet r = ApiCateService.GetLatestFiles(data);
            return r;
        }

        [HttpPost("ReOrder")]
        public I_BasicResult ReOrder([FromBody] string data)
        {
            var doc = JsonSerializer.Deserialize<List<api_cate>>(data, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                ReferenceHandler = ReferenceHandler.Preserve
            });
            I_BasicResult r = ApiCateService.ReOrder(doc);
            return r;
        }

        [HttpGet("DeleteDoc")]
        public I_BasicResult DeleteDoc(string docid) {
            return ApiCateService.DeleteDoc(docid);
        }

        [HttpGet("GenSort")]
        public short GenSort() {
            return ApiCateService.GenSort();
        }

        [HttpGet("ListDoc")]
        public List<vw_api_cate> ListDoc(string? search) {
            search = search == null ? "" : search;
            return ApiCateService.ListDoc(search);
        }

        [HttpGet("ListApiCate")]
        public List<api_cate> ListApiCate()
        {
            return ApiCateService.ListApiCate();
        }

    }
}
