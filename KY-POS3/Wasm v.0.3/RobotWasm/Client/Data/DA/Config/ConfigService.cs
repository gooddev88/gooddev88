using RobotWasm.Shared.Data.GaDB;
using System.Text.Json;
using System.Text.Json.Serialization;
using static RobotWasm.Shared.Data.ML.Shared.I_Result;
using System.Net.Http.Json;
using System.Collections.Generic;
using RobotWasm.Shared.Data.ML.Param;

namespace RobotWasm.Client.Data.DA.Master {
    public class ConfigService {

        private readonly HttpClient _http;
        public ConfigService(HttpClient Http) {
            _http = Http;
        }

        async public Task<ConfigParam> GetConfig(string? type) {
            type = type == null ? "" : type;
            ConfigParam result = new ConfigParam();
            try {
                var res = await _http.GetAsync($"api/Config/GetConfig?type={type}");
                result = JsonSerializer.Deserialize<ConfigParam>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            } catch (Exception ex) {
                var x = ex.Message;
            }
            return result;
        }

    }
}
