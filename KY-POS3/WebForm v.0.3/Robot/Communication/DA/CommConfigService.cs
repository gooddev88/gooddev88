using Robot.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Robot.Communication.DA {
    public static class CommConfigService {
        public static LineSender GetSenderInfo(string senderId,string appID)
        {
            LineSender result = new LineSender();
            using (GAEntities db = new GAEntities())
            {
                result = db.LineSender.Where(o => o.SenderID == senderId && o.AppID==appID).FirstOrDefault();
            }
            return result;
        }
    }
}