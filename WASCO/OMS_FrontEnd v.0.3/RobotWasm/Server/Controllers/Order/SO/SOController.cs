using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RobotWasm.Server.Data.DA.Master;
using RobotWasm.Shared.Data.GaDB; 
using System.Text.Json;
using System.Text.Json.Serialization;
using static RobotWasm.Shared.Data.ML.Shared.I_Result;
using RobotWasm.Server.Data.DA.Order.SO;
using System;
using RobotWasm.Shared.Data.ML.Param;
using RobotWasm.Shared.Data.ML.Shared;
using static RobotWasm.Shared.Data.DA.SOFuncService;
using RobotWasm.Shared.Data.ML.Order;
using RobotWasm.Shared.Data.ML.Promotion;

namespace RobotWasm.Server.Controllers.Order.SO {
    [Route("api/[controller]")]
    [ApiController]
    public class SOController : ControllerBase {


        //[HttpGet("GetDocSet")]
        //public I_SODocSet GetDocSet(string docid, string rcom, string com) {
        //    return SOService.GetDocSet(docid, rcom, com);
        //}

        [HttpPost("GetDocSet")]
        public I_SODocSet GetDocSet([FromBody] string data ) {
            var doc = JsonSerializer.Deserialize<SODocParam>(data, new JsonSerializerOptions {
                PropertyNameCaseInsensitive = true,
                ReferenceHandler = ReferenceHandler.Preserve
            });
          return SOService.GetDocSet(doc.OrdID,doc.RCom,doc.Com); 
        }


        [HttpPost("SaveSO")]
        public I_BasicResult SaveSO([FromBody] string data, [FromQuery] string action) {
            var doc = JsonSerializer.Deserialize<I_SODocSet>(data, new JsonSerializerOptions {
                PropertyNameCaseInsensitive = true,
                ReferenceHandler = ReferenceHandler.Preserve
            });
            I_BasicResult r = SOService.Save(doc, action);
            return r;
        }
   
        [HttpGet("ListNotificationStatus")]
        public int ListNotificationStatus(string user) {
            return SOService.ListNotificationStatus(user);
        }

        [HttpPost("ListDoc")]
        public List<vw_OSOHead> ListDocument([FromBody] I_SODocFiterSet f) {
            var r = SOService.ListDoc(f);
            return r;
        }

        [HttpPost("ListSaleItem")]
        public I_PromotionSet ListSaleItem([FromBody] string data) {
       
            var doc = JsonSerializer.Deserialize<ItemParam>(data, new JsonSerializerOptions {
                PropertyNameCaseInsensitive = true,
                ReferenceHandler = ReferenceHandler.Preserve
            });

            return SOService.ListSaleItem(doc.RComID,doc.ComID,doc.BrandID,doc.CateID,doc.LocID,doc.ProID);
        }

        [HttpGet("DeleteDoc")]
        public I_BasicResult DeleteDoc(string docid, string rcom, string com, string modified_by) {
            return SOService.DeleteDoc(docid, rcom, com, modified_by);
        }
       [HttpGet("LockCorder")]
        public I_BasicResult LockOrder(int action) {
            //action =1=lock,0=unlock,2=getstatus
            return SOService.LockOrder(action);
        }
        [HttpPost("GetGeoThailand")]
        public GeoThaiLand GetGeoThailand([FromBody] string data )
        {
            GeoThaiLand output = new GeoThaiLand();
            var doc = JsonSerializer.Deserialize<GeoParam>(data, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                ReferenceHandler = ReferenceHandler.Preserve
            });
            output = SOService.GetGeoThailand(doc.Latitude, doc.Longitude);
            return output;
        }

        #region Promotion
        [HttpPost("ListPromotion")]
        public List<Promotions> Promotion([FromBody] string data) {
            List<Promotions> output = new List<Promotions>();
            var doc = JsonSerializer.Deserialize<PromotionParam>(data, new JsonSerializerOptions {
                PropertyNameCaseInsensitive = true,
                ReferenceHandler = ReferenceHandler.Preserve
            });
            output = SOService.ListPromotion(doc.RComID, doc.ComID,doc.TranDate);
            return output;
        }

        //[HttpPost("GetPromotion")]
        //public I_PromotionSet GetPromotion([FromBody] string data) {
        //    I_PromotionSet? output = new I_PromotionSet();
        //    var doc = JsonSerializer.Deserialize<PromotionParam>(data, new JsonSerializerOptions {
        //        PropertyNameCaseInsensitive = true,
        //        ReferenceHandler = ReferenceHandler.Preserve
        //    });
        //    output = SOService.GetPromotion(doc.RComID, doc.ComID, doc.PromotionID);
        //    return output;
        //}
        #endregion

 
    }
}
