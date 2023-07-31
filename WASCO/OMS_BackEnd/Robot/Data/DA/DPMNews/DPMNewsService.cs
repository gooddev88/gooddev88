using Microsoft.EntityFrameworkCore;
using Robot.Data.DA.Login;
using Robot.Data.DA.Master;
using Robot.Data.DPMNEW.TT;
using Robot.Data.GADB.TT;
using System;
using System.Collections.Generic;
using System.Linq;
using static Robot.Data.ML.I_Result;

namespace Robot.Data.DA.DPMNews {
    public class DPMNewsService {

        public class I_NewsCateSet
        {
            public DPM_NewsCateKeywordHead Head { get; set; }
            public List<DPM_NewsCateKeywordLine> Line { get; set; }
            public List<TransactionLog> Log { get; set; }
            public I_BasicResult OutputAction { get; set; }
        }

        public I_NewsCateSet DocSet { get; set; }

        public DPMNewsService()
        {
            DocSet = NewTransaction("");
        }


        public I_NewsCateSet GetDocSet(string docid,string createby)
        {
            I_NewsCateSet n = NewTransaction(createby);

            using (DPMNewsEntities db = new DPMNewsEntities())
            {
                n.Head = db.DPM_NewsCateKeywordHead.Where(o => o.CateID == docid).FirstOrDefault();
                n.Line = db.DPM_NewsCateKeywordLine.Where(o => o.CateID == docid).OrderBy(o => o.CreatedDate).ToList();
            }
            return n;
        }

        //public static IEnumerable<DPM_NewsCateKeywordHead> ListDPM_NewsCateKeywordHead() {
        //    IEnumerable<DPM_NewsCateKeywordHead> result = new List<DPM_NewsCateKeywordHead>();
        //    using (DPMNewsEntities db = new DPMNewsEntities()) {
        //        result = db.DPM_NewsCateKeywordHead.AsNoTrackingWithIdentityResolution().ToArray();
        //        // SelectAddr = result.FirstOrDefault();
        //    }
        //    return result;
        //}

        public static IEnumerable<DPM_NewsCateKeywordHead> ListDPM_NewsCateKeywordHead()
        {
            IEnumerable<DPM_NewsCateKeywordHead> result;
            using (DPMNewsEntities db = new DPMNewsEntities())
            {
                result = db.DPM_NewsCateKeywordHead.OrderBy(o => o.ID).AsNoTrackingWithIdentityResolution().ToArray();
            }
            return result;
        }

        public static List<DPM_NewsCateKeywordHead> ListDoc()
        {
            List<DPM_NewsCateKeywordHead> result = new List<DPM_NewsCateKeywordHead>();
            using (DPMNewsEntities db = new DPMNewsEntities())
            {
                    result = db.DPM_NewsCateKeywordHead.OrderBy(o => o.Sort).ToList();
            }
            return result;
        }

        #region Save
        public static I_BasicResult Save(DPM_NewsCateKeywordHead data, LogInService.LoginSet login)
        {

            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try
            {
                using (DPMNewsEntities db = new DPMNewsEntities())
                {

                    var uh = db.DPM_NewsCateKeywordHead.Where(o => o.CateID == data.CateID).FirstOrDefault();
                    if (uh == null)
                    {
                        db.DPM_NewsCateKeywordHead.Add(data);
                        db.SaveChanges();
                        TransactionService.SaveLog(new TransactionLog { TransactionID = data.CateID, TableID = "NEWCATE", ParentID = "", TransactionDate = DateTime.Now, CompanyID = "", Action = "INSERT NEW Cate" }, login.CurrentRootCompany.CompanyID, login.CurrentCompany.CompanyID, login.CurrentUser);
                    }
                    else
                    {
                        uh.Name = data.Name;
                        uh.Remark = data.Remark;
                        uh.Sort = data.Sort;
                        uh.ModifiedBy = login.CurrentUser;
                        uh.ModifiedDate = DateTime.Now;
                        uh.IsActive = data.IsActive;
                        db.SaveChanges();
                        TransactionService.SaveLog(new TransactionLog { TransactionID = data.CateID, TableID = "NEWCATE", ParentID = "", TransactionDate = DateTime.Now, CompanyID = "", Action = "Update NEW Cate" }, login.CurrentRootCompany.CompanyID, login.CurrentCompany.CompanyID, login.CurrentUser);
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
                    result.Message1 = ex.Message.ToLower();

                }
            }
            return result;
        }


        public static I_BasicResult AddNewsCateKeywordLine(DPM_NewsCateKeywordLine data)
        {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try
            {
                using (DPMNewsEntities db = new DPMNewsEntities())
                {
                    var cateline = db.DPM_NewsCateKeywordLine.Where(o => o.CateID == data.CateID
                                                                && o.KeyWord == data.KeyWord
                                                                ).FirstOrDefault();
                    if (cateline == null)
                    {//add new 
                        db.DPM_NewsCateKeywordLine.Add(data);
                        db.SaveChanges();
                    }
                    else
                    {
                        //exist 
                        cateline.Remark = data.Remark;
                        cateline.IsActive = data.IsActive;
                        db.SaveChanges();
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
                    result.Message1 = ex.Message;
                }
            }
            return result;
        }

        #endregion

        public static I_BasicResult ReOrder(List<DPM_NewsCateKeywordHead> data) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                using (DPMNewsEntities db = new DPMNewsEntities()) {
                    foreach (var d in data) {
                        var query = db.DPM_NewsCateKeywordHead.Where(o => o.CateID == d.CateID).FirstOrDefault();
                        if (query!=null) {
                            query.Sort = d.Sort;
                        }
                    } 
                    db.SaveChanges();
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
        public static I_BasicResult DeleteNewsCateKeywordLine(int id)
        {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try
            {
                using (DPMNewsEntities db = new DPMNewsEntities())
                {
                    db.DPM_NewsCateKeywordLine.Remove(db.DPM_NewsCateKeywordLine.Where(o => o.ID == id).FirstOrDefault());
                    db.SaveChanges();
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
                    result.Message1 = ex.Message;
                }
            }
            return result;
        }

        #region Newtransaction
        public static I_NewsCateSet NewTransaction(string createby)
        {
            var doc = new I_NewsCateSet();
            doc.Head = NewHead(createby);
            doc.Line = new List<DPM_NewsCateKeywordLine>();
            doc.Log = new List<TransactionLog>();
            doc.OutputAction = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            return doc;
        }

        public static DPM_NewsCateKeywordHead NewHead(string createby)
        {
            DPM_NewsCateKeywordHead n = new DPM_NewsCateKeywordHead();

            n.CateID = "";
            n.Name = "";
            n.Remark = "";
            n.Sort = GenSort();
            n.CreatedBy = createby;
            n.CreatedDate = DateTime.Now;
            n.ModifiedBy = "";
            n.ModifiedDate = null;
            n.IsActive = true;
            return n;
        }

        public static DPM_NewsCateKeywordLine NewLine(string createby)
        {
            DPM_NewsCateKeywordLine n = new DPM_NewsCateKeywordLine();

            n.CateID = "";
            n.KeyWord = "";
            n.Remark = "";
            n.CreatedBy = createby;
            n.CreatedDate = DateTime.Now;
            n.IsActive = true;
            return n;
        }

        public static int GenSort()
        {
            int result = 1;
            try
            {
                using (DPMNewsEntities db = new DPMNewsEntities())
                {
                    var h = db.DPM_NewsCateKeywordHead.ToList();
                    var max_linenum = h.Max(o => o.Sort);
                    result = max_linenum + 1;
                }
            }
            catch { }
            return result;
        }

        #endregion

    }
}
