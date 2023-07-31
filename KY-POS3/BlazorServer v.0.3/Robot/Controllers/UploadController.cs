using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Robot.Data.DA;
using Robot.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static Robot.Data.DA.XFilesService;

namespace Robot.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : Controller {

        private readonly IWebHostEnvironment _hostingEnvironment;
        public XFilesService _xfileService { get; set; }
        public UploadController(IWebHostEnvironment hostingEnvironment, XFilesService xfileService) {
             _xfileService = xfileService;
            _hostingEnvironment = hostingEnvironment;
        }
        public IActionResult Index() {
            return View();
        }

        [HttpPost("UploadFile")]
        public ActionResult UploadFile(IFormFile myFile, [FromQuery] string docid) {
            try {
                var NameNoExt = Path.GetFileNameWithoutExtension(myFile.FileName);
                var ExtOnly = Path.GetExtension(myFile.FileName); 

                //var imageResourcePath = @"\TempFile"; 
                //var path = Path.Combine(@"D:\XFiles", imageResourcePath);
                var path = @"D:\XFiles\TempFile";
                if (!Directory.Exists(path)) { 
                    Directory.CreateDirectory(path);
                } 
                var filePath = Path.Combine(path, myFile.FileName);
                 
                using (var fileStream = System.IO.File.Create(filePath)) {
                    myFile.CopyTo(fileStream);
                }
             
            } catch {
                Response.StatusCode = 400;
            }

            return new EmptyResult();
        }

            [HttpGet("GetFileFromTempFile/{filename}")]
        public ActionResult GetFileFromTempFile(string filename) {
            try {
                var filePath = Path.Combine(_hostingEnvironment.WebRootPath, "TempFile", filename);

                byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);

                return File(fileBytes, "application/force-download", filename);

            } catch {
                Response.StatusCode = 400;
            }

            return new EmptyResult();
        }




        [HttpPost("UploadFileToByte")]
        public ActionResult UploadFileToByte(IFormFile myFile, [FromQuery] string info) {
            //docid:rcom:com:user;
            try {

                var fileLoc = FileService.GetCurrentFileLocation();
                if (fileLoc == null) {
                    Response.StatusCode = 400;
                }  
                string docref = "";
                string rcom = "";
                string com = "";
                string user = "";
                string[] doc = info.Split(":");
                int i = 0;
                foreach (string d in doc) {
                    if (i == 0) {
                        docref = d;
                    }
                    if (i == 1) {
                        rcom = d;
                    }
                    if (i == 2) {
                        com = d;
                    }
                    if (i == 3) {
                        user = d;
                    }
                    i++;
                }

                string filename = Path.GetFileNameWithoutExtension(myFile.FileName);
                string fileext = Path.GetExtension(myFile.FileName);

                XFilesSet DocSet = XFilesService.NewTransaction(rcom, user);
                _xfileService.UploadInfo = XFilesService.NewTemplateUploadItemsProfile(rcom, docref, com, user); 
                if (myFile.Length == 0) {
                    Response.StatusCode = 400;
                } 
                using (var ms = new MemoryStream()) {
                    myFile.CopyTo(ms);
                    DocSet.XFiles.Data = ms.ToArray();
                    if (_xfileService.UploadInfo.XFilesRef.FileType=="IMG") {
                        DocSet.XFiles.DataThumb = ImageService.SaveCroppedImage(DocSet.XFiles.Data, 200, 200, "");
                    }
                    
                    DocSet.XFiles.OriginFileExt = fileext;
                    DocSet.XFiles.FileExt = fileext;
                    DocSet.XFiles.OriginFileName = filename;
                    DocSet.XFilesRef = XFilesService.CopyUploadInfo2FileRef(_xfileService.UploadInfo.XFilesRef, DocSet.XFilesRef);
                } 
                var r = XFilesService.Save(DocSet, _xfileService.UploadInfo.UploadOption.IsReplace);
            } catch {
                Response.StatusCode = 400;
            }

            return new EmptyResult();
        }


        protected string ContentRootPath { get; set; }

        public string GetOrCreateUploadFolder() {
            ContentRootPath = _hostingEnvironment.ContentRootPath;
            var path = Path.Combine(ContentRootPath, "uploads");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            return path;
        }
        [HttpPost("DeleteFile")]
        public async Task<ActionResult<object>> DeleteFile(IFormFile file, [FromQuery] string uuid) {
            var imageResourcePath = @"assets\files";
            var fileName = $"{uuid}{Path.GetExtension(file.FileName)}";
            try {
                //  var path = Path.Combine(_hostingEnvironment.WebRootPath, imageResourcePath);
                var path = Path.Combine(@"D:\XFiles", imageResourcePath);
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                var filePath = Path.Combine(path, fileName);
                using (var fileStream = System.IO.File.Create(filePath)) {
                    await file.CopyToAsync(fileStream);
                }
            } catch {
                Response.StatusCode = 400;
            }
            var xx = Ok();
            return xx;
        }
    }
}
