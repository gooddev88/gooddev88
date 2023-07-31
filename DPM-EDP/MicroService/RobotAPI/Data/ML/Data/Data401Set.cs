using System;
using System.Collections.Generic;

namespace RobotAPI.Data.ML.Data {
    public class Data401Set {
        //สถิติการลา

        public class DocSet {
            public int status { get; set; }
            public object message { get; set; }
            public List<DataRow> rows { get; set; }
        }
        public class DataRow {
            public string Username { get; set; }
            public string FullName { get; set; }
            public string DepartmentID { get; set; }
            public string SubDepartmentID { get; set; }
            public string PositionID { get; set; }
            public string JobLevel { get; set; }
            public string JobType { get; set; }
            public string UserType { get; set; }
            public int WorkYear { get; set; }
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
