using Dapper;
using Npgsql;
using RobotWasm.Server.Data.CimsDB.TT;
using RobotWasm.Shared.Data.DimsDB;
using RobotWasm.Shared.Data.ML.ApiMaster;
using RobotWasm.Shared.Data.ML.Master;
using RobotWasm.Shared.Data.ML.DPMBaord.BoardData;
using System.Data;
using System.Linq;
using static RobotWasm.Shared.Data.ML.Shared.I_Result;
using Microsoft.Data.SqlClient;
using RobotWasm.Shared.Data.ML.Login;
using RobotWasm.Server.Helper;
using RobotWasm.Shared.Data.ML.Master.MasterType;

namespace RobotWasm.Server.Data.DA.Master {
    public class MastertypeService {

        public I_MasterTypeSet DocSet { get; set; }

        #region Get List

        public static I_MasterTypeSet GetDocSet(string docid) {
            I_MasterTypeSet n = new I_MasterTypeSet();
            using (cimsContext db = new cimsContext()) {
                n.lineAtive = db.master_type_line.Where(o => o.value_txt == docid && o.master_type_id == "document_cate" && o.is_active == 1).FirstOrDefault();
            }
            return n;
        }

        public static List<master_type_line> ListType(string masId, bool isShowBlank) {
            List<master_type_line> result = new List<master_type_line>();
            using (cimsContext db = new cimsContext()) {
                result = db.master_type_line.Where(o => o.master_type_id == masId
                                                        && o.is_active == 1).ToList();
                if (isShowBlank) {
                    master_type_line n = new master_type_line { master_type_id = "", value_txt = "", master_type_name = "", desc1 = "ทั้งหมด", desc2 = "", value_num = 0, sort = 0 };
                    result.Insert(0, n);
                }
            }
            return result;
        }

        public static List<master_type_line> ListDoc(string Search) {
            List<master_type_line> result = new List<master_type_line>();
            using (cimsContext db = new cimsContext()) {
                result = db.master_type_line.Where(o =>
                                        (o.value_txt.Contains(Search)
                                            || o.desc1.Contains(Search)
                                            || Search == ""
                                            )
                                            && (o.is_active == 1)
                                            ).OrderByDescending(o => o.sort).ToList();

            }
            return result;
        }

        #endregion

        #region Save

        public static I_BasicResult Save(I_MasterTypeSet doc) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            var h = doc.lineAtive;

            try {
                using (cimsContext db = new cimsContext()) {
                    var u = db.master_type_line.Where(o => o.value_txt == h.value_txt).FirstOrDefault();
                    if (u == null) {
                        doc.lineAtive.sort = GenSort();
                        db.master_type_line.Add(doc.lineAtive);
                        db.SaveChanges();
                    } else {
                        u.desc1 = h.desc1;
                        u.desc2 = h.desc2;
                        db.SaveChanges();
                    }
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
        #endregion

        public static I_BasicResult DeleteDoc(string docId) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                using (cimsContext db = new cimsContext()) {
                    var head = db.master_type_line.Where(o => o.value_txt == docId).FirstOrDefault();
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

        public static short GenSort() {
            short result = 1;
            try {
                using (cimsContext db = new cimsContext()) {
                    var h = db.master_type_line.ToList();
                    var max_linenum = h.Max(o => o.sort);
                    result = Convert.ToInt16(max_linenum + 1);
                }
            } catch { }
            return result;
        }

    }
}
