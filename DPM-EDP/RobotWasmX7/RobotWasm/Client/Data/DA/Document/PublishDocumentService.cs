using RobotWasm.Shared.Data.GaDB;
using RobotWasm.Shared.Data.ML.Shared;
using System.Text.Json;
using System.Text.Json.Serialization;
using RobotWasm.Shared.Data.ML.DocHead;
using RobotWasm.Shared.Data.DimsDB;
using System.Net.Http.Json;
using static RobotWasm.Shared.Data.ML.Shared.I_Result;
using RobotWasm.Shared.Data.ML.DPMBaord.BoardData;

namespace RobotWasm.Client.Data.DA.Document {
    public class PublishDocumentService {

        private readonly HttpClient _http;
        public PublishDocumentService(HttpClient Http) {
            _http = Http;
        }
   

        public I_DocHeadSet DocSet { get; set; }
        public ListDocumentHeadDocSet ListDocSet { get; set; }

        async public Task<I_DocHeadSet> GetDocSet(string docid) {
            docid = docid == null ? "" : docid;
            I_DocHeadSet? doc = new I_DocHeadSet();
            try {
                var res = await _http.GetAsync($"api/Document/GetDocSet?docid={docid}");
                doc = JsonSerializer.Deserialize<I_DocHeadSet>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            } catch (Exception ex) {
                var x = ex.Message;
            }
            return doc;
        }

        async public Task<ListDocumentHeadDocSet> GetLisDocumentHeadDoc(string? cateid)
        {
            cateid = cateid == null ? "" : cateid;
            ListDocumentHeadDocSet? doc = new ListDocumentHeadDocSet();
            try
            {
                var res = await _http.GetAsync($"api/Document/GetLisDocumentHeadDoc?cateid={cateid}");
                doc = JsonSerializer.Deserialize<ListDocumentHeadDocSet>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            }
            catch (Exception ex)
            {
                var x = ex.Message;
            }
            return doc;
        }

        async public Task<I_DocHeadSet> SaveDocHead(I_DocHeadSet data,bool isnew) {
            I_DocHeadSet? doc = new I_DocHeadSet();
            try {
                var res = await _http.GetAsync($"api/Document/SaveDocHead");
                doc = JsonSerializer.Deserialize<I_DocHeadSet>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            } catch (Exception ex) {
                var x = ex.Message;
            }
            return doc;
        }

        async public Task<I_BasicResult> DeleteDoc(string docid,string modified_by) {
            I_BasicResult? result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                var res = await _http.GetAsync($"api/Document/DeleteDoc?docid={docid}&modified_by={modified_by}");
                result = JsonSerializer.Deserialize<I_BasicResult>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            } catch (Exception ex) {
                var x = ex.Message;
            }
            return result;
        }

        async public Task<List<vw_doc_head>> ListDoc(I_DocHeadFiterSet f) {
            List<vw_doc_head>? output = new List<vw_doc_head>();
            try {
                string url = $"api/Document/ListDocument";
                var res = await _http.PostAsJsonAsync(url,f);
                output = JsonSerializer.Deserialize<List<vw_doc_head>>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            } catch (Exception ex) {
                var x = ex.Message;
            }
            return output;
        }

        async public Task<List<vw_publishdoc_cate>> ListDocCates() {
            List<vw_publishdoc_cate>? output = new List<vw_publishdoc_cate>();
            try {
                var res = await _http.GetAsync($"api/Document/ListDataCategory");
                output = JsonSerializer.Deserialize<List<vw_publishdoc_cate>>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            } catch (Exception ex) {

                var x = ex.Message;
            }
            return output;
        }
        async public Task<I_DocHeadSet> GetLatestFiles(I_DocHeadSet f) {
            I_DocHeadSet? output = new I_DocHeadSet();
            try {
                string url = $"api/Document/GetLatestFiles";
                var res = await _http.PostAsJsonAsync(url, f);
                output = JsonSerializer.Deserialize<I_DocHeadSet>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            } catch (Exception ex) {
                var x = ex.Message;
            }
            return output;
        }

        public static I_DocHeadFiterSet NewFilterSet() {
            I_DocHeadFiterSet n = new I_DocHeadFiterSet(); 
            n.DateFrom = null;
            n.DateTo = null;
            n.Cate = "";
            n.IsPublish = -1;
            n.SearchText = ""; 
            return n;
        }

        public static I_DocHeadSet NewTransaction() {
            I_DocHeadSet n = new I_DocHeadSet();
            n.head = NewHead();
            n.files = new List<vw_xfile_ref>();
            return n;
        }

        public static doc_head NewHead() {
            doc_head n = new doc_head();

            n.id = 0;
            n.doc_id = "";
            n.doc_desc = "";
            n.doc_type_id = "publish document";
            n.doc_cate_id = "";
            n.doc_remark = "";
            n.publish_date = null;
            n.is_publish = 1;
            n.created_by = "";
            n.created_date = DateTime.Now;
            n.modified_by = "";
            n.modified_date = null;
            n.count_file = 0;
            n.is_active = 1;

            return n;
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
