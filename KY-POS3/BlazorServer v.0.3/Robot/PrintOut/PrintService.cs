using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using static Robot.Data.ML.I_Result;
using static Robot.PrintOut.CreatePrintData.SalePrintConverter;

namespace Robot.PrintOut {
    public class PrintService {
        async public static Task<I_BasicResult> CreatePrintDataApi(PrintData data) {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                string dataEndpointUri = $"{Globals.ApiPrintMasterBaseUrl}/api/Print";
                var xdata = JsonSerializer.Serialize(data);
                HttpClient httpClient = new HttpClient();
                var response = await httpClient.PostAsJsonAsync(dataEndpointUri, data);
                var jsonStr = response.Content.ReadAsStringAsync().Result;

                if (response.StatusCode.ToString() != "OK") {
                    r.Result = "fail";
                    r.Message1 = "Web service Error!";
                } else {
                    r = JsonSerializer.Deserialize<I_BasicResult>(jsonStr);
                }

            } catch (Exception e) {
                r.Result = "fail";
                if (e.InnerException != null) {
                    r.Message1 = e.InnerException.Message;
                } else {
                    r.Message1 = e.Message;
                }
            }
            return r;
        }
    }
}
