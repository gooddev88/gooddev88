
using Dapper;
using Robot.Data.DA;
using Robot.Data.DA.Leave;
using Robot.Data.DA.Master;
using Robot.Data.GADB.TT;
using Robot.Service.FileGo;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using static Robot.Data.DA.Leave.HRLeaveReqService;
using static Robot.Data.ML.I_Result;

namespace Robot.PrintOut.Leave {
    public class RunReportLeave {

        public static List<sp_leave_report_by_person> ListDataCountLeave(int year, string DepartmentID, string SubDepartmentID) {
            List<sp_leave_report_by_person> result = new List<sp_leave_report_by_person>();
            try {
                string conStr = Globals.GAEntitiesConn;

                string strSQL = "";
                strSQL = VQueryService.GetCommand("sp_leave_report_by_person");

                using (var conn = new SqlConnection(conStr)) {
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("DepartmentID", DepartmentID);
                    dynamicParameters.Add("SubDepartmentID", SubDepartmentID);
                    dynamicParameters.Add("WorkYear", year);
                    result = conn.Query<sp_leave_report_by_person>(strSQL, dynamicParameters).ToList();
                    return result;
                }
            } catch (Exception e) {

            }
            return result;
        }

        async public static Task<I_BasicResult> Convert2PrintData(string printid,int year,string DepartmentID,string SubDepartmentID) {
            DepartmentID = DepartmentID == null ? "" : DepartmentID;
            SubDepartmentID = SubDepartmentID == null ? "" : SubDepartmentID;
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {

                var line = ListDataCountLeave(year,DepartmentID, SubDepartmentID);
                
                PrintData n = new PrintData();
                n.AppID = "LEAVE";
                n.FormPrintID = printid;
                JsonSerializerOptions jso = new JsonSerializerOptions();
                jso.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
                string json = JsonSerializer.Serialize(line, jso);
                n.JsonData = json;
                r = await Task.Run(() => PrintService.CreatePrintDataApi(n));

            } catch (Exception ex) {
                r.Result = "fail";
                r.Message1 = ex.Message;
            }

            return r;
        }

        public class sp_leave_report_by_person {
            public string Username { get; set; }
            public string FullName { get; set; }
            public string DepartmentID { get; set; }
            public string SubDepartmentID { get; set; }
            public string PositionID { get; set; }
            public string JobLevel { get; set; }
            public string JobType { get; set; }
            public string UserType { get; set; }
            public string DefaultCompany { get; set; }
            public string l_sick { get; set; }
            public string l_business { get; set; }
            public string l_vacation { get; set; }
            public string l_late { get; set; }
            public string l_absent { get; set; }
            public string l_sick_certificate { get; set; }
            public string l_maternity { get; set; }
            public string remark { get; set; }
        }

    }
}
