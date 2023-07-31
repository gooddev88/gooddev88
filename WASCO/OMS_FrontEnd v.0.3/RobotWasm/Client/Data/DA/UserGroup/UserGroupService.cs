using RobotWasm.Shared.Data.GaDB;
using RobotWasm.Shared.Data.ML.Login;
using RobotWasm.Shared.Data.ML.Master;
using RobotWasm.Shared.Data.ML.Master.UserGroup;
using RobotWasm.Shared.Data.ML.Shared;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using static RobotWasm.Shared.Data.ML.Master.UserGroup.I_userGroupSet;
using static RobotWasm.Shared.Data.ML.Shared.I_Result;
using static System.Net.WebRequestMethods;

namespace RobotWasm.Client.Data.DA.UserGroup {
    public class UserGroupService {

        private readonly HttpClient _http;
        public UserGroupService(HttpClient Http) {
            _http = Http;
        }


        public I_userGroupSet DocSet { get; set; }


        async public Task<I_userGroupSet> GetDocSet(string? groupid, string? rcom) {
            groupid = groupid == null ? "" : groupid;
            rcom = rcom == null ? "" : rcom;
            I_userGroupSet doc = new I_userGroupSet();
            try {
                var res = await _http.GetAsync($"api/UserGroup/GetDocSet?groupid={groupid}&rcom={rcom}");
                doc = JsonSerializer.Deserialize<I_userGroupSet>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            } catch (Exception ex) {
                var x = ex.Message;
            }
            return doc;
        }

        async public Task<List<UserGroupInfo>> ListDoc(string? search,string? rcom) {
            rcom = rcom == null ? "" : rcom;
            List<UserGroupInfo> output = new List<UserGroupInfo>();
            try {
                var res = await _http.GetAsync($"api/UserGroup/ListDoc?search={search}&rcom={rcom}");
                output = JsonSerializer.Deserialize<List<UserGroupInfo>>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions {
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
                var res = await _http.GetAsync($"api/UserGroup/GenSort");
                output = JsonSerializer.Deserialize<short>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            } catch (Exception ex) {
                var x = ex.Message;
            }
            return output;
        }

        async public Task<I_BasicResult> Save(I_userGroupSet data) {
            I_BasicResult output = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                string strPayload = JsonSerializer.Serialize(data);
                string url = $"api/UserGroup/Save";
                var response = await _http.PostAsJsonAsync(url, strPayload);
                if (response.StatusCode.ToString().ToUpper()!="OK") {
                    output.Result = "fail";
                    output.Message1=response.StatusCode.ToString();
                    return output;
                }
                output = response.Content.ReadFromJsonAsync<I_BasicResult>().Result;
               
                 
            } catch (Exception ex) {
                var x = ex.Message;
            }
            return output;
        }



        //public static List<SelectOption> ListLevelRisk() {
        //    return new List<SelectOption>() {
        //        new SelectOption(){ Value= "", Description="ไม่ระบุ" ,Sort = 1},
        //        new SelectOption(){ Value = "ความเสี่ยงสูงมาก", Description="ความเสี่ยงสูงมาก", Sort = 2},
        //        new SelectOption(){ Value = "ความเสี่ยงสูง", Description="ความเสี่ยงสูง", Sort = 3},
        //        new SelectOption(){ Value = "ความเสี่ยงปานกลาง", Description="ความเสี่ยงปานกลาง", Sort = 4},
        //        new SelectOption(){ Value = "ความเสี่ยงต่ำ", Description="ความเสี่ยงต่ำ", Sort = 5},
        //        new SelectOption(){ Value = "ความเสี่ยงต่ำมาก", Description="ความเสี่ยงต่ำมาก", Sort = 6},
        //    };
        //}

        public static I_userGroupSet NewTransaction(string rcom) {
            I_userGroupSet n = new I_userGroupSet();
            n.Group = NewUsergroup(rcom);
            n.XMenu = new List<XMenu>();
            n.XBoard = new List<XUserGroupInBoard>();
            n.XCompany = new List<XUserGroupInCompany>();
            return n;
        }

        public static UserGroupInfo NewUsergroup(string rcom) {
            UserGroupInfo n = new UserGroupInfo();
            n.RComID = rcom;
            n.UserGroupID = "";
            n.GroupName = "";
            n.LineToken = "";
            n.Sort = 0;
            n.CreatedBy = "";
            n.CreatedDate = DateTime.Now;
            n.ModifiedBy = "";
            n.ModifiedDate = null;
            n.IsActive = true;
            return n;
        }

    }
}
