using RobotWasm.Server.Data.GaDB;
using RobotWasm.Shared.Data.GaDB;
using static RobotWasm.Shared.Data.ML.Q.QFloodServiceModel;
using static RobotWasm.Shared.Data.ML.Shared.I_Result;

namespace RobotWasm.Server.Data.DA.Q {
    public class QFloodService {

        public static QFloodDocSet GetDocSet(string mcode) {
            QFloodDocSet doc = new QFloodDocSet();
            try {
                using (GAEntities db = new GAEntities()) {
                    doc.Flood = db.q_flood.Where(o => o.m_code == mcode).FirstOrDefault();
                    if (doc.Flood == null) {
                        doc = NewTransaction(mcode);
                    }
                }

            } catch (Exception ex) {

            }
            return doc;
        }

        public static List<q_group_master> ListGroupMaster( ) {
            List<q_group_master> output = new List<q_group_master>();
            using (GAEntities db=new GAEntities()) {
                output = db.q_group_master.Where(o => o.is_active).OrderBy(o => o.group_sort).ToList();
            }
            return output;
        }

        public static a_mm GetVillage(string mcode) {
            a_mm output = new a_mm();
            using (GAEntities db = new GAEntities()) {
                output = db.a_mm.Where(o => o.mcode == mcode).FirstOrDefault();
            }
            return output;
        }

        #region save

        public static I_BasicResult Save(QFloodDocSet doc, bool isnew) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                using (GAEntities db = new GAEntities()) {

                    if (isnew) {
                        db.q_flood.Add(doc.Flood);
                        db.SaveChanges();
                    } else {
                        var n = db.q_flood.Where(o => o.m_code == doc.Flood.m_code).FirstOrDefault();

                        n.r_code = doc.Flood.r_code;
                        n.area_type = doc.Flood.area_type;
                        n.p_name = doc.Flood.p_name;
                        n.a_name = doc.Flood.a_name;
                        n.t_name = doc.Flood.t_name;
                        n.m_name = doc.Flood.m_name;
                        n.c_name = doc.Flood.c_name;
                        n.aa_01 = doc.Flood.aa_01;
                        n.ab_01 = doc.Flood.ab_01;
                        n.ab_02 = doc.Flood.ab_02;
                        n.ac_01 = doc.Flood.ac_01;
                        n.ac_02 = doc.Flood.ac_02;
                        n.ac_03 = doc.Flood.ac_03;
                        n.ad_01 = doc.Flood.ad_01;
                        n.ad_02 = doc.Flood.ad_02;
                        n.ad_03 = doc.Flood.ad_03;
                        n.ad_04 = doc.Flood.ad_04;
                        n.ad_05 = doc.Flood.ad_05;
                        n.ae_01 = doc.Flood.ae_01;
                        n.ae_02 = doc.Flood.ae_02;
                        n.ae_03 = doc.Flood.ae_03;
                        n.af_01 = doc.Flood.af_01;
                        n.af_02 = doc.Flood.af_02;
                        n.af_03 = doc.Flood.af_03;
                        n.af_04 = doc.Flood.af_04;
                        n.ag_01 = doc.Flood.ag_01;
                        n.ag_02 = doc.Flood.ag_02;
                        n.ag_03 = doc.Flood.ag_03;
                        n.ag_04 = doc.Flood.ag_04;
                        n.ag_05 = doc.Flood.ag_05;
                        n.ag_06 = doc.Flood.ag_06;
                        n.ag_07 = doc.Flood.ag_07;
                        n.ag_08 = doc.Flood.ag_08;
                        n.ag_09 = doc.Flood.ag_09;
                        n.ag_10 = doc.Flood.ag_10;
                        n.ag_11 = doc.Flood.ag_11;
                        n.ag_12 = doc.Flood.ag_12;
                        n.modified_date = DateTime.Now;
                        n.modified_by = doc.Flood.modified_by;

                        db.SaveChanges();
                    }
                }
            } catch (Exception ex) {
                result.Result = "fail";
                if (ex.InnerException != null) {
                    result.Message1 = ex.InnerException.ToString();
                } else {
                    result.Message1 = ex.Message.ToString();
                }
            }
            return result;
        }

        #endregion


        public static QFloodDocSet NewTransaction(string mcode) {
            QFloodDocSet n = new QFloodDocSet();
            n.Flood = NewHead(mcode);
            return n;
        }

        public static q_flood NewHead(string mcode) {
            q_flood n = new q_flood();

            n.id = 0;            
            n.data_id = "";
            n.m_code = mcode;
            n.r_code = "";
            n.area_type = "";
            n.p_name = "";
            n.a_name = "";
            n.t_name = "";
            n.m_name = "";
            n.c_name = "";
            n.aa_01 = "";
            n.ab_01 = "";
            n.ab_02 = "";
            n.ac_01 = false;
            n.ac_02 = false;
            n.ac_03 = false;
            n.ad_01 = false;
            n.ad_02 = false;
            n.ad_03 = false;
            n.ad_04 = false;
            n.ad_05 = false;
            n.ae_01 = false;
            n.ae_02 = false;
            n.ae_03 = false;
            n.af_01 = false;
            n.af_02 = false;
            n.af_03 = false;
            n.af_04 = false;
            n.ag_01 = false;
            n.ag_02 = false;
            n.ag_03 = false;
            n.ag_04 = false;
            n.ag_05 = false;
            n.ag_06 = false;
            n.ag_07 = false;
            n.ag_08 = false;
            n.ag_09 = false;
            n.ag_10 = false;
            n.ag_11 = false;
            n.ag_12 = false;
            n.created_by = "";
            n.created_date = DateTime.Now;
            n.modified_by = "";
            n.modified_date = null;

            return n;
        }


        #region  convert
        public static q_answer ConvertChoiceToAnswer(q_choice i) {
            q_answer o = new q_answer();
            o.id = i.id;
            o.group_id = i.group_id;
            o.q_id = i.q_id;
            o.choice_id = i.choice_id;
            o.choice_description = i.choice_description;
            o.control_type = i.control_type;
            o.answer_text = "";
            //o.answer_number = i.answer_number;
            //o.answer_date = i.answer_date;
            //o.answer_bool = i.answer_bool;
            o.datatype = i.datatype;
            o.sort = i.sort;
            o.is_active = i.is_active;
            return o;
        }

        public static vw_q_answer_group ConvertGroupMasterToAnswerGroup(q_group_master n) {
            vw_q_answer_group o = new vw_q_answer_group();
            o.id = n.id;
            o.group_id = n.group_id;
            o.group_name = n.group_name;
            o.group_description= n.group_description;
            o.username = "";
            o.area_level = "";
            o.area_code = "";
            o.created_by = "";
            o.created_date = DateTime.Now;
            o.modified_by = "";
            o.modified_date = null;
            o.is_active = true;

            return o;
        }
        #endregion


        public static List<a_mm> ListVillage(string search) {
            List<a_mm> output = new List<a_mm>();
            using (GAEntities db = new GAEntities()) {
                if (search != "") {
                    output = db.a_mm.Where(o =>
                                    o.mname.ToLower().Contains(search)
                                  || o.tname.ToLower().Contains(search)
                                  || o.aname.ToLower().Contains(search))
                                    .Take(100).OrderBy(o => o.mcode).ToList();
                } else {
                    output = db.a_mm.Take(100).OrderBy(o => o.mcode).ToList();
                }
            }
            return output;
        }

    }
}
