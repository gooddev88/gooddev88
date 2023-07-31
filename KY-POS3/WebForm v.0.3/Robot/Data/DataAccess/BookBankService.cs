using Robot.Data.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static Robot.Data.BL.I_Result;

namespace Robot.Data.DataAccess
{
    public static class BookBankService
    {
        #region Class
        public class I_BookBankSet
        {

            public BookBankInfo Book { get; set; }
            public List<TransactionLog> Log { get; set; }
            public I_BasicResult OutputAction { get; set; }

        }
        public class I_BookBankFiterSet
        {
            public String SearchBy { get; set; }
            public String SearchText { get; set; }
            public String Status { get; set; }
            public bool ShowInActive { get; set; }
        }
        public class SelectOption
        {
            public String Value { get; set; }
            public String Description { get; set; }
            public int Sort { get; set; } 
        }

        #endregion

        #region Global var



        public static bool IsNewDoc { get { return HttpContext.Current.Session["isnewdoc"] == null ? false : (bool)HttpContext.Current.Session["isnewdoc"]; } set { HttpContext.Current.Session["isnewdoc"] = value; } }
        public static I_BookBankSet DocSet { get { return (I_BookBankSet)HttpContext.Current.Session["bookbank_docset"]; } set { HttpContext.Current.Session["bookbank_docset"] = value; } }
        public static List<BookBankInfo> BooKList { get { return (List<BookBankInfo>)HttpContext.Current.Session["bookbank_list"]; } set { HttpContext.Current.Session["bookbank_list"] = value; } }
        public static I_BookBankFiterSet FilterSet { get { return (I_BookBankFiterSet)HttpContext.Current.Session["bookbankfilter_set"]; } set { HttpContext.Current.Session["bookbankfilter_set"] = value; } }
        #endregion

        #region Query Transaction
        public static void GetDocSetByID(string docid)
        {
            var com = LoginService.LoginInfo.CurrentCompany.CompanyID;
            var rcom = LoginService.LoginInfo.CurrentRootCompany.CompanyID;
            try
            {
                using (GAEntities db = new GAEntities())
                {
                    NewTransaction();
                    DocSet.Book = db.BookBankInfo.Where(o => o.BookID == docid && o.RCompanyID == rcom && o.CompanyID == com).FirstOrDefault();
                    DocSet.Log = TransactionInfoService.ListLogByDocID(docid);

                }
            }
            catch (Exception ex)
            {
                DocSet.OutputAction.Result = "ok";
                DocSet.OutputAction.Message1 = ex.Message;
                if (ex.InnerException != null)
                {
                    DocSet.OutputAction.Message1 = DocSet.OutputAction.Message1 + " " + ex.InnerException.Message;
                }
            }

        }
        public static BookBankInfo GetBookBankInfo(string docid)
        {
            var com = LoginService.LoginInfo.CurrentCompany.CompanyID;
            var rcom = LoginService.LoginInfo.CurrentRootCompany.CompanyID;

            BookBankInfo result = new BookBankInfo();
            using (GAEntities db = new GAEntities())
            {
                result = db.BookBankInfo.Where(o => o.BookID == docid && o.RCompanyID == rcom && o.CompanyID == com && o.IsActive).FirstOrDefault();
            }
            return result;
        }
        public static List<BookBankInfo> ListBook(string comId)
        {
            var com = LoginService.LoginInfo.CurrentCompany.CompanyID;
            var rcom = LoginService.LoginInfo.CurrentRootCompany.CompanyID;
            List<BookBankInfo> result = new List<BookBankInfo>();
            using (GAEntities db = new GAEntities())
            {
                result = db.BookBankInfo.Where(o =>
                                                        o.RCompanyID == rcom
                                                        && (o.CompanyID == comId || comId == "")
                                                        && o.IsActive
                                                        )
                                                        .OrderByDescending(o => o.Sort).ToList();

            }
            return result;
        }
        public static void ListDoc(bool isShowAll)
        {
            var com = LoginService.LoginInfo.CurrentCompany.CompanyID;
            var rcom = LoginService.LoginInfo.CurrentRootCompany.CompanyID ;
            var f = FilterSet;
            using (GAEntities db = new GAEntities())
            {
                if (f.SearchBy == "")
                {
                    if (isShowAll)
                    {
                        BooKList = db.BookBankInfo.Where(o => o.RCompanyID == rcom && o.CompanyID == com
                                                               && (o.IsActive == true || f.ShowInActive == true)
                                                            ).Take(50).OrderByDescending(o => o.Sort).ToList();
                    }
                    else
                    {
                        BooKList = db.BookBankInfo.Where(o => o.RCompanyID == rcom && o.CompanyID == com
                                                               && (o.IsActive == true || f.ShowInActive == true)
                                                                    ).OrderByDescending(o => o.Sort).ToList();
                    }

                }
                else
                {
                    BooKList = db.BookBankInfo.Where(o => (
                                                o.BookID.Contains(f.SearchBy)
                                                || o.BookDesc.Contains(f.SearchBy)
                                                || o.BankName.Contains(f.SearchBy)
                                                || o.BankCode.Contains(f.SearchBy)
                                                || f.SearchBy == ""
                                               )
                                               && o.RCompanyID == rcom
                                               && o.CompanyID == com
                                                   && (o.IsActive == true || f.ShowInActive == true)
                                                  ).OrderByDescending(o => o.Sort).ToList();
                }

            }
        }

