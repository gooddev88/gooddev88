using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RobotWasm.Client.Service.Api;
using RobotWasm.Shared.Data.ML.FileGo;
using static RobotWasm.Shared.Data.ML.Shared.I_Result;

namespace RobotWasm.Server.Controllers.FileGo {
    [Route("api/file/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private Service.FileGo.FileGoService _filego;
        private ClientService _clientService;
        public UploadController(IWebHostEnvironment hostingEnvironment, Service.FileGo.FileGoService filego, ClientService clientService) {
            _filego = filego;
            _clientService = clientService;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpPost("Save")]
        async public Task<IActionResult> Save(IFormFile files, [FromQuery] string file_info, string token_filego) {
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

                var nfile = Service.FileGo.FileGoService.NewFilesInfo(info.doctype, info.rcom, info.com, info.docref);
                nfile.data = Convert.ToBase64String(ms.ToArray());
                nfile.file_type = files.ContentType;
                nfile.fileName = files.FileName;
                upload_file.Add(nfile);
                await Task.Run(() => _filego.UploadFileGoSQL(upload_file, info.user, token_filego));

            } catch {
                Response.StatusCode = 400;
            }

            return new EmptyResult();
        }

        [HttpPost("SaveToFileGo")]
        async public Task<IActionResult> SaveToFileGo(List<FilesInfo> files, [FromQuery] string user, string token_filego) {
            // docref: rcom: com: user: doctype
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            if (files == null) {
                r.Result = "ok";
                r.Message1 = "file not found.";
                return BadRequest(r);
            }
            try {
                r = await Task.Run(() => _filego.UploadFileGoSQL(files, user, token_filego));

            } catch (Exception ex) {
                r.Result = "fail";
                if (ex.InnerException != null) {
                    r.Message1 = ex.InnerException.ToString();
                } else {
                    r.Message1 = ex.Message;
                }

            }
            return Ok(r);
        }
        [HttpPost("SaveToFileGoPostgres")]
        async public Task<IActionResult> SaveToFileGoPostgres(List<FilesInfo> files, [FromQuery] string user) {
            // docref: rcom: com: user: doctype
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            if (files == null) {
                r.Result = "ok";
                r.Message1 = "file not found.";
                return BadRequest(r);
            }
            try {
                r = await Task.Run(() => _filego.UploadFileGoPostgres(files, user));

            } catch (Exception ex) {
                r.Result = "fail";
                if (ex.InnerException != null) {
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
    }
}
