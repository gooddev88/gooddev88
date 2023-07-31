using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Robot.Data.DA;
using Robot.Helper;
using Robot.Service.Api;
using Robot.Service.FileGo;
using Robot.Service.FileGo.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static Robot.Data.ML.I_Result;

namespace Robot.Controllers {
    [Route("api/file/[controller]")]
    [ApiController]
    public class UploadController : Controller {


        private readonly IWebHostEnvironment _hostingEnvironment;
        private FileGo _filego;
        private ClientService _clientService;
        public UploadController(IWebHostEnvironment hostingEnvironment, FileGo filego, ClientService clientService) {
            _filego = filego;
            _clientService = clientService;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpPost("Save")]
        async public Task<IActionResult> Save(IFormFile files, [FromQuery] string file_info,string token_filego) {
            // docref: rcom: com: user: doctype
            if (files == null) {
                return new EmptyResult();
            }
            try {
                var info = GetFileInfo(file_info);
                var upload_file = new List<FilesInfo>();


                string filename = Path.GetFileNameWithoutExtension(files.FileName);
                string fileext = Path.GetExtension(files.FileName);

                using var ms = new MemoryStream();
                files.CopyTo(ms);

                var nfile = FileGo.NewFilesInfo(info.doctype, info.rcom, info.com, info.docref);
                nfile.data = Convert.ToBase64String(ms.ToArray());
                nfile.file_type = files.ContentType;
                nfile.fileName = files.FileName;
                upload_file.Add(nfile);
                 await Task.Run(()=> _filego.UploadFileGo(upload_file, info.user,token_filego));
               
            } catch {
                Response.StatusCode = 400;
            }

            return new EmptyResult();
        }

        [HttpPost("SaveToFileGo")]
        async public Task<IActionResult> SaveToFileGo(List<FilesInfo>  files, [FromQuery] string user, string token_filego) {
            // docref: rcom: com: user: doctype
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            if (files == null) {
                r.Result = "ok";
                r.Message1 = "file not found.";
                return   BadRequest(r);
            }
            try { 
             r=   await Task.Run(() => _filego.UploadFileGo(files, user, token_filego));

            } catch( Exception ex ){
                r.Result = "fail";
                if (ex.InnerException!=null) {
                    r.Message1 = ex.InnerException.ToString();
                } else {
                    r.Message1 = ex.Message;
                }
             
            } 
            return Ok(r);
        }

        public static FileInfo GetFileInfo(string file_info) {
            FileInfo r = new FileInfo();
            string[] doc = file_info.Split(":");
            int i = 0;
            foreach (string d in doc) {
                if (i == 0) {
                    r.docref = d;
                }
                if (i == 1) {
                    r.rcom = d;
                }
                if (i == 2) {
                    r.com = d;
                }
                if (i == 3) {
                    r.user = d;
                }
                if (i == 4) {
                    r.doctype = d;
                }
                i++;
            }
            return r;
        }
        public class FileInfo {
            public string docref { get; set; }
            public string rcom { get; set; }
            public string com { get; set; }
            public string user { get; set; }

            public string doctype { get; set; }
        }

        //async public Task<I_BasicResult> UploadFileGo(List<FilesInfo> files, string user) {
        //    I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
        //    try {
        //        var login_filego = FileGo.GetFileGoLogin();
        //        string dataEndpointUri = $"{login_filego.RootUrl}/api/xfiles/XFilesService/UploadFileToDB";

        //        var query = await Task.Run(() => _clientService.Post<I_BasicResult>(dataEndpointUri, files));
        //        if (query.StatusCode != "OK") {
        //            r.Result = "fail";
        //            r.Message1 = query.StatusCode;
        //        } else {
        //            var rr = (I_BasicResult)query.Result;
        //            if (rr.Result == "fail") {
        //                r.Result = "fail";
        //                r.Message1 = rr.Message1;
        //                //ShowMessage(false, update_result.Message1);
        //            } else {
        //                //ShowMessage(true, "Upload Success");
        //                var xfile_ref = FileGo.Convert2XFilesRef(files, user);
        //                var rrr = FileGo.SaveXFileRef(xfile_ref);
        //                if (rrr.Result == "fail") {
        //                    r.Result = "fail";
        //                    r.Message1 = rrr.Message1;
        //                }
        //            }
        //        }
        //    } catch (Exception ex) {
        //        r.Result = "fail";
        //        if (ex.InnerException != null) {
        //            r.Message1 = ex.InnerException.ToString();
        //        } else {
        //            r.Message1 = ex.Message;
        //        }
        //    }
        //    return r;
        //}

        //  [HttpPost]
        //  public async Task<IActionResult> Save(IFormFile files) // must match SaveField
        //{
        //      if (files != null) {
        //          try {
        //              // save to wwwroot - Blazor Server only
        //              var saveLocation = Path.Combine(HostingEnvironment.WebRootPath, files.FileName);
        //              // save to project root - Blazor Server or WebAssembly
        //              //var saveLocation = Path.Combine(HostingEnvironment.ContentRootPath, files.FileName);

        //              using (var fileStream = new FileStream(saveLocation, FileMode.Create)) {
        //                  await files.CopyToAsync(fileStream);
        //              }
        //          } catch {
        //              Response.StatusCode = 500;
        //              await Response.WriteAsync("Upload failed.");
        //          }
        //      }

        //      return new EmptyResult();

        //  }



        //public IActionResult Index() {
        //    return View();
        //}

