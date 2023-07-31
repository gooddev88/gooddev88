using Robot.Data.ML;
using Robot.Data.GADB.TT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Robot.Data.ML.I_Result;
using Newtonsoft.Json;
using Blazored.SessionStorage;
using System.Text.Json;

namespace Robot.Data.DA
{
    public class BookBankService {

        public static string sessionActiveId = "activearbookid";
        #region Class
        public class I_BookBankSet {
            public BookBankInfo Book { get; set; }
            public List<TransactionLog> Log { get; set; }
            public I_BasicResult OutputAction { get; set; }

        }
        public class I_BookBankFiterSet {
            public String RCom { get; set; }
            public String Com { get; set; }
            public String SearchBy { get; set; }
            public String SearchText { get; set; }
            public String Status { get; set; }
            public bool ShowNotActive { get; set; }
        }

        #endregion

        public I_BookBankSet DocSet { get; set; }
        ISessionStorageService sessionStorage;

        public BookBankService(ISessionStorageService _sessionStorage) {
            sessionStorage = _sessionStorage;

        }


        #region Query Transaction

        public I_BookBankSet GetDocSet(string docid, string rcom, string com) {
            I_BookBankSet n = NewTransaction(rcom, com);

            using (GAEntities db = new GAEntities()) {
                n.Book = db.BookBankInfo.Where(o => o.BookID == docid && o.RCompanyID == rcom && o.CompanyID == com).FirstOrDefault();
                n.Log = TransactionService.ListLogByDocID(docid, rcom, com);
            }
            return n;
        }

        async public void SetSessionBookBankFiterSet(I_BookBankFiterSet data) {
            JsonSerializerOptions jso = new JsonSerializerOptions();
            jso.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
            string json = System.Text.Json.JsonSerializer.Serialize(data, jso);
            await sessionStorage.SetItemAsync("bookbank_Fiter", json);
        }
        async public Task<I_BookBankFiterSet> GetSessionORDFiterSet() {
            I_BookBankFiterSet result = NewFilterSet();
            var strdoc = await sessionStorage.GetItemAsync<string>("bookbank_Fiter");
            return JsonConvert.DeserializeObject<I_BookBankFiterSet>(strdoc);
        }

        public static BookBankInfo GetBookBankInfo(string docid, string rcom, string com) {
            BookBankInfo result = new BookBankInfo();
            using (GAEntities db = new GAEntities()) {
                result = db.BookBankInfo.Where(o => o.BookID == docid && o.RCompanyID == rcom && o.CompanyID == com && o.IsActive == true).FirstOrDefault();
            }
            return result;
        }
        public static List<BookBankInfo> ListBook(string rcom, string comId) {
            List<BookBankInfo> result = new List<BookBankInfo>();
            using (GAEntities db = new GAEntities()) {
                result = db.BookBankInfo.Where(o =>
                                                        o.RCompanyID == rcom
                                                        && (o.CompanyID == comId || comId == "")
                                                        && o.IsActive == true
                                                        )
                                                        .OrderByDescending(o => o.Sort).ToList();
            }
            return result;
        }

        public static List<BankInfo> ListBank(bool isShowBlankRow) {

            List<BankInfo> result = new List<BankInfo>();
            using (GAEntities db = new GAEntities()) {
                try {
                    result = db.BankInfo.Where(o => o.IsActive == true).ToList();

                    if (isShowBlankRow) {
                        BankInfo blank = new BankInfo { BankCode = "", Name_TH = "", Name_EN = "", Sort = 0 };
                        result.Insert(0, blank);
                    }
                } catch (Exception ex) {
                }
            }
            return result;
        }


        public static List<BookBankInfo> ListDoc(I_BookBankFiterSet f) {
            List<BookBankInfo> result = new List<BookBankInfo>();
            using (GAEntities db = new GAEntities()) {

                result = db.BookBankInfo.Where(o => (
                                           o.BookID.Contains(f.SearchText)
                                           || o.BookDesc.Contains(f.SearchText)
                                           || o.BankName.Contains(f.SearchText)
                                           || o.BookDesc.Contains(f.SearchText)
                                           || o.BankCode.Contains(f.SearchText)
                                           || o.BookID.Contains(f.SearchText)
                                           || f.SearchText == ""
                                          )
                                          && o.RCompanyID == f.RCom
                                          && o.CompanyID == f.Com
                                              && (o.IsActive == !f.ShowNotActive)
                                             ).OrderByDescending(o => o.Sort).ToList();

                return result;
            }
        }

