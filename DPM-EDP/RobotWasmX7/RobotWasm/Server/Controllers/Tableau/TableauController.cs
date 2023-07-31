using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace RobotWasm.Server.Controllers.Tableau {
    [Route("api/[controller]")]
    [ApiController]
    public class TableauController : ControllerBase {

        [HttpGet("GetBoardUrl")]
        async public Task<string> GetBoardUrl(string board_id) {
            string token = "";

            try {
                using (HttpClient http = new HttpClient()) {
                    //   string url = $"https://edpapim.gooddev.net/api/Tableau/GetBoardUrl?board_id={board_id}";
                    string url = $"https://edpapim.disaster.go.th/api/Tableau/GetBoardUrlForEdp?board_id={board_id}";
                    token = await http.GetStringAsync(url);
                }

            } catch (Exception ex) {
                var x = ex.Message;
            }
            return token;
        }
    }
}
