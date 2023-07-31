using Dapper;
using Npgsql;
using RobotWasm.Server.Data.CimsDB.TT;
using RobotWasm.Server.Data.DA.LogTran;
using RobotWasm.Shared.Data.DimsDB;
using RobotWasm.Shared.Data.ML.ApiMaster;
using RobotWasm.Shared.Data.ML.DPMBaord.BoardData;
using RobotWasm.Shared.Data.ML.Shared;
using System.Data;
using System.Linq;
using static RobotWasm.Shared.Data.ML.Shared.I_Result;

namespace RobotWasm.Server.Data.DA.Board {
    public class ApiMasterService {
        public static IDbConnection Connection {
            get {
                return new NpgsqlConnection((Globals.CimsConn));
            }
        }

        public static string sessionActiveId = "activetapiid";
      


        public I_ApiMasterSet DocSet { get; set; }

        public ApiMasterService() {

        }

        #region Get List
        public I_ApiMasterSet GetDocSet(string docid) {
            I_ApiMasterSet n = NewTransaction();
            using (cimsContext db = new cimsContext()) {
                n.Head = db.api_master.Where(o => o.api_id == docid).FirstOrDefault();
                n.paramLine = db.api_param_res.Where(o => o.api_id == docid).OrderBy(o => o.param_type).ThenBy(o => o.sort).ToList();
                n.TagLine = db.api_tag.Where(o => o.api_id == docid).ToList();
            }
            return n;
        }

        public static IEnumerable<api_master> Listapi_master(string search) {
            IEnumerable<api_master> result;
            search = search.ToLower();
            using (cimsContext db = new cimsContext()) {
                result = db.api_master.Where(o =>
                                                    (o.api_id.ToLower().Contains(search)
                                                        || o.api_desc.ToLower().Contains(search)
                                                        || o.api_name.ToLower().Contains(search)
                                                        || o.cate.ToLower().Contains(search)
                                                        || search == "")
                                                        && o.is_publish == 1
                                                        && o.is_active == 1
                                                    ).OrderByDescending(o => o.api_id).ToList();
            }

            return result;
        }
        //public static List<api_cate> ListApi_Cates() {
        //    List<api_cate> output = new List<api_cate>();
        //    try {
        //        using (cimsContext db = new cimsContext()) {
        //            output = db.api_cate.Where(o => o.is_active == 1).ToList();
        //            //foreach (var l in data) {
        //            //    l.count = ListApiMasterByCate(l.cate_id).Count();
        //            //}
        //            //output = data;
        //        }
        //    } catch (Exception ex) {

        //    }

        //    return output;
        //}
        public static IEnumerable<api_master> ListApi_BySearch(string search, string cate) {
            IEnumerable<api_master> result;
            search = search == null ? "" : search.ToLower();
            if (search != "") {
                cate = "";
            }

            search = search.ToLower();
            cate = cate.ToLower();
            using (cimsContext db = new cimsContext()) {
                var ids = db.api_tag.Where(o => o.tag.Contains(search)).Select(o => o.api_id).Distinct().ToList();
                if (search != "") {
                    result = db.api_master.Where(o =>
                                               (ids.Contains(o.api_id) || ids.Count == 0)
                                              &&(
                                                     o.api_id.ToLower().Contains(search)
                                                  || o.api_desc.ToLower().Contains(search)
                                                  || o.api_name.ToLower().Contains(search)
                                                  )
                                                  && o.is_publish==1
                                                  && o.is_active==1
                                                  && (o.cate.ToLower() == cate || cate == "")
                                              ).OrderByDescending(o => o.id).ToList();
                } else {
                    result = db.api_master.Where(o =>
                                               (o.cate.ToLower() == cate || cate == "")
                                                  && o.is_publish == 1
                                                  && o.is_active == 1
                                         ).OrderByDescending(o => o.id).ToList();
                }
            }
            return result;
        }

