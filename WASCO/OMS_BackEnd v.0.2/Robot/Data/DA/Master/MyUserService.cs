using Dapper;
using Robot.Data.GADB.TT;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Blazored.SessionStorage;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Text.Json;
using Robot.Helper.Hash;
using Microsoft.EntityFrameworkCore;
using static Robot.Data.ML.I_Result;
using Robot.Data.DA.Login;
using Robot.Service.FileGo;
using static Robot.Data.DA.Master.CompanyService;
using System.Security.Cryptography;

namespace Robot.Data.DA.Master {
    public class MyUserService {
        public static string sessionActiveId = "activeuserid";
        ISessionStorageService sessionStorage;
        public class I_FiterSet {
            public String Rcom { get; set; }
            public String Com { get; set; }
            public String RoleID { get; set; }
            public String SearchText { get; set; }
            public DateTime DateFrom { get; set; }
            public DateTime DateTo { get; set; }
            public string ComGroup { get; set; }
            public String Department { get; set; }
            public String Position { get; set; }
            public List<string> UIC { get; set; }
            public bool ShowDisAble { get; set; }
            public bool ShowInActive { get; set; }
            
        }


        public class I_UserSet {
            public string Action { get; set; }
            public UserInfo User { get; set; }
            public List<TransactionLog> Log { get; set; }
            public I_BasicResult OutputAction { get; set; }
            public List<XMenu> XMenu { get; set; }
            public List<XUserInCompany> XCompany { get; set; }
            public List<XUserInRCom> XRCompany { get; set; }
            public List<XUserInBoard> XBoard { get; set; }
            public List<XUserInLoc> XLoc { get; set; }
            public List<XUserInGroup> XGroup { get; set; }
            public List<vw_XFilesRef> files { get; set; }

        }


        public I_UserSet DocSet { get; set; }

        public MyUserService(ISessionStorageService _sessionStorage) {
            sessionStorage = _sessionStorage;
            //DocSet = NewTransaction( "");
        }

        public class SelectOption {
            public string Value { get; set; }
            public string Desc { get; set; }
        }

        public class XMenu : UserPermission {
            public bool X { get; set; }
            public string MenuCode { get; set; }
            public string MenuName { get; set; }


            public string MenuDesc1 { get; set; }
            public string MenuDesc2 { get; set; }
            public string MenuTypeID { get; set; }
            public string MenuGroupID { get; set; }
            public int MenuGroupSort { get; set; }
            public int MenuSubGroupSort { get; set; }

            public bool NeedOpenPermission { get; set; }
            public bool NeedCreatePermission { get; set; }
            public bool NeedEditPermission { get; set; }
            public bool NeedDeletePermission { get; set; }
            public bool NeedPrintPermission { get; set; }
            public string CaptionOpenPermission { get; set; }
            public string CaptionCreatePermission { get; set; }
            public string CaptionEditPermission { get; set; }
            public string CaptionDeletePermission { get; set; }
            public string CaptionPrintPermission { get; set; }
            public int Sort { get; set; }

        }
        public class XUserInRCom : UserInRCom {
            public bool X { get; set; }

            public string RCompanyName { get; set; }
        }
        public class XUserInCompany : UserInCompany {
            public bool X { get; set; }
            public string RComID { get; set; }
            public string CompanyName { get; set; }
            public string CompanyType { get; set; }
            public string CompnayGroup { get; set; }
            public string CompanyTypeName { get; set; }
        }


        public class XUserInBoard : UserInBoard {
            public bool X { get; set; }
            public string Name { get; set; }
        }

        public class XUserInGroup : UserInGroup {
            public bool X { get; set; }
            public string Name { get; set; }

        }

        public class XUserInLoc : UserInLoc {
            public bool X { get; set; }
            public string Name { get; set; }

        }
        #region Get&ListData

        public I_UserSet GetDocSet(string username, string rcom, string userlogin) {
            I_UserSet n = new I_UserSet();
            using (GAEntities db = new GAEntities()) {
                n.User = db.UserInfo.Where(o => o.Username == username).FirstOrDefault();
                n.XMenu = ListMenu(username);
                n.XCompany = ListCompany(username, rcom);
                n.XRCompany = ListRCompany(username, userlogin);
                n.XBoard = ListBoard(username);
                n.XLoc = ListLoc(username);
                n.XGroup = ListGroup(username, rcom);
                n.files = db.vw_XFilesRef.Where(o => o.IsActive == true && o.DocID==username && o.DocType == FileGo.Type_UserSignatureAll).ToList();
            }
            return n;
        }
        public static I_UserSet RefreshFile(I_UserSet doc) {
            try {
                var h = doc.User;
                using (GAEntities db=new GAEntities()) {
                    doc.files = db.vw_XFilesRef.Where(o => o.IsActive == true && o.DocID == doc.User.Username && o.DocType == FileGo.Type_UserSignatureAll).ToList();
                }
              
            } catch (Exception ex) {
            }
            return doc;
        }
        async public void SetSessionDocSet(I_UserSet data) {
            JsonSerializerOptions jso = new JsonSerializerOptions();
            jso.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
            string json = System.Text.Json.JsonSerializer.Serialize(data, jso);
            await sessionStorage.SetItemAsync("userdocset", json);
        }

        async public Task<I_UserSet> GetSessionDocSet() {
            var strdoc = await sessionStorage.GetItemAsync<string>("userdocset");
            return JsonConvert.DeserializeObject<I_UserSet>(strdoc);
        }

        public static UserInfo GetUserInfo(string userID) {
            UserInfo result = new UserInfo();
            using (GAEntities db = new GAEntities()) {
                result = db.UserInfo.Where(o => o.Username == userID  ).FirstOrDefault();
            }
            return result;
        }

        public static vw_UserInfo GetViewUserInfo(string userID) {
            vw_UserInfo result = new vw_UserInfo();
            using (GAEntities db = new GAEntities()) {
                result = db.vw_UserInfo.Where(o => o.Username == userID ).FirstOrDefault();
            }
            return result;
        }

