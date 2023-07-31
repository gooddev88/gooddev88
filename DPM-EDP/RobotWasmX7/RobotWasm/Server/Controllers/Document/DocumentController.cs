using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RobotWasm.Server.Data.DA.Q;
using RobotWasm.Shared.Data.GaDB;
using System.Text.Json;
using System.Text.Json.Serialization;
using RobotWasm.Shared.Data.ML.DocHead;
using static RobotWasm.Shared.Data.ML.Shared.I_Result;
using RobotWasm.Shared.Data.DimsDB;
using RobotWasm.Shared.Data.ML.DPMBaord.BoardData;

namespace RobotWasm.Server.Controllers.Document {
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentController : ControllerBase {



        [HttpGet("GetDocSet")]
        public I_DocHeadSet GetDocSet(string docid) {
            return DocumentService.GetDocSet(docid);
        }

        [HttpPost("GetLatestFiles")]
        public I_DocHeadSet GetLatestFiles([FromBody] I_DocHeadSet data) {

       
            I_DocHeadSet r = DocumentService.GetLatestFiles(data);
            return r;
        }

        [HttpPost("SaveDocument")]
        public I_BasicResult SaveDocument([FromBody] string data,[FromQuery] bool action) {

            var dochead = JsonSerializer.Deserialize<I_DocHeadSet>(data, new JsonSerializerOptions {
                PropertyNameCaseInsensitive = true,
                ReferenceHandler = ReferenceHandler.Preserve
            });
            I_BasicResult r = DocumentService.Save(dochead, action);
            return r;
        }

        [HttpGet("DeleteDoc")]
        public I_BasicResult DeleteDoc(string docid,string modified_by) {

            return DocumentService.DeleteDoc(docid, modified_by);
        }

        [HttpPost("ListDocument")]
        public List<vw_doc_head> ListDocument([FromBody] I_DocHeadFiterSet f) {
            var r = DocumentService.ListDocument(f);
            return r;
        }

        [HttpGet("GetLisDocumentHeadDoc")]
        public ListDocumentHeadDocSet GetLisDocumentHeadDoc(string? cateid)
        {
            cateid = cateid == null ? "" : cateid;
            return DocumentService.GetLisDocumentHeadDoc(cateid);
        }

        [HttpGet("ListDataCategory")]
        public List<vw_publishdoc_cate> ListDataCategory() {

            var output = DocumentService.ListDataCategory();
            return output;

        }
    }
}
