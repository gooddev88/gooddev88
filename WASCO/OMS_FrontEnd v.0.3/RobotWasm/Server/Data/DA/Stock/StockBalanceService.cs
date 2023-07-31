using System.Data;
using static RobotWasm.Shared.Data.ML.Shared.I_Result;
using RobotWasm.Shared.Data.GaDB;
using RobotWasm.Server.Data.GaDB;
using RobotWasm.Shared.Data.ML.Login;
using RobotWasm.Shared.Data.ML.Param;

namespace RobotWasm.Server.Data.DA.Stock {
    public class StockBalanceService {


        public StockBalanceService() {

        }

        #region Get List

        public static List<vw_STKBal> ListLot(string rcom, string comId,string itemid,string loc) {
            List<vw_STKBal> result = new List<vw_STKBal>();
            using (GAEntities db = new GAEntities()) {
                result = db.vw_STKBal.Where(o => o.RComID == rcom 
                                        && o.ComID == comId 
                                        && o.ItemID == itemid 
                                        && o.IsActive==true
                                        &&(o.LocID== loc || loc == "")
                                        ).ToList();
            }
            return result;
        }

        #endregion



        public static List<vw_ItemInfoWithPhotoAndStock> ListStockBalWithPhoto(StockBalParam f) {
            List<vw_ItemInfoWithPhotoAndStock> result = new List<vw_ItemInfoWithPhotoAndStock>();

            using (GAEntities db = new GAEntities()) {
                if (f.SearchText != "") {
                    result = db.vw_ItemInfoWithPhotoAndStock.Where(o => (
                                                                o.ItemID.Contains(f.SearchText)
                                                                || o.Name1.Contains(f.SearchText)

                                                             )
                                                             && f.UIC.Contains(f.Com)
                                                             && (o.LocID == f.LocID || f.LocID == "")
                                                              && (o.BrandID == f.Brand || f.Brand == "")
                                                             && o.RCompanyID == f.RCom
                                                            && o.IsActive == true
                                                            && (o.BalQty > 0 || !f.ShowNotZero)
                                                             )
                                                            .OrderBy(o => o.ItemID).ToList();
                } else {
                    result = db.vw_ItemInfoWithPhotoAndStock.Where(o => o.RCompanyID == f.RCom
                                                            && f.UIC.Contains(f.Com)
                                                            && (o.LocID == f.LocID || f.LocID == "")
                                                            && (o.BrandID == f.Brand || f.Brand == "")
                                                             && (o.BalQty > 0 || !f.ShowNotZero)
                                              && o.IsActive == true
                                            ).OrderBy(o => o.ItemID).ToList();
                }

            }
            result = result.Where(o => f.UILoc.Contains(o.LocID)).ToList();
            return result;
        }

        public static List<vw_ItemStockPromotionWithPhoto> ListStockPromotionWithPhoto(StockBalParam f) {
            List<vw_ItemStockPromotionWithPhoto> result = new List<vw_ItemStockPromotionWithPhoto>();
            try {
                using (GAEntities db = new GAEntities()) {
                    if (f.SearchText != "") {
                        result = db.vw_ItemStockPromotionWithPhoto.Where(o => (
                                                                    o.ItemID.Contains(f.SearchText)
                                                                    || o.Name1.Contains(f.SearchText)

                                                                 )
                                                                 && f.UIC.Contains(f.Com)
                                                                 && (o.LocID == f.LocID || f.LocID == "")

                                                                 && o.RCompanyID == f.RCom
                                                                && o.IsActive == true
                                                                 && (o.BalQty > 0 || !f.ShowNotZero)
                                                                 )
                                                                .OrderBy(o => o.ItemID).ToList();
                    } else {
                        result = db.vw_ItemStockPromotionWithPhoto.Where(o => o.RCompanyID == f.RCom
                                                                && f.UIC.Contains(f.Com)
                                                                && (o.LocID == f.LocID || f.LocID == "")
                                                                 && (o.BalQty > 0 || !f.ShowNotZero)
                                                  && o.IsActive == true
                                                ).OrderBy(o => o.ItemID).ToList();
                    }

                }
                
                result = result.Where(o => f.UILoc.Contains(o.LocID)).ToList();
            } catch (Exception ex) {
                var rr = ex.Message;
            }
      
            return result;
        }

        
        public static List<LocationInfo> ListLocID(string rcom, string com, List<string> loc_access) {
            List<LocationInfo> result = new List<LocationInfo>();
            using (GAEntities db = new GAEntities()) {
                result = db.LocationInfo
                    .Where(o => o.RCompanyID == rcom
                        && o.CompanyID == com
                        && o.IsActive == true
                        && loc_access.Contains(o.LocID))
                    .ToList();
                result.Insert(0, new LocationInfo { LocID = "", Name = "" });
            }
            return result;
        }

        

    }
}