        public static IEnumerable<vw_UserInfo> Listvw_UserInfo() {
            IEnumerable<vw_UserInfo> result = new List<vw_UserInfo>();
            using (GAEntities db = new GAEntities()) {
                result = db.vw_UserInfo.AsNoTrackingWithIdentityResolution().ToArray();
            }
            return result;
        }
        public static IEnumerable<vw_UserInfoMini> Listvw_UserInfoMini() {
            IEnumerable<vw_UserInfoMini> result = new List<vw_UserInfoMini>();
            using (GAEntities db = new GAEntities()) {
                result = db.vw_UserInfoMini.AsNoTrackingWithIdentityResolution().ToArray();
            }
            return result;
        }
        public static List<XMenu> ListMenu(string username) {
            List<XMenu> result = new List<XMenu>();
            try {
                using (GAEntities db = new GAEntities()) {
                    var uimenu = db.UserPermission.Where(o => o.Username == username).ToList();
                    var menu = db.UserMenu.Where(o => o.IsActive == true).OrderBy(o => o.GroupSort).ThenBy(o => o.SubGroupSort).ToList();
                    foreach (var m in menu) {
                        XMenu n = new XMenu();
                        var q = uimenu.Where(o => o.MenuID == m.MenuID).FirstOrDefault();
                        n.MenuID = m.MenuID;
                        n.MenuCode = m.MenuCode;
                        n.MenuName = m.Name;
                        n.Sort = m.GroupSort;
                        n.Username = username;


                        n.MenuGroupID = m.GroupID;
                        n.MenuDesc1 = m.Desc1;
                        n.MenuDesc2 = m.Desc2;
                        n.MenuTypeID = m.TypeID;
                        n.MenuGroupSort = m.GroupSort;
                        n.MenuSubGroupSort = m.SubGroupSort;


                        n.NeedCreatePermission = m.NeedCreatePermission;
                        n.NeedOpenPermission = m.NeedOpenPermission;
                        n.NeedEditPermission = m.NeedEditPermission;
                        n.NeedPrintPermission = m.NeedPrintPermission;
                        n.NeedDeletePermission = m.NeedDeletePermission;
                        n.CaptionOpenPermission = m.CaptionOpenPermission;
                        n.CaptionCreatePermission = m.CaptionCreatePermission;
                        n.CaptionEditPermission = m.CaptionEditPermission;
                        n.CaptionDeletePermission = m.CaptionDeletePermission;
                        n.CaptionPrintPermission = m.CaptionPrintPermission;
                        n.IsCreate = q == null ? false : q.IsCreate;
                        n.IsOpen = q == null ? false : q.IsOpen;
                        n.IsEdit = q == null ? false : q.IsEdit;
                        n.IsDelete = q == null ? false : q.IsDelete;
                        n.IsPrint = q == null ? false : q.IsPrint;
                        result.Add(n);

                    }

                }
            } catch (Exception) { }
            return result;
        }


        public static List<XUserInRCom> ListRCompany(string username, string userlogin) {
            List<XUserInRCom> result = new List<XUserInRCom>();
            try {

                using (GAEntities db = new GAEntities()) {
                    var coms = db.CompanyInfo.Where(o => o.TypeID == "COMPANY" && o.IsActive == true).OrderBy(o => o.CompanyID).ToList();
                    //var gircoms = db.UserInRCom.Where(o => o.UserName == username).ToList();
                    var gircoms = db.vw_UserInRCom.Where(o => o.UserName == username).ToList();

                    var ulrcoms = db.vw_UserInRCom.Where(o => o.UserName == userlogin).ToList();

                    if (gircoms.Count() > 0) {
                        foreach (var c in ulrcoms) {
                            XUserInRCom n = new XUserInRCom();
                            var girc = gircoms.Where(o => o.RComID == c.RComID).FirstOrDefault();
                            n.X = girc != null ? true : false;
                            n.UserName = username;
                            n.RComID = c.RComID;
                            n.RCompanyName = c.RComName;

                            result.Add(n);
                        }
                    } else {
                        foreach (var c in ulrcoms) {
                            XUserInRCom n = new XUserInRCom();
                            var girc = gircoms.Where(o => o.RComID == c.RComID).FirstOrDefault();
                            n.X = girc != null ? true : false;
                            n.UserName = username;
                            n.RComID = c.RComID;
                            n.RCompanyName = c.RComName;

                            result.Add(n);
                        }
                    }


                }
            } catch (Exception) { }
            return result;
        }
        public static List<XUserInCompany> ListCompany(string username, string rcom) {
            List<XUserInCompany> result = new List<XUserInCompany>();
            try {
                using (GAEntities db = new GAEntities()) {
                    List<string> filter = new List<string> { "BRANCH", "COMPANY" };
                    var coms = db.CompanyInfo.Where(o => o.RCompanyID == rcom && o.IsActive == true && filter.Contains(o.TypeID)).OrderBy(o => o.CompanyID).ToList();
                    var gicoms = db.UserInCompany.Where(o => o.RCompanyID == rcom && o.UserName == username).ToList();

                    foreach (var c in coms) {
                        XUserInCompany n = new XUserInCompany();
                        var gic = gicoms.Where(o => o.CompanyID == c.CompanyID).FirstOrDefault();
                        n.UserName = username;
                        n.RCompanyID = c.RCompanyID;
                        n.RComID = c.RCompanyID;
                        n.CompnayGroup = c.GroupCode;
                        n.X = gic != null ? true : false;
                        n.CompanyName = c.GroupCode + " " + c.Name1 + c.Name2;
                        n.CompanyType = c.TypeID;
                        n.CompanyTypeName = c.TypeID;
                        n.CompanyID = c.CompanyID;
                        n.IsActive = true;
                        result.Add(n);
                    }
                }
            } catch (Exception) { }
            return result;
        }
        public static List<XUserInGroup> ListGroup(string username, string rcom) {
            List<XUserInGroup> result = new List<XUserInGroup>();
            try {
                List<string> groupExclude = new List<string> { "SUPERMAN" };
                using (GAEntities db = new GAEntities()) {
                    var group = db.UserGroupInfo.Where(o => o.RComID == rcom
                                                   && !groupExclude.Contains(o.UserGroupID)
                                                   && o.IsActive == true).ToList();
                    var userin_group = db.UserInGroup.Where(o => o.RComID == rcom && o.UserName == username && o.IsActive == true).ToList();

                    foreach (var c in group) {
                        XUserInGroup n = new XUserInGroup();
                        var uig = userin_group.Where(o => o.UserGroupID == c.UserGroupID).FirstOrDefault();
                        n.X = uig != null ? true : false;
                        n.UserName = username;
                        n.RComID = c.RComID;

                        n.Name = c.GroupName;
                        n.UserGroupID = c.UserGroupID;
                        n.IsActive = c.IsActive;
                        result.Add(n);
                    }
                }
            } catch (Exception) { }
            return result;
        }
        public static List<XUserInBoard> ListBoard(string username) {
            List<XUserInBoard> result = new List<XUserInBoard>();
            //var rcom = LoginService.LoginInfo.CurrentRootCompany;
            using (GAEntities db = new GAEntities()) {
                var gib = db.UserInBoard.Where(o => o.Username == username).ToList();
                var boards = db.DashBoard.Where(o => o.IsActive == true).ToList();
                foreach (var c in boards) {
                    XUserInBoard n = new XUserInBoard();
                    var g_inB = gib.Where(o => o.DashBoardID == c.DashBoardID).FirstOrDefault();
                    n.X = g_inB == null ? false : true;
                    n.Username = username;
                    n.X = g_inB != null ? true : false;
                    n.DashBoardID = c.DashBoardID;
                    n.Name = c.Name;
                    result.Add(n);
                }
            }
            return result;
        }

