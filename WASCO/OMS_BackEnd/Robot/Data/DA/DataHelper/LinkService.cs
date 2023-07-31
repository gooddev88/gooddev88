using Microsoft.Extensions.Configuration;
using Robot.Data.GADB.TT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Robot.Data.DA.DataHelper {
    public class LinkService
    {
    
        public  static Link GetLinkInfo(string linkname)
        {
            Link r = new Link();
            try {
                using (GAEntities db=new GAEntities()) {
                    r = db.Link.Where(o => o.LinkName == linkname).FirstOrDefault();
                }
                
            } catch (Exception e) {

            }
            return r;
        }
    }
}
