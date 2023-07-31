using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RobotWasm.Server.Controllers.Login;
using RobotWasm.Server.Data;
using RobotWasm.Server.Data.DA.Promotion;
using RobotWasm.Server.Data.DA.Stock;
using RobotWasm.Server.PrintOut.OMS.STK112;
using RobotWasm.Shared.Data.GaDB;
using RobotWasm.Shared.Data.ML.Login;
using RobotWasm.Shared.Data.ML.Master; 
using RobotWasm.Shared.Data.ML.Param;
using RobotWasm.Shared.Data.ML.Shared;
using System;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text.Json.Serialization;
using Telerik.SvgIcons;
using static RobotWasm.Server.Data.DA.Stock.StockBalanceService;
using static RobotWasm.Shared.Data.ML.Shared.I_Result;

namespace RobotWasm.Server.Controllers.Stock {
    [Route("api/[controller]")]
    [ApiController]
    public class StockBalanceController : ControllerBase {

        [HttpPost("ListLot")]
        public List<vw_STKBal> ListLot([FromBody]  string data) {

            var p = JsonSerializer.Deserialize<ItemParam>(data, new JsonSerializerOptions {
                PropertyNameCaseInsensitive = true,
                ReferenceHandler = ReferenceHandler.Preserve
            });
            return StockBalanceService.ListLot(p.RComID, p.ComID, p.ItemID, p.LocID); 
        }
        [HttpPost("ListStockBalSale")]
        public List<vw_ItemInfoWithPhotoAndStock> ListStockBalSale([FromBody] string data) { 
            var p = JsonSerializer.Deserialize<StockBalParam>(data, new JsonSerializerOptions {
                PropertyNameCaseInsensitive = true,
                ReferenceHandler = ReferenceHandler.Preserve
            });
            return StockBalanceService.ListStockBalWithPhoto(p);
        }
        [HttpPost("PrintStockBalSale")]
    async    public Task<I_BasicResult> PrintStockBalSale([FromBody] string data) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            var p = JsonSerializer.Deserialize<StockBalParam>(data, new JsonSerializerOptions {
                PropertyNameCaseInsensitive = true,
                ReferenceHandler = ReferenceHandler.Preserve
            });
            var print_data = StockBalanceService.ListStockBalWithPhoto(p);
            result = await Task.Run(() => RobotWasm.Server.PrintOut.OMS.STK112.RunReport.Convert2PrintData(print_data, p.Brand));
            
            if (result.Result=="ok") {
                result.Message2 = Globals.ApiPrintMasterBaseUrl + $"/PrintFile/OMS/STK112/ViewerSK112.aspx?id={result.Message2}&export=0"; 
            }
            return result;
        }
        [HttpPost("PrintStockPromotion")]
     async   public Task<I_BasicResult> PrintStockPromotion([FromBody] string data) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            var p = JsonSerializer.Deserialize<StockBalParam>(data, new JsonSerializerOptions {
                PropertyNameCaseInsensitive = true,
                ReferenceHandler = ReferenceHandler.Preserve
            });
            var print_data= StockBalanceService.ListStockPromotionWithPhoto(p);
            var proInfo = PromotionService.GetPromotionInfo(p.RCom, p.Com, p.PromotionID);
            result = await Task.Run(() => RobotWasm.Server.PrintOut.OMS.STK113.RunReport.Convert2PrintData(print_data, proInfo.ProDesc));
            if (result.Result == "ok") {
                result.Message2 = Globals.ApiPrintMasterBaseUrl + $"/PrintFile/OMS/STK113/ViewerSK113.aspx?id={result.Message2}&export=0";
            }
            return result;
        }

        [HttpPost("ListLocID")]
        public List<LocationInfo> ListLocID([FromBody] string data) {
            var p = JsonSerializer.Deserialize<LocParamUser>(data, new JsonSerializerOptions {
                PropertyNameCaseInsensitive = true,
                ReferenceHandler = ReferenceHandler.Preserve
            });
            return StockBalanceService.ListLocID(p.RCom,p.Com,p.LocIds);
        }


     

        
    }
}
