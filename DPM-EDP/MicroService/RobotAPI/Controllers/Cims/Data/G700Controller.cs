using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RobotAPI.Data.DA.Cims;
using RobotAPI.Data.DA.Data;
using RobotAPI.Data.ML.Budget;
using RobotAPI.Data.ML.Data;
using RobotAPI.Services.Api;

namespace RobotAPI.Controllers.Budget {
    [Route("api/[controller]")]
    [ApiController]
    public class G700Controller : ControllerBase {
        ClientService _clientService;
        Data700Service _data700Service;
        public G700Controller(ClientService clientService, Data700Service data600Service) {
            _clientService = clientService;
            _data700Service = data600Service;   
        }
      
        [AllowAnonymous]
        [HttpGet("data701")]
        async public Task<ActionResult> data701() {
            //data quality
            Data606Set.DocSet output = new Data606Set.DocSet { message = "ok", rows = new List<Data606Set.DataRow>(), status = 1 };
            try {
               // output = await Task.Run(() => _data700Service.Data606());
                Ok(output);

            } catch (Exception ex) {
                output.status = 0;
                output.message = ex.Message;
                BadRequest(output);
            }
            return Ok(output);
        }

     
    }
}
