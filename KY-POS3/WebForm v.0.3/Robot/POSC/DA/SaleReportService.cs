using Robot.Data;
using Robot.Data.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Robot.POSC.DA
{
    public class SaleReportService
    {
        #region Class
        public class I_Filter
        {
            public DateTime date_begin { get; set; }
            public DateTime date_end { get; set; }
            public string docid { get; set; }
            public string company { get; set; }
            public string searchText { get; set; }
            public string reportname { get; set; }
            public bool isactive { get; set; }
        }
        #endregion
        #region global var
        public static I_Filter PrintFilter { get { return (I_Filter)HttpContext.Current.Session["printfilter"]; } set { HttpContext.Current.Session["printfilter"] = value; } }
        public static List<vw_POS_SaleHead> SaleList { get { return (List<vw_POS_SaleHead>)HttpContext.Current.Session["pos_sale_headlist"]; } set { HttpContext.Current.Session["pos_sale_headlist"] = value; } }
        #endregion
   


        #region Transaction
       
        public static void NewFilter() {
            PrintFilter = new I_Filter();
            PrintFilter.docid = "";
            PrintFilter.company = "";
            PrintFilter.reportname = "";
            PrintFilter.date_begin = DateTime.Now.Date;
            PrintFilter.date_end = DateTime.Now.Date;
            PrintFilter.searchText = "";
            PrintFilter.isactive = true;
         
        }
        #endregion

        #region New Report
        public static void ListDailySale() {
            SaleList = new List<vw_POS_SaleHead>();
            var comlist = LoginService.LoginInfo.UserInCompany;
            var rcom = LoginService.LoginInfo.CurrentRootCompany.CompanyID;
            var f = PrintFilter;
            using (GAEntities db = new GAEntities()) {
                if (f.searchText.Trim() != "") {//search แบบระบุคำเสริช
                    SaleList = db.vw_POS_SaleHead.Where(o =>
                                            (
                                                        o.INVID.Contains(f.searchText)
                                                        || o.BillID.Contains(f.searchText)
                                                        || o.CustBranchName.Contains(f.searchText)
                                                        || o.CustBranchID.Contains(f.searchText)
                                                        || o.TableID.Contains(f.searchText)

                                                )
                                                && (o.ComID == f.company)
                                                && comlist.Contains(o.ComID)
                                                && o.IsActive == f.isactive
                                                && o.RComID == rcom
                                                ).OrderByDescending(o => o.CreatedDate).ToList();
                } else {//search แบบระบุวันที่
                    SaleList = db.vw_POS_SaleHead.Where(o =>
                                                (o.BillDate >= f.date_begin && o.BillDate <= f.date_end)
                                                && (o.ComID == f.company )
                                                && comlist.Contains(o.ComID)
                                                && o.IsActive == f.isactive
                                                && o.RComID == rcom
                                            ).OrderByDescending(o => o.CreatedDate).ToList();

                }
            }

        }

        public static List<SP_RptPOS133A_Result> ReportPOS133A(string comId, DateTime dtbegin, DateTime dtend)
        {
            //สรุปยอดขายรายวัน
            List<SP_RptPOS133A_Result> result = new List<SP_RptPOS133A_Result>();
            var rcom = LoginService.LoginInfo.CurrentRootCompany.CompanyID;
            using (GAEntities db = new GAEntities())
            {
                result = db.SP_RptPOS133A(rcom, comId, dtbegin, dtend).ToList();
            }
            return result;
        }

        public static List<SP_RptPOS133_Result> ReportPOS133(string comId, DateTime dtbegin, DateTime dtend) {
            //สรุปยอดขายรายวัน
            List<SP_RptPOS133_Result> result = new List<SP_RptPOS133_Result>();
            var rcom = LoginService.LoginInfo.CurrentRootCompany.CompanyID;
            using (GAEntities db = new GAEntities()) {
                result = db.SP_RptPOS133(rcom, comId, dtbegin, dtend).ToList();
            }
            return result;
        }
        public static List<SP_RptPOS134_Result> ReportPOS134(string comId, DateTime dtbegin, DateTime dtend) {
            //สรุปยอดขายรายวัน
            List<SP_RptPOS134_Result> result = new List<SP_RptPOS134_Result>();
            var rcom = LoginService.LoginInfo.CurrentRootCompany.CompanyID;
            using (GAEntities db = new GAEntities()) {
                result = db.SP_RptPOS134(rcom, comId, dtbegin, dtend).ToList();
            }
            return result;
        }


        public static List<vw_POS_SaleHead> ListInvoice(string com, DateTime begain, DateTime end) {
            List<vw_POS_SaleHead> result = new List<vw_POS_SaleHead>();
            var comlist = LoginService.LoginInfo.UserInCompany;
            var rcom = LoginService.LoginInfo.CurrentRootCompany.CompanyID;
            using (GAEntities db = new GAEntities()) {
                result = db.vw_POS_SaleHead.Where(o =>
                                                           (o.ComID == com || com == "")

                                                            && o.BillDate >= begain && o.BillDate <= end
                                                            && o.INVID != ""
                                                            && o.RComID == rcom
                                                            && comlist.Contains(o.ComID)
                                                            && o.IsActive
                                                ).ToList();
            }
            return result;
        }
        #endregion
 


    }
}