using Robot.Data.GADB.TT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Robot.Data.ML.I_Result;

namespace Robot.Data.DA {
    public class MacService {


        
        public static I_BasicResult UpdateMacRegisterV2( MacRegister data) {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                using (GAEntities db = new GAEntities()) {
                    if (data.AppVersion==null) {
                        data.AppVersion = "";
                    }
                    var exist = db.MacRegister.Where(o => o.DeviceID == data.DeviceID).FirstOrDefault();
                    if (exist != null) {
                        //exist.DeviceID = data.DeviceID;
                        exist.MacID = data.MacID;
                        exist.RComID = data.RComID;
                        exist.ComID = data.ComID;
                        exist.IsUse = data.IsUse;
                        exist.UserLogin = data.UserLogin;
                        exist.AppVersion= data.AppVersion;
                        exist.NotificationToken = data.NotificationToken;
                        exist.DeviceModel = data.DeviceModel;
                        exist.DeviceName = data.DeviceName;
                        exist.LastLoginDate = data.LastLoginDate;
                        exist.IsActive = data.IsActive;
                    } else {
                        db.MacRegister.Add(data);
                    }
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


        public static List<MacRegister> ListMacRegister(string rcom, string com) {

            List<MacRegister> MacAvl = new List<MacRegister>();
            try {
                using (GAEntities db = new GAEntities()) {

                    MacAvl.Add(new MacRegister { MacID = "A" });
                    MacAvl.Add(new MacRegister { MacID = "B" });
                    MacAvl.Add(new MacRegister { MacID = "C" });
                    MacAvl.Add(new MacRegister { MacID = "D" });
                    MacAvl.Add(new MacRegister { MacID = "E" });
                    MacAvl.Add(new MacRegister { MacID = "F" });
                    MacAvl.Add(new MacRegister { MacID = "G" });
                    MacAvl.Add(new MacRegister { MacID = "H" });
                    MacAvl.Add(new MacRegister { MacID = "I" });
                    MacAvl.Add(new MacRegister { MacID = "J" });
                    MacAvl.Add(new MacRegister { MacID = "K" });
                    MacAvl.Add(new MacRegister { MacID = "L" });
                    MacAvl.Add(new MacRegister { MacID = "M" });
                    MacAvl.Add(new MacRegister { MacID = "N" });
                    MacAvl.Add(new MacRegister { MacID = "O" });
                    MacAvl.Add(new MacRegister { MacID = "P" });
                    MacAvl.Add(new MacRegister { MacID = "Q" });
                    MacAvl.Add(new MacRegister { MacID = "R" });
                    MacAvl.Add(new MacRegister { MacID = "S" });
                    MacAvl.Add(new MacRegister { MacID = "T" });
                    MacAvl.Add(new MacRegister { MacID = "U" });
                    MacAvl.Add(new MacRegister { MacID = "V" });
                    MacAvl.Add(new MacRegister { MacID = "W" });

                    var not_avl = db.MacRegister.Where(o =>
                                                             o.IsActive
                                                          && o.IsUse
                                                          && o.RComID == rcom
                                                          && o.ComID == com
                                                           ).Select(o => o.MacID).ToList();
                    MacAvl = MacAvl.Where(o => !not_avl.Contains(o.MacID)).ToList();


                }
            } catch (Exception ex) {

            }
            return MacAvl;
        }
    }
}
