using RobotWasm.Shared.Data.GaDB;
using System.Text.Json;
using System.Text.Json.Serialization;
using RobotWasm.Shared.Data.ML.Master.Company;
using RobotWasm.Shared.Data.DimsDB;
using static RobotWasm.Shared.Data.ML.Shared.I_Result;
using System.Net.Http.Json;

namespace RobotWasm.Client.Data.DA.Master
{
    public class CompanyService
    {

        private readonly HttpClient _http;
        public CompanyService(HttpClient Http) {
            _http = Http;
        }
   
        public I_CompanySet DocSet { get; set; }

        async public Task<I_CompanySet> GetDocSet(string docid) {
            docid = docid == null ? "" : docid;
            I_CompanySet doc = new I_CompanySet();
            try {
                var res = await _http.GetAsync($"api/Company/GetDocSet?docid={docid}");
                doc = JsonSerializer.Deserialize<I_CompanySet>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            } catch (Exception ex) {
                var x = ex.Message;
            }
            return doc;
        }


        async public Task<I_CompanySet> SaveCompany(I_CompanySet data) {
            I_CompanySet doc = new I_CompanySet();
            try {
                var res = await _http.GetAsync($"api/Company/SaveCompany");
                doc = JsonSerializer.Deserialize<I_CompanySet>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            } catch (Exception ex) {
                var x = ex.Message;
            }
            return doc;
        }

        async public Task<I_BasicResult> DeleteDoc(string docid, string modifyby) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                var res = await _http.GetAsync($"api/Company/DeleteDoc?docid={docid}&modifyby={modifyby}");
                result = JsonSerializer.Deserialize<I_BasicResult>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            } catch (Exception ex) {
                var x = ex.Message;
            }
            return result;
        }

        async public Task<company_info> Checkduplicate(string comid)
        {
            company_info result = new company_info();
            try
            {
                var res = await _http.GetAsync($"api/Company/Checkduplicate?comid={comid}");
                result = JsonSerializer.Deserialize<company_info>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions
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

        async public Task<List<company_info>> ListDoc(string? search) {
            List<company_info>? output = new List<company_info>();
            try {
                var res = await _http.GetAsync($"api/Company/ListDoc?search={search}");
                output = JsonSerializer.Deserialize<List<company_info>>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            } catch (Exception ex) {
                var x = ex.Message;
            }
            return output;
        }

        async public Task<List<mas_province>> ListProvince()
        {
            List<mas_province> output = new List<mas_province>();
            try
            {
                var res = await _http.GetAsync($"api/Company/ListProvince");
                output = JsonSerializer.Deserialize<List<mas_province>>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions
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

        async public Task<List<company_group_info>> ListGroupCompany()
        {
            List<company_group_info> output = new List<company_group_info>();
            try
            {
                var res = await _http.GetAsync($"api/Company/ListGroupCompany");
                output = JsonSerializer.Deserialize<List<company_group_info>>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions
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

        async public Task<List<company_info>> ListCompany()
        {
            List<company_info> output = new List<company_info>();
            try
            {
                var res = await _http.GetAsync($"api/Company/ListCompany");
                output = JsonSerializer.Deserialize<List<company_info>>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions
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

        public static I_CompanySet NewTransaction() {
            I_CompanySet n = new I_CompanySet();
            n.head = NewHead();
            return n;
        }

        public static company_info NewHead() {
            company_info n = new company_info();

            n.companyid = "";
            n.comgroupid = "";
            n.name1 = "";
            n.name2 = "";
            n.province = "";
            n.created_by = "";
            n.created_date = DateTime.Now;
            n.modified_by = "";
            n.modified_date = null;
            n.is_active = 1;
            return n;
        }

    }
}