        public static List<XUserInLoc> ListLoc(string username) {
            List<XUserInLoc> result = new List<XUserInLoc>();
            //var rcom = LoginService.LoginInfo.CurrentRootCompany;
            using (GAEntities db = new GAEntities()) {
                var gib = db.UserInLoc.Where(o => o.UserName == username).ToList();
                var boards = db.LocationInfo.Where(o => o.IsActive == true).ToList();
                foreach (var c in boards) {
                    XUserInLoc n = new XUserInLoc();
                    var g_inB = gib.Where(o => o.LocID == c.LocID).FirstOrDefault();
                    n.X = g_inB == null ? false : true;
                    n.UserName = username;
                    n.X = g_inB != null ? true : false;
                    n.LocID = c.LocID;
                    n.Name = c.Name;
                    n.IsActive = true;
                    result.Add(n);
                }
            }
            return result;
        }
        public static IEnumerable<vw_UserInfo> ListDoc(I_FiterSet f, LogInService.LoginSet login) {
            var currRCom = login.CurrentRootCompany.CompanyID;
            IEnumerable<vw_UserInfo> result;
            using (GAEntities db = new GAEntities()) {

                var userInRcom = db.UserInRCom.Where(o => o.RComID.Contains(currRCom)).Select(o => o.UserName).AsNoTrackingWithIdentityResolution().ToArray();
                result = db.vw_UserInfo.Where(o =>
                    (o.FullName.Contains(f.SearchText)
                    || o.FullName_En.Contains(f.SearchText)
                    || o.PositionID.Contains(f.SearchText)
                    || o.DepartmentID.Contains(f.SearchText)
                    || o.JobLevel.Contains(f.SearchText)
                    || o.JobType.Contains(f.SearchText)
                    || o.EmpCode.Contains(f.SearchText)
                    || o.Username.Contains(f.SearchText)
                    || f.SearchText == "")
                    && (o.DepartmentID == f.Department || f.Department == "")
                    && userInRcom.Contains(o.Username)
                    && (o.IsActive != f.ShowInActive)
                    ).OrderBy(o => o.CreatedDate).ToList();
            }

            return result;
        }

        public static IEnumerable<vw_UserInfo> ListDocUserName() {
            IEnumerable<vw_UserInfo> result;
            using (GAEntities db = new GAEntities()) {

                result = db.vw_UserInfo.Where(o => o.IsActive).OrderBy(o => o.CreatedDate).ToList();
            }

            return result;
        }

        public static IEnumerable<vw_UserInfo> ListDocByDepartment(I_FiterSet f, LogInService.LoginSet login) {
            var currRCom = login.CurrentRootCompany.CompanyID;
            IEnumerable<vw_UserInfo> result;
            using (GAEntities db = new GAEntities()) {

                var userInRcom = db.UserInRCom.Where(o => o.RComID.Contains(currRCom)).Select(o => o.UserName).AsNoTrackingWithIdentityResolution().ToArray(); ;
                result = db.vw_UserInfo.Where(o =>
                    (o.FullName.Contains(f.SearchText)
                    || o.FullName_En.Contains(f.SearchText)
                    || o.PositionID.Contains(f.SearchText)
                    || o.DepartmentID.Contains(f.SearchText)
                    || o.JobLevel.Contains(f.SearchText)
                    || o.JobType.Contains(f.SearchText)
                    || o.EmpCode.Contains(f.SearchText)
                    || o.Username.Contains(f.SearchText)
                    || f.SearchText == "")
                    && (o.Username != login.CurrentUser)
                    && (o.DepartmentID == login.CurrentUserInfo.DefaultCompany)
                    && userInRcom.Contains(o.Username)
                    ).OrderBy(o => o.CreatedDate).ToList();
            }

            return result;
        }

