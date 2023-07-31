using Robot.Data.DA.POSSY;
using Robot.Data.GADB.TT;
using System;
using System.Collections.Generic;
using System.Linq;
using static Robot.Data.ML.I_Result;
using static Robot.Data.DA.POSSY.POSSaleConverterService;

namespace Robot.Data.DA.Kitchen
{
    public class KitchenService
    {
     public class KitchenStatusParam {

            public string rcom { get; set; }
            public string comid { get; set; }
            public string billId { get; set; }
            public int linenum { get; set; }
            public string lineunq { get; set; }
            public string status { get; set; }
            public string Username { get; set; }
            public string RoomID { get; set; }
            public List<string> cates { get; set; }

        }
        public static I_BasicResult UpdatePOSLineStatus(KitchenStatusParam input) {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
           

            //var rcom = StorageService.CurrRComID;
            try {
                using (GAEntities db = new GAEntities()) {
                    var head = db.POS_SaleHead.Where(o => o.BillID == input.billId && o.RComID == input.rcom && o.ComID==input.comid).FirstOrDefault();
                    head.IsLink = false;
                    head.ModifiedBy = input.Username;
                    head.ModifiedDate = DateTime.Now;
                    //db.POS_SaleHead.Update(head);
                    //db.POS_SaleHead.Update(head);
                    var line = db.POS_SaleLine.Where(o => o.BillID == input.billId && o.RComID == input.rcom && o.LineUnq == input.lineunq && o.ComID == input.comid).FirstOrDefault();
                    var ontop = db.POS_SaleLine.Where(o => o.BillID == input.billId && o.RComID == input.rcom && o.RefLineUnq == input.lineunq && o.ComID == input.comid).ToList();
                    if (line != null) {
                        if (line.Qty==0) {
                            r.Result = "fail";
                            r.Message1 = "รายการถูกยกเลิกโดยผู้รับออเดอร์";
                            return r;
                        }
                        if (input.status.ToUpper()== "K-ACCEPT" || input.status.ToUpper() == "K-ACCEPT-ALL") {//"K-ACCPET-ALL" คือกดเสร็จทั้งหมด
                            if (input.status.ToUpper() == "K-ACCEPT-ALL") {
                                line.KitchenFinishCount =Convert.ToInt32( line.Qty);
                            } else {
                                line.KitchenFinishCount = line.KitchenFinishCount + 1;
                            }
                        
                            if (line.Qty == line.KitchenFinishCount) {
                                line.Status = "K-ACCEPT";
                            }
                          
                        } else {//"K-REJECT"
                            if (line.KitchenFinishCount>0) {//ถ้าครัวทำเสร็จบางรายการแล้วมากดยกเลิก
                                line.Status = "K-ACCEPT";
                            } else {
                                line.Status = input.status.ToUpper();
                            }
                          
                        }
                        line.ModifiedDate = DateTime.Now;
                        //db.POS_SaleLine.Update(line);
                        foreach (var o in ontop) {
                            o.Status = line.Status;
                            o.ModifiedDate = line.ModifiedDate;
                            
                            //db.POS_SaleLine.Update(line);
                        }
                        //  db.POS_SaleLine.Update(line);
                    }
                
                    db.SaveChanges();
                    var r2 =POSService. POS_SaleRefresh(head);
                     
                    if (r2.Result == "fail") {
                        r.Result = r2.Result;
                        r.Message1 = r2.Message1;
                    }
                    FireBaseService.Sendnotify(head.RComID, head.ComID);
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



        public static List<POS_SaleLine> ListOrderForKitchen(KitchenStatusParam input) {
            var result = new List<POS_SaleLine>();
            
            using (GAEntities db = new GAEntities()) {
                
                  result = db.POS_SaleLine.Where(o =>
                                                          (

                                                                 o.RComID == input.rcom
                                                               && o.ComID == input.comid
                                                               && o.IsActive == true
                                                               //&& o.INVID == ""
                                                               && o.IsOntopItem == false
                                                               && o.IsLineActive==true
                                                               && o.Status.ToUpper() == "OK"
                                                               && input.cates.Contains(o.ItemCateID)

                                                           )).OrderBy(o => o.ModifiedDate).ToList();
                


            }
            return result;

        }


 
    }
}
