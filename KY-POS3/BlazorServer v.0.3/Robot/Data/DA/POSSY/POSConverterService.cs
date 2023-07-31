using AutoMapper;
using DevExpress.Blazor.Internal;
using Robot.Data.GADB.TT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Robot.Data.DA.POSSY
{
    public class POSConverterService
    {
        POSService posService;
        ShipToService shiptoService;
        TableInfoService tableService;

        public POSConverterService(POSService _posService, ShipToService _shiptoService, TableInfoService _tableService) {
            this.posService = _posService;
            this.shiptoService = _shiptoService; 
            this.tableService=_tableService;
        }
 

        public class POS_SaleHeadModel : POS_SaleHead {
            public String ImageUrlShipTo { get; set; } = "frontstore.png";
            public String TableName { get; set; }

        }
        public class POS_SaleLineModel : POS_SaleLine {
            //public ICommand SelectCommand { get; private set; }
            public String ImageUrl { get; set; }
            public string ImageSource { get; set; }
        }
        public class POS_SalePaymentModel : POS_SalePayment {
            
            public string PaymentTypeName { get; set; }
        }

 
        public static POS_SaleHead ConvertHead2DB(POS_SaleHeadModel input) {
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<POS_SaleHeadModel, POS_SaleHead>();
            });
            IMapper iMapper = config.CreateMapper();
            var destination = iMapper.Map<POS_SaleHeadModel, POS_SaleHead>(input);
            return destination;
        }
        public static List<POS_SaleLine> ConvertLine2DB(List<POS_SaleLineModel> input) {
            var destination = new List<POS_SaleLine>();
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<POS_SaleLineModel, POS_SaleLine>();
            });
            IMapper iMapper = config.CreateMapper();
            foreach (var i in input) {
                destination.Add(iMapper.Map<POS_SaleLineModel, POS_SaleLine>(i));
            }

            return destination;
        }
        public static List<POS_SalePayment> ConvertPayment2DB(List<POS_SalePaymentModel> input) {
            var destination = new List<POS_SalePayment>();
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<POS_SalePaymentModel, POS_SalePayment>();
            });
            IMapper iMapper = config.CreateMapper();
            foreach (var i in input) {
                destination.Add(iMapper.Map<POS_SalePaymentModel, POS_SalePayment>(i));
            }

            return destination;
        }

        public   POS_SaleHeadModel ConvertHead2Model(POS_SaleHead input) {

            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<POS_SaleHead, POS_SaleHeadModel>();
            });
            IMapper iMapper = config.CreateMapper();
            var destination = iMapper.Map<POS_SaleHead, POS_SaleHeadModel>(input);

            destination.ImageUrlShipTo = "frontstore.png";
            var shipToInfo = ShipToService.ListShipTo().Where(o => o.ShipToID == destination.ShipToLocID).FirstOrDefault();
            if (shipToInfo != null) {
                destination.ImageUrlShipTo = shipToInfo.ImageUrl;
            }
            //if (destination.IsLink==true) {
            //    destination.Remark2 = "online_logo";
            //} else {
            //    destination.Remark2 = "offline_logo";
            //} 
            return destination;
        }

        public POS_SaleHeadModel ConvertVW_Head2Model(POS_SaleHead input)
        {

            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<POS_SaleHead, POS_SaleHeadModel>();
            });
            IMapper iMapper = config.CreateMapper();
            var destination = iMapper.Map<POS_SaleHead, POS_SaleHeadModel>(input);

            //} 
            return destination;
        }

        public   POS_SaleHeadModel ConvertHead2ModelDisplay(POS_SaleHead input) {

            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<POS_SaleHead, POS_SaleHeadModel>();
            });
            IMapper iMapper = config.CreateMapper();
            var destination = iMapper.Map<POS_SaleHead, POS_SaleHeadModel>(input);

            destination.ImageUrlShipTo = "frontstore.png";
            var shipToInfo = ShipToService.ListShipTo().Where(o => o.ShipToID == destination.ShipToLocID).FirstOrDefault();
            if (shipToInfo != null) {
                destination.ImageUrlShipTo = shipToInfo.ImageUrl;
            }
            var tab = TableInfoService.ListTable(destination.RComID,destination.ComID).Where(o => o.TableID == destination.TableID).FirstOrDefault();
            if (tab != null) {
                destination.Remark1 = tab.Image;
                destination.TableName = tab.TableName;
            }
            if (destination.IsLink == true) {
                destination.Remark2 = "online_logo";
            } else {
                destination.Remark2 = "offline_logo";
            }



            return destination;
        }
        public   List<POS_SaleLineModel> ConvertLine2Model(List<POS_SaleLine> input) {
            var destination = new List<POS_SaleLineModel>();
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<POS_SaleLine, POS_SaleLineModel>();
            });
            IMapper iMapper = config.CreateMapper();
            foreach (var i in input) {
                var m = iMapper.Map<POS_SaleLine, POS_SaleLineModel>(i);
                var iInfo = posService.Menu.Where(o => o.ItemID == m.ItemID).FirstOrDefault();
                m.ImageSource = "ghost.png";
                m.ImageUrl = "/SALE/assets/img/pear.png";
                if (iInfo != null) {
                    // m.ImageSource = ImageSource.FromStream(() => new MemoryStream(Convert.FromBase64String(iInfo.ImageUrl)));
                    m.ImageSource = iInfo.ImageUrl;
                }

                destination.Add(m);
            }
            return destination;
        }
        public   List<POS_SalePaymentModel> ConvertPayment2Model(List<POS_SalePayment> input) {
            var destination = new List<POS_SalePaymentModel>();
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<POS_SalePayment, POS_SalePaymentModel>();
            });
            IMapper iMapper = config.CreateMapper();
            foreach (var i in input) {
                destination.Add(iMapper.Map<POS_SalePayment, POS_SalePaymentModel>(i));
            }

            return destination;
        }
        

    }
}
