using Robot.Data.DA;
using Robot.Service.FileGo;
using System;
using System.Text.Json;
using System.Threading.Tasks;
using static Robot.Data.DA.POSSY.POSService;
using static Robot.Data.ML.I_Result;
using static Robot.PrintOut.CreatePrintData.SalePrintConverter;

namespace Robot.PrintOut.CreatePrintData.R40X {
    public class RunReport {
        async public static Task<I_BasicResult> Convert2PrintData(I_POSSaleSet i, string printform_id) {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {

                var d = SalePrintConverter.Convert2Print(i);     
                var comInfo = CompanyService.GetCompanyInfo(i.Head.RComID, i.Head.ComID);
                //var custInfo = CustomerService.GetCustInfo(i.Head.CustID, i.Head.RComID, i.Head.ComID);
                d.PriceTaxCon = comInfo.PriceTaxCondType;
                d.ComName = comInfo.Name1 + Environment.NewLine + comInfo.Name2;
                d.ComAddress = comInfo.BillAddr1 + " " + comInfo.BillAddr2; 
                var img_com_url = FileGo.GetFileUrl(i.Head.RComID, i.Head.ComID, FileGo.Type_CompanyProfile, i.Head.ComID);
                d.ComImage64 = img_com_url == null ? "" : img_com_url;
                d.ComTax = comInfo.TaxID;
                d.ComBrn = comInfo.BrnCode; 
                d.ComAddress = comInfo.BillAddr1 + " " + comInfo.BillAddr2; 
                d.QrPaymentData = comInfo.QrPaymentData;
                PrintData n = new PrintData();
                n.AppID = Globals.AppID;
                n.FormPrintID = printform_id;
                JsonSerializerOptions jso = new JsonSerializerOptions();
                jso.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
                string json = JsonSerializer.Serialize(d, jso);
                n.JsonData = json;
                // r = PrintService.CreatePrintData(n);
                r = await Task.Run(() => PrintService.CreatePrintDataApi(n));

            } catch (Exception ex) {
                r.Result = "fail";
                r.Message1 = ex.Message;
            }

            return r;
        }
    }
}
