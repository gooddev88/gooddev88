using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc; 
using System;

namespace Robot.Controllers.MASTER {
    [Route("api/master/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase {
        [HttpGet("DownloadItemImage/{rcom}/{itemid}")]
        public ActionResult DownloadItemImage(string rcom, string itemid) {
            try {
                var img_base64 = XFilesService.GetFileRefByDocAndTableSource2B64(rcom, "", itemid, "ITEMS_PHOTO_PROFILE", false, false);
                var fileext = Helper.ImageService.GetFileExtensionFromBase64(img_base64);
                byte[] bytes = Convert.FromBase64String(img_base64);
                return File(bytes, $"image/{fileext}", itemid + "." + fileext);
            } catch {
                Response.StatusCode = 400;
            }

            return new EmptyResult();
        }
    }
}
