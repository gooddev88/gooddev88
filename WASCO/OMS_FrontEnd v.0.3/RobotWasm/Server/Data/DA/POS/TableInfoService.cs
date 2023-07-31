using System.Data;
using static RobotWasm.Shared.Data.ML.Shared.I_Result;
using RobotWasm.Shared.Data.GaDB;
using RobotWasm.Server.Data.GaDB;
using static RobotWasm.Client.Data.DA.Master.CompanyService;
using RobotWasm.Shared.Data.ML.Login;
using RobotWasm.Shared.Data.ML.POS;

namespace RobotWasm.Server.Data.DA.POS {
    public class TableInfoService {

        #region Get List

        public static List<POS_TableModel> ListSelectTable(string rcom, string com) {
            List<POS_TableModel> result = new List<POS_TableModel>();
            using (GAEntities db = new GAEntities()) {
                var q = db.POS_Table.Where(o => o.RComID == rcom
                                                && o.ComID == com
                                                   && o.IsActive
                                              ).ToList();

                result = ConvertTable2Model(q);
            }
            return result;
        }

        public static List<POS_TableModel> ListTable(string rcom, string com) {
            List<POS_TableModel> result = new List<POS_TableModel>();
            List<string> exclude = new List<string> { "", "T-000", "T000" };
            using (GAEntities db = new GAEntities()) {
                var reserveTable = db.POS_SaleHead.Where(o => o.IsActive == true
                                                                 && o.INVID == ""
                                                                 && o.RComID == rcom
                                                                 && o.ComID == com
                                                                 && !exclude.Contains(o.TableID)).Select(o => o.TableID).ToList();
                var q = db.POS_Table.Where(o => o.RComID == rcom
                                                && o.ComID == com
                                                   && o.IsActive
                                              ).ToList();
                q = q.Where(o => !reserveTable.Contains(o.TableID)).ToList();
                result = ConvertTable2Model(q);
            }
            return result;
        }
        public static POS_TableModel GetTableInfo(string tableId, string rcom, string com) {
            POS_TableModel result = new POS_TableModel();
            using (GAEntities db = new GAEntities()) {

                var q = db.POS_Table.Where(o => o.RComID == rcom
                                                && o.ComID == com
                                                && o.TableID == tableId
                                              ).ToList();

                result = ConvertTable2Model(q).FirstOrDefault();
            }
            return result;
        }

        public static List<POS_TableModel> ConvertTable2Model(List<POS_Table> input) {

            List<POS_TableModel> result = new List<POS_TableModel>();

            foreach (var l in input) {
                POS_TableModel n = new POS_TableModel();

                n.RComID = l.RComID;
                n.ComID = l.ComID;
                n.TableID= l.TableID;
                n.TableName= l.TableName;
                n.Sort = l.Sort;
                n.IsActive= l.IsActive;
                if (n.TableName.Contains("กลับ")) {
                    n.Image = "img/togohome_logo.png";
                } else {
                    n.Image = "img/market-pos.png";
                }
                result.Add(n);
            }
            return result;
        }

        #endregion

    }
}
