using Robot.Data.ML;
using Robot.Data.GADB.TT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Robot.Data.DA
{
    public class VendorService
    {

        public static VendorInfo GetVendorInfo(string rcom,string vendorId) {
            VendorInfo ven = new VendorInfo();
            using (GAEntities db = new GAEntities()) {
                ven = db.VendorInfo.Where(o => o.RCompanyID==rcom && o.VendorID==vendorId).FirstOrDefault();
            }
            return ven;
        }

        public static List<VendorInfo> ListVendorInfo(string rcom)
        {
            List<VendorInfo> ven = new List<VendorInfo>();
            using (GAEntities db = new GAEntities())
            {
                ven = db.VendorInfo.Where(o => o.RCompanyID == rcom).ToList();
            }
            return ven;
        }


        public static List<vw_VendorInfo> ListViewVendorByID(string venid) {
            List<vw_VendorInfo> result = new List<vw_VendorInfo>();
            using (GAEntities db = new GAEntities()) {
                result = db.vw_VendorInfo.Where(o => o.VendorID == venid || venid == "" && o.IsActive).ToList();
            }
            return result;
        }

        public static List<SelectOption> ListVendorForSelect(string rcom ) {
            List<SelectOption> result = new List<SelectOption>();
            using (GAEntities db = new GAEntities()) {
                var vens = db.VendorInfo.Where(o => o.RCompanyID == rcom && o.IsActive == true).OrderBy(o=>o.NameTh1).ToList();
                int i = 0;
                foreach (var v in vens) {
                    i++;
                    SelectOption n = new SelectOption();
                    n.IsSelect = true;
                    n.Value = v.VendorID;
                    n.Description = v.FullNameTh;
                    n.Sort = i;
                    result.Add(n);
                }
                SelectOption nb = new SelectOption();
                nb.IsSelect = true;
                nb.Value = "อื่นๆ";
                nb.Description = "อื่นๆ";
                nb.Sort = vens.Count() + 1;
                result.Add(nb);
            }
            return result;
        }
    }
}
