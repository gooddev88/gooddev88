using RobotWasm.Shared.Data.GaDB;
using System.Text.Json;
using System.Text.Json.Serialization;
using RobotWasm.Shared.Data.DimsDB;
using static RobotWasm.Shared.Data.ML.Shared.I_Result;

namespace RobotWasm.Client.Data.DA.Master {
    public class LogTranService {

        private readonly HttpClient _http;
        public LogTranService(HttpClient Http) {
            _http = Http;
        }   
        async public Task<List<vw_trans_logs>> ListDoc(string? app_id, string? search,string? DateFrom, string? DateTo) {
            List<vw_trans_logs> output = new List<vw_trans_logs>();
            try {
                var res = await _http.GetAsync($"api/LogTrans/ListLogsHistory?app_id={app_id}&search={search}&DateFrom={DateFrom}&DateTo={DateTo}");
                output = JsonSerializer.Deserialize<List<vw_trans_logs>>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            } catch (Exception ex) {
                var x = ex.Message;
            }
            return output;
        }

        async public Task<I_BasicResult> CreateTransLog(string? docid, string? user, string? module, string? action)
        {
            docid = docid == null ? "" : docid;
            user = user == null ? "" : user;
            module = module == null ? "" : module;
            action = action == null ? "" : action;
            I_BasicResult output = new I_BasicResult();
            try
            {
                var res = await _http.GetAsync($"api/LogTrans/CreateTransLog?docid={docid}&user={user}&module={module}&action={action}");
                output = JsonSerializer.Deserialize<I_BasicResult>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            }
            catch (Exception ex)
            {
                var x = ex.Message;
            }
            return output;
        }

    }
}
