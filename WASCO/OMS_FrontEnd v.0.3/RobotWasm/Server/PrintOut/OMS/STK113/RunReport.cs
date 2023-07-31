using RobotWasm.Shared.Data.ML.Order;
using RobotWasm.Shared.Data.ML.Print;
using static RobotWasm.Shared.Data.ML.Shared.I_Result;
using System.Text.Json;
using RobotWasm.Shared.Data.GaDB;

namespace RobotWasm.Server.PrintOut.OMS.STK113 {
    public class RunReport {
        async public static Task<I_BasicResult> Convert2PrintData(List<vw_ItemStockPromotionWithPhoto> rows,string ProDesc) {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                STK113Set d = new STK113Set();
                d.rows = rows;
                d.ProDesc = ProDesc;
              
                PrintData n = new PrintData();
                n.AppID = "OMS";
                n.FormPrintID = "STK113";
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


        public class STK113Set {
           
            public string ProDesc { get; set; }
            public List<vw_ItemStockPromotionWithPhoto> rows { get; set; }
      
        }
    }
}