        #endregion

        #region Save
        public static I_BasicResult Save(I_BookBankSet doc, string rcom, string com, string createby) {
            var h = doc.Book;
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                using (GAEntities db = new GAEntities()) {
                    var uh = db.BookBankInfo.Where(o => o.BookID == h.BookID && o.RCompanyID == rcom && o.CompanyID == com).FirstOrDefault();
                    if (uh == null) {
                        db.BookBankInfo.Add(doc.Book);
                        db.SaveChanges();
                        TransactionService.SaveLog(new TransactionLog { TransactionID = doc.Book.BookID, TableID = "BOOKBANK", ParentID = "", TransactionDate = DateTime.Now, CompanyID = doc.Book.CompanyID, Action = "Create NEW BookBank" }, rcom, doc.Book.BookID, createby);
                    } else {
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
                        TransactionService.SaveLog(new TransactionLog { TransactionID = doc.Book.BookID, TableID = "BOOKBANK", ParentID = "", TransactionDate = DateTime.Now, CompanyID = doc.Book.CompanyID, Action = "Update BookBank" }, rcom, doc.Book.BookID, createby);
                    }
                }
            } catch (Exception ex) {
                result.Result = "fail";

                if (ex.InnerException != null) {
                    result.Message1 = ex.InnerException.ToString();
                } else {
                    result.Message1 = ex.Message.ToString();
                }
            }
            return result;
        }

        public static I_BookBankSet NewTransaction(string rcom, string com) {
            var doc = new I_BookBankSet();
            doc.Book = NewBook(rcom, com);
            doc.Log = new List<TransactionLog>();
            doc.OutputAction = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            return doc;
        }

        #endregion

        #region Newtransaction
        public static BookBankInfo NewBook(string rcom, string com) {
            BookBankInfo n = new BookBankInfo();
            n.BookID = "";
            n.RCompanyID = rcom;
            n.CompanyID = com;
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

        public static int GenSort() {
            int result = 1;
            try {
                using (GAEntities db = new GAEntities()) {
                    var max_linenum = db.BookBankInfo.OrderByDescending(o => o.Sort).FirstOrDefault();
                    result = max_linenum.Sort + 1;
                }
            } catch { }
            return result;
        }

        public static I_BookBankFiterSet NewFilterSet() {
            var doc = new I_BookBankFiterSet();
            doc.RCom = "";
            doc.Com = "";
            doc.SearchBy = "";
            doc.SearchText = "";
            doc.Status = "OPEN";
            doc.ShowNotActive = false;

            return doc;
        }
        #endregion

        #region  Book bank
        public static List<BookBankInfo> ListBookBank(string rcom, string com, bool isShowBlankRow) {
            List<BookBankInfo> result = new List<BookBankInfo>();
            using (GAEntities db = new GAEntities()) {
                try {
                    result = db.BookBankInfo.Where(o => o.RCompanyID == rcom && o.CompanyID == com && o.IsActive == true).ToList();

                    if (isShowBlankRow) {
                        BookBankInfo blank = new BookBankInfo { BankCode = "", BankName = "", BookDesc = "", Sort = 0 };
                        result.Insert(0, blank);
                    }
                } catch (Exception ex) {
                }
            }
            return result;
        }

        public static BankInfo GetBankInfo(string BankCode) {
            BankInfo result = new BankInfo();
            using (GAEntities db = new GAEntities()) {
                result = db.BankInfo.Where(o => o.BankCode == BankCode).FirstOrDefault();
            }
            return result;
        }

        #endregion


        public static List<SelectOption> ListPromptPayType() {
            List<SelectOption> r = new List<SelectOption>();
            r.Add(new SelectOption { Value = "", Description = "ไม่มี", Sort = 0 });
            r.Add(new SelectOption { Value = "MOBILE", Description = "เบอร์โทรศัพท์", Sort = 1 });
            r.Add(new SelectOption { Value = "TAXID", Description = "เลขบัตรประชาชน", Sort = 2 });
            r.Add(new SelectOption { Value = "E_WALLET", Description = "W-Wallet", Sort = 3 });
            return r;
        }

    }

}
