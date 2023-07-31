using Microsoft.EntityFrameworkCore;
using Robot.Data.DA.Login;
using Robot.Data.GADB.TT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Robot.Data.ML.I_Result;

namespace Robot.Data.DA.Master {
    public class MasterTypeService {
        public static string sessionActiveId = "activemastertypeid";
        public class I_MasterTypeSet {

            public MasterTypeHead Head { get; set; }
            public List<MasterTypeLine> Line { get; set; }
            public MasterTypeLine LineActive { get; set; }
            public List<TransactionLog> Log { get; set; }
            public I_BasicResult OutputAction { get; set; }
        }

        public class I_MasterTypeFiterSet {
            public String SearchBy { get; set; }
            public String SearchText { get; set; }
            public String Status { get; set; }
            public bool ShowActive { get; set; }
        }

        public I_MasterTypeSet DocSet { get; set; }

        public MasterTypeService() {

        }

        public class SelectListMaster {
            public bool isChecked { get; set; } = false;
            public string MasterTypeID { get; set; }
            public string ValueTXT { get; set; }
            public string Description1 { get; set; }
            public string Description2 { get; set; }
            public int Sort { get; set; }
        }


        public I_MasterTypeSet GetDocSet(string docid, string rcom, string valuetxt) {
            I_MasterTypeSet n = NewTransaction(rcom);

            using (GAEntities db = new GAEntities()) {
                n.Head = db.MasterTypeHead.Where(o => o.MasterTypeID == docid).FirstOrDefault();
                if (n.Head.UseFor == "ALL") {
                    n.Line = db.MasterTypeLine.Where(o => o.MasterTypeID == docid).OrderBy(o => o.Sort).ToList();
                } else {
                    n.Line = db.MasterTypeLine.Where(o => o.MasterTypeID == docid && o.RComID == rcom).OrderBy(o => o.Sort).ToList();
                }
                //if (n.Line.Count==0) {
                //    n.LineActive = new MasterTypeLine { Description1 = "", Description2 = "", Description3 = "", Description4 = "NEW", ValueTXT = "", MasterTypeID = "", IsActive = true, ParentID = "", Sort = 1, RComID = "" };
                //} else {
                //    if (!string.IsNullOrEmpty(valuetxt)) {
                //        n.LineActive = n.Line.Where(o => o.ValueTXT == valuetxt).FirstOrDefault();
                //    } else {
                //        n.LineActive = NewLine(n.Head.UseFor, rcom, n.Line);
                //    }
                //}
                //if (n.LineActive==null) {
                //    n.LineActive = new MasterTypeLine { Description1 = "", Description2 = "", Description3 = "", Description4 = "NEW", ValueTXT = "", MasterTypeID = "", IsActive = true, ParentID = "", Sort = 1, RComID = "" };
                //}
               
            
            }
            return n;
        }

        public static I_MasterTypeSet SetActiveRow(I_MasterTypeSet doc, MasterTypeLine select) {
            doc.LineActive = doc.Line.Where(o => o.ValueTXT == select.ValueTXT).FirstOrDefault();
            return doc;
        }

        public MasterTypeHead GetMasterTypeHead(string typeId) {
            MasterTypeHead result = new MasterTypeHead();

            using (GAEntities db = new GAEntities()) {
                result = db.MasterTypeHead.Where(o => o.MasterTypeID == typeId).FirstOrDefault();
            }
            return result;
        }
        public static MasterTypeLine GetMasterLinebyValueTXT(string valuetxt, string rcom) {
            MasterTypeLine result = new MasterTypeLine();

            using (GAEntities db = new GAEntities()) {
                result = db.MasterTypeLine.Where(o => o.ValueTXT == valuetxt && o.RComID == rcom).FirstOrDefault();
            }
            return result;
        }

