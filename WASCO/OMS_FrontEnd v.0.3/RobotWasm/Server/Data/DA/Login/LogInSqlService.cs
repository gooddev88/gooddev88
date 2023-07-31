using Dapper;
using Microsoft.Data.SqlClient;
using RobotWasm.Server.Data.GaDB;
using RobotWasm.Server.Helper;
using RobotWasm.Shared.Data.ML.Login;
using System.Data;
using static RobotWasm.Shared.Data.ML.Shared.I_Result;
using RobotWasm.Shared.Data.GaDB;
using System.Text.Json;

namespace RobotWasm.Server.Data.DA.Login {
    public class LogInSqlService {
       

 
        #region List Data


    
        public static List<vw_PermissionInMenu> ListUserInMenuDisplay(List<vw_PermissionInMenu> input) {
            List<vw_PermissionInMenu> result = new List<vw_PermissionInMenu>();
            try {

                result = input.Where(o => o.IsOpen == true).GroupBy(x => new { x.Username, x.MenuID, x.GroupID, x.TypeID, x.MenuName, x.MenuDesc1, x.MenuDesc2, x.Url, x.Icon, x.GroupSort })
                                    .Select(r => new vw_PermissionInMenu {
                                        IsOpen = r.LastOrDefault().IsOpen,
                                        Username = r.Key.Username,
                                        MenuID = r.Key.MenuID,
                                        GroupID = r.Key.GroupID,
                                        TypeID = r.Key.TypeID,
                                        MenuName = r.Key.MenuName,
                                        MenuDesc1 = r.Key.MenuDesc1,
                                        MenuDesc2 = r.Key.MenuDesc2,
                                        Url = r.Key.Url,
                                        Icon = r.Key.Icon,
                                        //App = r.Key.App,
                                        GroupSort = r.Key.GroupSort
                                    }).ToList();

            } catch (Exception ex) {

            }
            return result;
        }


        public static List<vw_PermissionInBoard> ListUserInBoard(string username) {
            List<vw_PermissionInBoard> result = new List<vw_PermissionInBoard>();
            try {

            } catch (Exception ex) {
            }
            return result;
        }




        public static LoginSet NewLoginSet() {

            LoginSet n = new LoginSet();
            n.CurrentUser = "";
            n.CurrentUserInfo = new UserInfo();
            n.CurrentRootCompany = new CompanyInfo();
            n.CurrentCompany = new CompanyInfo();
            n.UserInRCompany = new List<string>();
            n.UserInCompany = new List<string>();
            n.CurrentTransactionDate = DateTime.Now.Date;
            n.UserInMenu = new List<vw_PermissionInMenu>();
            n.UserInMenuDisplay = new List<vw_PermissionInMenu>();
            n.UserInBoard = new List<vw_PermissionInBoard>();
            n.CurrentBoard = new vw_PermissionInBoard();
            n.UserMenu = new List<UserMenu>();
            n.AppLogoImage = "../../../Image/Logo/app_logo.png";
            n.BackgroundImage = "";
            n.LoginResult = "";
            n.LoginResultInfo = "";
            n.PageError = "";
            n.AppID = "";
            n.LogInByApp = "WEB";
            n.RefUserID = "";
            n.LatestPage = "Login";

            return n;
        }



        #endregion
        #region  mapping
       
        public static CompanyInfo CreateCompany(string r_comid, string comid) {
            CompanyInfo n = new CompanyInfo();
            n.RCompanyID = r_comid;
            n.CompanyID = comid;
            n.GroupCode = "";
            n.ComCode = "";
            n.BrnCode = "";
            n.ParentID = "";
            n.ShortCode = "";
            n.TypeID = "";
            n.Name1 = comid;
            n.Name2 = comid;
            //n.NameEn1 = "";
            //n.NameEn2 = "";
            n.BillAddr1 = "";
            n.BillAddr2 = "";
            n.TaxID = "";
            //n.TaxCalType = "";
            //n.TaxGroupS = "";
            //n.TaxGroupP = "";
            n.AddrFull = "";
            n.AddrFull2 = "";
            n.AddrNo = "";
            n.AddrTanon = "";
            n.AddrTumbon = "";
            n.AddrAmphoe = "";
            n.AddrProvince = "";
            n.AddrPostCode = "";
            n.AddrCountry = "";
            //n.Currency = "";
            n.Tel1 = "";
            n.Tel2 = "";
            n.Mobile = "";
            n.Email = "";
            n.Fax = "";
            n.Remark1 = "";
            n.Remark2 = "";
            n.IsWH = false;
            n.SalePersonID = "";
            n.CreatedBy = "";
            n.CreatedDate = DateTime.Now;
            n.ModifiedBy = "";
            n.ModifiedDate = null;
            n.IsActive = true;
            return n;

        }
        #endregion


        #region  login api
        public static LoginCrossReturn CreateCrossAppReq(LoginCrossRequest request) {
            LoginCrossReturn output = new LoginCrossReturn();
            try {
                using (GAEntities db = new GAEntities()) {
                    LogInCrossAppReq n = new LogInCrossAppReq {
                        ReqID = Guid.NewGuid().ToString(),
                        ReqTime = DateTime.Now,
                        AppID = request.AppID,
                        ExpiryTime = DateTime.Now.AddMinutes(60),
                        RComID = request.RComID,
                        Username = request.Username,
                        CallBackUrl = request.BackPage,
                        UseTime = null,
                        ForwardToUrl = request.ToPage
                    };

                    db.LogInCrossAppReq.Add(n);
                    db.SaveChanges();
                    output.Status = "ok";
                    output.ReqID = n.ReqID;
                    output.GoToUrl = Globals.BlazorBack_URL + $"/Login/LoginFromApp/{n.ReqID}";
                    output.Messeage = "";
                }
            } catch (Exception ex) {
                output.Status = "fail";
                if (ex.InnerException != null) {
                    output.Messeage = ex.InnerException.ToString();
                } else {
                    output.Messeage = ex.Message;
                }
            }
            return output;
        }

        public static LogInCrossAppReq GetCrossAppReq(string req_id) {
            LogInCrossAppReq output = new LogInCrossAppReq();
            try {
                using (GAEntities db = new GAEntities()) {
                    DateTime cur_datetime = DateTime.Now;
                    output = db.LogInCrossAppReq.Where(o => o.ReqID == req_id && o.ExpiryTime >= cur_datetime).FirstOrDefault();
                }
            } catch (Exception ex) { }
            return output;
        }

        async public static Task<string> CreateCrossToBackEnd(string rcom, string com, string username, string to_page) {
            string output = "";
            try {
                LoginCrossRequest data = new LoginCrossRequest();
                data.Username = username;
                data.RComID = rcom;
                data.AppID = "OMS";
                data.ToPage = to_page;
                data.BackPage = "";
                string url = $"{Globals.BlazorBack_URL}/api/AppsLogin";
                HttpClient http = new HttpClient();
                var response = await http.PostAsJsonAsync(url, data);
                var str_json = response.Content.ReadAsStringAsync().Result;

                if (response.StatusCode.ToString() != "OK") {
                    output = "error";
                    return output;
                }
                var result_api = JsonSerializer.Deserialize<LoginCrossReturn>(str_json);
                output = Globals.BlazorBack_URL + $"/Account/MyLogin/LoginFromApp?reqid={result_api.ReqID}";

            } catch (Exception ex) { }
            return output;
        }
        #endregion
    }
}
