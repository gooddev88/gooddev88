using Robot.Data.GADB.TT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Robot.Data.DA {
    public class TempFileService {

    

        public static TempFilePrinter CreateTempFile(TempFilePrinter input) {
            TempFilePrinter n = new TempFilePrinter();
            try {
                using (GAEntities db = new GAEntities()) {
                    n.FileUrl = input.FileUrl;
                    n.AppPrinterUrl = "https://kyprint/slip/";
                    n.CreatedDate = DateTime.Now;
                     db.TempFilePrinter.Add(n);
                    db.SaveChanges();
                    n.AppPrinterUrl = $"https://kyprint/slip/{n.ID}";
                    db.TempFilePrinter.Update(n);
                    db.SaveChanges();
                }
                } catch (Exception) {

                throw;
            }

            return n;
        }
        public static TempFilePrinter GetTempFIleByID(int id) {
            TempFilePrinter n = new TempFilePrinter();
         
                using (GAEntities db = new GAEntities()) {
                      n = db.TempFilePrinter.Where(o => o.ID == id).FirstOrDefault();
                  
                }
       
            return n;
        }
    }
}
