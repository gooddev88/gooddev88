using Microsoft.EntityFrameworkCore;
using Robot.Data.GADB.TT;
using System.Collections.Generic;
using System.Linq;

namespace Robot.Data.DA.Master {
    public class AddressService {
        public static IEnumerable<vw_ThaiPostAddress> ListThaiPostAddress() {
            IEnumerable<vw_ThaiPostAddress> result = new List<vw_ThaiPostAddress>();
            using (GAEntities db = new GAEntities()) {
                result = db.vw_ThaiPostAddress.AsNoTrackingWithIdentityResolution().ToArray();
                // SelectAddr = result.FirstOrDefault();
            }
            return result;
        }
    }
}
