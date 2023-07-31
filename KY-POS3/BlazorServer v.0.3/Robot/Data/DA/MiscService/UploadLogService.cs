using Robot.Data.GADB.TT;
using System;
using System.Linq;
using static Robot.Data.ML.I_Result;

namespace Robot.Data.DA.MiscService {
    public class UploadLogService {
        public static I_BasicResult CreateLog(string uploadid,string func,string result,string message) {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try { 
            UploadLog n= new UploadLog();
                n.UPloadID= uploadid;
                n.FucnID = func;
                n.AppID = "KY POS";
                n.UploadResult = result.ToLower();
                n.Message = message;
                n.UploadDate = DateTime.Now;
                using (GAEntities db = new GAEntities()) {
                    db.UploadLog.Add(n);
                    db.SaveChanges();
                }
            } catch (Exception e) {
                r.Result = "fail";
                if (e.InnerException!=null) {
                    r.Message1 = e.InnerException.ToString();
                } else {
                    r.Message1 = e.Message;
                }
                
            }
            return r;
        }

        public static UploadLog GetUploadLog(string uploadid) {
            UploadLog output = new UploadLog();
            try {
               
                using (GAEntities db = new GAEntities()) {
                    output= db.UploadLog.Where(o => o.UPloadID == uploadid).FirstOrDefault();
                }
            } catch (Exception e) {
                

            }
            return output;
        }
        public   class Uploadinfo {
            public string docref { get; set; }
            public string rcom { get; set; }
            public string com { get; set; }
            public string user { get; set; }
            public string uploadid { get; set; }

        }
        public static Uploadinfo GetUploadInfo(string info) {
            Uploadinfo n = new Uploadinfo();
            string[] doc = info.Split(":");
            if (doc==null) {
                n.docref = "";
                n.rcom = "";
                n.com = "";
                n.user = "";
                n.uploadid = "";
            } else {
                n.docref = doc[0] == null ? "" : doc[0];
                n.rcom = doc[1] == null ? "" : doc[1];
                n.com = doc[2] == null ? "" : doc[2];
                n.user = doc[3] == null ? "" : doc[3];
                n.uploadid = doc[4] == null ? "" : doc[4];
            }
           
            return n;
        }
    }
}