        //[HttpPost("UploadFile")]
        //public ActionResult UploadFile(IFormFile myFile, [FromQuery] string docid) {
        //    try {
        //        var NameNoExt = Path.GetFileNameWithoutExtension(myFile.FileName);
        //        var ExtOnly = Path.GetExtension(myFile.FileName); 

        //        //var imageResourcePath = @"\TempFile"; 
        //        //var path = Path.Combine(@"D:\XFiles", imageResourcePath);
        //        var path = @"D:\XFiles\TempFile";
        //        if (!Directory.Exists(path)) { 
        //            Directory.CreateDirectory(path);
        //        } 
        //        var filePath = Path.Combine(path, myFile.FileName);

        //        using (var fileStream = System.IO.File.Create(filePath)) {
        //            myFile.CopyTo(fileStream);
        //        }

        //    } catch {
        //        Response.StatusCode = 400;
        //    }

        //    return new EmptyResult();
        //}

        //    [HttpGet("GetFileFromTempFile/{filename}")]
        //public ActionResult GetFileFromTempFile(string filename) {
        //    try {
        //        var filePath = Path.Combine(_hostingEnvironment.WebRootPath, "TempFile", filename);

        //        byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);

        //        return File(fileBytes, "application/force-download", filename);

        //    } catch {
        //        Response.StatusCode = 400;
        //    }

        //    return new EmptyResult();
        //}




        //[HttpPost("UploadFileToByte")]
        //public ActionResult UploadFileToByte(IFormFile myFile, [FromQuery] string docid) {
        //    //docid:rcom:com:user;
        //    try {

        //        var fileLoc = XFilesService.GetCurrentFileLocation();
        //        if (fileLoc == null) {
        //            Response.StatusCode = 400;
        //        }


        //        string docref = "";
        //        string rcom = "";
        //        string com = "";
        //        string user = "";
        //        string doctype = "";
        //        string[] doc = docid.Split(":");
        //        int i = 0;
        //        foreach (string d in doc) {
        //            if (i == 0) {
        //                docref = d;
        //            }
        //            if (i == 1) {
        //                rcom = d;
        //            }
        //            if (i == 2) {
        //                com = d;
        //            }
        //            if (i == 3) {
        //                user = d;
        //            }
        //            if (i == 4)
        //            {
        //                doctype = d;
        //            }
        //            i++;
        //        }

        //        string filename = Path.GetFileNameWithoutExtension(myFile.FileName);
        //        string fileext = Path.GetExtension(myFile.FileName);

        //        XFilesSet DocSet = XFilesService.NewTransaction(rcom, user);

        //        switch (doctype)
        //        {
        //            case "ITEMS_PHOTO_PROFILE":
        //                _xfileService.UploadInfo = XFilesService.NewTemplateUploadItemsProfile(rcom, com, docref, user);
        //                break;
        //            case "COMPANY":
        //                _xfileService.UploadInfo = XFilesService.NewTemplateUploadCompanyProfile(rcom, com, docref, user);
        //                break;            
        //            default:
        //                _xfileService.UploadInfo = XFilesService.NewTemplateUploadItemsProfile(rcom, com, docref, user);
        //                break;
        //        }


        //        if (myFile.Length == 0) {
        //            Response.StatusCode = 400;
        //        }

        //        using (var ms = new MemoryStream()) {
        //            myFile.CopyTo(ms);
        //            DocSet.XFiles.Data = ms.ToArray();
        //            if (_xfileService.UploadInfo.XFilesRef.FileType == "IMG") {
        //                DocSet.XFiles.DataThumb = ImageService.SaveCroppedImage(DocSet.XFiles.Data, 200, 200, "");
        //            }

        //            myFile.CopyTo(ms);
        //            DocSet.XFiles.Data = ms.ToArray();
        //            DocSet.XFiles.OriginFileExt = fileext;
        //            DocSet.XFiles.FileExt = fileext;
        //            DocSet.XFiles.OriginFileName = filename;
        //            DocSet.XFilesRef = XFilesService.CopyUploadInfo2FileRef(_xfileService.UploadInfo.XFilesRef, DocSet.XFilesRef);
        //        }

        //        var r = XFilesService.Save(DocSet, _xfileService.UploadInfo.UploadOption.IsReplace);
        //    } catch {
        //        Response.StatusCode = 400;
        //    }

        //    return new EmptyResult();
        //}


        //protected string ContentRootPath { get; set; }

        //public string GetOrCreateUploadFolder() {
        //    ContentRootPath = _hostingEnvironment.ContentRootPath;
        //    var path = Path.Combine(ContentRootPath, "uploads");
        //    if (!Directory.Exists(path))
        //        Directory.CreateDirectory(path);
        //    return path;
        //}
        //[HttpPost("DeleteFile")]
        //public async Task<ActionResult<object>> DeleteFile(IFormFile file, [FromQuery] string uuid) {
        //    var imageResourcePath = @"assets\files";
        //    var fileName = $"{uuid}{Path.GetExtension(file.FileName)}";
        //    try {
        //        //  var path = Path.Combine(_hostingEnvironment.WebRootPath, imageResourcePath);
        //        var path = Path.Combine(@"D:\XFiles", imageResourcePath);
        //        if (!Directory.Exists(path))
        //            Directory.CreateDirectory(path);

        //        var filePath = Path.Combine(path, fileName);
        //        using (var fileStream = System.IO.File.Create(filePath)) {
        //            await file.CopyToAsync(fileStream);
        //        }
        //    } catch {
        //        Response.StatusCode = 400;
        //    }
        //    var xx = Ok();
        //    return xx;
        //}
    }
}
