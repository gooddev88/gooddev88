
using Dapper;
using Robot.Data;
using Robot.Data.DataAccess;
using Robot.Data.FileStore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using static Robot.Data.BL.I_Result;
using static Robot.Data.DataAccess.DBDapperService;

namespace Robot.POS.DA
{
    public static class POSStockService
    {

        public class I_StockFiterSet
        {
            public DateTime DateFrom { get; set; }
            public DateTime DateTo { get; set; }
            public String SearchText { get; set; }
            public String SearchItem { get; set; }
            public String Company { get; set; }
            public String Location { get; set; }
            public string ItemType { get; set; }
            public bool IsShowAviable { get; set; }
        }

        public static I_StockFiterSet FilterSet { get { return (I_StockFiterSet)HttpContext.Current.Session["stockfilter_set"]; } set { HttpContext.Current.Session["stockfilter_set"] = value; } }

        #region Query Transaction

        public static POS_STKBal GetBalance(string itemId, string comId) {
            POS_STKBal r = new POS_STKBal();
            var rcom = LoginService.LoginInfo.CurrentRootCompany.CompanyID;
            using (GAEntities db = new GAEntities()) {

                r = db.POS_STKBal.Where(o => o.RComID == rcom && o.ItemID == itemId && o.ComID == comId && o.LocID== "STORE").FirstOrDefault();
                if (r == null) {
                    r = new POS_STKBal { ItemID = itemId, BalQty = 0, InstQty = 0, OrdQty = 0, RetQty = 0, ComID = comId, RComID = rcom,LocID= "STORE" };
                }
                return r;
            }
        }
        public static vw_POS_STKBalInUnitConvert GetBalanceByUnit(string itemId, string comId,string unit) {
            vw_POS_STKBalInUnitConvert r = new vw_POS_STKBalInUnitConvert();
            var rcom = LoginService.LoginInfo.CurrentRootCompany.CompanyID;
            using (GAEntities db = new GAEntities()) {

                r = db.vw_POS_STKBalInUnitConvert.Where(o => o.RCompanyID == rcom && o.ItemID == itemId && o.CompanyID == comId && o.LocID == "STORE").FirstOrDefault();
                if (r == null) {
                    r = new vw_POS_STKBalInUnitConvert { ItemID = itemId, QtyBalInToUnit = 0,  CompanyID = comId, RCompanyID = rcom, LocID = "STORE" };
                }
                return r;
            }
        }
        public static vw_POS_STKBal GetStkBalByLoc(string itemId, string comId, string locId) {
            vw_POS_STKBal r = new vw_POS_STKBal();
            var rcom = LoginService.LoginInfo.CurrentRootCompany.CompanyID;
            using (GAEntities db = new GAEntities()) {

                r = db.vw_POS_STKBal.Where(o => o.RComID == rcom && o.ItemID == itemId && o.ComID == comId && o.LocID == locId).FirstOrDefault();
                return r;
            }
        }

        public static List<vw_POS_STKBal> ListStkBalByComID(string comId)
        {
            List<vw_POS_STKBal> r = new List<vw_POS_STKBal>();
            var rcom = LoginService.LoginInfo.CurrentRootCompany.CompanyID;
            using (GAEntities db = new GAEntities())
            {
                r = db.vw_POS_STKBal.Where(o => o.RComID == rcom && o.ComID == comId).ToList();
                return r;
            }
        }

        public static List<vw_POS_STKBal> ListViewStockBalance() {

            var f = FilterSet;
            var rcom = LoginService.LoginInfo.CurrentRootCompany.CompanyID;
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
                                                                          && o.RComID == rcom
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
                                                                     && o.RComID == rcom
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
        public static List<SP_GetStkBalByDate_Result> ReportStockBalByDate(string comid,string locid,DateTime date)
        {
            var rcom = LoginService.LoginInfo.CurrentRootCompany.CompanyID;
            List<SP_GetStkBalByDate_Result> result = new List<SP_GetStkBalByDate_Result>();
            using (GAEntities db = new GAEntities())
            {
                result = db.SP_GetStkBalByDate(rcom, comid, locid, date).ToList();
            }
            return result;
        }

        public static List<vw_POS_STKMove> ListViewStockMove() {

            var f = FilterSet;
            var rcom = LoginService.LoginInfo.CurrentRootCompany.CompanyID;
            List<vw_POS_STKMove> result = new List<vw_POS_STKMove>();
            using (GAEntities db = new GAEntities()) {
            
                    result = db.vw_POS_STKMove.Where(o =>
                                                        (
                                                                o.ItemID.Contains(f.SearchText)
                                                                || o.ItemName.Contains(f.SearchText) 
                                                                || o.LotNo.Contains(f.SearchText)
                                                                || o.SerialNo.Contains(f.SearchText)
                                                                || o.DocID.Contains(f.SearchText) 
                                                                || f.SearchText==""
                                                        )
                                                        && (o.DocDate >= f.DateFrom && o.DocDate <= f.DateTo)
                                                        && o.ComID == f.Company
                                                        && o.RComID == rcom
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

        public static List<vw_POS_STKBal> ListPOS_STKBal(string company, string typeId) {
            List<vw_POS_STKBal> result = new List<vw_POS_STKBal>();
            var rcom = LoginService.LoginInfo.CurrentRootCompany.CompanyID;
            using (GAEntities db = new GAEntities()) {
                result = db.vw_POS_STKBal.Where(o => o.RComID == rcom
                                                                && o.ComID == company
                                                                && o.TypeID == typeId
                                                                && o.IsActive
                                                                ).ToList();
            }
            return result;
        }

        #endregion

        #region New transaction

        public static void NewFilterSet() {
            FilterSet = new I_StockFiterSet();
            FilterSet.DateFrom = DateTime.Now.Date;
            FilterSet.DateTo = DateTime.Now.Date;
            FilterSet.Company = "";
            FilterSet.Location = "";
            FilterSet.SearchText = "";
            FilterSet.SearchItem = "";
            FilterSet.ItemType = "";
            FilterSet.IsShowAviable = true;
        }

        #endregion
        #region  recal stock
        public static I_BasicResult RecalStock(DateTime begin,DateTime end,string rcom,string com) {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                string conStr = GetDBConnectFromAppConfig();
                using (var conn = new SqlConnection(conStr)) {
                    var dynamicParameters = new DynamicParameters();
                    string date_begin = begin.ToString("yyyy/MM/dd");
                    string date_end = end.ToString("yyyy/MM/dd");
                    string strSQL =string.Format(@"
                    exec [dbo].[SP_CalStkEndDay] @begin = N'{0}', @end = N'{1}', @rcom = N'{2}', 	@com = N'{3}', @forceRepeat = 1
", date_begin, date_end, rcom, com
);
                 var x = conn.Execute(strSQL);
                
                }
            } catch (Exception e) {
                r.Result = "fail";
                if (e.InnerException==null) {
                    r.Message1 = e.InnerException.ToString();
                } else {
                    r.Message1 = e.Message;
                }
            }
            return r;
        }
        #endregion
    }
}