        public static MasterTypeLine GetType(string masterId, string valuetxt) {
            MasterTypeLine result = new MasterTypeLine();

            using (GAEntities db = new GAEntities()) {
                result = db.MasterTypeLine.Where(o => o.MasterTypeID == masterId && o.ValueTXT == valuetxt).FirstOrDefault();
                if (result == null) {
                    result = new MasterTypeLine { MasterTypeID = masterId, ValueTXT = "", Description1 = "", Description2 = "", Description3 = "", Description4 = "" };
                }
            }
            return result;
        }


        public static List<MasterTypeLine> ListType(string rcom, string masId, bool isShowBlank) {
            List<MasterTypeLine> result = new List<MasterTypeLine>();
            using (GAEntities db = new GAEntities()) {
                result = db.MasterTypeLine.Where(o => o.MasterTypeID == masId
                                                     && (o.RComID == rcom || o.RComID == "")
                                                        && o.IsActive == true).ToList();

                if (isShowBlank) {
                    MasterTypeLine n = new MasterTypeLine { MasterTypeID = "", ValueTXT = "", Description1 = "", Description2 = "", Description3 = "", Description4 = "" };
                    result.Insert(0, n);
                }
            }
            return result;
        }


        //public static List<MasterTypeLine> ListService_cate(string rcom, string cateId) {
        //    List<MasterTypeLine> result = new List<MasterTypeLine>();
        //    using (GAEntities db = new GAEntities()) {
        //        result = db.MasterTypeLine.Where(o => o.MasterTypeID == cateId
        //                                             && (o.RComID == rcom || o.RComID == "")
        //                                                && o.IsActive == true).ToList();

        //    }
        //    return result;
        //}

        public static List<MasterTypeLine> ListTypeByParentID(string masId, string ParentID, bool isShowBlank) {
            List<MasterTypeLine> result = new List<MasterTypeLine>();
            using (GAEntities db = new GAEntities()) {
                result = db.MasterTypeLine.Where(o => o.MasterTypeID == masId && o.ParentID == ParentID && o.IsActive == true).ToList();

                if (isShowBlank) {
                    MasterTypeLine n = new MasterTypeLine { MasterTypeID = "", ValueTXT = "", Description1 = "", Description2 = "", Description3 = "", Description4 = "" };
                    result.Insert(0, n);
                }
            }
            return result;
        }

        public List<SelectListMaster> ListSelectMasterType(string masterid) {
            List<SelectListMaster> listmaster = new List<SelectListMaster>();
            using (GAEntities db = new GAEntities()) {
                var line = db.MasterTypeLine.Where(o => o.MasterTypeID == masterid && o.IsActive == true).ToList();
                foreach (var l in line) {
                    SelectListMaster n = new SelectListMaster();

                    n.MasterTypeID = l.MasterTypeID;
                    n.ValueTXT = l.ValueTXT;
                    n.Description1 = l.Description1;
                    n.Description2 = l.Description2;
                    n.Sort = l.Sort;
                    listmaster.Add(n);
                }
            }

            return listmaster;
        }

        public static IEnumerable<MasterTypeHead> ListDoc(string search) {
            IEnumerable<MasterTypeHead> result;
            using (GAEntities db = new GAEntities()) {
                result = db.MasterTypeHead.Where(o => (
                                                      o.MasterTypeID.Contains(search)
                                                      || o.Name.Contains(search)
                                                      || search == ""
                                                     )
                                                     && o.IsActive == true
                                                     && o.UserAddNew == true
                                                     ).OrderByDescending(o => o.CreatedDate).AsNoTrackingWithIdentityResolution().ToArray();
            }
            return result;
        }


