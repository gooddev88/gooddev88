
using Dapper;
using PrintMaster.Data.PrintDB;
using PrintMaster.Service.Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace PrintMaster.Data.DA {
    public class PrintService {
        public static I_BasicResult CreatePrintData(PrintData data) {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                using (PrintEntities db = new PrintEntities()) { 
                    data.PrintID = Guid.NewGuid().ToString();
                    data.PrintDate = DateTime.Now;
                    db.PrintDatas.Add(data);
                    db.SaveChanges();
                  
                    r.Message2 = data.PrintID;
                  var rr=  ClearOldPrintData();
                   // db.PrintDatas.RemoveRange()
                }
            } catch (Exception ex) {
                r.Result = "fail";
                r.Message1 = ex.Message;
            }

            return r;
        }



        public static int ClearOldPrintData() {
            int r = 0;
            try {
                string conStr = DBDapperService. GetDBConnectFromAppConfig();
                using (var conn = new SqlConnection(conStr)) {
                    var dynamicParameters = new DynamicParameters();
                    DateTime today = DateTime.Now.Date.AddDays(-2);
                    dynamicParameters.Add("today", today);
                    string strSQL = "  delete from PrintData where PrintDate<@today";
                      r = conn.Execute(strSQL, dynamicParameters);
                
                }
            } catch (Exception e) {
                r = 0;
            }
            return r;
        }

    }
}