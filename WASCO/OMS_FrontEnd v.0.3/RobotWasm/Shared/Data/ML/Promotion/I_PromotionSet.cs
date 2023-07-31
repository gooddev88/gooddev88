using RobotWasm.Shared.Data.GaDB;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;
using static RobotWasm.Shared.Data.ML.Shared.I_Result;
using static RobotWasm.Shared.Data.DA.SOFuncService;
using RobotWasm.Shared.Data.ML.Order;

namespace RobotWasm.Shared.Data.ML.Promotion
{
   
    public class I_PromotionSet {
        public Promotions Promotion { get; set; }
        //public List<PromotionItem> PromotionItems { get; set; }
        public List<ItemDesplay> PromotionItems { get; set; }
    }


   

}
