using Robot.Data.GADB.TT;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using static Robot.Data.ML.I_Result;

namespace Robot.Service.Mail {
    public class EmailHelper {
        public class SendMailData {
            public string send_to { get; set; }
            public List<string> filename { get; set; }
            public string topic { get; set; }
        }
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
        public static I_BasicResult SendMail(SendMailData data) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                #region create Mail Data

                MailDataSet m = NewMailDataSet();
                result = m.OutputAction;
                if (m.OutputAction.Result == "fail") {
                    return result;
                }
                m.MailSubject = data.topic;
                m.AttachmentFile = data.filename;
                m.SendTo = data.send_to;
                if (data.filename==null) {
                    data.filename = new List<string>();
                }
                m.MailBody = NewBodyInvalid(data.filename);


                #endregion

                var rm = SubmitMail(m);
                if (rm.Result == "fail") {
                    result.Result = rm.Result;
                    result.Message1 = result.Message1 + " " + rm.Message1;
                }


            } catch (Exception ex) {
                result.Result = "fail";
                if (ex.InnerException != null) {
                    result.Message1 = result.Message1 + " " + ex.InnerException.ToString();
                } else {
                    result.Message1 = ex.Message;
                }
            }
            return result;
        }

        public static string NewBodyInvalid(List<string> filename) {
            string r = "xxxx";
            r = @"<html>
                      <body>
                      <h5>Dear xxxx </h5>  
                     To open and correct the attached invalid file format, download from attached files <br> <br> 
                     or direct open the Excel file in <br><br>  ";
            foreach (var f in filename) {
                r = r + @"<span>" + f + "</span> <br> ";
            }
            r = r + @"<br><br> Please manual import this Excel file again after corrected file format<br> <br> 
                               <p>Regards,</p>
                      </body>
                      </html>";
            return r;
        }

        public static I_BasicResult SubmitMail(MailDataSet data) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                data.Password = data.Password == "*" ? "" : data.Password;
                data.SendTo = data.SendTo.Replace(';', ',');
                using (MailMessage mm = new MailMessage(data.SendFrom, data.SendTo)) {
                    mm.IsBodyHtml = true;
                    mm.Subject = data.MailSubject;
                    mm.Body = data.MailBody;
                    data.AttachmentFile = data.AttachmentFile == null ? new List<string>() : data.AttachmentFile;
                    //foreach (var a in data.AttachmentFile) {
                    //    string filename = Path.GetFileName(a);
                    //    FileStream fs = new FileStream(a, FileMode.Open, FileAccess.Read);
                    //    MemoryStream ms = new MemoryStream();
                    //    fs.CopyTo(ms);
                    //    mm.Attachments.Add(new Attachment(fs, filename));
                    //}
                    foreach (var a in data.AttachmentFile) { 
                        //byte[] bytes = Convert.FromBase64String(a); 
                        //Stream stream = new MemoryStream(bytes);
                        //mm.Attachments.Add(new Attachment(stream, filename));

                        Attachment attachment = new Attachment(a);
                        attachment.TransferEncoding = TransferEncoding.Base64;
                        mm.Attachments.Add(attachment);
                    }
                    //foreach (var a in data.AttachmentFile) {
                    //    Attachment attachment = new System.Net.Mail.Attachment(a);
                    //    mm.Attachments.Add(attachment);
                    //}

                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = data.Smtp;
                    smtp.UseDefaultCredentials = false;
                    NetworkCredential NetworkCred = new NetworkCredential(data.SendFrom, data.Password);
                    smtp.Credentials = NetworkCred;

                    if (data.Password == "") {
                        smtp.EnableSsl = false;
                    } else {
                        smtp.EnableSsl = true;
                    }


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
        public static bool IsValidEmail(string email) {
            try {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            } catch {
                return false;
            }
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
                var mailconfig = db.mail_sender.FirstOrDefault();
                if (mailconfig == null) {
                    n.OutputAction.Result = "fail";
                    n.OutputAction.Message1 = "No Email sender config.";
                    return n;
                }

                n.SendFrom = mailconfig.mail_user;
                n.Password = mailconfig.password;
                n.SenderName = mailconfig.sender_name;
                n.Smtp = mailconfig.smtp;
                n.Port = Convert.ToInt32(mailconfig.port);
                n.UserCredentials = Convert.ToBoolean(mailconfig.use_credentials);
                n.SendTo = "";
            }



            return n;
        }
        #endregion
    }
}
