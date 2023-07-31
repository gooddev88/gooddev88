using RobotWasm.Shared.Data.DimsDB;
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


        async public Task<I_userGroupSet> GetDocSet(string? groupid) {
            groupid = groupid == null ? "" : groupid;
            I_userGroupSet doc = new I_userGroupSet();
            try {
                var res = await _http.GetAsync($"api/UserGroup/GetDocSet?groupid={groupid}");
                doc = JsonSerializer.Deserialize<I_userGroupSet>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            } catch (Exception ex) {
                var x = ex.Message;
            }
            return doc;
        }

        async public Task<List<usergroup_info>> ListDoc(string? search) {
            List<usergroup_info> output = new List<usergroup_info>();
            try {
                var res = await _http.GetAsync($"api/UserGroup/ListDoc?search={search}");
                output = JsonSerializer.Deserialize<List<usergroup_info>>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions {
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

        async public Task<I_BasicResult> DeleteUserGroup(string usergroup)
        {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try
            {
                var res = await _http.GetAsync($"api/UserGroup/DeleteUserGroup?usergroup={usergroup}");
                result = JsonSerializer.Deserialize<I_BasicResult>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions
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

        #region get menuinfo

        public static usermenu GetMenuInfo(LoginSet xlogin, string menu) {
            usermenu result = new usermenu { menu_id = "", menu_name = "" };

            var menu_info = xlogin.UserInMenu.Where(o => o.MenuID == menu).FirstOrDefault();

            if (menu_info == null) {
                return result;
            }
            result.menu_id = menu_info.MenuID;
            result.menu_name = menu_info.MenuName;
            result.menu_code = menu_info.MenuCode;
            result.menu_description1 = menu_info.MenuDesc1;
            result.menu_description2 = menu_info.MenuDesc2;
            result.group_id = menu_info.GroupID;
            return result;
        }
        public static usermenu GetMenuGroup(LoginSet xlogin, string group) {
            usermenu result = new usermenu { menu_id = "", menu_name = "" };

            var menu_info = xlogin.UserInMenu.Where(o => o.GroupID == group && o.TypeID == "MENUGROUP").FirstOrDefault();

            if (menu_info == null) {
                return result;
            }
            result.menu_id = menu_info.MenuID;
            result.menu_name = menu_info.MenuName;
            result.menu_code = menu_info.MenuCode;
            result.menu_description1 = menu_info.MenuDesc1;
            result.menu_description2 = menu_info.MenuDesc2;
            return result;
        }

        #endregion


        public static I_userGroupSet NewTransaction() {
            I_userGroupSet n = new I_userGroupSet();
            n.Group = NewUsergroup();
            n.UserInGroup = new List<vw_user_in_group>();
            n.XMenu = new List<xusergroup_in_menu>();
            n.XApi = new List<xapi_master>();
            n.XBoard = new List<xusergroup_in_board>();
            return n;
        }

        public static usergroup_info NewUsergroup() {
            usergroup_info n = new usergroup_info();
            n.group_id = "";
            n.group_name = "";
            n.group_description = "";
            n.group_sort = 0;
            n.created_by = "";
            n.created_date = DateTime.Now;
            n.modified_by = "";
            n.modified_date = null;
            n.is_active = 1;
            n.count_user = 0;
            return n;
        }

    }
}
