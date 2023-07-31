using System.Text.Json;
using System.Text.Json.Serialization;
using RobotWasm.Shared.Data.GaDB;
using static RobotWasm.Shared.Data.ML.Shared.I_Result;
using System.Net.Http.Json;
using RobotWasm.Shared.Data.ML.Shared;
using Blazored.LocalStorage;
using RobotWasm.Shared.Data.DA;
using RobotWasm.Shared.Data.ML.Param;
using static RobotWasm.Shared.Data.DA.SOFuncService;
using Blazored.SessionStorage;
using RobotWasm.Shared.Data.ML.Promotion;
using RobotWasm.Shared.Data.ML.Order;

namespace RobotWasm.Client.Data.DA.Order.SO {
    public class SOService {
         
        private readonly HttpClient _http;
        private ILocalStorageService _localStorage;
        private ISessionStorageService _sessionStorage;
        public SOService(HttpClient Http, ILocalStorageService localStorage, ISessionStorageService sessionStorage) {
            _http = Http;
            _localStorage = localStorage;
            _sessionStorage= sessionStorage;
        }

        public I_SODocSet DocSet { get; set; }
        public I_PromotionSet PromotionSet { get; set; }
        //public List<ItemDesplay> ListProduct { get; set; } = new List<ItemDesplay>();
        I_PromotionSet promotionSet { get; set; } = new I_PromotionSet();
        public List<Promotions> PromotionList { get; set; } = new List<Promotions>();
 
