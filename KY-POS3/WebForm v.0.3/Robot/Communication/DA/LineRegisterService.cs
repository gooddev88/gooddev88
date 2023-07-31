using Robot.Communication.API.Line;
using Robot.Data;
using Robot.Data.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static Robot.Data.BL.I_Result;

namespace Robot.Communication.DA {
    public static class LineRegisterService {
        #region clss
        public class I_FiterSet {
            public String ActionStatus { get; set; }
            public String Search { get; set; }

        }
        #endregion

        #region query

        public static List<LineLogInRequest> ListLineLogInRequest(string uType, string actionStatus) {
            List<LineLogInRequest> result = new List<LineLogInRequest>();
            using (GAEntities db = new GAEntities()) {
                result = db.LineLogInRequest.Where(o => o.ActionStatus == actionStatus && o.UserType == uType && o.IsActive).ToList();
            }
            return result;
        }
        public static List<vw_LineLogInRequest> ListViewLineLogInRequest(string uType, string actionStatus) {
            List<vw_LineLogInRequest> result = new List<vw_LineLogInRequest>();
            using (GAEntities db = new GAEntities()) {
                result = db.vw_LineLogInRequest.Where(o => o.ActionStatus == actionStatus && o.UserType == uType && o.IsActive).ToList();
            }
            return result;
        }
        public static List<vw_LineLogIn> ListLineLogIn(string uType, string search,bool isShowAll) {
            List<vw_LineLogIn> result = new List<vw_LineLogIn>();
            using (GAEntities db = new GAEntities()) {
                if (search=="") {
                    if (isShowAll) {
                        result = db.vw_LineLogIn.Where(o =>
                                                              o.UserType == uType
                                                              && o.UserType == uType
                                                              && o.IsActive

                                                  ).ToList();
                    } else {
                        result = db.vw_LineLogIn.Where(o =>
                                                           o.UserType == uType
                                                           && o.UserType == uType
                                                           && o.IsActive

                                               ).Take(50).ToList();
                    }
               
                } else {
                    result = db.vw_LineLogIn.Where(o =>
                                                          o.UserType == uType
                                                          && o.UserType == uType 
                                                          && o.IsActive
                                                           &&
                                                                (
                                                                      o.UserFullName.Contains(search)
                                                                      || o.UserID.Contains(search)
                                                              )
                                                           
                                              ).ToList();
                }
           
            }
            return result;
        }

        public static List<LineLogIn> ListLineLogIn()
        {
            List<LineLogIn> result = new List<LineLogIn>();
            using (GAEntities db = new GAEntities())
            {
                result = db.LineLogIn.Where(o => o.IsActive).ToList();
            }
            return result;
        }

        public static List<LineLogInRequest> ListPendingReqWithLineID(string lineId, string uType) {
            List<LineLogInRequest> result = new List<LineLogInRequest>();
            using (GAEntities db = new GAEntities()) {
                result = db.LineLogInRequest.Where(o => o.LineID == lineId && o.UserType == uType && o.IsActive && o.ActionStatus == "PENDING").ToList();
            }
            return result;
        }

        public static UserInfo GetUserInfoWithAuthen(string username, string password) {
            UserInfo user = new UserInfo();
            username = username.ToLower();
            try {
                using (GAEntities db = new GAEntities()) {
                    string encrypt_password = EncryptService.hashPassword("MD5", password);
                    user = db.UserInfo.Where(o =>
                                                    (
                                                                o.Username.ToLower() == username || o.EmpCode.ToLower() == username
                                                    )
                                              && o.Password == encrypt_password
                                              && o.IsActive
                                            ).FirstOrDefault();



                }
            } catch (Exception ex) {

            }
            return user;

        }

        public static LineLogInRequest GetReq(string reqId) {
            LineLogInRequest r = new LineLogInRequest();
            try {
                using (GAEntities db = new GAEntities()) {
                    r = db.LineLogInRequest.Where(o => o.ReqID == reqId).FirstOrDefault();
                }
            } catch (Exception ex) { }
            return r;

        }

        public static vw_LineLogIn GetLogin(int id) {
            vw_LineLogIn r = new vw_LineLogIn();

            using (GAEntities db = new GAEntities()) {
                r = db.vw_LineLogIn.Where(o => o.ID == id).FirstOrDefault();
            }

            return r;

        }

        #endregion

