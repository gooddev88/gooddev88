using RobotWasm.Shared.Data.GaDB;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Net.Http.Json;
using RobotWasm.Shared.Data.ML.Login;
using RobotWasm.Shared.Data.ML.Master.Company;
using RobotWasm.Shared.Data.ML.Param;

namespace RobotWasm.Client.Data.DA.Master {
    public class CompanyService {

        private readonly HttpClient _http;
        public CompanyService(HttpClient Http) {
            _http = Http;
        }

        #region GetData

        async public Task<CompanyInfo> GetComInfoByComID(string? rcom,string? comId) {
            rcom = rcom == null ? "" : rcom;
            comId = comId == null ? "" : comId;
            CompanyInfo doc = new CompanyInfo();
            try {
                var res = await _http.GetAsync($"api/Company/GetComInfoByComID?rcom={rcom}&comId={comId}");
                doc = JsonSerializer.Deserialize<CompanyInfo>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            } catch (Exception ex) {
                var x = ex.Message;
            }
            return doc;
        }

        async public Task<LocationInfo> GetLocationInfoByLocID(string? rcom, string? comId,string? locid) {
            rcom = rcom == null ? "" : rcom;
            comId = comId == null ? "" : comId;
            locid = locid == null ? "" : locid;
            LocationInfo doc = new LocationInfo();
            try {
                var res = await _http.GetAsync($"api/Company/GetLocationInfoByLocID?rcom={rcom}&comId={comId}&locid={locid}");
                doc = JsonSerializer.Deserialize<LocationInfo>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            } catch (Exception ex) {
                var x = ex.Message;
            }
            return doc;
        }

        async public Task<List<LocationInfo>> ListLocactionsByUser(string rcom, string? com, string user) {
            List<LocationInfo>? doc = new List<LocationInfo>();
            try {
                LocParam p = new LocParam { RCom = rcom, Com = com ,User=user};
                string strPayload = JsonSerializer.Serialize(p);
                string url = $"api/Company/ListLocationsByUser";
                var response = await _http.PostAsJsonAsync(url, strPayload);
                var status = response.StatusCode;

                if (status.ToString().ToLower() != "ok") {
                } else {
                    doc = JsonSerializer.Deserialize<List<LocationInfo>>(await response.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                        PropertyNameCaseInsensitive = true,
                        ReferenceHandler = ReferenceHandler.Preserve
                    });
             
                }
            } catch (Exception ex) {
                var x = ex.Message;
            }
            return doc;
        }

        async public Task<CompanyInfo> GetComInfoByRComID(string? comId) {
            comId = comId == null ? "" : comId;
            CompanyInfo doc = new CompanyInfo();
            try {
                var res = await _http.GetAsync($"api/Company/GetComInfoByRComID?comId={comId}");
                doc = JsonSerializer.Deserialize<CompanyInfo>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            } catch (Exception ex) {
                var x = ex.Message;
            }
            return doc;
        }

        async public Task<List<CompanyInfoList>> ListCompanyInfo(string type,bool addShowAll) {
            List<CompanyInfoList> doc = new List<CompanyInfoList>();
            try {
                var res = await _http.GetAsync($"api/Company/ListCompanyInfo?type={type}&addShowAll={addShowAll}");
                doc = JsonSerializer.Deserialize<List<CompanyInfoList>>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            } catch (Exception ex) {
                var x = ex.Message;
            }
            return doc;
        }

        async public Task<List<CompanyInfoList>> ListCompanyInfoUIC(LoginSet data, string? type, bool addShowAll) {
            List<CompanyInfoList>? doc = new List<CompanyInfoList>();
            try {
                string strPayload = JsonSerializer.Serialize(data);
                string url = $"api/Company/ListCompanyInfoUIC?type={type}&addShowAll={addShowAll}";
                var response = await _http.PostAsJsonAsync(url,strPayload);
                var status = response.StatusCode;

                if (status.ToString().ToLower() != "ok") {
                } else {
                    doc = JsonSerializer.Deserialize<List<CompanyInfoList>>(await response.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                        PropertyNameCaseInsensitive = true,
                        ReferenceHandler = ReferenceHandler.Preserve
                    });
                }
            } catch (Exception ex) {
                var x = ex.Message;
            }
            return doc;
        }




        public async Task<CompanyInfo> GetCompanyInfo(string? rcom, string? comId) {
            rcom = rcom == null ? "" : rcom;
            comId = comId == null ? "" : comId;
            CompanyInfo doc = new CompanyInfo();
            try {
                var res = await _http.GetAsync($"api/Company/GetCompanyInfo?rcom={rcom}&comId={comId}");
                doc = JsonSerializer.Deserialize<CompanyInfo>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            } catch (Exception ex) {
                var x = ex.Message;
            }
            return doc;
        }
        
       







        #endregion

    }
}
