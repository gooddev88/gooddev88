using RobotWasm.Client.Service.Api;
using RobotWasm.Shared.Data.ML.DPMBaord.BoardData.Widget.G10;

namespace RobotWasm.Client.Data.DA.Board.G20 {
    public class G20Service {
        private HttpClient _http;
        private ClientService _clientService;
        public G20Service(HttpClient http, ClientService clientService) {
            _http = http;
            _clientService = clientService;
        }


        async public Task<List<WG201Data>> GetWG201Data(string board_id) {
            //จำนวนสิ่งของสำรองจ่าย แบ่งตามจังหวัด 
             
            List<WG201Data>? output = new List<WG201Data>();
            try {
                var query = await Task.Run(() => _clientService.GetAllAsync<List<WG201Data>>($"{Globals.BaseURL}/api/portal/est008"));
                output = (List<WG201Data>)query.Result;
            } catch (Exception ex) {
            }
            return output;
        }
    }
}
