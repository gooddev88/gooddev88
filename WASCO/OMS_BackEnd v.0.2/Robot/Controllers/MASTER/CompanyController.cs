using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Robot.Data.DA.FileStore;
using System;

namespace Robot.Controllers.MASTER {
    [Route("api/master/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase {
        [HttpGet("DownloadCompanyImage/{rcom}/{comid}")]
        public ActionResult DownloadCompanyImage(string rcom, string comid) {
            try {
                var img_base64 = XFilesService.GetFileRefByDocAndTableSource2B64(rcom, "", comid, "COMPANY_PHOTO_PROFILE", false, false);
                var fileext = Helper.ImageService.GetFileExtensionFromBase64(img_base64);
                byte[] bytes = Convert.FromBase64String(img_base64);
                return File(bytes, $"image/{fileext}", comid + "." + fileext);
            } catch {
                Response.StatusCode = 400;
            }

            return new EmptyResult();
        }
    }
}
