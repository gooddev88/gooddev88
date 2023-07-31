using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Robot.Data.DataAccess;
using System.Net.Mail;
using System.Net;

namespace Robot.Data.DataAccess {

    public static class EmailServer {
        #region Class

        #endregion


        #region  send mail
        public static List<string> SendMail(string senderMailCode, string receiverMail, string subject, string body, bool isBodyHtml) {
            List<string> result = new List<string>();
            result.Add("");//RESULT ACTION R0=ERROR:R1=SUCCESS
            result.Add("");
            try {
                //Smpt Client Details
                //gmail >> smtp server : smtp.gmail.com, port : 587 , ssl required
                //yahoo >> smtp server : smtp.mail.yahoo.com, port : 587 , ssl required
                using (GAEntities db = new GAEntities()) {
                    var mailconfig = db.MailConfig.Where(o => o.MailCode == "Notify").FirstOrDefault();
                    if (mailconfig == null) {
                        result[0] = "R0";//RESULT ACTION R0=ERROR:R1=SUCCESS
                        result[1] = "Mail sender not config";
                        return result;
                    }

                    SmtpClient clientDetails = new SmtpClient();
                    clientDetails.Port = mailconfig.PortNumber;
                    clientDetails.Host = mailconfig.StmptServer;
                    clientDetails.EnableSsl = mailconfig.EnableSSL;
                    clientDetails.DeliveryMethod = SmtpDeliveryMethod.Network;
                    clientDetails.UseDefaultCredentials = false;
                    clientDetails.Credentials = new NetworkCredential(mailconfig.SenderEmail, mailconfig.SenderPassword.Trim());

                    //Message Details
                    MailMessage mailDetails = new MailMessage();
                    mailDetails.From = new MailAddress(mailconfig.SenderEmail, mailconfig.SenderName);
                    mailDetails.To.Add(receiverMail);
                    //for multiple recipients
                    //mailDetails.To.Add("another recipient email address");
                    //for bcc
                    //mailDetails.Bcc.Add("bcc email address")
                    mailDetails.Subject = subject;
                    mailDetails.IsBodyHtml = isBodyHtml;
                    mailDetails.Body = body;


                    ////file attachment
                    //if (fileName.Length > 0) {
                    //    Attachment attachment = new Attachment(fileName);
                    //    mailDetails.Attachments.Add(attachment);
                    //}

                    clientDetails.Send(mailDetails);
                    result[0] = "R1";//RESULT ACTION R0=ERROR:R1=SUCCESS
                    result[1] = "Email has been sent.";
                }
            } catch (Exception ex) {
                result[0] = "R0";//RESULT ACTION R0=ERROR:R1=SUCCESS
                result[1] = ex.Message;
            }

            return result;
        }
        #endregion

        #region  GET

        public static MailConfig GetDataByID(string ID)
        {
            MailConfig result = new MailConfig();
            using (GAEntities db = new GAEntities())
            {
                result = db.MailConfig.Where(o => o.MailCode == ID).FirstOrDefault();
            }
            return result;
        }

        public static List<MailConfig> ListSearch(string search, bool showInActive)
        {
            List<MailConfig> result = new List<MailConfig>();
            using (GAEntities db = new GAEntities())
            {
                result = db.MailConfig.Where(o =>
                                              (o.MailCode.Contains(search)
                                            || o.SenderEmail.Contains(search)
                                            || o.SenderPassword.Contains(search)
                                            || o.SenderName.Contains(search)
                                            || o.StmptServer.Contains(search)
                                            || o.ProviderID.Contains(search)
                                            || search == "")
                                            && (o.IsActive || showInActive)
                                            ).OrderByDescending(o => o.ID).ToList();
            }
            return result;
        }


        #endregion

    }
}
