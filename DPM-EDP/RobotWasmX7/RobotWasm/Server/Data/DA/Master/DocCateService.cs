using Dapper;
using Npgsql;
using RobotWasm.Server.Data.CimsDB.TT;
using RobotWasm.Shared.Data.DimsDB;
using System.Data;
using System.Linq;
using static RobotWasm.Shared.Data.ML.Shared.I_Result;
using Microsoft.Data.SqlClient;
using RobotWasm.Shared.Data.ML.Login;
using RobotWasm.Server.Helper;
using RobotWasm.Shared.Data.ML.Master.DocCate;
using System.Text.Json.Serialization;
using System.Text.Json;
using RobotWasm.Server.Data.DA.LogTran;

namespace RobotWasm.Server.Data.DA.Master {
    public class DocCateService
    {

        public I_DocCateSet DocSet { get; set; }

        #region Get List

        public static I_DocCateSet GetDocSet(string docid) {
            I_DocCateSet n = new I_DocCateSet();
            using (cimsContext db = new cimsContext()) {
                n.head = db.vw_publishdoc_cate.Where(o => o.cate_id == docid && o.is_active == 1).FirstOrDefault();
                n.files = db.vw_xfile_ref.Where(o => o.doc_id == docid && o.rcom_id == "DPM" && o.com_id == "" && o.doctype == "PUBLISH_CATEGORY" && o.is_active == 1).ToList();
            }
            return n;
        }

        public static List<vw_publishdoc_cate> ListDoc(string Search) {
            Search = Search.ToLower();
            List<vw_publishdoc_cate> result = new List<vw_publishdoc_cate>();
            using (cimsContext db = new cimsContext()) {
                result = db.vw_publishdoc_cate.Where(o =>
                                        (o.cate_id.ToLower().Contains(Search)
                                            || o.cate_name.ToLower().Contains(Search)
                                            || Search == ""
                                            )
                                            && (o.is_active == 1)
                                            ).OrderBy(o => o.sort).ToList();

            }
            return result;
        }

        public static publishdoc_cate GetDocCate(string cateid)
        {
            publishdoc_cate result = new publishdoc_cate();
            using (cimsContext db = new cimsContext())
            {
                result = db.publishdoc_cate.Where(o => o.cate_id == cateid && o.is_active == 1).FirstOrDefault();
            }
            return result;
        }

        public static List<publishdoc_cate> ListDocCate()
        {
            List<publishdoc_cate> result = new List<publishdoc_cate>();
            using (cimsContext db = new cimsContext())
            {
                result = db.publishdoc_cate.Where(o => o.is_active == 1
                                            ).OrderByDescending(o => o.sort).ToList();

            }
            return result;
        }

        public static I_DocCateSet GetLatestFiles(I_DocCateSet doc) {
            try {
                using (cimsContext db = new cimsContext()) {
                    doc.files = db.vw_xfile_ref.Where(o => o.doc_id == doc.head.cate_id && o.rcom_id == "DPM" && o.com_id == "" && o.doctype == "PUBLISH_CATEGORY" && o.is_active == 1).ToList();

                }
            } catch (Exception ex) {

            }
            return doc;
        }


        #endregion

        #region Save

        public static I_BasicResult Save(I_DocCateSet doc) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            var h = doc.head;

            try {
                using (cimsContext db = new cimsContext()) {
                    var u = db.publishdoc_cate.Where(o => o.cate_id == h.cate_id && o.is_active == 1).FirstOrDefault();
                    if (u == null) {
                        //doc.head.sort = GenSort();
                        var data = ConvertViewTo_PublishDoc_Cate(doc.head);
                        db.publishdoc_cate.Add(data);
                        db.SaveChanges();
                    } else {
                        u.cate_name = h.cate_name;
                        u.page = h.page;
                        u.sort = h.sort;
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

        public static I_BasicResult ReOrder(List<publishdoc_cate> data)
        {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try
            {
                using (cimsContext db = new cimsContext())
                {
                    foreach (var d in data)
                    {
                        var query = db.publishdoc_cate.Where(o => o.cate_id == d.cate_id).FirstOrDefault();
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

        public static publishdoc_cate ConvertViewTo_PublishDoc_Cate(vw_publishdoc_cate cate) {
            publishdoc_cate n = new publishdoc_cate();
            n.cate_id = cate.cate_id;
            n.cate_name = cate.cate_name;
            n.sort = cate.sort;
            n.page = cate.page;
            n.img_path = cate.img_path;
            n.is_active = cate.is_active;
            return n;
        }

        #endregion

        public static I_BasicResult DeleteDoc(string docId) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                using (cimsContext db = new cimsContext()) {
                    var head = db.publishdoc_cate.Where(o => o.cate_id == docId).FirstOrDefault();
                    db.publishdoc_cate.Remove(head);
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
                    var h = db.publishdoc_cate.ToList();
                    var max_linenum = h.Max(o => o.sort);
                    result = Convert.ToInt16(max_linenum + 1);
                }
            } catch { }
            return result;
        }

    }
}