        public static List<api_master> ListApiMasterByCate(string cateid) {
            List<api_master> result = new List<api_master>();
            try {
                using (cimsContext db = new cimsContext()) {
                    result = db.api_master.Where(o => o.cate == cateid).ToList();
                }
            } catch (System.Exception ex) {
                var message = ex.Message;
            }

            return result;
        }

        public static List<ApiCate> ListDataCategory() {
            List<ApiCate> result = new List<ApiCate>();
            using (cimsContext db = new cimsContext()) {
                var cate = db.vw_api_cate.Where(o => o.is_active == 1).OrderBy(o => o.sort).ToList();
                foreach (var l in cate) {
                    var data = ConvertToClassApiCate(l);
                    result.Add(data);
                }
                result.FirstOrDefault().IsVisible = true;
            }
            return result;
        }

        //public static List<SelectOption> ListCate() {
        //    var output = new List<SelectOption>();
        //    var cate = ListDataCategory();
        //    foreach (var c in cate) {
        //        output.Add(new SelectOption { Value = c.cate_id, Description = c.cate_name, Sort = c.sort });
        //    }
        //    return output;
        //}

        public static ApiCate ConvertToClassApiCate(vw_api_cate cate) {
            ApiCate n = new ApiCate();
            n.cate_id = cate.cate_id;
            n.cate_name = cate.cate_name;
            n.sort = cate.sort;
            n.Count = ListApiMasterByCate(cate.cate_id).Count().ToString("n0");
            n.img_path = cate.img_path;
            n.filename = cate.filename;
            n.page = cate.page;
            n.IsVisible = false;
            n.is_active = cate.is_active;
            return n;
        }

        public static api_master GetApiMaster(string apiid) {
            api_master result = new api_master();
            try {
                using (cimsContext db = new cimsContext()) {
                    result = db.api_master.Where(o => o.api_id == apiid).FirstOrDefault();
                }
            } catch (System.Exception ex) {
                var message = ex.Message;
            }

            return result;
        }

        public static List<api_master> ListApiMaster() {
            List<api_master> output = new List<api_master>();
            try {
                using (cimsContext db = new cimsContext()) {
                    var ddd = db.api_master.ToList();
                }
            } catch (System.Exception ex) {
                var message = ex.Message;
            }

            return output;
        }
        #endregion

        #region Save

        public static I_BasicResult Save(api_master doc) {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                using (cimsContext db = new cimsContext()) {
                    var h = db.api_master.Where(o => o.api_id == doc.api_id).FirstOrDefault();
                    if (h == null) {
                        db.api_master.Add(doc);
                        db.SaveChanges();
                    } else {
                        h.owner_code = doc.owner_code;
                        h.source_connection_code = doc.source_connection_code;
                        h.source_api_url = doc.source_api_url;
                        h.base_url = doc.base_url;
                        h.api_url = doc.api_url;
                        h.api_name = doc.api_name;
                        h.api_desc = doc.api_desc;
                        h.api_type = doc.api_type;
                        h.method = doc.method;
                        h.version = doc.version;
                        h.authen = doc.authen;
                        h.api_url_external = doc.api_url_external;
                        h.data_source = doc.data_source;
                        h.update_frequency = doc.update_frequency;
                        h.parameter_sample = doc.parameter_sample;
                        h.output_sample = doc.output_sample;
                        h.contact = doc.contact;
                        h.cate = doc.cate;
                        h.source_cate = doc.source_cate;
                        h.url_page = doc.url_page;
                        h.remark = doc.remark;
                        h.has_api = doc.has_api;
                        h.is_publish = doc.is_publish;
                        h.is_active = doc.is_active;

                        db.SaveChanges();
                    }
                }

            } catch (Exception ex) {
                r.Result = "fail";
                if (ex.InnerException != null) {
                    r.Message1 = ex.InnerException.ToString();
                } else {
                    r.Message1 = ex.Message;
                }
            }
            return r;
        }

        #endregion

        #region New
        public static I_ApiMasterSet NewTransaction() {
            I_ApiMasterSet n = new I_ApiMasterSet();
            n.Head = NewHead();
            n.TagLine = new List<api_tag>();
            n.paramLine = new List<api_param_res>();
            return n;
        }

