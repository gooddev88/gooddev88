
using Robot.Data;
using Robot.Data.DataAccess;
using Robot.Data.FileStore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static Robot.Data.BL.I_Result;

namespace Robot.POS.DA
{
    public static class POSBOMService
    {

        public class I_BOMSet
        {
            public string Action { get; set; }
            public POS_BOMHead Info { get; set; }
            public List<POS_BOMLine> line { get; set; }
            public List<TransactionLog> Log { get; set; }
            public bool NeedRunNextID { get; set; }
            public I_BasicResult OutputAction { get; set; }
        }
        public class I_BOMFiterSet
        {
            public DateTime DateFrom { get; set; }
            public DateTime DateTo { get; set; }
            public String DocType { get; set; }
            public String Status { get; set; }
            public String SearchBy { get; set; }
            public String SearchText { get; set; }
            public bool ShowActive { get; set; }
        }

        public static string PreviousPage { get { return (string)HttpContext.Current.Session["bom_previouspage"]; } set { HttpContext.Current.Session["bom_previouspage"] = value; } }

        public static string DetailPreviousPage { get { return (string)HttpContext.Current.Session["bomdetail_previouspage"]; } set { HttpContext.Current.Session["bomdetail_previouspage"] = value; } }
        public static string NewDocPreviousPage { get { return (string)HttpContext.Current.Session["bomnewdoc_previouspage"]; } set { HttpContext.Current.Session["bomnewdoc_previouspage"] = value; } }
        public static bool IsNewDoc { get { return HttpContext.Current.Session["isnewdoc"] == null ? false : (bool)HttpContext.Current.Session["isnewdoc"]; } set { HttpContext.Current.Session["isnewdoc"] = value; } }

        public static I_BOMSet DocSet { get { return (I_BOMSet)HttpContext.Current.Session["bomdocset"]; } set { HttpContext.Current.Session["bomdocset"] = value; } }
        public static List<POS_BOMHead> DocList { get { return (List<POS_BOMHead>)HttpContext.Current.Session["bomdoc_list"]; } set { HttpContext.Current.Session["bomdoc_list"] = value; } }
        public static I_BOMFiterSet FilterSet { get { return (I_BOMFiterSet)HttpContext.Current.Session["bomfilter_set"]; } set { HttpContext.Current.Session["bomfilter_set"] = value; } }

        #region Query Transaction
        public static void GetDocSetByID(string docid,string rcom) {
            NewTransaction("");
            DocSet.OutputAction = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                using (GAEntities db = new GAEntities()) {
                    DocSet.Info = db.POS_BOMHead.Where(o => o.BomID == docid && o.RComID == rcom ).FirstOrDefault();
                    DocSet.line = db.POS_BOMLine.Where(o => o.BomID == docid && o.RComID == rcom && o.IsActive ).ToList();
                    DocSet.Log = TransactionInfoService.ListLogByDocID(docid);
                }
            } catch (Exception ex) {
                DocSet.OutputAction = new I_BasicResult { Result = "fail", Message1 = ex.Message, Message2 = "" };
            }
        }

        public static void ListDoc() {
            // var uic = LoginService.LoginInfo.UserInCompany;
            var rcom = LoginService.LoginInfo.CurrentRootCompany.CompanyID;
            string doctype = FilterSet.DocType.ToLower();
            string filterby = FilterSet.SearchBy.ToLower();
            string search = FilterSet.SearchText;
            bool isactive = FilterSet.ShowActive;
            DateTime begin = FilterSet.DateFrom;
            DateTime end = FilterSet.DateTo;
            using (GAEntities db = new GAEntities()) {

                DocList = db.POS_BOMHead.Where(o =>
                                                        (o.ComID.Contains(search)
                                                        || o.BomID.Contains(search)
                                                        || o.ItemIDFG.Contains(search)
                                                        || o.Description.Contains(search)
                                                        || search == "")
                                                        && (o.UserForModule == FilterSet.DocType || FilterSet.DocType == "")
                                                        //&& uic.Contains(o.ComID)
                                                        && o.RComID== rcom
                                                        && o.IsActive == isactive
                                                        ).OrderByDescending(o => o.CreatedDate).ToList();
                return;
            }
        }

