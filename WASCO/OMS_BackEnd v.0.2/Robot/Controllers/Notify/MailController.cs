using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Robot.Service.Mail;
using System;
using static Robot.Data.ML.I_Result;
using static Robot.Service.Mail.EmailHelper;

namespace Robot.Controllers.Notify {
    [Route("api/notify/[controller]")]
    [ApiController]
    public class MailController : ControllerBase {

        [HttpPost("Send")]
        public I_BasicResult Send(SendMailData data) {
            I_BasicResult output = new I_BasicResult();
            try {
                output = EmailHelper.SendMail(data);
            } catch  { 
            }
            return output;
        }
    }
}
