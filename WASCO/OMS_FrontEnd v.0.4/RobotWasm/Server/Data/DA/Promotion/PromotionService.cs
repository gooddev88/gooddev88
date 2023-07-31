using RobotWasm.Server.Data.GaDB;
using static RobotWasm.Shared.Data.ML.Shared.I_Result;
using RobotWasm.Shared.Data.GaDB;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using RobotWasm.Shared.Data.ML.Shared; 
using RobotWasm.Server.Data.DA.Line;
using RobotWasm.Shared.Data.ML.Promotion;
using Microsoft.AspNetCore.Mvc;
using RobotWasm.Shared.Data.ML.Param;
using System.Text.Json.Serialization;
using System.Text.Json;
using RobotWasm.Shared.Data.ML.Order;

namespace RobotWasm.Server.Data.DA.Promotion
{
    public class PromotionService {

        #region promotion
        public static List<Promotions> ListPromotion(string rcom, string com, DateTime tranDate) {
            List<Promotions> result = new List<Promotions>();
            try {
                using (GAEntities db = new GAEntities()) {
                    result = db.Promotions.Where(o => o.RCompanyID == rcom
                                                            && o.CompanyID == com
                                                            && o.IsActive == true
                                                            && (tranDate >= o.DateBegin && tranDate <= o.DateEnd)
                                                            ).OrderBy(o => o.ProID).ToList();
                }
            } catch (Exception ex) {
            }
            return result;
        }


        public static Promotions? GetPromotionInfo(string rcom, string com, string proId) {
            Promotions? result = new Promotions();
            try {
                using (GAEntities db = new GAEntities()) {
                    result = db.Promotions.Where(o => o.RCompanyID == rcom
                                                            && o.CompanyID == com
                                                            && o.ProID == proId 
                                                            ).FirstOrDefault(); 
                }
            } catch (Exception ex) {
            }
            return result;
        }


        public static I_PromotionSet GetPromotionSet(string rcom, string com, string brandid, string cateid, string locid, string proid) {
            brandid = brandid == null ? "" : brandid;
            cateid = cateid == null ? "" : cateid;
            I_PromotionSet result = new I_PromotionSet();
            using (GAEntities db = new GAEntities()) {

                result.Promotion = db.Promotions.Where(o => o.RCompanyID == rcom
                                                         && o.CompanyID == com
                                                         && o.ProID == proid
                                                         && o.IsActive == true
                                                         ).FirstOrDefault();
                //  var group_Stkbal = Stkbal.GroupBy(x => x.ItemID).Select(g => g.First());

                if (result.Promotion.PatternID == "P000") {
                    var Stkbal = db.vw_ItemInfoWithPhotoAndStock.Where(o =>
                                                      (o.BrandID == brandid || brandid == "")
                                                      && (o.CateID == cateid || cateid == "")
                                                      && o.LocID == locid
                                                      ).ToList();
                    result.PromotionItems = new List<ItemDesplay>();
                    foreach (var l in Stkbal) {
                        ItemDesplay n = new ItemDesplay();
                        n.ItemID = l.ItemID;
                        n.ItemName = l.Name1;
                        n.LocID = l.LocID;
                        n.TypeID = l.TypeID;
                        n.CateID = l.CateID;
                        n.Price = l.Price;
                        n.PriceIncVat = l.PriceIncVat;
                        n.PriceProIncVat = l.PriceProIncVat;
                        n.IsSpecialPrice = l.IsSpecialPrice;
                        n.BrandID = l.BrandID;
                        n.Qty = 1;
                        if (string.IsNullOrEmpty(l.PhotoUrl)) {
                            n.ImageUrl = @"/img/no_image.png";
                        } else {
                            n.ImageUrl = l.PhotoUrl;
                        }
                        n.BalQty = l.BalQty;
                        n.DiscAmt = 0;
                        n.UnitID = l.UnitID;
                        n.ProID = "P000";
                        n.ProName = "ราคาปกติ";
                        n.PatternID = "PRO000";
                        result.PromotionItems.Add(n);
                    }
                }
                if (result.Promotion.PatternID == "P100") {
                    result.PromotionItems = new List<ItemDesplay>();
                    var items = db.vw_PromotionItemWithPattern.Where(o =>
                                                  (o.BrandID == brandid || brandid == "")
                                                  && (o.CateID == cateid || cateid == "")
                                                  && o.LocID == locid
                                                  && o.ProID == proid
                                                  ).ToList();
                    foreach (var l in items) {
                        ItemDesplay n = new ItemDesplay();
                        n.ItemID = l.ItemID;
                        n.ItemName = l.Name1;
                        n.LocID = l.LocID;
                        n.TypeID = l.TypeID;
                        n.CateID = l.CateID;
                        n.Price = l.Price;
                        n.PriceIncVat = 0;
                        n.PriceProIncVat = result.Promotion.YValue;
                        n.IsSpecialPrice = true;
                        n.BrandID = l.BrandID;
                        n.Qty = 1;
                        if (string.IsNullOrEmpty(l.PhotoUrl)) {
                            n.ImageUrl = @"/img/no_image.png";
                        } else {
                            n.ImageUrl = l.PhotoUrl;
                        }
                        n.BalQty = l.BalQty;
                        n.DiscAmt = 0;
                        n.UnitID = l.UnitID;
                        n.ProID = result.Promotion.ProID;
                        n.ProName = result.Promotion.ProDesc;
                        n.PatternID = result.Promotion.PatternID;
                        result.PromotionItems.Add(n);
                    }
                }

            }
            return result;
        }
        #endregion




    }
}
