using Microsoft.EntityFrameworkCore;
using RobotWasm.Server.Data.CimsDB.TT;
using RobotWasm.Server.Data.DA.LogTran;
using RobotWasm.Server.Data.GaDB;
using RobotWasm.Shared.Data.DimsDB;
using RobotWasm.Shared.Data.GaDB;
using RobotWasm.Shared.Data.ML.DocHead;
using RobotWasm.Shared.Data.ML.DPMBaord.BoardData;
using static RobotWasm.Shared.Data.ML.Shared.I_Result;

namespace RobotWasm.Server.Data.DA.Q {
    public class DocumentService {

        public static I_DocHeadSet GetDocSet(string docid) {
            I_DocHeadSet doc = new I_DocHeadSet();
            try {
                using (cimsContext db = new cimsContext()) {
                    doc.head = db.doc_head.Where(o => o.doc_id == docid).FirstOrDefault();
                    doc.files = db.vw_xfile_ref.Where(o => o.doc_id == docid && o.rcom_id == "DPM" && o.com_id == "" && o.doctype == "PUBLISH_DOCUMENT" && o.is_active == 1).ToList();
                }
            } catch (Exception ex) {

            }
            return doc;
        }

        public static List<vw_doc_head> ListDocument(I_DocHeadFiterSet f)
        {
            List<vw_doc_head> result = new List<vw_doc_head>();
            using (cimsContext db = new cimsContext())
            {
                if (!string.IsNullOrEmpty(f.SearchText))
                {
                    result = db.vw_doc_head.Where(o =>
                        (o.doc_id.Contains(f.SearchText)
                                    || o.doc_desc.Contains(f.SearchText)
                                    || o.doc_cate_id.Contains(f.SearchText)
                                    || o.doc_type_id.Contains(f.SearchText)
                                    || f.SearchText == ""
                            )
                            && (o.is_publish == f.IsPublish || f.IsPublish == -1)
                            && (o.doc_cate_id == f.Cate || f.Cate == "")
                            && (o.is_active == 1)
                            ).OrderByDescending(o => o.created_date).ToList();
                }
                else
                {
                    if (f.DateFrom == null || f.DateTo == null)
                    {
                        result = db.vw_doc_head.Where(o =>
                            (o.is_publish == f.IsPublish || f.IsPublish == -1)
                            && (o.doc_cate_id == f.Cate || f.Cate == "")
                            && (o.is_active == 1)
                            ).OrderByDescending(o => o.created_date).ToList();
                    }
                    else
                    {
                        result = db.vw_doc_head.Where(o =>
                                   (o.doc_cate_id == f.Cate || f.Cate == "")
                                   && o.created_date_not_time >= f.DateFrom && o.created_date_not_time <= f.DateTo
                                   && (o.is_publish == f.IsPublish || f.IsPublish == -1)
                                   && o.is_active == 1
                             ).OrderByDescending(o => o.created_date).ToList();
                    }
                }
            }
            return result;
        }

        #region save

