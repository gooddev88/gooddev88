using System.Data;
using static RobotWasm.Shared.Data.ML.Shared.I_Result;
using RobotWasm.Shared.Data.ML.Master.User;
using RobotWasm.Server.Helper;
using RobotWasm.Shared.Data.GaDB;
using RobotWasm.Server.Data.GaDB;
using Microsoft.EntityFrameworkCore;
using RobotWasm.Shared.Data.ML.Login;
using System.Text.RegularExpressions;

namespace RobotWasm.Server.Data.DA.Master {
    public class UserService {

        public I_UserSet DocSet { get; set; }

        public UserService() {

        }

        #region Get List
        public static I_UserSet GetDocSet(string username, string rcom,string userlogin) {
            I_UserSet n = new I_UserSet();
            using (GAEntities db = new GAEntities()) {

                n.User = db.UserInfo.Where(o => o.Username == username && o.IsActive == true).FirstOrDefault();
                n.XGroup = ListGroup(username, rcom);
                n.XRcom = ListRCompany(username,userlogin);
            }
            return n;
        }

        public static List<XUserInGroup> ListGroup(string username,string rcom) {
            List<XUserInGroup> result = new List<XUserInGroup>();
            try {
                List<string> groupExclude = new List<string> { "SUPERMAN" };
                using (GAEntities db = new GAEntities()) {
                    var group = db.UserGroupInfo.Where(o => !groupExclude.Contains(o.UserGroupID)
                                                   && o.RComID == rcom && o.IsActive == true).ToList();
                    var userin_group = db.UserInGroup.Where(o => o.UserName == username && o.RComID == rcom).ToList();

                    foreach (var c in group) {
                        XUserInGroup n = new XUserInGroup();
                        var uig = userin_group.Where(o => o.UserGroupID == c.UserGroupID).FirstOrDefault();
                        n.X = uig != null ? true : false;
                        n.UserName = username;
                        n.UserGroupID = c.UserGroupID;
                        n.RComID = c.RComID;
                        n.Name = c.GroupName;
                        result.Add(n);
                    }
                }
            } catch (Exception ex) { }
            return result;
        }

        public static List<XUserInRCom> ListRCompany(string username, string userlogin)
        {
            List<XUserInRCom> result = new List<XUserInRCom>();
            try
            {
                using (GAEntities db = new GAEntities())
                {
                    var coms = db.CompanyInfo.Where(o => o.TypeID == "COMPANY" && o.IsActive == true).OrderBy(o => o.CompanyID).ToList();
                    var gircoms = db.UserInRCom.Where(o => o.UserName == username).ToList();
                    var ulrcoms = db.UserInRCom.Where(o => o.UserName == userlogin).ToList();

                    if (gircoms.Count() > 0)
                    {
                        foreach (var c in ulrcoms)
                        {
                            XUserInRCom n = new XUserInRCom();
                            var girc = gircoms.Where(o => o.RComID == c.RComID).FirstOrDefault();
                            n.X = girc != null ? true : false;
                            n.UserName = username;
                            n.RComID = c.RComID;
                            n.RCompanyName = c.RComID;

                            result.Add(n);
                        }
                    }
                    else
                    {
                        foreach (var c in ulrcoms)
                        {
                            XUserInRCom n = new XUserInRCom();
                            var girc = gircoms.Where(o => o.RComID == c.RComID).FirstOrDefault();
                            n.X = girc != null ? true : false;
                            n.UserName = username;
                            n.RComID = c.RComID;
                            n.RCompanyName = c.RComID;

                            result.Add(n);
                        }
                    }
                }
            }
            catch (Exception) { }
            return result;
        }

