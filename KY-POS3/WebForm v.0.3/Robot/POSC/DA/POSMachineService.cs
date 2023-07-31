using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Robot.POSC.DA
{
    public class POSMachineService
    {
        public String MacID { get; set; }
        public String MacName { get; set; }
        public static List<POSMachineService> ListMachine() {
            List<POSMachineService> mm = new List<POSMachineService>();
            mm.Add(new POSMachineService { MacID = "A", MacName = "A" });
            mm.Add(new POSMachineService { MacID = "B", MacName = "B" });
            mm.Add(new POSMachineService { MacID = "C", MacName = "C" });
            //mm.Add(new POSMachineService { MacID = "D", MacName = "D" });
            //mm.Add(new POSMachineService { MacID = "E", MacName = "E" });
            //mm.Add(new POSMachineService { MacID = "F", MacName = "F" });
            //mm.Add(new POSMachineService { MacID = "G", MacName = "G" });
            //mm.Add(new POSMachineService { MacID = "H", MacName = "H" });
            //mm.Add(new POSMachineService { MacID = "I", MacName = "I" });
            //mm.Add(new POSMachineService { MacID = "J", MacName = "J" });
            //mm.Add(new POSMachineService { MacID = "K", MacName = "K" });
            //mm.Add(new POSMachineService { MacID = "L", MacName = "L" });
            //mm.Add(new POSMachineService { MacID = "M", MacName = "M" });
            //mm.Add(new POSMachineService { MacID = "N", MacName = "N" });
            //mm.Add(new POSMachineService { MacID = "O", MacName = "O" });
            //mm.Add(new POSMachineService { MacID = "P", MacName = "P" });
            //mm.Add(new POSMachineService { MacID = "Q", MacName = "Q" });
            //mm.Add(new POSMachineService { MacID = "R", MacName = "R" });
            //mm.Add(new POSMachineService { MacID = "U", MacName = "U" });
            //mm.Add(new POSMachineService { MacID = "V", MacName = "V" });
            //mm.Add(new POSMachineService { MacID = "W", MacName = "W" });
            mm.Add(new POSMachineService { MacID = "X", MacName = "X" });
            mm.Add(new POSMachineService { MacID = "Y", MacName = "Y" });
            mm.Add(new POSMachineService { MacID = "Z", MacName = "Z" });


            return mm;

        }
    }
}