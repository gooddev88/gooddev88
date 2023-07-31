using RobotWasm.Shared.Data.GaDB;
using System.Text.Json;
using System.Text.Json.Serialization;
using static RobotWasm.Shared.Data.ML.Q.QServiceModel;

namespace RobotWasm.Client.Data.DA.Q {
    public class QService {

        private readonly HttpClient _http;
        public QService(HttpClient Http) {
            _http = Http;
        }
   

        public QDocSet DocSet { get; set; }


     async   public   Task<QDocSet> GetDocSet(string username, string group_id) {
            QDocSet doc = new QDocSet(); 
            try {
                var res = await _http.GetAsync($"api/Q/getdocset?username={username}&group_id={group_id}");
                doc = JsonSerializer.Deserialize<QDocSet>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            } catch (Exception ex) { 
                var x = ex.Message;
            } 
            return doc;
        }

        async public Task<List<q_group_master>> ListGroupMaster() {
            List<q_group_master> output = new List<q_group_master>();
            try {
                var res = await _http.GetAsync($"api/Q/ListGroupMaster");
                output = JsonSerializer.Deserialize<List<q_group_master>>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions {
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
