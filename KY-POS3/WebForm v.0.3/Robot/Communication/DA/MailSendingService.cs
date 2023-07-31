using Robot.Data;
using Robot.Data.DataAccess;
using Robot.HREmpData.Data.DA;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using static Robot.Data.BL.I_Result;

namespace Robot.Communication.DA {
    public static class MailSendingService {

        public class MailAddressBook {
            public string Email { get; set; }
            public string Username { get; set; }
            public string Fullname { get; set; }
        }
        public class MailDataSet {
            public string SendFrom { get; set; }
            public string SenderName { get; set; }
            public string Password { get; set; }
            public string SendTo { get; set; }
            public string MailSubject { get; set; }
            public string MailBody { get; set; }
            public List<string> AttachmentFile { get; set; }
            public string Smtp { get; set; }
            public int Port { get; set; }
            public bool UserCredentials { get; set; }
            public I_BasicResult OutputAction { get; set; }
        }


        public static bool IsValidEmail(string email) {
            try {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            } catch {
                return false;
            }
        }
        public static  I_BasicResult CreateMail(string mailGroupId) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                //01 get ประเภทการส่งเมล
                var groupInfo = MasterTypeInfoV2Service.GetType("MAIL_GROUP", mailGroupId);

                //02 list mail ผู้รับ

                var users = MailGroupInfoService.ListUserFromMailGroup(mailGroupId);
                var receiverMail = users.Select(o => o.Email).ToList();
                // string onlineReceiverMail = CollapseMailInSingleReceiver(receiverMail);


