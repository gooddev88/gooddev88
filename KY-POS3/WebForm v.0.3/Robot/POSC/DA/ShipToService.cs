using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Robot.POSC.DA
{
    public  class ShipToService
    {
        public class ShipTo
        {
            public String ShipToID { get; set; }
            public String ShipToName { get; set; }
            public String ShortID { get; set; }
            public String UsePrice { get; set; }
            public String ImageUrl { get; set; }

        }

        public static List<ShipTo> ListShipTo() {
            List<ShipTo> shipList = new List<ShipTo>();
            shipList.Add(new ShipTo {
                ShipToID = "",
                ShipToName = "หน้าร้าน",
                ShortID = "",
                UsePrice = "",
                ImageUrl = "Image/frontstore.png"
            });
            shipList.Add(new ShipTo {
                ShipToID = "GRAB",
                ShipToName = "Grab",
                ShortID = "G",
                UsePrice = "GRAB",
                ImageUrl = "Image/grab.png"
            });
            shipList.Add(new ShipTo {
                ShipToID = "GOJEK",
                ShipToName = "Gojek",
                ShortID = "J",
                UsePrice = "GOJEK",
                ImageUrl = "Image/gojek.png"
            });
            shipList.Add(new ShipTo {
                ShipToID = "LINEMAN",
                ShipToName = "Line Man",
                ShortID = "L",
                UsePrice = "LINEMAN",
                ImageUrl = "Image/lineman.png"
            });

            shipList.Add(new ShipTo {
                ShipToID = "PANDA",
                ShipToName = "Food Padda",
                ShortID = "P",
                UsePrice = "PANDA",
                ImageUrl = "Image/padda.png"
            });
            shipList.Add(new ShipTo {
                ShipToID = "ONLINE",
                ShipToName = "Online",
                ShortID = "O",
                UsePrice = "ONLINE",
                ImageUrl = "Image/online.png"
            });
            return shipList;

        }
   
    }
}
 