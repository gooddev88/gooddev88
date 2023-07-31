using RobotWasm.Server.Data.CimsDB.TT;
using RobotWasm.Server.Data.DA.LogTran;
using RobotWasm.Server.Data.GaDB;
using RobotWasm.Shared.Data.DimsDB;
using RobotWasm.Shared.Data.GaDB;
using RobotWasm.Shared.Data.ML.PublishDoc;
using static RobotWasm.Shared.Data.ML.Shared.I_Result;

namespace RobotWasm.Server.Data.DA.PublishDocument {
    public class PublishDocService {

        public static I_PublishDoc_DocSet GetDocSet(string docid) {
            I_PublishDoc_DocSet doc = new I_PublishDoc_DocSet();
            try {
                using (cimsContext db = new cimsContext()) {
                    doc.head = db.publish_doc_head.Where(o => o.data_key == docid).FirstOrDefault();
                    doc.line = db.vw_publish_doc_line.Where(o => o.data_key == docid).ToList();
                    doc.files = db.vw_xfile_ref.Where(o => o.doc_id == docid && o.rcom_id == "DPM" && o.com_id == "" && o.doctype == "PUBLISH_DOC" && o.is_active == 1).ToList();
                }
            } catch (Exception ex) {

            }
            return doc;
        }

        public static ListPublishDocSet GetListPublishDoc() {
            ListPublishDocSet doc = new ListPublishDocSet();
            try {
                using (cimsContext db = new cimsContext()) {
                    doc.ListHead = new List<xpublish_doc_head>();
                    doc.ListLine = new List<vw_publish_doc_line>();
                    var data = db.publish_doc_head.ToList();
                    foreach (var l in data) {
                        doc.ListHead.Add(ConvertHead2XHead(l));
                    }

                    var Firstdata = doc.ListHead.Where(o => o.id == 1).FirstOrDefault();
                    Firstdata.IsVisible = true;

                    var manual = doc.ListHead.Where(o => o.data_key == "data_manual").FirstOrDefault();
                    manual.IsVisible = true;

                    doc.ListLine = db.vw_publish_doc_line.ToList();
                }
            } catch (Exception ex) {

            }
            return doc;
        }

        

        public static List<xpublish_doc_head> ListDocHead()
        {
            List<xpublish_doc_head> result = new List<xpublish_doc_head>();
            using (cimsContext db = new cimsContext())
            {
                var data = db.publish_doc_head.ToList();
                foreach (var l in data) {
                    result.Add(ConvertHead2XHead(l));
                }

                var Firstdata = result.Where(o => o.id == 1).FirstOrDefault();
                Firstdata.IsVisible = true;
            }
            return result;
        }

        public static xpublish_doc_head ConvertHead2XHead(publish_doc_head input) {
            xpublish_doc_head n = new xpublish_doc_head();
            n.IsVisible = false;
            n.id = input.id;
            n.title = input.title;
            n.data_key = input.data_key;
            n.message = input.message;
            n.count_file = input.count_file;
            n.modified_by = input.modified_by;
            n.modified_date = input.modified_date;
            return n;
        }

        public static List<vw_publish_doc_line> ListDocLine() {
            List<vw_publish_doc_line> result = new List<vw_publish_doc_line>();
            using (cimsContext db = new cimsContext()) {
                result = db.vw_publish_doc_line.ToList();
            }
            return result;
        }

        public static publish_doc_line GetDocLine(string docid) {
            publish_doc_line result = new publish_doc_line();
            using (cimsContext db = new cimsContext()) {
                result = db.publish_doc_line.Where(o => o.file_id == docid).FirstOrDefault();
            }
            return result;
        }

        #region save

        public static I_BasicResult Save(I_PublishDoc_DocSet doc, bool action) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                using (cimsContext db = new cimsContext()) {

                    if (action) {
                        var checkdup = db.publish_doc_head.Where(o => o.data_key == doc.head.data_key).FirstOrDefault();
                        if (checkdup != null) {
                            result.Result = "fail";
                            result.Message1 = "รหัส เอกสารนี้มีในระบบแล้ว";
                        }else {
                            db.publish_doc_head.Add(doc.head);
                            db.SaveChanges();
                        }
                    } else {
                        var n = db.publish_doc_head.Where(o => o.data_key == doc.head.data_key).FirstOrDefault();

                        n.title = doc.head.title;
                        n.message = doc.head.message;
                        n.count_file = doc.head.count_file;
                        n.modified_by = doc.head.modified_by;
                        n.modified_date = DateTime.Now;

                        List<publish_doc_line> lines = new List<publish_doc_line>();
                        foreach (var l in doc.line) {
                            l.modified_by = doc.head.modified_by;
                            lines.Add(Convertvw2publishdocline(l));
                        }
                        db.publish_doc_line.RemoveRange(db.publish_doc_line.Where(o => o.data_key == n.data_key));
                        db.publish_doc_line.AddRange(lines);

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

        public static I_BasicResult SaveDocLine(publish_doc_line doc, bool action) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                using (cimsContext db = new cimsContext()) {
                    if (action) {
                        db.publish_doc_line.Add(doc);
                        db.SaveChanges();
                    } else {
                        var n = db.publish_doc_line.Where(o => o.file_id == doc.file_id).FirstOrDefault();

                        n.file_description = doc.file_description;
                        n.modified_by = doc.modified_by;
                        n.modified_date = DateTime.Now;
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

        public static publish_doc_line Convertvw2publishdocline(vw_publish_doc_line input) {
            publish_doc_line n = new publish_doc_line();
            n.data_key = input.data_key;
            n.file_description = input.file_description;
            n.file_id = input.file_id;
            n.modified_by = input.modified_by;
            n.modified_date = DateTime.Now;
            n.sort = input.sort;
            return n;
        }

        #endregion

        public static I_BasicResult DeleteDocLine(string docId) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                using (cimsContext db = new cimsContext()) {
                    db.publish_doc_line.RemoveRange(db.publish_doc_line.Where(o => o.file_id == docId));
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

        //public static I_PublishDoc_DocSet GetLatestFiles(I_PublishDoc_DocSet doc)
        //{
        //    try
        //    {
        //        using (cimsContext db = new cimsContext())
        //        {
        //            doc.files = db.vw_xfile_ref.Where(o => o.doc_id == doc.head.data_key && o.rcom_id == "DPM" && o.com_id == "" && o.doctype == "PUBLISH_DOC" && o.is_active == 1).ToList();
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    return doc;
        //}

        public static short GenSort() {
            short result = 1;
            try {
                using (cimsContext db = new cimsContext()) {
                    var h = db.publish_doc_line.ToList();
                    var max_linenum = h.Max(o => o.sort);
                    result = Convert.ToInt16(max_linenum + 1);
                }
            } catch { }
            return result;
        }


    }
}
