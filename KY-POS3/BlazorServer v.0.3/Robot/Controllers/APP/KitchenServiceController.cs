using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Robot.Data.DA.Kitchen;
using Robot.Data.GADB.TT;
using System.Collections.Generic;
using static Robot.Data.ML.I_Result;
using static Robot.Data.DA.Kitchen.KitchenService;

namespace Robot.Controllers.APP {
    [Route("api/[controller]")]
    [ApiController]
    public class KitchenServiceController : ControllerBase {
        [HttpPost("UpdatePOSLineStatus")]
        public I_BasicResult UpdatePOSLineStatus([FromBody] KitchenStatusParam input) {
            return KitchenService.UpdatePOSLineStatus(input);
        }


        [HttpPost("ListOrderForKitchen")]
        public List<POS_SaleLine> ListOrderForKitchen([FromBody] KitchenStatusParam input) {
            return KitchenService.ListOrderForKitchen(input);
        }
    }
}