        public static I_BasicResult Save(I_DocHeadSet doc, bool isnew) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                using (cimsContext db = new cimsContext()) {

                    if (isnew) {
                        db.doc_head.Add(doc.head);
                        db.SaveChanges();
                        var rs = LogTranService.CreateTransLog(doc.head.doc_id,doc.head.created_by, "จัดการเอกสารเผยแพร่", "เพิ่มเอกสารเผยแพร่");
                    } else {
                        var n = db.doc_head.Where(o => o.doc_id == doc.head.doc_id).FirstOrDefault();

                        n.doc_desc = doc.head.doc_desc;
                        n.doc_cate_id = doc.head.doc_cate_id;
                        n.doc_remark = doc.head.doc_remark;
                        n.publish_date = doc.head.publish_date;
                        n.is_publish = doc.head.is_publish;
                        n.count_file = doc.head.count_file;
                        n.modified_by = doc.head.modified_by;
                        n.modified_date = DateTime.Now;

                        db.SaveChanges();
                        var rs = LogTranService.CreateTransLog(doc.head.doc_id, doc.head.modified_by, "จัดการเอกสารเผยแพร่", "แก้ไขเอกสารเผยแพร่");
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

        public static I_DocHeadSet GetLatestFiles(I_DocHeadSet doc) {
            try {
                using (cimsContext db = new cimsContext()) {
                    doc.files = db.vw_xfile_ref.Where(o => o.doc_id == doc.head.doc_id && o.rcom_id == "DPM" && o.com_id == "" && o.doctype == "PUBLISH_DOCUMENT" && o.is_active == 1).ToList();

                }
            } catch (Exception ex) {

            }
            return doc;
        }

        public static I_BasicResult DeleteDoc(string docId, string modified_by) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                using (cimsContext db = new cimsContext()) {
                    var head = db.doc_head.Where(o => o.doc_id == docId).FirstOrDefault();

                    head.modified_by = modified_by;
                    head.modified_date = DateTime.Now;
                    head.is_active = 0;

                    db.SaveChanges();
                    var rs = LogTranService.CreateTransLog(docId, modified_by, "จัดการเอกสารเผยแพร่", "ลบเอกสารเผยแพร่");
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

        public static List<vw_publishdoc_cate> ListDataCategory() {
            List<vw_publishdoc_cate> result = new List<vw_publishdoc_cate>();
            using (cimsContext db = new cimsContext()) {
                result = db.vw_publishdoc_cate.Where(o => o.is_active == 1).OrderBy(o => o.sort).ToList(); 
            }
            return result;
        }


        public static ListDocumentHeadDocSet GetLisDocumentHeadDoc(string cateid)
        {
            ListDocumentHeadDocSet doc = new ListDocumentHeadDocSet();
            try
            {
                using (cimsContext db = new cimsContext())
                {
                    doc.ListHead = new List<xdoc_head>();
                    doc.files = new List<vw_xfile_ref>();
                    var data = db.doc_head.Where(o => o.doc_cate_id == cateid && o.is_publish == 1 && o.is_active == 1).OrderByDescending(o => o.publish_date).ToList();
                    foreach (var l in data)
                    {
                        doc.ListHead.Add(ConvertHead2XHead(l));
                    }

                    if (data.Count() > 0)
                    {
                        var Firstdata = doc.ListHead.FirstOrDefault();
                        Firstdata.IsVisible = true;
                        var list_docid = doc.ListHead.Select(o => o.doc_id).ToList();
                        doc.files = db.vw_xfile_ref.Where(o => list_docid.Contains(o.doc_id) && o.rcom_id == "DPM" && o.com_id == "" && o.doctype == "PUBLISH_DOCUMENT" && o.is_active == 1).ToList();
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return doc;
        }

        public static xdoc_head ConvertHead2XHead(doc_head input)
        {
            xdoc_head n = new xdoc_head();
            n.IsVisible = false;
            n.id = input.id;
            n.doc_id = input.doc_id;
            n.doc_desc = input.doc_desc;
            n.doc_cate_id = input.doc_cate_id;
            n.doc_type_id = input.doc_type_id;
            n.doc_remark = input.doc_remark;
            n.publish_date = input.publish_date;
            n.is_publish = input.is_publish;
            n.count_file = input.count_file;
            n.modified_by = input.modified_by;
            n.modified_date = input.modified_date;
            n.is_active = input.is_active;
            return n;
        }

        public static I_DocHeadSet NewTransaction() {
            I_DocHeadSet n = new I_DocHeadSet();
            n.head = NewHead();
            n.files = new List<vw_xfile_ref>();
            return n;
        }

        public static doc_head NewHead() {
            doc_head n = new doc_head();

            n.id = 0;
            n.doc_id = "";
            n.doc_desc = "";
            n.doc_type_id = "publish document";
            n.doc_cate_id = "";
            n.doc_remark = "";
            n.publish_date = null;
            n.is_publish = 0;
            n.created_by = "";
            n.created_date = DateTime.Now;
            n.modified_by = "";
            n.modified_date = null;
            n.count_file = 0;
            n.is_active = 1;

            return n;
        }

    }
}