        #endregion

        #region  Method GET

        public static POS_BOMHead GetBOM(string bomId,string rcom) {
            POS_BOMHead result = new POS_BOMHead();
        
            using (GAEntities db = new GAEntities()) {
                result = db.POS_BOMHead.Where(o => o.BomID == bomId && o.RComID == rcom).FirstOrDefault();
            }
            return result;
        }
        public static decimal GetItemVatRate(string itemId) {
            decimal result = 0;
            try {
                using (GAEntities db = new GAEntities()) {
                    var query = db.ItemInfo.Where(o => o.ItemID == itemId).FirstOrDefault();
                    var vat = db.TaxInfo.Where(o => o.TaxID == query.VatTypeID && o.Type == "SALE").FirstOrDefault();
                    if (vat != null) {
                        result = vat.TaxValue;
                    }

                }
            } catch (Exception) {

                throw;
            }

            return result;
        }
        public static List<vw_ItemUnit> ListItemUnit( string rcom,string itemId) {
            List<vw_ItemUnit> result = new List<vw_ItemUnit>();

            using (GAEntities db = new GAEntities()) {
                result = db.vw_ItemUnit.Where(o => o.RCompanyID == rcom && o.ItemID == itemId).ToList();
            }
            return result;
        }
        #endregion

        #region Save

