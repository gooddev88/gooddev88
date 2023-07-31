using Robot.Data;
using Robot.Data.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static Robot.Data.BL.I_Result;

namespace Robot.Communication.DA {

    public static class MailGroupInfoService {

        #region Class
        public class IFilterSet {
            public string GroupID { get; set; }
            public string SearchText { get; set; }
        }
        #endregion

        #region  send mail
        public static I_BasicResult Save(MailGroupReceiver i) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                using (GAEntities db = new GAEntities()) {
                    var uh = db.MailGroupReceiver.Where(o => o.MailGroupID == i.MailGroupID && o.Email == i.Email && o.IsActive).FirstOrDefault();
                    if (uh == null) {
                        db.MailGroupReceiver.Add(i);
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

        public static I_BasicResult GetMailInUser(string email) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };

            try {
                using (GAEntities db = new GAEntities()) {
                    var u = db.UserInfo.Where(o => o.Email.ToLower() == email.ToLower()).FirstOrDefault();
                    if (u == null) {
                        result.Result = "fail";
                        result.Message1 = "No user use this email: " + email;
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

        #region  GET

        public static MailGroupReceiver GetMailGroupReceiver(string groupId) {
            MailGroupReceiver result = new MailGroupReceiver();
            using (GAEntities db = new GAEntities()) {
                result = db.MailGroupReceiver.Where(o => o.MailGroupID == groupId).FirstOrDefault();
            }
            return result;
        }

        public static List<MailGroupReceiver> ListMailGroupReceiver(string groupId) {
            List<MailGroupReceiver> result = new List<MailGroupReceiver>();
            using (GAEntities db = new GAEntities()) {
                result = db.MailGroupReceiver.Where(o => o.MailGroupID == groupId && o.IsActive).OrderBy(o=>o.Email).ToList();
            }
            return result;
        }


        public static List<MailGroupReceiver> ListMailGroupReceiver(IFilterSet f) {
            List<MailGroupReceiver> result = new List<MailGroupReceiver>();
            using (GAEntities db = new GAEntities()) {
                result = db.MailGroupReceiver.Where(o =>
                                              (  o.Email.Contains(f.SearchText)
                                                    || o.Remark.Contains(f.SearchText)
                                                    || f.SearchText == ""
                                            )
                                            && o.IsActive
                                            && (o.MailGroupID == f.GroupID || f.GroupID == "")
                                            ).OrderByDescending(o => o.ID).ToList();
            }
            return result;
        }

        public static List<UserInfo> ListUserFromMailGroup(string mailGroupId) {
            List<UserInfo> result = new List<UserInfo>();
            using (GAEntities db = new GAEntities()) {
                var qMail = db.MailGroupReceiver.Where(o =>
                                                          o.IsActive
                                                          && (o.MailGroupID == mailGroupId)
                                                          ).Select(o => o.Email).Distinct().ToList();

                result = db.UserInfo.Where(o => qMail.Contains(o.Email)).ToList();
                //var xx = LoginService.ListUserInCompany("");
            }
            return result;
        }
        public static List<UserInfo> ListAllEmailUser() {
            List<UserInfo> result = new List<UserInfo>();
            using (GAEntities db = new GAEntities()) {
                  result = db.UserInfo.Where(o =>
                                                          o.IsActive
                                                          && o.Status!= "EXPIRE"
                                                          && o.Email!=""
                                                          ).ToList();

         
            }
            return result;
        }

        public static MailGroupReceiver GetMailInGroup(string email, string MailGroupReceiverID) {
            MailGroupReceiver result = new MailGroupReceiver();
            using (GAEntities db = new GAEntities()) {
                result = db.MailGroupReceiver.Where(o => o.Email == email && o.MailGroupID == MailGroupReceiverID && o.IsActive).FirstOrDefault();
            }
            return result;
        }


        #endregion

        public static I_BasicResult DeleteMailInGroup(int id) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };


            try {
                using (GAEntities db = new GAEntities()) {
                    DateTime modytime = DateTime.Now;
                    string modyby = LoginService.GetCurrentloginUser();

                    var MailGroupReceiver = db.MailGroupReceiver.Where(o => o.ID == id).FirstOrDefault();

                    db.MailGroupReceiver.Remove(MailGroupReceiver);
                    db.SaveChanges();

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
        public static bool IsValidEmail(string email) {
            try {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            } catch {
                return false;
            }
        }
        #endregion
        #region transaction
        public static IFilterSet NewFilter() {
            IFilterSet r = new IFilterSet();
            r.GroupID = "";
            r.SearchText = "";
            return r;
        }
        public static MailGroupReceiver NewTransaction() {
            MailGroupReceiver n = new MailGroupReceiver();
            n.MailGroupID = "";
            n.ComID = "";
            n.RComID = LoginService.LoginInfo.CurrentRootCompany;
            n.Email = "";
            n.Remark = "";
            n.CreatedBy = LoginService.GetCurrentloginUser();
            n.CreatedDate = DateTime.Now;
            n.IsActive = true;

            return n;
        }
        #endregion

    }
}