﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PrintMaster.Data.DA {
    public class I_ComplexResult {
        public I_BasicResult result { get; set; }
        public List<I_MatchDocument> MatchDocument { get; set; }

    }
    public class I_MatchDocument {
        public string NumberType { get; set; }
        public string InNumber { get; set; }
        public string OutNumber { get; set; }
    }

    public class I_BasicResult {
        public string Result { get; set; }
        public string Message1 { get; set; }
        public string Message2 { get; set; }
    }
}