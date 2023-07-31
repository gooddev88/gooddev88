using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Robot.Data.DA.POSSY {
    
        public class ShipToService {
        
        public ShipToService() {
            
        }
        public class ShipTo {
                public String ShipToID { get; set; }
                public String ShipToName { get; set; }
                public String ShortID { get; set; }
                public String UsePrice { get; set; }
                public String ImageUrl { get; set; }
                
             
            }

        public  static List<ShipTo> ListShipTo() {
            List<ShipTo> shipList = new List<ShipTo>();
            shipList.Add(new ShipTo {
                ShipToID = "",
                ShipToName = "หน้าร้าน",
                ShortID = "",
                UsePrice = "",
                ImageUrl = "/SALE/assets/img/frontstore.png"
            });
            shipList.Add(new ShipTo {
                ShipToID = "GRAB",
                ShipToName = "Grab",
                ShortID = "G",
                UsePrice = "GRAB",
                ImageUrl = "/SALE/assets/img/grab.png"
            });
            shipList.Add(new ShipTo {
                ShipToID = "SHOPEE",
                ShipToName = "Shopee",
                ShortID = "J",
                UsePrice = "SHOPEE",
                ImageUrl = "/SALE/assets/img/shopee_logo.png"
            });
            shipList.Add(new ShipTo {
                ShipToID = "LINEMAN",
                ShipToName = "Line Man",
                ShortID = "L",
                UsePrice = "LINEMAN",
                ImageUrl = "/SALE/assets/img/lineman.png"
            });

            shipList.Add(new ShipTo {
                ShipToID = "PANDA",
                ShipToName = "Food Padda",
                ShortID = "P",
                UsePrice = "PANDA",
                ImageUrl = "/SALE/assets/img/padda.png"
            });
            shipList.Add(new ShipTo
            {
                ShipToID = "ROBINHOOD",
                ShipToName = "Robinhood",
                ShortID = "R",
                UsePrice = "ROBINHOOD",
                ImageUrl = "/SALE/assets/img/robinhood.png"
            });
            shipList.Add(new ShipTo {
                ShipToID = "ONLINE",
                ShipToName = "Online",
                ShortID = "O",
                UsePrice = "ONLINE",
                ImageUrl = "/SALE/assets/img/online.png"
            });
            return shipList;

        }
         
        }
    
}
