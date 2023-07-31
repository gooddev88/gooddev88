﻿using Robot.Data.GADB.TT;
using System;
using System.Linq;
using static Robot.Data.ML.I_Result;

namespace Robot.Data.DA.LoginCrossApp
{
    public class LogInCrossAppService
    {
        public class LogInCrossAppReqX : LogInCrossAppReq
        {
            public string Status { get; set; }
        }

        public   LogInCrossAppReqX GetReqInfo(string reqid)
        {
            LogInCrossAppReqX r = new LogInCrossAppReqX();
            try
            {
                using (GAEntities db = new GAEntities())
                {
                    var q = db.LogInCrossAppReq.Where(o => o.ReqID == reqid).FirstOrDefault();
                    r = Mapper2X(q);
                    if (q==null)
                    {
                        r.Status = "No request found";
                        return r;
                    }
                    if (q.UseTime!=null)
                    {
                        r.Status = "Request was used.";
                        return r;
                    }
                    if (q.ExpiryTime <=DateTime.Now)
                    {
                        r.Status = "Request expired";
                        return r;
                    }
                    r.Status = "ok";

                }

            }
            catch (Exception ex)
            {

            }
            return r;
        }
        public static LogInCrossAppReq CreateReqInfo(string appid, string rcom, string username,string forwardurl="")
        {
            var n = NewReqTransaction(appid, rcom, username, forwardurl);
            try
            { 
                using (GAEntities db = new GAEntities())
                {
                    db.LogInCrossAppReq.Add(n);
                    db.SaveChanges();
                }

            }
            catch (Exception ex)
            {
                n.ReqID = "";
            }
            return n;
        }
        public static   LogInCrossAppReq NewReqTransaction(string appid, string rcom, string username, string forwardurl)
        {
            LogInCrossAppReq n = new LogInCrossAppReq();
            n.ReqID = "";
            n.Username = "";

            n.ReqID = Guid.NewGuid().ToString();
            n.RComID = rcom;
            n.Username = username;
            n.AppID = appid;
            n.ReqTime = DateTime.Now;
            n.ExpiryTime = DateTime.Now.AddMinutes(60);
            n.UseTime = null;
            n.ForwardToUrl = forwardurl;
            n.CallBackUrl = "";
            return n;


        }
        private   LogInCrossAppReqX Mapper2X(LogInCrossAppReq i)
        {
            LogInCrossAppReqX n = new LogInCrossAppReqX();
            n.ReqID = i.ReqID;
            n.Username = i.Username;

            n.ReqID = i.ReqID;
            n.RComID = i.RComID;
            n.Username = i.Username;
            n.AppID = i.AppID;
            n.ReqTime = i.ReqTime;
            n.ExpiryTime = i.ExpiryTime;
            n.UseTime = i.UseTime;
            n.ForwardToUrl = i.ForwardToUrl;
            n.CallBackUrl = i.CallBackUrl;
            n.Status = "";
            return n;
        }

    }
}