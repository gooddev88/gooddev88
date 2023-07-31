using Robot.Data.ML;
using Robot.Data.GADB.TT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Robot.Data.ML.I_Result;

namespace Robot.Data.DA {
    public class POSItemService {


        public class I_ItemSet
        {
            public ItemInfo Info { get; set; }
            public List<ItemInfo> LineItem { get; set; }
            public List<ItemPriceInfo> ItemPrice { get; set; }
            public List<vw_ItemPriceInfo> VItemPrice { get; set; }
            public List<ItemInPointRate> ItemPointRate { get; set; }
            public List<TransactionLog> Log { get; set; }
            public List<XFilesRef> Files { get; set; }
            public I_BasicResult OutputAction { get; set; }
        }


        public I_ItemSet DocSet { get; set; }

        public POSItemService() {
            DocSet = NewTransaction("","");
        }

        //public class SelectOption {
        //    public string Value { get; set; }
        //    public string Desc { get; set; }
        //}

        public class I_ShipTo
        {
            public string ShipToID { get; set; }
            public string ShipToName { get; set; }
        }

        #region Get&ListData

        public I_ItemSet GetDocSet(string docid,string rcom) {
            I_ItemSet n = NewTransaction("", rcom);
            using (GAEntities db = new GAEntities()) {
                n.Info = db.ItemInfo.Where(o => o.ItemID == docid && o.RCompanyID == rcom).FirstOrDefault();
                n.ItemPrice = db.ItemPriceInfo.Where(o => o.ItemID == docid && o.RCompanyID == rcom && o.IsActive).ToList();
                n.VItemPrice = db.vw_ItemPriceInfo.Where(o => o.ItemID == docid && o.RCompanyID == rcom && o.IsActive).ToList();
                n.ItemPointRate = db.ItemInPointRate.Where(o => o.ItemID == docid && o.RComID == rcom && o.IsActive).OrderByDescending(o => o.DateEnd).ToList();
                //n.Files = GetFileInDoc(docid);
                n.Log = db.TransactionLog.Where(o => o.TransactionID == docid && o.TableID == "ITEM").OrderBy(o => o.CreatedDate).ToList();
            }
            return n;
        }

        public static ItemInfo GetItemInfo(string rcom, string itemid)
        {
            ItemInfo ven = new ItemInfo();
            using (GAEntities db = new GAEntities())
            {
                ven = db.ItemInfo.Where(o => o.RCompanyID == rcom && o.ItemID == itemid).FirstOrDefault();
            }
            return ven;
        }

        public static List<SelectOption> ListItemForSelect(string rcom, List<string> types)
        {
            List<SelectOption> result = new List<SelectOption>();
            using (GAEntities db = new GAEntities())
            {
                var items = db.ItemInfo.Where(o => o.RCompanyID == rcom
                                                            && types.Contains(o.TypeID)
                                                            && o.IsActive == true
                                                            ).OrderBy(o => o.TypeID).ThenBy(o => o.Name1).ToList();
                int i = 0;
                foreach (var v in items)
                {
                    i++;
                    SelectOption n = new SelectOption();
                    n.IsSelect = true;
                    n.Value = v.ItemID;
                    n.Description = v.Name1;
                    n.Sort = i;
                    result.Add(n);
                }

            }
            return result;
        }

        public static List<vw_ItemInfo> ListDoc(string Search,string rcom,string doctype,bool ShowActive, int skip, int take)
        {

            List<vw_ItemInfo> result = new List<vw_ItemInfo>();
            using (GAEntities db = new GAEntities())
            {

                result = db.vw_ItemInfo.Where(o =>
                                                                (o.ItemID.Contains(Search)
                                                                        || o.Name1.Contains(Search)
                                                                        || o.Group1ID.Contains(Search)
                                                                        || o.Group1Name.Contains(Search)
                                                                        || o.TypeID.Contains(Search)
                                                                        || o.TypeName.Contains(Search)
                                                                        || Search == "")
                                                                && o.ItemID != "DEFAULTMENU"
                                                                && o.IsSysData == false
                                                                && o.RCompanyID == rcom
                                                                && (o.TypeID == doctype || doctype == "")
                                                                && (o.IsActive == ShowActive
                                                        )
                                                        ).OrderBy(o => o.TypeID).ThenBy(o => o.ItemID).Skip(skip).Take(take).ToList();

            }

            return result;
        }

