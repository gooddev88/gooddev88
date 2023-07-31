using AutoMapper;
using Robot.Data.GADB.TT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Robot.Data.DA.POSSY.POSSaleConverterService;

namespace Robot.Data.DA.POSSY {
    public class TableInfoService {
        
        public TableInfoService( ) {
            
        }
        
        
        public  class POS_TableModel : POS_Table {
            public string Image { get; set; }
        }

        public static List<POS_TableModel> ListSelectTable(string rcom, string com)
        {
            List<POS_TableModel> result = new List<POS_TableModel>();
            using (GAEntities db = new GAEntities())
            {
                var q = db.POS_Table.Where(o => o.RComID == rcom
                                                && o.ComID == com
                                                   && o.IsActive
                                              ).ToList();

                result = ConvertTable2Model(q);
            }
            return result;
        }

        public static List<POS_TableModel> ListTable(string rcom,string com) {
            List<POS_TableModel> result = new List<POS_TableModel>();
            List<string> exclude = new List<string> { "", "T-000" };
            using (GAEntities db = new GAEntities()) {
                var reserveTable = db.POS_SaleHead.Where(o => o.IsActive == true
                                                                 && o.INVID == ""
                                                                 && o.RComID == rcom
                                                                 && o.ComID == com                                                                
                                                                 && !exclude.Contains( o.TableID ) ).Select(o => o.TableID).ToList();
                var q = db.POS_Table.Where(o => o.RComID == rcom
                                                && o.ComID==com
                                                   && o.IsActive
                                              ).ToList();
                q = q.Where(o => !reserveTable.Contains(o.TableID)).ToList();
                result = ConvertTable2Model(q);
            }
            return result;
        }
        public static POS_TableModel GetTableInfo(string tableId, string rcom, string com)
        {
            POS_TableModel result = new POS_TableModel();
            using (GAEntities db = new GAEntities())
            {
           
                var q = db.POS_Table.Where(o => o.RComID == rcom
                                                && o.ComID == com
                                                && o.TableID ==tableId
                                              ).ToList();

                result = ConvertTable2Model(q).FirstOrDefault() ; 
            }
            return result;
        }
        public static List<POS_TableModel> ConvertTable2Model(List<POS_Table> input) {


            var destination = new List<POS_TableModel>();
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<POS_Table, POS_TableModel>();
            });
            IMapper iMapper = config.CreateMapper();
            foreach (var i in input) {
                var n = iMapper.Map<POS_Table, POS_TableModel>(i);
                if (n.TableName.Contains("กลับ")) {
                    n.Image = "/SALE/assets/img/home-pos.png";
                } else {
                    n.Image = "/SALE/assets/img/food-table-pos.png";
                }
                destination.Add(n);
            }

            return destination;
        }
    }
}
