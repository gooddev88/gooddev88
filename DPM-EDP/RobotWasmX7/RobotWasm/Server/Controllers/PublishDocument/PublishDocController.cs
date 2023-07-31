using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RobotWasm.Server.Data.CimsDB.TT;
using RobotWasm.Server.Data.DA.PublishDocument;
using RobotWasm.Shared.Data.DimsDB;
using RobotWasm.Shared.Data.GaDB;
using RobotWasm.Shared.Data.ML.DPMBaord.BoardData;
using RobotWasm.Shared.Data.ML.PublishDoc;
using System.Text.Json;
using System.Text.Json.Serialization;
using static RobotWasm.Shared.Data.ML.Shared.I_Result;

namespace RobotWasm.Server.Controllers.PublishDocument {
    [Route("api/[controller]")]
    [ApiController]
    public class PublishDocController : ControllerBase {


        [HttpGet("GetDocSet")]
        public I_PublishDoc_DocSet GetDocSet(string docid) {
            return PublishDocService.GetDocSet(docid);
        }

        [HttpPost("SavePublishDoc")]
        public I_BasicResult SavePublishDoc([FromBody] string data, bool action) {
            var head = JsonSerializer.Deserialize<I_PublishDoc_DocSet>(data, new JsonSerializerOptions {
                PropertyNameCaseInsensitive = true,
                ReferenceHandler = ReferenceHandler.Preserve
            });
            I_BasicResult r = PublishDocService.Save(head, action);
            return r;
        }

        [HttpPost("SaveDocLine")]
        public I_BasicResult SaveDocLine([FromBody] string data, bool action) {
            var head = JsonSerializer.Deserialize<publish_doc_line>(data, new JsonSerializerOptions {
                PropertyNameCaseInsensitive = true,
                ReferenceHandler = ReferenceHandler.Preserve
            });
            I_BasicResult r = PublishDocService.SaveDocLine(head, action);
            return r;
        }

        [HttpGet("GetDocLine")]
        public publish_doc_line GetDocLine(string? docid) {
            docid = docid == null ? "" : docid;
            return PublishDocService.GetDocLine(docid);
        }

        //[HttpPost("GetLatestFiles")]
        //public I_PublishDoc_DocSet GetLatestFiles([FromBody] I_PublishDoc_DocSet data) {
        //    I_PublishDoc_DocSet r = PublishDocService.GetLatestFiles(data);
        //    return r;
        //}

        [HttpGet("GetListPublishDoc")]
        public ListPublishDocSet GetListPublishDoc() {
            return PublishDocService.GetListPublishDoc();
        }

        [HttpGet("DeleteDocLine")]
        public I_BasicResult DeleteDocLine(string docid) {
            return PublishDocService.DeleteDocLine(docid);
        }

        [HttpGet("GenSort")]
        public short GenSort() {
            return PublishDocService.GenSort();
        }

        [HttpGet("ListDocHead")]
        public List<xpublish_doc_head> ListDocHead() {
            return PublishDocService.ListDocHead();
        }

        [HttpGet("ListDocLine")]
        public List<vw_publish_doc_line> ListDocLine() {
            return PublishDocService.ListDocLine();
        }

      


    }
}
