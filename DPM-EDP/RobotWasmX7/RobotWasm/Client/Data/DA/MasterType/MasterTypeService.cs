using RobotWasm.Shared.Data.GaDB;
using System.Text.Json;
using System.Text.Json.Serialization;
using RobotWasm.Shared.Data.ML.Master.MasterType;
using RobotWasm.Shared.Data.DimsDB;
using static RobotWasm.Shared.Data.ML.Shared.I_Result;

namespace RobotWasm.Client.Data.DA.MasterType {
    public class MasterTypeService {

        private readonly HttpClient _http;
        public MasterTypeService(HttpClient Http) {
            _http = Http;
        }
   

        public I_MasterTypeSet DocSet { get; set; }


        async public Task<I_MasterTypeSet> GetDocSet(string docid) {
            docid = docid == null ? "" : docid;
            I_MasterTypeSet doc = new I_MasterTypeSet();
            try {
                var res = await _http.GetAsync($"api/MasterType/GetDocSet?docid={docid}");
                doc = JsonSerializer.Deserialize<I_MasterTypeSet>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            } catch (Exception ex) {
                var x = ex.Message;
            }
            return doc;
        }


        async public Task<I_MasterTypeSet> SaveBoardMaster(I_MasterTypeSet data) {
            I_MasterTypeSet doc = new I_MasterTypeSet();
            try {
                var res = await _http.GetAsync($"api/MasterType/SaveMaster");
                doc = JsonSerializer.Deserialize<I_MasterTypeSet>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            } catch (Exception ex) {
                var x = ex.Message;
            }
            return doc;
        }

        async public Task<I_BasicResult> DeleteDoc(string docid) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                var res = await _http.GetAsync($"api/MasterType/DeleteDoc?docid={docid}");
                result = JsonSerializer.Deserialize<I_BasicResult>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            } catch (Exception ex) {
                var x = ex.Message;
            }
            return result;
        }

        async public Task<List<master_type_line>> ListGroupMaster(string masid,bool isShowBlank) {
            List<master_type_line> output = new List<master_type_line>();
            try {
                var res = await _http.GetAsync($"api/MasterType/ListType?masid={masid}&isShowBlank={isShowBlank}");
                output = JsonSerializer.Deserialize<List<master_type_line>>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            } catch (Exception ex) {
                var x = ex.Message;
            }
            return output;
        }

        async public Task<List<master_type_line>> ListDoc(string? search) {
            List<master_type_line> output = new List<master_type_line>();
            try {
                var res = await _http.GetAsync($"api/MasterType/ListDoc?search={search}");
                output = JsonSerializer.Deserialize<List<master_type_line>>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            } catch (Exception ex) {
                var x = ex.Message;
            }
            return output;
        }

        async public Task<short> GenSort() {
            short output = 0;
            try {
                var res = await _http.GetAsync($"api/MasterType/GenSort");
                output = JsonSerializer.Deserialize<short>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            } catch (Exception ex) {
                var x = ex.Message;
            }
            return output;
        }

        public static I_MasterTypeSet NewTransaction() {
            I_MasterTypeSet n = new I_MasterTypeSet();
            n.lineAtive = NewHead();
            return n;
        }

        public static master_type_line NewHead() {
            master_type_line n = new master_type_line();

            n.master_type_id = "document_cate";
            n.master_type_name = "หมวดเอกสารเผยแพร่";
            n.value_txt = "";
            n.desc1 = "";
            n.desc2 = "";
            n.sort = 0;
            n.value_num = 0;
            n.is_active = 1;
            return n;
        }

    }
}
