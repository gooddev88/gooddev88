using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using OfficeOpenXml.Table;
using Robot.Data.GADB.TT;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Robot.Controllers {
    [Route("api/Files/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private const string XlsxContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        public FilesController(IWebHostEnvironment hostingEnvironment) {
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpPost("DownloadExcel")]
        public IActionResult DownloadExcel([FromBody]  List<CompanyInfo> vv) {
            byte[] reportBytes; 
            using (var package = Utils.createExcelPackage(vv)) {
                reportBytes = package.GetAsByteArray();
            } 
            return File(reportBytes, XlsxContentType, $"MyReport{DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)}.xlsx");
        }
        public class Utils {
            public static ExcelPackage createExcelPackage(List<CompanyInfo> vv) {
              
                var package = new ExcelPackage();
                package.Workbook.Properties.Title = "Company List";
       
                var worksheet = package.Workbook.Worksheets.Add("Comapny"); 
                //First add the headers
                worksheet.Cells[1, 1].Value = "ComanyID";
                worksheet.Cells[1, 2].Value = "Name";
               

                //Add values

                //var numberformat = "#,##0";
                //var dataCellStyleName = "TableNumber";
                //var numStyle = package.Workbook.Styles.CreateNamedStyle(dataCellStyleName);
                //numStyle.Style.Numberformat.Format = numberformat;

                int i = 2;
                foreach (var v in vv) {
                    worksheet.Cells[i, 1].Value = v.CompanyID;
                    worksheet.Cells[i, 2].Value = v.Name1+" "+v.Name2; 
                    i++;
                }
                

                // Add to table / Add summary row
                var tbl = worksheet.Tables.Add(new ExcelAddressBase(fromRow: 1, fromCol: 1, toRow: vv.Count, toColumn: 13), "Data");
                //tbl.ShowHeader = true;
                //tbl.TableStyle = TableStyles.Dark9;
                //tbl.ShowTotal = true;
                //tbl.Columns[3].DataCellStyleName = dataCellStyleName;
                //tbl.Columns[3].TotalsRowFunction = RowFunctions.Sum;
                //worksheet.Cells[5, 4].Style.Numberformat.Format = numberformat;

                // AutoFitColumns
                worksheet.Cells[1, 1, vv.Count, 15].AutoFitColumns();

                return package;
            }
        }
    }
}
