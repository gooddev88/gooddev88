using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RobotWasm.Server.Data.DA.Board.G10;
using RobotWasm.Shared.Data.DimsDB;
using RobotWasm.Shared.Data.ML.DPMBaord.BoardData.Widget;

namespace RobotWasm.Server.Controllers.Board.G10 {
    [Route("api/CustomBoard/group/[controller]")]
    [ApiController]
    public class G10Controller : ControllerBase {
        [HttpGet("GetWG101Data")]
        public List<WG101Data> GetWG101Data(string board_id ) {
            return G10Service.GetWG101(board_id);
          
        }
    }
}
