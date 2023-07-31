using RobotWasm.Shared.Data.ML.Order;
using RobotWasm.Shared.Data.ML.Print;
using static RobotWasm.Shared.Data.ML.Shared.I_Result;
using System.Text.Json;
using RobotWasm.Shared.Data.GaDB;

namespace RobotWasm.Server.PrintOut.OMS.STK112 {
    public class RunReport {
        async public static Task<I_BasicResult> Convert2PrintData(List<vw_ItemInfoWithPhotoAndStock> rows,string brand) {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                STK112Set d = new STK112Set();
                d.rows = rows;
                d.Brand = brand;
              
                PrintData n = new PrintData();
                n.AppID = "OMS";
                n.FormPrintID = "STK112";
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


        public class STK112Set {
            public string Brand { get; set; }
            public List<vw_ItemInfoWithPhotoAndStock> rows { get; set; }
      
        }
    }
}
