using RobotWasm.Server.Data.CimsDB.TT;
using RobotWasm.Server.Data.DA.LogTran;
using RobotWasm.Server.Data.GaDB;
using RobotWasm.Shared.Data.DimsDB;
using RobotWasm.Shared.Data.GaDB;
using RobotWasm.Shared.Data.ML.DPMBaord.ExclusiveBoard;
using static RobotWasm.Shared.Data.ML.Shared.I_Result;

namespace RobotWasm.Server.Data.DA.Board
{
    public class BoardMasterService {

        public static I_Board_MasterSet GetDocSet(string docid) {
            I_Board_MasterSet doc = new I_Board_MasterSet();
            try {
                using (cimsContext db = new cimsContext()) {
                    doc.head = db.board_master.Where(o => o.board_id == docid).FirstOrDefault();
                    doc.vhead = db.vw_board_master.Where(o => o.board_id == docid).FirstOrDefault();
                    doc.files = db.vw_xfile_ref.Where(o => o.doc_id == docid && o.rcom_id == "DPM" && o.com_id == "" && o.doctype == "IMAGE_BOARD" && o.is_active == 1).ToList();
                }
            } catch (Exception ex) {

            }
            return doc;
        }

        public static List<vw_board_master> ListDoc(string Search)
        {
            List<vw_board_master> result = new List<vw_board_master>();
            using (cimsContext db = new cimsContext())
            {
                result = db.vw_board_master.Where(o =>
                                        (o.board_id.Contains(Search)
                                            || o.name.Contains(Search)
                                            || o.description.Contains(Search)
                                            || o.filename.Contains(Search)
                                            || Search == ""
                                            )
                                            && (o.is_active == 1)
                                            ).OrderBy(o => o.sort).ToList();

            }
            return result;
        }


        #region save

        public static I_BasicResult Save(I_Board_MasterSet doc, bool action) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                using (cimsContext db = new cimsContext()) {

                    if (action) {
                        var checkdup = db.board_master.Where(o => o.board_id == doc.head.board_id).FirstOrDefault();
                        if (checkdup != null) {
                            result.Result = "fail";
                            result.Message1 = "รหัส บอร์ดนี้มีในระบบแล้ว";
                        }else {
                            db.board_master.Add(doc.head);
                            db.SaveChanges();
                        }
                    } else {
                        var n = db.board_master.Where(o => o.board_id == doc.head.board_id).FirstOrDefault();

                        n.name = doc.head.name;
                        n.description = doc.head.description;
                        n.page = doc.head.page;
                        n.board_type = doc.head.board_type;
                        n.board_url = doc.head.board_url;
                        //n.img_path = doc.head.img_path;
                        n.authen_id = doc.head.authen_id;
                        n.sort = doc.head.sort;
                        n.is_active = doc.head.is_active;
                        n.is_default = doc.head.is_default;
                        db.SaveChanges();
                    }
                    if (doc.head.is_default==1) {
                        //update is_default ให้มีแค่ 1 record ในระบบเท่านั้น
                        var update_isdefault = db.board_master.Where(o => o.board_id != doc.head.board_id && o.board_type== "standard").ToList();
                        foreach (var u in update_isdefault) {
                            u.is_default = 0;
                        }
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

        public static I_BasicResult ReOrder(List<board_master> data)
        {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try
            {
                using (cimsContext db = new cimsContext())
                {
                    foreach (var d in data)
                    {
                        var query = db.board_master.Where(o => o.board_id == d.board_id).FirstOrDefault();
                        if (query != null)
                        {
                            query.sort = d.sort;
                        }
                    }
                    db.SaveChanges();
                }

            }
            catch (Exception ex)
            {
                result.Result = "fail";
                if (ex.InnerException != null)
                {
                    result.Message1 = ex.InnerException.ToString();
                }
                else
                {
                    result.Message1 = ex.Message;
                }
            }
            return result;
        }

        #endregion

        public static I_BasicResult DeleteDoc(string docId,string modified_by) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                using (cimsContext db = new cimsContext()) {
                    var head = db.board_master.Where(o => o.board_id == docId).FirstOrDefault();
                    head.is_active = 0;

                    db.SaveChanges();
                }

            } catch (Exception ex) {
                result.Result = "fail";
                if (ex.InnerException != null) {
                    result.Message1 = ex.InnerException.ToString();
                } else {
                    result.Message1 = ex.Message;
                }
            }

            return result;
        }

        public static I_Board_MasterSet GetLatestFiles(I_Board_MasterSet doc)
        {
            try
            {
                using (cimsContext db = new cimsContext())
                {
                    doc.files = db.vw_xfile_ref.Where(o => o.doc_id == doc.head.board_id && o.rcom_id == "DPM" && o.com_id == "" && o.doctype == "IMAGE_BOARD" && o.is_active == 1).ToList();

                }
            }
            catch (Exception ex)
            {

            }
            return doc;
        }


        public static I_Board_MasterSet NewTransaction() {
            I_Board_MasterSet n = new I_Board_MasterSet();
            n.head = NewHead();
            return n;
        }

        public static board_master NewHead() {
            board_master n = new board_master();
            
            n.board_id = "";
            n.name = "";
            n.description = "";
            n.board_type = "exclusive";
            n.sort = 0;
            n.board_url = "";
            n.img_path = "";
            n.page = "";
            n.authen_id = "dpm_prod";
            n.is_active = 1;

            return n;
        }

        public static short GenSort() {
            short result = 1;
            try {
                using (cimsContext db = new cimsContext()) {
                    var h = db.board_master.ToList();
                    var max_linenum = h.Max(o => o.sort);
                    result = Convert.ToInt16(max_linenum + 1);
                }
            } catch { }
            return result;
        }

    }
}
