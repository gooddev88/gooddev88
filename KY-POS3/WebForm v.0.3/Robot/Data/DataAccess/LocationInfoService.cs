using Robot.Master.DA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Robot.Data.DataAccess {
    public static class LocationInfoService {
        #region Class
        public class SelectListLocation {
            public string LocID { get; set; }
            public string LocCode { get; set; }
            public string CompanyID { get; set; }

            public string LocName { get; set; }
            public string LocTypeID { get; set; }
            public string LocTypeName { get; set; }
            public string ParentID { get; set; }

        }
        #endregion


        #region  Method GET
        //public static List<SelectListLocation> MiniSelectList(string type, string company) {

        //    List<SelectListLocation> result = new List<SelectListLocation>();
        //    var rcom = LoginService.LoginInfo.CurrentRootCompany;
        //    using (GAEntities db = new GAEntities()) {
        //        var query = db.vw_LocationInfo.Where(o => (o.LocTypeID == type || type == "")
        //                                         &&    (o.CompanyID == company || company == "")

        //                                         && o.IsActive
        //                                        ).ToList();
        //        foreach (var q in query) {
        //            SelectListLocation n = new SelectListLocation();
        //            n.LocID = q.LocID;
        //            n.LocCode = q.LocCode;
        //            n.LocName = q.LocName + " (" + q.LocID + ")"; 
        //            n.LocTypeID = q.LocTypeID;
        //            n.LocTypeName = q.LocTypeName;
        //            n.CompanyID = q.CompanyID;
        //            n.ParentID = q.ParentID;
        //            result.Add(n);
        //        }
        //    }
        //    return result;
        //}

        public static List<LocationInfo> ListStockBalLocation(string type, string comId, bool isShowEmpty) {
            List<LocationInfo> result = new List<LocationInfo>();
            var rcom = LoginService.LoginInfo.CurrentRootCompany.CompanyID;
            using (GAEntities db = new GAEntities()) {
                var comInf = db.CompanyInfo.Where(o => o.RCompanyID == rcom && o.CompanyID == comId).FirstOrDefault();
                if (comInf != null)
                {
                    if (comInf.IsWH == true)
                    {
                        result = db.LocationInfo.Where(o =>
                                                      (o.LocTypeID == type || type == "")
                                                       && (o.CompanyID == comId || comId == "")
                                                       && o.RCompany == rcom
                                                       && o.IsActive
                                                  ).ToList();
                    }
                    else
                    {
                        result = db.LocationInfo.Where(o =>
                                                  (o.LocTypeID == type || type == "")
                                                   && (o.CompanyID == comId || comId == "")
                                                   && o.LocID == "STORE"
                                                   && o.RCompany == rcom
                                                   && o.IsActive
                                              ).ToList();
                        isShowEmpty = false;
                    }
                }
                else
                {
                    result = db.LocationInfo.Where(o =>
                              (o.LocTypeID == type || type == "")
                               && (o.CompanyID == comId || comId == "")
                               && o.RCompany == rcom
                               && o.IsActive
                          ).ToList();
                }


                //foreach (var q in result)
                //{
                //    SelectListLocation n = new SelectListLocation();
                //    n.LocID = q.LocID;
                //    n.LocName = q.LocName;
                //    n.LocTypeID = q.LocTypeID;
                //    n.CompanyID = q.CompanyID;
                //    n.LocCode = q.LocCode;
                //    result.Add(n);
                //}
                if (isShowEmpty) {
                    LocationInfo blank = new LocationInfo { LocID = "X", CompanyID = "", Name = "ไม่ระบุที่เก็บ", LocTypeID = "", LocCode = "ไม่ระบุที่เก็บ", ParentID = "" };
                    result.Insert(0, blank);
                }

            }
            return result;
        }
        public static List<LocationInfo> ListStockLocation(string type, string comId, bool isShowEmpty)
        {
            List<LocationInfo> result = new List<LocationInfo>();
            var rcom = LoginService.LoginInfo.CurrentRootCompany.CompanyID;
            using (GAEntities db = new GAEntities())
            {
           
                    result = db.LocationInfo.Where(o =>
                                                  (o.LocTypeID == type || type == "")
                                                   && (o.CompanyID == comId || comId == "")
                                                   && o.RCompany == rcom
                                                   && o.IsActive
                                              ).ToList();
            
                
                //foreach (var q in result)
                //{
                //    SelectListLocation n = new SelectListLocation();
                //    n.LocID = q.LocID;
                //    n.LocName = q.LocName;
                //    n.LocTypeID = q.LocTypeID;
                //    n.CompanyID = q.CompanyID;
                //    n.LocCode = q.LocCode;
                //    result.Add(n);
                //}
                if (isShowEmpty)
                {
                    LocationInfo blank = new LocationInfo { LocID = "X", CompanyID = "", Name = "ไม่ระบุที่เก็บ", LocTypeID = "", LocCode = "ไม่ระบุที่เก็บ", ParentID = "" };
                    result.Insert(0, blank);
                }

            }
            return result;
        }
        public static List<SelectListLocation> StockLoc(string type, string company, bool isShowEmpty) {
            List<SelectListLocation> result = new List<SelectListLocation>();
            using (GAEntities db = new GAEntities()) {
                var query = db.vw_LocationInfo.Where(o =>
                                                    (o.LocTypeID == type || type == "")
                                                     && (o.CompanyID == company || company == "")

                                                     && o.IsActive
                                                ).ToList();
                foreach (var q in query) {
                    SelectListLocation n = new SelectListLocation();
                    n.LocID = q.LocID;
                    n.LocName = q.LocName;
                    n.LocTypeID = q.LocTypeID;
                    n.CompanyID = q.CompanyID;
                    n.LocCode = q.LocCode;
                    result.Add(n);
                }
                if (isShowEmpty) {
                    SelectListLocation blank = new SelectListLocation { LocID = "X", CompanyID = "", LocName = "ไม่ระบุที่เก็บ", LocTypeName = "", LocCode = "ไม่ระบุที่เก็บ", ParentID = "" };
                    result.Insert(0, blank);
                }

            }
            return result;
        }
        public static LocationInfo GetDataByLocID(string locID) {
            LocationInfo result = new LocationInfo();
            using (GAEntities db = new GAEntities()) {
                result = db.LocationInfo.Where(o => o.LocID == locID).FirstOrDefault();
            }
            return result;
        }
        public static vw_LocationInfo GetDataViewByLocID(string locID) {

            vw_LocationInfo result = new vw_LocationInfo();
            using (GAEntities db = new GAEntities()) {
                result = db.vw_LocationInfo.Where(o => o.LocID == locID).FirstOrDefault();
            }
            return result;
        }
        public static List<vw_LocationInfo> ListDataView(string companyID) {
            //var comlist = LoginService.LoginInfo.UserInCompany;
            List<vw_LocationInfo> result = new List<vw_LocationInfo>();
            using (GAEntities db = new GAEntities()) {
                result = db.vw_LocationInfo.Where(o => o.CompanyID == companyID && o.IsActive == true).ToList();
            }
            return result;
        }
        public static List<LocationInfo> ListLocByComID(string companyID) {
            //var comlist = LoginService.LoginInfo.UserInCompany;
            List<LocationInfo> result = new List<LocationInfo>();
            using (GAEntities db = new GAEntities()) {
                result = db.LocationInfo.Where(o => o.CompanyID == companyID && o.IsActive==true).ToList();
            }
            return result;
        }
        public static List<vw_LocationInfo> ListDataViewByMainLocation(string company_id) {
         
            List<vw_LocationInfo> result = new List<vw_LocationInfo>();
            using (GAEntities db = new GAEntities()) {
   result = db.vw_LocationInfo.Where(o => o.CompanyID == company_id && o.LocTypeID == "MAIN" && o.IsActive == true).ToList();

                
            }
            return result;
        }
        public static List<vw_LocationInfo> ListViewSearch(string search, bool showInActive) {
            List<vw_LocationInfo> result = new List<vw_LocationInfo>();
            using (GAEntities db = new GAEntities()) {
                result = db.vw_LocationInfo.Where(o => (o.LocID.Contains(search)
                                            || o.CompanyID.Contains(search)
                                            || o.LocCode.Contains(search)
                                            || o.LocTypeID.Contains(search)
                                            || o.ParentID.Contains(search)
                                            || o.LocName.Contains(search)
                                            || search == "") && (o.IsActive || showInActive)
                                            ).OrderBy(o => o.LocID).ToList();
            }
            return result;
        }

        #endregion

        #region  Save
        public static List<string> Save(LocationInfo loca, string action) {
            DateTime mydate = DateTime.Now;
            string myuser = LoginService.LoginInfo.CurrentUser; 
            List<string> result = new List<string>();

            result.Add("");//RESULT ACTION R0=ERROR:R1=SUCCESS
            result.Add("");

            try {
                using (GAEntities db = new GAEntities()) {
                    if (action == "insert") {
                        loca.CreatedBy = myuser;
                        loca.CreatedDate = mydate;
                        db.LocationInfo.Add(loca);
                        db.SaveChanges();
                        TransactionInfoService.SaveLog(new TransactionLog { TransactionID = loca.LocID, ParentID = "", TransactionDate = DateTime.Now, CompanyID = loca.CompanyID, Action = "INSERT DATA" });

                        result[0] = "R1";//RESULT ACTION R0=ERROR:R1=SUCCESS
                        result[1] = "";
                    }

                    if (action == "update") {
                        var l = db.LocationInfo.Where(o => o.LocID == loca.LocID).FirstOrDefault();
                        l.LocTypeID = loca.LocTypeID;
                        l.ParentID = loca.ParentID;
                        l.Name = loca.Name;
                        l.Remark = loca.Remark;
                        l.CreatedBy = loca.CreatedBy;
                        l.CreatedDate = loca.CreatedDate;
                        l.ModifiedBy = myuser;
                        l.ModifiedDate = mydate;
                        l.IsActive = loca.IsActive;

                        db.SaveChanges();
                        TransactionInfoService.SaveLog(new TransactionLog { TransactionID = loca.LocID, ParentID = "", TransactionDate = DateTime.Now, CompanyID = loca.CompanyID, Action = "UPDATE DATA" });

                        result[0] = "R1";//RESULT ACTION R0=ERROR:R1=SUCCESS
                        result[1] = "";
                    }

                }

                return result;
            } catch (Exception ex) {
                result[0] = "R0";//RESULT ACTION R0=ERROR:R1=SUCCESS
                result[1] = ex.Message;
            }
            return result;

        }



        public static List<string> CreateDefualtLocation(string comID) {

            string myuser = LoginService.LoginInfo.CurrentUser;
            DateTime mydate = DateTime.Now;

            List<string> result = new List<string>();
            result.Add("");//RESULT ACTION R0=ERROR:R1=SUCCESS
            result.Add("");
            try {
                var com = CompanyService.GetCompanyInfo(comID);
                if (com.TypeID!= "BRANCH") {
                    result[0] = "R0";//RESULT ACTION R0=ERROR:R1=SUCCESS
                    result[1] = "Organize type is not  of branch..";
                    return result;
                }
                var loctype = MasterTypeService.ListType("LOC TYPE", false);
                List <LocationInfo> loc_list = new List<LocationInfo>();
                using (GAEntities db = new GAEntities()) {

                    foreach (var a in loctype) {
                        LocationInfo lc = new LocationInfo();
                        var chkExitType = db.LocationInfo.Where(o => o.LocTypeID == a.ValueTXT && o.CompanyID == comID && o.IsActive).FirstOrDefault();
                        if (chkExitType != null) {
                            continue;
                        } 
                        string locid = ""; 
                        if (a.ValueTXT=="MAIN") { 
                            locid = comID;
                        } else {
                            locid = comID + "-" + a.ValueTXT;
                        }
                          

                        lc.LocID = locid;
                        lc.CompanyID = com.CompanyID;
                        lc.LocCode = locid;
                        lc.LocTypeID = a.ValueTXT;
                        lc.ParentID = "";
                        lc.Name = com.Name1;
                        lc.Remark = "";
                        lc.CreatedBy = myuser;
                        lc.CreatedDate = mydate;
                        lc.ModifiedBy = "";
                        lc.ModifiedDate = null;
                        lc.IsActive = com.IsActive; 
                        loc_list.Add(lc);
                    }
                    db.LocationInfo.AddRange(loc_list);
                    db.SaveChanges();
                    result[0] = "R1";//RESULT ACTION R0=ERROR:R1=SUCCESS
                    result[1] = "";
                }
                return result;
            } catch (Exception ex) {
                result[0] = "R0";//RESULT ACTION R0=ERROR:R1=SUCCESS
                result[1] = ex.Message;
            }
            return result;
        }
        #endregion

        #region Delete
        public static List<string> Delete(string id) {
            List<string> result = new List<string>();
            result.Add("");//RESULT ACTION R0=ERROR:R1=SUCCESS
            result.Add("");

            try {
                using (GAEntities db = new GAEntities()) {
                    DateTime modytime = DateTime.Now;
                    string modyby = LoginService.LoginInfo.CurrentUser;

                    var v = db.LocationInfo.Where(o => o.LocID == id).FirstOrDefault();
                    if (v.LocTypeID=="MAIN") {
                        result[0] = "R0";//RESULT ACTION R0=ERROR:R1=SUCCESS
                        result[1] = "Cannot delete MAIN Location..";
                        return result;
                    }

                    v.ModifiedBy = modyby;
                    v.ModifiedDate = modytime;

                    v.IsActive = false;

                    db.SaveChanges();
                    TransactionInfoService.SaveLog(new TransactionLog { TransactionID = v.LocID, ParentID = "", TransactionDate = DateTime.Now, CompanyID = v.CompanyID, Action = "DELETE DATA" });

                    result[0] = "R1";//RESULT ACTION R0=ERROR:R1=SUCCESS
                    result[1] = "Delete Successfull";

                }

                return result;
            } catch (Exception ex) {
                result[0] = "R0";//RESULT ACTION R0=ERROR:R1=SUCCESS
                result[1] = ex.Message;
            }
            return result;

        }

        public static List<string> DeleteByComID(string com_id) {
            List<string> result = new List<string>();
            string myuser = LoginService.LoginInfo.CurrentUser;
            DateTime mydate = DateTime.Now;
            result.Add("");//RESULT ACTION R0=ERROR:R1=SUCCESS
            result.Add("");
            try {
                using (GAEntities db = new GAEntities()) {
                    var loc = db.LocationInfo.Where(o => o.CompanyID == com_id).ToList();
                    foreach (var c in loc) {
                        c.ModifiedBy = myuser;
                        c.ModifiedDate = mydate;
                        c.IsActive = false;
                    
                    } 
                    db.SaveChanges(); 
                    result[0] = "R1";//RESULT ACTION R0=ERROR:R1=SUCCESS
                    result[1] = ""; 
                }
                return result;
            } catch (Exception ex) {

                result[0] = "R0";//RESULT ACTION R0=ERROR:R1=SUCCESS
                result[1] = ex.Message;
            }
            return result;
        }
        #endregion
    }
}