        #region Create Delete transaction
        public static I_BasicResult CreateUserLogInRequest(string userId, string lineId, string appId, string memo) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                using (GAEntities db = new GAEntities()) {
                    var onPending = db.LineLogInRequest.Where(o => o.IsActive && o.UserID == userId && o.LineID==lineId && o.AppID == appId && o.ActionStatus == "PENDING").FirstOrDefault();
                    if (onPending!=null) {
                        result.Result = "fail";
                        result.Message1 = "เคยยืนคำขอไปเมื่อ " + onPending.RequestDate.ToString("dd/MM/yyyy HH:mm") +" กรุณารอการอนุมัติ";
                        return result;
                    }
                    var onAccept = db.LineLogInRequest.Where(o => o.IsActive && o.UserID == userId && o.LineID == lineId && o.AppID == appId && o.ActionStatus == "ACCEPTED").FirstOrDefault();
                    if (onAccept != null) {
                        var onMemberWithAnthorLine = db.LineLogIn.Where(o => o.IsActive && o.UserID == userId && o.LineID!= lineId).FirstOrDefault();
                        if (onMemberWithAnthorLine!=null) {
                            result.Result = "fail";
                            result.Message1 = "ชื่อถูกผูกกับบัญชี Line อื่นกรุณาแจ้งเจ้าหน้าที่เพื่อปลดล๊อกบัญชีเก่าออกก่อนลงทะเบียนใหม่";
                            return result;
                        }
                       
                    } else {
                        var onSussessButNoHasLogin = db.LineLogIn.Where(o => o.IsActive && o.UserID == userId && o.LineID == lineId).FirstOrDefault();
                        if (onSussessButNoHasLogin!=null) {
                            result.Result = "fail";
                            result.Message1 = "ชื่อและบัญชี Line นี้ถูกผูกถูกความสัมพันธ์กันแล้ว";
                            return result;
                        }

                    }
                    var user = UserService.GetUserInfo(userId);
                    var rq = NewLineLogInRequest("USER", appId.ToUpper());
                    rq.LineID = lineId;
                    rq.UserID = userId;
                    rq.AutoDesc = user.FullName + " ส่งคำขอใช้บริการน้องโมผ่าน Line";
                    rq.ReqMemo = memo;
                    rq.AppID = appId.ToUpper();
                    db.LineLogInRequest.Add(rq);
                    db.SaveChanges();
                    //  var chkDupReq = db.LineLogInRequest.Where(o => o.IsActive && o.UserID == userId && o.AppID == appId && o.ActionStatus == "PENDING").FirstOrDefault();
                    //if (chkDupReq == null) {// ยังไม่เคย Request

                    //    //var myMsg = LineMsgAPI.MessageHelper("รอการอนุมัติก่อนนะคะ " + user.FullName  );
                    //    //result = LineMsgAPI.SendLineMsgAPI(myMsg, lineId, "MODERN_HR");
                    //    LineMsgAPI.SendLineGroupMsgOnly(user.FullName + " No. " + user.EmpCode + " ขอใช้บริการ Modern HR " + Environment.NewLine + "อนุมัติได้ที่ " + @"https://mhrprv.gooddev.net", "MODERN_HR");//send notify to group admin
                    //} else {//อยู่ระหว่างการอนุมัติ
                    //    result.Result = "fail";
                    //    result.Message1 = "เคยยืนคำขอไปแล้วเมื่อ " + chkDupReq.RequestDate.ToString("dd/MM/yyyy HH:mm");
                    //    return result;
                    //}


                }
            } catch (Exception ex) {
                result.Result = "fail";
                if (ex.InnerException != null) {
                    result.Message1 = result.Message1 + ex.InnerException.ToString();
                } else {
                    result.Message1 = result.Message1 + ex.Message;
                }
            }
            return result;

        }


        public static I_BasicResult ActionRequest(string action, string reqId, string memo) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                using (GAEntities db = new GAEntities()) {
                    var rq = db.LineLogInRequest.Where(o => o.ReqID == reqId).FirstOrDefault();
                    rq.ActionStatus = action.ToUpper();
                    rq.ActionBy = LoginService.LoginInfo.CurrentUser;
                    rq.ActionDate = DateTime.Now;
                    rq.ActionMemo = memo;
                    db.SaveChanges();

                    if (action == "ACCEPTED") {
                        var login = NewLineLogIn(rq);
                        db.LineLogIn.Add(login);
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


        public static I_BasicResult DeleteLineLogIn(int id, string appId) {
            ;
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                using (GAEntities db = new GAEntities()) {
                    var login = db.LineLogIn.Where(o => o.ID == id).FirstOrDefault();
                    login.IsActive = false;
                    var rq = db.LineLogInRequest.Where(o => o.ReqID == login.FromReqID).ToList();
                    foreach (var q in rq) {
                        q.IsActive = false;
                    }
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
        #endregion


        #region transaction 
        public static LineLogIn NewLineLogIn(LineLogInRequest rq) {
            LineLogIn n = new LineLogIn();

            n.RComID = rq.RComID;
            n.ComID = rq.ComID;
            n.AppID = rq.AppID;
            n.FromReqID = rq.ReqID;
            n.UserType = rq.UserType;
            n.LineID = rq.LineID;
            n.UserID = rq.UserID;
            n.ReqMemo = rq.ReqMemo;
            n.ApprovedMemo = rq.ActionMemo;
            n.RequestDate = rq.RequestDate;
            n.ApprovedBy = "";
            n.ApprovedDate = null;
            n.IsActive = true;
            return n;
        }
        public static LineLogInRequest NewLineLogInRequest(string userType, string appId) {
            LineLogInRequest n = new LineLogInRequest();

            n.ReqID = Guid.NewGuid().ToString();
            n.RComID = "PRV";
            n.ComID = "";
            n.UserType = userType;
            n.AppID = appId;
            n.LineID = "";
            n.UserID = "";
            n.AutoDesc = "";
            n.ReqMemo = "";
            n.RequestDate = DateTime.Now;
            n.ActionStatus = "PENDING";
            n.ActionBy = "";
            n.ActionDate = null;
            n.ActionMemo = "";
            n.IsActive = true;
            return n;
        }

        public static I_FiterSet NewFilterSet() {
            I_FiterSet n = new I_FiterSet();
            n.ActionStatus = "PENDING";
            n.Search = "";
            return n;
        }
        #endregion
    }
}