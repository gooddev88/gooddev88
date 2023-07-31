using Robot.Data.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static Robot.Data.BL.I_Result;

namespace Robot.Data.DataAccess
{
    public static class MasterTypeService
    {
        #region Class
        public class I_MasterTypeSet
        {
            public string Action { get; set; }
            public List<MasterTypeLine> Line { get; set; }
            public MasterTypeLine LineActive { get; set; }


            public List<TransactionLog> Log { get; set; }
            public I_BasicResult OutputAction { get; set; }
        }
        public class I_MasterTypeFiterSet
        {
            public String SearchBy { get; set; }
            public String SearchText { get; set; }
            public String Status { get; set; }
            public bool ShowActive { get; set; }
        }

        #endregion

        #region Global var



        public static string PreviousNewDocPage { get { return (string)HttpContext.Current.Session["mastertypenewdoc_previouspage"]; } set { HttpContext.Current.Session["mastertypenewdoc_previouspage"] = value; } }
        public static bool IsNewDoc { get { return HttpContext.Current.Session["isnewdoc"] == null ? false : (bool)HttpContext.Current.Session["isnewdoc"]; } set { HttpContext.Current.Session["isnewdoc"] = value; } }
        public static I_MasterTypeSet DocSet { get { return (I_MasterTypeSet)HttpContext.Current.Session["mastertype_docset"]; } set { HttpContext.Current.Session["mastertype_docset"] = value; } }
        //public static List<MasterTypeLine> DocList { get { return (List<MasterTypeLine>)HttpContext.Current.Session["mastertypeline_list"]; } set { HttpContext.Current.Session["mastertypeline_list"] = value; } }
        public static List<MasterTypeHead> DocList { get { return (List<MasterTypeHead>)HttpContext.Current.Session["mastertypehead_list"]; } set { HttpContext.Current.Session["mastertypehead_list"] = value; } }
        public static I_MasterTypeFiterSet FilterSet { get { return (I_MasterTypeFiterSet)HttpContext.Current.Session["mastertypefilter_set"]; } set { HttpContext.Current.Session["mastertypefilter_set"] = value; } }
        #endregion

        #region Query Transaction
        public static void GetDocSetByID(string masType)
        {

            try
            {
                using (GAEntities db = new GAEntities())
                {
                    NewTransaction();
                    var h = db.MasterTypeHead.Where(o => o.MasterTypeID == masType).FirstOrDefault();
                    //  DocSet.Head = db.MasterTypeHead.Where(o => o.MasterTypeID == docid).FirstOrDefault();
                    DocSet.Line = db.MasterTypeLine.Where(o => o.MasterTypeID == masType).OrderBy(o => o.Sort).ToList();
                    DocSet.LineActive = NewLine(h.UseFor);
                    DocSet.Log = TransactionInfoService.ListLogByDocID(masType);
                }
            }
            catch (Exception ex)
            {
                DocSet.OutputAction.Result = "ok";

                if (ex.InnerException != null)
                {
                    DocSet.OutputAction.Message1 = ex.InnerException.ToString();
                }
                else
                {
                    DocSet.OutputAction.Message1 = ex.Message.ToString();
                }
            }

        }
        public static MasterTypeHead GetMasterTypeHead(string typeId)
        {
            MasterTypeHead result = new MasterTypeHead();

            using (GAEntities db = new GAEntities())
            {
                result = db.MasterTypeHead.Where(o => o.MasterTypeID == typeId).FirstOrDefault();
            }
            return result;
        }
        public static MasterTypeLine GetMasterLinebyValueTXT(string valuetxt)
        {
            MasterTypeLine result = new MasterTypeLine();

            using (GAEntities db = new GAEntities())
            {
                result = db.MasterTypeLine.Where(o => o.ValueTXT == valuetxt).FirstOrDefault();
            }
            return result;
        }

        public static MasterTypeLine GetType(string masterId, string valuetxt)
        {
            MasterTypeLine result = new MasterTypeLine();

            using (GAEntities db = new GAEntities())
            {
                result = db.MasterTypeLine.Where(o => o.MasterTypeID == masterId && o.ValueTXT == valuetxt).FirstOrDefault();
                if (result == null)
                {
                    result = new MasterTypeLine { MasterTypeID = masterId, ValueTXT = "", Description1 = "", Description2 = "", Description3 = "", Description4 = "" };
                }
            }
            return result;
        }
        //public static void ListLine2DocSetLine() {
        //    var h = DocSet.Head;
        //    using (GAEntities db = new GAEntities()) {
        //        DocSet.Line = db.MasterTypeLine.Where(o => o.MasterTypeID == h.MasterTypeID).ToList();
        //    }

        //}
        public static List<MasterTypeLine> ListType(string masId, bool isShowBlank)
        {
            List<MasterTypeLine> result = new List<MasterTypeLine>();
            using (GAEntities db = new GAEntities())
            {
                result = db.MasterTypeLine.Where(o => o.MasterTypeID == masId && o.IsActive).ToList();

                if (isShowBlank)
                {
                    MasterTypeLine n = new MasterTypeLine { MasterTypeID = "", ValueTXT = "", Description1 = "", Description2 = "", Description3 = "", Description4 = "" };
                    result.Insert(0, n);
                }
            }
            return result;
        }