        public static IEnumerable<vw_UserInfo> ListDocBySubDepartment(I_FiterSet f, LogInService.LoginSet login) {
            var currRCom = login.CurrentRootCompany.CompanyID;
            IEnumerable<vw_UserInfo> result;
            using (GAEntities db = new GAEntities()) {

                var userInRcom = db.UserInRCom.Where(o => o.RComID.Contains(currRCom)).Select(o => o.UserName).AsNoTrackingWithIdentityResolution().ToArray(); ;
                result = db.vw_UserInfo.Where(o =>
                    (
                                   o.FullName.Contains(f.SearchText)
                                || o.FullName_En.Contains(f.SearchText)
                                || o.PositionID.Contains(f.SearchText)
                                || o.DepartmentID.Contains(f.SearchText)
                                || o.JobLevel.Contains(f.SearchText)
                                || o.JobType.Contains(f.SearchText)
                                || o.EmpCode.Contains(f.SearchText)
                                || o.Username.Contains(f.SearchText)
                                || f.SearchText == ""
                    )
                    && (o.Username != login.CurrentUser)
                    && (o.DepartmentID == login.CurrentUserInfo.DefaultCompany)
                    //&& (o.SubDepartmentID == login.CurrentUserInfo.SubDepartmentID)
                    && userInRcom.Contains(o.Username)
                    ).OrderBy(o => o.CreatedDate).ToList();
            }

            return result;
        }

        public static List<vw_UserInfo> ListDoc(I_FiterSet f, string rcom) {
            List<vw_UserInfo> result = new List<vw_UserInfo>();
            using (GAEntities db = new GAEntities()) {
                var userInRcom = db.UserInRCom.Where(o => o.RComID.Contains(rcom)).Select(o => o.UserName).ToList();
                result = db.vw_UserInfo.Where(o =>
                                       (
                                               o.FirstName.Contains(f.SearchText)
                                               || o.LastName.Contains(f.SearchText)
                                               || o.FirstName.Contains(f.SearchText)
                                               || o.EmpCode.Contains(f.SearchText)
                                               || o.Username.Contains(f.SearchText)
                                               || f.SearchText == ""
                                        )
                                        && userInRcom.Contains(o.Username)
                                           && (o.IsActive == true || f.ShowDisAble == true)
                                            ).OrderByDescending(o => o.CreatedDate).ToList();

            }
            return result;
        }

        #endregion