        #endregion

        #region Save
        public static I_BasicResult Save()
        {
            var com = LoginService.LoginInfo.CurrentCompany.CompanyID;
            var rcom = LoginService.LoginInfo.CurrentRootCompany.CompanyID;
            var h = DocSet.Book;
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try
            {
                using (GAEntities db = new GAEntities())
                {
                    var uh = db.BookBankInfo.Where(o => o.BookID == h.BookID && o.RCompanyID == rcom && o.CompanyID == com).FirstOrDefault();
                    if (uh == null)
                    {
                        db.BookBankInfo.Add(DocSet.Book);
                        db.SaveChanges();
                        TransactionInfoService.SaveLog(new TransactionLog { TransactionID = DocSet.Book.BookID, TableID = "BOOKBANK", ParentID = "", TransactionDate = DateTime.Now, CompanyID = com, RCompanyID = rcom, Action = "Create New BookBank " + h.BookID });

                    }
                    else
                    {
                        uh.CompanyID = h.CompanyID;
                        uh.BookNo = h.BookNo;
                        uh.BookDesc = h.BookDesc;
                        uh.BankCode = h.BankCode;
                        uh.BankName = h.BankName;
                        uh.BookOwner = h.BookOwner;
                        uh.BranchName = h.BranchName;
                        uh.AcctType = h.AcctType;
                        uh.AccID = h.AccID;
                        uh.AccGroup = h.AccGroup;
                        uh.RefID = h.RefID;
                        uh.Remark1 = h.Remark1;
                        uh.Sort = h.Sort;
                        uh.IsActive = h.IsActive;
                        db.SaveChanges();
                        TransactionInfoService.SaveLog(new TransactionLog { TransactionID = DocSet.Book.BookID, TableID = "BOOKBANK", ParentID = "", TransactionDate = DateTime.Now, CompanyID = com, RCompanyID = rcom, Action = "Update BookBank " + h.BookID });
                    }

                }
            }
            catch (Exception ex)
            {
                result.Result = "fail";

                if (ex.InnerException != null)
                {
                    result.Message1 = ex.InnerException.ToString();
                }
                else
                {
                    result.Message1 = ex.Message.ToString();
                }

            }
            return result;
        }


        public static void NewTransaction()
        {
            DocSet = new I_BookBankSet();
            DocSet.Book = NewBook();
            DocSet.Log = new List<TransactionLog>();

            DocSet.OutputAction = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
        }

        #endregion

        #region Newtransaction
        public static BookBankInfo NewBook()
        {
            BookBankInfo n = new BookBankInfo();
            n.BookID = "";
            n.RCompanyID =  LoginService.LoginInfo.CurrentRootCompany.CompanyID;
            n.CompanyID = LoginService.LoginInfo.CurrentCompany.CompanyID;
            n.BookNo = "";
            n.BookDesc = "";
            n.BankCode = "";
            n.BankName = "";
            n.BranchName = "";
            n.BookOwner = "";
            n.AcctType = "";
            n.Sort = GenSort();
            n.AccGroup = "";
            n.AccID = "";
            n.RefID = "";
            n.Remark1 = "";
            n.IsActive = true;

            return n;
        }

        public static int GenSort()
        {
            int result = 1;
            try
            {
                using (GAEntities db = new GAEntities())
                {
                    var max_linenum = db.BookBankInfo.OrderByDescending(o => o.Sort).FirstOrDefault();
                    result = max_linenum.Sort + 1;
                }
            }
            catch { }
            return result;
        }



        public static void NewFilterSet()
        {
            FilterSet = new I_BookBankFiterSet();
            FilterSet.SearchBy = "";
            FilterSet.SearchText = "";
            FilterSet.Status = "OPEN";
            FilterSet.ShowInActive = false;
        }
        #endregion


        #region  Book bank
        public static List<BankInfo> ListBank(bool isShowBlankRow)
        {

            List<BankInfo> result = new List<BankInfo>();
            using (GAEntities db = new GAEntities())
            {
                result = db.BankInfo.Where(o => o.IsActive).ToList();

                if (isShowBlankRow)
                {
                    BankInfo blank = new BankInfo { BankCode = "", Name_TH = "", Name_EN = "", Sort = 0 };
                    result.Insert(0, blank);
                }

            }
            return result;
        }

        public static BankInfo GetBankInfo(string BankCode)
        {
            BankInfo result = new BankInfo();
            using (GAEntities db = new GAEntities())
            {
                result = db.BankInfo.Where(o => o.BankCode == BankCode).FirstOrDefault();
            }
            return result;
        }
       public static List<SelectOption> ListPromptPayType()
        {
            List<SelectOption> r = new List<SelectOption>();
            r.Add(new SelectOption { Value = "", Description = "ไม่มี", Sort = 0 });
            r.Add(new SelectOption { Value = "MOBILE", Description = "เบอร์โทรศัพท์", Sort = 1 });
            r.Add(new SelectOption { Value = "TAXID", Description = "เลขบัตรประชาชน", Sort = 2 });
            r.Add(new SelectOption { Value = "E_WALLET", Description = "W-Wallet", Sort = 3 });
            return r;
        }
        #endregion

    }
}