        //public List<XFilesRef> GetFileInDoc(string docId)
        //{
        //    List<XFilesRef> files = new List<XFilesRef>();
        //    using (GAEntities db = new GAEntities())
        //    {
        //        files = db.XFilesRef.Where(o => o.DocID == docId).OrderBy(o => o.DocLineNum).ThenBy(o => o.ID).ToList();

        //        foreach (var f in files)
        //        {
        //            f.Remark = XFilesService.GetFileRefByDocAndTableSource2B64(f.FileID, false);
        //        }
        //    }
        //    return files;
        //}

        //public static List<vw_ItemPriceInfo> ListItemPrice(ItemPriceList.I_ItemPriceSet filter)
        //{
        //    filter.Search = filter.Search.Trim().ToLower();
        //    int UseLevel = 0;
        //    if (filter.UseLevel != "")
        //    {
        //        UseLevel = Convert.ToInt32(filter.UseLevel);
        //    }


        //    List<vw_ItemPriceInfo> result = new List<vw_ItemPriceInfo>();
        //    using (GAEntities db = new GAEntities())
        //    {
        //        result = db.vw_ItemPriceInfo.Where(o =>
        //                                (
        //                                    o.ItemID.ToLower().Contains(filter.Search)
        //                                    || o.CompanyID.ToLower().Contains(filter.Search)
        //                                    || o.ItemName.ToLower().Contains(filter.Search)
        //                                    || o.CompanyName.ToLower().Contains(filter.Search)
        //                                    || o.CustID.ToLower().Contains(filter.Search)
        //                                    || filter.Search == ""
        //                                )
        //                                && (o.ItemID == filter.ItemID || filter.ItemID == "")
        //                                && (o.CompanyID == filter.Company || filter.Company == "")
        //                                && (o.PriceTaxCondType == filter.PriceTaxcon || filter.PriceTaxcon == "")
        //                                && (o.UseLevel == UseLevel || filter.UseLevel == "")
        //                                && (o.CustID == filter.ShipTo || filter.ShipTo == "")
        //                                && o.IsActive == true
        //                                ).OrderBy(o => o.ItemID).ThenByDescending(o => o.CompanyID).ToList();
        //    }
        //    return result;

        //}

        public static ItemInfo GetItem(string itemId,string rcom)
        {
            ItemInfo result = new ItemInfo();
            using (GAEntities db = new GAEntities())
            {
                result = db.ItemInfo.Where(o => o.ItemID == itemId && o.RCompanyID == rcom).FirstOrDefault();
            }
            return result;
        }

        public static List<vw_ItemInfo> ListViewItemByType(string typeID,string rcom)
        {
            List<vw_ItemInfo> result = new List<vw_ItemInfo>();
            using (GAEntities db = new GAEntities())
            {
                result = db.vw_ItemInfo.Where(o => (o.TypeID == typeID || typeID == "")
                                                    && o.RCompanyID == rcom && o.IsActive).ToList();
            }
            return result;
        }

        public static List<vw_ItemPriceInfo> ListViewItemPriceInfoALL(string rcom)
        {
            List<vw_ItemPriceInfo> result = new List<vw_ItemPriceInfo>();
            using (GAEntities db = new GAEntities())
            {
                result = db.vw_ItemPriceInfo.Where(o => o.RCompanyID == rcom && o.IsActive).ToList();
            }
            return result;
        }

        public static List<I_ShipTo> ListShipTo()
        {
            List<I_ShipTo> result = new List<I_ShipTo>();
            result.Add(new I_ShipTo { ShipToID = "", ShipToName = "ขายเอง" });
            result.Add(new I_ShipTo { ShipToID = "GRAB", ShipToName = "GRAB" });
            result.Add(new I_ShipTo { ShipToID = "PANDA", ShipToName = "PANDA" });
            result.Add(new I_ShipTo { ShipToID = "LINEMAN", ShipToName = "LINEMAN" });
            result.Add(new I_ShipTo { ShipToID = "ROBINHOOD", ShipToName = "ROBINHOOD" });
            result.Add(new I_ShipTo { ShipToID = "SHOPEE", ShipToName = "SHOPEE" });
            result.Add(new I_ShipTo { ShipToID = "ONLINE", ShipToName = "ONLINE" });
            return result;
        }

