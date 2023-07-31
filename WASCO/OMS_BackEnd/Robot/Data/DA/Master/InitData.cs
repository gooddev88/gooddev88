using Robot.Data.ML;
using Robot.Data.GADB.TT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Robot.Data.ML.I_Result;

namespace Robot.Data.DA.Master {
    public class InitData {



        #region Item Info
        public static I_BasicResult NewMiscBeginOfNewCompany(string rcom, string comID) {
            var r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {

                var n = ItemService.NewItem(rcom, comID);
                n.ItemID = "MISC1X";
                n.RCompanyID = rcom;
                n.CompanyID = comID;

                n.Name1 = "คำอธิบายเพิ่มเติม";
                n.TypeID = "MISC";
                //n.IsSysData = true;
                using (GAEntities db = new GAEntities()) {
                    var chk_exist = db.ItemInfo.Where(o => o.RCompanyID == n.RCompanyID && o.CompanyID == comID && o.ItemID == "MISC1X").FirstOrDefault();
                    if (chk_exist == null) {
                        db.ItemInfo.Add(n);
                        db.SaveChanges();
                    }
                }
            } catch (Exception ex) {
                r.Result = "fail";
                if (ex.InnerException != null) {
                    r.Message1 = ex.InnerException.ToString();
                } else {
                    r.Message1 = ex.Message;
                }

            }
            return r;

        }
        #endregion

        #region O Payment
        public static I_BasicResult NewDefaultPayment(string rcom, string com) {
            var r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {

                List<OPaymentType> pp = new List<OPaymentType>();
                var opayInit = OPaymentService.InitPaymentDataNewCompany(rcom, com);
                using (GAEntities db = new GAEntities()) {
                    var chkExistCode = db.OPaymentType.Where(o => o.CompanyID == com && o.RCompanyID == rcom).Select(o => o.Code).ToList();
                    opayInit.RemoveAll(o => chkExistCode.Contains(o.Code));
                    db.OPaymentType.AddRange(opayInit);
                    db.SaveChanges();
                }

            } catch (Exception ex) {
                r.Result = "fail";
                if (ex.InnerException != null) {
                    r.Message1 = ex.InnerException.ToString();
                } else {
                    r.Message1 = ex.Message;
                }

            }
            return r;

        }
        #endregion
    }
}
