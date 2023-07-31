using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static Robot.Data.BL.I_Result;

namespace Robot.Data.DataAccess {
    public static class TransactionInfoService {
        #region Class

        #endregion


        #region Method GET
    
        public static List<TransactionLog> ListLogByDocID(string ID) {
            List<TransactionLog> result = new List<TransactionLog>();
            using (GAEntities db = new GAEntities()) {
                result = db.TransactionLog.Where(o => (o.TransactionID == ID )).ToList();
            }
            return result;
        }

        public static List<TransactionLog> ListLogByDocID(string ID,string comID, string table)
        {
            List<TransactionLog> result = new List<TransactionLog>();
            var rcom = LoginService.LoginInfo.CurrentRootCompany.CompanyID;
            using (GAEntities db = new GAEntities())
            {
                result = db.TransactionLog.Where(o => (o.TransactionID == ID && o.RCompanyID==rcom && o.CompanyID==comID && o.TableID.ToLower() == table.ToLower())).ToList();
            }
            return result;
        }
        public static List<TransactionLog> ListLogByDocID(string docId, string com)
        {
            List<TransactionLog> result = new List<TransactionLog>();
            var rcom = LoginService.LoginInfo.CurrentRootCompany.CompanyID;
            using (GAEntities db = new GAEntities())
            {
                result = db.TransactionLog.Where(o => (o.TransactionID == docId && o.RCompanyID == rcom && o.CompanyID == com)).ToList();
            }
            return result;
        }
        #endregion

        #region Create
        public static List<string> SaveLog(TransactionLog data) {
            List<string> result = new List<string>();
            var rcom = LoginService.LoginInfo.CurrentRootCompany.CompanyID;
            result.Add("");
            result.Add("");
       
            
            if (data == null) { 
                result[0]="R0";//RESULT ACTION R0=ERROR:R1=SUCCESS
                result[1] = "No data insert"; ;
            }
            try {
                data.ActionType= data.ActionType==null?"" : data.ActionType;
                data.RCompanyID = rcom;
                data.CreatedBy = LoginService.LoginInfo.CurrentUser;
                data.CreatedDate = DateTime.Now;
                data.ChangeValue = data.ChangeValue == null ? "" : data.ChangeValue;
                data.CompanyID = data.CompanyID == null ? "" : data.CompanyID;
                data.ParentID = data.ParentID == null ? "" : data.ParentID;             
                data.IsActive = true;
                using (GAEntities db = new GAEntities()) {
                    db.TransactionLog.Add(data);
                var r=    db.SaveChanges(); 
                    result[0] = "R1";//RESULT ACTION R0=ERROR:R1=SUCCESS
                    result[1] = ""; ;
                }
            } catch (Exception ex) {
                result[0] = "R0";//RESULT ACTION R0=ERROR:R1=SUCCESS
                result[1] = ex.Message;

            }

            return result;
        }


        public static I_BasicResult SaveLogV2(TransactionLog data)
        {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            //if (data.CreatedBy== null) {
            //    data.create
            //    //result.Result = "fail";
            //    //result.Message1 = "No created by Value";
            //    //return result;
            //}

            try
            {
                data.TableID = data.TableID == null ? "Unknown table" : data.TableID;
                data.CreatedBy = data.CreatedBy == null ? "" : data.CreatedBy;
                data.CreatedDate = DateTime.Now;
                data.ChangeValue = data.ChangeValue == null ? "" : data.ChangeValue;
                data.CompanyID = data.CompanyID == null ? "" : data.CompanyID;
                data.ParentID = data.ParentID == null ? "" : data.ParentID;
                data.Action = data.Action == null ? "" : data.Action;
                data.ActionType = data.ActionType == null ? "" : data.ActionType;
                data.IsActive = true;
                using (GAEntities db = new GAEntities())
                {
                    db.TransactionLog.Add(data);
                    var r = db.SaveChanges();
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
    }
}