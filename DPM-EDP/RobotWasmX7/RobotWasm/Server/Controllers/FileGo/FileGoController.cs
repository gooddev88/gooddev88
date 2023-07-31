using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RobotWasm.Server.Service.FileGo;
using RobotWasm.Shared.Data.DimsDB;
using RobotWasm.Shared.Data.ML.FileGo;
using static RobotWasm.Shared.Data.ML.Shared.I_Result;

namespace RobotWasm.Server.Controllers.FileGo {
    [Route("api/[controller]")]
    [ApiController]
    public class FileGoController : ControllerBase {
        private readonly FileGoService _filego;
        public FileGoController(FileGoService filego) {
            _filego = filego;   
        }
        
        [HttpGet("LoginApiFileGo")]
    async     public   Task<I_BasicResult>  LoginApiFileGo() { 
           return  await _filego.LoginApiFileGo(); 
        }

        [HttpGet("NewFilesInfoPostgres")]
        async public Task<FilesInfo> NewFilesInfoPostgres(string doctype, string rcom, string? com, string docid) {
            com = com == null ? "" : com;
            return FileGoService.NewFilesInfoPostgres(doctype,rcom,com,docid);
        }
        


        [HttpGet("GetFileUrlPostgres")]
        public string GetFileUrlPostgres(string file_id) {
            return FileGoService.GetFileUrlPostgres(file_id);
        }

        [HttpGet("GetXfileRefPostgres")]
        async public Task<vw_xfile_ref> GetXfileRefPostgres(string rcom, string? com, string doctype, string docid)
        {
            com = com == null ? "" : com;
            return FileGoService.GetXfileRefPostgres(rcom, com, doctype, docid);
        }


        [HttpGet("GetFileInBytePostgres")]
        async public Task<xfiles> GetFileInBytePostgres(string rcom, string? com, string doctype, string docid)
        {
            com = com == null ? "" : com;
            return await _filego.GetFileInBytePostgres(rcom, com, doctype, docid);
        }
        

        [HttpGet("DeleteFileByFileIdPostgres")]
        async public Task<I_BasicResult> DeleteFileByFileIdPostgres(string rcom, string? com, string doctype, string docid, string fileid, string deleted_by) {
            return await _filego.DeleteFileByFileIdPostgres(rcom, com, doctype, docid, fileid, deleted_by);
        }
        

    }
}
