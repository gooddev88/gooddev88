using Microsoft.AspNetCore.Mvc;
using RobotWasm.Server.Data.DA.Master;
using RobotWasm.Shared.Data.GaDB;
using RobotWasm.Shared.Data.ML.Param;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace RobotWasm.Server.Controllers.Customer {
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase {


        [HttpGet("GetCustomerInfoByCusID")]
        public CustomerInfo GetCustomerInfoByCusID(string? rcom,string? comId,string cusid)
        {
            rcom = rcom == null ? "" : rcom;
            comId = comId == null ? "" : comId;
            return CustomerService.GetCustomerInfoByCusID(rcom,comId,cusid);
        }

        [HttpPost("ListCustomerInfo")]
        public List<CustomerInfo> ListCustomerInfo([FromBody] string? data) {
      
            var doc = JsonSerializer.Deserialize<CustomerParam>(data, new JsonSerializerOptions {
                PropertyNameCaseInsensitive = true,
                ReferenceHandler = ReferenceHandler.Preserve
            });
            return CustomerService.ListCustomerInfo(doc.Rcom, doc.Com,doc.Brand, doc.Search);
        }

        


        #region tax
        [HttpGet("GetTaxRate")]
        public decimal GetTaxRate(string rcom, string type, string taxid) {
            return CustomerService.GetTaxRate(rcom, type, taxid);
        } 
        #endregion
  
    }
}
