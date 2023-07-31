using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Npgsql.Logging;
using RobotAPI.Data.DA.Cims;
using RobotAPI.Data.DA.Data;
using RobotAPI.Data.ML.Budget;
using RobotAPI.Data.ML.Data;
using RobotAPI.Services.Api;

namespace RobotAPI.Controllers.Budget {
    [Route("api/[controller]")]
    [ApiController]
    public class G600Controller : ControllerBase {
        ClientService _clientService;
        Data600Service _data600Service;
        public G600Controller(ClientService clientService, Data600Service data600Service) {
            _clientService = clientService;
            _data600Service = data600Service;   
        }
        //[AllowAnonymous]
        //[HttpGet("data606")]
        //async public Task<ActionResult> data606( ) {
        //    //แสดงแผนก
        //    Data606Set.DocSet result = new Data606Set.DocSet();
        //    try {
        //        var conn = ApiConnService.GetApiInfo("data606");
        //        string url = "";
        //        url = $"{conn.source_base_url}{conn.source_api_url}";
        //        var responds = await _clientService.Post<List<Data606Set.DataRow>>(url, conn.source_api_token,"");
        //        if (responds.StatusCode.ToString().ToUpper() != "OK") {
        //            BadRequest(result);
        //        } else {
        //            result = (List<Data606Set.DataRow>)responds.Result; 
        //            Ok(result);
        //        }
        //    } catch {
        //        BadRequest(result);
        //    }
        //    return Ok(result);
        //}

       
        [AllowAnonymous]
        [HttpGet("data601")]
        async public Task<ActionResult> data601(string? year) {
            ////ข้อมูลภาพรวมการใช้งบประมาณ BudgetOverall
            Data601Set.DocSet output = new Data601Set.DocSet { message = "ok", rows = new List<Data601Set.DataRow>(), status = 1 };
            try {
                output = await Task.Run(() => _data600Service.Data601(year));
                Ok(output);
            } catch (Exception ex) {
                output.status = 0;
                output.message = ex.Message;
                BadRequest(output);
            }
            return Ok(output);
        }
        [AllowAnonymous]
        [HttpGet("data602")]
        async public Task<ActionResult> data602(string? year) {
            //ข้อมูลสัดส่วนงบประมาณหลังโอนเปลี่ยนแปลง budget transfer
            Data602Set.DocSet output = new Data602Set.DocSet { message = "ok", rows = new List<Data602Set.DataRow>(), status = 1 };
            try {
                output = await Task.Run(() => _data600Service.Data602(year));
                Ok(output);
            } catch (Exception ex) {
                output.status = 0;
                output.message = ex.Message;
                BadRequest(output);
            }
            return Ok(output);
        }

    


        [AllowAnonymous]
        [HttpGet("data603")]
        async public Task<ActionResult> data603(string? year) {
            //ข้อมูลภาพรวมการใช้งบประมาณตามประเภทงบ Expense
            Data603Set.DocSet output = new Data603Set.DocSet { message = "ok", rows = new List<Data603Set.DataRow>(), status = 1 };
            try {
                output = await Task.Run(() => _data600Service.Data603(year));
                Ok(output);
            } catch (Exception ex) {
                output.status = 0;
                output.message = ex.Message;
                BadRequest(output);
            }
            return Ok(output);
        }

        [AllowAnonymous]
        [HttpGet("data604")]
        async public Task<ActionResult> data604(string? year) {
            //ข้อมูลเป้าหมายตาม ครม.
            Data604Set.DocSet output = new Data604Set.DocSet { message = "ok", rows = new List<Data604Set.DataRow>(), status = 1 };
            try {
                output = await Task.Run(() => _data600Service.Data604(year));
                if (output==null) {
                    output = new Data604Set.DocSet();
                    if (output.rows==null) {
                        output.rows = new List<Data604Set.DataRow>();
                        output.rows.Add(new Data604Set.DataRow { Year = 0, TotalAmount = 0, TargetAmountQuater1 = 0, TargetAmountQuater2 = 0, TargetAmountQuater3 = 0, TargetAmountQuater4 = 0, TargetPercentQuater1 = 0, TargetPercentQuater2 = 0, TargetPercentQuater3 = 0, TargetPercentQuater4 = 0 });
                    }
                    
                }
                if (output.rows.Count==0) {
                    output.rows.Add(new Data604Set.DataRow { Year = 0, TotalAmount = 0, TargetAmountQuater1 = 0, TargetAmountQuater2 = 0, TargetAmountQuater3 = 0, TargetAmountQuater4 = 0, TargetPercentQuater1 = 0, TargetPercentQuater2 = 0, TargetPercentQuater3 = 0, TargetPercentQuater4 = 0 });
                }
                Ok(output);
            } catch (Exception ex) {
                output.status = 0;
                output.message = ex.Message;
                BadRequest(output);
            }
            return Ok(output);
        }

