using System.Text.Json;
using System.Text.Json.Serialization;
using RobotWasm.Shared.Data.ML.POS;
using RobotWasm.Shared.Data.GaDB;
using static RobotWasm.Shared.Data.ML.Shared.I_Result;
using System.Net.Http.Json;
using RobotWasm.Shared.Data.ML.Shared;
using Blazored.LocalStorage;
using RobotWasm.Shared.Data.ML.POS;
using RobotWasm.Shared.Data.ML.Login;
using RobotWasm.Shared.Data.ML.POS.ShipTo;

namespace RobotWasm.Client.Data.DA.POS
{
    public class POSService {

        private readonly HttpClient _http;
        private ILocalStorageService _localStorage;
        public POSService(HttpClient Http, ILocalStorageService localStorage) {
            _http = Http;
            _localStorage = localStorage;
        }

        async public Task<string> SetLocalSttorageShowProductImage() {
            var isshowproduct = await _localStorage.GetItemAsync<string>("isshowproductimage");
            if (isshowproduct == null) {
                await _localStorage.SetItemAsync("isshowproductimage", "1");
                return "1";
            } else {
                if (isshowproduct == "1") {
                    await _localStorage.SetItemAsync("isshowproductimage", "0");
                    return "0";
                } else {
                    await _localStorage.SetItemAsync("isshowproductimage", "1");
                    return "1";
                }
            }
        }
        async public Task<string> GetLocalSttorageShowProductImage() {
            var isshowproduct = await _localStorage.GetItemAsync<string>("isshowproductimage");
            return isshowproduct;
        }


        public I_POSSaleSet DocSet { get; set; }
        public List<POSMenuItem> Menu { get; set; } = new List<POSMenuItem>();
        public List<MasterTypeLine> ItemCate { get; set; } = new List<MasterTypeLine>();
        public List<SelectOption> Tenders { get; set; } = new List<SelectOption>();

        async public Task<I_POSSaleSet> GetDocSet(string docid, string rcom) {
            I_POSSaleSet result = new I_POSSaleSet();
            try {
                var res = await _http.GetAsync($"api/POS/GetDocSet?docid={docid}&rcom={rcom}");
                result = JsonSerializer.Deserialize<I_POSSaleSet>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            } catch (Exception ex) {
                var x = ex.Message;
            }
            return result;
        }

        async public Task<I_POSSaleSet> NewTransaction(LoginSet data,string version, string? shipto) {
            shipto = shipto == null ? "" : shipto;
            I_POSSaleSet doc = new I_POSSaleSet();
            try {
                string strPayload = JsonSerializer.Serialize(data);
                string url = $"api/POS/NewTransaction?version={version}&shipto={shipto}";
                var response = await _http.PostAsJsonAsync(url, strPayload);
                var status = response.StatusCode;

                if (status.ToString().ToLower() != "ok") {
                } else {
                    doc = JsonSerializer.Deserialize<I_POSSaleSet>(await response.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                        PropertyNameCaseInsensitive = true,
                        ReferenceHandler = ReferenceHandler.Preserve
                    });
                }
            } catch (Exception ex) {
                var x = ex.Message;
            }
            return doc;
        }

        async public Task<List<POSMenuItem>> ListMenuItem(string rcom, string comId, string custId) {
            List<POSMenuItem> result = new List<POSMenuItem>();
            try {
                var res = await _http.GetAsync($"api/POS/ListMenuItem?rcom={rcom}&comId={comId}&custId={custId}");
                result = JsonSerializer.Deserialize<List<POSMenuItem>>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            } catch (Exception ex) {
                var x = ex.Message;
            }
            return result;
        }

        async public Task<List<MasterTypeLine>> ListItemCate(string rcom) {
            List<MasterTypeLine> result = new List<MasterTypeLine>();
            try {
                var res = await _http.GetAsync($"api/POS/ListItemCate?rcom={rcom}");
                result = JsonSerializer.Deserialize<List<MasterTypeLine>>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            } catch (Exception ex) {
                var x = ex.Message;
            }
            return result;
        }

        async public Task<List<POS_SaleLog>> ListPOS_SaleLog(string rcom, string com, string billId) {
            List<POS_SaleLog> result = new List<POS_SaleLog>();
            try {
                var res = await _http.GetAsync($"api/POS/ListPOS_SaleLog?rcom={rcom}&com={com}&billId={billId}");
                result = JsonSerializer.Deserialize<List<POS_SaleLog>>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            } catch (Exception ex) {
                var x = ex.Message;
            }
            return result;
        }

