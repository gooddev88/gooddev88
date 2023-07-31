using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RobotWasm.Server.Data.DA.Board;
using RobotWasm.Shared.Data.DimsDB;
using RobotWasm.Shared.Data.ML.ApiMaster;
using RobotWasm.Shared.Data.ML.DPMBaord.BoardData;
using System.Text.Json;
using System.Text.Json.Serialization;
using static RobotWasm.Server.Data.DA.Board.ApiMasterService;
using static RobotWasm.Shared.Data.ML.Shared.I_Result;

namespace RobotWasm.Server.Controllers.ApiMaster {
    [Route("api/[controller]")]
    [ApiController]
    public class ApiMasterController : ControllerBase {
        ApiMasterService _apiMasterService;
        public ApiMasterController(ApiMasterService apiMasterService) {
            _apiMasterService= apiMasterService;
        }
        [HttpGet("GetDocset")]
        public I_ApiMasterSet GetDocset(string docid) {
           return _apiMasterService.GetDocSet(docid); 
        }

        [HttpGet("GetApiInfo")]
        public vw_api_master GetApiInfo(string? apiid) {
            apiid = apiid == null ? "" : apiid;
            var output = ApiMasterService.GetApiInfo(apiid);
            return output;
        }

       [HttpGet("ListApi")]
        public IEnumerable<api_master> ListApi(string? search, string? cate) {
            cate = cate == null ? "" : cate;
            search = search == null ? "" : search;
            return  ApiMasterService.ListApi_BySearch(search, cate); 
        }

        //topadd
        [HttpGet("ListDataCategory")]
        public List<ApiCate> ListDataCategory() {

            var output = ApiMasterService.ListDataCategory();
            return output;

        }
        [HttpGet("Listapi_master")]
        public IEnumerable<api_master> Listapi_master(string? search) {
            search = search == null ? "" : search;
            return ApiMasterService.Listapi_master(search);
        }

        [HttpPost("SaveApiMaster")]
        public I_BasicResult SaveApiMaster([FromBody] api_master data) {

            //var docapi = JsonSerializer.Deserialize<I_ApiMasterSet>(data, new JsonSerializerOptions {
            //    PropertyNameCaseInsensitive = true,
            //    ReferenceHandler = ReferenceHandler.Preserve
            //});
            I_BasicResult r = ApiMasterService.Save(data);
            return r;
        }

        [HttpGet("GenSort")]
        public int GenSort() {
            return ApiMasterService.GenSort();
        }

        [HttpPost("ReOrder")]
        public I_BasicResult ReOrder([FromBody] string data) {
            var doc = JsonSerializer.Deserialize<List<api_param_res>>(data, new JsonSerializerOptions {
                PropertyNameCaseInsensitive = true,
                ReferenceHandler = ReferenceHandler.Preserve
            });
            I_BasicResult r = ApiMasterService.ReOrder(doc);
            return r;
        }

        #region ApiiParam

        [HttpPost("AddApiiParam")]
        public I_BasicResult AddApiiParam([FromBody] string data) {
            var data_api_param_res = JsonSerializer.Deserialize<api_param_res>(data, new JsonSerializerOptions {
                PropertyNameCaseInsensitive = true,
                ReferenceHandler = ReferenceHandler.Preserve
            });
            I_BasicResult r = ApiMasterService.AddApiiParam(data_api_param_res);
            return r;
        }

        [HttpGet("DeleteApiParam")]
        public I_BasicResult DeleteApiParam(string? ID) {
            ID = ID == null ? "0" : ID;
            return ApiMasterService.DeleteApiParam(Convert.ToInt32(ID));
        }

        #endregion

        #region ApiTag

        [HttpPost("AddApiTag")]
        public I_BasicResult AddApiTag([FromBody] string data) {
            var data_api_tag = JsonSerializer.Deserialize<api_tag>(data, new JsonSerializerOptions {
                PropertyNameCaseInsensitive = true,
                ReferenceHandler = ReferenceHandler.Preserve
            });
            I_BasicResult r = ApiMasterService.AddApiTag(data_api_tag);
            return r;
        }

        [HttpGet("DeleteApiTag")]
        public I_BasicResult DeleteApiTag(string? ID) {
            ID = ID == null ? "0" : ID;
            return ApiMasterService.DeleteApiTag(Convert.ToInt32(ID));
        }

        #endregion


        [HttpGet("ListAPIMasterALLCate")]
        public List<LISTSELECT_APIMasterALLCate> ListAPIMasterALLCate()
        {
            var output = ApiMasterService.ListAPIMasterALLCate();
            return output;
        }

        [HttpPost("UpdateCateApiByListApi")]
        public I_BasicResult UpdateCateApiByListApi([FromBody] string data,string apicate)
        {
            var data_list_apiid = JsonSerializer.Deserialize<List<string>>(data, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                ReferenceHandler = ReferenceHandler.Preserve
            });
            I_BasicResult r = ApiMasterService.UpdateCateApiByListApi(data_list_apiid, apicate);
            return r;
        }

    }
}
