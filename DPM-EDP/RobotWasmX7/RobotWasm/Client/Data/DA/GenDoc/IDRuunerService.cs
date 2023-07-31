using RobotWasm.Shared.Data.GaDB;
using System.Text.Json;
using System.Text.Json.Serialization;
using RobotWasm.Shared.Data.DimsDB;
using static RobotWasm.Shared.Data.ML.Shared.I_Result;
using System.Net.Http.Json;

namespace RobotWasm.Client.Data.DA.GenDoc
{
    public class IDRuunerService
    {
        private readonly HttpClient _http;
        public IDRuunerService(HttpClient Http) {
            _http = Http;
        }

        async public Task<List<string>?> GetNewIDV2(string docTypeId, string? rcom, string? comId, DateTime docdate, bool isrun_next, string year_culture) {
            List<string>? output = new List<string> { "R1", "" };
            rcom = rcom == null ? "" : rcom;
            comId = comId == null ? "" : comId;
            try {
                var res = await _http.GetAsync($"api/RuunerID/GetNewIDV2?docTypeId={docTypeId}&rcom&={rcom}&comId={comId}&docdate={docdate}&isrun_next={isrun_next}&year_culture={year_culture}");
                output = JsonSerializer.Deserialize<List<string>>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions {
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