        async public Task<I_BasicResult> DeleteDoc(string docId, string rcom, string modifiedby, string? remark) {
            remark = remark == null ? "" : remark;
            I_BasicResult? result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                var res = await _http.GetAsync($"api/POS/DeleteDoc?docId={docId}&rcom={rcom}&modifiedby={modifiedby}&remark={remark}");
                result = JsonSerializer.Deserialize<I_BasicResult>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            } catch (Exception ex) {
                var x = ex.Message;
            }
            return result;
        }

        //async public Task<I_POSSaleSet> CalDocSet(I_POSSaleSet data) {
        //    I_POSSaleSet doc = new I_POSSaleSet();
        //    try {
        //        string strPayload = JsonSerializer.Serialize(data);
        //        string url = $"api/POS/CalDocSet";
        //        var response = await _http.PostAsJsonAsync(url, strPayload);
        //        var status = response.StatusCode;

        //        if (status.ToString().ToLower() != "ok") {
        //        } else {
        //            doc = JsonSerializer.Deserialize<I_POSSaleSet>(await response.Content.ReadAsStringAsync(), new JsonSerializerOptions {
        //                PropertyNameCaseInsensitive = true,
        //                ReferenceHandler = ReferenceHandler.Preserve
        //            });
        //        }
        //    } catch (Exception ex) {
        //        var x = ex.Message;
        //    }
        //    return doc;
        //}

        //async public Task<I_POSSaleSet> AddPayment(I_POSSaleSet data,string paymentType, decimal getAmt) {
        //    I_POSSaleSet doc = new I_POSSaleSet();
        //    try {
        //        string strPayload = JsonSerializer.Serialize(data);
        //        string url = $"api/POS/AddPayment?paymentType={paymentType}&getAmt={getAmt}";
        //        var response = await _http.PostAsJsonAsync(url, strPayload);
        //        var status = response.StatusCode;

        //        if (status.ToString().ToLower() != "ok") {
        //        } else {
        //            doc = JsonSerializer.Deserialize<I_POSSaleSet>(await response.Content.ReadAsStringAsync(), new JsonSerializerOptions {
        //                PropertyNameCaseInsensitive = true,
        //                ReferenceHandler = ReferenceHandler.Preserve
        //            });
        //        }
        //    } catch (Exception ex) {
        //        var x = ex.Message;
        //    }
        //    return doc;
        //}

        async public Task<I_POSSaleSet> AddItem(I_POSSaleSet data, decimal qty, decimal discamt = 0) {
            I_POSSaleSet doc = new I_POSSaleSet();
            try {
                string strPayload = JsonSerializer.Serialize(data);
                string url = $"api/POS/AddItem?qty={qty}&discamt={discamt}";
                var response = await _http.PostAsJsonAsync(url, strPayload);
                var status = response.StatusCode;

                if (status.ToString().ToLower() != "ok") {
                } else {
                    doc = JsonSerializer.Deserialize<I_POSSaleSet>(await response.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                        PropertyNameCaseInsensitive = true,
                        ReferenceHandler = ReferenceHandler.Preserve
                    });
                }
            } catch (Exception ex) {
                var x = ex.Message;
            }
            return doc;
        }

        async public Task<I_POSSaleSet> CheckDupBillID(I_POSSaleSet data) {
            I_POSSaleSet doc = new I_POSSaleSet();
            try {
                string strPayload = JsonSerializer.Serialize(data);
                string url = $"api/POS/CheckDupBillID";
                var response = await _http.PostAsJsonAsync(url, strPayload);
                var status = response.StatusCode;

                if (status.ToString().ToLower() != "ok") {
                } else {
                    doc = JsonSerializer.Deserialize<I_POSSaleSet>(await response.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                        PropertyNameCaseInsensitive = true,
                        ReferenceHandler = ReferenceHandler.Preserve
                    });
                }
            } catch (Exception ex) {
                var x = ex.Message;
            }
            return doc;
        }

        async public Task<I_POSSaleSet> CheckDupInvoiceID(I_POSSaleSet data,string macno) {
            I_POSSaleSet doc = new I_POSSaleSet();
            try {
                string strPayload = JsonSerializer.Serialize(data);
                string url = $"api/POS/CheckDupInvoiceID?macno={macno}";
                var response = await _http.PostAsJsonAsync(url, strPayload);
                var status = response.StatusCode;

                if (status.ToString().ToLower() != "ok") {
                } else {
                    doc = JsonSerializer.Deserialize<I_POSSaleSet>(await response.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                        PropertyNameCaseInsensitive = true,
                        ReferenceHandler = ReferenceHandler.Preserve
                    });
                }
            } catch (Exception ex) {
                var x = ex.Message;
            }
            return doc;
        }

        async public Task<I_BasicResult> SavePos(I_POSSaleUploadDoc data,bool iswebSave) {
            I_BasicResult doc = new I_BasicResult();
            try {
                string strPayload = JsonSerializer.Serialize(data);
                string url = $"api/POS/SavePos?iswebSave={iswebSave}";
                var response = await _http.PostAsJsonAsync(url, strPayload);
                var status = response.StatusCode;

                if (status.ToString().ToLower() != "ok") {
                } else {
                    doc = JsonSerializer.Deserialize<I_BasicResult>(await response.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                        PropertyNameCaseInsensitive = true,
                        ReferenceHandler = ReferenceHandler.Preserve
                    });
                }
            } catch (Exception ex) {
                var x = ex.Message;
            }
            return doc;
        }

        async public Task<List<POS_SaleHeadModel>> ListBill(I_BillFilterSet data) {
            List<POS_SaleHeadModel> doc = new List<POS_SaleHeadModel>();
            try {
                string strPayload = JsonSerializer.Serialize(data);
                string url = $"api/POS/ListBill";
                var response = await _http.PostAsJsonAsync(url, strPayload);
                var status = response.StatusCode;

                if (status.ToString().ToLower() != "ok") {
                } else {
                    doc = JsonSerializer.Deserialize<List<POS_SaleHeadModel>>(await response.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                        PropertyNameCaseInsensitive = true,
                        ReferenceHandler = ReferenceHandler.Preserve
                    });
                }
            } catch (Exception ex) {
                var x = ex.Message;
            }
            return doc;
        }

        async public Task<I_BasicResult> ListPendingCheckBill(string rcom, string com, string macno) {
            I_BasicResult? result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                var res = await _http.GetAsync($"api/POS/ListPendingCheckBill?rcom={rcom}&com={com}&macno={macno}");
                result = JsonSerializer.Deserialize<I_BasicResult>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            } catch (Exception ex) {
                var x = ex.Message;
            }
            return result;
        }

        async public Task<I_POSSaleSet> SaveTaxSlip(I_POSSaleSet data,string macno) {
            I_POSSaleSet doc = new I_POSSaleSet();
            try {
                string strPayload = JsonSerializer.Serialize(data);
                string url = $"api/POS/SaveTaxSlip?macno={macno}";
                var response = await _http.PostAsJsonAsync(url, strPayload);
                var status = response.StatusCode;

                if (status.ToString().ToLower() != "ok") {
                } else {
                    doc = JsonSerializer.Deserialize<I_POSSaleSet>(await response.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                        PropertyNameCaseInsensitive = true,
                        ReferenceHandler = ReferenceHandler.Preserve
                    });
                }
            } catch (Exception ex) {
                var x = ex.Message;
            }
            return doc;
        }

        //async public Task<I_BasicResult> CreateAssetCountList(string doc_no, List<string> data)
        //{
        //    I_BasicResult? output = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
        //    try
        //    {
        //        string strPayload = JsonSerializer.Serialize(data);
        //        string url = $"api/AssetDoc/CreateAssetCountList";
        //        var response = await _http.PostAsJsonAsync(url, strPayload);
        //        if (response.StatusCode.ToString().ToUpper() != "OK")
        //        {
        //            output.Result = "fail";
        //            output.Message1 = response.StatusCode.ToString();
        //            return output;
        //        }
        //        output = response.Content.ReadFromJsonAsync<I_BasicResult>().Result;
        //    }
        //    catch (Exception ex)
        //    {
        //        var x = ex.Message;
        //    }
        //    return output;
        //}


        //#region Filter
        //async public void SetSessionFiterSet(I_AssetDocFiterSet data) {
        //    JsonSerializerOptions jso = new JsonSerializerOptions();
        //    jso.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
        //    string json = System.Text.Json.JsonSerializer.Serialize(data, jso);
        //    await _localStorage.SetItemAsync("asset_Fiter", json);
        //}
        //async public Task<I_AssetDocFiterSet>? GetSessionFiterSet() {
        //    I_AssetDocFiterSet? result = NewFilterSet();
        //    var strdoc = await _localStorage.GetItemAsync<string>("asset_Fiter");
        //    if (strdoc != null) {
        //        result= JsonSerializer.Deserialize<I_AssetDocFiterSet>(strdoc, new JsonSerializerOptions {
        //            PropertyNameCaseInsensitive = true,
        //            ReferenceHandler = ReferenceHandler.Preserve
        //        });
        //    } else {
        //        result = NewFilterSet();
        //    }
        //    return result;
        //}
        //#endregion


        #region Cal Data

        public static decimal CalDiscountInVat(I_POSSaleSet doc, decimal disc_input) {
            decimal discount = 0;
            if (doc.Head.IsVatRegister == true) {
                discount = (disc_input * 100) / (100 + doc.Head.VatRate);
                //discount = disc_input;
            } else {
                discount = disc_input;
            }
            return discount;
        }
        public static I_POSSaleSet CalDocSet(I_POSSaleSet doc) {
            var h = doc.Head;
            //copy head 2 line
            decimal baseDisc = doc.Line.Where(o => o.DocTypeID != "DISCOUNT" && o.IsLineActive == true && o.Status != "K-REJECT").Sum(o => o.BaseTotalAmt);
            foreach (var l in doc.Line) {
                //1.cal line base total amt 
                if (l.ItemTypeID == "DISCOUNT") {//  line ที่เป็น discount
                    //2. cal disc amt
                    if (l.DiscCalBy == "P") {
                        l.DiscAmt = Math.Round((baseDisc * l.DiscPer) / 100, 3, MidpointRounding.AwayFromZero);
                        //l.BaseTotalAmt = l.DiscAmt;

                    }
                    //3. cal disc per
                    if (l.DiscCalBy == "A") {
                        l.DiscPer = 0;
                        //l.BaseTotalAmt = l.DiscAmt;
                        if (baseDisc != 0) {
                            l.DiscPer = Math.Round((100 * l.DiscAmt) / baseDisc, 3, MidpointRounding.AwayFromZero);
                        }
                    }
                } else {//เป็นรายการขาย
                    l.BaseTotalAmt = Math.Round(l.Qty * l.Price, 3, MidpointRounding.AwayFromZero);
                    l.TotalAmt = l.BaseTotalAmt;
                }
                //4.assign head 2 line
                l.INVID = h.INVID;
                l.BillID = h.BillID;
                l.INVID = h.INVID;
                l.FINVID = h.FINVID;
                l.BillDate = h.BillDate;
                l.ComID = h.ComID;
                l.RComID = h.RComID;
                l.MacNo = h.MacNo;
                l.DocTypeID = h.DocTypeID;
                l.CustID = h.CustomerID;
                l.TableID = h.TableID;
                l.TableName = h.TableName;
                l.ShipToLocID = h.ShipToLocID;
                l.ShipToLocName = h.ShipToLocName;
                l.VatRate = h.VatRate;
                l.VatTypeID = h.VatTypeID;
                l.CreatedBy = h.CreatedBy;
                l.CreatedDate = l.CreatedDate == null ? DateTime.Now : l.CreatedDate;
                l.ModifiedDate = l.ModifiedDate == null ? l.CreatedDate : l.ModifiedDate;
            }

            //5. cal head base total amt            
            doc.Head.BaseNetTotalAmt = Math.Round(doc.Line.Where(o => o.IsLineActive == true).Sum(o => o.BaseTotalAmt) - doc.Line.Where(o => o.IsLineActive == true).Sum(o => o.DiscAmt), 2, MidpointRounding.AwayFromZero);
            //6. cal head disc amt
            if (doc.Head.DiscCalBy == "P") {
                doc.Head.OntopDiscAmt = Math.Round((doc.Head.BaseNetTotalAmt * doc.Head.OntopDiscPer) / 100, 2, MidpointRounding.AwayFromZero);
            }
            //7. cal head disc per
            if (doc.Head.DiscCalBy == "A") {
                if (doc.Head.BaseNetTotalAmt != 0) {
                    doc.Head.OntopDiscPer = Math.Round((100 * doc.Head.OntopDiscAmt) / doc.Head.BaseNetTotalAmt, 2, MidpointRounding.AwayFromZero);
                }
            }

            foreach (var l in doc.Line.Where(o => o.IsLineActive == true)) {
                //7.ตั้งต้น Ontop discount จาก Header ก่อนนำไปคำนวณใน Line
                l.OntopDiscPer = h.OntopDiscPer;
                l.OntopDiscAmt = Math.Round(h.OntopDiscAmt, 3, MidpointRounding.AwayFromZero);

                //8.cal line disc weight ontop percent & amt

                if (h.BaseNetTotalAmt != 0) {
                    l.OntopDiscAmt = Math.Round(((l.BaseTotalAmt - l.DiscAmt) * h.OntopDiscAmt) / h.BaseNetTotalAmt, 3, MidpointRounding.AwayFromZero);
                    l.OntopDiscPer = (l.BaseTotalAmt - l.DiscAmt) == 0 ? 0 : (100 * l.OntopDiscAmt) / (l.BaseTotalAmt - l.DiscAmt);
                }
                //9 cal line total amt 
                l.TotalAmt = Math.Round(l.BaseTotalAmt - l.DiscAmt - l.OntopDiscAmt, 3, MidpointRounding.AwayFromZero);
                //10.cal line vat amt
                l.VatAmt = Math.Round((l.TotalAmt * l.VatRate) / 100, 3, MidpointRounding.AwayFromZero);
                //11.cal line nettotal amt inc vat
                l.TotalAmtIncVat = Math.Round(l.TotalAmt + l.VatAmt, 3, MidpointRounding.AwayFromZero);
            }

            //sum 2 head 
            h.ItemDiscAmt = doc.Line.Where(o => o.ItemTypeID == "DISCOUNT" && o.Status != "K-REJECT" && o.IsLineActive == true).Sum(o => o.TotalAmtIncVat);
            h.LineDisc = (doc.Line.Where(o => o.ItemTypeID == "DISCOUNT" && o.Status != "K-REJECT" && o.IsLineActive == true).Sum(o => o.TotalAmt)) * -1;
            h.Qty = doc.Line.Where(o => o.ItemTypeID != "DISCOUNT" && o.Status != "K-REJECT" && o.IsLineActive == true).Sum(o => o.Qty);
            //h.CountLine = doc.Line.Where(o => o.IsActive == true && o.Status != "K-REJECT").Count();
            h.CountLine = doc.Line.Count();
            h.NetTotalAmt = doc.Line.Where(o => o.IsActive == true && o.Status != "K-REJECT" && o.IsLineActive == true).Sum(o => o.TotalAmt);
            h.NetTotalVatAmt = doc.Line.Where(o => o.IsActive == true && o.Status != "K-REJECT" && o.IsLineActive == true).Sum(o => o.VatAmt);
            h.NetTotalAmtIncVat = doc.Line.Where(o => o.IsActive == true && o.Status != "K-REJECT" && o.IsLineActive == true).Sum(o => o.TotalAmtIncVat);
            h.NetTotalAfterRound = h.NetTotalAmtIncVat - GetDecimalPart(h.NetTotalAmtIncVat);
            h.NetDiff = GetDecimalPart(h.NetTotalAmtIncVat);


            //copy head 2 line payment
            foreach (var l in doc.Payment) {
                l.BillID = h.BillID;
                l.INVID = h.INVID;
                l.FINVID = h.FINVID;
                l.ComID = h.ComID;
                l.RComID = h.RComID;
                l.BillAmt = h.NetTotalAmtIncVat;
                l.RoundAmt = h.NetTotalAfterRound;
                if (l.GetAmt >= l.RoundAmt) {//รับมากกว่ายอดเต็ม
                    l.PayAmt = l.RoundAmt;
                } else {//รับน้อยกว่ายอดเต็ม
                    l.PayAmt = l.GetAmt;
                }
                l.CreatedDate = l.CreatedDate == null ? DateTime.Now : l.CreatedDate;
                l.ModifiedDate = l.ModifiedDate == null ? l.CreatedDate : l.ModifiedDate;
            }




            //cal payment
            var change = doc.Payment.Where(o => o.IsLineActive == true).Sum(o => o.GetAmt) - Convert.ToDecimal(h.NetTotalAfterRound);
            var payCash = doc.Payment.Where(o => o.PaymentType == "CASH" && o.IsLineActive == true).FirstOrDefault();
            if (payCash != null) {
                payCash.ChangeAmt = change >= 0 ? change : 0;
            }


            //cal payby
            h.PayByOther = doc.Payment.Where(o => o.PaymentType != "CASH" && o.IsLineActive == true).Sum(o => o.PayAmt);
            h.PayByCash = doc.Payment.Where(o => o.PaymentType == "CASH" && o.IsLineActive == true).Sum(o => o.PayAmt);
            h.PayByVoucher = doc.Payment.Where(o => o.PaymentType == "VOUCHER" && o.IsLineActive == true).Sum(o => o.PayAmt);
            h.PayTotalAmt = doc.Payment.Where(o => o.IsLineActive == true).Sum(o => o.PayAmt);
            h.PayMethodCount = doc.Payment.Count();
            doc.Line.OrderBy(o => o.LineNum);

            return doc;
        }

        public static I_POSSaleUploadDoc CalDocSet(I_POSSaleUploadDoc doc) {
            //เวลาแก้ไขสามารถ copy  CalDocset ที่ param เป็น I_POSSaleSet ได้ทั้งหมด โดยไม่ต้องแก้ไขไร
            var h = doc.Head;
            //copy head 2 line
            decimal baseDisc = doc.Line.Where(o => o.DocTypeID != "DISCOUNT" && o.IsLineActive == true).Sum(o => o.BaseTotalAmt);
            foreach (var l in doc.Line) {
                //1.cal line base total amt 
                if (l.ItemTypeID == "DISCOUNT") {//  line ที่เป็น discount
                    //2. cal disc amt
                    if (l.DiscCalBy == "P") {
                        l.DiscAmt = (baseDisc * l.DiscPer) / 100;
                        //l.BaseTotalAmt = l.DiscAmt;

                    }
                    //3. cal disc per
                    if (l.DiscCalBy == "A") {
                        l.DiscPer = 0;
                        //l.BaseTotalAmt = l.DiscAmt;
                        if (baseDisc != 0) {
                            l.DiscPer = (100 * l.DiscAmt) / baseDisc;
                        }
                    }
                } else {//เป็นรายการขาย
                    l.BaseTotalAmt = l.Qty * l.Price;
                    l.TotalAmt = l.BaseTotalAmt;
                }
                //4.assign head 2 line
                l.INVID = h.INVID;
                l.BillID = h.BillID;
                l.INVID = h.INVID;
                l.FINVID = h.FINVID;
                l.BillDate = h.BillDate;
                l.ComID = h.ComID;
                l.RComID = h.RComID;
                l.MacNo = h.MacNo;
                l.DocTypeID = h.DocTypeID;
                l.CustID = h.CustomerID;
                l.TableID = h.TableID;
                l.TableName = h.TableName;
                l.VatRate = h.VatRate;
                l.VatTypeID = h.VatTypeID;
                l.CreatedBy = h.CreatedBy;
                l.CreatedDate = l.CreatedDate == null ? DateTime.Now : l.CreatedDate;
                l.ModifiedDate = l.ModifiedDate == null ? l.CreatedDate : l.ModifiedDate;
            }

            //5. cal head base total amt            
            doc.Head.BaseNetTotalAmt = doc.Line.Where(o => o.IsLineActive == true).Sum(o => o.BaseTotalAmt) - doc.Line.Where(o => o.IsLineActive == true).Sum(o => o.DiscAmt);
            //6. cal head disc amt
            if (doc.Head.DiscCalBy == "P") {
                doc.Head.OntopDiscAmt = (doc.Head.BaseNetTotalAmt * doc.Head.OntopDiscPer) / 100;
            }
            //7. cal head disc per
            if (doc.Head.DiscCalBy == "A") {
                if (doc.Head.BaseNetTotalAmt != 0) {
                    doc.Head.OntopDiscPer = (100 * doc.Head.OntopDiscAmt) / doc.Head.BaseNetTotalAmt;
                }
            }

            foreach (var l in doc.Line.Where(o => o.IsLineActive == true)) {
                //7.ตั้งต้น Ontop discount จาก Header ก่อนนำไปคำนวณใน Line
                l.OntopDiscPer = h.OntopDiscPer;
                l.OntopDiscAmt = Math.Round(h.OntopDiscAmt, 3, MidpointRounding.AwayFromZero);

                //8.cal line disc weight ontop percent & amt

                if (h.BaseNetTotalAmt != 0) {
                    l.OntopDiscAmt = Math.Round(((l.BaseTotalAmt - l.DiscAmt) * h.OntopDiscAmt) / h.BaseNetTotalAmt, 3, MidpointRounding.AwayFromZero);
                    l.OntopDiscPer = (l.BaseTotalAmt - l.DiscAmt) == 0 ? 0 : (100 * l.OntopDiscAmt) / (l.BaseTotalAmt - l.DiscAmt);
                }
                //9 cal line total amt 
                l.TotalAmt = Math.Round(l.BaseTotalAmt - l.DiscAmt - l.OntopDiscAmt, 3, MidpointRounding.AwayFromZero); ;
                //10.cal line vat amt
                l.VatAmt = Math.Round((l.TotalAmt * l.VatRate) / 100, 3, MidpointRounding.AwayFromZero);
                //11.cal line nettotal amt inc vat
                l.TotalAmtIncVat = Math.Round(l.TotalAmt + l.VatAmt, 3, MidpointRounding.AwayFromZero);
            }

            //sum 2 head 
            h.ItemDiscAmt = doc.Line.Where(o => o.ItemTypeID == "DISCOUNT" && o.Status != "K-REJECT" && o.IsLineActive == true).Sum(o => o.TotalAmtIncVat);
            h.LineDisc = (doc.Line.Where(o => o.ItemTypeID == "DISCOUNT" && o.Status != "K-REJECT" && o.IsLineActive == true).Sum(o => o.TotalAmt)) * -1;
            h.Qty = doc.Line.Where(o => o.ItemTypeID != "DISCOUNT" && o.Status != "K-REJECT" && o.IsLineActive == true).Sum(o => o.Qty);
            //h.CountLine = doc.Line.Where(o => o.IsActive == true && o.Status != "K-REJECT").Count();
            h.CountLine = doc.Line.Count();
            h.NetTotalAmt = doc.Line.Where(o => o.IsActive == true && o.Status != "K-REJECT" && o.IsLineActive == true).Sum(o => o.TotalAmt);
            h.NetTotalVatAmt = doc.Line.Where(o => o.IsActive == true && o.Status != "K-REJECT" && o.IsLineActive == true).Sum(o => o.VatAmt);
            h.NetTotalAmtIncVat = doc.Line.Where(o => o.IsActive == true && o.Status != "K-REJECT" && o.IsLineActive == true).Sum(o => o.TotalAmtIncVat);
            h.NetTotalAfterRound = h.NetTotalAmtIncVat - GetDecimalPart(h.NetTotalAmtIncVat);
            h.NetDiff = GetDecimalPart(h.NetTotalAmtIncVat);

            //copy head 2 line payment
            foreach (var l in doc.Payment) {
                l.BillID = h.BillID;
                l.INVID = h.INVID;
                l.FINVID = h.FINVID;
                l.ComID = h.ComID;
                l.RComID = h.RComID;
                l.BillAmt = h.NetTotalAmtIncVat;
                l.RoundAmt = h.NetTotalAfterRound;
                if (l.GetAmt >= l.RoundAmt) {//รับมากกว่ายอดเต็ม
                    l.PayAmt = l.RoundAmt;
                } else {//รับน้อยกว่ายอดเต็ม
                    l.PayAmt = l.GetAmt;
                }
                l.CreatedDate = l.CreatedDate == null ? DateTime.Now : l.CreatedDate;
                l.ModifiedDate = l.ModifiedDate == null ? l.CreatedDate : l.ModifiedDate;
            }

            //cal payment
            List<string> payVTC = new List<string> { "VOUCHER", "TRANSFER", "CREDIT" };
            var hasVoucherTranCredit = doc.Payment.Where(o => payVTC.Contains(o.PaymentType) && o.IsLineActive == true && o.IsActive).FirstOrDefault();
            var change = doc.Payment.Where(o => o.IsLineActive == true).Sum(o => o.GetAmt) - Convert.ToDecimal(h.NetTotalAfterRound);
            var payCash = doc.Payment.Where(o => o.PaymentType == "CASH" && o.IsLineActive == true).FirstOrDefault();
            if (payCash != null) {
                payCash.ChangeAmt = change >= 0 ? change : 0;
                if (hasVoucherTranCredit != null) {//มีชำระด้วย voucher ให้คำนวณ Paymat ของ cash ใหม่
                    payCash.PayAmt = payCash.GetAmt - payCash.ChangeAmt;
                }
            }

            //cal payby
            List<string> otherPayTypeInExcept = new List<string> { "CASH", "CREDIT" };  //ONLINE , TRANSFER , CREDIT , CASH , OTHER
            h.PayByOther = doc.Payment.Where(o => !otherPayTypeInExcept.Contains(o.PaymentType) && o.IsLineActive == true).Sum(o => o.PayAmt);
            h.PayByCash = doc.Payment.Where(o => o.PaymentType == "CASH" && o.IsLineActive == true).Sum(o => o.PayAmt);
            h.PayByCredit = doc.Payment.Where(o => o.PaymentType == "CREDIT" && o.IsLineActive == true).Sum(o => o.PayAmt);
            h.PayByVoucher = doc.Payment.Where(o => o.PaymentType == "VOUCHER" && o.IsLineActive == true).Sum(o => o.PayAmt);

            h.PayTotalAmt = doc.Payment.Where(o => o.IsLineActive == true).Sum(o => o.PayAmt);
            h.PayMethodCount = doc.Payment.Count();
            doc.Line.OrderBy(o => o.LineNum);

            return doc;
        }
        public static decimal GetDecimalPart(decimal number) {
            decimal result = (number - Math.Truncate(number));
            return result;
        }

        public I_POSSaleSet DeleteItem(I_POSSaleSet doc, string lineunq) {
            var line_update = doc.Line.Where(o => o.LineUnq == lineunq).FirstOrDefault();
            line_update.IsLineActive = false;
            line_update.ModifiedDate = DateTime.Now;
            doc = CalDocSet(doc);
            return doc;
        }

        public static I_POSSaleSet AddPayment(I_POSSaleSet doc, string paymentType, decimal getAmt) {

            doc = CalDocSet(doc);
            doc.PaymentActive = doc.Payment.Where(o => o.PaymentType == paymentType).FirstOrDefault();
            if (doc.PaymentActive != null) {
                //doc.PaymentActive = NewPayment(paymentType);
                doc.PaymentActive.PaymentTypeName = paymentType;
                doc.PaymentActive.GetAmt = getAmt;
                doc.PaymentActive.ModifiedDate = DateTime.Now;
                doc.PaymentActive.IsLineActive = true;
                //doc.Payment.Add(doc.PaymentActive);
            } else {
                doc.PaymentActive = NewPayment(paymentType);
                doc.PaymentActive.PaymentTypeName = paymentType;
                doc.PaymentActive.GetAmt = getAmt;
                doc.PaymentActive.IsLineActive = true;
                doc.Payment.Add(doc.PaymentActive);
            }

            doc = CalDocSet(doc);
            return doc;

        }
        public I_POSSaleSet RemovePayment(I_POSSaleSet doc, string paymentType) {
            var update_payment = doc.Payment.Where(o => o.PaymentType == paymentType).FirstOrDefault();
            if (update_payment != null) {
                update_payment.IsLineActive = false;
                update_payment.ModifiedDate = DateTime.Now;
            }
            doc = CalDocSet(doc);
            return doc;
        }
        #endregion

        #region New
        public static POS_SaleLineModel NewLine(I_POSSaleSet doc) {
            POS_SaleLineModel n = new POS_SaleLineModel();
            n.RComID = "";
            n.ComID = "";
            n.MacNo = "";
            n.LineNum = GenLineNum(doc);
            n.LineUnq = Guid.NewGuid().ToString().Substring(0, 10);
            n.RefLineUnq = "";
            n.RefLineNum = 0;
            n.BillID = "";
            n.INVID = "";
            n.FINVID = "";
            n.BelongToLineNum = "";
            n.DocTypeID = "";
            n.BillDate = DateTime.Now.Date;
            n.BillRefID = "";
            n.CustID = "";
            n.TableID = "";
            n.TableName = "";
            n.Barcode = "";
            n.ItemID = "";
            n.ItemName = "";
            n.ItemTypeID = "";
            n.ItemCateID = "";

            n.ItemGroupID = "";
            n.IsStockItem = true;
            n.Weight = 0;
            n.WUnit = "";
            n.Unit = "";
            n.Qty = 0;
            n.Cost = 0;
            n.Price = 0;
            n.PriceIncVat = 0;
            n.BaseTotalAmt = 0;
            n.TotalAmt = 0;
            n.VatAmt = 0;
            n.TotalAmtIncVat = 0;
            n.VatRate = 0;
            n.VatTypeID = "";
            n.OntopDiscAmt = 0;
            n.OntopDiscPer = 0;
            n.DiscPer = 0;
            n.DiscAmt = 0;
            n.DiscCalBy = "";
            n.IsFree = false;
            n.ShareGpPer = 0;
            n.IsOntopItem = false;
            n.PromotionID = "";
            n.PatternID = "";
            n.PaternValue = "";
            n.MatchingNumber = 1;
            n.ProPackCode = "";
            n.IsProCompleted = false;
            n.LocID = "";
            n.SubLocID = "";
            n.LotNo = "";
            n.SerialNo = "";
            n.BatchNo = "";
            n.Remark = "";
            n.Memo = "";
            n.ImgUrl = "hisotrylogo.png";
            n.ImageSource = "hisotrylogo.png";
            n.CreatedBy = "";
            n.CreatedDate = DateTime.Now;
            n.ModifiedDate = DateTime.Now;
            n.KitchenFinishCount = 0;
            n.Sort = n.LineNum;
            n.Status = "OK";
            n.IsLineActive = true;
            n.IsActive = true;
            return n;
        }
        public static POS_SalePaymentModel NewPayment(string paymentType) {
            POS_SalePaymentModel n = new POS_SalePaymentModel();
            n.RComID = "";
            n.ComID = "";
            n.LineUnq = Guid.NewGuid().ToString().Substring(0, 10);
            n.BillID = "";
            n.PaymentType = paymentType;
            n.INVID = "";
            n.FINVID = "";
            n.BillAmt = 0;
            n.RoundAmt = 0;
            n.GetAmt = 0;
            n.PayAmt = 0;
            n.ChangeAmt = 0;
            n.CreatedDate = DateTime.Now;
            n.ModifiedDate = DateTime.Now;
            n.IsLineActive = true;
            n.IsActive = true;
            return n;
        }

        public static int GenLineNum(I_POSSaleSet doc) {
            var next = 0;
            if (doc.Line.Count > 0) {
                next = doc.Line.Where(o => o.ItemTypeID != "DISCOUNT").Max(o => o.LineNum);
            }
            next = next + 10;
            return next;
        }
        #endregion

        public static List<SelectOption> ListTenderType() {
            return new List<SelectOption>() {
                new SelectOption(){ Description= "เงินสด", Value="CASH"},
                new SelectOption(){ Description= "โอนเงิน", Value="TRANSFER"},
                new SelectOption(){ Description= "บัตรเครดิต", Value="CREDIT"},
                new SelectOption(){ Description= "Voucher", Value="VOUCHER"},
            };
        }

        public static List<ShipTo> ListShipTo() {
            List<ShipTo> shipList = new List<ShipTo>();
            shipList.Add(new ShipTo {
                ShipToID = "",
                ShipToName = "หน้าร้าน",
                ShortID = "",
                UsePrice = "",
                ImageUrl = "img/SALE/frontstore.png"
            });
            shipList.Add(new ShipTo {
                ShipToID = "GRAB",
                ShipToName = "Grab",
                ShortID = "G",
                UsePrice = "GRAB",
                ImageUrl = "img/SALE/grab.png"
            });
            shipList.Add(new ShipTo {
                ShipToID = "SHOPEE",
                ShipToName = "Shopee",
                ShortID = "J",
                UsePrice = "SHOPEE",
                ImageUrl = "img/SALE/shopee_logo.png"
            });
            shipList.Add(new ShipTo {
                ShipToID = "LINEMAN",
                ShipToName = "Line Man",
                ShortID = "L",
                UsePrice = "LINEMAN",
                ImageUrl = "img/SALE/lineman.png"
            });

            shipList.Add(new ShipTo {
                ShipToID = "PANDA",
                ShipToName = "Food Padda",
                ShortID = "P",
                UsePrice = "PANDA",
                ImageUrl = "img/SALE/padda.png"
            });
            shipList.Add(new ShipTo {
                ShipToID = "ROBINHOOD",
                ShipToName = "Robinhood",
                ShortID = "R",
                UsePrice = "ROBINHOOD",
                ImageUrl = "img/SALE/robinhood.png"
            });
            shipList.Add(new ShipTo {
                ShipToID = "ONLINE",
                ShipToName = "Online",
                ShortID = "O",
                UsePrice = "ONLINE",
                ImageUrl = "img/SALE/online.png"
            });
            return shipList;

        }

    }
}