        [AllowAnonymous]
        [HttpGet("data605")]
        async public Task<ActionResult> data605(string? year) {
            //ข้อมูลเป้าหมายตาม ครม.
            Data605Set.DocSet output = new Data605Set.DocSet { message = "ok", rows = new List<Data605Set.DataRow>(), status = 1 };
            try {
                output = await Task.Run(() => _data600Service.Data605(year));
                Ok(output);
            } catch (Exception ex) {
                output.status = 0;
                output.message = ex.Message;
                BadRequest(output);
            }
            return Ok(output);
        }
        [AllowAnonymous]
        [HttpGet("data606")]
        async public Task<ActionResult> data606() {
            //แสดงแผนก
            Data606Set.DocSet output = new Data606Set.DocSet { message = "ok", rows = new List<Data606Set.DataRow>(), status = 1 };
            try {
                output = await Task.Run(() => _data600Service.Data606());
                Ok(output);

            } catch (Exception ex) {
                output.status = 0;
                output.message = ex.Message;
                BadRequest(output);
            }
            return Ok(output);
        }
        //[AllowAnonymous]
        //[HttpGet("OverallBudget")]
        //async public Task<ActionResult> OverallBudget(string? year = "") {
        //    List<OverallBudget> result = new List<OverallBudget>();
        //    try {
        //        year = year == null ? "" : year;
        //        var conn = ApiConnService.GetApiInfo("OverallBudget");

        //        string url = "";
        //        if (year == "") {
        //            url = $"{conn.source_base_url}{conn.source_api_url}";
        //        } else {
        //            url = $"{conn.source_base_url}{conn.source_api_url}?year={year}";
        //        }

        //        var responds = await _clientService.Post<List<OverallBudget>>(url, conn.source_api_token, "");
        //        if (responds.StatusCode.ToString().ToUpper() != "OK") {
        //            BadRequest(result);
        //        } else {
        //            result = (List<OverallBudget>)responds.Result;
        //            Ok(result);
        //        }
        //    } catch {
        //        BadRequest(result);
        //    }
        //    return Ok(result);
        //}
        //[AllowAnonymous]
        //     [HttpGet("BudgetTransfer")]
        //     async public Task<ActionResult> BudgetTransfer(string? year = "") {
        //         List<BudgetTransfer> result = new List<BudgetTransfer>();
        //         try {
        //             year = year == null ? "" : year;
        //             var conn = ApiConnService.GetApiInfo("BudgetTransfer");

        //             string url = "";
        //             if (year == "") {
        //                 url = $"{conn.source_base_url}{conn.source_api_url}";
        //             } else {
        //                 url = $"{conn.source_base_url}{conn.source_api_url}?year={year}";
        //             }

        //             var responds = await _clientService.Post<List<BudgetTransfer>>(url, conn.source_api_token, "");
        //             if (responds.StatusCode.ToString().ToUpper() != "OK") {
        //                 BadRequest(result);
        //             } else {
        //                 result = (List<BudgetTransfer>)responds.Result;
        //                 Ok(result);
        //             }
        //         } catch {
        //             BadRequest(result);
        //         }
        //         return Ok(result);
        //     }

        [AllowAnonymous]
        [HttpGet("BudgetExpense")]
        async public Task<ActionResult> BudgetExpense(string? year = "") {
            List<BudgetExpense> result = new List<BudgetExpense>();
            try {
                year = year == null ? "" : year;
                var conn = ApiConnService.GetApiInfo("BudgetExpense");

                string url = "";
                if (year == "") {
                    url = $"{conn.source_base_url}{conn.source_api_url}";
                } else {
                    url = $"{conn.source_base_url}{conn.source_api_url}?year={year}";
                }

                var responds = await _clientService.Post<List<BudgetExpense>>(url, conn.source_api_token, "");
                if (responds.StatusCode.ToString().ToUpper() != "OK") {
                    BadRequest(result);
                } else {
                    result = (List<BudgetExpense>)responds.Result;
                    Ok(result);
                }
            } catch {
                BadRequest(result);
            }
            return Ok(result);
        }
     [AllowAnonymous]
        [HttpGet("Government")]
        async public Task<ActionResult> Government(string? year = "") {
            List<Government> result = new List<Government>();
            try {
                year = year == null ? "" : year;
                var conn = ApiConnService.GetApiInfo("Government");

                string url = "";
                if (year == "") {
                    url = $"{conn.source_base_url}{conn.source_api_url}";
                } else {
                    url = $"{conn.source_base_url}{conn.source_api_url}?year={year}";
                }

                var responds = await _clientService.Post<List<Government>>(url, conn.source_api_token, "");
                if (responds.StatusCode.ToString().ToUpper() != "OK") {
                    BadRequest(result);
                } else {
                    result = (List<Government>)responds.Result;
                    Ok(result);
                }
            } catch {
                BadRequest(result);
            }
            return Ok(result);
        }
     [AllowAnonymous]
        [HttpGet("Organization")]
        async public Task<ActionResult> Organization(string? year = "") {
            List<Organization> result = new List<Organization>();
            try {
                year = year == null ? "" : year;
                var conn = ApiConnService.GetApiInfo("Organization");

                string url = "";
                if (year == "") {
                    url = $"{conn.source_base_url}{conn.source_api_url}";
                } else {
                    url = $"{conn.source_base_url}{conn.source_api_url}?year={year}";
                }

                var responds = await _clientService.Post<List<Organization>>(url, conn.source_api_token, "");
                if (responds.StatusCode.ToString().ToUpper() != "OK") {
                    BadRequest(result);
                } else {
                    result = (List<Organization>)responds.Result;
                    Ok(result);
                }
            } catch {
                BadRequest(result);
            }
            return Ok(result);
        }


    }
}
