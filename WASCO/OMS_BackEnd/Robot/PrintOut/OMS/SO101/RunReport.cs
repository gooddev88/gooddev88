 
using Robot.Data.DA.Master;
using Robot.Data.GADB.TT;
using Robot.Service.FileGo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using static Robot.Data.DA.HR.InventoryService;
using static Robot.Data.DA.Order.SO.SOService;
using static Robot.Data.ML.I_Result;

namespace Robot.PrintOut.OMS.SO101 {
    public class RunReport {


      async public static Task<I_BasicResult> Convert2PrintData(I_SODocSet i, string printid, string printby) {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                SO101Set d =new SO101Set();
                d.Head = i.Head;
                d.Line= i.Line;
                var comInfo = CompanyService.GetCompanyInfo(i.Head.RComID, i.Head.ComID);
                var custInfo = CustomerService.GetCustInfo(i.Head.CustID, i.Head.RComID, i.Head.ComID);

                //d.ComID = i.Head.ComID;
                d.ComName = comInfo.Name1 + Environment.NewLine + comInfo.Name2;
                d.ComAddress=comInfo.PrintHeader1 +" "+ comInfo.PrintHeader2;
                
                var img_com_url = FileGo.GetFileUrl(i.Head.RComID, i.Head.ComID, FileGo.Type_CompanyProfile, i.Head.ComID);
                d.ComImage64 = img_com_url == null ? "" : img_com_url;
                d.ComTax = comInfo.TaxID;
                d.ComBrn = comInfo.BrnCode;
                d.CustAddress = custInfo.BillAddr1 + custInfo.BillAddr2;
                d.CustTax = custInfo.TaxID;

                //using (GAEntities db = new GAEntities()) {
                //    var c = db.vw_OSOHead.FirstOrDefault(o => o.CustID == custInfo.CustomerID && o.RComID == custInfo.RCompanyID && o.ComID == custInfo.CompanyID);
                //    if (c != null) {
                //        c.CustTaxID = custInfo.TaxID;
                //        c.CustAddr1 = custInfo.BillAddr1;
                //        c.CustAddr2 = custInfo.BillAddr2;

                //        db.SaveChanges();
                //    }
                //}


                PrintData n = new PrintData();
                n.AppID = "OMS";
                n.FormPrintID = "SO101";
                JsonSerializerOptions jso = new JsonSerializerOptions();
                jso.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
                string json = JsonSerializer.Serialize(d, jso);
                n.JsonData = json;
                //r = PrintService.CreatePrintData(n);
                r = await Task.Run(() => PrintService.CreatePrintDataApi(n));

            } catch (Exception ex) {
                r.Result = "fail";
                r.Message1 = ex.Message;
            }

            return r;
        }


        public class SO101Set {
            public vw_OSOHead Head { get; set; }
            public List<vw_OSOLine> Line { get; set; }
            public vw_OSOLine LineActive { get; set; }
            public List<vw_OSOLot> Lot { get; set; }
            public vw_OSOLot LotActive { get; set; }

            //public string ComID { get; set; }
            public string ComName { get; set; }
            public string ComAddress { get; set; }
            public string ComImage64 { get; set; }
            public string ComTax { get; set; }
            public string ComBrn { get; set; }
            public string CustAddress { get; set; }
            public string CustTax { get; set; }


        }
      
    }
}