        public static List<MasterTypeLine> ListTypeByParentID(string masId, string ParentID, bool isShowBlank)
        {
            List<MasterTypeLine> result = new List<MasterTypeLine>();
            using (GAEntities db = new GAEntities())
            {
                result = db.MasterTypeLine.Where(o => o.MasterTypeID == masId && o.ParentID == ParentID && o.IsActive).ToList();

                if (isShowBlank)
                {
                    MasterTypeLine n = new MasterTypeLine { MasterTypeID = "", ValueTXT = "", Description1 = "", Description2 = "", Description3 = "", Description4 = "" };
                    result.Insert(0, n);
                }
            }
            return result;
        }


        public static List<MasterTypeHead> ListHeadMastertype()
        {

            List<MasterTypeHead> result = new List<MasterTypeHead>();
            using (GAEntities db = new GAEntities())
            {
                result = db.MasterTypeHead.Where(o => o.IsActive).ToList();
            }
            return result;
        }

        public static void ListDoc(string docid)
        {
            using (GAEntities db = new GAEntities())
            {
                DocSet.Line = db.MasterTypeLine.Where(o => (
                                                      o.MasterTypeID == docid)
                                                      && o.IsActive).OrderBy(o => o.Sort).ToList();
            }
        }

        public static void ListDoc()
        {
            string search = FilterSet.SearchText;
            using (GAEntities db = new GAEntities())
            {
                DocList = db.MasterTypeHead.Where(o => (
                                                      o.MasterTypeID.Contains(search)
                                                      || o.Name.Contains(search)

                                                      || search == ""

                                                     )
                                                     && o.IsActive
                                                     && o.UserAddNew == true
                                                     ).OrderByDescending(o => o.CreatedDate).ToList();
            }
        }

        #endregion

        #region Save
        public static I_BasicResult Save(MasterTypeHead head)
        {

            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };

            var a = DocSet.LineActive;
            try
            {
                using (GAEntities db = new GAEntities())
                {

                    var uh = db.MasterTypeLine.Where(o => o.ValueTXT == a.ValueTXT && o.MasterTypeID == a.MasterTypeID && o.RComID == a.RComID).FirstOrDefault();
                    if (uh == null)
                    {
                        db.MasterTypeLine.Add(a);
                        db.SaveChanges();
                        TransactionInfoService.SaveLog(new TransactionLog { TransactionID = head.MasterTypeID, TableID = "MASTERTYPELINE", ParentID = "", TransactionDate = DateTime.Now, CompanyID = "", Action = "Insert master : " + a.ValueTXT });
                    }
                    else
                    {
                        uh.ValueTXT = a.ValueTXT;
                        uh.RComID = a.RComID;
                        uh.Description1 = a.Description1;
                        uh.Description2 = a.Description2;
                        uh.Description3 = a.Description3;
                        uh.Sort = a.Sort;
                        uh.RefID = a.RefID;
                        uh.IsActive = a.IsActive;
                        db.SaveChanges();
                        TransactionInfoService.SaveLog(new TransactionLog { TransactionID = head.MasterTypeID, TableID = "MASTERTYPELINE", ParentID = "", TransactionDate = DateTime.Now, CompanyID = "", Action = "Update master : " + a.ValueTXT });



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






        #endregion

        #region Newtransaction

        public static void NewTransaction()
        {
            DocSet = new I_MasterTypeSet();
            DocSet.LineActive = NewLine("ALL");
            DocSet.Line = new List<MasterTypeLine>();
            DocSet.Log = new List<TransactionLog>();

            DocSet.OutputAction = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
        }



        public static MasterTypeLine NewLine(string useFor)
        {
            MasterTypeLine n = new MasterTypeLine();

            n.MasterTypeID = "";
            if (useFor.ToUpper() == "ALL")
            {
                n.RComID = "";
            }
            else
            {
                n.RComID = LoginService.LoginInfo.CurrentRootCompany.CompanyID;
            }
            n.ValueTXT = "";
            n.ValueNUM = 0;
            n.Description1 = "";
            n.Description2 = "";
            n.Description3 = "";
            n.Description4 = "NEW";
            n.ParentID = "";
            n.ParentValue = "";
            n.Sort = GenSort();
            n.RefID = "";
            n.IsSysData = false;
            n.RefID = "";
            n.IsActive = true;
            return n;
        }
        public static int GenSort()
        {
            int result = 1;
            try
            {
                var max_linenum = DocSet.Line.Max(o => o.Sort);
                result = max_linenum + 1;
            }
            catch { }
            return result;
        }
        public static void AddLineItem(string checkExistItem, string usefor)
        {
            DocSet.Line.RemoveAll(o => o.Description4 == "NEW");
            DocSet.LineActive = DocSet.Line.Where(o => o.ValueTXT == checkExistItem).FirstOrDefault();
            if (DocSet.LineActive == null)
            {//new line
                DocSet.Line.Add(NewLine(usefor));
                DocSet.LineActive = DocSet.Line.Where(o => o.Description4 == "NEW").OrderByDescending(o => o.Sort).FirstOrDefault();
            }

        }

        public static void NewFilterSet()
        {
            FilterSet = new I_MasterTypeFiterSet();
            FilterSet.SearchBy = "";
            FilterSet.SearchText = "";
            FilterSet.ShowActive = true;
        }


        #endregion


    }
}