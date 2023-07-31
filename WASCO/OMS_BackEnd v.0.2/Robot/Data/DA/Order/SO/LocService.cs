using Blazored.SessionStorage;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Robot.Data.DA.Document;
using Robot.Data.DA.Login;
using Robot.Data.DA.Master;
using Robot.Data.GADB.TT;
using Robot.Data.ML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Telerik.SvgIcons;
using static Robot.Data.DA.HR.InventoryService;
using static Robot.Data.ML.I_Result;

namespace Robot.Data.DA.Order.SO {
    public class LocService {
        public static string sessionActiveId = "activecomid";
       

        public class I_LocationInfo_MemoSet {
            public LocationInfo locationInfo { get; set; }
            public List<TransactionLog> Log { get; set; }
            public I_BasicResult OutputAction { get; set; }
            public bool NeedRunNextID { get; set; }
        }
        public class I_LocationInfoSetFiterSet {
            public string Rcom { get; set; }
            public string Com { get; set; }
            public string SearchText { get; set; }
            public string LocID { get; set; }
            public string SelectOption { get; set; }
            public bool ShowActive { get; set; }

        }

     
        public I_LocationInfo_MemoSet DocSet { get; set; }

        ISessionStorageService sessionStorage;

        public LocService(ISessionStorageService _sessionStorage) {
            sessionStorage = _sessionStorage;
        }
        

        #region Query Transaction



        public I_LocationInfo_MemoSet GetDocSet(int id, string rcom,string com) {
            I_LocationInfo_MemoSet n = NewTransaction(rcom,com);
            using (GAEntities db = new GAEntities()) {
                n.locationInfo = db.LocationInfo.Where(o => o.ID == id && o.RCompanyID == rcom && o.CompanyID==com).FirstOrDefault();
                n.Log = db.TransactionLog.Where(o => o.TransactionID ==  ""&& o.RCompanyID==rcom && o.CompanyID==com && o.TableID == "MASERVICE").OrderBy(o => o.CreatedDate).ToList();
            }
            return n;
        }




        public static List<LocationInfo> ListDoc(I_LocationInfoSetFiterSet f) {
            List<LocationInfo> result = new List<LocationInfo>();
            try {
                using (GAEntities db = new GAEntities()) {
                    f.SearchText = f.SearchText.ToLower();

                    result = db.LocationInfo.Where(o =>
                                                        (o.LocID.Contains(f.SearchText)
                                                        || o.LocCode.Contains(f.SearchText)
                                                        || o.Name.Contains(f.SearchText)
                                                        || f.SearchText == "")
                                                          && (o.LocID == f.LocID || f.LocID == "")
                                                        && o.RCompanyID == f.Rcom
                                                        && o.CompanyID == f.Com
                                                        && o.IsActive == f.ShowActive
                                                        ).OrderByDescending(o => o.CreatedDate).ToList();



                }
            } catch (Exception ex) {

                var xx = ex.Message;
            }
      
            return result;
        }
       
        #endregion

        #region Save

