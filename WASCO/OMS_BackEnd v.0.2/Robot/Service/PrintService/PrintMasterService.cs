using Robot.Data.GADB.TT;
using System;
using static Robot.Data.ML.I_Result;

namespace Robot.Service.PrintService
{
    public class PrintMasterService
    {

        public static I_BasicResult CreatePrintData(PrintData input )
        {
            I_BasicResult r = new I_BasicResult { Result="ok",Message1="",Message2=""};
            try
            {

            }
            catch ( Exception e)
            {
                r.Result = "fail";
                if (e.InnerException!=null)
                {
                    r.Message1= e.InnerException.Message;
                }
                else
                {
                    r.Message1 = e.Message;
                }
            }
            return r;
        }
    }
}
