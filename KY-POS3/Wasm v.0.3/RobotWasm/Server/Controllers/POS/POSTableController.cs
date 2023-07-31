using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RobotWasm.Server.Controllers.Login;
using RobotWasm.Server.Data.DA.Master;
using RobotWasm.Server.Data.DA.POS;
using RobotWasm.Shared.Data.GaDB;
using RobotWasm.Shared.Data.ML.Login;
using System.Text.Json;
using System.Text.Json.Serialization;
using static RobotWasm.Shared.Data.DA.POSFuncService;
using static RobotWasm.Shared.Data.ML.Shared.I_Result;

namespace RobotWasm.Server.Controllers.POS
{
    [Route("api/[controller]")]
    [ApiController]
    public class POSTableController : ControllerBase {


        [HttpGet("ListTable")]
        public List<POS_TableModel> ListTable(string? rcom,string? com)
        {
            rcom = rcom == null ? "" : rcom;
            com = com == null ? "" : com;
            return TableInfoService.ListTable(rcom,com);
        }

    }
}
