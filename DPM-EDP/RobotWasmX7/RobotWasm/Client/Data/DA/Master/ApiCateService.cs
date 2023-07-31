using RobotWasm.Shared.Data.GaDB;
using System.Text.Json;
using System.Text.Json.Serialization;
using RobotWasm.Shared.Data.ML.Master.ApiCate;
using RobotWasm.Shared.Data.DimsDB;
using static RobotWasm.Shared.Data.ML.Shared.I_Result;
using System.Net.Http.Json;

namespace RobotWasm.Client.Data.DA.Master {
    public class ApiCateService {

        private readonly HttpClient _http;
        public ApiCateService(HttpClient Http) {
            _http = Http;
        }

        public string Select_ApicateID { get; set; }
        public I_ApiCateSet DocSet { get; set; }

        async public Task<I_ApiCateSet> GetDocSet(string docid) {
            docid = docid == null ? "" : docid;
            I_ApiCateSet doc = new I_ApiCateSet();
            try {
                var res = await _http.GetAsync($"api/ApiCate/GetDocSet?docid={docid}");
                doc = JsonSerializer.Deserialize<I_ApiCateSet>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            } catch (Exception ex) {
                var x = ex.Message;
            }
            return doc;
        }


        async public Task<I_ApiCateSet> SaveApiCate(I_ApiCateSet data) {
            I_ApiCateSet doc = new I_ApiCateSet();
            try {
                var res = await _http.GetAsync($"api/ApiCate/SaveApiCate");
                doc = JsonSerializer.Deserialize<I_ApiCateSet>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions {
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
                var res = await _http.GetAsync($"api/ApiCate/DeleteDoc?docid={docid}");
                result = JsonSerializer.Deserialize<I_BasicResult>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            } catch (Exception ex) {
                var x = ex.Message;
            }
            return result;
        }

        async public Task<I_ApiCateSet> GetLatestFiles(I_ApiCateSet f) {
            I_ApiCateSet? output = new I_ApiCateSet();
            try {
                string url = $"api/ApiCate/GetLatestFiles";
                var res = await _http.PostAsJsonAsync(url, f);
                output = JsonSerializer.Deserialize<I_ApiCateSet>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            } catch (Exception ex) {
                var x = ex.Message;
            }
            return output;
        }

        async public Task<I_BasicResult?> ReOrder(List<api_cate> data)
        {
            I_BasicResult? output = new I_BasicResult();
            try
            {
                string strPayload = JsonSerializer.Serialize(data);
                var response = await _http.PostAsJsonAsync($"api/ApiCate/ReOrder", strPayload);
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

        async public Task<List<vw_api_cate>> ListDoc(string? search) {
            List<vw_api_cate> output = new List<vw_api_cate>();
            try {
                var res = await _http.GetAsync($"api/ApiCate/ListDoc?search={search}");
                output = JsonSerializer.Deserialize<List<vw_api_cate>>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            } catch (Exception ex) {
                var x = ex.Message;
            }
            return output;
        }

        async public Task<List<api_cate>> ListApiCate()
        {
            List<api_cate> output = new List<api_cate>();
            try
            {
                var res = await _http.GetAsync($"api/ApiCate/ListApiCate");
                output = JsonSerializer.Deserialize<List<api_cate>>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions
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
                var res = await _http.GetAsync($"api/ApiCate/GenSort");
                output = JsonSerializer.Deserialize<short>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            } catch (Exception ex) {
                var x = ex.Message;
            }
            return output;
        }

        public static I_ApiCateSet NewTransaction() {
            I_ApiCateSet n = new I_ApiCateSet();
            n.head = NewHead();
            n.files = new List<vw_xfile_ref>();
            return n;
        }

        public static List<api_cate> ConvertViewToHead(List<vw_api_cate> input)
        {
            List<api_cate> result = new List<api_cate>();
            foreach (var l in input)
            {
                api_cate n = new api_cate();

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

        public static vw_api_cate NewHead() {
            vw_api_cate n = new vw_api_cate();

            n.cate_id = "";
            n.cate_name = "";
            n.page = "";
            n.img_path = "";
            n.sort = 0;
            n.is_active = 1;
            return n;
        }

    }
}
