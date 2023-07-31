using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RobotWasm.Server.Data.DA.Q;
using RobotWasm.Shared.Data.GaDB;
using static RobotWasm.Shared.Data.ML.Q.QServiceModel;

namespace RobotWasm.Server.Controllers.Q {
    [Route("api/[controller]")]
    [ApiController]
    public class QController : ControllerBase {



        [HttpGet("GetDocSet")]
        public QDocSet GetDocSet(string username, string group_id) {

            return QService.GetDocSet(username, group_id);

        }
        [HttpGet("ListGroupMaster")]
        public List<q_group_master> ListGroupMaster( ) {

            return QService.ListGroupMaster();
        }

    }
}
