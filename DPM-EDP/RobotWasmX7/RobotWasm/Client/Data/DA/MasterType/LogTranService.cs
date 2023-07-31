using RobotWasm.Shared.Data.GaDB;
using System.Text.Json;
using System.Text.Json.Serialization;
using RobotWasm.Shared.Data.DimsDB;
using static RobotWasm.Shared.Data.ML.Shared.I_Result;

namespace RobotWasm.Client.Data.DA.MasterType {
    public class LogTranService {

        private readonly HttpClient _http;
        public LogTranService(HttpClient Http) {
            _http = Http;
        }   
        async public Task<List<trans_logs>> ListDoc(string? app_id, string? search) {
            List<trans_logs> output = new List<trans_logs>();
            try {
                var res = await _http.GetAsync($"api/LogTrans/ListLogsHistory?app_id={app_id}&search={search}");
                output = JsonSerializer.Deserialize<List<trans_logs>>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            } catch (Exception ex) {
                var x = ex.Message;
            }
            return output;
        }

    }
}
