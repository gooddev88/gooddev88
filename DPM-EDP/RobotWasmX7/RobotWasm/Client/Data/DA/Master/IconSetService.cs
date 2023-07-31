using RobotWasm.Shared.Data.ML.Shared;
using System.Text.Json;
using System.Text.Json.Serialization;
using RobotWasm.Shared.Data.ML.Master.IconSet;
using RobotWasm.Shared.Data.DimsDB;
using System.Net.Http.Json;
using static RobotWasm.Shared.Data.ML.Shared.I_Result;

namespace RobotWasm.Client.Data.DA.Master {
    public class IconSetService
    {
        private readonly HttpClient _http;
        public IconSetService(HttpClient Http) {
            _http = Http;
        }
   
        public I_IconSet DocSet { get; set; }

        async public Task<I_IconSet> GetDocSet(string docid) {
            docid = docid == null ? "" : docid;
            I_IconSet doc = new I_IconSet();
            try {
                var res = await _http.GetAsync($"api/IconSet/GetDocSet?docid={docid}");
                doc = JsonSerializer.Deserialize<I_IconSet>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            } catch (Exception ex) {
                var x = ex.Message;
            }
            return doc;
        }

        async public Task<I_BasicResult> Save(I_IconSet data,bool action)
        {
            I_BasicResult output = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try
            {
                string strPayload = JsonSerializer.Serialize(data);
                string url = $"api/IconSet/SaveIconSet?action={action}";
                var response = await _http.PostAsJsonAsync(url,strPayload);
                if (response.StatusCode.ToString().ToUpper() != "OK")
                {
                    output.Result = "fail";
                    output.Message1 = response.StatusCode.ToString();
                    return output;
                }
                output = response.Content.ReadFromJsonAsync<I_BasicResult>().Result;
            }
            catch (Exception ex)
            {
                var x = ex.Message;
            }
            return output;
        }

        //async public Task<I_Board_MasterSet> SaveBoardMaster(I_Board_MasterSet data,bool isnew) {
        //    I_Board_MasterSet doc = new I_Board_MasterSet();
        //    try {
        //        var res = await _http.GetAsync($"api/IconSet/SaveBoardMaster");
        //        doc = JsonSerializer.Deserialize<I_Board_MasterSet>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions {
        //            PropertyNameCaseInsensitive = true,
        //            ReferenceHandler = ReferenceHandler.Preserve
        //        });
        //    } catch (Exception ex) {
        //        var x = ex.Message;
        //    }
        //    return doc;
        //}

        async public Task<I_BasicResult> DeleteDoc(string docid,string modified_by) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                var res = await _http.GetAsync($"api/IconSet/DeleteDoc?docid={docid}");
                result = JsonSerializer.Deserialize<I_BasicResult>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            } catch (Exception ex) {
                var x = ex.Message;
            }
            return result;
        }

        async public Task<I_IconSet> GetLatestFiles(I_IconSet f)
        {
            I_IconSet? output = new I_IconSet();
            try
            {
                string url = $"api/IconSet/GetLatestFiles";
                var res = await _http.PostAsJsonAsync(url, f);
                output = JsonSerializer.Deserialize<I_IconSet>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions
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

        async public Task<List<vw_icon_set>> ListDoc(string? search)
        {
            List<vw_icon_set>? output = new List<vw_icon_set>();
            try
            {
                var res = await _http.GetAsync($"api/IconSet/ListDoc?search={search}");
                output = JsonSerializer.Deserialize<List<vw_icon_set>>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions
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
                var res = await _http.GetAsync($"api/IconSet/GenSort");
                output = JsonSerializer.Deserialize<short>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            } catch (Exception ex) {
                var x = ex.Message;
            }
            return output;
        }

        public static I_IconSet NewTransaction() {
            I_IconSet n = new I_IconSet();
            n.head = NewHead();
            n.files = new List<vw_xfile_ref>();
            return n;
        }

        public static icon_set NewHead() {
            icon_set n = new icon_set();

            n.icon_id = ""; 
            n.icon_name = "";
            n.sort = 0;
            n.is_active = 1;

            return n;
        }

    }
}