        public static void Save() {
            var h = DocSet.Info;
            DocSet.OutputAction = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                using (GAEntities db = new GAEntities()) {
                    var b = db.POS_BOMHead.Where(o => o.BomID == DocSet.Info.BomID && o.RComID==DocSet.Info.RComID).FirstOrDefault();
                    if (b == null) {
                        CalDocSet();
                        checkDupID();
                        if (DocSet.OutputAction.Result == "fail") {
                            return;
                        }

                        db.POS_BOMHead.Add(DocSet.Info);
                        db.POS_BOMLine.AddRange(DocSet.line);
                        db.SaveChanges(); 
                            IDRuunerService.GetNewID("BOM", h.ComID, true, "th", h.CreatedDate); 
                        if (h.IsDefault) {
                            SetDefaultBom(h.BomID, h.RComID);
                        }
                        TransactionInfoService.SaveLog(new TransactionLog { TransactionID = DocSet.Info.BomID, TableID = "BOM", ParentID = "", TransactionDate = DateTime.Now, CompanyID = DocSet.Info.BomID, Action = "INSERT NEW BOM" });

                    } else {
                        CalDocSet();
                        b.ItemIDFG = DocSet.Info.ItemIDFG;
                        b.ItemUnitFG = DocSet.Info.ItemUnitFG;
                        b.Description = DocSet.Info.Description;
                        b.Remark1 = DocSet.Info.Remark1;
                        b.IsDefault = DocSet.Info.IsDefault;
                        b.Priority = DocSet.Info.Priority;
                        b.UserForModule = DocSet.Info.UserForModule;
                        b.IsActive = DocSet.Info.IsActive;

                        db.POS_BOMLine.RemoveRange(db.POS_BOMLine.Where(o => o.BomID == h.BomID &&  o.RComID==h.RComID));
                        db.POS_BOMLine.AddRange(DocSet.line);

                        db.SaveChanges();
                        if (h.IsDefault) {
                            SetDefaultBom(h.BomID, h.RComID);
                        }
                        TransactionInfoService.SaveLog(new TransactionLog { TransactionID = DocSet.Info.BomID, TableID = "BOM", ParentID = "", TransactionDate = DateTime.Now, CompanyID = DocSet.Info.BomID, Action = "UPDATE BOM" });
                    }

                }
            } catch (Exception ex) {
                DocSet.OutputAction.Result = "fail";

                if (ex.InnerException != null) {
                    DocSet.OutputAction.Message1 = ex.InnerException.ToString();
                } else {
                    DocSet.OutputAction.Message1 = ex.Message;
                }
            }
        }

        public static void CalDocSet() {
            var h = DocSet.Info;

            //1. copy head to line
            foreach (var l in DocSet.line) {

                l.BomID = h.BomID;
                l.UserForModule = h.UserForModule;
                l.ComID = h.ComID;
                l.RComID = h.RComID;
                l.ItemIDFG = h.ItemIDFG;
                l.FgUnit = h.ItemUnitFG;
                l.IsDefault = h.IsDefault;
            }
        }


        public static I_BasicResult NewLineByBomLine(POS_BOMLine bomline, string rmitem) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };

            try {
                var bline = DocSet.line.Where(o => o.ItemIDRM == rmitem).FirstOrDefault();
                if (bline == null) {//add new item
                    DocSet.line.Add(bomline);
                } else {
                    //exist item
                    bline.RmQty = bomline.RmQty;
                }
            } catch (Exception ex) {
                result.Result = "fail";
                if (ex.InnerException != null) {
                    result.Message1 = ex.InnerException.ToString();
                } else {
                    result.Message1 = ex.Message;
                }
            }
            return result;
        }


        public static bool checkDupID() {
            bool result = false;
            try {
                DocSet.OutputAction = new I_BasicResult { Result = "fail", Message1 = "Duplicate Bom code", Message2 = "" };
                string bomid = DocSet.Info.BomID;
                string rcom = DocSet.Info.RComID;
                using (GAEntities db = new GAEntities()) {
                    var get_id = db.POS_BOMHead.Where(o => o.BomID == bomid && o.RComID==rcom).FirstOrDefault();

                    if (get_id == null) {
                        result = true;
                        DocSet.OutputAction = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
                    }
                }
            } catch (Exception ex) {
            }
            return result;
        }

        #endregion

        #region Delete

        public static void DeleteDoc(string docid,string rcom) {
            DocSet.OutputAction = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                using (GAEntities db = new GAEntities()) {
                    var head = db.POS_BOMHead.Where(o => o.BomID == docid && o.RComID== rcom && o.IsActive == true).FirstOrDefault();
                    head.ModifiedBy = LoginService.LoginInfo.CurrentUser;
                    head.ModifiedDate = DateTime.Now;
                    head.IsActive = false;

                    db.SaveChanges();
                    TransactionInfoService.SaveLog(new TransactionLog { TransactionID = DocSet.Info.BomID, TableID = "BOM", ParentID = "", TransactionDate = DateTime.Now, CompanyID = DocSet.Info.ComID, Action = "DELETE DATA BOM" });
                }
            } catch (Exception ex) {

                DocSet.OutputAction = new I_BasicResult { Result = "fail", Message1 = ex.Message, Message2 = "" };
            }
        }

        public static void DeleteBOMLine(int linenum) {
            DocSet.OutputAction = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                DocSet.line.RemoveAll(o => o.LineNum == linenum);
            } catch (Exception ex) {

                DocSet.OutputAction.Result = "fail";
                DocSet.OutputAction.Message1 = ex.Message;
                if (ex.InnerException != null) {
                    DocSet.OutputAction.Message1 = DocSet.OutputAction.Message1 + " " + ex.InnerException.Message;
                }
            }
        }

        #endregion

        #region New transaction

        public static void NewFilterSet() {
            FilterSet = new I_BOMFiterSet();
            FilterSet.DateFrom = DateTime.Now.Date;
            FilterSet.DateTo = DateTime.Now.Date;
            FilterSet.DocType = "";
            FilterSet.SearchBy = "DOCDATE";
            FilterSet.SearchText = "";
            FilterSet.ShowActive = true;
        }

        public static void NewTransaction(string doctype) {
            DocSet = new I_BOMSet();
            DocSet.Action = "";
            DocSet.Info = NewHead();
            DocSet.line = new List<POS_BOMLine>();
            DocSet.NeedRunNextID = false;
            DocSet.Log = new List<TransactionLog>();
            DocSet.OutputAction = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            IsNewDoc = false;
        }

        public static POS_BOMHead NewHead() {
            POS_BOMHead n = new POS_BOMHead();

            n.ComID = "";
            n.RComID = LoginService.LoginInfo.CurrentRootCompany.CompanyID;
            n.BomID = "";
            n.UserForModule = "SALE";
            n.ItemIDFG = "";
            n.ItemUnitFG = "";
            n.Description = "";
            n.Remark1 = "";
            n.IsDefault = true;
            n.Priority = 0;
            n.CreatedBy = LoginService.LoginInfo.CurrentUser;
            n.CreatedDate = DateTime.Now;
            n.ModifiedBy = "";
            n.ModifiedDate = null;
            n.IsActive = true;
            return n;
        }

        public static POS_BOMLine NewLine() {
            POS_BOMLine n = new POS_BOMLine();

            n.ComID = "";
            n.RComID = "";
            n.BomID = "";
            n.UserForModule = "SALE";
            n.LineNum = GenLineLineNum();
            n.ItemIDFG = "";
            n.ItemIDRM = "";
            n.RMDescription = "";
            n.FgQty = 1;
            n.RmQty = 0;
            n.FgUnit = "";
            n.RmUnit = "";
            n.IsActive = true;
            return n;
        }

        public static int GenLineLineNum() {
            int result = 10;
            try {
                var max_linenum = DocSet.line.Max(o => o.LineNum);
                result = max_linenum + 10;
            } catch { }
            return result;
        }

        #endregion


        #region function move data management
        public static POS_BOMHead GetPreviousData(string currID) {
            POS_BOMHead result = new POS_BOMHead();
            var rcom = LoginService.LoginInfo.CurrentRootCompany.CompanyID;
            using (GAEntities db = new GAEntities()) {
                var curr = db.POS_BOMHead.Where(o => o.BomID == currID && o.RComID==rcom).FirstOrDefault();
                result = db.POS_BOMHead.Where(o => o.ID < curr.ID && o.RComID == rcom && o.IsActive).FirstOrDefault();
            }
            return result;
        }
        public static POS_BOMHead GetLastData() {
            POS_BOMHead result = new POS_BOMHead();
            var rcom = LoginService.LoginInfo.CurrentRootCompany.CompanyID;
            using (GAEntities db = new GAEntities()) {
                result = db.POS_BOMHead.Where(o=>o.RComID==rcom).OrderByDescending(o => o.ID ).FirstOrDefault();
            }
            return result;
        }

        public static POS_BOMHead GetNextData(string currID) {
            POS_BOMHead result = new POS_BOMHead();
            var rcom = LoginService.LoginInfo.CurrentRootCompany.CompanyID;
            using (GAEntities db = new GAEntities()) {
                var curr = db.POS_BOMHead.Where(o => o.BomID == currID && o.RComID == rcom).FirstOrDefault();
                result = db.POS_BOMHead.Where(o => o.ID > curr.ID && o.RComID == rcom && o.IsActive).FirstOrDefault();
            }
            return result;
        }

        #endregion


        public static I_BasicResult SetDefaultBom(string bomId, string rComID) {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                using (GAEntities db =new GAEntities()) {
                    var bom = db.POS_BOMHead.Where(o => o.RComID == rComID && o.BomID == bomId).FirstOrDefault();
                    var otherBom = db.POS_BOMHead.Where(o => o.RComID == rComID && o.ItemIDFG == bom.ItemIDFG && o.BomID!=bomId && o.UserForModule==bom.UserForModule ).ToList();
              
                    foreach (var b in otherBom) {
                        b.IsDefault = false;
                    }
                    bom.IsDefault = true;
                    db.SaveChanges();
                }
            } catch (Exception ex) {
                r.Result = "fail";
                if (ex.InnerException != null) {
                    r.Message1 = ex.InnerException.ToString();
                } else {
                    r.Message1 = ex.Message;
                }
            }
            return r;
        }
    }
}