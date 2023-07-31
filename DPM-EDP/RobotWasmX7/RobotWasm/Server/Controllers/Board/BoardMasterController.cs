using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RobotWasm.Server.Data.DA.Board;
using RobotWasm.Shared.Data.GaDB;
using System.Text.Json;
using System.Text.Json.Serialization;
using RobotWasm.Shared.Data.ML.DPMBaord.ExclusiveBoard;
using static RobotWasm.Shared.Data.ML.Shared.I_Result;
using RobotWasm.Shared.Data.DimsDB;

namespace RobotWasm.Server.Controllers.Board {
    [Route("api/[controller]")]
    [ApiController]
    public class BoardMasterController : ControllerBase {



        [HttpGet("GetDocSet")]
        public I_Board_MasterSet GetDocSet(string docid) {
            return BoardMasterService.GetDocSet(docid);
        }

        [HttpPost("SaveBoardMaster")]
        public I_BasicResult SaveBoardMaster([FromBody] string data,bool action) { 
            var dochead = JsonSerializer.Deserialize<I_Board_MasterSet>(data, new JsonSerializerOptions {
                PropertyNameCaseInsensitive = true,
                ReferenceHandler = ReferenceHandler.Preserve
            });
            I_BasicResult r = BoardMasterService.Save(dochead, action);
            return r;
        }

        [HttpPost("ReOrder")]
        public I_BasicResult ReOrder([FromBody] string data)
        {
            var doc = JsonSerializer.Deserialize<List<board_master>>(data, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                ReferenceHandler = ReferenceHandler.Preserve
            });
            I_BasicResult r = BoardMasterService.ReOrder(doc);
            return r;
        }

        [HttpPost("GetLatestFiles")]
        public I_Board_MasterSet GetLatestFiles([FromBody] I_Board_MasterSet data)
        {
            I_Board_MasterSet r = BoardMasterService.GetLatestFiles(data);
            return r;
        }

        [HttpGet("DeleteDoc")]
        public I_BasicResult DeleteDoc(string docid,string modified_by) {
            return BoardMasterService.DeleteDoc(docid, modified_by);
        }

        [HttpGet("GenSort")]
        public short GenSort() {
            return BoardMasterService.GenSort();
        }

        [HttpGet("ListDoc")]
        public List<vw_board_master> ListDoc(string? search)
        {
            search = search == null ? "" : search;
            return BoardMasterService.ListDoc(search);
        }

    }
}
