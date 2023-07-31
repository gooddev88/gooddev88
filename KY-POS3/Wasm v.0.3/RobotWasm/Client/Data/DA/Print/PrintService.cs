using static RobotWasm.Shared.Data.DA.POSFuncService;
using static RobotWasm.Shared.Data.ML.Shared.I_Result;
using static System.Net.WebRequestMethods;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Net.Http.Json;

namespace RobotWasm.Client.Data.DA.Print {
    public class PrintService {

        private readonly HttpClient _http;
        public PrintService(HttpClient Http) {
            _http = Http;
        }
        async public Task<I_BasicResult> RunReport(I_POSSaleSet data, string printform_id) {
            I_BasicResult doc = new I_BasicResult();
            try {
                string strPayload = JsonSerializer.Serialize(data);
                string url = $"api/Print/RunReport?printform_id={printform_id}";
                var response = await _http.PostAsJsonAsync(url,strPayload);
                var status = response.StatusCode;

                if (status.ToString().ToLower() != "ok") {
                } else {
                    doc = JsonSerializer.Deserialize<I_BasicResult>(await response.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                        PropertyNameCaseInsensitive = true,
                        ReferenceHandler = ReferenceHandler.Preserve
                    });
                }
            } catch (Exception ex) {
                var x = ex.Message;
            }
            return doc;
        }

    }
}
