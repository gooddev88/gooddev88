using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Robot.Data.DataLayer {
    public class DLDocLine {
        public int ID { get; set; }
        public int LineNum { get; set; }
        public string Key { get; set; }
        public string Description { get; set; }
        public int Index { get; set; }
        public string Type { get; set; }

        public bool CurrentIndex { get; set; }
        public bool IsFirst { get; set; }
        public bool IsLast { get; set; }
    }
}