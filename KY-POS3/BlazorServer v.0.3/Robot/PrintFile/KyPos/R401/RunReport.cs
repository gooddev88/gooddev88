using Newtonsoft.Json;
using System.Collections.Generic;
using static Robot.PrintOut.CreatePrintData.SalePrintConverter;

namespace Robot.PrintFile.KyPos.R401 {
    public class RunReport
    {

        public static R401 OpenReport(PrintData print_row ) {
            var doc = JsonConvert.DeserializeObject<I_POSSaleSetX>(print_row.JsonData);
            List<int> print_list = new List<int> { 0, 1 }; 

            R401 report0 = new R401(doc );
            report0.CreateDocument();  
            return report0;
        }   
    }
}