        public static api_master NewHead() {
            api_master n = new api_master();

            n.api_id = "";
            n.owner_code = "";
            n.source_connection_code = "";
            n.source_api_url = "";
            n.base_url = "";
            n.api_url = "";
            n.api_name = "";
            n.api_desc = "";
            n.api_type = "";
            n.method = "GET";
            n.version = "";
            n.authen = "";
            n.data_source = "";
            n.update_frequency = "";
            n.parameter_sample = "";
            n.output_sample = "";
            n.contact = "";
            n.cate = "";
            n.source_cate = "";
            n.url_page = "";
            n.remark = "";
            n.is_active = 1;
            return n;
        }

        public static api_param_res NewParam() {
            api_param_res n = new api_param_res();

            n.api_id = "";
            n.field_id = "";
            n.param_type = "";
            n.description = "";
            n.data_type = "";
            n.sort = 0;
            n.is_require = 1;
            return n;
        }

        public static int GenSort() {
            int result = 1;
            try {
                using (cimsContext db = new cimsContext()) {
                    var h = db.api_param_res.ToList();
                    var max_linenum = h.Max(o => o.sort);
                    result = max_linenum + 1;
                }
            } catch { }
            return result;
        }

        public static api_tag NewTag() {
            api_tag n = new api_tag();

            n.api_id = "";
            n.tag = "";
            n.is_active = 1;
            return n;
        }


        #endregion

