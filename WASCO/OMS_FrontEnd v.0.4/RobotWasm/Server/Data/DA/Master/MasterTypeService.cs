using System.Data;
using static RobotWasm.Shared.Data.ML.Shared.I_Result;
using RobotWasm.Shared.Data.ML.Master.MasterType;
using RobotWasm.Server.Data.GaDB;
using RobotWasm.Shared.Data.GaDB;

namespace RobotWasm.Server.Data.DA.Master {
    public class MastertypeService {

        public I_MasterTypeSet DocSet { get; set; }

        #region Get List

        public static I_MasterTypeSet GetDocSet(string docid, string rcom) {
            I_MasterTypeSet n = new I_MasterTypeSet();
            using (GAEntities db = new GAEntities()) {
                n.Head = db.MasterTypeHead.Where(o => o.MasterTypeID == docid).FirstOrDefault();
                if (n.Head.UseFor == "ALL")
                {
                    n.Line = db.MasterTypeLine.Where(o => o.MasterTypeID == docid).OrderBy(o => o.Sort).ToList();
                }
                else
                {
                    n.Line = db.MasterTypeLine.Where(o => o.MasterTypeID == docid && o.RComID == rcom).OrderBy(o => o.Sort).ToList();
                }
            }
            return n;
        }

        public static I_MasterTypeSet SetActiveRow(I_MasterTypeSet doc, MasterTypeLine select)
        {
            doc.LineActive = doc.Line.Where(o => o.ValueTXT == select.ValueTXT).FirstOrDefault();
            return doc;
        }

        public MasterTypeHead GetMasterTypeHead(string typeId)
        {
            MasterTypeHead result = new MasterTypeHead();

            using (GAEntities db = new GAEntities())
            {
                result = db.MasterTypeHead.Where(o => o.MasterTypeID == typeId).FirstOrDefault();
            }
            return result;
        }
        public MasterTypeLine GetMasterLinebyValueTXT(string valuetxt, string rcom)
        {
            MasterTypeLine result = new MasterTypeLine();

            using (GAEntities db = new GAEntities())
            {
                result = db.MasterTypeLine.Where(o => o.ValueTXT == valuetxt && o.RComID == rcom).FirstOrDefault();
            }
            return result;
        }

        public MasterTypeLine GetType(string masterId, string valuetxt)
        {
            MasterTypeLine result = new MasterTypeLine();

            using (GAEntities db = new GAEntities())
            {
                result = db.MasterTypeLine.Where(o => o.MasterTypeID == masterId && o.ValueTXT == valuetxt).FirstOrDefault();
                if (result == null)
                {
                    result = new MasterTypeLine { MasterTypeID = masterId, ValueTXT = "", Description1 = "", Description2 = "", Description3 = "", Description4 = "" };
                }
            }
            return result;
        }


        public static List<MasterTypeLine> ListType(string rcom, string masId, bool isShowBlank)
        {
            List<MasterTypeLine> result = new List<MasterTypeLine>();
            using (GAEntities db = new GAEntities())
            {
                result = db.MasterTypeLine.Where(o => o.MasterTypeID == masId
                                                     && (o.RComID == rcom || o.RComID == "")
                                                        && o.IsActive == true).ToList();

                if (isShowBlank)
                {
                    MasterTypeLine n = new MasterTypeLine { MasterTypeID = "", ValueTXT = "", Description1 = "", Description2 = "", Description3 = "", Description4 = "" };
                    result.Insert(0, n);
                }
            }
            return result;
        }

        public static List<vw_MasterTypeLine> ListViewType(string rcom, string masId, bool isShowBlank) {
            List<vw_MasterTypeLine> result = new List<vw_MasterTypeLine>();
            using (GAEntities db = new GAEntities()) {
                result = db.vw_MasterTypeLine.Where(o => o.MasterTypeID == masId
                                                     && (o.RComID == rcom || o.RComID == "")
                                                        && o.IsActive == true).ToList();

                if (isShowBlank) {
                    vw_MasterTypeLine n = new vw_MasterTypeLine { MasterTypeID = "", ValueTXT = "", Description1 = "", Description2 = "", Description3 = "", Description4 = "" };
                    result.Insert(0, n);
                }
            }
            return result;
        }

        public static List<MasterTypeHead> ListDoc(string Search) {
            List<MasterTypeHead> result = new List<MasterTypeHead>();
            using (GAEntities db = new GAEntities()) {
                result = db.MasterTypeHead.Where(o =>
                                        (o.MasterTypeID.Contains(Search)
                                            || o.Name.Contains(Search)
                                            || Search == ""
                                            )
                                            && (o.IsActive == true)
                                            && o.UserAddNew == true
                                            ).OrderByDescending(o => o.MasterTypeID).ToList();

            }
            return result;
        }

        #endregion

        #region Save

        public static I_BasicResult Save(MasterTypeLine data)
        {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try
            {
                using (GAEntities db = new GAEntities())
                {
                    var uh = db.MasterTypeLine.Where(o => o.ValueTXT == data.ValueTXT && o.MasterTypeID == data.MasterTypeID && o.RComID == data.RComID).FirstOrDefault();
                    if (uh == null)
                    {
                        db.MasterTypeLine.Add(data);
                        db.SaveChanges();
                    }
                    else
                    {
                        uh.ValueTXT = data.ValueTXT;
                        uh.Description1 = data.Description1;
                        uh.Description2 = data.Description2;
                        uh.Description3 = data.Description3;
                        uh.Sort = data.Sort;
                        uh.RefID = data.RefID;
                        uh.IsActive = data.IsActive;
                        db.SaveChanges();
                    }
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
                    result.Message1 = ex.Message.ToLower();

                }
            }
            return result;
        }

        public static I_BasicResult ReOrder(List<MasterTypeLine> data)
        {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try
            {
                using (GAEntities db = new GAEntities())
                {
                    foreach (var d in data)
                    {
                        var query = db.MasterTypeLine.Where(o => o.MasterTypeID == d.MasterTypeID && o.RComID == d.RComID && o.ValueTXT == d.ValueTXT).FirstOrDefault();
                        if (query != null)
                        {
                            query.Sort = d.Sort;
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

        #endregion

        public static short GenSort() {
            short result = 1;
            try {
                using (GAEntities db = new GAEntities()) {
                    var h = db.MasterTypeLine.ToList();
                    var max_linenum = h.Max(o => o.Sort);
                    result = Convert.ToInt16(max_linenum + 1);
                }
            } catch { }
            return result;
        }

    }
}
