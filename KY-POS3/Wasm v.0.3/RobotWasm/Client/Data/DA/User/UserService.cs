using RobotWasm.Shared.Data.ML.Login;
using RobotWasm.Shared.Data.ML.Master.User;
using RobotWasm.Shared.Data.ML.Shared;
using RobotWasm.Shared.Data.GaDB;
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
        async public Task<I_UserSet> GetDocSet(string? username,string? rcom,string? userlogin) {
            username = username == null ? "" : username;
            rcom = rcom == null ? "" : rcom;
            userlogin = userlogin == null ? "" : userlogin;
            I_UserSet? doc = new I_UserSet();
            try {
                var res = await _http.GetAsync($"api/User/GetDocSet?username={username}&rcom={rcom}&userlogin={userlogin}");
                doc = JsonSerializer.Deserialize<I_UserSet>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            } catch (Exception ex) {
                var x = ex.Message;
            }
            return doc;
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

        async public Task<List<vw_UserInfo>> ListDoc(string? search,string? rcom) {
            List<vw_UserInfo> output = new List<vw_UserInfo>();
            try {
                var res = await _http.GetAsync($"api/User/ListDoc?search={search}&rcom={rcom}");
                output = JsonSerializer.Deserialize<List<vw_UserInfo>>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            } catch (Exception ex) {
                var x = ex.Message;
            }
            return output;
        }

        async public Task<UserInfo> GetUserInfo(string? username)
        {
            UserInfo? output = new UserInfo();
            try
            {
                var res = await _http.GetAsync($"api/User/GetUserInfo?username={username}");
                output = JsonSerializer.Deserialize<UserInfo>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions
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

        async public Task<IEnumerable<UserInfo>> ListUserInfo()
        {
            IEnumerable<UserInfo>? output = new List<UserInfo>();
            try
            {
                var res = await _http.GetAsync($"api/User/ListUserInfo");
                output = JsonSerializer.Deserialize<IEnumerable<UserInfo>>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions
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

        async public Task<I_BasicResult> Save(I_UserSet data) {
            I_BasicResult? output = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
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


        #region get menuinfo

        public static UserMenu GetMenuInfo(LoginSet xlogin, string menu) {
            UserMenu result = new UserMenu { MenuID = "", Name = "" };

            var menu_info = xlogin.UserInMenu.Where(o => o.MenuID == menu).FirstOrDefault();

            if (menu_info == null) {
                return result;
            }
            result.MenuID = menu_info.MenuID;
            result.Name = menu_info.MenuName;
            result.MenuCode = menu_info.MenuCode;
            result.Desc1 = menu_info.MenuDesc1;
            result.Desc2 = menu_info.MenuDesc2;
            result.GroupID = menu_info.GroupID;
            return result;
        }
        public static UserMenu GetMenuGroup(LoginSet xlogin, string group) {
            UserMenu result = new UserMenu { MenuID = "", Name = "" };

            var menu_info = xlogin.UserInMenu.Where(o => o.GroupID == group && o.TypeID == "MENUGROUP").FirstOrDefault();

            if (menu_info == null) {
                return result;
            }
            result.MenuID = menu_info.MenuID;
            result.Name = menu_info.MenuName;
            result.MenuCode = menu_info.MenuCode;
            result.Desc1 = menu_info.MenuDesc1;
            result.Desc2 = menu_info.MenuDesc2;
            return result;
        }

        #endregion


        public static I_UserSet NewTransaction() {
            I_UserSet n = new I_UserSet();
            n.User = NewUser();
            n.XGroup = new List<XUserInGroup>();
            n.XRcom = new List<XUserInRCom>();
            return n;
        }

        public static UserInfo NewUser() {
            UserInfo n = new UserInfo();

            n.Username = "";
            n.Password = "";
            n.EmpCode = "";
            n.FirstName = "";
            n.LastName = "";
            n.FullName = "";
            n.FirstName_En = "";
            n.LastName_En = "";
            n.FullName_En = "";
            n.NickName = "";
            n.Gender = "";
            n.DepartmentID = "";
            n.PositionID = "";
            n.IsProgramUser = false;
            n.IsNewUser = true;
            n.JobStartDate = null;
            n.ResignDate = null;
            n.AddrFull = "";
            n.AddrNo = "";
            n.AddrMoo = "";
            n.AddrTumbon = "";
            n.AddrAmphoe = "";
            n.AddrProvince = "";
            n.AddrPostCode = "";
            n.AddrCountry = "";
            n.Tel = "";
            n.Mobile = "";
            n.Email = "";
            n.Birthdate = null;
            n.MaritalStatus = null;
            n.CitizenId = null;
            n.BookBankNumber = null;
            n.ApproveBy = "";
            n.IsProgramUser = true;
            n.UseTimeStamp = false;
            n.ImageProfile = null;
            n.LineToken = null;
            n.DefaultCompany = null;
            n.CreatedBy = "";
            n.CreatedDate = DateTime.Now;
            n.ModifiedBy = "";
            n.ModifiedDate = null;
            n.IsActive = true;
            return n;
        }

    }
}