        #endregion

        #region Save

        public static I_BasicResult SaveItem(ItemInfo doc) {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                using (GAEntities db = new GAEntities()) { 
                    var i = db.ItemInfo.Where(o => o.ItemID == doc.ItemID && o.RCompanyID == doc.RCompanyID).FirstOrDefault();
                    if (i == null) {
                        db.ItemInfo.Add(doc);
                        db.SaveChanges();
                        SaveLog(new TransactionLog { TransactionID = doc.ItemID, TableID = "ITEM", ParentID = doc.CreatedBy, TransactionDate = DateTime.Now, CompanyID = "", Action = "INSERT NEW ITEM" }, doc.RCompanyID, doc.CreatedBy);
                    } else {

                        i.ItemCode = doc.ItemCode;
                        i.GLGroupID = doc.GLGroupID;
                        i.RefID = doc.RefID;
                        i.BrandID = doc.BrandID;
                        i.Model = doc.Model;
                        i.Size = doc.Size;
                        i.Color = doc.Color;
                        i.Name1 = doc.Name1;
                        i.Name2 = doc.Name2;
                        i.TypeID = doc.TypeID;
                        i.CateID = doc.CateID;
                        i.Group1ID = doc.Group1ID;
                        i.Group2ID = doc.Group2ID;
                        i.Group3ID = doc.Group3ID;
                        i.SerialNumber = doc.SerialNumber;
                        i.PackingID = doc.PackingID;
                        i.VendorID = doc.VendorID;
                        i.IsPackaging = doc.IsPackaging;
                        i.PackagingID = doc.PackagingID;
                        i.Cost = doc.Cost;
                        i.Price = doc.Price;
                        i.UnitID = doc.UnitID;
                        i.StkUnitID = doc.StkUnitID;
                        i.Dimension = doc.Dimension;
                        i.VatTypeID = doc.VatTypeID;
                        i.IsKeepStock = doc.IsKeepStock;
                        i.Sort = doc.Sort;
                        i.Remark1 = doc.Remark1;
                        i.Remark2 = doc.Remark2;
                        i.ModifiedDate = DateTime.Now;
                        i.ModifiedBy = doc.CreatedBy;
                        i.IsHold = doc.IsHold;
                        i.IsActive = doc.IsActive;
                        db.SaveChanges();
                    }
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

        public static I_BasicResult checkDupID(ItemInfo doc)
        {
            I_BasicResult r = new I_BasicResult { Result = "fail", Message1 = "Duplicate iteminfo code", Message2 = "" };
            try
            {       
                using (GAEntities db = new GAEntities())
                {
                    var get_id = db.ItemInfo.Where(o => o.ItemID == doc.ItemID && o.RCompanyID == doc.RCompanyID).FirstOrDefault();

                    if (get_id == null)
                    {
                        r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
                    }
                }
            }
            catch (Exception ex)
            {
                r.Result = "fail";
                if (ex.InnerException != null)
                {
                    r.Message1 = ex.InnerException.ToString();
                }
                else
                {
                    r.Message1 = ex.Message;
                }
            }
            return r;
        }

        public static void SaveLog(TransactionLog data, string rcom,string username)
        {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try
            {
                data.RCompanyID = rcom;
                data.TableID = data.TableID == null ? "" : data.TableID;
                data.TransactionID = data.TransactionID == null ? "" : data.TransactionID;
                data.TransactionDate = data.TransactionDate == null ? data.CreatedDate : data.TransactionDate;
                data.CreatedBy = username;
                data.CreatedDate = DateTime.Now;
                data.ChangeValue = data.ChangeValue == null ? "" : data.ChangeValue;
                data.CompanyID = data.CompanyID == null ? "" : data.CompanyID;
                data.ParentID = data.ParentID == null ? "" : data.ParentID;
                data.Action = data.ActionType == null ? "" : data.ActionType;
                data.Action = data.Action == null ? "" : data.Action;
                data.ChangeValue = data.ChangeValue == null ? "" : data.ChangeValue;
                data.IsActive = true;
                using (GAEntities db = new GAEntities())
                {
                    db.TransactionLog.Add(data);
                    var r = db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                result.Result = "fail";
                if (ex.InnerException != null)
                {
                    result.Message1 = ex.InnerException.ToString();
                }
                else
                {
                    result.Message1 = ex.Message;
                }
            }
        }

        #endregion

        #region New transaction

        public I_ItemSet NewTransaction(string username, string rcom) {
            I_ItemSet n = new I_ItemSet();

            n.Info = NewItem(username, rcom);
            n.LineItem = new List<ItemInfo>();
            n.VItemPrice = new List<vw_ItemPriceInfo>();
            n.ItemPointRate = new List<ItemInPointRate>();
            n.Files = new List<XFilesRef>();
            n.Log = new List<TransactionLog>();

            return n;
        }
        public static ItemInfo NewItem(string username,string rcom)
        {
            ItemInfo n = new ItemInfo();
            n.ItemID = "";
            n.MasterItemID = "";
            n.CompanyID = "";
            n.RCompanyID = rcom;
            n.ItemCode = "";
            n.Barcode = "";
            n.Model = "";
            n.Color = "";
            n.Size = "";
            n.GLGroupID = "";
            n.RefID = "";
            n.Name1 = "";
            n.Name2 = "";
            n.PriceTaxCondType = "EXC VAT";
            n.TypeID = "";
            n.CateID = "";
            n.Group1ID = "";
            n.Group2ID = "";
            n.Group3ID = "";
            n.BrandID = "";
            n.SerialNumber = "";
            n.PackingID = "";
            n.Cost = 0;
            n.Price = 0;
            n.UnitID = "";
            n.StkUnitID = "";
            n.VolPerUOM = 0;
            n.Weight = 0;
            n.Dimension = 0;
            n.VendorID = "";
            n.VatTypeID = "";
            n.IsKeepStock = false;
            n.IsLotItem = false;
            n.IsPackaging = false;
            n.PackagingID = "";
            n.IsOntop = false;
            n.Remark1 = "";
            n.Remark2 = "";
            n.CreatedBy = username;
            n.CreatedDate = DateTime.Now;
            n.ModifiedBy = "";
            n.ModifiedDate = null;
            n.Status = "";
            n.Sort = 0;
            n.IsHold = false;
            n.IsActive = true;
            return n;
        }

        public static ItemPriceInfo NewPrice(string username, string rcom)
        {
            ItemPriceInfo newdata = new ItemPriceInfo();
            newdata.ItemID = "";
            newdata.CompanyID = "";
            newdata.RCompanyID = rcom;
            newdata.Price = 0;
            newdata.UseLevel = 0;
            newdata.RefID = "";
            newdata.DataSource = "";
            newdata.CompanyID = "";
            newdata.Remark = "";
            newdata.RefID = "";
            newdata.DateBegin = DateTime.Now.Date;
            newdata.DateEnd = DateTime.Now.Date;
            newdata.IsActive = true;
            return newdata;
        }

        public static ItemInPointRate NewItemPoint(string username, string rcom)
        {
            ItemInPointRate newdata = new ItemInPointRate();
            newdata.RateID = Guid.NewGuid().ToString();
            newdata.ItemID = "";
            newdata.ComID = "";
            newdata.RComID = rcom;
            newdata.AmtPerPointRate = 0;
            newdata.DateBegin = DateTime.Now.Date;
            newdata.DateEnd = DateTime.Now.Date;
            newdata.ExpireInMont = 0;
            newdata.IsActive = true;
            return newdata;
        }


        //public List<SelectOption> ListGender() {
        //    return new List<SelectOption>() {
        //        new SelectOption(){ Value= "Male", Desc="ชาย"},
        //        new SelectOption(){ Value= "Female", Desc="หญิง"}
        //    };
        //}

        #endregion


    }
}
