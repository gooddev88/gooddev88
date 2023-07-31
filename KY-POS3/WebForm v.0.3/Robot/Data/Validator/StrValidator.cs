using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Robot.Data.Validator {
    public static class StrValidator {
        public static bool IsEnglishAndNumber(string inputstring) {
          
        
            bool result = Regex.IsMatch(inputstring, "^[0-9a-zA-Z|]+$");
            return result;
        }
    }
} 