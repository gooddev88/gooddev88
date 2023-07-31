using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Robot.Data.DA;
using Robot.Data.DA.API.APP;
using Robot.Data.GADB.TT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Robot.Data.ML.I_Result;
using static Robot.Data.DA.API.APP.SyncMasterDataService;
using static Robot.Data.DA.API.APP.SyncSalesService;

namespace Robot.Controllers.APP {
    [Route("api/[controller]")]
    [ApiController]
    public class SyncDataController : ControllerBase {

        private SyncMasterDataService _syncMasterDataService;
        public SyncDataController(SyncMasterDataService syncMasterDataService) {
            _syncMasterDataService = syncMasterDataService;
        }


        #region Master Data
        [HttpGet("GetMasterData")]
     async   public Task<I_MasterDataSet> GetMasterData(string rcom, string com, string username) {
            I_MasterDataSet result = new I_MasterDataSet();
            try {
                result = await Task.Run(()=> _syncMasterDataService.GetMasterData(rcom, com, username));
                result.MyXDate = DateTime.Now.Date;
            } catch { }
            return result;
        }


        [HttpGet("ListStore")]
        public List<CompanyInfo> ListStore(string rcom, string username) {
            List<CompanyInfo> result = null;
            try {
                //doctype : PICKPROD / PICKSPARE / RETURN
                result = CompanyService.ListCompanyByUserPermission(rcom, username);
            } catch (Exception ex) {
                var r = ex.Message;
            }
            return result;
        }
        #endregion

