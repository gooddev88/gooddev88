using System.Text.Json;

namespace RobotWasm.Client.Data.DA.Tableau {
    public class TableauService {
        private readonly HttpClient _http;
        public TableauService(HttpClient Http) {
            _http = Http;
        }
        async public Task<string> GetBoardUrl(string board_id) {
            string token = "";
            try {
                var res = await _http.GetAsync($"api/Tableau/GetBoardUrl?board_id={board_id}");
                
                token = res.Content.ReadAsStringAsync().Result;

            } catch (Exception ex) {
                var x = ex.Message;
            }
            return token;
        }

    }
}
