using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OfficeOpenXml;
using Robot.Data.DA;
using Robot.Data.DA.Stock;
using Robot.Data.GADB.TT;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static Robot.Data.ML.I_Result;

namespace Robot.Controllers.STOCK {
    [Route("api/stock/[controller]")]
    [ApiController]
    public class StockAdjController : Controller {

        private readonly IWebHostEnvironment _hostingEnvironment;
        public StockAdjController(IWebHostEnvironment hostingEnvironment) {
            _hostingEnvironment = hostingEnvironment;
        }
        public IActionResult Index() {
            return View();
        }

        //[HttpPost("UploadItem")]
        //public ActionResult UploadItem(IFormFile myFile, [FromQuery] string info) {
        //    I_BasicResult output_result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
        //    //List<ItemPriceInfo> price_list = new List<ItemPriceInfo>();
        //    try {
               
        //        //var upinfo = UploadLogService.GetUploadInfo(info);
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
        //            //price_list = ItemPriceService.ReadSheetItemPriceFile(excelWorksheet, upinfo.rcom, out read_result);
        //            //UploadLogService.CreateLog(upinfo.uploadid, "item_price", read_result.Result, read_result.Message1);
        //            //if (read_result.Result == "fail") {
        //            //    return Ok(read_result);
        //            //}
        //        }
        //        //var rs = ItemPriceService.SaveBulkV2(price_list);
        //    } catch (Exception e) {
        //        output_result.Result = "fail";
        //        output_result.Message1 = e.Message;
        //        return BadRequest(output_result);
        //    }
        //    return Ok(output_result);
        //}

        [HttpGet("DownloadItem/{rcom}/{com}/{loc}")]
        public ActionResult DownloadItem(string rcom,string com,string loc) {
            try {
                string filename = "ItemsAdjust-" + DateTime.Now.Year.ToString("0000") + DateTime.Now.Month.ToString("00") + DateTime.Now.Day.ToString("00")+".xlsx";
                var stream = StkAdjustService.DownloadStockAdjustExcel(rcom,com, loc);
                return File(stream.ToArray(), "application/force-download", filename); 
            } catch {
                Response.StatusCode = 400;
            } 
            return new EmptyResult();
        }
     [HttpGet("DownloadItemTransfer/{rcom}/{com}/{locfr}/{locto}")]
        public ActionResult DownloadItemTransfer(string rcom,string com,string locfr,string locto) {
            try {
                string filename = "ItemsTransfer-" + DateTime.Now.Year.ToString("0000") + DateTime.Now.Month.ToString("00") + DateTime.Now.Day.ToString("00")+".xlsx";
                var stream = StkAdjustService.DownloadStockTransferExcel(rcom,com, locfr,locto);
                return File(stream.ToArray(), "application/force-download", filename); 
            } catch {
                Response.StatusCode = 400;
            } 
            return new EmptyResult();
        }

    }
}
