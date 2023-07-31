using Robot.Data;
using Robot.Data.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Robot.POSC.DA
{
    public class POSTableService
    {

        public static List<POS_Table> ListTable() { 
            List<POS_Table> result = new List<POS_Table>();
            string rcom = LoginService.LoginInfo.CurrentRootCompany.CompanyID;
            string com = LoginService.LoginInfo.CurrentCompany.CompanyID;
            using (GAEntities db = new GAEntities()) {
                  result = db.POS_Table.Where(o =>  o.RComID == rcom
                                                     && o.ComID==com
                                                     && o.IsActive
                                                ).ToList();
              
            }
            return result;
        }

        public static POS_Table GetTable(string tableId ) {
            POS_Table result = new POS_Table();
            string rcom = LoginService.LoginInfo.CurrentRootCompany.CompanyID;
            string com = LoginService.LoginInfo.CurrentCompany.CompanyID;
            using (GAEntities db = new GAEntities()) {
                result = db.POS_Table.Where(o => o.RComID == rcom && o.ComID== com && o.TableID == tableId).FirstOrDefault(); 
            }
            return result;
        }
    }
}