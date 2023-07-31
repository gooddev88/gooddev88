using Robot.Data.GADB.TT;
using Robot.Data.ML;
using System.Collections.Generic;
using System.Linq;

namespace Robot.Data.DA.DPM.Area {
    public class AreaInfoService {

        public static List<a_mm> ListProvince(string search_text) {
            List<a_mm> output = new List<a_mm>();
            try {
                using (GAEntities db=new GAEntities()) {
          
                    output = (from t in db.a_mm .Where(o=>o.pname.Contains(search_text) || search_text=="")
                              group t by new { t.pname, t.pcode  }
               into grp
                              select new a_mm {
                                  pcode = grp.Key.pcode,
                                  pname = grp.Key.pname 
                              }).OrderBy(o=>o.pcode).ToList();
                }
            } catch (System.Exception ex) {
                 
            }
            return output;
        }

        public static List<a_mm> ListDistinct(string pcode, string search_text) {
            List<a_mm> output = new List<a_mm>();
            try {
                using (GAEntities db = new GAEntities()) {
          

                    output = (from t in db.a_mm.Where(o => o.pcode == pcode
                                                      && (o.aname.Contains(search_text) || search_text=="")
                                                     )
                              group t by new { t.pcode , t.acode, t.aname}
                       into grp
                              select new a_mm {
                                  pcode = grp.Key.pcode,
                                  acode = grp.Key.acode,
                                  aname = grp.Key.aname
                              }).OrderBy(o=>o.acode).ToList(); 
                }
            } catch (System.Exception ex) {

            }
            return output;
        }
        public static List<a_mm> ListSubDistinct(string acode, string search_text) {
            List<a_mm> output = new List<a_mm>();
            try {
                using (GAEntities db = new GAEntities()) {
                  

                    output = (from t in db.a_mm.Where(o => o.acode == acode
                                                         && (o.tname.Contains(search_text) || search_text == "")
                                                    )
                              group t by new {  t.tcode, t.tname }
               into grp
                              select new a_mm {
                                  tcode = grp.Key.tcode,
                                  tname = grp.Key.tname 
                              }).OrderBy(o=>o.tcode).ToList();
                }
            } catch (System.Exception ex) {

            }
            return output;
        }
        public static List<a_mm> ListMooBan(string tcode, string search_text) {
            List<a_mm> output = new List<a_mm>();
            try {
                using (GAEntities db = new GAEntities()) {
                    output = db.a_mm.Where(o => o.tcode == tcode
                                                  && (o.mname.Contains(search_text) || search_text == "")
                                                    ).OrderBy(o => o.tcode).ToList();
                }
            } catch (System.Exception ex) {

            }
            return output;
        }
        public static List<a_community> ListApt(string acode, string search_text) {
            List<a_community> output = new List<a_community>();
            try {
                using (GAEntities db = new GAEntities()) {
                    output = (from t in db.a_community.Where(o => o.acode == acode

                                                            && (o.apt_name.Contains(search_text) || search_text == "")
                                                            )
                                 group t by new { t.acode, t.aname,t.apt_type,t. apt_name,t.apt_code}
                              into grp
                                 select new a_community {
                                acode=      grp.Key.acode,
                                aname=     grp.Key.aname,
                                    apt_type= grp.Key.apt_type,
                                   apt_name=  grp.Key.apt_name,
                                   apt_code=  grp.Key.apt_code
                                 }).OrderBy(o=>o.apt_code).ToList();
                }
            } catch (System.Exception ex) {

            }
            return output;
        }
        public static List<a_community> ListCommu(string apt_code, string search_text) {
            List<a_community> output = new List<a_community>();
            try {
                using (GAEntities db = new GAEntities()) {
                    output  = (from t in db.a_community.Where(o => o.apt_code == apt_code 
                                                                && (o.commu_name.Contains(search_text) || search_text == "")
                                                                )
                                 group t by new  {  t.apt_code , t.commu_name }
                              into grp
                                 select new a_community {
                                     apt_code=     grp.Key.apt_code,
                                     commu_name = grp.Key.commu_name 
                                 }).OrderBy(o=>o.apt_code).ToList();
                }
            } catch (System.Exception ex) {

            }
            return output;
        }

        public static List<SelectOption> ListDimension() {
            List<SelectOption> output = new List<SelectOption>();
            try {
                output.Add(new SelectOption { Description = "ข้อมูลตามพื้นที่", IsSelect = false, Sort = 1, Value = "ข้อมูลตามพื้นที่" });
                output.Add(new SelectOption { Description = "ข้อมูลตาม อปท", IsSelect = false, Sort = 2, Value = "ข้อมูลตาม อปท" });
            } catch (System.Exception ex) {

            }
            return output;
        }
    }
}
