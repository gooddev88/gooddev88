using RobotWasm.Shared.Data.DimsDB;
using RobotWasm.Shared.Data.GaDB;
using RobotWasm.Shared.Data.ML.Login;
using RobotWasm.Shared.Data.ML.Master.User;
using RobotWasm.Shared.Data.ML.Shared;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using static RobotWasm.Shared.Data.ML.Master.User.I_UserSet;
using static RobotWasm.Shared.Data.ML.Shared.I_Result;
using static System.Net.WebRequestMethods;

namespace RobotWasm.Client.Data.DA.UserGroup {
    public class UserService {

        private readonly HttpClient _http;
        public UserService(HttpClient Http) {
            _http = Http;
        }

        public I_UserSet DocSet { get; set; }

        async public Task<I_UserSet> GetDocSet(string? username) {
            username = username == null ? "" : username;
            I_UserSet doc = new I_UserSet();
            try {
                var res = await _http.GetAsync($"api/User/GetDocSet?username={username}");
                doc = JsonSerializer.Deserialize<I_UserSet>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            } catch (Exception ex) {
                var x = ex.Message;
            }
            return doc;
        }
        async public Task<List<user_info>> ListDoc(string? search) {
            List<user_info> output = new List<user_info>();
            try {
                var res = await _http.GetAsync($"api/User/ListDoc?search={search}");
                output = JsonSerializer.Deserialize<List<user_info>>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            } catch (Exception ex) {
                var x = ex.Message;
            }
            return output;
        }
        async public Task<I_BasicResult> Save(I_UserSet data) {
            I_BasicResult output = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                string strPayload = JsonSerializer.Serialize(data);
                string url = $"api/User/Save";
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

        async public Task<I_BasicResult> ReSetPassword(I_UserSet data)
        {
            I_BasicResult output = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try
            {
                string strPayload = JsonSerializer.Serialize(data);
                string url = $"api/User/ReSetPassword";
                var response = await _http.PostAsJsonAsync(url, strPayload);
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
            result.is_open = Convert.ToInt32(menu_info.IsOpen);
            result.is_create = Convert.ToInt32(menu_info.IsCreate);
            result.is_edit = Convert.ToInt32(menu_info.IsEdit);
            result.is_delete = Convert.ToInt32(menu_info.IsDelete);
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

        async public Task<I_BasicResult> DeleteUser(string username)
        {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try
            {
                var res = await _http.GetAsync($"api/User/DeleteUser?username={username}");
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

        async public Task<I_BasicResult> UpdatePasswordNew(string? username,string? password,string? newPassword)
        {
            username = username == null ? "" : username;
            password = password == null ? "" : password;
            newPassword = newPassword == null ? "" : newPassword;
            I_BasicResult? result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try
            {
                var res = await _http.GetAsync($"api/User/UpdatePasswordNew?username={username}&password={password}&newPassword={newPassword}");
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

        public static I_UserSet NewTransaction() {
            I_UserSet n = new I_UserSet();
            n.User = NewUser();
            n.XGroup = new List<xuser_in_group>();
            return n;
        }

        public static user_info NewUser() {
            user_info n = new user_info();
            n.username = "";
            n.password = "";
            n.first_name = "";
            n.last_name = "";
            n.comgroupid = "";
            n.companyid = "";
            n.email = "";
            n.api_token = "";
            n.created_by = "";
            n.created_date = DateTime.Now;
            n.modified_by = "";
            n.modified_date = null;
            n.is_newuser = 0;
            n.is_active = 1;
            return n;
        }

    }
}
