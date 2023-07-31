using Newtonsoft.Json;
using PrintMaster.Data.PrintDB;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;


namespace PrintMaster.PrintFile.OMS.STK113
{
    public class RunReport {
        public const string BasePath = @"~/TempFile/Print/";
        public static STK113 OpenReport(PrintData print_row, string isExport="0") { 
            var doc = JsonConvert.DeserializeObject<STK113Set>(print_row.JsonData);
            STK113 report0 = new STK113(doc);
            report0.DisplayName ="Stock"+ DateTime.Now.Date.ToString("yyyyMMdd");
            report0.CreateDocument();
       
            if (isExport == "1") {
                MemoryStream stream = new MemoryStream();
                report0.ExportToPdf(stream);
                ExportPdf(stream, print_row.PrintID);
            }
            return report0;
        }
        #region Export
        public static void ExportPdf(MemoryStream stream, string printId) { 
            if (string.IsNullOrEmpty(printId)) {
                return;
            }
            string myfilename = printId + ".pdf";
            string myfillfilename = BasePath + myfilename;
            string serverpath = HttpContext.Current.Server.MapPath(myfillfilename);
            FileStream myfile = new FileStream(serverpath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
            stream.WriteTo(myfile);
            myfile.Close();
            stream.Close(); 
        }
        #endregion
        #region  class

        public class STK113Set
        {
            public string ProDesc { get; set; }
            public List<vw_ItemStockPromotionWithPhoto> rows { get; set; }

        }
        public class vw_ItemStockPromotionWithPhoto
        {
            public int ID { get; set; }
            public string ItemID { get; set; }
            public string ItemCode { get; set; }
            public string RCompanyID { get; set; }
            public string CompanyID { get; set; }
            public string Name1 { get; set; }
            public string ProID { get; set; }
            public string ProDesc { get; set; }
            public decimal? XValue { get; set; }
            public decimal? YValue { get; set; }
            public DateTime? DateBegin { get; set; }
            public DateTime? DateEnd { get; set; }
            public string BrandID { get; set; }
            public string BrandName { get; set; }
            public string TypeID { get; set; }
            public string TypeName { get; set; }
            public string CateID { get; set; }
            public string CateName { get; set; }
            public decimal? Price { get; set; }
            public decimal? PriceIncVat { get; set; }
            public decimal? PriceProIncVat { get; set; }
            public decimal? CustomerPrice { get; set; }
            public bool? IsSpecialPrice { get; set; }
            public decimal? Cost { get; set; }
            public string UnitID { get; set; }
            public string StkUnitID { get; set; }
            public string PhotoID { get; set; }
            public string PhotoUrl { get; set; }
            public decimal BalQty { get; set; }
            public string LocID { get; set; }
            public string LocName { get; set; }
            public bool? IsHold { get; set; }
            public string Status { get; set; }
            public string CreatedBy { get; set; }
            public DateTime? CreatedDate { get; set; }
            public string ModifiedBy { get; set; }
            public DateTime? ModifiedDate { get; set; }
            public bool? IsActive { get; set; }
        }
        #endregion
    }
}