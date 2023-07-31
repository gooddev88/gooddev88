using RobotWasm.Server.Data.GaDB;
using RobotWasm.Shared.Data.GaDB;
using static RobotWasm.Shared.Data.ML.Q.QServiceModel;

namespace RobotWasm.Server.Data.DA.Q {
    public class QService {




        public static QDocSet GetDocSet(string username,string group_id) {
            QDocSet doc = new QDocSet();
            try {
                using (GAEntities db = new GAEntities()) {
                    doc.AnsGroup = db.vw_q_answer_group.Where(o => o.username == username && o.group_id == group_id).FirstOrDefault();
                    if (doc.AnsGroup==null) {
                        var g = db.q_group_master.Where(o => o.group_id == group_id).FirstOrDefault();
                        doc.AnsGroup = ConvertGroupMasterToAnswerGroup(g);
                    }
                    doc.Questions = db.q_question.Where(o => o.group_id == group_id && o.is_active == true).ToList();
                    doc.Choices=db.q_choice.Where(o => o.group_id == group_id && o.is_active==true).OrderBy(o=>o.sort).ToList();
                    doc.Answers = db.q_answer.Where(o => o.group_id == group_id && o.is_active == true).OrderBy(o=>o.sort).ToList();
                    foreach (var c in doc.Choices) {
                        var chk_ans = doc.Answers.Where(o => o.q_id == c.q_id && o.choice_id == c.choice_id).FirstOrDefault();
                        if (chk_ans==null) {
                            doc.Answers.Add(ConvertChoiceToAnswer(c));
                        }
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

    }
}
