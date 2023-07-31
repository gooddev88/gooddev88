using System.Text.Json;
using System.Text.Json.Serialization;
using static RobotWasm.Shared.Data.DA.POSFuncService;

namespace RobotWasm.Client.Data.DA.POS {
    public class TableInfoService {

        private readonly HttpClient _http;
        public TableInfoService(HttpClient Http) {
            _http = Http;
        }

        #region GetData

        async public Task<List<POS_TableModel>> ListTable(string? rcom, string? com) {
            rcom = rcom == null ? "" : rcom;
            com = com == null ? "" : com;
            List<POS_TableModel> doc = new List<POS_TableModel>();
            try {
                var res = await _http.GetAsync($"api/POSTable/ListTable?rcom={rcom}&com={com}");
                doc = JsonSerializer.Deserialize<List<POS_TableModel>>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            } catch (Exception ex) {
                var x = ex.Message;
            }
            return doc;
        }

        #endregion

    }
}
