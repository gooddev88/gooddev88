using RobotWasm.Shared.Data.DimsDB;
using RobotWasm.Shared.Data.ML.ApiMaster;
using RobotWasm.Shared.Data.ML.DPMBaord.BoardData;
using RobotWasm.Shared.Data.ML.Shared;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using static RobotWasm.Shared.Data.ML.Shared.I_Result;

namespace RobotWasm.Client.Data.DA.Data {
    public class ApiMasterService {
        private readonly HttpClient _http;
        public ApiMasterService(HttpClient Http) {
            _http = Http;
        }

        public I_ApiMasterSet DocSet { get; set; }

        async public Task<I_ApiMasterSet> GetDocSet(string docid) {
            docid = docid == null ? "" : docid;
            I_ApiMasterSet? doc = new I_ApiMasterSet();
            try {
                var res = await _http.GetAsync($"api/ApiMaster/GetDocSet?docid={docid}");
                doc = JsonSerializer.Deserialize<I_ApiMasterSet>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            } catch (Exception ex) {
                var x = ex.Message;
            }
            return doc;
        }


        async public Task<List<api_master>> ListApi(string search, string cate) {
            List<api_master> output = new List<api_master>();
            try {
                var res = await _http.GetAsync($"api/ApiMaster/ListApi?search={search}&cate={cate}");
                output = JsonSerializer.Deserialize<List<api_master>>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            } catch (Exception ex) {

                var x = ex.Message;
            }
            return output;
        }

        async public Task<List<api_master>> Listapi_master(string search) {
            List<api_master> output = new List<api_master>();
            try {
                var res = await _http.GetAsync($"api/ApiMaster/Listapi_master?search={search}");
                output = JsonSerializer.Deserialize<List<api_master>>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
                output = output.OrderBy(o => o.api_id).ToList();
            } catch (Exception ex) {

                var x = ex.Message;
            }
            return output;
        }

        async public Task<List<ApiCate>> ListApiCates() {
            List<ApiCate>? output = new List<ApiCate>();
            try {
                var res = await _http.GetAsync($"api/ApiMaster/ListDataCategory");
                output = JsonSerializer.Deserialize<List<ApiCate>>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            } catch (Exception ex) {

                var x = ex.Message;
            }
            return output;
        }

        async public Task<List<SelectOption>> ListCate() {
            List<SelectOption> output = new List<SelectOption>();
            try {
                var res = await _http.GetAsync($"api/ApiMaster/ListDataCategory");
                var cates = JsonSerializer.Deserialize<List<ApiCate>>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });

