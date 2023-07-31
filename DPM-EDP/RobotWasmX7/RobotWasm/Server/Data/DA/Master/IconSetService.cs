using RobotWasm.Server.Data.CimsDB.TT;
using RobotWasm.Server.Data.DA.LogTran;
using RobotWasm.Shared.Data.DimsDB;
using RobotWasm.Shared.Data.ML.Master.IconSet;
using static RobotWasm.Shared.Data.ML.Shared.I_Result;

namespace RobotWasm.Server.Data.DA.Master
{
    public class IconSetService {

        public static I_IconSet GetDocSet(string docid) {
            I_IconSet doc = new I_IconSet();
            try {
                using (cimsContext db = new cimsContext()) {
                    doc.head = db.icon_set.Where(o => o.icon_id == docid).FirstOrDefault();
                    doc.vhead = db.vw_icon_set.Where(o => o.icon_id == docid).FirstOrDefault();
                    doc.files = db.vw_xfile_ref.Where(o => o.doc_id == docid && o.rcom_id == "DPM" && o.com_id == "" && o.doctype == "ICON_CATEGORY" && o.is_active == 1).ToList();
                }
            } catch (Exception ex) {

            }
            return doc;
        }

        public static List<vw_icon_set> ListDoc(string Search)
        {
            List<vw_icon_set> result = new List<vw_icon_set>();
            using (cimsContext db = new cimsContext())
            {
                result = db.vw_icon_set.Where(o =>
                                        (o.icon_name.Contains(Search)
                                            || o.filename.Contains(Search)
                                            || Search == ""
                                            )
                                            && (o.is_active == 1)
                                            ).OrderByDescending(o => o.sort).ToList();
            }
            return result;
        }


        #region save

        public static I_BasicResult Save(I_IconSet doc, bool action) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                using (cimsContext db = new cimsContext()) {
                    if (action) {
                        db.icon_set.Add(doc.head);
                        db.SaveChanges();
                    } else {
                        var n = db.icon_set.Where(o => o.icon_id == doc.head.icon_id).FirstOrDefault();
                        n.icon_name = doc.head.icon_name;
                        n.sort = doc.head.sort;
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

        public static I_BasicResult DeleteDoc(string docId) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                using (cimsContext db = new cimsContext()) {
                    var head = db.icon_set.Where(o => o.icon_id == docId).FirstOrDefault();
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

        public static I_IconSet GetLatestFiles(I_IconSet doc)
        {
            try
            {
                using (cimsContext db = new cimsContext())
                {
                    doc.files = db.vw_xfile_ref.Where(o => o.doc_id == doc.head.icon_id && o.rcom_id == "DPM" && o.com_id == "" && o.doctype == "ICON_CATEGORY" && o.is_active == 1).ToList();
                }
            }
            catch (Exception ex)
            {
            }
            return doc;
        }

        public static short GenSort()
        {
            short result = 1;
            try
            {
                using (cimsContext db = new cimsContext())
                {
                    var h = db.icon_set.ToList();
                    var max_linenum = h.Max(o => o.sort);
                    result = Convert.ToInt16(max_linenum + 1);
                }
            }
            catch { }
            return result;
        }

    }
}
