using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RobotAPI.Data.DA.Xfiles;
using RobotAPI.Data.DA.Xfiles.Model;
using RobotAPI.Data.XFilesCenterDB.TT;
using RobotAPI.Services.Image;

using static RobotAPI.Data.DA.Xfiles.XFilesService;
using static RobotAPI.Data.ML.Shared.I_Result;

namespace RobotAPI.Controllers.Xfiles {
    [Route("api/xfiles/[controller]")]
    [ApiController]
    public class XFilesServiceController : BaseApiController {

        private readonly IWebHostEnvironment env;

        public XFilesServiceController(IWebHostEnvironment env) {
            this.env = env;
        }


        [AllowAnonymous]
        [HttpPost]
        [Route("CreateFileRef")]
        public IActionResult CreateFileRef(xfiles_ref data) {
            var output = XFilesService.CreateFileRef(data); 
            return Ok(output);
        }


        #region donwload get
        [AllowAnonymous]
        [HttpGet("GetFileInByte/{file_id}")]
        public ActionResult GetFileInByte(string file_id) {
            xfiles? f = new xfiles();
            try {
                f = XFilesService.GetFile(file_id, false);
            } catch {
                return BadRequest(f);
            }
            return Ok(f);
        }

        [AllowAnonymous]
        [HttpGet("GetFile/{file_id}")]
        public ActionResult GetFile(string file_id) {
            //get file
            try {
                var file = XFilesService.GetFile(file_id, false);
                return File(file.data, file.file_type, file.origin_filename + file.origin_file_ext);
            } catch {
                Response.StatusCode = 400;
            }
            return new EmptyResult();
        }


        [AllowAnonymous]
        [HttpGet("GetThumb/{file_id}")]
        public ActionResult GetThumb(string file_id) {
            //get thumnail image
            try {
                var file = XFilesService.GetFile(file_id, true);
                return File(file.data_thumb, file.file_type, file.origin_filename + file.origin_file_ext);
            } catch {
                Response.StatusCode = 400;
            }
            return new EmptyResult();
        }

        #endregion



        #region upload / delete
        [AllowAnonymous]
        [HttpPost("UploadFileToByte")]
        public ActionResult UploadFileToByte(IFormFile myFile, [FromQuery] string file_id) {
            //ใช้สำหรับ dxupload ของ devexpress
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = file_id };
            try {
                if (myFile.Length == 0) {
                    result.Result = "fail";
                    result.Message1 = "incomplete file";
                    return BadRequest(result);
                }
                var fileLoc = XFilesService.GetCurrentFileLocation();
                if (fileLoc == null) {
                    result.Result = "fail";
                    result.Message1 = "file store location not found.";
                    return BadRequest(result);
                }

                xfiles_ref? file_info = XFilesService.GetFileRef(file_id);
                if (file_info == null) {
                    result.Result = "fail";
                    result.Message1 = "file ref not found.";
                    return BadRequest(result);
                }
                XFilesSet DocSet = new XFilesSet();
                DocSet.XFilesRef = XFilesService.GetFileRef(file_id);

                DocSet.XFiles = XFilesService.NewXFiles(DocSet.XFilesRef);
                DocSet.XFiles.origin_filename = Path.GetFileNameWithoutExtension(myFile.FileName); ;
                DocSet.XFiles.origin_file_ext = Path.GetExtension(myFile.FileName);
                DocSet.XFiles.file_name = Guid.NewGuid().ToString().ToLower();
                DocSet.XFiles.file_ext = Path.GetExtension(myFile.FileName);
                //DocSet.XFilesRef.upload_completed_time = DateTime.Now; 

                using (var ms = new MemoryStream()) {
                    myFile.CopyTo(ms);
                    DocSet.XFiles.data = ms.ToArray();
                    if (DocSet.XFilesRef.file_type == "image") {
                        DocSet.XFiles.data_thumb = ImageService.SaveCroppedImage(DocSet.XFiles.data, 200, 200, "");
                    }
                }
                var r = XFilesService.Save(DocSet);
            } catch (Exception e) {
                result.Result = "fail";
                if (e.InnerException != null) {
                    result.Message1 = e.InnerException.ToString();
                } else {
                    result.Message1 = e.Message;
                }

                return BadRequest(result);

            }

            return Ok(result);
        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("UploadFileToDB")]
        public ActionResult UploadFileToDB(FilesInfo[] files) {
            //ใช้สำหรับระบบอัพโหลดข้อขึ้น Document ทั่วไป
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                foreach (var f in files) {
                    //string file_id = "";
                    var f_refResult = XFilesService.CreateFileRef(XFilesService.Convert2xFile_ref(f));
                    f.file_id = string.IsNullOrEmpty(f.file_id) ? f_refResult.Message2 : f.file_id;

                    if (f.data.Length == 0) {
                        result.Result = "fail";
                        result.Message1 = "incomplete file";
                        return BadRequest(result);
                    }
                    var fileLoc = XFilesService.GetCurrentFileLocation();
                    if (fileLoc == null) {
                        result.Result = "fail";
                        result.Message1 = "file store location not found.";
                        return BadRequest(result);
                    }

                    xfiles_ref? file_info = XFilesService.GetFileRef(f.file_id);
                    if (file_info == null) {
                        result.Result = "fail";
                        result.Message1 = "file ref not found.";
                        return BadRequest(result);
                    }
                    XFilesSet DocSet = new XFilesSet();
                    DocSet.XFilesRef = XFilesService.GetFileRef(f.file_id);

                    DocSet.XFiles = XFilesService.NewXFiles(DocSet.XFilesRef);
                    DocSet.XFiles.origin_filename = Path.GetFileNameWithoutExtension(f.fileName); ;
                    DocSet.XFiles.origin_file_ext = Path.GetExtension(f.fileName);
                    DocSet.XFiles.file_name = Guid.NewGuid().ToString().ToLower();
                    DocSet.XFiles.file_ext = Path.GetExtension(f.fileName);
                    //DocSet.XFilesRef.upload_completed_time = DateTime.Now; 

                    using (var ms = new MemoryStream()) {

                        DocSet.XFiles.data = Convert.FromBase64String(f.data);
                        //   DocSet.XFiles.data = f.data;
                        if (DocSet.XFilesRef.file_type.StartsWith("image")) {
                            DocSet.XFiles.data_thumb = ImageService.SaveCroppedImage(DocSet.XFiles.data, 200, 200, "");
                        }
                    }
                    var r = XFilesService.Save(DocSet);
                }
            } catch (Exception e) {
                result.Result = "fail";
                if (e.InnerException != null) {
                    result.Message1 = result.Message1 + " : " + e.InnerException.ToString();
                } else {
                    result.Message1 = result.Message1 + " : " + e.Message;
                }

                return BadRequest(result);

            }

