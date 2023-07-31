using Newtonsoft.Json;
using System.Collections.Generic;
using static Robot.PrintOut.CreatePrintData.SalePrintConverter;

namespace Robot.PrintFile.KyPos.R402 {
    public class RunReport
    {

        public static R402 OpenReport(PrintData print_row, string typeprint) {
            var doc = JsonConvert.DeserializeObject<I_POSSaleSetX>(print_row.JsonData);
            List<int> print_list = new List<int> { 0, 1 };

            R402 report0 = new R402(doc );
            report0.CreateDocument();  
            return report0;
        }
    }
}
