using System.Data;
using static RobotWasm.Shared.Data.ML.Shared.I_Result;
using RobotWasm.Shared.Data.GaDB;
using RobotWasm.Server.Data.GaDB;
using RobotWasm.Shared.Data.ML.Print;
using static RobotWasm.Shared.Data.DA.POSFuncService;
using static RobotWasm.Shared.Data.ML.Print.SalePrintConverter;
using System.Text.Json;
using RobotWasm.Server.Data.DA.Master;
using RobotWasm.Server.Service.FileGo;

namespace RobotWasm.Server.Data.DA.Print {
    public class PrintService {

        async public static Task<I_BasicResult> Convert2PrintData(I_POSSaleSet i, string printform_id) {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {

                var d = SalePrintConverter.Convert2Print(i);
                var comInfo = CompanyService.GetComInfoByComID(i.Head.RComID, i.Head.ComID);
                d.PriceTaxCon = comInfo.PriceTaxCondType;
                d.ComName = comInfo.Name1 + Environment.NewLine + comInfo.Name2;
                d.ComAddress = comInfo.BillAddr1 + " " + comInfo.BillAddr2;
                var img_com_url = "";//FileGoService.GetFileUrl(i.Head.RComID,i.Head.ComID,"COMPANY_PHOTO_PROFILE", i.Head.ComID);
                d.ComImage64 = img_com_url == null ? "" : img_com_url;
                d.ComTax = comInfo.TaxID;
                d.ComBrn = comInfo.BrnCode;
                d.ComAddress = comInfo.BillAddr1 + " " + comInfo.BillAddr2;
                d.QrPaymentData = comInfo.QrPaymentData;
                PrintData n = new PrintData();
                n.AppID = "KYPOS";
                n.FormPrintID = printform_id;
                JsonSerializerOptions jso = new JsonSerializerOptions();
                jso.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
                string json = JsonSerializer.Serialize(d, jso);
                n.JsonData = json;
                r = await Task.Run(() => CreatePrintDataApi(n));

            } catch (Exception ex) {
                r.Result = "fail";
                r.Message1 = ex.Message;
            }

            return r;
        }

        async public static Task<I_BasicResult> CreatePrintDataApi(PrintData data) {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                string dataEndpointUri = $"{Globals.ApiPrintMasterBaseUrl}/api/Print";
                var xdata = JsonSerializer.Serialize(data);
                HttpClient httpClient = new HttpClient();
                var response = await httpClient.PostAsJsonAsync(dataEndpointUri, data);
                var jsonStr = response.Content.ReadAsStringAsync().Result;

                if (response.StatusCode.ToString() != "OK") {
                    r.Result = "fail";
                    r.Message1 = "Web service Error!";
                } else {
                    r = JsonSerializer.Deserialize<I_BasicResult>(jsonStr);
                }

            } catch (Exception e) {
                r.Result = "fail";
                if (e.InnerException != null) {
                    r.Message1 = e.InnerException.Message;
                } else {
                    r.Message1 = e.Message;
                }
            }
            return r;
        }

    }
}
