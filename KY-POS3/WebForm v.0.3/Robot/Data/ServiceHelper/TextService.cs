using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Robot.Data.ServiceHelper {
 
    public static class TextService {
        public static string SemiStringFromListString(List<String> data) {
            String result = "";
#pragma warning disable CS0219 // The variable 'i' is assigned but its value is never used
            int i = 0;
#pragma warning restore CS0219 // The variable 'i' is assigned but its value is never used
            foreach (var d in data) {
                result = result + d + ",";
            }
            if (result != "") {
                result = result.Remove(result.Length - 1);
            }
            return result;
        }
    }
}