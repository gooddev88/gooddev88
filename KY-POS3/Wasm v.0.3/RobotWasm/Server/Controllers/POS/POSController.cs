using Microsoft.AspNetCore.Mvc;
using RobotWasm.Shared.Data.GaDB;
using System.Text.Json;
using System.Text.Json.Serialization;
using static RobotWasm.Shared.Data.ML.Shared.I_Result;
using RobotWasm.Server.Data.DA.POS;
using static RobotWasm.Shared.Data.DA.POSFuncService;
using RobotWasm.Shared.Data.ML.Param;

namespace RobotWasm.Server.Controllers.POS {
    [Route("api/[controller]")]
    [ApiController]
    public class POSController : ControllerBase {

        [HttpGet("GetDocSet")]
        public I_POSSaleSet GetDocSet(string docid, string rcom) {
            return POSService.GetDocSet(docid,rcom);
        }

        [HttpPost("ListMenuItem")]
        public List<POSMenuItem> ListMenuItem([FromBody] string data) {
            var doc = JsonSerializer.Deserialize<MenuItemParam>(data, new JsonSerializerOptions {
                PropertyNameCaseInsensitive = true,
                ReferenceHandler = ReferenceHandler.Preserve
            });
            return POSService.ListMenuItem(doc.RComID,doc.ComID,doc.CustID);
        }

        [HttpGet("ListItemCate")]
        public List<MasterTypeLine> ListItemCate(string rcom) {
            return POSService.ListItemCate(rcom);
        }

        [HttpGet("ListPOS_SaleLog")]
        public List<POS_SaleLog> ListPOS_SaleLog(string rcom, string com, string billId) {
            return POSService.ListPOS_SaleLog(rcom, com, billId);
        }


        // ลบเอกสาร Pos
        [HttpGet("DeleteDoc")]
        public I_BasicResult DeleteDoc(string docId, string rcom, string modifiedby, string? remark) {
            remark = remark == null ? "" : remark;
            return POSService.DeleteDoc(docId, rcom, modifiedby, remark);
        }

        [HttpPost("GenNumberOrderID")]
        public I_POSSaleSet GenNumberOrderID([FromBody] string data) {
            var doc = JsonSerializer.Deserialize<I_POSSaleSet>(data, new JsonSerializerOptions {
                PropertyNameCaseInsensitive = true,
                ReferenceHandler = ReferenceHandler.Preserve
            });
            I_POSSaleSet r = POSService.GenNumberOrderID(doc);
            return r;
        }

        [HttpPost("GenNumberInvoiceID")]
        public I_POSSaleSet GenNumberInvoiceID([FromBody] string data) {
            var doc = JsonSerializer.Deserialize<I_POSSaleSet>(data, new JsonSerializerOptions {
                PropertyNameCaseInsensitive = true,
                ReferenceHandler = ReferenceHandler.Preserve
            });
            I_POSSaleSet r = POSService.GenNumberInvoiceID(doc);
            return r;
        }    

        [HttpPost("SavePos")]
        public I_BasicResult SavePos([FromBody] string data,string action) {
            var doc = JsonSerializer.Deserialize<I_POSSaleSet>(data, new JsonSerializerOptions {
                PropertyNameCaseInsensitive = true,
                ReferenceHandler = ReferenceHandler.Preserve
            });
            I_BasicResult r = POSService.Save(doc, action);
            return r;
        }

        [HttpPost("ListBill")]
        public List<POS_SaleHeadModel> ListBill([FromBody] string data) {
            var doc = JsonSerializer.Deserialize<I_BillFilterSet>(data, new JsonSerializerOptions {
                PropertyNameCaseInsensitive = true,
                ReferenceHandler = ReferenceHandler.Preserve
            });
            List<POS_SaleHeadModel> r = POSService.ListBill(doc);
            return r;
        }

        [HttpGet("ListPendingCheckBill")]
        public List<POS_SaleHeadModel> ListPendingCheckBill(string rcom, string com, string macno) {
            return POSService.ListPendingCheckBill(rcom,com,macno);
        }

        [HttpPost("ListOrderForKitchen")]
        public List<POS_SaleLineModel> ListOrderForKitchen([FromBody] string data) {
            var doc = JsonSerializer.Deserialize<KitchenStatusParam>(data, new JsonSerializerOptions {
                PropertyNameCaseInsensitive = true,
                ReferenceHandler = ReferenceHandler.Preserve
            });
            List<POS_SaleLineModel> r = POSService.ListOrderForKitchen(doc);
            return r;
        }

        [HttpPost("UpdatePOSLineStatus")]
        public I_BasicResult UpdatePOSLineStatus([FromBody] string data) {
            var doc = JsonSerializer.Deserialize<KitchenStatusParam>(data, new JsonSerializerOptions {
                PropertyNameCaseInsensitive = true,
                ReferenceHandler = ReferenceHandler.Preserve
            });
            I_BasicResult r = POSService.UpdatePOSLineStatus(doc);
            return r;
        }

        [HttpPost("SaveTaxSlip")]
        public I_BasicResult SaveTaxSlip([FromBody] string data){
            var doc = JsonSerializer.Deserialize<I_POSSaleSet>(data, new JsonSerializerOptions {
                PropertyNameCaseInsensitive = true,
                ReferenceHandler = ReferenceHandler.Preserve
            });
            I_BasicResult r = POSService.SaveTaxSlip(doc);
            return r;
        }

    }
}
