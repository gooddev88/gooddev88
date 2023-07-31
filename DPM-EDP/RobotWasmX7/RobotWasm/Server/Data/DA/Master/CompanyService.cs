using Dapper;
using Npgsql;
using RobotWasm.Server.Data.CimsDB.TT;
using RobotWasm.Shared.Data.DimsDB;
using RobotWasm.Shared.Data.ML.ApiMaster;
using RobotWasm.Shared.Data.ML.Master;
using System.Data;
using System.Linq;
using static RobotWasm.Shared.Data.ML.Shared.I_Result;
using Microsoft.Data.SqlClient;
using RobotWasm.Shared.Data.ML.Login;
using RobotWasm.Server.Helper;
using RobotWasm.Shared.Data.ML.Master.Company;
using System.Text.Json.Serialization;
using System.Text.Json;
using RobotWasm.Server.Data.DA.LogTran;

namespace RobotWasm.Server.Data.DA.Master {
    public class CompanyService
    {

        public I_CompanySet DocSet { get; set; }

        #region Get List

        public static I_CompanySet GetDocSet(string docid) {
            I_CompanySet n = new I_CompanySet();
            using (cimsContext db = new cimsContext()) {
                n.head = db.company_info.Where(o => o.companyid == docid && o.is_active == 1).FirstOrDefault();
            }
            return n;
        }

        public static List<company_info> ListDoc(string Search) {
            List<company_info>? result = new List<company_info>();
            using (cimsContext db = new cimsContext()) {
                result = db.company_info.Where(o =>
                                        (o.companyid.Contains(Search)
                                            || o.comgroupid.Contains(Search)
                                            || o.name1.Contains(Search)
                                            || o.name2.Contains(Search)
                                            || o.province.Contains(Search)
                                            || Search == ""
                                            )
                                            && (o.is_active == 1)
                                            ).OrderByDescending(o => o.created_date).ToList();

            }
            return result;
        }

        public static List<mas_province> ListProvince()
        {
            List<mas_province> result = new List<mas_province>();
            using (cimsContext db = new cimsContext())
            {
                result = db.mas_province.Where(o => o.isactive == 1).OrderBy(o => o.id).ToList();

            }
            return result;
        }

        public static List<company_group_info> ListGroupCompany()
        {
            List<company_group_info> result = new List<company_group_info>();
            using (cimsContext db = new cimsContext())
            {
                result = db.company_group_info.Where(o => o.is_active == 1).OrderByDescending(o => o.created_date).ToList();
            }
            return result;
        }

        public static List<company_info> ListCompany()
        {
            List<company_info> result = new List<company_info>();
            using (cimsContext db = new cimsContext())
            {
                result = db.company_info.Where(o => o.is_active == 1).OrderByDescending(o => o.created_date).ToList();
            }
            return result;
        }

        #endregion

        #region Save

        public static I_BasicResult Save(I_CompanySet doc) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            var h = doc.head;

            try {
                using (cimsContext db = new cimsContext()) {
                    var u = db.company_info.Where(o => o.companyid == h.companyid).FirstOrDefault();
                    if (u == null) {
                        db.company_info.Add(h);
                        db.SaveChanges();
                        var rs = LogTranService.CreateTransLog(h.companyid, h.created_by, "หน่วยงาน", "เพิ่มหน่วยงาน");
                    } else {
                        u.name1 = h.name1;
                        u.name2 = h.name2;
                        u.comgroupid = h.comgroupid;
                        u.province = h.province;
                        u.modified_by = h.modified_by;
                        u.modified_date = DateTime.Now;
                        db.SaveChanges();
                        var rs = LogTranService.CreateTransLog(u.companyid, u.modified_by, "หน่วยงาน", "แก้ไขหน่วยงาน");
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

        public static company_info Checkduplicate(string comid)
        {
            company_info result = new company_info();
            try
            {
                using (cimsContext db = new cimsContext())
                {
                    result = db.company_info.Where(o => o.companyid == comid && o.is_active == 1).FirstOrDefault();
                }
            }
            catch (System.Exception ex)
            {
                var message = ex.Message;
            }

            return result;
        }

        #endregion

        public static I_BasicResult DeleteDoc(string docId,string modifyby) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                using (cimsContext db = new cimsContext()) {
                    var head = db.company_info.Where(o => o.companyid == docId).FirstOrDefault();
                    head.modified_by = modifyby;
                    head.modified_date = DateTime.Now;
                    head.is_active = 0;
                    db.SaveChanges();
                    var rs = LogTranService.CreateTransLog(docId, modifyby, "หน่วยงาน", "ลบหน่วยงาน");
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
