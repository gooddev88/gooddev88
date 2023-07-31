using RobotWasm.Shared.Data.ML.Order;
using RobotWasm.Shared.Data.ML.Print;
using static RobotWasm.Shared.Data.ML.Shared.I_Result;
using System.Text.Json;
using RobotWasm.Shared.Data.GaDB;
using RobotWasm.Client.Data.DA.Master;
using static RobotWasm.Server.PrintOut.OMS.SO102.RunReport;
using RobotWasm.Server.Data.DA.Master;
using Telerik.SvgIcons;
using CompanyService = RobotWasm.Server.Data.DA.Master.CompanyService;
using CustomerService = RobotWasm.Server.Data.DA.Master.CustomerService;

namespace RobotWasm.Server.PrintOut.OMS.SO102 {
    public class RunReport {
        async public static Task<I_BasicResult> Convert2PrintData(I_SODocSet i) {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                SO102Set d = new SO102Set();
                d.Head = i.Head;
                d.Line = i.Line;
                
                var comInfo = CompanyService.GetCompanyInfo(i.Head.RComID, i.Head.ComID);
                var custInfo = CustomerService.GetCustInfo(i.Head.CustID, i.Head.RComID, i.Head.ComID);

                d.ComName = comInfo.Name1 + Environment.NewLine + comInfo.Name2;
                d.ComAddress = comInfo.PrintHeader1 + " " + comInfo.PrintHeader2;

                //var img_com_url = FileGo.GetFileUrl(i.Head.RComID, i.Head.ComID, FileGo.Type_CompanyProfile, i.Head.ComID);
                //d.ComImage64 = img_com_url == null ? "" : img_com_url;

                d.CustAddress = custInfo.BillAddr1 + custInfo.BillAddr2;
                d.CustTax = custInfo.TaxID;


                PrintData n = new PrintData();
                n.AppID = "OMS";
                n.FormPrintID = "SO102";
                JsonSerializerOptions jso = new JsonSerializerOptions();
                jso.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
                string json = JsonSerializer.Serialize(d, jso);
                n.JsonData = json; 
                r = await Task.Run(() => PrintService.CreatePrintDataApi(n));
            } catch (Exception ex) {
                r.Result = "fail";
                r.Message1 = ex.Message;
            } 
            return r;
        }


        public class SO102Set {
            public vw_OSOHead Head { get; set; }
            public List<vw_OSOLine> Line { get; set; }
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
