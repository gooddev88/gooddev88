using Blazored.SessionStorage;
using Newtonsoft.Json;
using Robot.Data.GADB.TT;
using Robot.Service.FileGo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using static Robot.Data.ML.I_Result;

namespace Robot.Data.DA.Document {
    public class DocumentGoService {
        public static string sessionActiveId = "activedocgoid";

        public I_DocGoSet DocSet { get; set; }
        ISessionStorageService sessionStorage;

        public DocumentGoService(ISessionStorageService _sessionStorage) {
            sessionStorage = _sessionStorage;
            DocSet = NewTransaction();
        }
        public class I_DocGoSet {
            public vw_doc_head Header { get; set; }
            public List<doc_tag> DocTag { get; set; }
            public List<doc_tag_person> DocPersonTag { get; set; }
            public List<vw_doc_approver> DocApprover { get; set; }
            public List<vw_XFilesRef> files { get; set; }
            public I_BasicResult OutputAction { get; set; }
        }
        public class I_DocGoFiterSet {
            public string Rcom { get; set; }
            public string Com { get; set; }
            public DateTime DateFrom { get; set; }
            public DateTime DateTo { get; set; }
            public string SearchBy { get; set; }
            public string SearchText { get; set; }
            public string DocType { get; set; }
            public bool ShowActive { get; set; }
        }

        #region Get / Query Transaction
        public I_DocGoSet GetDocSet(string docid, string rcom, string com) {
            I_DocGoSet n = NewTransaction();
            try {
                using (GAEntities db = new GAEntities()) {
                    n.Header = db.vw_doc_head.Where(o => o.doc_id == docid && o.rcom == rcom && o.com == com).FirstOrDefault();
                    n.DocApprover = db.vw_doc_approver.Where(o => o.doc_id == docid && o.rcom == rcom && o.com == com).ToList();
                    n.DocPersonTag = db.doc_tag_person.Where(o => o.doc_id == docid && o.rcom == rcom && o.com == com).ToList();
                    n.files = FileGo.ListFilesRef(n.Header.rcom, n.Header.com, n.Header.doc_type, n.Header.doc_id);
                    n.DocTag = db.doc_tag.Where(o => o.doc_id == docid && o.rcom == rcom && o.com == com).ToList();
                }
            } catch (Exception ex) {
                n.OutputAction = new I_BasicResult { Result = "fail", Message1 = ex.Message, Message2 = "" };
            }
            return n;
        }
        public static I_DocGoSet RefreshFile(I_DocGoSet doc) {
            try {
                var h = doc.Header;
                doc.files = FileGo.ListFilesRef(h.rcom, h.com, h.doc_type, h.doc_id);
            } catch (Exception ex) {
            }
            return doc;
        }
        public static List<vw_doc_head_with_user> ListDocHeadInUser(string search, string doctype, string rcom, string tagperson) {
            List<vw_doc_head_with_user> result = new List<vw_doc_head_with_user>();
            using (GAEntities db = new GAEntities()) {
                if (search != "") {
                    var ids = db.doc_tag.Where(o => o.keyword.Contains(search) && o.rcom == rcom).Select(o => o.doc_id).Distinct().ToList();
                    result = db.vw_doc_head_with_user.Where(o => ids.Contains(o.doc_id)
                                                                   && (o.doc_type == doctype || doctype == "")
                                                                   && o.rcom == rcom
                                                                   && o.TagPerson == tagperson
                                                               && o.is_active == true
                                                                          ).OrderByDescending(o => o.created_date).Take(500).ToList();
                } else {
                    result = db.vw_doc_head_with_user.Where(o =>
                                                (o.doc_type == doctype || doctype == "")
                                               && o.rcom == rcom
                                               && o.TagPerson == tagperson
                                           && o.is_active == true
                           ).Take(100).OrderByDescending(o => o.created_date).ToList();
                }

            }
            return result;
        }
        //public static List<string> ListUserInCompany(string username, string rcom) {

        //    List<string> result = new List<string>();
        //    try {
        //        string conStr = Globals.GAEntitiesConn;
        //        using (var connection = new SqlConnection(conStr)) {

