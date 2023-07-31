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
using RobotWasm.Shared.Data.ML.Master.ApiCate;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace RobotWasm.Server.Data.DA.Master {
    public class ApiCateService {

        public I_ApiCateSet DocSet { get; set; }

        #region Get List

        public static I_ApiCateSet GetDocSet(string docid) {
            I_ApiCateSet n = new I_ApiCateSet();
            using (cimsContext db = new cimsContext()) {
                n.head = db.vw_api_cate.Where(o => o.cate_id == docid && o.is_active == 1).FirstOrDefault();
                n.files = db.vw_xfile_ref.Where(o => o.doc_id == docid && o.rcom_id == "DPM" && o.com_id == "" && o.doctype == "API_CATEGORY" && o.is_active == 1).ToList();
            }
            return n;
        }

        public static List<vw_api_cate> ListDoc(string Search) {
            Search = Search.ToLower();
            List<vw_api_cate> result = new List<vw_api_cate>();
            using (cimsContext db = new cimsContext()) {
                result = db.vw_api_cate.Where(o =>
                                        (o.cate_id.ToLower().Contains(Search)
                                            || o.cate_name.ToLower().Contains(Search)
                                            || Search == ""
                                            )
                                            && (o.is_active == 1)
                                            ).OrderBy(o => o.sort).ToList();

            }
            return result;
        }

        public static List<api_cate> ListApiCate()
        {
            List<api_cate> result = new List<api_cate>();
            using (cimsContext db = new cimsContext())
            {
                result = db.api_cate.Where(o => o.is_active == 1
                                            ).OrderByDescending(o => o.sort).ToList();

            }
            return result;
        }

        public static I_ApiCateSet GetLatestFiles(I_ApiCateSet doc) {
            try {
                using (cimsContext db = new cimsContext()) {
                    doc.files = db.vw_xfile_ref.Where(o => o.doc_id == doc.head.cate_id && o.rcom_id == "DPM" && o.com_id == "" && o.doctype == "API_CATEGORY" && o.is_active == 1).ToList();

                }
            } catch (Exception ex) {

            }
            return doc;
        }


        #endregion

        #region Save

        public static I_BasicResult Save(I_ApiCateSet doc) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            var h = doc.head;

            try {
                using (cimsContext db = new cimsContext()) {
                    var u = db.api_cate.Where(o => o.cate_id == h.cate_id && o.is_active == 1).FirstOrDefault();
                    if (u == null) {
                        doc.head.sort = GenSort();
                        var data = ConvertViewToApiCate(doc.head);
                        db.api_cate.Add(data);
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

        public static api_cate ConvertViewToApiCate(vw_api_cate cate) {
            api_cate n = new api_cate();
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
                    var head = db.api_cate.Where(o => o.cate_id == docId).FirstOrDefault();
                    //head.is_active = 0;
                    db.api_cate.Remove(head);
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

        public static I_BasicResult ReOrder(List<api_cate> data)
        {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try
            {
                using (cimsContext db = new cimsContext())
                {
                    foreach (var d in data)
                    {
                        var query = db.api_cate.Where(o => o.cate_id == d.cate_id).FirstOrDefault();
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

        public static short GenSort() {
            short result = 1;
            try {
                using (cimsContext db = new cimsContext()) {
                    var h = db.api_cate.ToList();
                    var max_linenum = h.Max(o => o.sort);
                    result = Convert.ToInt16(max_linenum + 1);
                }
            } catch { }
            return result;
        }

    }
}
