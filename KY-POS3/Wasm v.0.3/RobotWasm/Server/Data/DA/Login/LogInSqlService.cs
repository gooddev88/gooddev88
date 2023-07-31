using Dapper;
using Microsoft.Data.SqlClient;
using RobotWasm.Server.Helper;
using RobotWasm.Shared.Data.ML.Login;
using System.Data;
using static RobotWasm.Shared.Data.ML.Shared.I_Result;
using RobotWasm.Shared.Data.GaDB;

namespace RobotWasm.Server.Data.DA.Login {
    public class LogInSqlService {
        //public static IDbConnection Connection {
        //    get {
        //        return new NpgsqlConnection((Globals.CimsConn));
        //    }
        //}


        public static LoginSet Login(string username, string password, string appid, string rcom = "") {
            //password ="hhhhhHhhhh" คือ login โดยไม่ต้องมี Username (hack mode) 
            // LoginSet LoginInfo = new LoginSet();
            var loginInfo = NewLoginSet();
            loginInfo.LoginResult = "ok";
            string encrypt_password = Hash.hashPassword("MD5", password);
            string fixRcom = "DPM";
            string fixCom = "DPM";
            username = username.ToLower();
            try {
                using (TFEDBFContext db = new TFEDBFContext()) {

                    var query = db.y_user_info.Where(o => o.username.ToLower() == username   && o.is_active == true).FirstOrDefault();
                  
                    if (query == null) {//login success  
                        loginInfo.LoginResult = "fail";
                        loginInfo.LoginResultInfo = "Incorrect username or password";
                        CreateLoginLog(username, false);
                        return loginInfo;
                    }

                    loginInfo.CurrentUserInfo = MappingUserInfo(query);
                    if (password != "silent") {//ไม่ใช่ hack mode
                        //login by db
                        if (query.password != encrypt_password) {
                            loginInfo.LoginResult = "fail";
                            loginInfo.LoginResultInfo = "Incorrect username or password.";
                            CreateLoginLog(username, false);
                            return loginInfo;
                        }
                    }
                    //  LoginInfo.CurrentRootCompany = ListUserInRCompany(query.Username, rcom).FirstOrDefault();
                    //loginInfo.UserInRCompany = new List<string> { "DPM" };
                    //if (loginInfo.UserInRCompany.Count == 0) {
                    //    loginInfo.LoginResult = "fail";
                    //    loginInfo.LoginResultInfo = "No setup root company";
                    //    CreateLoginLog(username, false);
                    //    return loginInfo;
                    //}
                    //var currRcomID = loginInfo.UserInRCompany.FirstOrDefault();
                   
                 
                    //var rcom_Info = new List<string> { fixRcom };
                    //loginInfo.CurrentRootCompany = CreateCompany(fixRcom,fixRcom); 
                    //loginInfo.UserInCompany = new List<string> {fixCom }; 
                    //var first_com = loginInfo.UserInCompany.FirstOrDefault();
        
                    //loginInfo.CurrentCompany = CreateCompany(fixRcom, fixRcom);


                    loginInfo.LoginResult = "ok";
                    if (query.is_newuser == 1) {
                        loginInfo.LoginResult = "new";
                    }

                    loginInfo.CurrentUser = query.username;
                    loginInfo.CurrentTransactionDate = DateTime.Now.Date;
                    loginInfo.UserInMenu = ListUserInMenu(  query.username);
                    loginInfo.UserInMenuDisplay = ListUserInMenuDisplay(loginInfo.UserInMenu);
                    //loginInfo.UserInBoard = ListUserInBoard(query.username);
                    //loginInfo.CurrentBoard = loginInfo.UserInBoard.OrderBy(o => o.BoardSort).FirstOrDefault();

                    loginInfo.CurrentMacNo = "Z";
                    loginInfo.CurrentVatRate = 7;
                    loginInfo.BackgroundImage = "";
                    CreateLoginLog(username, true);
                }
            } catch (Exception ex) {
                loginInfo.LoginResult = "fail";

                if (ex.InnerException != null) {
                    loginInfo.LoginResultInfo = ex.InnerException.ToString();
                } else {
                    loginInfo.LoginResultInfo = ex.Message.ToString();
                }

            }
            return loginInfo;
        }

        public static I_BasicResult CreateLoginLog(string user,bool issuccess) {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                y_trans_logs n = new y_trans_logs();
                n.module = "login";
                n.app_id = "asset";
                n.doc_id = user;
                n.username = user;
                n.action = issuccess ? "login success" : "login fail";
                n.log_date = DateTime.Now;
                n.log_desc = $"user {user} {n.action} เมื่อ {Convert.ToDateTime(n.log_date).ToString("dd/MM/yyyy HH:mm:ss")}";


                r = LogTranService.CreateLog(n);
            } catch (Exception ex) {
                r.Result = "fail";
                r.Message1 = ex.Message;
            }
            return r;
        }

        #region  ChangePassword