            return Ok(result);
        }


        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("DeleteFile")]
        public ActionResult DeleteFile([FromBody] List<string> file_ids) {
            //ลบไฟล์จาก file_id
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                result = XFilesService.DeleteFile(file_ids);
            } catch (Exception e) {
                result.Result = "fail";
                if (e.InnerException != null) {
                    result.Message1 = result.Message1 + " : " + e.InnerException.ToString();
                } else {
                    result.Message1 = result.Message1 + " : " + e.Message;
                }

                return BadRequest(result); 
            } 
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost("UploadFileToDisk")]
        async public Task<ActionResult> UploadFileToDisk([FromBody] FilesInfo[] files) {
            //ทดสอบsave ไฟล์ลอง server
            foreach (var file in files) {
               var buf = Convert.FromBase64String(file.data);
                // var buf = file.data;
                await System.IO.File.WriteAllBytesAsync(env.ContentRootPath + System.IO.Path.DirectorySeparatorChar + Guid.NewGuid().ToString("N") + "-" + file.fileName, buf);
            }
            return Ok();
        }

        #endregion
    




        //[AllowAnonymous]
        //[HttpGet("DownloadFile/{file_id}")]
        //public ActionResult DownloadFile(string file_id) {
        //    FileContentResult output = null;
        //    try {
        //        var file = XFilesService.GetFile(file_id, false);
        //        output = File(file.data, file.file_type, file.origin_filename + file.origin_file_ext);
        //    } catch {
        //        return BadRequest(output);
        //    }
        //    return Ok(output);
        //}
        //[AllowAnonymous]
        //[HttpGet("GetImage/{file_id}")]
        //public IActionResult GetImage() {
        //    Byte[] b = System.IO.File.ReadAllBytes(@"E:\\Test.jpg");   // You can use your own method over here.         
        //    return File(b, "image/jpeg");
        //}
        //[AllowAnonymous]
        //[HttpPost("UploadFile")]
        //public ActionResult UploadFile(IFormFile myFile, [FromQuery] string info) {
        //    I_BasicResult output_result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
        //    List<ItemPriceInfo> price_list = new List<ItemPriceInfo>();
        //    try {

        //        var upinfo = UploadLogService.GetUploadInfo(info);
        //        var NameNoExt = Path.GetFileNameWithoutExtension(myFile.FileName);
        //        var ExtOnly = Path.GetExtension(myFile.FileName);
        //        var path = @"D:\XFiles\TempFile";
        //        if (!Directory.Exists(path)) {
        //            Directory.CreateDirectory(path);
        //        }
        //        var filePath = Path.Combine(path, myFile.FileName);

        //        using (var fileStream = System.IO.File.Create(filePath)) {
        //            myFile.CopyTo(fileStream);
        //        }
        //        //var myExcel=    System.IO.File.ReadAllBytes(filePath);

        //        using (ExcelPackage excelPackage = new ExcelPackage()) {
        //            using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)) {
        //                excelPackage.Load(fileStream);
        //            }

        //            ExcelWorksheet excelWorksheet = excelPackage.Workbook.Worksheets[1];

        //            if (excelWorksheet == null) {
        //                //ShowAlert("ไม่พบ Sheet TRIPRATE", "Error");
        //                output_result.Result = "fail";
        //                output_result.Message1 = "Error sheet name";
        //                return Ok(output_result);
        //            }

        //            I_BasicResult read_result;
        //            price_list = ItemPriceService.ReadSheetItemPriceFile(excelWorksheet, upinfo.rcom, out read_result);
        //            UploadLogService.CreateLog(upinfo.uploadid, "item_price", read_result.Result, read_result.Message1);
        //            if (read_result.Result == "fail") {
        //                return Ok(read_result);
        //            }
        //        }
        //        var rs = ItemPriceService.SaveBulkV2(price_list);
        //    } catch (Exception e) {
        //        output_result.Result = "fail";
        //        output_result.Message1 = e.Message;
        //        return BadRequest(output_result);
        //    }
        //    return Ok(output_result);
        //}
        //[AllowAnonymous]
        //[HttpGet("GetFileInfo/{file_id}")]
        //public ActionResult GetFileInfo(string file_id) {
        //    vw_files? inf = new vw_files();
        //    try {
        //        inf = XFilesService.GetFileInfo(file_id);
        //    } catch {
        //        return BadRequest(inf);
        //    }
        //    return Ok(inf);
        //}
    }
}