        //            var procedure = "[SP_LIST_USERINCOMPANY]";
        //            var values = new { Username = username, RCompany = rcom };
        //            var query = connection.Query(procedure, values, commandType: CommandType.StoredProcedure).ToList();
        //            query.ForEach(q => result.Add(q.CompanyID));
        //        }
        //    } catch (Exception ex) {

        //    }
        //    return result;
        //}
        public static List<doc_type_info> ListDocType(string rcom) {
            List<doc_type_info> result = new List<doc_type_info>();
            using (GAEntities db = new GAEntities()) {
                result = db.doc_type_info.Where(o => o.rcom == rcom && o.is_active == true).OrderBy(o => o.sort).ToList();
            }
            return result;
        }
        public static List<vw_doc_head_with_approver> ListForApprove(string user_approver, string rcom,string com) {
            List<vw_doc_head_with_approver> result = new List<vw_doc_head_with_approver>();
            using (GAEntities db = new GAEntities()) {
                result = db.vw_doc_head_with_approver.Where(o => o.rcom == rcom && o.com==com && o.approved_by == user_approver && o.approved_status == "PENDING" && o.is_active == true).OrderByDescending(o => o.created_date).ToList();
            }
            return result;
        }
        public static List<vw_doc_head_with_user> ListForTagPersion(string user_tag, string rcom, string com) {
            List<vw_doc_head_with_user> result = new List<vw_doc_head_with_user>();
            using (GAEntities db = new GAEntities()) {
                result = db.vw_doc_head_with_user.Where(o => o.rcom == rcom && o.com == com && o.TagPerson == user_tag && o.is_read ==false && o.is_active == true).OrderByDescending(o => o.created_date).ToList();
            }
            return result;
        }
        //public static List<doc_tag> ListTagMaster(string rcom) {
        //    List<doc_tag> result = new List<doc_tag>();
        //    using (GAEntities db = new GAEntities()) {
        //        var query = db.doc_tag_info.Where(o => o.rcom == rcom).OrderByDescending(o => o.description).ToList();
        //        result = Convert2doc_tag(query); 
        //    } 
        //    return result;
        //}

        #endregion