        #region Save
        public static I_BasicResult Save(MasterTypeLine data, LogInService.LoginSet login) {

            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                using (GAEntities db = new GAEntities()) {

                    var uh = db.MasterTypeLine.Where(o => o.ValueTXT == data.ValueTXT && o.MasterTypeID == data.MasterTypeID && o.RComID == data.RComID).FirstOrDefault();
                    if (uh == null) {
                        db.MasterTypeLine.Add(data);
                        db.SaveChanges();
                        TransactionService.SaveLog(new TransactionLog { TransactionID = data.ValueTXT, TableID = "MASTERTYPELINE", ParentID = "", TransactionDate = DateTime.Now, CompanyID = "", Action = "INSERT NEW Master" }, login.CurrentRootCompany.CompanyID, login.CurrentCompany.CompanyID, login.CurrentUser);
                    } else {
                        uh.ValueTXT = data.ValueTXT;
                        uh.Description1 = data.Description1;
                        uh.Description2 = data.Description2;
                        uh.Description3 = data.Description3;
                        uh.Sort = data.Sort;
                        uh.RefID = data.RefID;
                        uh.RefIDL2 = data.RefIDL2;
                        uh.RefIDL3 = data.RefIDL3;
                        uh.IsActive = data.IsActive;
                        db.SaveChanges();
                        TransactionService.SaveLog(new TransactionLog { TransactionID = data.ValueTXT, TableID = "MASTERTYPELINE", ParentID = "", TransactionDate = DateTime.Now, CompanyID = "", Action = "Update Master" }, login.CurrentRootCompany.CompanyID, login.CurrentCompany.CompanyID, login.CurrentUser);
                    }
                }
            } catch (Exception ex) {
                result.Result = "fail";
                if (ex.InnerException != null) {
                    result.Message1 = ex.InnerException.ToString();
                } else {
                    result.Message1 = ex.Message.ToLower();

                }
            }
            return result;
        }

        public static I_BasicResult ReOrder(List<MasterTypeLine> data) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                using (GAEntities db = new GAEntities()) {
                    foreach (var d in data) {
                        var query = db.MasterTypeLine.Where(o => o.MasterTypeID == d.MasterTypeID && o.RComID == d.RComID && o.ValueTXT == d.ValueTXT).FirstOrDefault();
                        if (query != null) {
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
        #endregion


        #region Newtransaction

        public static I_MasterTypeSet NewTransaction(string rcom) {
            var doc = new I_MasterTypeSet();
            doc.Head = new MasterTypeHead();
            doc.Line = new List<MasterTypeLine>();
            doc.LineActive = NewLine("ALL", rcom, doc.Line);
            doc.Log = new List<TransactionLog>();
            doc.OutputAction = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            return doc;
        }



        public static MasterTypeLine NewLine(string useFor, string rcom, List<MasterTypeLine> line) {
            MasterTypeLine n = new MasterTypeLine();

            n.MasterTypeID = "";
            if (useFor.ToUpper() == "ALL") {
                n.RComID = "";
            } else {
                n.RComID = rcom;
            }
            n.ValueTXT = "";
            n.ValueNUM = 0;
            n.Description1 = "";
            n.Description2 = "";
            n.Description3 = "";
            n.Description4 = "NEW";
            n.ParentID = "";
            n.ParentValue = "";
            n.Sort = GenSort(line);
            n.RefID = "";
            n.RefIDL2 = "";
            n.RefIDL3 = "";
            n.IsSysData = false;
            n.RefID = "";
            n.IsActive = true;
            return n;
        }
        public static int GenSort(List<MasterTypeLine> line) {
            int result = 1;
            try {
                var max_linenum = line.Max(o => o.Sort);
                result = max_linenum + 1;
            } catch { }
            return result;
        }
        public static MasterTypeLine AddLineItem(string checkExistItem, string usefor, string rcom, I_MasterTypeSet input) {
            input.Line.RemoveAll(o => o.Description4 == "NEW");
            input.LineActive = input.Line.Where(o => o.ValueTXT == checkExistItem).FirstOrDefault();
            if (input.LineActive == null) {//new line
                input.Line.Add(NewLine(usefor, rcom, input.Line));
                input.LineActive = input.Line.Where(o => o.Description4 == "NEW").OrderByDescending(o => o.Sort).FirstOrDefault();
            }

            return input.LineActive;
        }

        public static I_MasterTypeFiterSet NewFilterSet() {
            var doc = new I_MasterTypeFiterSet();
            doc.SearchBy = "";
            doc.SearchText = "";
            doc.ShowActive = true;

            return doc;
        }


        #endregion


    }
}