        public static I_BasicResult Save(I_LocationInfo_MemoSet doc,bool isnew, LogInService.LoginSet login) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            var rcom = login.CurrentRootCompany.CompanyID;
            var com = login.CurrentCompany.CompanyID;
            var createby = login.CurrentUser;
            var modified_by = login.CurrentUser;
            var modified_date = DateTime.Now.Date;
            try {
                var h = doc.locationInfo;
                using (GAEntities db = new GAEntities()) { 
                    if (isnew) {
                        h.ModifiedBy = modified_by;
                        h.ModifiedDate = modified_date;
                          doc = checkLoc(doc);
                        if (doc.OutputAction.Result == "fail") {
                            result.Result = "fail";
                            result.Message1 = doc.OutputAction.Message1;
                        } else {
                            db.LocationInfo.Add(doc.locationInfo);
                            db.SaveChanges();
                            TransactionService.SaveLog(new TransactionLog { TransactionID = doc.locationInfo.LocID, TableID = "LOCATION SERVICE", ParentID = "", TransactionDate = DateTime.Now, RCompanyID = doc.locationInfo.RCompanyID = rcom, CompanyID = doc.locationInfo.CompanyID, Action = "Insert new ma record" }, h.RCompanyID, h.CompanyID, h.ModifiedBy);


                        }
                    } else {
                        doc = checkLoc(doc);
                        if (doc.OutputAction.Result == "fail") {
                            result.Result = "fail";
                            result.Message1 = doc.OutputAction.Message1;
                            return result;
                        }


                        var n = db.LocationInfo.Where(o => o.ID == h.ID && o.RCompanyID == rcom && o.CompanyID==com).FirstOrDefault();
                       
                        //if (doc.locationInfo.id!= n.id) {
                            n.RCompanyID = doc.locationInfo.RCompanyID;
                            n.CompanyID = doc.locationInfo.CompanyID;
                            n.LocID = doc.locationInfo.LocID;
                            n.LocCode = doc.locationInfo.LocCode;
                            n.LocTypeID = doc.locationInfo.LocTypeID;
                            n.ParentID = doc.locationInfo.ParentID;
                            n.Name = doc.locationInfo.Name;
                            n.Remark = doc.locationInfo.Remark;
                            n.CreatedDate = doc.locationInfo.CreatedDate;
                            n.CreatedBy = login.CurrentUser;
                            n.ModifiedDate = doc.locationInfo.ModifiedDate;
                            n.ModifiedBy = login.CurrentUser;
                            n.IsActive = doc.locationInfo.IsActive;
                        db.SaveChanges();
                        TransactionService.SaveLog(new TransactionLog { TransactionID = doc.locationInfo.LocID, TableID = "LOCATION SERVICE", ParentID = "", TransactionDate = DateTime.Now, RCompanyID = doc.locationInfo.RCompanyID = rcom, CompanyID = doc.locationInfo.CompanyID, Action = "Update new ma record" }, h.RCompanyID, h.CompanyID, h.ModifiedBy);

                      

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


        public static List<LocationInfo> ListLocID(string rcom ,string com,List<string> loc_access) {
            List<LocationInfo> result = new List<LocationInfo>();
            using (GAEntities db = new GAEntities()) {
                result = db.LocationInfo.Where(o => (o.RCompanyID == rcom  )
                 && (o.CompanyID == com )
                 && o.IsActive == true
                 && loc_access.Contains(o.LocID)
                 ).ToList();

                  result.Insert(0, new LocationInfo { LocID = "", Name = "" });
            }
            return result;
        }

        public static I_LocationInfo_MemoSet checkLoc(I_LocationInfo_MemoSet input) {

            try {
                input.OutputAction = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
                using (GAEntities db = new GAEntities()) {
                    var h = input.locationInfo;

                    var get_id = db.LocationInfo.Where(o => o.ID != h.ID && o.RCompanyID == h.RCompanyID && o.CompanyID == h.CompanyID && o.LocID == h.LocID && o.IsActive == true).FirstOrDefault();
                    if (get_id != null) {
                        input.OutputAction = new I_BasicResult { Result = "fail", Message1 = "  รหัสคลัง" + h.LocID + "ซ้ำ", Message2 = "" };
                    }



                }
            } catch (Exception ex) {
                input.OutputAction = new I_BasicResult { Result = "fail", Message1 = ex.Message, Message2 = "" };
            }
            return input;
        }
        public static I_LocationInfo_MemoSet checkDupID(I_LocationInfo_MemoSet input) {
            try {
                input.OutputAction = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
                using (GAEntities db = new GAEntities()) {
                    var h = input.locationInfo;

                    var get_id = db.LocationInfo.Where(o => o.LocID == h.LocID && o.RCompanyID == h.RCompanyID && o.CompanyID==h.CompanyID).FirstOrDefault();
                 
                        while (get_id != null) { // is dup 
                        IDRuunerService.GetNewIDV2("PASS", h.RCompanyID, "", h.CreatedDate, true, "th");
                           h.LocID= IDRuunerService.GetNewIDV2("PASS", h.RCompanyID, "", h.CreatedDate, false, "th")[1];
                        get_id = db.LocationInfo.Where(o => o.LocID == h.LocID && o.RCompanyID == h.RCompanyID && o.CompanyID == h.CompanyID).FirstOrDefault();
                    } 
                } 
            } catch (Exception ex) {
                input.OutputAction = new I_BasicResult { Result = "fail", Message1 = ex.Message, Message2 = "" };
            }
            return input;
        }
        #endregion


        #region Delete


        public static I_BasicResult Delete(int id , string rcom, string comid, string user) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                using (GAEntities db = new GAEntities()) {
                    var com = db.LocationInfo.Where(o => o.ID==id && o.RCompanyID == rcom && o.CompanyID == comid  ).FirstOrDefault();
                    com.ModifiedBy = user;
                    com.ModifiedDate = DateTime.Now;
                    com.IsActive = false; 
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

        

        #region New transaction

        public I_LocationInfo_MemoSet NewTransaction(string rcom, string com) {
            I_LocationInfo_MemoSet n = new I_LocationInfo_MemoSet();
           
            n.locationInfo = NewHead(rcom,com);
            n.Log = new List<TransactionLog>();
            n.NeedRunNextID = false;

            return n;
        }

        public static LocationInfo NewHead(string rcom,string com) {
            LocationInfo n = new LocationInfo();

            n.RCompanyID = rcom;
            n.CompanyID = com;
            n.LocID = "";
            n.LocCode = "";
            n.LocTypeID = "";
            n.ParentID = "";
            n.Name = "";
            n.Remark = "";
            n.CreatedDate = DateTime.Now;
            n.CreatedBy = "";
            n.ModifiedDate = DateTime.Now;
            n.ModifiedBy = "";
            n.IsActive = true;
            return n;
        }



        #endregion

        public static I_LocationInfoSetFiterSet NewFilterSet() {
            I_LocationInfoSetFiterSet n = new I_LocationInfoSetFiterSet();

            n.Rcom = "";
            n.Com = "";
            n.LocID = "";
            n.SelectOption = "";
            n.SearchText = "";
            n.ShowActive = true;

            return n;
        }

        async public Task<I_LocationInfoSetFiterSet> GetSessionLocationInfoFiterSet() {
            I_LocationInfoSetFiterSet result = NewFilterSet();
            var strdoc = await sessionStorage.GetItemAsync<string>("LocationInfo_Fiter");
            if (strdoc != null) {
                return JsonConvert.DeserializeObject<I_LocationInfoSetFiterSet>(strdoc);
            } else {
                return null;
            }

        }

        async public void SetSessionLocationInfoFiterSet(I_LocationInfoSetFiterSet data) {
            JsonSerializerOptions jso = new JsonSerializerOptions();
            jso.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
            string json = System.Text.Json.JsonSerializer.Serialize(data, jso);
            await sessionStorage.SetItemAsync("LocationInfo_Fiter", json);
        }


    }
}