                //Send mail
                foreach (var u in users) {
                    if (!IsValidEmail(u.Email)) {
                        continue;
                    }
                    #region create Mail Data

                    MailDataSet m = NewMailDataSet();
                    m.MailSubject = groupInfo.Description1;
                    m.SendTo = u.Email;
                    m.MailBody = NewBodyLicense(mailGroupId, u.Username);
                    #endregion

                    SubmitMail(m);
                    // var rr = Task.Run(() => SubmitMail(m));   


                    //04 save send noti
                    SaveNoti(u.Username);


                    //05 save mail log
                    MailSendLog log = new MailSendLog {
                        SendTo = u.Email,
                        SendFrom = m.SendFrom,
                        SendDate = DateTime.Now,
                        MailBody = m.MailBody,
                        IsActive = true
                    };
                    SaveLog(log);
                }
            } catch (Exception ex) {
                result.Result = "ok";
                if (ex.InnerException != null) {
                    result.Message1 = ex.InnerException.ToString();
                } else {
                    result.Message1 = ex.Message;
                }
            }
            return result;
        }
        public static I_BasicResult CreateMailBirthDay() {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {

                //01 list mail all

              //  var users = MailGroupInfoService.ListAllEmailUser();
              //var receiverMail = users.Select(o => o.Email).ToList();
              //  string onlineReceiverMail = CollapseMailInSingleReceiver(receiverMail);



                //01 get ประเภทการส่งเมล
                var groupInfo = MasterTypeInfoV2Service.GetType("MAIL_GROUP", "BIRTHDAY");

                //02 list mail ผู้รับ

                var users = MailGroupInfoService.ListUserFromMailGroup("BIRTHDAY");
                var receiverMail = users.Select(o => o.Email).ToList();
                // string onlineReceiverMail = CollapseMailInSingleReceiver(receiverMail);


                var str_month = DatetimeInfoService.ListEngMonthSelect().Where(o => o.ValueNum == DateTime.Now.Month).FirstOrDefault();
                foreach (var u in users) {
                    if (!IsValidEmail(u.Email)) {
                        continue;
                    }
                    #region create Mail Data

                    MailDataSet m = NewMailDataSet();
                    m.MailSubject = "สุขสันต์วันเกิดประจำเดือน" + str_month.Desc;
                    m.SendTo = u.Email;
                    m.MailBody = NewBodyBirthDay(  u.Username, DateTime.Now.Month);
                    #endregion

                    SubmitMail(m);
                    // var rr = Task.Run(() => SubmitMail(m));   


                    //04 save send noti
                    SaveNoti(u.Username);


                    //05 save mail log
                    MailSendLog log = new MailSendLog {
                        SendTo = u.Email,
                        SendFrom = m.SendFrom,
                        SendDate = DateTime.Now,
                        MailBody = m.MailBody,
                        IsActive = true
                    };
                    SaveLog(log);
                }












                ////Send mail


                //#region create Mail Data
                //var str_month = DatetimeInfoService.ListEngMonthSelect().Where(o => o.ValueNum == DateTime.Now.Month).FirstOrDefault();
                //    MailDataSet m = NewMailDataSet();
                //    m.MailSubject = "สุขสันวันเกิดประจำเดือน " + str_month.Desc;
                //    m.SendTo = onlineReceiverMail;
                //    m.MailBody = NewBodyBirthDay(DateTime.Now.Month);
                //    #endregion

                //    SubmitMail(m);
                //    // var rr = Task.Run(() => SubmitMail(m));   
                     
                    
          
               
            } catch (Exception ex) {
                result.Result = "ok";
                if (ex.InnerException != null) {
                    result.Message1 = ex.InnerException.ToString();
                } else {
                    result.Message1 = ex.Message;
                }
            }
            return result;
        }

        public static string NewBodyLicense(string groupId, string userId) {
            string r = "";
            //string baseUrl = Request.Url.GetLeftPart(UriPartial.Authority);
            var baseurl = LinkService.GetLinkByLinkInfo("app_baseurl").BaseUrl;
            switch (groupId) {

                case "LICENSE_NOTIFY":
                    r = @"<html>
                      <body>
                      <h5>สวัสดีท่านผู้เกี่ยวข้องทุกท่าน </h5> 
                      <h2>แจ้งเตือนบัตรอนุญาตหมดอายุ </h2> 
                      ตรวจสอบข้อมูลได้จาก Link ด้านล่าง <br>
                        ";
                    r = r + baseurl + $"/HREmpData/BirthDayWarning?id={userId}";
                    DateTime checkDate = DateTime.Now.Date;


                    var listLicense = LicenseService.ListLicenseNoti(userId).ToList();
                    var comIds = listLicense.Select(o => (string)o.CompanyID).Distinct().ToList();

                    foreach (var cId in comIds) {//loop by หน่วยงาน\
                        //render comifno
                        var cominfo = listLicense.Where(o => o.CompanyID == cId).FirstOrDefault();
                        r = r + @"<h3> @" + cominfo.CompanyName + " (" + cominfo.CompanyID + ")" + "  </h3>";

                        var cardTypeId = listLicense.Where(o => o.CompanyID == cId).Select(o => o.LicenseTypeID).Distinct().ToList();

                        foreach (var ctId in cardTypeId) {//loop by ประเภทอนุญาต
                                                          //render license type
                            var cardTypeInfo = listLicense.Where(o => o.CompanyID == cId && o.LicenseTypeID == ctId).FirstOrDefault();
                            r = r + @"<h5>" + cardTypeInfo.LicenseName + "</h5>  ";
                            var licenseS = listLicense.Where(o => o.CompanyID == cId && o.LicenseTypeID == ctId).ToList();
                            foreach (var u in licenseS) {
                                r = r + @" <strong>" + u.FullName + "</strong> จะหมดอายุเมื่อ <strong>" + u.ValidTo.ToString("dd/MM/yyyy") + "</strong>  <br>";
                            }
                        }
                    }
                    r = r + @"<h6>ขอขอบคุณ</h6>
                                <p> Modern HR</p>
                      </body>
                      </html>";


                    break;
                default:
                    break;
            }
            return r;

        }
        public static string NewBodyBirthDay( string userId, int month) {
            string r = "";
            //string baseUrl = Request.Url.GetLeftPart(UriPartial.Authority);
            var baseurl = LinkService.GetLinkByLinkInfo("app_baseurl").BaseUrl; 
            var str_month = DatetimeInfoService.ListEngMonthSelect().Where(o => o.ValueNum == DateTime.Now.Month).FirstOrDefault();

            r = @"<html>
                      <body>
                      <h5>สวัสดีทุกๆท่าน </h5> 
                      <h2>ขอแสดงความยินดีและอวยพรให้กับผู้ที่เกิดประจำเดือน  ";
                    r = r + str_month.Desc + "</h2> ";
                    r = r + "ขอให้ท่านประสบแต่สิ่งดีๆ เนื่องในวันเกิดนี้ค่ะ <br>";
                    r = r + "ตรวจสอบข้อมูลได้จาก Link ด้านล่าง <br>";
                    r = r + baseurl + $"/HREmpData/EmpBirthdayWarning?id={userId}&month={month.ToString()}";
                    DateTime checkDate = DateTime.Now.Date;


            var emps = EmpV2Service.ListEmployeeByBirthday(userId,month);
            var comIds = emps.Select(o => (string)o.CompanyID).Distinct().ToList();

                    foreach (var cId in comIds) {//loop by หน่วยงาน\
                        //render comifno
                        var cominfo = emps.Where(o => o.CompanyID == cId).FirstOrDefault();
                        r = r + @"<h3> " + cominfo.CompanyName + " (" + cominfo.CompanyID + ")" + "  </h3>";

                //var cardTypeId = emps.Where(o => o.CompanyID == cId).Select(o => o.LicenseTypeID).Distinct().ToList();
                var empinCom = emps.Where(o => o.CompanyID == cId).ToList();
                        foreach (var e in empinCom) {//loop by ประเภทอนุญาต
                                                          //render license type
                            var empInfo = emps.Where(o => o.CompanyID == cId && o.Username == e.Username).FirstOrDefault();
                    r = r + @"<br> <span>" + empInfo.FullName + "   วันที่ " + Convert.ToDateTime(e.Birthdate).Day + " " + str_month.Desc + "</span> ";
                        }
                    }
                    r = r + @"<h6>ขอขอบคุณ</h6>
                                <p> Modern HR</p>
                      </body>
                      </html>";

 
            return r;

        }

        //public static string NewBodyBirthDay(int month) {
        //    string r = "";
        //    //string baseUrl = Request.Url.GetLeftPart(UriPartial.Authority);
        //    var baseurl = LinkService.GetLinkByLinkInfo("app_baseurl").BaseUrl;
        //    var emps = EmpV2Service.ListEmployeeByBirthday(month );
        //    var str_month = DatetimeInfoService.ListEngMonthSelect().Where(o => o.ValueNum == DateTime.Now.Month).FirstOrDefault();
        //    r = @"<html>
        //              <body>
        //              <h5>สวัสดีทุกๆท่าน </h5> 
        //              <h2>ขอแสดงความยินดีและอวยพรให้กับผู้ที่เกิดประจำเดือน  ";
        //    r = r + str_month.Desc + "</h2> ";
        //    r = r + "ขอให้ท่านประสบแต่สิ่งดีๆ เนื่องในวันเกิดนี้ค่ะ";  
        //    var comIds = emps.Select(o => (string)o.CompanyID).Distinct().ToList();
        //    foreach (var cId in comIds) {//loop by หน่วยงาน 
        //        var cominfo = emps.Where(o => o.CompanyID == cId).FirstOrDefault();
        //        r = r + @"<h3> @" + cominfo.CompanyName + " (" + cominfo.CompanyID + ")" + "  </h3>";

        //        var emp = emps.Where(o => o.CompanyID == cId).OrderBy(o=>o.BirthDateInMonth).ToList();
        //        foreach (var e in emp) {//loop by ประเภทอนุญาต
        //            r = r + @"<br> <span>" + e.FullName +"   วันที่ "+Convert.ToDateTime(e.Birthdate ).Day+ " "+ str_month.Desc + "</span> ";
        //        }
        //    }
        //    r = r + @"<h4>ขอขอบคุณ</h4>
        //             <p> Modern HR</p>
        //              </body>
        //              </html>";


         

        //    return r;

        //}
        public static I_BasicResult SubmitMail(MailDataSet data) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {

                using (MailMessage mm = new MailMessage(data.SendFrom, data.SendTo)) {
                    mm.IsBodyHtml = true;
                    mm.Subject = data.MailSubject;
                    mm.Body = data.MailBody;

                    foreach (var a in data.AttachmentFile) {
                        string filename = Path.GetFileName(a);
                        FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read);
                        MemoryStream ms = new MemoryStream();
                        fs.CopyTo(ms);
                        mm.Attachments.Add(new Attachment(fs, filename));
                    }

                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = data.Smtp;
                    smtp.EnableSsl = true;
                    NetworkCredential NetworkCred = new NetworkCredential(data.SendFrom, data.Password);
                    smtp.UseDefaultCredentials = true;
                    smtp.Credentials = NetworkCred;
                    smtp.Port = data.Port;
                    smtp.Send(mm);
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
        public static I_BasicResult SaveNoti(string user) {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };

            try {
                using (GAEntities db = new GAEntities()) {
                    var listLicense_id = LicenseService.ListLicenseNoti(user).Select(o => o.ID);
                    var cc = db.EmpLicense.Where(o => listLicense_id.Contains(o.ID)).ToList();
                    foreach (var c in cc) {
                        c.SendNotifyDate = DateTime.Now;
                    }

                    db.SaveChanges();
                }
            } catch (Exception ex) {
                r.Result = "fail";
                if (ex.InnerException != null) {
                    r.Message1 = ex.InnerException.ToString();
                } else {
                    r.Message1 = "";
                }
            }
            return r;
        }
        public static I_BasicResult SaveLog(MailSendLog log) {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };

            //MailSendLog log = new MailSendLog {
            //    SendTo = "",
            //    SendFrom = "",
            //    SendDate = DateTime.Now,
            //    MailBody = "",
            //    IsActive = true

            //};
            try {
                using (GAEntities db = new GAEntities()) {
                    db.MailSendLog.Add(log);
                    db.SaveChanges();
                }
            } catch (Exception ex) {
                r.Result = "fail";
                if (ex.InnerException != null) {
                    r.Message1 = ex.InnerException.ToString();
                } else {
                    r.Message1 = "";
                }
            }
            return r;

        }

        #region NEWTRANSACTION
        public static MailDataSet NewMailDataSet() {
            MailDataSet n = new MailDataSet();
            n.SendFrom = "";
            n.SenderName = "";
            n.Password = "";
            n.SendTo = "";
            n.MailSubject = "";
            n.MailBody = "";
            n.AttachmentFile = new List<string>();
            n.Smtp = "";
            n.Port = 0;
            n.UserCredentials = false;
            n.OutputAction = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            using (GAEntities db = new GAEntities()) {
                var sender = db.MailSenderConfig.Where(o => o.MailCode == "hr_notify").FirstOrDefault();
                n.SendFrom = sender.SenderEmail;
                n.Password = sender.SenderPassword;
                n.SenderName = sender.SenderName;
                n.Smtp = sender.SmtptServer;
                n.Port = sender.PortNumber;
                n.UserCredentials = sender.UseDefaultCredentials;
            }
            return n;
        }


        #endregion

        #region Tools
        public static string CollapseMailInSingleReceiver(List<string> input) {
            string r = "";
            int j = 0;
            foreach (var i in input) {
                if (j == 0) {
                    r = i;
                } else {
                    r = r + "," + i;
                }
                j++;
            }

            return r;
        }
        #endregion

    }




}