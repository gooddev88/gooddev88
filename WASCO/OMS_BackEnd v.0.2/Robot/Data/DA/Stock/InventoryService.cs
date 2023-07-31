using Blazored.SessionStorage;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Robot.Data.DA.Document;
using Robot.Data.DA.Login;
using Robot.Data.DA.Master;
using Robot.Data.GADB.TT;
using Robot.Data.ML;
using Robot.Helper.Datetime;
using RobotWasm.Shared.Data.GaDB;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

using static Robot.Data.DA.HR.InventoryService;
using static Robot.Data.DA.Login.LogInService;
using static Robot.Data.ML.I_Result;

namespace Robot.Data.DA.HR {
    public class InventoryService {

        public static string sessionActiveId = "activecomid";


        //public class I_SP_CreateDriverPayRollExport_CarrierSet {
        //    public SP_CreateDriverPayRollExport_Carrier sp_CreateDriverPayRollExport_Carrier { get; set; }
        //    public List<TransactionLog> Log { get; set; }
        //    public I_BasicResult OutputAction { get; set; }
        //    public bool NeedRunNextID { get; set; }
        //}


        
        public class I_ItemBalSetFiterSet {

            public String RCom { get; set; }
            public String Com { get; set; }
            public String Type { get; set; }
            public String Brand { get; set; }
            public String Month { get; set; }
            public String LocID { get; set; }
            public String Year { get; set; }
            public String CostCenter { get; set; }
            public String SearchText { get; set; }
            public bool ShowNotZero { get; set; }



        }

        public class I_ItemMoveFiterSet {
            public String RCom { get; set; }
            public String Com { get; set; }
            public String Type { get; set; }
            public String LocID { get; set; }
            public String CostCenter { get; set; }
            public DateTime DateFrom { get; set; }
            public DateTime DateTo { get; set; }
            public String User { get; set; }
            public String SearchText { get; set; }




        }

        public class Loc {
            public string Value { get; set; }
            public string Description { get; set; }
        }
        public class Brand {
            public string Value { get; set; }
            public string Description { get; set; }
        }

        public class Month {
            public string Value { get; set; }
            public string Description { get; set; }
        }

        public class CostCenter {
            public string Value { get; set; }
            public string Description { get; set; }
        }

        public class PayNo {
            public string Value { get; set; }
            public string Description { get; set; }
        }
        public class ThaiMonthList {
            public string No { get; set; }
            public string Name { get; set; }
        }

        ISessionStorageService sessionStorage;
       
        public InventoryService(ISessionStorageService _sessionStorage) {
            sessionStorage = _sessionStorage;
        }



       
        public static List<Brand> ListBrand() {
            List<Brand> n = new List<Brand>();
            n.Add(new Brand { Value = "", Description = "ALL" });
            n.Add(new Brand { Value = "ALT", Description = "ALT" });
            n.Add(new Brand { Value = "CAT", Description = "CAT" });
            n.Add(new Brand { Value = "CTP", Description = "CTP" });
            n.Add(new Brand { Value = "EYK", Description = "EYK" });
            n.Add(new Brand { Value = "PPC", Description = "PPC" });
            n.Add(new Brand { Value = "RBK", Description = "RBK" });
            n.Add(new Brand { Value = "USM", Description = "USM" });


            return n;
        }
 

        public static List<Loc> ListLoc() {
            List<Loc> n = new List<Loc>();
            n.Add(new Loc { Value = "", Description = "ALL" });
            n.Add(new Loc { Value = "BUY", Description = "BUY" });
            n.Add(new Loc { Value = "STORE", Description = "STORE" });
            n.Add(new Loc { Value = "SALE", Description = "SALE" });


            return n;
        }



        //public static IEnumerable<UserInfo> ListUserInfo(LoginSet login) {
        //    IEnumerable<UserInfo> result = new List<UserInfo>();
        //    var uic = login.UserInCompany;
        //    var rcom = login.CurrentRootCompany.CompanyID;
        //    var com = login.CurrentCompany.CompanyID;
        //    using (GAEntities db = new GAEntities()) {
        //        result = db.UserInfo.Where(o => o.RCompanyID == rcom
        //        && o.CompanyID == com
        //        && uic.Contains(o.CompanyID)
        //        ).AsNoTrackingWithIdentityResolution().ToArray();
        //    }
        //    return result;
        //}
        public static async Task<List<vw_STKMove>> ItemMoveList(string rcom,string com, string itemID,string locId, LoginSet login) {
            var result = new List<vw_STKMove>();
            await Task.Run(() => {
                using (var db = new GAEntities()) {
                    result = db.vw_STKMove.Where(o => o.ItemID == itemID && o.LocID== locId && o.IsActive==true).ToList();
                }
            });
            return result;
        }
        public static   vw_STKBal GetBalance(string itemId, string rcomId,string comId ,string locId) {
            var output = new vw_STKBal(); 
                using (var db = new GAEntities()) {
                output = db.vw_STKBal.Where(o => o.ItemID == itemId && o.RComID==rcomId && o.ComID==comId && o.LocID==locId).FirstOrDefault();
               
                } 
            return output;
        }


