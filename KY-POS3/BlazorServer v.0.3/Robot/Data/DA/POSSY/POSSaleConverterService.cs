using AutoMapper;

using Robot.Data.GADB.TT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Robot.Data.DA.API.APP.SyncSalesService;
using static Robot.Data.DA.POSSY.POSService;

namespace Robot.Data.DA.POSSY {
    public class POSSaleConverterService {
        POSService posService;
 

        public POSSaleConverterService(POSService _posService ) {
            this.posService = _posService;
      
        }
 

        public class POS_SaleHeadModel : POS_SaleHead {
            public String ImageUrlShipTo { get; set; } = "frontstore.png";
        

        }
        public class POS_SaleLineModel : POS_SaleLine {
            //public ICommand SelectCommand { get; private set; }
     

         
            public String KitchenMessageLogo { get; set; }
            public bool IsEditAble { get; set; }
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

        public static  POS_SaleHeadModel ConvertHead2Model(POS_SaleHead input) {

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

        public static POS_SaleHeadModel ConvertVW_Head2Model(POS_SaleHead input)
        {

            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<POS_SaleHead, POS_SaleHeadModel>();
            });
            IMapper iMapper = config.CreateMapper();
            var destination = iMapper.Map<POS_SaleHead, POS_SaleHeadModel>(input);

            //} 
            return destination;
        }

        public  static POS_SaleHeadModel ConvertHead2ModelDisplay(POS_SaleHead input) {

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
            var tab = TableInfoService.GetTableInfo(destination.TableID, destination.RComID, destination.ComID);
            //var tab = tableService.ListTable(destination.RComID,destination.ComID).Where(o => o.TableID == destination.TableID).FirstOrDefault();
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
        public static  List<POS_SaleLineModel> ConvertLine2Model(List<POS_SaleLine> input, List<POSMenuItem> menu) {
            var destination = new List<POS_SaleLineModel>();
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<POS_SaleLine, POS_SaleLineModel>();
            });
            IMapper iMapper = config.CreateMapper();
            foreach (var i in input) {
                var m = iMapper.Map<POS_SaleLine, POS_SaleLineModel>(i);
              
                m.ImageSource = "ghost.png";
                m.ImageUrl = "/SALE/assets/img/pear.png";
                if (menu!=null) {
                    var iInfo = menu.Where(o => o.ItemID == m.ItemID).FirstOrDefault();
                    if (iInfo != null) {
                        // m.ImageSource = ImageSource.FromStream(() => new MemoryStream(Convert.FromBase64String(iInfo.ImageUrl)));
                        //m.ImageSource = null;
                        m.ImageUrl = iInfo.ImageUrl;
                        m.ImageSource = iInfo.ImageUrl;
                    }
                }
               

                switch (i.Status.ToLower()) {
                    case "ok":
                        m.IsEditAble = true;
                        m.KitchenMessageLogo = "";
                        break;
                    case "k-accept":
                        m.IsEditAble = false;
                        if (Convert.ToInt32(i.Qty) - Convert.ToInt32(i.KitchenFinishCount) == 0) {
                            //ครัวทำเสร็จครบตาม order
                            m.KitchenMessageLogo = "/SALE/assets/img/success01.png";
                        } else {
                            //ครัวทำเสร็จบางรายการ
                            m.KitchenMessageLogo = "/SALE/assets/img/warning01.png";
                        }
                      

                        break;
                    case "k-reject":
                        m.IsEditAble = false;
                        m.KitchenMessageLogo = "/SALE/assets/img/delete01.png";
                        break;
                }
                destination.Add(m);
            }
            return destination;
        }
        public static  List<POS_SalePaymentModel> ConvertPayment2Model(List<POS_SalePayment> input) {
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


        public static I_POSSaleUploadDoc ConvertI_POSSaleSet2I_POSSaleUploadDoc(I_POSSaleSet input) {
            I_POSSaleUploadDoc ouptput = new I_POSSaleUploadDoc(); 
            ouptput.Head = POSSaleConverterService.ConvertHead2DB(input.Head);
            ouptput.Line = POSSaleConverterService.ConvertLine2DB(input.Line);
            ouptput.Payment = POSSaleConverterService.ConvertPayment2DB(input.Payment);
             
            return ouptput;
        }



    }
}
