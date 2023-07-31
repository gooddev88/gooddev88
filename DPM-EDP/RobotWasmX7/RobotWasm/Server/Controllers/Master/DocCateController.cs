using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RobotWasm.Server.Data.DA.Master;
using RobotWasm.Shared.Data.DimsDB;
using RobotWasm.Shared.Data.GaDB;
using RobotWasm.Shared.Data.ML.Master.DocCate;
using System.Text.Json;
using System.Text.Json.Serialization;
using static RobotWasm.Shared.Data.ML.Shared.I_Result;

namespace RobotWasm.Server.Controllers.Master {
    [Route("api/[controller]")]
    [ApiController]
    public class DocCateController : ControllerBase {


        [HttpGet("GetDocSet")]
        public I_DocCateSet GetDocSet(string docid) {
            return DocCateService.GetDocSet(docid);
        }

        [HttpPost("SaveDocCate")]
        public I_BasicResult SaveDocCate([FromBody] string data, bool action) {

            var head = JsonSerializer.Deserialize<I_DocCateSet>(data, new JsonSerializerOptions {
                PropertyNameCaseInsensitive = true,
                ReferenceHandler = ReferenceHandler.Preserve
            });
            I_BasicResult r = DocCateService.Save(head);
            return r;
        }

        [HttpPost("ReOrder")]
        public I_BasicResult ReOrder([FromBody] string data)
        {
            var doc = JsonSerializer.Deserialize<List<publishdoc_cate>>(data, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                ReferenceHandler = ReferenceHandler.Preserve
            });
            I_BasicResult r = DocCateService.ReOrder(doc);
            return r;
        }

        [HttpPost("GetLatestFiles")]
        public I_DocCateSet GetLatestFiles([FromBody] I_DocCateSet data) {
            I_DocCateSet r = DocCateService.GetLatestFiles(data);
            return r;
        }

        [HttpGet("DeleteDoc")]
        public I_BasicResult DeleteDoc(string docid) {
            return DocCateService.DeleteDoc(docid);
        }

        [HttpGet("GetDocCate")]
        public publishdoc_cate GetDocCate(string cateid)
        {
            return DocCateService.GetDocCate(cateid);
        }

        [HttpGet("GenSort")]
        public short GenSort() {
            return DocCateService.GenSort();
        }

        [HttpGet("ListDoc")]
        public List<vw_publishdoc_cate> ListDoc(string? search) {
            search = search == null ? "" : search;
            return DocCateService.ListDoc(search);
        }

        [HttpGet("ListDocCate")]
        public List<publishdoc_cate> ListDocCate()
        {
            return DocCateService.ListDocCate();
        }

    }
}
