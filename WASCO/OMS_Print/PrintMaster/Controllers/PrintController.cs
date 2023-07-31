using Microsoft.AspNetCore.Mvc;
using PrintMaster.Data.DA;
using PrintMaster.Data.PrintDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace PrintMaster.Controllers {
    public class PrintController : ApiController {
        // GET: Print

        public I_BasicResult Post([FromBody] PrintData data) {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                r = PrintService.CreatePrintData(data);
            } catch (Exception e) {
                r.Result = "fail";
                r.Message1 = e.Message;
            }
            return r;
        }
    public I_BasicResult Get(  ) {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
              var  k = PrintService.ClearOldPrintData();
            } catch (Exception e) {
                r.Result = "fail";
                r.Message1 = e.Message;
            }
            return r;
        }

    }
}