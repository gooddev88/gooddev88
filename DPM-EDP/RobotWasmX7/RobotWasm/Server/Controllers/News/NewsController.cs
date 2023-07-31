using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RobotWasm.Server.Data.DA.Master;
using RobotWasm.Shared.Data.DimsDB;
using RobotWasm.Shared.Data.GaDB;
using RobotWasm.Shared.Data.ML.Master.News;
using System.Text.Json;
using System.Text.Json.Serialization;
using static RobotWasm.Shared.Data.ML.Shared.I_Result;

namespace RobotWasm.Server.Controllers.News {
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase {


        [HttpGet("GetDocSet")]
        public I_NewsSet GetDocSet(string docid) {
            return NewsService.GetDocSet(docid);
        }

        [HttpPost("SaveNews")]
        public I_BasicResult SaveNews([FromBody] string data, bool action) {
            var head = JsonSerializer.Deserialize<I_NewsSet>(data, new JsonSerializerOptions {
                PropertyNameCaseInsensitive = true,
                ReferenceHandler = ReferenceHandler.Preserve
            });
            I_BasicResult r = NewsService.Save(head);
            return r;
        }

        [HttpGet("DeleteDoc")]
        public I_BasicResult DeleteDoc(string docid) {
            return NewsService.DeleteDoc(docid);
        }

        [HttpGet("ListDoc")]
        public List<news_info> ListDoc(string? search) {
            search = search == null ? "" : search;
            return NewsService.ListDoc(search);
        }

    }
}
