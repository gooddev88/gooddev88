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
using RobotWasm.Shared.Data.ML.Master.ComGroup;
using System.Text.Json.Serialization;
using System.Text.Json;
using RobotWasm.Server.Data.DA.LogTran;

namespace RobotWasm.Server.Data.DA.Master {
    public class CompanyGroupService {

        public I_ComGroupSet DocSet { get; set; }

        #region Get List

        public static I_ComGroupSet GetDocSetComGroup(string docid) {
            I_ComGroupSet n = new I_ComGroupSet();
            using (cimsContext db = new cimsContext()) {
                n.head = db.company_group_info.Where(o => o.company_groupid == docid && o.is_active == 1).FirstOrDefault();
            }
            return n;
        }

        public static List<company_group_info> ListDocHead(string Search) {
            List<company_group_info> result = new List<company_group_info>();
            using (cimsContext db = new cimsContext()) {
                result = db.company_group_info.Where(o =>
                                        (o.company_groupid.Contains(Search)
                                            || o.name1.Contains(Search)
                                            || o.name2.Contains(Search)
                                            || Search == ""
                                            )
                                            && (o.is_active == 1)
                                            ).OrderByDescending(o => o.created_date).ToList();

            }
            return result;
        }

        #endregion

        #region Save

        public static I_BasicResult Save(I_ComGroupSet doc) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            var h = doc.head;

            try {
                using (cimsContext db = new cimsContext()) {
                    var u = db.company_group_info.Where(o => o.company_groupid == h.company_groupid).FirstOrDefault();
                    if (u == null) {
                        db.company_group_info.Add(h);
                        db.SaveChanges();
                        var rs = LogTranService.CreateTransLog(h.company_groupid, h.created_by, "กลุ่มหน่วยงาน", "เพิ่มกลุ่มหน่วยงาน");
                    } else {
                        u.name1 = h.name1;
                        u.name2 = h.name2;
                        u.modified_by = h.modified_by;
                        u.modified_date = DateTime.Now;
                        db.SaveChanges();
                        var rs = LogTranService.CreateTransLog(u.company_groupid,u.modified_by, "กลุ่มหน่วยงาน", "แก้ไขกลุ่มหน่วยงาน");
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

        public static company_group_info Checkduplicate(string comgroupid)
        {
            company_group_info result = new company_group_info();
            try
            {
                using (cimsContext db = new cimsContext())
                {
                    result = db.company_group_info.Where(o => o.company_groupid == comgroupid && o.is_active == 1).FirstOrDefault();
                }
            }
            catch (System.Exception ex)
            {
                var message = ex.Message;
            }

            return result;
        }

        #endregion

        public static I_BasicResult DeleteDocComGroup(string docId,string modifyby) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                using (cimsContext db = new cimsContext()) {
                    var head = db.company_group_info.Where(o => o.company_groupid == docId).FirstOrDefault();
                    head.modified_by = modifyby;
                    head.modified_date = DateTime.Now;
                    head.is_active = 0;
                    db.SaveChanges();
                    var rs = LogTranService.CreateTransLog(docId, modifyby, "กลุ่มหน่วยงาน", "ลบกลุ่มหน่วยงาน");
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


    }
}
