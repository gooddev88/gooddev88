using Robot.Data;
using Robot.Data.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static Robot.Data.BL.I_Result;

namespace Robot.POS.DA {
    public   class OPaymentService {
        #region Class

        public class I_OPaymentSet {

            public OPaymentType Info { get; set; }
            public List<TransactionLog> Log { get; set; }
            public bool NeedRunNextID { get; set; }
            public I_BasicResult OutputAction { get; set; }
        }
        public class I_FiterSet {
            public String SearchText { get; set; }
            public bool ShowActive { get; set; }
        }

        #endregion

        #region Golbal var
        public static I_OPaymentSet DocSet { get { return (I_OPaymentSet)HttpContext.Current.Session["opayment_set"]; } set { HttpContext.Current.Session["opayment_set"] = value; } }
        public static bool IsNewDoc { get { return HttpContext.Current.Session["isnewdoc"] == null ? false : (bool)HttpContext.Current.Session["isnewdoc"]; } set { HttpContext.Current.Session["isnewdoc"] = value; } }
        public static I_FiterSet MyFilter { get { return (I_FiterSet)HttpContext.Current.Session["opaymentfilter_set"]; } set { HttpContext.Current.Session["opaymentfilter_set"] = value; } }

        #endregion
        #region  get data
        public static OPaymentType GetPayType(string code,string com) {
            OPaymentType result = new OPaymentType();
            var rcom = LoginService.LoginInfo.CurrentRootCompany.CompanyID;
            //var com = LoginService.LoginInfo.CurrentCompany;
            using (GAEntities db = new GAEntities()) {
                result = db.OPaymentType.Where(o =>  o.Code==code 
                                                            && o.RCompanyID==rcom
                                                            && (o.CompanyID == com || com == "")
                                                        ) .FirstOrDefault();
            }
            return result;

        }
        public static List<OPaymentType> ListPayType(string type,string com) {
            List<OPaymentType> result = new List<OPaymentType>();
            var rcom = LoginService.LoginInfo.CurrentRootCompany.CompanyID;
            //var com = LoginService.LoginInfo.CurrentCompany;
            using (GAEntities db = new GAEntities()) {
                result = db.OPaymentType.Where(o => o.IsActive
                                                        && o.RCompanyID==rcom
                                                        && (o.CompanyID==com || com == "")
                                                        && (o.Type == type || type=="")
                                                        ).OrderBy(o=>o.Sort).ToList();
                }
            return result;

        }
        #endregion


        public static void NewTransaction() {
            var rcom = LoginService.LoginInfo.CurrentRootCompany.CompanyID;
            //var com = LoginService.LoginInfo.CurrentCompany;
            DocSet = new I_OPaymentSet();
            DocSet.Info = NewInfo(rcom,""); 
            DocSet.NeedRunNextID = false;
            DocSet.Log = new List<TransactionLog>();
            DocSet.OutputAction = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            IsNewDoc = false;
        }

        public static OPaymentType NewInfo(string rcom,string com) {
            OPaymentType n = new OPaymentType();
            n.RCompanyID = rcom;
            n.CompanyID = "";
            n.Code = "";
            n.Type = "";
            n.Cate = "";
            n.Name = "";
            n.DRCR = "";
            n.AccID = "";
            n.Sort = 0;
            n.Remark = "";
            n.IsActive = true;
            return n;
        }
        public static List<OPaymentType> InitPaymentDataNewCompany(string rcom,string com) {
            var pp = new List<OPaymentType>();
            var n = NewInfo(rcom,com);
            n.Type = "PAYMENT";
            n.Code = "CASH";
            n.Name = "เงินสด";
            n.Sort = 10;
            pp.Add(n);
            //**********//
            n = NewInfo(rcom, com);
            n.Type = "PAYMENT";
            n.Code = "CHEQUE";
            n.Name = "เช็ค";
            n.Sort = 20;
            pp.Add(n);
            //**********//
            n = NewInfo(rcom, com);
            n.Type = "PAYMENT";
            n.Code = "TRANSFER";
            n.Name = "เงินโอน";
            n.Sort = 30;
            pp.Add(n);
            pp.Add(n);
            //**********//
            n = NewInfo(rcom, com);
            n.Type = "PAYMENT";
            n.Code = "OTHER";
            n.Name = "อื่นๆ";
            n.Sort = 40;
            pp.Add(n);
            //**********//
            n = NewInfo(rcom, com);
            n.Type = "TAX";
            n.Code = "WHT";
            n.Name = "หัก ณ ที่จ่าย";
            n.Sort = 110;
            pp.Add(n);
            //**********//
            n = NewInfo(rcom, com);
            n.Type = "ADJUST";
            n.Code = "TRANSFER_FEE";
            n.Name = "ค่าธรรมเนียมโอน";
            n.Sort = 1110;
            pp.Add(n);
            //**********//
            n = NewInfo(rcom, com);
            n.Type = "ADJUST";
            n.Code = "DIFF_DIGIT";
            n.Name = "ส่วนต่างเศษสตางค์";
            n.Sort = 1120;
            pp.Add(n);

            return pp;
        }
    }
}