        #region update หน่วยงาน & เลือกหน่วยงานใหม่
        public static I_BasicResult RemoveDepartment(string username, string rcom, string com) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                using (GAEntities db = new GAEntities()) {
                    var u = db.UserInfo.Where(o => o.Username == username).FirstOrDefault();
                    u.DepartmentID = "";
                    u.SubDepartmentID = "";
                    db.SaveChanges();
                    //if (u != null) {
                    //    u.DepartmentID = "";
                    //    db.UserInCompany.RemoveRange(db.UserInCompany.Where(o => o.UserName == username && o.RCompanyID == rcom && o.CompanyID == com));
                    //    db.SaveChanges();
                    //}

                }
            } catch (Exception ex) {
                if (ex.InnerException != null) {
                    result.Message1 = ex.InnerException.ToString();
                } else {
                    result.Message1 = ex.Message;
                }
            }
            return result;
        }

        public static I_BasicResult RemoveSubDepartment(string username) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                using (GAEntities db = new GAEntities()) {
                    var u = db.UserInfo.Where(o => o.Username == username).FirstOrDefault();
                    u.SubDepartmentID = "";
                    db.SaveChanges();
                }
            } catch (Exception ex) {
                if (ex.InnerException != null) {
                    result.Message1 = ex.InnerException.ToString();
                } else {
                    result.Message1 = ex.Message;
                }
            }
            return result;
        }

        // list UserNotDepartment select

        public class LISTSELECT_UserNotDepartment : UserInfo {
            public bool IsSelect { get; set; }
        }

        public static List<LISTSELECT_UserNotDepartment> ListUserNotDepartment() {
            List<LISTSELECT_UserNotDepartment> result = new List<LISTSELECT_UserNotDepartment>();

            using (GAEntities db = new GAEntities()) {
                var query = new List<UserInfo>();
                query = db.UserInfo.Where(o =>
                                        o.DepartmentID == ""
                                        && o.IsActive == true
                        ).OrderByDescending(o => o.CreatedDate).ToList();
                foreach (var q in query) {
                    var qq = ConvertUserInfo(q);
                    result.Add(qq);
                }
            }

            return result;
        }

        public static List<LISTSELECT_UserNotDepartment> ListUserNotSubDepartment(string department) {
            List<LISTSELECT_UserNotDepartment> result = new List<LISTSELECT_UserNotDepartment>();

            using (GAEntities db = new GAEntities()) {
                var query = new List<UserInfo>();
                query = db.UserInfo.Where(o =>
                                        o.DepartmentID == department
                                        && o.SubDepartmentID == ""
                                        && o.IsActive == true
                        ).OrderByDescending(o => o.CreatedDate).ToList();
                foreach (var q in query) {
                    var qq = ConvertUserInfo(q);
                    result.Add(qq);
                }
            }

            return result;
        }

        public static LISTSELECT_UserNotDepartment ConvertUserInfo(UserInfo h) {
            LISTSELECT_UserNotDepartment u = new LISTSELECT_UserNotDepartment();
            u.IsSelect = false;
            u.RCompanyID = h.RCompanyID;
            u.CompanyID = h.CompanyID;
            u.Username = h.Username;
            u.FullName = h.FullName;
            u.EmpCode = h.EmpCode;
            u.Title = h.Title;
            u.FirstName = h.FirstName;
            u.LastName = h.LastName;
            u.Title_En = h.Title_En;
            u.FirstName_En = h.FirstName_En;
            u.LastName_En = h.LastName_En;
            u.NickName = h.NickName;
            u.Gender = h.Gender;
            u.DepartmentID = h.DepartmentID;
            u.SubDepartmentID = h.SubDepartmentID;
            u.PositionID = h.PositionID;
            u.JobLevel = h.JobLevel;
            u.JobType = h.JobType;
            u.AddrNo = h.AddrNo;
            u.AddrMoo = h.AddrMoo;
            u.AddrTumbon = h.AddrTumbon;
            u.AddrAmphoe = h.AddrAmphoe;
            u.AddrProvince = h.AddrProvince;
            u.AddrPostCode = h.AddrPostCode;
            u.AddrCountry = h.AddrCountry;
            u.DefaultCompany = h.DefaultCompany;
            u.Tel = h.Tel;
            u.Mobile = h.Mobile;
            u.Email = h.Email;
            u.CitizenId = h.CitizenId;
            u.BookBankNumber = h.BookBankNumber;
            u.MaritalStatus = h.MaritalStatus;
            u.Birthdate = h.Birthdate;
            u.JobStartDate = h.JobStartDate;
            u.ResignDate = h.ResignDate;
            u.ApproveBy = h.ApproveBy;
            u.IsProgramUser = h.IsProgramUser;
            u.UseTimeStamp = h.UseTimeStamp;
            u.IsNewUser = h.IsNewUser;

            return u;
        }

        public static I_BasicResult UpdateListUserNotSubDepartment(string username, string Department, string SubDepartment) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };

            try {
                using (GAEntities db = new GAEntities()) {
                    var u = db.UserInfo.Where(o => o.Username == username && o.DepartmentID == Department).FirstOrDefault();
                    if (u != null) {
                        u.SubDepartmentID = SubDepartment;
                        db.SaveChanges();
                    }
                }
            } catch (Exception ex) {
                if (ex.InnerException != null) {
                    result.Message1 = ex.InnerException.ToString();
                } else {
                    result.Message1 = ex.Message;
                }
            }
            return result;
        }

        public static I_BasicResult UpdateListUserNotDepartment(string username, string comid) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };

            try {
                using (GAEntities db = new GAEntities()) {
                    var u = db.UserInfo.Where(o => o.Username == username).FirstOrDefault();
                    if (u != null) {
                        u.DepartmentID = comid;
                        u.DefaultCompany = comid;
                        db.SaveChanges();
                    }
                }
            } catch (Exception ex) {
                if (ex.InnerException != null) {
                    result.Message1 = ex.InnerException.ToString();
                } else {
                    result.Message1 = ex.Message;
                }
            }
            return result;
        }

        // end list UserNotDepartment select

        #endregion

        #region Save
        public static I_BasicResult Save(I_UserSet doc, LogInService.LoginSet login) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            var h = doc.User;

            try {
                using (GAEntities db = new GAEntities()) {
                    var u = db.UserInfo.Where(o => o.Username == doc.User.Username).FirstOrDefault();
                    if (u == null) {
                        db.UserInfo.Add(doc.User);
                        db.SaveChanges();
                        Create2RCompany(doc.User.Username, login.CurrentRootCompany.CompanyID); ;
                    } else {
                        u.EmpCode = h.EmpCode;
                        u.Title = h.Title;
                        u.RCompanyID = h.RCompanyID;
                        u.CompanyID = h.CompanyID;
                        u.FirstName = h.FirstName;
                        u.LastName = h.LastName;
                        u.Title_En = h.Title_En;
                        u.FirstName_En = h.FirstName_En;
                        u.LastName_En = h.LastName_En;
                        u.NickName = h.NickName;
                        u.Gender = h.Gender;
                        u.DepartmentID = h.DepartmentID;
                        u.PositionID = h.PositionID;
                        u.SubDepartmentID = h.SubDepartmentID;
                        u.JobLevel = h.JobLevel;
                        u.JobType = h.JobType;
                        u.AddrNo = h.AddrNo;
                        u.AddrMoo = h.AddrMoo;
                        u.AddrTumbon = h.AddrTumbon;
                        u.AddrAmphoe = h.AddrAmphoe;
                        u.AddrProvince = h.AddrProvince;
                        u.AddrPostCode = h.AddrPostCode;
                        u.AddrCountry = h.AddrCountry;
                        u.DefaultCompany = h.DefaultCompany;

                        u.Tel = h.Tel;
                        u.Mobile = h.Mobile;
                        u.Email = h.Email;
                        u.CitizenId = h.CitizenId;
                        u.BookBankNumber = h.BookBankNumber;
                        u.MaritalStatus = h.MaritalStatus;
                        u.Birthdate = h.Birthdate;
                        u.JobStartDate = h.JobStartDate;
                        u.ResignDate = h.ResignDate;
                        u.ApproveBy = h.ApproveBy;
                        u.IsProgramUser = h.IsProgramUser;
                        u.UseTimeStamp = h.UseTimeStamp;
                        u.IsNewUser = h.IsNewUser;
                        u.ModifiedBy = login.CurrentUser;
                        u.ModifiedDate = DateTime.Now;
                        u.IsActive = h.IsActive;
                        db.SaveChanges();
                        Create2RCompany(doc.User.Username, login.CurrentRootCompany.CompanyID); ;
                        var rp = InsertAllPermission(doc, login.CurrentRootCompany.CompanyID, h.Username);

                        if (rp.Result == "fail") {
                            result.Result = "fail";
                            result.Message1 = result.Message1 + " " + rp.Message1;
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

        public static List<UserPermission> ConvertX2Menu(I_UserSet input, string rcom) {
            List<UserPermission> result = new List<UserPermission>();
            foreach (var s in input.XMenu) {
                var n = new UserPermission();
                n.RComID = rcom;
                n.Username = s.Username;
                n.MenuID = s.MenuID;
                n.IsOpen = Convert.ToBoolean(s.IsOpen); ;
                n.IsCreate = Convert.ToBoolean(s.IsCreate);
                n.IsEdit = Convert.ToBoolean(s.IsEdit);
                n.IsDelete = Convert.ToBoolean(s.IsDelete);
                n.IsPrint = Convert.ToBoolean(s.IsPrint);
                result.Add(n);
            }
            return result;
        }

        public static List<UserInRCom> ConvertX2RCompany(I_UserSet input) {
            List<UserInRCom> result = new List<UserInRCom>();
            foreach (var s in input.XRCompany) {
                if (s.X) {
                    var n = new UserInRCom();
                    n.RComID = s.RComID;
                    n.UserName = s.UserName;
                    result.Add(n);
                }
            }
            return result;
        }
        public static I_BasicResult Create2RCompany(string user, string rcom) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                using (GAEntities db = new GAEntities()) {
                    var check_exist = db.UserInRCom.Where(o => o.UserName == user && o.RComID == rcom).FirstOrDefault();
                    if (check_exist == null) {
                        UserInRCom n = new UserInRCom();
                        n.RComID = rcom;
                        n.UserName = user;
                        db.UserInRCom.Add(n);
                        db.SaveChanges();
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

        public static I_BasicResult CreateUserInCompany(string user, string rcom, string com) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                using (GAEntities db = new GAEntities()) {
                    var check_exist = db.UserInCompany.Where(o => o.UserName == user && o.RCompanyID == rcom && o.CompanyID == com).FirstOrDefault();
                    if (check_exist == null) {
                        UserInCompany n = new UserInCompany();
                        n.RCompanyID = rcom;
                        n.UserName = user;
                        n.CompanyID = com;
                        n.IsActive = true;
                        db.UserInCompany.Add(n);
                        db.SaveChanges();
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

        public static List<UserInCompany> ConvertX2Company(I_UserSet input) {
            List<UserInCompany> result = new List<UserInCompany>();
            foreach (var s in input.XCompany) {
                if (s.X) {
                    var n = new UserInCompany();
                    n.RCompanyID = s.RCompanyID;
                    n.CompanyID = s.CompanyID;
                    n.UserName = s.UserName;
                    n.IsActive = s.IsActive;
                    result.Add(n);
                }

            }

            return result;
        }
        public static List<UserInBoard> ConvertX2Board(I_UserSet input) {
            List<UserInBoard> result = new List<UserInBoard>();
            var h = input.User;
            foreach (var s in input.XBoard) {
                var n = new UserInBoard();
                if (s.X) {
                    n.Username = h.Username;
                    n.DashBoardID = s.DashBoardID;
                    result.Add(n);
                }
            }
            return result;
        }

        public static List<UserInLoc> ConvertX2Loc(I_UserSet input) {
            List<UserInLoc> result = new List<UserInLoc>();
            var h = input.User;
            foreach (var s in input.XLoc) {
                var n = new UserInLoc();
                if (s.X) {
                    n.UserName = h.Username;
                    n.RComID = h.RCompanyID;
                    n.LocID = s.LocID;
                    n.IsActive= s.IsActive;
                    result.Add(n);
                }
            }
            return result;
        }
        public static List<UserInGroup> ConvertX2Group(I_UserSet input) {
            List<UserInGroup> result = new List<UserInGroup>();
            var h = input.User;
            foreach (var s in input.XGroup) {
                var n = new UserInGroup();
                if (s.X) {
                    n.UserName = s.UserName;
                    n.RComID = s.RComID;
                    n.UserGroupID = s.UserGroupID;
                    n.IsActive = s.IsActive;
                    result.Add(n);
                }
            }
            return result;
        }

        public static void SaveLog(TransactionLog data, string rcom, string username) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                data.RCompanyID = rcom;
                data.TableID = data.TableID == null ? "" : data.TableID;
                data.TransactionID = data.TransactionID == null ? "" : data.TransactionID;
                data.TransactionDate = data.TransactionDate == null ? data.CreatedDate : data.TransactionDate;
                data.CreatedBy = username;
                data.CreatedDate = DateTime.Now;
                data.ChangeValue = data.ChangeValue == null ? "" : data.ChangeValue;
                data.CompanyID = data.CompanyID == null ? "" : data.CompanyID;
                data.ParentID = data.ParentID == null ? "" : data.ParentID;
                data.Action = data.ActionType == null ? "" : data.ActionType;
                data.Action = data.Action == null ? "" : data.Action;
                data.ChangeValue = data.ChangeValue == null ? "" : data.ChangeValue;
                data.IsActive = true;
                using (GAEntities db = new GAEntities()) {
                    db.TransactionLog.Add(data);
                    var r = db.SaveChanges();
                }
            } catch (Exception ex) {
                result.Result = "fail";
                if (ex.InnerException != null) {
                    result.Message1 = ex.InnerException.ToString();
                } else {
                    result.Message1 = ex.Message;
                }
            }
        }

        #endregion

        #region  permission manangement
        public static I_BasicResult InsertMenuToUser(List<UserPermission> user_menu) {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            string conStr = Globals.GAEntitiesConn;
            var user = user_menu.FirstOrDefault();
            if (user == null) {
                //r.Result = "fail";
                //r.Message1 = "Menu not found";
                return r;
            }
            try {

                using (SqlConnection conn = new SqlConnection(conStr)) {
                    conn.Open();
                    string sql = "delete from UserPermission where  Username=@username";
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("Username", user.Username);
                    var rd = conn.Execute(sql, dynamicParameters);
                    foreach (var f in user_menu) {

                        sql = @"insert into  UserPermission  (
                               RComID
                              ,Username
                              ,MenuID
                              ,IsOpen
                              ,IsCreate
                              ,IsEdit
                              ,IsDelete
                              ,IsPrint
	                          ) values(
                                @RComID
                              , @Username
                              , @MenuID
                              , @IsOpen
                              , @IsCreate
                              ,@IsEdit
                              , @IsDelete
                              , @IsPrint                             
                         )";
                        conn.Execute(sql, new {
                            @RComID = f.RComID,
                            @Username = f.Username,
                            @MenuID = f.MenuID,
                            @IsOpen = f.IsOpen,
                            @IsCreate = f.IsCreate,
                            @IsEdit = f.IsEdit,
                            @IsDelete = f.IsDelete,
                            @IsPrint = f.IsPrint
                        });
                    }

                }

            } catch (Exception ex) {
                r.Result = "fail";
                if (ex.InnerException != null) {
                    r.Message1 = ex.InnerException.ToString();
                } else {
                    r.Message1 = ex.Message;
                }

            }
            return r;
        }
        public static I_BasicResult InsertRCompanyToUser(List<UserInRCom> uircom) {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            string conStr = Globals.GAEntitiesConn;

            var user = uircom.FirstOrDefault();
            if (user == null) {
                //r.Result = "fail";
                //r.Message1 = "Menu not found";
                return r;
            }
            string rcom = user.RComID;

            try {

                using (SqlConnection conn = new SqlConnection(conStr)) {
                    conn.Open();
                    string sql = "delete from UserInRCom where  UserName=@UserName";
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("UserName", user.UserName);
                    var rd = conn.Execute(sql, dynamicParameters);
                    foreach (var f in uircom) {

                        sql = @"insert into  UserInRCom  (
                               RComID
                              ,UserName           
	                          ) values(
                                @RComID
                              , @UserName 
                         )";
                        conn.Execute(sql, new {
                            @RComID = f.RComID,
                            @UserName = f.UserName

                        });
                    }

                }

            } catch (Exception ex) {
                r.Result = "fail";
                if (ex.InnerException != null) {
                    r.Message1 = ex.InnerException.ToString();
                } else {
                    r.Message1 = ex.Message;
                }

            }
            return r;
        }
        public static I_BasicResult InsertUserToCurrentRcom(string username, string rcom) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                using (GAEntities db = new GAEntities()) {
                    var chk_exist = db.UserInRCom.Where(o => o.RComID == rcom && o.UserName == username).FirstOrDefault();
                    if (chk_exist == null) {
                        UserInRCom n = new UserInRCom { RComID = rcom, UserName = username };
                        db.UserInRCom.Add(n);
                        db.SaveChanges();
                    }

                }
            } catch (Exception ex) {
                if (ex.InnerException != null) {
                    result.Message1 = ex.InnerException.ToString();
                } else {
                    result.Message1 = ex.Message;
                }
            }
            return result;
        }
        public static I_BasicResult InsertCompanyToUser(List<UserInCompany> uicom, string rcom, string username) {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            string conStr = Globals.GAEntitiesConn;

            try {

                using (SqlConnection conn = new SqlConnection(conStr)) {
                    conn.Open();

                    string sql = "delete from UserInCompany where RCompanyID = @rcom and UserName=@UserName";
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("rcom", rcom);
                    dynamicParameters.Add("UserName", username);
                    var rd = conn.Execute(sql, dynamicParameters);
                    foreach (var f in uicom) {

                        sql = @"insert into  UserInCompany  (
                               UserName
                              ,RCompanyID
                              ,CompanyID 
                              ,IsActive 
	                          ) values(
                                @UserName
                              , @RCompanyID
                              , @CompanyID 
                              , @IsActive
                         )";
                        conn.Execute(sql, new {
                            @UserName = f.UserName,
                            @RCompanyID = f.RCompanyID,
                            @CompanyID = f.CompanyID,
                            @IsActive = f.IsActive

                        });
                    }

                }

            } catch (Exception ex) {
                r.Result = "fail";
                if (ex.InnerException != null) {
                    r.Message1 = ex.InnerException.ToString();
                } else {
                    r.Message1 = ex.Message;
                }

            }
            return r;
        }
        public static I_BasicResult InsertGroupToUser(List<UserInGroup> uig, string rcom, string username) {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            string conStr = Globals.GAEntitiesConn;

            try {

                using (SqlConnection conn = new SqlConnection(conStr)) {
                    conn.Open();
                    string sql = "delete from UserInGroup where RComID = @rcom and UserName=@UserName";
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("rcom", rcom);
                    dynamicParameters.Add("UserName", username);
                    var rd = conn.Execute(sql, dynamicParameters);
                    foreach (var f in uig) {

                        sql = @"insert into  UserInGroup  (
                               RComID
                              ,UserName
                              ,UserGroupID
                              ,IsActive  
	                          ) values(
                                @RComID
                              , @UserName
                              , @UserGroupID
                              , @IsActive 
                         )";
                        conn.Execute(sql, new {
                            @RComID = f.RComID,
                            @UserName = f.UserName,
                            @UserGroupID = f.UserGroupID,
                            @IsActive = f.IsActive,

                        });
                    }

                }

            } catch (Exception ex) {
                r.Result = "fail";
                if (ex.InnerException != null) {
                    r.Message1 = ex.InnerException.ToString();
                } else {
                    r.Message1 = ex.Message;
                }

            }
            return r;
        }

        public static I_BasicResult InsertBoardToUser(List<UserInBoard> uiboard, string username) {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };

            try {
                using (GAEntities db = new GAEntities()) {
                    db.UserInBoard.RemoveRange(db.UserInBoard.Where(o => o.Username == username));
                    db.UserInBoard.AddRange(uiboard);
                    db.SaveChanges();
                }

            } catch (Exception ex) {
                r.Result = "fail";
                if (ex.InnerException != null) {
                    r.Message1 = ex.InnerException.ToString();
                } else {
                    r.Message1 = ex.Message;
                }

            }
            return r;
        }

        public static I_BasicResult InsertLocToUser(List<UserInLoc> uiLoc, string username) {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };

            try {
                using (GAEntities db = new GAEntities()) {
                    db.UserInLoc.RemoveRange(db.UserInLoc.Where(o => o.UserName == username));
                    db.UserInLoc.AddRange(uiLoc);
                    db.SaveChanges();
                }
            } catch (Exception ex) {
                r.Result = "fail";
                if (ex.InnerException != null) {
                    r.Message1 = ex.InnerException.ToString();
                } else {
                    r.Message1 = ex.Message;
                }

            }
            return r;
        }
        public static I_BasicResult InsertAllPermission(I_UserSet input, string rcom, string user) {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                var menu = ConvertX2Menu(input, rcom);
                var uic = ConvertX2Company(input);
                var uirc = ConvertX2RCompany(input);
                var board = ConvertX2Board(input);
                var loc = ConvertX2Loc(input);
                var group = ConvertX2Group(input);
                var r1 = InsertMenuToUser(menu);
                var r2 = InsertCompanyToUser(uic, rcom, input.User.Username);
                var r3 = InsertBoardToUser(board, input.User.Username);
                var r4 = InsertGroupToUser(group, rcom, input.User.Username);
                var r5 = InsertRCompanyToUser(uirc);
                var r6 = InsertLocToUser(loc, input.User.Username);

                if (r1.Result == "fail") {
                    r.Result = "fail";
                    r.Message1 = r.Message1 + " " + r1.Message1;
                }
                if (r2.Result == "fail") {
                    r.Result = "fail";
                    r.Message1 = r.Message1 + " " + r2.Message1;
                }
                if (r3.Result == "fail") {
                    r.Result = "fail";
                    r.Message1 = r.Message1 + " " + r3.Message1;
                }
                if (r4.Result == "fail") {
                    r.Result = "fail";
                    r.Message1 = r.Message1 + " " + r4.Message1;
                }
                if (r5.Result == "fail") {
                    r.Result = "fail";
                    r.Message1 = r.Message1 + " " + r5.Message1;
                }
                if (r6.Result == "fail") {
                    r.Result = "fail";
                    r.Message1 = r.Message1 + " " + r5.Message1;
                }

            } catch (Exception ex) {
                r.Result = "fail";
                if (ex.InnerException != null) {
                    r.Message1 = ex.InnerException.ToString();
                } else {
                    r.Message1 = ex.Message;
                }

            }
            return r;
        }

        #endregion

        #region New transaction

        public static I_UserSet NewTransaction(string rcom) {
            I_UserSet n = new I_UserSet();
            n.User = NewUser(rcom);

            n.XMenu = new List<XMenu>();
            n.XCompany = new List<XUserInCompany>();
            n.XRCompany = new List<XUserInRCom>();
            n.XBoard = new List<XUserInBoard>();
            n.XLoc = new List<XUserInLoc>();
            n.XGroup = new List<XUserInGroup>();
            n.files = new List<vw_XFilesRef>();
            n.Log = new List<TransactionLog>();

            return n;
        }

        public static I_FiterSet NewFilterSet() {
            I_FiterSet n = new I_FiterSet();

            n.Rcom = "";
            n.Com = "";
            n.ComGroup = "";
            n.DateFrom = DateTime.Now.AddYears(-1);
            n.DateTo = DateTime.Now.Date;
            n.SearchText = "";
            n.Position = "";
            n.Department = "";
            n.ShowInActive = false;
            n.UIC = new List<string>();

            return n;
        }

        public static UserInfo NewUser(string rcom) {
            UserInfo newdata = new UserInfo();

            newdata.Username = "";
            newdata.Password = "";
            newdata.RCompanyID = "";
            newdata.CompanyID = "";
            newdata.EmpCode = "";
            newdata.FirstName = "";
            newdata.LastName = "";
            newdata.FullName = "";
            newdata.FirstName_En = "";
            newdata.LastName_En = "";
            newdata.FullName_En = "";
            newdata.NickName = "";
            newdata.Gender = "";
            newdata.DepartmentID = "";
            newdata.SubDepartmentID = "";
            newdata.PositionID = "";
            newdata.IsProgramUser = false;
            newdata.IsNewUser = true;
            newdata.JobStartDate = null;
            newdata.ResignDate = null;
            newdata.AddrFull = "";
            newdata.AddrNo = "";
            newdata.AddrMoo = "";
            newdata.AddrTumbon = "";
            newdata.AddrAmphoe = "";
            newdata.AddrProvince = "";
            newdata.AddrPostCode = "";
            newdata.AddrCountry = "";
            newdata.Tel = "";
            newdata.Mobile = "";
            newdata.Email = "";
            newdata.Birthdate = null;
            newdata.MaritalStatus = null;
            newdata.CitizenId = null;
            newdata.BookBankNumber = null;
            newdata.ApproveBy = "";
            newdata.IsProgramUser = true;
            newdata.UseTimeStamp = false;
            newdata.ImageProfile = null;
            newdata.LineToken = null;
            newdata.DefaultCompany = null;
            newdata.CreatedBy = "";
            newdata.CreatedDate = DateTime.Now;
            newdata.ModifiedBy = "";
            newdata.ModifiedDate = null;
            newdata.IsActive = true;
            return newdata;
        }

        //public static List<SelectOption> ListUseLevel()
        //{
        //    return new List<SelectOption>() {
        //        new SelectOption(){ IsSelect = true ,Value= "0", Description="0" ,Sort = 1},
        //        new SelectOption(){ IsSelect = true ,Value = "1", Description="1", Sort = 2}
        //    };
        //}
        #endregion

        public static I_BasicResult ResetPassword(string username) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };

            try {
                using (GAEntities db = new GAEntities()) {
                    string defaultPass = GetDefualtPassword();
                    var u = db.UserInfo.Where(o => o.Username == username).FirstOrDefault();
                    u.IsNewUser = true;
                    u.Password = Hash.hashPassword("MD5", defaultPass);

                    db.SaveChanges();
                    result.Message2 = defaultPass;
                }
            } catch (Exception ex) {
                if (ex.InnerException != null) {
                    result.Message1 = ex.InnerException.ToString();
                } else {
                    result.Message1 = ex.Message;
                }
            }
            return result;
        }

        public static string GetDefualtPassword() {
            string result = "";
            try {
                using (GAEntities db = new GAEntities()) {
                    result = db.Config.Where(o => o.ConfigID == "defualtpassword").FirstOrDefault().ValueString1;
                }
            } catch (Exception ex) { }
            return result;
        }

        async public void SetSessionUserFiterSet(I_FiterSet data) {
            JsonSerializerOptions jso = new JsonSerializerOptions();
            jso.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
            string json = System.Text.Json.JsonSerializer.Serialize(data, jso);
            await sessionStorage.SetItemAsync("user_Fiter", json);
        }
        async public Task<I_FiterSet> GetSessionUserFiterSet() {
            I_FiterSet result = NewFilterSet();
            var strdoc = await sessionStorage.GetItemAsync<string>("user_Fiter");

            if (strdoc != null) {
                return JsonConvert.DeserializeObject<I_FiterSet>(strdoc);
            } else {
                return null;
            }

        }


    }
}