        #region tag param
        public static I_BasicResult AddApiiParam(api_param_res data) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                using (cimsContext db = new cimsContext()) {
                    var param = db.api_param_res.Where(o => o.api_id == data.api_id
                                                                && o.field_id == data.field_id
                                                                && o.param_type == data.param_type
                                                                ).FirstOrDefault();
                    if (param == null) {
                        db.api_param_res.Add(data);
                        db.SaveChanges();
                    } else {
                        //exist 
                        param.description = data.description;
                        param.field_id = data.field_id;
                        param.data_type = data.data_type;
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

        public static I_BasicResult DeleteApiParam(int ID) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                using (cimsContext db = new cimsContext()) {
                    db.api_param_res.Remove(db.api_param_res.Where(o => o.id == ID).FirstOrDefault());
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

        #endregion


        #region tag api
        public static I_BasicResult AddApiTag(api_tag data) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                using (cimsContext db = new cimsContext()) {
                    var tag = db.api_tag.Where(o => o.api_id == data.api_id
                                                                && o.tag == data.tag
                                                                ).FirstOrDefault();
                    if (tag == null) {
                        db.api_tag.Add(data);
                        db.SaveChanges();
                    } else {
                        //exist 
                        tag.tag = data.tag;
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

        public static I_BasicResult DeleteApiTag(int id) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                using (cimsContext db = new cimsContext()) {
                    db.api_tag.Remove(db.api_tag.Where(o => o.id == id).FirstOrDefault());
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

        public static I_BasicResult ReOrder(List<api_param_res> data) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                using (cimsContext db = new cimsContext()) {
                    foreach (var d in data) {
                        var query = db.api_param_res.Where(o => o.field_id == d.field_id).FirstOrDefault();
                        if (query != null) {
                            query.sort = d.sort;
                        }
                    }
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
        #endregion

        #region sample of dapper

        //public static List<AccidentCountProvinceResult> GetTopProvinceHasAccident(DateTime datebegin, DateTime dateend, int top, string type) {
        //    List<AccidentCountProvinceResult> result = new List<AccidentCountProvinceResult>();
        //    try {
        //        using (IDbConnection conn = Connection) {
        //            string sql = "";
        //            if (type == "most_event") { //อุบัติเหตสูงสุด

        //                conn.Open();
        //                sql = @"
        //            select
        //               province
        //            ,count(*) as count_result
        //            from data_accidents
        //            where accident_date between @datebegin and @dateend
        //            group by province
        //            order by count(*) desc
        //            limit @top
        //            ";
        //            }

        //            if (type == "most_deceased") {//เสียชีวิตสูงสุด

        //                conn.Open();
        //                sql = @"
        //            select
        //               province
        //            ,count(*) as count_result
        //            from data_accidents
        //            where accident_date between @datebegin and @dateend
        //            group by province
        //            order by count(*) desc
        //            limit @top
        //            ";
        //            }
        //            if (type == "most_injured") {//บาดเจ็บสูงสุด

        //                conn.Open();
        //                sql = @"
        //            select
        //               province
        //            ,count(*) as count_result
        //            from data_accidents
        //            where accident_date between @datebegin and @dateend
        //            group by province
        //            order by count(*) desc
        //            limit @top
        //            ";
        //            }
        //            var dynamicParameters = new DynamicParameters();
        //            dynamicParameters.Add("datebegin", datebegin);
        //            dynamicParameters.Add("dateend", dateend);
        //            dynamicParameters.Add("top", top);

        //            result = conn.Query<AccidentCountProvinceResult>(sql, dynamicParameters).ToList();
        //        }


        //    } catch (Exception) {

        //        throw;
        //    }

        //    return result;
        //}
        #endregion

        #region connection
        public static vw_api_master GetApiInfo(string api_code) {
            vw_api_master refx = new vw_api_master();
            using (cimsContext db = new cimsContext()) {
                refx = db.vw_api_master.Where(o => o.api_id.ToLower() == api_code.ToLower()).FirstOrDefault();
            }
            return refx;
        }

        #endregion


        // list UserNotDepartment select



        public static List<LISTSELECT_APIMasterALLCate> ListAPIMasterALLCate()
        {
            List<LISTSELECT_APIMasterALLCate> result = new List<LISTSELECT_APIMasterALLCate>();

            using (cimsContext db = new cimsContext())
            {
                var query = new List<vw_api_master>();
                query = db.vw_api_master.Where(o =>
                                        o.is_active == 1
                                        && o.is_publish == 1
                        ).OrderBy(o => o.cate).ThenBy(o => o.api_id).ToList();
                foreach (var q in query)
                {
                    var qq = ConvertToApiMaster(q);
                    result.Add(qq);
                }
            }
            return result;
        }

        public static LISTSELECT_APIMasterALLCate ConvertToApiMaster(vw_api_master doc)
        {
            LISTSELECT_APIMasterALLCate h = new LISTSELECT_APIMasterALLCate();
            h.IsSelect = false;
            h.api_id = doc.api_id;
            h.owner_code = doc.owner_code;
            h.source_connection_code = doc.source_connection_code;
            h.source_api_url = doc.source_api_url;
            h.base_url = doc.base_url;
            h.api_url = doc.api_url;
            h.api_name = doc.api_name;
            h.api_desc = doc.api_desc;
            h.api_type = doc.api_type;
            h.method = doc.method;
            h.version = doc.version;
            h.authen = doc.authen;
            //h.api_url_external = doc.api_url_external;
            h.data_source = doc.data_source;
            h.update_frequency = doc.update_frequency;
            h.parameter_sample = doc.parameter_sample;
            h.output_sample = doc.output_sample;
            h.contact = doc.contact;
            h.cate = doc.cate;
            h.cate_name = doc.cate_name;
            h.is_publish = doc.is_publish;
            h.source_cate = doc.source_cate;
            h.url_page = doc.url_page;
            h.remark = doc.remark;
            h.is_active = doc.is_active;

            return h;
        }

        public static I_BasicResult UpdateCateApiByListApi(List<string> list_apiid, string apicate)
        {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };

            try
            {
                using (cimsContext db = new cimsContext())
                {

                    foreach (var l in list_apiid)
                    {
                        var u = db.api_master.Where(o => o.api_id == l).FirstOrDefault();
                        if (u != null)
                        {
                            u.cate = apicate;
                            db.SaveChanges();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
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

    }
}
