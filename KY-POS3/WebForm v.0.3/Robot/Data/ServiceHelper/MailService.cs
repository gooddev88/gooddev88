using Robot.Data.DataAccess;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace Robot.Data.ServiceHelper {

    public class MailService {

        public class MailDataSet {
            public string SendFrom { get; set; }
            public string Password { get; set; }
            public List<string> SendTo { get; set; }
            public string MailSubject { get; set; }
            public string MailBody { get; set; }
            public List<string> AttachmentFile { get; set; }
            public string Smtp { get; set; }
            public int Port { get; set; }
        }

        public List<string> SendMail(MailDataSet mail_data) {
            List<string> result = new List<string> { "R0", "" };
            try {
                //var email_acc = SendNotifyInfoService.GetNotifySender_BySenderID("asset_report");
                string send_to = TextService.SemiStringFromListString(mail_data.SendTo);
                using (MailMessage mm = new MailMessage(mail_data.SendFrom, send_to)) {
                    mm.Subject = mail_data.MailSubject;
                    mm.Body = mail_data.MailBody;

                    foreach (var a in mail_data.AttachmentFile) {
                        string filename = Path.GetFileName(a);
                        FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read);
                        MemoryStream ms = new MemoryStream();
                        fs.CopyTo(ms);
                        mm.Attachments.Add(new Attachment(fs, filename));
                    }




                    mm.IsBodyHtml = false;
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = mail_data.Smtp;
                    smtp.EnableSsl = true;
                    NetworkCredential NetworkCred = new NetworkCredential(mail_data.SendFrom,mail_data.Password);
                    smtp.UseDefaultCredentials = true;
                    smtp.Credentials = NetworkCred;
                    smtp.Port = mail_data.Port;

                     
                    smtp.Send(mm); 
                  
                }
            
            } catch (Exception ex) {
                result[0] = "R0";
                result[1] = ex.Message;
            }
                return result;
        }


        //public   List<string> CreateMail(string subject, string body, List<string> files) {
        //    List<string> result = new List<string> { "R1", "" };
        //    var senderdata = SendNotifyInfoService.GetNotifySender_BySenderID("asset_report");
        //    var sendtodata = SendNotifyInfoService.ListNotifyByFunction("").Select(o => (string)o.NotifyValue).ToList();
        //    MailDataSet mail_data = new MailDataSet {
        //        SendFrom = senderdata.Username,
        //        Password=senderdata.Password,
        //        SendTo = sendtodata,
        //        MailSubject = subject,
        //        MailBody = body,
        //        AttachmentFile = files,
        //        Smtp=senderdata.Smtp,
        //        Port=Convert.ToInt32( senderdata.Port)
        //    };
        //    MailService mail = new MailService();

        //    result = mail.SendMail(mail_data);
        //    return result;
        //}


    }
}