                var data = new List<SelectOption>();
                foreach (var c in cates) {
                    data.Add(new SelectOption { Value = c.cate_id, Description = c.cate_name, Sort = c.sort });
                }
                output = data;

            } catch (Exception ex) {

                var x = ex.Message;
            }
            return output;
        }

        async public Task<short> GenSort() {
            short output = 0;
            try {
                var res = await _http.GetAsync($"api/ApiMaster/GenSort");
                output = JsonSerializer.Deserialize<short>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            } catch (Exception ex) {
                var x = ex.Message;
            }
            return output;
        }

        async public Task<I_BasicResult?> ReOrder(List<api_param_res> data) {
            I_BasicResult? output = new I_BasicResult();
            try {
                string strPayload = JsonSerializer.Serialize(data);
                var response = await _http.PostAsJsonAsync($"api/ApiMaster/ReOrder", strPayload);
                output = JsonSerializer.Deserialize<I_BasicResult>(await response.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            } catch (Exception ex) {
                var x = ex.Message;
            } finally {
            }
            return output;
        }

        #region ApiiParam
        async public Task<I_BasicResult?> AddApiiParam(api_param_res widgets) {
            I_BasicResult? output = new I_BasicResult();
            try {
                string strPayload = JsonSerializer.Serialize(widgets);
                var response = await _http.PostAsJsonAsync($"api/ApiMaster/AddApiiParam", strPayload);

                output = JsonSerializer.Deserialize<I_BasicResult>(await response.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            } catch (Exception ex) {
                var x = ex.Message;
            } finally {
            }
            return output;
        }

        async public Task<I_BasicResult> DeleteApiParam(string ID) {
            ID = ID == null ? "0" : ID;
            I_BasicResult? output = new I_BasicResult();
            try {
                var res = await _http.GetAsync($"api/ApiMaster/DeleteApiParam?ID={ID}");
                output = JsonSerializer.Deserialize<I_BasicResult>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            } catch (Exception ex) {

                var x = ex.Message;
            }
            return output;
        }

        #endregion

        #region ApiiTag
        async public Task<I_BasicResult?> AddApiTag(api_tag widgets) {
            I_BasicResult? output = new I_BasicResult();
            try {
                string strPayload = JsonSerializer.Serialize(widgets);
                var response = await _http.PostAsJsonAsync($"api/ApiMaster/AddApiTag", strPayload);

                output = JsonSerializer.Deserialize<I_BasicResult>(await response.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            } catch (Exception ex) {
                var x = ex.Message;
            } finally {
            }
            return output;
        }

        async public Task<I_BasicResult> DeleteApiTag(string ID) {
            ID = ID == null ? "0" : ID;
            I_BasicResult? output = new I_BasicResult();
            try {
                var res = await _http.GetAsync($"api/ApiMaster/DeleteApiTag&{ID}");
                output = JsonSerializer.Deserialize<I_BasicResult>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            } catch (Exception ex) {

                var x = ex.Message;
            }
            return output;
        }

        #endregion

        async public Task<List<LISTSELECT_APIMasterALLCate>> ListAPIMasterALLCate()
        {
            List<LISTSELECT_APIMasterALLCate>? output = new List<LISTSELECT_APIMasterALLCate>();
            try
            {
                var res = await _http.GetAsync($"api/ApiMaster/ListAPIMasterALLCate");
                output = JsonSerializer.Deserialize<List<LISTSELECT_APIMasterALLCate>>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
                output = output.OrderBy(o => o.api_id).ToList();
            }
         
            catch (Exception ex)
            {

                var x = ex.Message;
            }
            return output;
        }

        async public Task<I_BasicResult?> UpdateCateApiByListApi(List<string> data,string apicate)
        {
            I_BasicResult? output = new I_BasicResult();
            try
            {
                string strPayload = JsonSerializer.Serialize(data);
                var response = await _http.PostAsJsonAsync($"api/ApiMaster/UpdateCateApiByListApi?apicate={apicate}", strPayload);

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


        #region List DataMaster

        public static List<SelectOption> ListMethod() {
            return new List<SelectOption>() {
                new SelectOption(){ Value= "GET", Description="GET" ,Sort = 1},
                new SelectOption(){ Value = "POST", Description="POST", Sort = 2},
            };
        }
        public static List<SelectOption> ListSourceCate() {
            return new List<SelectOption>() {

                new SelectOption(){ Value= "DPM PORTAL", Description="DPM PORTAL" ,Sort = 1},
                new SelectOption(){ Value = "EOC", Description="EOC", Sort = 2},
                new SelectOption(){ Value = "BUDGET PORTAL", Description="BUDGET PORTAL", Sort = 3},
                new SelectOption(){ Value = "TDA", Description="TDA", Sort = 4},
                new SelectOption(){ Value = "MEETING RESERVE", Description="MEETING RESERVE", Sort = 5},
            };
        }

        public static List<SelectOption> ListDataType() {
            return new List<SelectOption>() {
                new SelectOption(){ Value= "String", Description="String" ,Sort = 1},
                new SelectOption(){ Value = "Int", Description="Int", Sort = 2},
                new SelectOption(){ Value = "decimal", Description="decimal", Sort = 3},
                new SelectOption(){ Value = "object", Description="object", Sort = 4},
                new SelectOption(){ Value = "boolean", Description="boolean", Sort = 5},
            };
        }

        public static List<SelectOption> ListParamType() {
            return new List<SelectOption>() {
                new SelectOption(){ Value= "INPUT", Description="INPUT" ,Sort = 1},
                new SelectOption(){ Value = "OUTPUT", Description="OUTPUT", Sort = 2},
            };
        }

        #endregion

        #region New
        public static I_ApiMasterSet NewTransaction() {
            I_ApiMasterSet n = new I_ApiMasterSet();
            n.Head = NewHead();
            n.TagLine = new List<api_tag>();
            n.paramLine = new List<api_param_res>();
            return n;
        }

        public static api_master NewHead() {
            api_master n = new api_master();

            n.api_id = "";
            n.owner_code = "";
            n.source_connection_code = "";
            n.source_api_url = "";
            n.base_url = "";
            n.api_url = "";
            n.api_name = "";
            n.api_desc = "";
            n.api_type = "";
            n.method = "GET";
            n.version = "";
            n.authen = "";
            n.data_source = "";
            n.update_frequency = "";
            n.parameter_sample = "";
            n.output_sample = "";
            n.contact = "";
            n.cate = "";
            n.source_cate = "";
            n.url_page = "";
            n.remark = "";
            n.is_publish = 1;
            n.has_api = 1;
            n.is_active = 1;
            return n;
        }

        public static api_param_res NewParam() {
            api_param_res n = new api_param_res();

            n.api_id = "";
            n.field_id = "";
            n.param_type = "";
            n.description = "";
            n.data_type = "";
            n.sort = 0;
            n.is_require = 1;
            return n;
        }

        public static api_tag NewTag() {
            api_tag n = new api_tag();

            n.api_id = "";
            n.tag = "";
            n.is_active = 1;
            return n;
        }

        #endregion

    }
}
