using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Robot.Data.DataAccess {
    public static class LinkService {
        #region  Method GET

        public static string GetLinkByLinkName(string name) {
            string result = "";
            using (GAEntities db = new GAEntities()) {
             var   q = db.Link.Where(o => o.LinkName == name).FirstOrDefault();
                if (q!=null) {
                    if (q.AppLink!=null) {
                        result = q.AppLink;
                    }
                }
            }
            //fortest33
            //result = "http://localhost:7078/api/po";
            return result;
        }

        public static Link GetLinkByLinkInfo(string linkname) {
            Link result = new Link();
            using (GAEntities db = new GAEntities()) {
                result = db.Link.Where(o => o.LinkName == linkname).FirstOrDefault();
            }
            return result;
        }
        #endregion



    }
   
    
}