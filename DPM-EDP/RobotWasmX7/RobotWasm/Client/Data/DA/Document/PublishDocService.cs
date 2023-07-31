using RobotWasm.Shared.Data.GaDB;
using System.Text.Json;
using System.Text.Json.Serialization;
using RobotWasm.Shared.Data.ML.PublishDoc;
using RobotWasm.Shared.Data.DimsDB;
using static RobotWasm.Shared.Data.ML.Shared.I_Result;
using System.Net.Http.Json;

namespace RobotWasm.Client.Data.DA.Document {
    public class PublishDocService {

        private readonly HttpClient _http;
        public PublishDocService(HttpClient Http) {
            _http = Http;
        }
   
        public I_PublishDoc_DocSet DocSet { get; set; }
        public ListPublishDocSet ListDocSet { get; set; }

        async public Task<I_PublishDoc_DocSet> GetDocSet(string docid) {
            docid = docid == null ? "" : docid;
            I_PublishDoc_DocSet doc = new I_PublishDoc_DocSet();
            try {
                var res = await _http.GetAsync($"api/PublishDoc/GetDocSet?docid={docid}");
                doc = JsonSerializer.Deserialize<I_PublishDoc_DocSet>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            } catch (Exception ex) {
                var x = ex.Message;
            }
            return doc;
        }

        async public Task<I_BasicResult> DeleteDocLine(string docid) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                var res = await _http.GetAsync($"api/PublishDoc/DeleteDocLine?docid={docid}");
                result = JsonSerializer.Deserialize<I_BasicResult>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            } catch (Exception ex) {
                var x = ex.Message;
            }
            return result;
        }

        async public Task<ListPublishDocSet> GetListPublishDoc() {
            ListPublishDocSet? doc = new ListPublishDocSet();
            try {
                var res = await _http.GetAsync($"api/PublishDoc/GetListPublishDoc");
                doc = JsonSerializer.Deserialize<ListPublishDocSet>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            } catch (Exception ex) {
                var x = ex.Message;
            }
            return doc;
        }

        async public Task<I_PublishDoc_DocSet> GetLatestFiles(I_PublishDoc_DocSet f) {
            I_PublishDoc_DocSet? output = new I_PublishDoc_DocSet();
            try {
                string url = $"api/PublishDoc/GetLatestFiles";
                var res = await _http.PostAsJsonAsync(url, f);
                output = JsonSerializer.Deserialize<I_PublishDoc_DocSet>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            } catch (Exception ex) {
                var x = ex.Message;
            }
            return output;
        }

        async public Task<vw_publish_doc_line> GetDocLine(string? docid) {
            vw_publish_doc_line output = new vw_publish_doc_line();
            try {
                var res = await _http.GetAsync($"api/PublishDoc/GetDocLine?docid={docid}");
                output = JsonSerializer.Deserialize<vw_publish_doc_line>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            } catch (Exception ex) {
                var x = ex.Message;
            }
            return output;
        }

        async public Task<List<xpublish_doc_head>> ListDocHead() {
            List<xpublish_doc_head> output = new List<xpublish_doc_head>();
            try {
                var res = await _http.GetAsync($"api/PublishDoc/ListDocHead");
                output = JsonSerializer.Deserialize<List<xpublish_doc_head>>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            } catch (Exception ex) {
                var x = ex.Message;
            }
            return output;
        }

        async public Task<List<vw_publish_doc_line>> ListDocLine() {
            List<vw_publish_doc_line> output = new List<vw_publish_doc_line>();
            try {
                var res = await _http.GetAsync($"api/PublishDoc/ListDocLine");
                output = JsonSerializer.Deserialize<List<vw_publish_doc_line>>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions {
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
                var res = await _http.GetAsync($"api/PublishDoc/GenSort");
                output = JsonSerializer.Deserialize<short>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            } catch (Exception ex) {
                var x = ex.Message;
            }
            return output;
        }

        public static I_PublishDoc_DocSet NewTransaction() {
            I_PublishDoc_DocSet n = new I_PublishDoc_DocSet();
            n.head = NewHead();
            n.line = new List<vw_publish_doc_line>();
            n.files = new List<vw_xfile_ref>();
            return n;
        }

        public static publish_doc_head NewHead() {
            publish_doc_head n = new publish_doc_head();

            n.data_key = "";
            n.message = "";
            n.count_file = 0;
            n.modified_by = "";
            n.modified_date = null;
            return n;
        }

        public static publish_doc_line NewLine() {
            publish_doc_line n = new publish_doc_line();

            n.data_key = "";
            n.file_id = "";
            n.file_description = "";
            n.sort = 0;
            n.modified_by = "";
            n.modified_date = null;
            return n;
        }

        public static publish_doc_line Convertvw2publishdocline(vw_publish_doc_line input) {
            publish_doc_line n = new publish_doc_line();
            n.data_key = input.data_key;
            n.file_description = input.file_description;
            n.file_id = input.file_id;
            n.modified_by = input.modified_by;
            n.modified_date = input.modified_date;
            n.sort = input.sort;
            return n;
        }

    }
}
