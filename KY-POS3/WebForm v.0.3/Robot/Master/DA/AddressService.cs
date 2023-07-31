using Robot.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Robot.Master.DA {
    public static class AddressService {
        public static vw_ThaiPostAddress  GetAddressInfoById(int id) {
            vw_ThaiPostAddress result = new vw_ThaiPostAddress();
            try {
                using (GAEntities db = new GAEntities()) {
                    result = db.vw_ThaiPostAddress.Where(o => o.ID == id).FirstOrDefault();
                }
            } catch (Exception ex) {

            }
            return result;
        }
    }
}