        #region Save
        public static I_BasicResult Save(I_DocGoSet doc) {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                using (GAEntities db = new GAEntities()) {
                    doc = CalDocSet(doc);
                    var h = db.doc_head.Where(o => o.doc_id == doc.Header.doc_id && o.rcom == doc.Header.rcom && o.com == doc.Header.com).FirstOrDefault();
                    if (h == null) {
                        var data_head = Convert2DocHead(doc.Header);
                        var data_Approver = Convert2DocApprover(doc.DocApprover);
                        db.doc_head.Add(data_head);
                        db.doc_approver.AddRange(data_Approver);
                        db.doc_tag.AddRange(doc.DocTag);
                        db.doc_tag_person.AddRange(doc.DocPersonTag);
                        db.SaveChanges();
                    } else {
                        h.owner_id = doc.Header.owner_id;
                        h.doc_type = doc.Header.doc_type;
                        h.doc_cate = doc.Header.doc_cate;
                        h.doc_desc = doc.Header.doc_desc;
                        h.remark1 = doc.Header.remark1;
                        h.remark2 = doc.Header.remark2;
                        h.status = doc.Header.status;
                        h.modified_by = doc.Header.modified_by;
                        h.modified_date = DateTime.Now;
                        var data_Approver = Convert2DocApprover(doc.DocApprover);
                        db.doc_approver.RemoveRange(db.doc_approver.Where(o => o.rcom == h.rcom && o.com == h.com && o.doc_id == h.doc_id));
                        db.doc_approver.AddRange(data_Approver);
                        db.doc_tag.RemoveRange(db.doc_tag.Where(o => o.rcom == h.rcom && o.com == h.com && o.doc_id == h.doc_id));
                        db.doc_tag.AddRange(doc.DocTag);
                        db.doc_tag_person.RemoveRange(db.doc_tag_person.Where(o => o.rcom == h.rcom && o.com == h.com && o.doc_id == h.doc_id));
                        db.doc_tag_person.AddRange(doc.DocPersonTag);
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

        public static I_DocGoSet Adddoc_approver(I_DocGoSet docset, string approver_id, string created_by, string remark) {

            try {

                var h = docset.Header;

                using (GAEntities db = new GAEntities()) {
                    var chk_dup = docset.DocApprover.Where(o => o.approved_by == approver_id && o.is_active == true).FirstOrDefault();
                    if (chk_dup != null) {
                        docset.OutputAction.Result = "fail";
                        docset.OutputAction.Message1 = "Duplicate approver name.";
                        return docset;
                    }
                    var user_info = db.UserInfo.Where(o => o.Username == approver_id).FirstOrDefault();
                    if (user_info == null) {
                        docset.OutputAction.Result = "fail";
                        docset.OutputAction.Message1 = "Approver not found.";
                        return docset;
                    }
                    var approver = DocumentGoService.NewApprover(docset);
                    approver.approved_by_name = user_info.FullName;
                    approver.doc_id = h.doc_id;
                    approver.approved_by = approver_id;
                    approver.approved_date = null;
                    approver.remark1 = remark;
                    approver.rcom = h.rcom;
                    approver.com = h.com;
                    approver.created_by = created_by;
                    docset.DocApprover.Add(approver);

                }


            } catch (Exception ex) {
                docset.OutputAction.Result = "fail";
                if (ex.InnerException != null) {
                    docset.OutputAction.Message1 = ex.InnerException.ToString();
                } else {
                    docset.OutputAction.Message1 = ex.Message;
                }
            }
            return docset;
        }


        public static I_DocGoSet Delete_doc_approver(I_DocGoSet docset, string approver_id, string delete_by) {

            try {

                var h = docset.Header;

                using (GAEntities db = new GAEntities()) {
                    var chk_dup = docset.DocApprover.Where(o => o.approved_by == approver_id && o.is_active == true).FirstOrDefault();
                    if (chk_dup != null) {
                        chk_dup.modified_by = delete_by;
                        chk_dup.modified_date = DateTime.Now;
                        chk_dup.is_active = false;
                    }
                }


            } catch (Exception ex) {
                docset.OutputAction.Result = "fail";
                if (ex.InnerException != null) {
                    docset.OutputAction.Message1 = ex.InnerException.ToString();
                } else {
                    docset.OutputAction.Message1 = ex.Message;
                }
            }
            return docset;
        }
        public static I_BasicResult Adddoc_tag(doc_tag data) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                using (GAEntities db = new GAEntities()) {
                    db.doc_tag.Add(data);
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
        public static I_BasicResult SetApproverRead(vw_doc_head_with_approver data) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                using (GAEntities db = new GAEntities()) {
                    var query = db.doc_approver.Where(o => o.rcom == data.rcom && o.doc_id == data.doc_id).FirstOrDefault();
                    if (query != null) {
                        query.is_read = true;
                        query.read_date = DateTime.Now;
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
     public static doc_tag_person SetTagPersionUnRead(string rcom,string docid, string user) {
            doc_tag_person result = null;
            try {
                using (GAEntities db = new GAEntities()) {
                    var query = db.doc_tag_person.Where(o => o.rcom == rcom && o.doc_id == docid && o.userid==user).FirstOrDefault();
                    if (query != null) {
                        query.is_read = false;
                        query.read_date = null;
                        db.SaveChanges();
                        result = query;
                    }

                }
            } catch (Exception ex) {
              
            }
            return result;
        }
        public static I_BasicResult SetTagPersonRead(vw_doc_head_with_user data) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                using (GAEntities db = new GAEntities()) {
                    var query = db.doc_tag_person.Where(o => o.rcom == data.rcom && o.doc_id==data.doc_id).FirstOrDefault();
                    if (query != null) {
                        query.is_read = true;
                        query.read_date = DateTime.Now;
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

        #region Delete
        //public static I_BasicResult DeleteLine(int id) {
        //    I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
        //    try {
        //        using (GAEntities db = new GAEntities()) {
        //            db.doc_head.Remove(db.doc_head.Where(o => o.id == id).FirstOrDefault());
        //            db.SaveChanges();
        //        }
        //    } catch (Exception ex) {
        //        result.Result = "fail";
        //        if (ex.InnerException != null) {
        //            result.Message1 = ex.InnerException.ToString();
        //        } else {
        //            result.Message1 = ex.Message;
        //        }
        //    }
        //    return result;
        //}
        #endregion

        #region new / convert / caldocset
        public static I_DocGoSet CalDocSet(I_DocGoSet doc) {
            var h = doc.Header;


            doc.DocTag.RemoveAll(o => o.tag == "");
            doc.DocTag.Add(NewTag(h.doc_id));
            doc.DocTag.Add(NewTag(h.owner_name));
            doc.DocTag.Add(NewTag(h.doc_desc));
            doc.DocTag.Add(NewTag(h.doc_type));
            doc.DocTag.Add(NewTag(h.remark1));
            doc.DocTag.Add(NewTag(h.remark2));


            foreach (var l in doc.DocApprover) {
                //add approve in tagperson
                var chkInPersontag = doc.DocPersonTag.Where(o => o.userid == l.approved_by).FirstOrDefault();
                if (chkInPersontag == null) {
                    doc_tag_person tag = DocumentGoService.NewPersonTag();
                    tag.userid = l.approved_by;
                    tag.created_by = l.created_by;
                    doc.DocPersonTag.Add(tag);
                }

                l.doc_id = h.doc_id;
                l.rcom = h.rcom;
                l.com = h.com;
                doc.DocTag.Add(NewTag(l.approved_by_name));
            }
            //add owner in tagperson
            var chkOwnerInPersontag = doc.DocPersonTag.Where(o => o.userid == h.owner_id).FirstOrDefault();
            if (chkOwnerInPersontag == null) {
                doc_tag_person tag = DocumentGoService.NewPersonTag();
                tag.userid = h.owner_id;
                tag.created_by = h.created_by;
                doc.DocPersonTag.Add(tag);
            }

            foreach (var l in doc.DocPersonTag) {
                l.doc_id = h.doc_id;
                l.rcom = h.rcom;
                l.com = h.com;
                l.created_by = h.created_by;
                l.created_date = DateTime.Now;
            }
            foreach (var l in doc.DocTag) {
                l.doc_id = h.doc_id;
                l.rcom = h.rcom;
                l.com = h.com;
                l.created_by = h.created_by;
                l.created_date = DateTime.Now;
            }
            doc.DocTag.RemoveAll(o => o.keyword == "");


            //update latest approve 
            var chk_last_appv = doc.DocApprover.Where(o => o.approved_status != "PENDING").OrderByDescending(o => o.approved_date).FirstOrDefault();
            if (chk_last_appv != null) {
                h.latest_approved_by = chk_last_appv.approved_by;
                h.latest_approved_date = chk_last_appv.approved_date;
                h.status = chk_last_appv.approved_status;
            }
            return doc;
        }

        async public void SetSessionDocGoFiterSet(I_DocGoFiterSet data) {
            JsonSerializerOptions jso = new JsonSerializerOptions();
            jso.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
            string json = System.Text.Json.JsonSerializer.Serialize(data, jso);
            await sessionStorage.SetItemAsync("docgo_Fiter", json);
        }
        async public Task<I_DocGoFiterSet> GetSessionDocGoFiterSet() {
            I_DocGoFiterSet result = NewFilterSet();
            var strdoc = await sessionStorage.GetItemAsync<string>("docgo_Fiter");

            if (strdoc != null) {
                return JsonConvert.DeserializeObject<I_DocGoFiterSet>(strdoc);
            } else {
                return null;
            }
        }

        public static I_DocGoFiterSet NewFilterSet() {
            I_DocGoFiterSet n = new I_DocGoFiterSet();

            n.Rcom = "";
            n.Com = "";
            n.DocType = "";
            n.DateFrom = DateTime.Now.Date.AddDays(-7);
            n.DateTo = DateTime.Now.Date;
            n.SearchBy = "DOCDATE";
            n.SearchText = "";
            n.ShowActive = true;
            return n;
        }


        public static I_DocGoSet NewTransaction() {
            I_DocGoSet n = new I_DocGoSet();
            n.Header = NewHead();
            n.DocApprover = new List<vw_doc_approver>();
            n.DocTag = new List<doc_tag>();
            n.DocPersonTag = new List<doc_tag_person>();
            n.files = new List<vw_XFilesRef>();
            n.OutputAction = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            return n;
        }

        public static vw_doc_head NewHead() {
            vw_doc_head n = new vw_doc_head();
            n.rcom = "";
            n.com = "";
            n.doc_id = "";
            n.owner_id = "";
            n.owner_name = "";
            n.doc_type = "";
            n.doc_cate = "";
            n.doc_desc = "";
            n.remark1 = "";
            n.remark2 = "";
            n.status = "PENDING";
            n.latest_approved_by = "";
            n.latest_approved_date = null;
            n.created_by = "";
            n.created_date = DateTime.Now;
            n.modified_by = "";
            n.modified_date = null;
            n.is_active = true;
            return n;
        }


        public static vw_doc_approver NewApprover(I_DocGoSet docset) {
            vw_doc_approver n = new vw_doc_approver();
            int seq = 1;
            var next_seq = docset.DocApprover.OrderByDescending(o => o.approved_seq).FirstOrDefault();
            if (next_seq != null) {
                seq = next_seq.approved_seq + 1;
            }
            n.rcom = "";
            n.com = "";
            n.doc_id = "";
            n.approved_by_name = "";
            n.approved_seq = seq;
            n.approved_by = "";
            n.approved_status = "PENDING";
            n.approved_date = null;
            n.is_read = false;
            n.remark1 = "";
            n.remark2 = "";
            n.created_by = "";
            n.created_date = DateTime.Now;
            n.modified_by = "";
            n.modified_date = null;
            n.is_active = true;
            return n;
        }
        public static doc_tag NewTag(string keyword) {
            doc_tag n = NewTag();
            n.keyword = keyword;
            return n;
        }
        public static doc_tag NewTag() {
            doc_tag n = new doc_tag();
            n.rcom = "";
            n.com = "";
            n.doc_id = "";
            n.tag = "";
            n.keyword = "";
            n.created_by = "";
            n.created_date = DateTime.Now;
            n.is_active = true;
            return n;
        }
        public static doc_tag_person NewPersonTag() {
            doc_tag_person n = new doc_tag_person();
            n.id = 0;
            n.rcom = "";
            n.com = "";
            n.doc_id = "";
            n.userid = "";
            n.is_read = false;
            n.created_by = "";
            n.created_date = DateTime.Now;
            n.is_active = true;

            return n;
        }
        public static doc_head Convert2DocHead(vw_doc_head i) {
            doc_head n = new doc_head();
            n.rcom = i.rcom;
            n.com = i.com;
            n.doc_id = i.doc_id;
            n.owner_id = i.owner_id;
            n.doc_type = i.doc_type;
            n.doc_cate = i.doc_cate;
            n.doc_desc = i.doc_desc;
            n.remark1 = i.remark1;
            n.remark2 = i.remark2;
            n.status = i.status;
            n.latest_approved_by = i.latest_approved_by;
            n.latest_approved_date = i.latest_approved_date;
            n.created_by = i.created_by;
            n.created_date = i.created_date;
            n.modified_by = i.modified_by;
            n.modified_date = i.modified_date;
            n.is_active = i.is_active;
            return n;
        }
        public static List<doc_approver> Convert2DocApprover(List<vw_doc_approver> ii) {
            List<doc_approver> oo = new List<doc_approver>();
            foreach (var i in ii) {
                doc_approver n = new doc_approver();
                n.rcom = i.rcom;
                n.com = i.com;
                n.doc_id = i.doc_id;
                n.approved_seq = i.approved_seq;
                n.approved_by = i.approved_by;
                n.approved_status = i.approved_status;
                n.approved_date = i.approved_date;
                n.remark1 = i.remark1;
                n.remark2 = i.remark2;
                n.is_read = i.is_read;
                n.created_by = i.created_by;
                n.created_date = i.created_date;
                n.modified_by = i.modified_by;
                n.modified_date = i.modified_date;
                n.is_active = i.is_active;
                oo.Add(n);
            }

            return oo;
        }

        #endregion
    }
}