        public static I_BasicResult UpdatePasswordNew(string username, string password, string newPassword) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                using (TFEDBFContext db = new TFEDBFContext()) {
                    var u = db.y_user_info.Where(o => o.username == username).FirstOrDefault();
                    if (u == null) {
                        result.Result = "fail";
                        result.Message1 = "ไม่พบผู้ใช้งาน";
                    } else {
                        if (u.password != password) {
                            result.Result = "fail";
                            result.Message1 = "รหัสผ่านไม่ถูกต้อง";
                        } else {
                            u.password = newPassword;
                            u.is_newuser = 0;
                            db.SaveChanges();
                        }
                    }
                }
            } catch (Exception ex) {
                result.Result = "fail";
                if (ex.InnerException != null) {
                    result.Message1 = ex.InnerException.ToString();
                } else {
                    result.Message1 = ex.Message;
                }
            }
            return result;
        }
        #endregion
        #region List Data
  

        public static List<string> ListUserInCompany(string username, string rcom) {

            List<string> result = new List<string>();
            try {
                string conStr = Globals.TFEDBFConn;
                using (var connection = new SqlConnection(conStr)) {
                    result.Add("");
                }
            } catch (Exception ex) {

            }
            return result;
        }

        public static List<vw_PermissionInMenu> ListUserInMenu(string username) {
            List<vw_PermissionInMenu> result = new List<vw_PermissionInMenu>();
            try {
                using (SqlConnection conn = new SqlConnection(Globals.TFEDBFConn)) {
               
                    conn.Open();
                    string sql = @"
                        select * from vw_permission_in_menu
                        where username = @username  
                        order by group_sort,menu_sort
                    ";
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("username", username); 
                    var query = conn.Query<vw_permission_in_menu>(sql, dynamicParameters).ToList();
                    foreach (var q in query) {
                        result.Add(MappingPermissionMenu(q));
                    } 
                }
                    
            } catch (Exception ex) {

            }
            return result;
        }
        public static List<vw_PermissionInMenu> ListUserInMenuDisplay(List<vw_PermissionInMenu> input) {
            List<vw_PermissionInMenu> result = new List<vw_PermissionInMenu>();
            try {

                result = input.Where(o => o.IsOpen == true).GroupBy(x => new { x.Username, x.MenuID, x.GroupID, x.TypeID, x.MenuName, x.MenuDesc1, x.MenuDesc2, x.Url, x.Icon, x.GroupSort,x.App })
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
                                        App = r.Key.App,
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
        public static UserInfo MappingUserInfo(y_user_info x) {
            UserInfo n = new UserInfo();
            n.Username = x.username;
            n.Password = x.password;
            n.FirstName = x.first_name;
            n.LastName = x.last_name;
            n.FullName = x.first_name + " " + x.last_name;
            n.IsNewUser = Convert.ToBoolean( x.is_newuser); 
            n.Email = x.email;
            n.JwtToken = x.api_token;
            n.CreatedBy = x.created_by;
            n.CreatedDate = x.created_date;
            n.ModifiedBy = x.modified_by;
            n.ModifiedDate = x.modified_date;
            n.IsActive = Convert.ToBoolean(x.is_active) ;
            return n;
        }
      public static vw_PermissionInMenu MappingPermissionMenu(vw_permission_in_menu x) {
            vw_PermissionInMenu n = new vw_PermissionInMenu();
            n.RN = Convert.ToInt32(x.rowkey);
            n.RComID = "";
            n.Username = x.username;
            n.FullName = x.full_name;
            n.App = x.app_id;
            n.TypeID = x.menu_type;
            n.MenuID = x.menu_id;
            n.MenuName = x.menu_name;
            n.MenuDesc1 = x.menudesc1;
            n.MenuDesc2 = x.menudesc2;
            n.MenuType = x.menu_type;
            n.MenuCode = x.menu_code;
            n.GroupID = x.menu_group;
            n.GroupSort = Convert.ToInt32( x.group_sort);
            n.SubGroupID = "";
            n.SubGroupSort = 0;
            n.Url = x.url;
            n.Icon = x.icon;
            n.IsToggleGroupmenu = false;
            n.DataSource = x.data_source;
            n.IsOpen = Convert.ToBoolean( x.is_open);
            n.NeedOpenPermission = Convert.ToBoolean(x.need_open);
            n.IsCreate = Convert.ToBoolean(x.is_create);
            n.NeedCreatePermission = Convert.ToBoolean(x.need_create);
            n.IsEdit = Convert.ToBoolean(x.is_edit);
            n.NeedEditPermission = Convert.ToBoolean(x.need_edit);
            n.IsDelete = Convert.ToBoolean(x.is_delete);
            n.NeedDeletePermission = Convert.ToBoolean(x.need_delete);
            n.IsPrint = Convert.ToBoolean(x.is_print);
            n.NeedPrintPermission = Convert.ToBoolean(x.need_print);
            n.CaptionOpenPermission = x.text_open;
            n.CaptionCreatePermission = x.text_create;
            n.CaptionEditPermission = x.text_edit;
            n.CaptionDeletePermission = x.text_delete;
            n.CaptionPrintPermission = x.text_print;
            return n;
        }
        public static CompanyInfo CreateCompany(string r_comid,string comid) {
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

    }
}
