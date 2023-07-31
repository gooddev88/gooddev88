using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Robot.Data.ML;
using Robot.Data.DA;
using Robot.Data.DA.POSSY;
using Robot.Data.GADB.TT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Robot.Data.ML.I_Result;
using static Robot.Data.DA.API.APP.SyncSalesService;
using static Robot.Data.DA.POSSY.POSService;
using static Robot.Data.DA.POSSY.POSSaleConverterService;

namespace Robot.Controllers.APP {
    [Route("api/[controller]")]
    [ApiController]
    public class POSSalesController : ControllerBase {

        POSService _posService;
        public POSSalesController(POSService posService) {
            _posService = posService;
        }
        [HttpGet("GetPOSSale")]
        public I_POSSaleUploadDoc GetPOSSale(string rcom, string com, string billID) {
            I_POSSaleUploadDoc doc = new I_POSSaleUploadDoc();
            try {
                using (GAEntities db = new GAEntities()) {
                    doc.Head = db.POS_SaleHead.Where(o => o.RComID == rcom && o.ComID == com && o.BillID == billID).FirstOrDefault();
                    doc.Line = db.POS_SaleLine.Where(o => o.RComID == rcom && o.ComID == com && o.BillID == billID).ToList();
                    doc.Payment = db.POS_SalePayment.Where(o => o.RComID == rcom && o.ComID == com && o.BillID == billID).ToList();
                }
            } catch (Exception ex) { }
            return doc;
        }
        #region Stock
        [HttpPost("CalStockEndDay")]
        public I_BasicResult CalStockEndDay(POSParamModel input) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                result = POSService.CalStockEndDay(input.DateBegin, input.DateEnd, input.RComID, input.ComID, input.forceRepeat);
            } catch (Exception ex) { }
            return result;
        }

        #endregion
        #region  item onhold 
        [HttpPost("UpdateItemOnHold")]
        public I_BasicResult UpdateItemOnHold(List<ItemOnHold> data) {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                ItemService.UpdateItemOnHold(data);
            } catch (Exception ex) {
                r.Result = "fail";
                if (ex.InnerException != null) {
                    r.Message1 = ex.InnerException.ToString();
                } else {
                    r.Message1 = ex.Message;
                }
            }
            return r;
        }
        [HttpGet("GetItemOnHold")]
        public List<ItemOnHold> GetItemOnHold(string rcom, string com) {
            return ItemService.GetItemOnHold(rcom, com);
        }

        #endregion



        #region  New fuction for pos


        [HttpPost("SaveBill")]
        public I_POSSaleSet SaveBill(I_POSSaleSet doc) {
            //order , bill , invoice
            I_POSSaleSet output = new I_POSSaleSet();
            if (doc .Action== "order") {
                bool isNew = doc.Head.BillID == "" ? true : false;
                if (isNew) { // new
                    string shortShiptoId = "";
                    var getShiptoInfo = ShipToService.ListShipTo().Where(o => o.ShipToID == doc.Head.ShipToLocID).FirstOrDefault();
                    if (getShiptoInfo != null) {
                        shortShiptoId = getShiptoInfo.ShortID;
                    }
                    doc.Head.BillID = IDRuunerService.GenPOSSaleID("ORDER", doc.Head.RComID, doc.Head.ComID, doc.Head.MacNo, shortShiptoId, false, doc.Head.BillDate)[1];
                    _posService.DocSet = _posService.checkDupBillID(_posService.DocSet);
                }
            } 
            if (doc.Action == "bill") {
                var shortShiptoId = ShipToService.ListShipTo().Where(o => o.ShipToID == doc.Head.ShipToLocID).FirstOrDefault().ShortID;
                var comInfo = CompanyService.GetComInfoByComID(doc.Head.RComID, doc.Head.ComID);
                doc.Head.INVID = IDRuunerService.GenPOSSaleID("INV", doc.Head.RComID, doc.Head.ComID, doc.Head.MacNo, shortShiptoId, false, doc.Head.BillDate)[1];
                doc = _posService.checkDupInvoiceID(doc, doc.Head.MacNo); 
                doc.Head.Status = "CLOSED"; 
            }

            if (doc.Action == "invoice") {
                _posService.SaveTaxSlip(doc, doc.Head.MacNo);
            }
            var rr = POSService.Save(POSSaleConverterService.ConvertI_POSSaleSet2I_POSSaleUploadDoc(doc), true);
            output = _posService.GetDocSet(doc.Head.BillID, doc.Head.RComID);
            return output;
        }
        [HttpGet("GetBill")]
        public I_POSSaleSet GetBill(string rcom, string billid ) {
            return _posService.GetDocSet(billid,rcom);
        }

        [HttpGet("ListPendingCheckBill")]
        public List<POS_SaleHeadModel> ListPendingCheckBill(string rcom, string com, string macno  ) {
            macno = macno == null ? "" : macno;
            return POSService.ListPendingCheckBill(rcom, com, macno);
        }
  [HttpPost("ListBill")]
        public List<POS_SaleHeadModel> ListBill(I_BillFilterSetMobile f ) {
            return POSService.ListBill_Online(f);
        }
        #endregion
    }
}