        #region Sale transaction
        [HttpPost("UploadPOSSale")]
        public I_POSSalesUploadSet UploadPOSSale([FromBody] I_POSSalesUploadSet data) {

            try {
                data.POSDocSet = SyncSalesService.UploadSaleTransactionV2(data.POSDocSet);
                data.IDGenerator = SyncSalesService.UploadIDGenerator(data.IDGenerator);
            } catch (Exception ex) {
                data.OutputAction.Result = "fail";
                if (ex.InnerException != null) {
                    data.OutputAction.Message1 = data.OutputAction.Message1 + " " + ex.InnerException.ToString();
                } else {
                    data.OutputAction.Message1 = data.OutputAction.Message1 + " " + ex.Message;
                }
            }
            return data;
        }
        [HttpPost("UploadPOSSaleLog")]
        public I_BasicResult UploadPOSSaleLog([FromBody] List<POS_SaleLog> data) {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };      
                try {
                if (data.Count == 0) {
                    return r;
                }
                var first_row = data.FirstOrDefault();
                var rcom = first_row.RComID;
                var com = first_row.ComID;
                var billid = first_row.BillID;
                int saveNo = 1;
                using (GAEntities db = new GAEntities()) {
                        var getLatestSaveNo = db.POS_SaleLog.Where(o => o.RComID == rcom && o.ComID == com && o.BillID == billid).OrderByDescending(o => o.SaveNo).FirstOrDefault();
                        if (getLatestSaveNo != null) {
                            saveNo = getLatestSaveNo.SaveNo + 1;
                        }
                        foreach (var l in data) {
                            POS_SaleLog n = new POS_SaleLog {
                                RComID = l.RComID,
                                SaveNo = saveNo,
                                BillID = l.BillID,
                                ComID = l.ComID,
                                LineUnq = l.LineUnq,
                                CreatedBy = l.CreatedBy,
                                CreatedByApp = l.CreatedByApp,
                                CreatedDate = l.CreatedDate,
                                IsActive = l.IsActive,
                                ItemID = l.ItemID,
                                ItemName = l.ItemName,
                                LineNum = l.LineNum,
                                MacNo = l.MacNo,
                                Price = l.Price,
                                Qty = l.Qty,
                                UploadedDate = DateTime.Now,
                                TotalAmt = l.TotalAmt
                            };
                            db.POS_SaleLog.Add(n);
                        }
                        db.SaveChanges();
                    }
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
  
        [HttpPost("DownloadPOSSale")]
            public I_POSSalesUploadSet DownloadPOSSale([FromBody] I_FilterSet data) {
                I_POSSalesUploadSet result = new I_POSSalesUploadSet();
                result.POSDocSet = new List<I_POSSaleUploadDoc>();
                result.IDGenerator = new List<IDGeneratorModel>();
                result.OutputAction = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };

                DateTime limitDate = DateTime.Now.Date.AddDays(-2);
                try {
                    using (GAEntities db = new GAEntities()) {
                        #region update null created date
                        var updateCreatedDate = db.IDGenerator.Where(o => o.FuncID == "SALE" && o.CreatedDate == null && o.RComID == data.RComID).ToList();
                        foreach (var u in updateCreatedDate) {
                            u.CreatedDate = DateTime.Now;
                        }
                        db.SaveChanges();
                        #endregion

                        var head = db.POS_SaleHead.Where(o => o.RComID == data.RComID && o.ComID == data.ComID && o.BillDate >= data.Begin && o.BillDate <= data.End).ToList();

                        List<string> billIds = head.Select(o => (string)o.BillID).ToList();

                        var lines = db.POS_SaleLine.Where(o => o.RComID == data.RComID && o.ComID == data.ComID && billIds.Contains(o.BillID)).ToList();

                        //var lines = db.POS_SaleLine.Take(10).ToList();
                        var payments = db.POS_SalePayment.Where(o => o.RComID == data.RComID && o.ComID == data.ComID && billIds.Contains(o.BillID)).ToList();
                        var idsgen = db.IDGenerator.Where(o => o.RComID == data.RComID && o.ComID == data.ComID && o.FuncID == "SALE" && o.CreatedDate >= limitDate).ToList();
                        result.IDGenerator = SyncSalesService.CopyIDRunningToModel(idsgen);
                        foreach (var h in head) {
                            var n = new I_POSSaleUploadDoc();
                            n.Head = h;
                            n.Head.IsLink = true;
                            n.Head.LinkDate = n.Head.LinkDate == null ? h.CreatedDate : n.Head.LinkDate;
                            n.Head.LinkBy = n.Head.LinkBy == null ? h.CreatedBy : n.Head.LinkBy;
                            n.Line = lines.Where(o => o.BillID == h.BillID).ToList();
                            n.Payment = payments.Where(o => o.BillID == h.BillID).ToList();
                            result.POSDocSet.Add(n);
                        }

                    }
                } catch (Exception ex) {
                    result.OutputAction.Result = "fail";

                    if (ex.InnerException != null) {
                        result.OutputAction.Message1 = ex.InnerException.ToString();
                    } else {
                        result.OutputAction.Message1 = ex.Message;
                    }
                }
                return result;
            }
            [HttpPost("DownloadPOSSaleByLatest")]
            public I_POSSalesUploadSet DownloadPOSSaleByLatest([FromBody] I_FilterSet data) {
                I_POSSalesUploadSet result = new I_POSSalesUploadSet();
                result.POSDocSet = new List<I_POSSaleUploadDoc>();

                result.OutputAction = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
                DateTime limitDate = DateTime.Now.Date.AddDays(-2);
                DateTime moData = data.Begin.AddTicks(-(data.Begin.Ticks % 10000000));
                try {
                    using (GAEntities db = new GAEntities()) {
                        var getbillIds = db.POS_SaleRefresh.Where(o => o.ComID == data.ComID && o.RComID == data.RComID && o.LatestModifiedDate > moData).Select(o => o.BillID).ToList();
                        var head = db.POS_SaleHead.Where(o => o.RComID == data.RComID && o.ComID == data.ComID && getbillIds.Contains(o.BillID)).ToList();
                        List<string> billIds = head.Select(o => (string)o.BillID).ToList();
                        var lines = db.POS_SaleLine.Where(o => o.RComID == data.RComID && o.ComID == data.ComID && billIds.Contains(o.BillID)).ToList();
                        var payments = db.POS_SalePayment.Where(o => o.RComID == data.RComID && o.ComID == data.ComID && billIds.Contains(o.BillID)).ToList();
                        var idsgen = db.IDGenerator.Where(o => o.RComID == data.RComID && o.ComID == data.ComID && o.FuncID == "SALE" && o.CreatedDate >= limitDate).ToList();
                        result.IDGenerator = SyncSalesService.CopyIDRunningToModel(idsgen);
                        foreach (var h in head) {
                            var n = new I_POSSaleUploadDoc();
                            n.Head = h;
                            n.Head.IsLink = true;
                            n.Head.LinkDate = n.Head.LinkDate == null ? h.CreatedDate : n.Head.LinkDate;
                            n.Head.LinkBy = n.Head.LinkBy == null ? h.CreatedBy : n.Head.LinkBy;
                            n.Line = lines.Where(o => o.BillID == h.BillID).ToList();
                            n.Payment = payments.Where(o => o.BillID == h.BillID).ToList();
                            result.POSDocSet.Add(n);
                        }
                    }
                } catch (Exception ex) {
                    result.OutputAction.Result = "fail";
                    if (ex.InnerException != null) {
                        result.OutputAction.Message1 = ex.InnerException.ToString();
                    } else {
                        result.OutputAction.Message1 = ex.Message;
                    }
                }
                return result;
            }
            [HttpPost("DownloadPOSSaleByLatestV2")]
            public I_POSSalesUploadSet DownloadPOSSaleByLatestV2([FromBody] List<I_FilterLatestTransaction> data) {
                //ใช้แทน V1 แล้วสามารถลบได้
                I_POSSalesUploadSet result = new I_POSSalesUploadSet();
                result.POSDocSet = new List<I_POSSaleUploadDoc>();

                result.OutputAction = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };

