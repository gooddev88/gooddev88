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
using RobotWasm.Shared.Data.ML.Master.News;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace RobotWasm.Server.Data.DA.Master {
    public class NewsService {

        public I_NewsSet DocSet { get; set; }

        #region Get List

        public static I_NewsSet GetDocSet(string docid) {
            I_NewsSet n = new I_NewsSet();
            using (cimsContext db = new cimsContext()) {
                n.head = db.news_info.Where(o => o.newid == docid && o.is_active == 1).FirstOrDefault();
                //n.files = db.vw_xfile_ref.Where(o => o.doc_id == docid && o.rcom_id == "DPM" && o.com_id == "" && o.doctype == "API_CATEGORY" && o.is_active == 1).ToList();
            }
            return n;
        }

        public static List<news_info> ListDoc(string search) {
            List<news_info>? result = new List<news_info>();
            using (cimsContext db = new cimsContext()) {
                result = db.news_info.Where(o =>
                                        (o.title.Contains(search)
                                            || o.desc.Contains(search)
                                            || search == ""
                                            )
                                            && (o.is_active == 1)
                                            ).OrderByDescending(o => o.newdate).ToList();

            }
            return result;
        }

        //public static I_ApiCateSet GetLatestFiles(I_ApiCateSet doc) {
        //    try {
        //        using (cimsContext db = new cimsContext()) {
        //            doc.files = db.vw_xfile_ref.Where(o => o.doc_id == doc.head.cate_id && o.rcom_id == "DPM" && o.com_id == "" && o.doctype == "API_CATEGORY" && o.is_active == 1).ToList();

        //        }
        //    } catch (Exception ex) {

        //    }
        //    return doc;
        //}


        #endregion

        #region Save

        public static I_BasicResult Save(I_NewsSet doc) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            var h = doc.head;

            try {
                using (cimsContext db = new cimsContext()) {
                    var u = db.news_info.Where(o => o.newid == h.newid).FirstOrDefault();
                    if (u == null) {
                        db.news_info.Add(h);
                        db.SaveChanges();
                    } else {
                        u.title = h.title;
                        u.newdate = h.newdate;
                        u.desc = h.desc;
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
                    var head = db.news_info.Where(o => o.newid == docId).FirstOrDefault();
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

    }
}