        public static List<vw_STKMove> GetItemMove(I_ItemMoveFiterSet f, LoginSet login) {
            List<vw_STKMove> result = new List<vw_STKMove>();
            var uic = login.UserInCompany;
            var loc_access = login.UserInLoc.Select(o => o.LocID).ToList();
            using (GAEntities db = new GAEntities()) {
                if (f.SearchText != "") {
                    result = db.vw_STKMove.Where(o => (
                                                                o.ItemID.Contains(f.SearchText)
                                                                || o.ItemName.Contains(f.SearchText) 
                                                             )

                                                             && uic.Contains(f.Com)
                                                             && (o.LocID == f.LocID || f.LocID == "")
                                                             && (o.BrandID == f.Type || f.Type == "")
                                                             && o.RComID == f.RCom
                                                            && o.IsActive == true
                                                             )
                                                            .OrderByDescending(o => o.StkDate).ThenBy(o=>o.ItemID).ToList();
                } else {
                    result = db.vw_STKMove.Where(o => (o.DocDate >= f.DateFrom && o.DocDate <= f.DateTo)

                                                            && o.RComID == f.RCom
                                                            && uic.Contains(f.Com)
                                                            && (o.LocID == f.LocID || f.LocID == "")
                                                            && (o.BrandID == f.Type || f.Type == "")

                                              && o.IsActive == true
                                            ).OrderByDescending(o => o.StkDate).ThenBy(o=>o.ItemID).ToList();
                }

            }
            result = result.Where(o => loc_access.Contains(o.LocID)).ToList();
            return result;
        }
        public static List<vw_STKBal> ListDoc(I_ItemBalSetFiterSet f, LoginSet login) {
            List<vw_STKBal> result = new List<vw_STKBal>();
            var loc_access = login.UserInLoc.Select(o => o.LocID).ToList();
            var uic = login.UserInCompany;
            using (GAEntities db = new GAEntities()) {
                if (f.SearchText != "") {
                    result = db.vw_STKBal.Where(o => (
                                                                o.ItemID.Contains(f.SearchText)
                                                                || o.ItemName.Contains(f.SearchText)
                                                       
                                                             ) 
                                                             && uic.Contains(f.Com)
                                                             && (o.LocID == f.LocID || f.LocID == "")
                                                              && (o.BrandID == f.Brand || f.Brand == "")
                                                             && o.RComID == f.RCom
                                                            && o.IsActive == true
                                                            && (o.BalQty>0 || !f.ShowNotZero)
                                                             )
                                                            .OrderBy(o => o.ItemID).ToList();
                } else {
                    result = db.vw_STKBal.Where(o =>  o.RComID == f.RCom
                                                            && uic.Contains(f.Com)
                                                            && (o.LocID == f.LocID || f.LocID == "")
                                                            && (o.BrandID == f.Brand || f.Brand == "")
                                                             && (o.BalQty > 0 || !f.ShowNotZero)
                                              && o.IsActive == true
                                            ).OrderBy(o => o.ItemID).ToList();
                }

            }
            result = result.Where(o => loc_access.Contains(o.LocID)).ToList();
            return result;
        }
        public static I_ItemBalSetFiterSet NewItemBalFilterSet() {
            I_ItemBalSetFiterSet n = new I_ItemBalSetFiterSet();
            n.RCom = "";
            n.Com = "";
            n.Brand = "";
            n.LocID = "";
            n.Type = "";
            n.CostCenter = "";
            n.Year = DateTime.Now.Date.Year.ToString();
            n.Month = DateTime.Now.Date.Month.ToString();
            n.SearchText = "";
            n.ShowNotZero = true;


            return n;
        }

        public static I_ItemMoveFiterSet NewItemMoveFilterSet() {
            I_ItemMoveFiterSet n = new I_ItemMoveFiterSet();

            n.User = "";
            n.LocID = "";
            n.Type = "";
            n.DateFrom = DateTime.Now.Date.AddMonths(-7);
            n.DateTo = DateTime.Now.Date.AddMonths(1);
            n.RCom = "";
            n.Com = "";
            n.SearchText = "";


            return n;
        }

        public static List<LocationInfo> ListLocID(string com) {
            List<LocationInfo> result = new List<LocationInfo>();
            using (GAEntities db = new GAEntities()) {
                result = db.LocationInfo.Where(o =>  (o.CompanyID == com || o.CompanyID == "" )
                 && o.IsActive == true).ToList();

            }
            return result;
        }
        public static List<MasterTypeLine> ListService_Type(string rcom, string cateId) {
            List<MasterTypeLine> result = new List<MasterTypeLine>();
            using (GAEntities db = new GAEntities()) {
                result = db.MasterTypeLine.Where(o => o.MasterTypeID == cateId
                                                     && (o.RComID == rcom || o.RComID == "")
                                                        && o.IsActive == true).ToList();

            }
            return result;
        }


        async public Task<I_ItemBalSetFiterSet> GetSessionItemBalFiterSet() {
            I_ItemBalSetFiterSet result = NewItemBalFilterSet();
            var strdoc = await sessionStorage.GetItemAsync<string>("ItemBal_Fiter");
            if (strdoc != null) {
                return JsonConvert.DeserializeObject<I_ItemBalSetFiterSet>(strdoc);
            } else {
                return null;
            }

        }

        async public void SetSessionItemBalFiterSet(I_ItemBalSetFiterSet data) {
            JsonSerializerOptions jso = new JsonSerializerOptions();
            jso.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
            string json = System.Text.Json.JsonSerializer.Serialize(data, jso);
            await sessionStorage.SetItemAsync("ItemBal_Fiter", json);
        }




        async public Task<I_ItemMoveFiterSet> GetSessionItemMoveFiterSet() {
            I_ItemMoveFiterSet result = NewItemMoveFilterSet();
            var strdoc = await sessionStorage.GetItemAsync<string>("ItemMove_Fiter");
            if (strdoc != null) {
                return JsonConvert.DeserializeObject<I_ItemMoveFiterSet>(strdoc);
            } else {
                return null;
            }

        }

        async public void SetSessionItemMoveFiterSet(I_ItemMoveFiterSet data) {
            JsonSerializerOptions jso = new JsonSerializerOptions();
            jso.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
            string json = System.Text.Json.JsonSerializer.Serialize(data, jso);
            await sessionStorage.SetItemAsync("ItemMove_Fiter", json);
        }


    }
}
