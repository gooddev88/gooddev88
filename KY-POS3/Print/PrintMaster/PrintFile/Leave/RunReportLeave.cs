using Newtonsoft.Json;
using PrintMaster.Data.PrintDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
 

namespace PrintMaster.PrintFile.Leave
{
    public class RunReportLeave
    {
        public static LEAVE101 OpenReport(PrintData print_row,string typeprint) {
            var doc = JsonConvert.DeserializeObject<List<sp_leave_report_by_person>>(print_row.JsonData);

            LEAVE101 report0 = new LEAVE101(doc);
            report0.CreateDocument();
            return report0;
        }

        public class sp_leave_report_by_person
        {
            public string Username { get; set; }
            public string FullName { get; set; }
            public string DepartmentID { get; set; }
            public string SubDepartmentID { get; set; }
            public string PositionID { get; set; }
            public string JobLevel { get; set; }
            public string JobType { get; set; }
            public string UserType { get; set; }
            public int WorkYear { get; set; }
            public int WorkMonth { get; set; }
            public string DefaultCompany { get; set; }
            public string l_sick { get; set; }
            public string l_business { get; set; }
            public string l_vacation { get; set; }
            public string l_late { get; set; }
            public string l_absent { get; set; }
            public string l_sick_certificate { get; set; }
            public string l_maternity { get; set; }
            public string l_ordination { get; set; }
            public string remark { get; set; }
        }
    }
}