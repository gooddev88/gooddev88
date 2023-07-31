using RobotWasm.Shared.Data.GaDB;
using System.Text.Json;
using System.Text.Json.Serialization;
using RobotWasm.Shared.Data.ML.Master.ComGroup;
using RobotWasm.Shared.Data.DimsDB;
using static RobotWasm.Shared.Data.ML.Shared.I_Result;
using System.Net.Http.Json;

namespace RobotWasm.Client.Data.DA.Master
{
    public class CompanyGroupService
    {

        private readonly HttpClient _http;
        public CompanyGroupService(HttpClient Http) {
            _http = Http;
        }
   
        public I_ComGroupSet DocSet { get; set; }

        async public Task<I_ComGroupSet> GetDocSetComGroup(string docid) {
            docid = docid == null ? "" : docid;
            I_ComGroupSet doc = new I_ComGroupSet();
            try {
                var res = await _http.GetAsync($"api/ComGroup/GetDocSetComGroup?docid={docid}");
                doc = JsonSerializer.Deserialize<I_ComGroupSet>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            } catch (Exception ex) {
                var x = ex.Message;
            }
            return doc;
        }


        async public Task<I_ComGroupSet> SaveComGroup(I_ComGroupSet data) {
            I_ComGroupSet doc = new I_ComGroupSet();
            try {
                var res = await _http.GetAsync($"api/ComGroup/SaveComGroup");
                doc = JsonSerializer.Deserialize<I_ComGroupSet>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            } catch (Exception ex) {
                var x = ex.Message;
            }
            return doc;
        }

        async public Task<I_BasicResult> DeleteDocComGroup(string docid, string modifyby) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                var res = await _http.GetAsync($"api/ComGroup/DeleteDocComGroup?docid={docid}&modifyby={modifyby}");
                result = JsonSerializer.Deserialize<I_BasicResult>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            } catch (Exception ex) {
                var x = ex.Message;
            }
            return result;
        }

        async public Task<company_group_info> Checkduplicate(string comgroupid)
        {
            company_group_info result = new company_group_info();
            try
            {
                var res = await _http.GetAsync($"api/ComGroup/Checkduplicate?comgroupid={comgroupid}");
                result = JsonSerializer.Deserialize<company_group_info>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions
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

        async public Task<List<company_group_info>> ListDoc(string? search) {
            List<company_group_info> output = new List<company_group_info>();
            try {
                var res = await _http.GetAsync($"api/ComGroup/ListDocHead?search={search}");
                output = JsonSerializer.Deserialize<List<company_group_info>>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            } catch (Exception ex) {
                var x = ex.Message;
            }
            return output;
        }

        public static I_ComGroupSet NewTransaction() {
            I_ComGroupSet n = new I_ComGroupSet();
            n.head = NewHead();
            return n;
        }

        public static company_group_info NewHead() {
            company_group_info n = new company_group_info();

            n.company_groupid = "";
            n.name1 = "";
            n.name2 = "";
            n.created_by = "";
            n.created_date = DateTime.Now;
            n.modified_by = "";
            n.modified_date = null;
            n.is_active = 1;
            return n;
        }

    }
}
