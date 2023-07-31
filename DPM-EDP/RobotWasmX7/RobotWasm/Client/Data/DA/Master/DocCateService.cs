using RobotWasm.Shared.Data.GaDB;
using System.Text.Json;
using System.Text.Json.Serialization;
using RobotWasm.Shared.Data.ML.Master.DocCate;
using RobotWasm.Shared.Data.DimsDB;
using static RobotWasm.Shared.Data.ML.Shared.I_Result;
using System.Net.Http.Json;

namespace RobotWasm.Client.Data.DA.Master {
    public class DocCateService {

        private readonly HttpClient _http;
        public DocCateService(HttpClient Http) {
            _http = Http;
        }

        public string Select_DoccateID { get; set; }
        public I_DocCateSet DocSet { get; set; }

        async public Task<I_DocCateSet> GetDocSet(string docid) {
            docid = docid == null ? "" : docid;
            I_DocCateSet doc = new I_DocCateSet();
            try {
                var res = await _http.GetAsync($"api/DocCate/GetDocSet?docid={docid}");
                doc = JsonSerializer.Deserialize<I_DocCateSet>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            } catch (Exception ex) {
                var x = ex.Message;
            }
            return doc;
        }


        async public Task<I_DocCateSet> SaveApiCate(I_DocCateSet data) {
            I_DocCateSet doc = new I_DocCateSet();
            try {
                var res = await _http.GetAsync($"api/DocCate/SaveDocCate");
                doc = JsonSerializer.Deserialize<I_DocCateSet>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions {
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
                var res = await _http.GetAsync($"api/DocCate/DeleteDoc?docid={docid}");
                result = JsonSerializer.Deserialize<I_BasicResult>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            } catch (Exception ex) {
                var x = ex.Message;
            }
            return result;
        }

        async public Task<publishdoc_cate> GetDocCate(string cateid)
        {
            publishdoc_cate result = new publishdoc_cate();
            try
            {
                var res = await _http.GetAsync($"api/DocCate/GetDocCate?cateid={cateid}");
                result = JsonSerializer.Deserialize<publishdoc_cate>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            }
            catch (Exception ex)
            {
                var x = ex.Message;
            }
            return result;
        }

        async public Task<I_BasicResult?> ReOrder(List<publishdoc_cate> data)
        {
            I_BasicResult? output = new I_BasicResult();
            try
            {
                string strPayload = JsonSerializer.Serialize(data);
                var response = await _http.PostAsJsonAsync($"api/DocCate/ReOrder", strPayload);
                output = JsonSerializer.Deserialize<I_BasicResult>(await response.Content.ReadAsStringAsync(), new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            }
            catch (Exception ex)
            {
                var x = ex.Message;
            }
            finally
            {
            }
            return output;
        }

        async public Task<I_DocCateSet> GetLatestFiles(I_DocCateSet f) {
            I_DocCateSet? output = new I_DocCateSet();
            try {
                string url = $"api/DocCate/GetLatestFiles";
                var res = await _http.PostAsJsonAsync(url, f);
                output = JsonSerializer.Deserialize<I_DocCateSet>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            } catch (Exception ex) {
                var x = ex.Message;
            }
            return output;
        }

        async public Task<List<vw_publishdoc_cate>> ListDoc(string? search){
            List<vw_publishdoc_cate> output = new List<vw_publishdoc_cate>();
            try {
                var res = await _http.GetAsync($"api/DocCate/ListDoc?search={search}");
                output = JsonSerializer.Deserialize<List<vw_publishdoc_cate>>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            } catch (Exception ex) {
                var x = ex.Message;
            }
            return output;
        }

        async public Task<List<publishdoc_cate>> ListDocCate()
        {
            List<publishdoc_cate> output = new List<publishdoc_cate>();
            try
            {
                var res = await _http.GetAsync($"api/DocCate/ListDocCate");
                output = JsonSerializer.Deserialize<List<publishdoc_cate>>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            }
            catch (Exception ex)
            {
                var x = ex.Message;
            }
            return output;
        }

        async public Task<short> GenSort() {
            short output = 0;
            try {
                var res = await _http.GetAsync($"api/DocCate/GenSort");
                output = JsonSerializer.Deserialize<short>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            } catch (Exception ex) {
                var x = ex.Message;
            }
            return output;
        }

        public static I_DocCateSet NewTransaction() {
            I_DocCateSet n = new I_DocCateSet();
            n.head = NewHead();
            n.files = new List<vw_xfile_ref>();
            return n;
        }

        public static vw_publishdoc_cate NewHead() {
            vw_publishdoc_cate n = new vw_publishdoc_cate();

            n.cate_id = "";
            n.cate_name = "";
            n.page = "";
            n.img_path = "";
            n.sort = 0;
            n.is_active = 1;
            return n;
        }

        public static List<publishdoc_cate> ConvertViewToHead(List<vw_publishdoc_cate> input)
        {
            List<publishdoc_cate> result = new List<publishdoc_cate>();
            foreach (var l in input)
            {
                publishdoc_cate n = new publishdoc_cate();

                n.cate_id = l.cate_id;
                n.cate_name = l.cate_name;
                n.page = l.page;
                n.img_path = l.img_path;
                n.sort = l.sort;
                n.is_active = l.is_active;
                result.Add(n);
            }


            return result;
        }

    }
}
