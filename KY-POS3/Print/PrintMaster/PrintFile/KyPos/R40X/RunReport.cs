using Newtonsoft.Json;
using PrintMaster.Data.PrintDB; 
using Robot.PrintFile.KyPos.R40X;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using static PrintMaster.PrintFile.KyPos.R401X.Model;

namespace PrintMaster.PrintFile.KyPos.R40X {
    public class RunReport {
     public const string BasePath = @"~/TempFile/Print/";

        public static R401 OpenReportR401(PrintData print_row ,string isExport ) {
            var doc = JsonConvert.DeserializeObject<I_POSSaleSetX>(print_row.JsonData);
            List<int> print_list = new List<int> { 0, 1 }; 
            R401 report0 = new R401(doc);
            if (isExport=="1") {
                MemoryStream stream = new MemoryStream();
                report0.ExportToPdf(stream);
                ExportPdf(stream,print_row.PrintID);
            }
            report0.CreateDocument();
            return report0;
        }

        public static R402 OpenReportR402(PrintData print_row, string isExport) {
            var doc = JsonConvert.DeserializeObject<I_POSSaleSetX>(print_row.JsonData);
            List<int> print_list = new List<int> { 0, 1 };

            R402 report0 = new R402(doc);
            if (isExport == "1") {
                MemoryStream stream = new MemoryStream();
                report0.ExportToPdf(stream);
                ExportPdf(stream, print_row.PrintID);
            }
            report0.CreateDocument();
            return report0;
        }
        public static R411 OpenReportR411(PrintData print_row, string isExport) {
            var doc = JsonConvert.DeserializeObject<I_POSSaleSetX>(print_row.JsonData);
            List<int> print_list = new List<int> { 0, 1 };

            R411 report0 = new R411(doc);
            if (isExport == "1") {
                MemoryStream stream = new MemoryStream();
                report0.ExportToPdf(stream);
                ExportPdf(stream, print_row.PrintID);
            }
            report0.CreateDocument();
            return report0;
        }
        public static R412 OpenReportR412(PrintData print_row, string isExport) {
            var doc = JsonConvert.DeserializeObject<I_POSSaleSetX>(print_row.JsonData);
            List<int> print_list = new List<int> { 0, 1 };

            R412 report0 = new R412(doc);
            if (isExport == "1") {
                MemoryStream stream = new MemoryStream();
                report0.ExportToPdf(stream);
                ExportPdf(stream, print_row.PrintID);
            }
            report0.CreateDocument();
            return report0;
        }
        public static R421 OpenReportR421(PrintData print_row, string isExport) {
            var doc = JsonConvert.DeserializeObject<I_POSSaleSetX>(print_row.JsonData);
            List<int> print_list = new List<int> { 0, 1 };

            R421 report0 = new R421(doc);
            if (isExport == "1") {
                MemoryStream stream = new MemoryStream();
                report0.ExportToPdf(stream);
                ExportPdf(stream, print_row.PrintID);
            }
            report0.CreateDocument();
            return report0;
        }
        public static void ExportPdf(MemoryStream stream,string printId) { 
            //var report = new R412();
            //report.initData(h.BillID);
            //report.ExportToPdf(stream); 
            if (string.IsNullOrEmpty(printId)) {
                return;
            }
            string myfilename = printId + ".pdf";
            string myfillfilename = BasePath + myfilename;
            string serverpath = HttpContext.Current.Server.MapPath (myfillfilename);
            FileStream myfile = new FileStream(serverpath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
            stream.WriteTo(myfile);
            myfile.Close();
            stream.Close();



        }
    }
}