        public static List<vw_UserInfo> ListDoc(string Search, string rcom)
        {
            List<vw_UserInfo> result = new List<vw_UserInfo>();
            using (GAEntities db = new GAEntities())
            {

                var userInRcom = db.UserInRCom.Where(o => o.RComID.Contains(rcom)).Select(o => o.UserName).AsNoTrackingWithIdentityResolution().ToArray();

                result = db.vw_UserInfo.Where(o =>
                                (o.FirstName.Contains(Search)
                                || o.LastName.Contains(Search)
                                || o.FullName.Contains(Search)
                                || o.EmpCode.Contains(Search)
                                || o.Username.Contains(Search)
                                || Search == "")
                                             && (o.IsActive == true)
                                             && userInRcom.Contains(o.Username)
                                            ).OrderByDescending(o => o.CreatedDate).ToList();
            }
            return result;
        }

        #endregion

        #region userinfo
        public static IEnumerable<UserInfo> ListUserInfo(string search) {
            IEnumerable<UserInfo> result = new List<UserInfo>();
            using (GAEntities db = new GAEntities()) {
                result = db.UserInfo.Where(o => o.IsActive == true
                                                    && (
                                                            o.FullName.Contains(search)
                                                            || o.Username.Contains (search)
                                                            || search==""
                                                            )
                                                        ).ToList();
            }
            return result;
        }

        public static UserInfo GetUserInfo(string username)
        {
            UserInfo result = new UserInfo();
            using (GAEntities db = new GAEntities())
            {
                result = db.UserInfo.Where(o => o.Username == username).FirstOrDefault();
            }
            return result;
        }

        #endregion

        #region Save

        public static I_BasicResult Save(I_UserSet doc) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            var h = doc.User;

