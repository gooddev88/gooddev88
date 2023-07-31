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
using RobotWasm.Server.Data.DA.Promotion;
using RobotWasm.Shared.Data.ML.Promotion;

namespace RobotWasm.Server.Controllers.Promotion
{
    [Route("api/[controller]")]
    [ApiController]
    public class PromotionController : ControllerBase {


 

        #region Promotion
        [HttpPost("ListPromotion")]
        public List<Promotions> Promotion([FromBody] string data) {
            List<Promotions> output = new List<Promotions>();
            var doc = JsonSerializer.Deserialize<PromotionParam>(data, new JsonSerializerOptions {
                PropertyNameCaseInsensitive = true,
                ReferenceHandler = ReferenceHandler.Preserve
            });
            output = PromotionService.ListPromotion(doc.RComID, doc.ComID, doc.TranDate);
            return output;
        }

        [HttpPost("GetPromotion")]
        public Promotions GetPromotion([FromBody] string data) {
            Promotions? output = new Promotions();
            var doc = JsonSerializer.Deserialize<PromotionParam>(data, new JsonSerializerOptions {
                PropertyNameCaseInsensitive = true,
                ReferenceHandler = ReferenceHandler.Preserve
            });
            //output = SOService.GetPromotion(doc.RComID, doc.ComID, doc.PromotionID);
            return output;
        }

        [HttpPost("GetPromotionSet")]
        public I_PromotionSet GetPromotionSet([FromBody] string data) {

            var doc = JsonSerializer.Deserialize<ItemParam>(data, new JsonSerializerOptions {
                PropertyNameCaseInsensitive = true,
                ReferenceHandler = ReferenceHandler.Preserve
            });

            return PromotionService.GetPromotionSet(doc.RComID, doc.ComID, doc.BrandID, doc.CateID, doc.LocID, doc.ProID);
        }
        #endregion


    }
}
