using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RobotAPI.Data.DA.Tableau;
using System.Net.Http.Headers;
using System.Text;

namespace RobotAPI.Controllers.Authen {
    [Route("api/[controller]")]
    [ApiController]
    public class TableauController : ControllerBase {

        [HttpGet]
        [Route("GenToken")]
        async public Task<string> GenToken() {
            return await Task.Run(() => TrustedAuth.GenToken());
        }
        [HttpGet]
        [Route("GenTokenForEdp")]
        async public Task<string> GenTokenForEdp() {
            return await Task.Run(() => TrustedAuth.GenTokenForEdp());
        }

        [HttpGet]
        [Route("GetBoardUrl")]
        async public Task<string> GetBoardUrl(string board_id) {
          return  await Task.Run(()=> TrustedAuth.GetBoardUrl(board_id));

        } 
        [HttpGet]
        [Route("GetBoardUrlForEdp")]
        async public Task<string> GetBoardUrlForEdp(string board_id) {
          return  await Task.Run(()=> TrustedAuth.GetBoardUrlForEdp(board_id));

        }
    }
}