        async public Task<I_SODocSet> GetDocSet(string docid, string rcom, string com) {
            docid = docid == null ? "" : docid;
            rcom = rcom == null ? "" : rcom;
            com = com == null ? "" : com;
            I_SODocSet? doc = new I_SODocSet();
            try {
                SODocParam p = new SODocParam { RCom = rcom, Com = com, OrdID = docid };
                string url = $"api/SO/GetDocSet";
                string strPayload = JsonSerializer.Serialize(p);
                var res = await _http.PostAsJsonAsync(url, strPayload);
                doc = JsonSerializer.Deserialize<I_SODocSet>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                }); 
            } catch (Exception ex) {
                var x = ex.Message;
            }
            return doc;
        }

        async public Task<int> ListNotificationStatus(string? username) {
            int output = 0;
            username = username == null ? "" : username;
            username = username.Replace(".", "%2E");
            try {
                var res = await _http.GetAsync($"api/SO/ListNotificationStatus?user={username}");
                output = JsonSerializer.Deserialize<int>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            } catch (Exception ex) {
                var x = ex.Message;
            }
            return output;
        }

        async public Task<List<vw_OSOHead>> ListDoc(I_SODocFiterSet f) {
            List<vw_OSOHead>? output = new List<vw_OSOHead>();
            try {
                f.LockShowInSale = f.LockShowInSale == null ?"": f.LockShowInSale;
                string url = $"api/SO/ListDoc";
                string strPayload = JsonSerializer.Serialize(f);
                var res = await _http.PostAsJsonAsync(url, f);
                output = JsonSerializer.Deserialize<List<vw_OSOHead>>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            } catch (Exception ex) {
                var x = ex.Message;
            }
            return output;
        }

        async public Task<I_PromotionSet> ListSaleItemSelect(string rcom, string com, string brandid, string cateid, string locid, string proid) {
            brandid = brandid == null ? "" : brandid;
            I_PromotionSet? output = new I_PromotionSet();
            try {
                ItemParam p =new ItemParam { RComID=rcom,ComID=com,BrandID=brandid,CateID=cateid,ProID=proid, LocID=locid };
                string strPayload = JsonSerializer.Serialize(p);
                string url = "api/SO/ListSaleItem";
                var res = await _http.PostAsJsonAsync(url, strPayload); 
                output = JsonSerializer.Deserialize<I_PromotionSet>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            } catch (Exception ex) {
                var x = ex.Message;
            }
            return output;
        }

        public static vw_OSOLine GetLineActive(int linenum, List<vw_OSOLine> line) {
            vw_OSOLine result = new vw_OSOLine();
            result = line.Where(o => o.LineNum == linenum).FirstOrDefault();
            return result;
        }

        async public Task<I_BasicResult> SaveSO(I_SODocSet doc, string action) {
            I_BasicResult? r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                doc = SOFuncService.CalDocSet(doc);
                string url = $"api/SO/SaveSO?action={action}";

                string strPayload = JsonSerializer.Serialize(doc);
                var res = await _http.PostAsJsonAsync(url, strPayload);

                r = JsonSerializer.Deserialize<I_BasicResult>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            } catch (Exception ex) {
                if (ex.InnerException != null) {
                    r.Message1 = ex.InnerException.Message;
                } else {
                    r.Message1 = ex.Message;
                }
            }
            return r;
        }

        async public Task<GeoThaiLand> GetGeoThailand(decimal? lat, decimal? lon) {
            GeoThaiLand? r = new();
            lat = lat == null ? 0 : lat;
            lon = lon == null ? 0 : lon;
            if (lat == 0 || lon == 0) {
                r = null;
                return r;
            }
            try {
                GeoParam p = new GeoParam { Latitude = lat, Longitude = lon };
                string url = $"api/SO/GetGeoThailand";

                string strPayload = JsonSerializer.Serialize(p);
                var res = await _http.PostAsJsonAsync(url, strPayload);

                r = JsonSerializer.Deserialize<GeoThaiLand>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            } catch (Exception ex) {

            }
            return r;
        }
        #region Delete

        async public Task<I_BasicResult> DeleteDoc(string docid, string rcom, string com, string modified_by) {
            I_BasicResult? result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                var res = await _http.GetAsync($"api/SO/DeleteDoc?docid={docid}&rcom={rcom}&com={com}&modified_by={modified_by}");
                result = JsonSerializer.Deserialize<I_BasicResult>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            } catch (Exception ex) {
                var x = ex.Message;
            }
            return result;
        }
        async public Task<I_BasicResult> LockCorder(int action) {
            //action =1=lock,0=unlock,2=getstatus
            I_BasicResult? result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                var res = await _http.GetAsync($"api/SO/LockCorder?action={action}");
                result = JsonSerializer.Deserialize<I_BasicResult>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            } catch (Exception ex) {
                var x = ex.Message;
            }
            return result;
        }
        public I_SODocSet DeleteLine(int linenum, I_SODocSet input) {

            try {
                input.OutputAction = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
                input.Line.RemoveAll(o => o.LineNum == linenum);
                input.Lot.RemoveAll(o => o.LineLineNum == linenum);
                input = SOFuncService.CalDocSet(input);
            } catch (Exception ex) {
                input.OutputAction = new I_BasicResult { Result = "fail", Message1 = ex.Message, Message2 = "" };
            }
            return input;
        }
        #endregion



        #region new

        public static List<SelectOption> ListDocStatus() {
            List<SelectOption> output = new List<SelectOption>();
            output.Add(new SelectOption { Sort = 1, Value = "", Description = "ALL" });
            output.Add(new SelectOption { Sort = 2, Value = "WAIT", Description = "WAIT" });
            output.Add(new SelectOption { Sort = 2, Value = "OPEN", Description = "OPEN" });
            output.Add(new SelectOption { Sort = 2, Value = "COMPLETED", Description = "Link Express" });
            //output.Add(new SelectOption { Sort = 3, Value = "CANCLE", Description = "CANCEL" });

            //output.Add(new SelectOption { Sort = 2, Value = "NEW", Description = "NEW" });
            //output.Add(new SelectOption { Sort = 3, Value = "PENDING", Description = "PENDING" });
            //output.Add(new SelectOption { Sort = 4, Value = "COMPLETED", Description = "COMPLETED" }); 
            return output;
        }

        //public static I_SODocFiterSet NewFilterSet() {
        //    I_SODocFiterSet n = new I_SODocFiterSet();
        //    n.DateFrom = DateTime.Now.Date.AddMonths(-1);
        //    n.DateTo = DateTime.Now.Date.AddMinutes(1);
        //    n.DocType = "";
        //    n.Status = "";
        //    n.SearchText = "";
        //    n.ShowActive = true;
        //    return n;
        //}

        //public static I_SODocSet NewTransaction(string rcom, string com, string doctype) {
        //    I_SODocSet n = new I_SODocSet();
        //    n.Head = NewHead(rcom, com, doctype);
        //    n.Line = new List<vw_OSOLine>();
        //    n.Lot = new List<vw_OSOLot>();
        //    n.LineActive = NewLine(rcom, com, n);
        //    n.LotActive = new vw_OSOLot();
        //    n.Log = new List<TransactionLog>();
        //    n.OutputAction = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
        //    return n;
        //}
        #region delete
        //public static OSOHead NewHead(string rcom, string com, string doctype) {
        //    OSOHead n = new OSOHead();

        //    n.OrdID = "";
        //    n.OrdDate = DateTime.Now.Date;
        //    n.RComID = rcom;
        //    n.ComID = com;
        //    n.INVID = "";
        //    n.INVDate = null;
        //    n.CustBrnID = "";
        //    n.CustBrnName = "";
        //    n.DocTypeID = doctype;
        //    n.CustID = "";
        //    n.CustName = "";
        //    n.CustAddr1 = "";
        //    n.CustAddr2 = "";
        //    n.CustMobile = "";
        //    n.CustEmail = "";
        //    n.PaymentType = "";
        //    n.RefDocID = "";
        //    n.ShipID = "";
        //    n.ShipDate = DateTime.Now.Date;
        //    n.AccGroupID = "";
        //    n.CustTaxID = "";
        //    n.POID = "";
        //    n.PODate = null;
        //    n.BillToCustID = "";
        //    n.BillAddr1 = "";
        //    n.BillAddr2 = "";
        //    n.ShipFrLocID = "";
        //    n.ShipFrSubLocID = "";
        //    n.SalesID1 = "";
        //    n.SalesID2 = "";
        //    n.Currency = "THB";
        //    n.RateExchange = 1;
        //    n.RateBy = "SP";
        //    n.RateDate = DateTime.Now.Date;
        //    n.TermID = "";
        //    n.PayDueDate = null;
        //    n.Qty = 0;
        //    n.QtyShip = 0;
        //    n.QtyInvoice = 0;
        //    n.QtyReturn = 0;
        //    n.CountLine = 0;
        //    n.QtyInvoicePending = 0;
        //    n.AmtInvoicePending = 0;
        //    n.AmtShipPending = 0;
        //    n.NetTotalAmt = 0;
        //    n.NetTotalVatAmt = 0;
        //    n.NetTotalAmtIncVat = 0;
        //    n.BaseNetTotalAmt = 0;
        //    n.OntopDiscPer = 0;
        //    n.OntopDiscAmt = 0;
        //    n.DiscCalBy = "A";
        //    n.VatRate = 0;
        //    n.VatTypeID = "7";
        //    n.Remark1 = "";
        //    n.Remark2 = "";

        //    n.PaymentMemo = "";

        //    n.IsPrint = false;
        //    n.PrintDate = null;
        //    n.IsLink = false;
        //    n.LinkDate = null;
        //    n.Status = "OPEN";
        //    n.Source = "BACK";
        //    n.CreatedBy = "";
        //    n.CreatedDate = DateTime.Now;
        //    n.ModifiedBy = "";
        //    n.ModifiedDate = null;
        //    n.IsActive = true;
        //    return n;
        //}

        //public static vw_OSOLine NewLine(string rcom, string com, I_SODocSet input) {
        //    vw_OSOLine n = new vw_OSOLine();
        //    n.OrdID = "";
        //    n.OrdDate = DateTime.Now.Date;
        //    n.RComID = rcom;
        //    n.ComID = com;
        //    n.DocTypeID = "";
        //    n.LineNum = GenLineNum(input);
        //    n.Sort = GenLineSort(input);
        //    n.PageBreak = false;
        //    n.ShipID = "";
        //    n.ShipLineNum = 0;
        //    n.INVDate = null;
        //    n.RefDocID = "";
        //    n.RefDocLineNum = 0;
        //    n.CustID = "";
        //    n.ItemID = "";
        //    n.ItemName = "";
        //    n.ItemTypeID = "";
        //    n.ItemTypeName = "";
        //    n.ItemCateID = "";
        //    n.ItemGroupID = "";
        //    n.ItemAccGroupID = "";
        //    n.IsStockItem = false;
        //    n.Weight = 0;
        //    n.WUnit = "";
        //    n.Qty = 1;
        //    n.QtyShip = 0;
        //    n.QtyInvoice = 0;
        //    n.QtyShipPending = 0;
        //    n.QtyInvoicePending = 0;
        //    n.AmtShipPending = 0;
        //    n.AmtInvoicePending = 0;
        //    n.Unit = "";
        //    n.Packaging = "";
        //    n.Price = 0;
        //    n.PriceIncVat = 0;
        //    n.TotalAmt = 0;
        //    n.VatAmt = 0;
        //    n.TotalAmtIncVat = 0;
        //    n.VatRate = input.Head.VatRate;
        //    n.VatTypeID = input.Head.VatTypeID;
        //    n.BaseTotalAmt = 0;
        //    n.OntopDiscAmt = 0;
        //    n.OntopDiscPer = 0;
        //    n.DiscPer = 0;
        //    n.DiscAmt = 0;
        //    n.DiscCalBy = "A";
        //    n.LocID = "";
        //    n.SubLocID = "";
        //    n.PackagingNo = "";
        //    n.LotNo = "";
        //    n.SerialNo = "";
        //    n.BatchNo = "";
        //    n.PointID = "";
        //    n.PointName = "";
        //    n.Remark1 = "";
        //    n.Remark2 = "";
        //    n.Status = "NEW";
        //    n.Sort = 1;
        //    n.ImageUrl = "";
        //    n.PageBreak = false;
        //    n.IsComplete = false;
        //    n.IsActive = true;
        //    n.ImageUrl = "/img/watch2.png";
        //    return n;
        //}

        //public static vw_OSOLot NewLot(string rcom, string com, int line, I_SODocSet input) {
        //    vw_OSOLot n = new vw_OSOLot();
        //    n.RComID = "";
        //    n.ComID = "";
        //    n.OrdID = "";
        //    n.LineLineNum = line;
        //    n.LineNum = GenLotLineNum(input);
        //    n.DocTypeID = "";
        //    n.OrdDate = DateTime.Now.Date;
        //    n.CustID = "";
        //    n.ItemID = "";
        //    n.IsStockItem = true;
        //    n.Qty = 0;
        //    n.Unit = "";
        //    n.LocID = "";
        //    n.LotNo = "";
        //    n.SerialNo = "";
        //    n.Status = "NEW";
        //    n.IsActive = true;
        //    return n;
        //}

        //public static List<OSOLine> Convert2SOLine(List<vw_OSOLine> input) {
        //    List<OSOLine> output = new List<OSOLine>();
        //    foreach (var i in input) {
        //        OSOLine n = new OSOLine();
        //        n.ID = i.ID;
        //        n.RComID = i.RComID;
        //        n.ComID = i.ComID;
        //        n.OrdID = i.OrdID;
        //        n.LineNum = i.LineNum;
        //        n.DocTypeID = i.DocTypeID;
        //        n.OrdDate = i.OrdDate;
        //        n.INVID = i.INVID;
        //        n.INVDate = i.INVDate;
        //        n.RefDocID = i.RefDocID;
        //        n.RefDocLineNum = i.RefDocLineNum;
        //        n.CustID = i.CustID;
        //        n.ItemID = i.ItemID;
        //        n.ItemName = i.ItemName;
        //        n.ItemTypeID = i.ItemTypeID;
        //        n.ItemTypeName = i.ItemTypeName;
        //        n.ItemCateID = i.ItemCateID;
        //        n.ItemGroupID = i.ItemGroupID;
        //        n.ItemAccGroupID = i.ItemAccGroupID;
        //        n.IsStockItem = i.IsStockItem;
        //        n.Weight = i.Weight;
        //        n.WUnit = i.WUnit;
        //        n.Qty = i.Qty;
        //        n.QtyShip = i.QtyShip;
        //        n.QtyInvoice = i.QtyInvoice;
        //        n.QtyShipPending = i.QtyShipPending;
        //        n.QtyInvoicePending = i.QtyInvoicePending;
        //        n.AmtShipPending = i.AmtShipPending;
        //        n.AmtInvoicePending = i.AmtInvoicePending;
        //        n.Unit = i.Unit;
        //        n.Packaging = i.Packaging;
        //        n.Price = i.Price;
        //        n.PriceIncVat = i.PriceIncVat;
        //        n.BaseTotalAmt = i.BaseTotalAmt;
        //        n.TotalAmt = i.TotalAmt;
        //        n.VatAmt = i.VatAmt;
        //        n.TotalAmtIncVat = i.TotalAmtIncVat;
        //        n.VatRate = i.VatRate;
        //        n.VatTypeID = i.VatTypeID;
        //        n.OntopDiscAmt = i.OntopDiscAmt;
        //        n.OntopDiscPer = i.OntopDiscPer;
        //        n.DiscPer = i.DiscPer;
        //        n.DiscAmt = i.DiscAmt;
        //        n.DiscCalBy = i.DiscCalBy;
        //        n.LocID = i.LocID;
        //        n.SubLocID = i.SubLocID;
        //        n.PackagingNo = i.PackagingNo;
        //        n.LotNo = i.LotNo;
        //        n.SerialNo = i.SerialNo;
        //        n.BatchNo = i.BatchNo;
        //        n.Remark1 = i.Remark1;
        //        n.Remark2 = i.Remark2;
        //        n.PointID = i.PointID;
        //        n.PointName = i.PointName;
        //        n.ShipID = i.ShipID;
        //        n.ShipLineNum = i.ShipLineNum;
        //        n.Status = i.Status;
        //        n.IsComplete = i.IsComplete;
        //        n.Sort = i.Sort;
        //        n.PageBreak = i.PageBreak;
        //        n.IsActive = i.IsActive;
        //        output.Add(n);
        //    }

        //    return output;
        //}
        //public static List<OSOLot> Convert2SOLot(List<vw_OSOLot> input) {
        //    List<OSOLot> output = new List<OSOLot>();
        //    foreach (var i in input) {
        //        OSOLot n = new OSOLot();
        //        n.ID = i.ID;
        //        n.RComID = i.RComID;
        //        n.ComID = i.ComID;
        //        n.OrdID = i.OrdID;
        //        n.LineLineNum = i.LineLineNum;
        //        n.LineNum = i.LineNum;
        //        n.DocTypeID = i.DocTypeID;
        //        n.OrdDate = i.OrdDate;
        //        n.CustID = i.CustID;
        //        n.ItemID = i.ItemID;
        //        n.IsStockItem = i.IsStockItem;
        //        n.Qty = i.Qty;
        //        n.Unit = i.Unit;
        //        n.LocID = i.LocID;
        //        n.LotNo = i.LotNo;
        //        n.SerialNo = i.SerialNo;
        //        n.IsActive = i.IsActive;

        //        output.Add(n);
        //    }

        //    return output;
        //}

        #endregion

        //public static void AddLine(string rcom, string com, I_SODocSet input) {
        //    ClearPendingLine(input);
        //    input.Line.Add(NewLine(rcom, com, input));
        //    ClearPendingLot(input);
        //    input.LineActive = input.Line.Where(o => o.Status == "NEW").OrderByDescending(o => o.LineNum).FirstOrDefault();
        //    //ListDocLine();
        //}

        //public static void AddLot(string rcom, string com, I_SODocSet input) {
        //    input.Lot.Add(NewLot(rcom, com, input.LineActive.LineNum, input));
        //}

        //public static void ClearPendingLine(I_SODocSet input) {
        //    try {
        //        var r1 = input.Line.RemoveAll(o => o.Status == "NEW");
        //        input.OutputAction = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
        //    } catch (Exception ex) {
        //        input.OutputAction = new I_BasicResult { Result = "fail", Message1 = ex.Message, Message2 = "" };
        //    }
        //}

        //public static void ClearPendingLot(I_SODocSet input) {
        //    try {
        //        var r1 = input.Lot.RemoveAll(o => o.Status == "NEW");
        //        input.OutputAction = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
        //    } catch (Exception ex) {
        //        input.OutputAction = new I_BasicResult { Result = "fail", Message1 = ex.Message, Message2 = "" };
        //    }
        //}

        //public static int GenLineNum(I_SODocSet input) {
        //    int result = 10;
        //    try {
        //        var max_linenum = input.Line.Max(o => o.LineNum);
        //        result = max_linenum + 10;
        //    } catch { }
        //    return result;
        //}
        //public static int GenLotLineNum(I_SODocSet input) {
        //    int result = 10;
        //    try {
        //        var max_linenum = input.Lot.Max(o => o.LineNum);
        //        result = max_linenum + 10;
        //    } catch { }
        //    return result;
        //}
        //public static int GenLineSort(I_SODocSet input) {
        //    int result = 1;
        //    try {
        //        var max_sort = input.Line.Max(o => o.Sort);
        //        result = Convert.ToInt32(max_sort) + 1;
        //    } catch { }
        //    return result;
        //}
        #endregion

        #region Filter
        public static I_SODocFiterSet NewFilterSet() {
            I_SODocFiterSet n = new I_SODocFiterSet();
            var begin_month = DateTime.Now.Date.AddMonths(-1);

            n.DateFrom = new DateTime(begin_month.Year, begin_month.Month, 1);
            n.DateTo = DateTime.Now.Date;
            n.DocType = "";
            n.Status = "";
            n.SearchText = "";
            n.ShowActive = true;
            return n;
        }
        async public void SetSessionFiterSet(I_SODocFiterSet data) {
            JsonSerializerOptions jso = new JsonSerializerOptions();
            jso.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
            string json = JsonSerializer.Serialize(data, jso);
            await _sessionStorage.SetItemAsync("so_Fiter", json);
        }
        async public Task<I_SODocFiterSet>? GetSessionFiterSet() {
            I_SODocFiterSet? result = NewFilterSet();
            var strdoc = await _sessionStorage.GetItemAsync<string>("so_Fiter");
            if (strdoc != null) {
                result = JsonSerializer.Deserialize<I_SODocFiterSet>(strdoc, new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            } else {
                result = NewFilterSet();
            }
            return result;
        }
        #endregion

        #region Promotion
        async public Task<List<Promotions>> ListPromotion(string rcom, string com,DateTime tranDate) {
            List<Promotions>? result = new List<Promotions>();
            try {
                PromotionParam p = new PromotionParam { RComID = rcom, ComID = com, TranDate= tranDate };
                string url = $"api/SO/ListPromotion";

                string strPayload = JsonSerializer.Serialize(p);
                var res = await _http.PostAsJsonAsync(url, strPayload);

                result = JsonSerializer.Deserialize<List<Promotions>>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            } catch (Exception ex) {

            }
            return result;
        }
     async public Task<I_PromotionSet> GetPromotion(string rcom, string com, string brandid, string cateid, string locid, string proid) {
            I_PromotionSet? result = new I_PromotionSet();
            try {

                ItemParam p = new ItemParam { RComID = rcom, ComID = com, ProID= proid ,BrandID=brandid,CateID=cateid,LocID=locid,ItemID=proid};
                string url = $"api/SO/ListSaleItem";

                string strPayload = JsonSerializer.Serialize(p);
                var res = await _http.PostAsJsonAsync(url, strPayload);

                result = JsonSerializer.Deserialize<I_PromotionSet>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            } catch (Exception ex) {
                var x = ex.Message;
            }
            return result;
        }
        #endregion
      
    }
}
