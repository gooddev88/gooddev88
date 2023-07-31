using RobotWasm.Shared.Data.GaDB;
using System.Text.Json;
using System.Text.Json.Serialization;
using RobotWasm.Shared.Data.ML.Master.News;
using RobotWasm.Shared.Data.DimsDB;
using static RobotWasm.Shared.Data.ML.Shared.I_Result;
using System.Net.Http.Json;

namespace RobotWasm.Client.Data.DA.Master {
    public class NewsService {

        private readonly HttpClient _http;
        public NewsService(HttpClient Http) {
            _http = Http;
        }
   
        public I_NewsSet DocSet { get; set; }

        async public Task<I_NewsSet> GetDocSet(string docid) {
            docid = docid == null ? "" : docid;
            I_NewsSet doc = new I_NewsSet();
            try {
                var res = await _http.GetAsync($"api/News/GetDocSet?docid={docid}");
                doc = JsonSerializer.Deserialize<I_NewsSet>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            } catch (Exception ex) {
                var x = ex.Message;
            }
            return doc;
        }


        async public Task<I_NewsSet> SaveNews(I_NewsSet data) {
            I_NewsSet doc = new I_NewsSet();
            try {
                var res = await _http.GetAsync($"api/News/SaveNews");
                doc = JsonSerializer.Deserialize<I_NewsSet>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions {
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
                var res = await _http.GetAsync($"api/News/DeleteDoc?docid={docid}");
                result = JsonSerializer.Deserialize<I_BasicResult>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            } catch (Exception ex) {
                var x = ex.Message;
            }
            return result;
        }

        //async public Task<I_ApiCateSet> GetLatestFiles(I_ApiCateSet f) {
        //    I_ApiCateSet? output = new I_ApiCateSet();
        //    try {
        //        string url = $"api/ApiCate/GetLatestFiles";
        //        var res = await _http.PostAsJsonAsync(url, f);
        //        output = JsonSerializer.Deserialize<I_ApiCateSet>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions {
        //            PropertyNameCaseInsensitive = true,
        //            ReferenceHandler = ReferenceHandler.Preserve
        //        });
        //    } catch (Exception ex) {
        //        var x = ex.Message;
        //    }
        //    return output;
        //}

        async public Task<List<news_info>> ListDoc(string? search) {
            List<news_info> output = new List<news_info>();
            try {
                var res = await _http.GetAsync($"api/News/ListDoc?search={search}");
                output = JsonSerializer.Deserialize<List<news_info>>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            } catch (Exception ex) {
                var x = ex.Message;
            }
            return output;
        }

        public static I_NewsSet NewTransaction() {
            I_NewsSet n = new I_NewsSet();
            n.head = NewHead();
            n.files = new List<vw_xfile_ref>();
            return n;
        }

        public static news_info NewHead() {
            news_info n = new news_info();

            n.newid = "";
            n.title = "";
            n.desc = "";
            n.newdate = DateTime.Now.Date;
            n.is_active = 1;
            return n;
        }

    }
}