                var com = data.FirstOrDefault().ComID;
                var rcom = data.FirstOrDefault().RComID;
                var mac = data.FirstOrDefault().MacNo;
                DateTime limitDate = data.FirstOrDefault().limitDate;
                try {
                    using (GAEntities db = new GAEntities()) {
                        var macregis = db.MacRegister.Where(o => o.RComID == rcom && o.ComID == com && o.MacID != "" && o.IsActive).Select(o => o.MacID).ToList();
                        macregis.Add("Z");
                        foreach (var m in macregis) {
                        try {
                            var client_latest = data.Where(o => o.MacNo == m).FirstOrDefault();
                            List<string> bill_ids = new List<string>();
                            if (client_latest == null) {//ไม่มีบิลล่าสุดชอง mac นี้ในเครื่อง client
                                bill_ids = db.POS_SaleRefresh.Where(o => o.ComID == com
                                    && o.RComID == rcom
                                    && o.MacID == m
                                    && o.LatestModifiedDate >= limitDate).Select(o => o.BillID).Distinct().ToList();
                            } else {
                                bill_ids = db.POS_SaleRefresh.Where(o => o.ComID == com
                                            && o.RComID == rcom
                                            && o.MacID == m
                                            && o.LatestModifiedDate >= client_latest.LatestDate).Select(o => o.BillID).Distinct().ToList();
                            }
                            var head = db.POS_SaleHead.Where(o => o.RComID == rcom && o.ComID == com && bill_ids.Contains(o.BillID)).ToList();
                            List<string> billIds = head.Select(o => (string)o.BillID).ToList();
                            var lines = db.POS_SaleLine.Where(o => o.RComID == rcom && o.ComID == com && billIds.Contains(o.BillID)).ToList();
                            var payments = db.POS_SalePayment.Where(o => o.RComID == rcom && o.ComID == com && billIds.Contains(o.BillID)).ToList();
                            var idsgen = db.IDGenerator.Where(o => o.RComID == rcom && o.ComID == com && o.FuncID == "SALE" && o.CreatedDate >= limitDate).ToList();

                            result.IDGenerator = SyncSalesService.CopyIDRunningToModel(idsgen);
                            foreach (var h in head) {
                                var n = new I_POSSaleUploadDoc();
                                n.Head = h;
                                n.Head.IsLink = true;
                                n.Head.LinkDate = n.Head.LinkDate == null ? h.CreatedDate : n.Head.LinkDate;
                                n.Head.LinkBy = n.Head.LinkBy == null ? h.CreatedBy : n.Head.LinkBy;
                                n.Line = lines.Where(o => o.BillID == h.BillID).ToList();
                                n.Payment = payments.Where(o => o.BillID == h.BillID).ToList();
                                result.POSDocSet.Add(n);
                            }
                        } catch (Exception) {
                             
                        }
                        

                        }

                    }
                } catch (Exception ex) {
                    result.OutputAction.Result = "fail";
                    if (ex.InnerException != null) {
                        result.OutputAction.Message1 = ex.InnerException.ToString();
                    } else {
                        result.OutputAction.Message1 = ex.Message;
                    }
                }
                return result;
            }
            [HttpPost("UpdateMacRegister")]//จะยกเลิกใช้อันนี้แล้ว ใช้ Update macRegister V2 แทน
            public I_BasicResult UpdateMacRegister([FromBody] MacRegister data) {
                I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
                try {
                    using (GAEntities db = new GAEntities()) {
                        var exist = db.MacRegister.Where(o => o.MacID == data.MacID
                                                              && o.IsActive
                                                              && o.IsUse
                                                              && o.RComID == data.RComID
                                                              && o.ComID == data.ComID
                                                               ).FirstOrDefault();
                        if (!data.IsUse) {
                            if (exist != null) {
                                db.MacRegister.Remove(exist);
                            }
                        } else {
                            db.MacRegister.Add(data);
                        }
                        db.SaveChanges();
                    }
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

            [HttpPost("UpdateMacRegisterV2")]
            public I_BasicResult UpdateMacRegisterV2([FromBody] MacRegister data) {
                I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
                try {
                    r = MacService.UpdateMacRegisterV2(data);
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

            [HttpGet("ListMacRegister")]
            public List<MacRegister> ListMacRegister(string rcom, string com) {
                List<MacRegister> MacAvl = new List<MacRegister>();
                try {
                    MacAvl = MacService.ListMacRegister(rcom, com);
                } catch (Exception ex) {

                }
                return MacAvl;
            }
            #endregion
        }
    }
