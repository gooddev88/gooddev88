using Dapper;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using Robot.Data.GADB.TT;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using static Robot.Data.ML.I_Result;

namespace Robot.Data.DA {
    public class POSStockService {
        public class I_StockFiterSet {
            public DateTime DateFrom { get; set; }
            public DateTime DateTo { get; set; }
            public String SearchText { get; set; }
            public String SearchItem { get; set; }
            public String Rcom { get; set; }
            public String Company { get; set; }
            public String Location { get; set; }
            public string ItemType { get; set; }
            public bool IsShowAviable { get; set; }
        }

        #region Query Transaction

        public static POS_STKBal GetBalance(string itemId, string comId,string  rcom) {
            POS_STKBal r = new POS_STKBal();
          
            using (GAEntities db = new GAEntities()) {

                r = db.POS_STKBal.Where(o => o.RComID == rcom && o.ItemID == itemId && o.ComID == comId && o.LocID == "STORE").FirstOrDefault();
                if (r == null) {
                    r = new POS_STKBal { ItemID = itemId, BalQty = 0, InstQty = 0, OrdQty = 0, RetQty = 0, ComID = comId, RComID = rcom, LocID = "STORE" };
                }
                return r;
            }
        }
        public static vw_POS_STKBalInUnitConvert GetBalanceByUnit(string itemId, string comId,string rcom, string unit) {
            vw_POS_STKBalInUnitConvert r = new vw_POS_STKBalInUnitConvert();
           
            using (GAEntities db = new GAEntities()) {

                r = db.vw_POS_STKBalInUnitConvert.Where(o => o.RCompanyID == rcom && o.ItemID == itemId && o.CompanyID == comId && o.LocID == "STORE").FirstOrDefault();
                if (r == null) {
                    r = new vw_POS_STKBalInUnitConvert { ItemID = itemId, QtyBalInToUnit = 0, CompanyID = comId, RCompanyID = rcom, LocID = "STORE" };
                }
                return r;
            }
        }
        public static vw_POS_STKBal GetStkBalByLoc(string itemId, string comId,string rcom, string locId) {
            vw_POS_STKBal r = new vw_POS_STKBal();
           
            using (GAEntities db = new GAEntities()) {

                r = db.vw_POS_STKBal.Where(o => o.RComID == rcom && o.ItemID == itemId && o.ComID == comId && o.LocID == locId).FirstOrDefault();
                return r;
            }
        }

        public static List<vw_POS_STKBal> ListStkBalByComID(string rcom,string comId) {
            List<vw_POS_STKBal> r = new List<vw_POS_STKBal>(); 
            using (GAEntities db = new GAEntities()) {
                r = db.vw_POS_STKBal.Where(o => o.RComID == rcom && o.ComID == comId).ToList();
                return r;
            }
        }

        public static List<vw_POS_STKBal> ListViewStockBalance(I_StockFiterSet f) {

             
           
            List<vw_POS_STKBal> result = new List<vw_POS_STKBal>();
            using (GAEntities db = new GAEntities()) {
                if (f.IsShowAviable) {
                    result = db.vw_POS_STKBal.Where(o =>
                                                                          (
                                                                                        o.ItemID.Contains(f.SearchText)
                                                                                      || o.ItemName.Contains(f.SearchText)
                                                                                      || o.TypeID.Contains(f.SearchText)
                                                                                      || o.LotNo.Contains(f.SearchText)
                                                                                      || o.SerialNo.Contains(f.SearchText)
                                                                                      || f.SearchText == ""
                                                                          )
                                                                          && o.RComID == f.Rcom
                                                                          && o.ComID == f.Company
                                                                          && (o.TypeID == f.ItemType || f.ItemType == "")
                                                                          && (o.LocID == f.Location || f.Location == "X")
                                                                          && o.IsActive == true
                                                                          && (Math.Abs(o.OrdQty) + Math.Abs(o.BalQty) + Math.Abs(o.InstQty) + Math.Abs(o.RetQty)) != 0
                                                                          ).ToList();
                } else {
                    result = db.vw_POS_STKBal.Where(o =>
                                                                     (
                                                                                        o.ItemID.Contains(f.SearchText)
                                                                                     || o.ItemName.Contains(f.SearchText)
                                                                                     || o.TypeID.Contains(f.SearchText)
                                                                                     || o.LotNo.Contains(f.SearchText)
                                                                                     || o.SerialNo.Contains(f.SearchText)
                                                                                     || f.SearchText == ""
                                                                     )
                                                                     && o.RComID == f.Rcom
                                                                     && o.ComID == f.Company
                                                                     && (o.TypeID == f.ItemType || f.ItemType == "")
                                                                     && (o.LocID == f.Location || f.Location == "X")
                                                                     && o.IsActive == true
                                                                     ).ToList();
                }

                return result;
            }
        }

        // รายงาน สต็อกคงเหลือแบบเลือกวันที่
        public static List<SP_GetStkBalByDate_Result> ReportStockBalByDate(string rcom, string comid, string locid, DateTime date) { 
            List<SP_GetStkBalByDate_Result> result = new List<SP_GetStkBalByDate_Result>();
            using (var connection = new SqlConnection(Globals.GAEntitiesConn)) { 
                var sql = "exec SP_GetStkBalByDate @rcompany, @company,@LocID,@Date";
                var values = new { rcompany = rcom, company = comid, LocID= locid , Date= date };
                result = connection.Query<SP_GetStkBalByDate_Result>(sql, values).ToList();
            }
          
            return result;
        }

      

        public static List<vw_POS_STKMove> ListViewStockMove(I_StockFiterSet f) {

            
            List<vw_POS_STKMove> result = new List<vw_POS_STKMove>();
            using (GAEntities db = new GAEntities()) {

                result = db.vw_POS_STKMove.Where(o =>
                                                    (
                                                            o.ItemID.Contains(f.SearchText)
                                                            || o.ItemName.Contains(f.SearchText)
                                                            || o.LotNo.Contains(f.SearchText)
                                                            || o.SerialNo.Contains(f.SearchText)
                                                            || o.DocID.Contains(f.SearchText)
                                                            || f.SearchText == ""
                                                    )
                                                    && (o.DocDate >= f.DateFrom && o.DocDate <= f.DateTo)
                                                    && o.ComID == f.Company
                                                    && o.RComID == f.Rcom
                                                    && (o.LocID == f.Location || f.Location == "X")
                                                    && o.IsActive
                                                    ).OrderByDescending(o => o.CreatedDate).ToList();

            }
            return result;
        }

        public static List<vw_VendorInfo> ListViewVendor(string search) {
            List<vw_VendorInfo> result = new List<vw_VendorInfo>();
            using (GAEntities db = new GAEntities()) {
                result = db.vw_VendorInfo.Where(o =>
                o.VendorID.ToLower().Contains(search)
                || o.NameTh1.ToLower().Contains(search)
                || o.NameTh2.ToLower().Contains(search)
                || o.FullNameTh.ToLower().Contains(search)
                && o.IsActive).ToList();
            }
            return result;
        }

        public static List<vw_POS_STKBal> ListPOS_STKBal(string rcom,string comid, string typeId) {
            List<vw_POS_STKBal> result = new List<vw_POS_STKBal>();
         
            using (GAEntities db = new GAEntities()) {
                result = db.vw_POS_STKBal.Where(o => o.RComID == rcom
                                                                && o.ComID == comid
                                                                && o.TypeID == typeId
                                                                && o.IsActive
                                                                ).ToList();
            }
            return result;
        }

        #endregion
    }
}
