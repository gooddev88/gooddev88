using RobotWasm.Shared.Data.GaDB;
using RobotWasm.Shared.Data.ML.Shared;
using System.Text.Json;
using System.Text.Json.Serialization;
using static RobotWasm.Shared.Data.ML.Q.QFloodServiceModel;

namespace RobotWasm.Client.Data.DA.Q {
    public class QFloodService {

        private readonly HttpClient _http;
        public QFloodService(HttpClient Http) {
            _http = Http;
        }
   

        public QFloodDocSet DocSet { get; set; }


        async public Task<QFloodDocSet> GetDocSet(string mcode) {
            mcode = mcode == null ? "" : mcode;
            QFloodDocSet doc = new QFloodDocSet();
            try {
                var res = await _http.GetAsync($"api/QFlood/GetDocSet?mcode={mcode}");
                doc = JsonSerializer.Deserialize<QFloodDocSet>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            } catch (Exception ex) {
                var x = ex.Message;
            }
            return doc;
        }

        async public Task<a_mm> GetVillage(string mcode) {
            a_mm doc = new a_mm();
            try {
                var res = await _http.GetAsync($"api/QFlood/GetVillage?mcode={mcode}");
                doc = JsonSerializer.Deserialize<a_mm>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            } catch (Exception ex) {
                var x = ex.Message;
            }
            return doc;
        }

        async public Task<QFloodDocSet> NewTransaction(string mcode) {
            QFloodDocSet doc = new QFloodDocSet();
            try {
                var res = await _http.GetAsync($"api/QFlood/NewTransaction?mcode={mcode}");
                doc = JsonSerializer.Deserialize<QFloodDocSet>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            } catch (Exception ex) {
                var x = ex.Message;
            }
            return doc;
        }

        async public Task<QFloodDocSet> SaveQFlood(QFloodDocSet data,bool isnew) {
            QFloodDocSet doc = new QFloodDocSet();
            try {
                var res = await _http.GetAsync($"api/QFlood/SaveQFlood");
                doc = JsonSerializer.Deserialize<QFloodDocSet>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            } catch (Exception ex) {
                var x = ex.Message;
            }
            return doc;
        }

        async public Task<List<a_mm>> ListVillage(string? search) {
            List<a_mm> output = new List<a_mm>();
            try {
                var res = await _http.GetAsync($"api/QFlood/ListVillage?search={search}");
                output = JsonSerializer.Deserialize<List<a_mm>>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            } catch (Exception ex) {
                var x = ex.Message;
            }
            return output;
        }

        public static List<SelectOption> ListLevelRisk() {
            return new List<SelectOption>() {
                new SelectOption(){ Value= "", Description="ไม่ระบุ" ,Sort = 1},
                new SelectOption(){ Value = "ความเสี่ยงสูงมาก", Description="ความเสี่ยงสูงมาก", Sort = 2},
                new SelectOption(){ Value = "ความเสี่ยงสูง", Description="ความเสี่ยงสูง", Sort = 3},
                new SelectOption(){ Value = "ความเสี่ยงปานกลาง", Description="ความเสี่ยงปานกลาง", Sort = 4},
                new SelectOption(){ Value = "ความเสี่ยงต่ำ", Description="ความเสี่ยงต่ำ", Sort = 5},
                new SelectOption(){ Value = "ความเสี่ยงต่ำมาก", Description="ความเสี่ยงต่ำมาก", Sort = 6},
            };
        }

    }
}
