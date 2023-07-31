using System;

namespace Robot.Data.GADB.TT
{
    public class LogInCrossAppReq
    {
        public int ID { get; set; }
        public string ReqID { get; set; }
        public string RComID { get; set; }
        public string Username { get; set; }
        public string AppID { get; set; }
        public DateTime ReqTime { get; set; }
        public DateTime ExpiryTime { get; set; }
        public DateTime? UseTime { get; set; }
        public string ForwardToUrl { get; set; }
        public string CallBackUrl { get; set; }
    }

}