            try {
                using (GAEntities db = new GAEntities()) {
                    var u = db.UserInfo.Where(o => o.Username == h.Username).FirstOrDefault();
                    if (u == null) {
                        doc.User.Password= Hash.hashPassword("MD5","123456");
                        db.UserInfo.Add(doc.User);
                        db.SaveChanges();
                    } else {
                        u.EmpCode = h.EmpCode;
                        u.FirstName = h.FirstName;
                        u.LastName = h.LastName;
                        u.FirstName_En = h.FirstName_En;
                        u.LastName_En = h.LastName_En;
                        u.NickName = h.NickName;
                        u.Gender = h.Gender;
                        u.DepartmentID = h.DepartmentID;
                        u.PositionID = h.PositionID;
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
                        u.ModifiedBy = h.ModifiedBy;
                        u.ModifiedDate = DateTime.Now;
                        u.IsActive = h.IsActive;
                        db.SaveChanges();

                        var rp = InsertAllPermission(doc,h.Username);
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

        public static I_BasicResult InsertAllPermission(I_UserSet input,string username) {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                var menu = ConvertX2UserInGroup(input);
                var rcom = ConvertX2RCompany(input);
                var r1 = InsertUserToGroup(menu, username);
                if (r1.Result == "fail") {
                    r.Result = "fail";
                    r.Message1 = r.Message1 + " " + r1.Message1;
                }
                var r2 = InsertUserToRcom(rcom, username);
                if (r2.Result == "fail")
                {
                    r.Result = "fail";
                    r.Message1 = r.Message1 + " " + r2.Message1;
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

        public static List<UserInGroup> ConvertX2UserInGroup(I_UserSet input) {
            List<UserInGroup> result = new List<UserInGroup>();
            var h = input.User;
            foreach (var s in input.XGroup) {
                var n = new UserInGroup();
                if (s.X) {
                    n.UserName = h.Username;
                    n.UserGroupID = s.UserGroupID;
                    n.RComID = s.RComID;
                    result.Add(n);
                }
            }
            return result;
        }

        public static List<UserInRCom> ConvertX2RCompany(I_UserSet input)
        {
            List<UserInRCom> result = new List<UserInRCom>();
            foreach (var s in input.XRcom)
            {
                if (s.X)
                {
                    var n = new UserInRCom();
                    n.RComID = s.RComID;
                    n.UserName = s.UserName;
                    result.Add(n);
                }
            }
            return result;
        }

        public static I_BasicResult InsertUserToGroup(List<UserInGroup> uigroup, string username) {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                using (GAEntities db = new GAEntities()) {
                    db.UserInGroup.RemoveRange(db.UserInGroup.Where(o => o.UserName == username));
                    db.UserInGroup.AddRange(uigroup);
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

        public static I_BasicResult InsertUserToRcom(List<UserInRCom> uircom, string username)
        {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try
            {
                using (GAEntities db = new GAEntities())
                {
                    db.UserInRCom.RemoveRange(db.UserInRCom.Where(o => o.UserName == username));
                    db.UserInRCom.AddRange(uircom);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                r.Result = "fail";
                if (ex.InnerException != null)
                {
                    r.Message1 = ex.InnerException.ToString();
                }
                else
                {
                    r.Message1 = ex.Message;
                }
            }
            return r;
        }

        #endregion

        public static I_BasicResult ReSetPassword(I_UserSet doc)
        {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try
            {
                using (GAEntities db = new GAEntities())
                {
                    var u = db.UserInfo.Where(o => o.Username == doc.User.Username).FirstOrDefault();
                    u.Password = Hash.hashPassword("MD5", doc.User.Password);
                    u.IsNewUser = doc.User.IsNewUser;
                    db.SaveChanges();

                    //var rs = LogTranService.CreateTransLog(u.username, u.modified_by, "ผู้ใช้งานระบบ", "รีเซ้ตรหัส ผู้ใช้งานระบบ");
                }
            }
            catch (Exception ex)
            {
                result.Result = "fail";
                if (ex.InnerException != null)
                {
                    result.Message1 = ex.InnerException.ToString();
                }
                else
                {
                    result.Message1 = ex.Message;
                }
            }
            return result;
        }
        public static I_BasicResult ChangePassword(ResetPasswordRequest data) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                string cur_pass = Hash.hashPassword("MD5", data.password);
                string new_pass = Hash.hashPassword("MD5", data.NewPassword);

                using (GAEntities db = new GAEntities()) {
                    var u = db.UserInfo.Where(o => o.Username == data.User).FirstOrDefault();
                    if (u == null) {
                        result.Result = "fail";
                        result.Message1 = "No user found.";
                        return result;
                    } else {
                        if (u.Password != cur_pass) {
                            result.Result = "fail";
                            result.Message1 = "The username or password is incorrect.";
                            return result;
                        } else {
                            u.Password = new_pass;
                            u.IsNewUser = false; 
                            db.SaveChanges();
                            //var isvalid_policy = ValidatePasswordPolicy(data.NewPassword);
                            //if (!isvalid_policy) {
                            //    result.Result = "fail";
                            //    result.Message1 = @"<p> Password policy is required";
                            //    result.Message1 = result.Message1 + @"<br> At least one lower case letter,";
                            //    result.Message1 = result.Message1 + @"<br> At least one upper case letter,";
                            //    result.Message1 = result.Message1 + @"<br> At least special character,";
                            //    result.Message1 = result.Message1 + @"<br> At least one number,";
                            //    result.Message1 = result.Message1 + @"<br> At least 8 characters length</p>";

                            //    return result;
                            //} else {
                            //    u.Password = new_pass;
                            //    u.IsNewUser = false;
                            //    //u.last_change_pass = DateTime.Now.Date; 
                            //    db.SaveChanges();
                            //}

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
        #region helper
        static bool ValidatePasswordPolicy(string passwd) {
            if (passwd.Length < 8) {
                return false;
            }
            if (!passwd.Any(char.IsUpper)) {
                return false;
            }
            if (!passwd.Any(char.IsLower)) {
                return false;
            }
            if (Regex.Matches(passwd, "[0-9]").Count == 0) {
                return false;
            }
            bool hasSpecialChar = false;
            string specialCh = @"%!@#$%^&*()?/>.<,:;'\|}]{[_~`+=-" + "\"";
            char[] specialChArray = specialCh.ToCharArray();
            foreach (char ch in specialChArray) {
                if (passwd.Contains(ch))
                    hasSpecialChar = true;
            }
            if (!hasSpecialChar) {
                return false;
            }
            return true;
        }
        #endregion
    }
}
