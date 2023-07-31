using RobotWasm.Shared.Data.GaDB;
using System.Text.Json;
using System.Text.Json.Serialization;
using RobotWasm.Shared.Data.DimsDB;
using static RobotWasm.Shared.Data.ML.Shared.I_Result;
using RobotWasm.Shared.Data.ML.Shared;

namespace RobotWasm.Client.Data.DA.DataQuality {
    public class DataQualityService {

        private readonly HttpClient _http;
        public DataQualityService(HttpClient Http) {
            _http = Http;
        }   
        async public Task<List<dqt_data_logs>> ListDoc(string? search,string? datebegin, string? dateend) {
            List<dqt_data_logs> output = new List<dqt_data_logs>();
            try {
                var res = await _http.GetAsync($"api/DataQuality/ListDataQuality?search={search}&datebegin={datebegin}&dateend={dateend}");
                output = JsonSerializer.Deserialize<List<dqt_data_logs>>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            } catch (Exception ex) {
                var x = ex.Message;
            }
            return output;
        }

        public static List<SelectOption> ListStatus() {
            return new List<SelectOption>() {
                new SelectOption(){ Value= "", Description="ALL" ,Sort = 0},
                new SelectOption(){ Value= "success", Description="success" ,Sort = 1},
                new SelectOption(){ Value = "fail", Description="fail", Sort = 2}
            };
        }


    }
}
