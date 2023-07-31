using Robot.Data.ML;
using Robot.Data.GADB.TT;
using Robot.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static Robot.Data.ML.I_Result;

namespace Robot.Data.DA {
    public class ItemService {
        public static string sessionActiveId = "activeitemid";
        public class I_ItemSet {
            public ItemInfo Info { get; set; }
            public List<ItemInfo> LineItem { get; set; }
            public List<ItemPriceInfo> ItemPrice { get; set; }
            public List<vw_ItemPriceInfo> VItemPrice { get; set; }
            public List<ItemInPointRate> ItemPointRate { get; set; }
            public List<vw_ItemRelate> ItemRelate { get; set; }
            public List<vw_ItemUnitConvert> ItemUnitConvert { get; set; }
            public List<TransactionLog> Log { get; set; }
            public List<XFilesRef> Files { get; set; }
            public I_BasicResult OutputAction { get; set; }
        }
        //private XFilesService _xfile;
        //public ItemService(XFilesService xfile) {
        //    _xfile = xfile;
        //}
        public I_ItemSet DocSet { get; set; }

        public ItemService() {
            //DocSet = NewTransaction("", "");
        }

        //public class SelectOption
        //{
        //    public string Value { get; set; }
        //    public string Desc { get; set; }
        //}

        public class I_ShipTo {
            public string ShipToID { get; set; }
            public string ShipToName { get; set; }
        }

        #region Get&ListData

        public I_ItemSet GetDocSet(string itemId, string rcom) {
            I_ItemSet n = NewTransaction("", rcom);
            try {
                using (GAEntities db = new GAEntities()) {
                    n.Info = db.ItemInfo.Where(o => o.ItemID == itemId && o.RCompanyID == rcom).FirstOrDefault();
                    n.ItemPrice = db.ItemPriceInfo.Where(o => o.ItemID == itemId && o.RCompanyID == rcom && o.IsActive).ToList();
                    n.VItemPrice = db.vw_ItemPriceInfo.Where(o => o.ItemID == itemId && o.RCompanyID == rcom && o.IsActive).ToList();
                    n.ItemPointRate = db.ItemInPointRate.Where(o => o.ItemID == itemId && o.RComID == rcom && o.IsActive).OrderByDescending(o => o.DateEnd).ToList();
                    n.ItemRelate = db.vw_ItemRelate.Where(o => o.MasterItemID == itemId && o.RCompanyID == rcom && o.IsActive).ToList();
                    n.ItemUnitConvert = db.vw_ItemUnitConvert.Where(o => o.ItemID == itemId && o.RCompanyID == rcom && !o.IsBaseUnit).ToList();
                    //n.Files = GetFileInDoc(docid);
                    n.Log = db.TransactionLog.Where(o => o.TransactionID == itemId && o.TableID == "ITEM").OrderBy(o => o.CreatedDate).ToList();
                }
            } catch (Exception ex) {
                var r = ex.Message;
            }
      
            return n;
        }

        public static ItemInfo GetItemInfo(string rcom, string itemid) {
            ItemInfo ven = new ItemInfo();
            using (GAEntities db = new GAEntities()) {
                ven = db.ItemInfo.Where(o => o.RCompanyID == rcom && o.ItemID == itemid).FirstOrDefault();
            }
            return ven;
        }

        public static List<SelectOption> ListItemForSelect(string rcom, List<string> types) {
            List<SelectOption> result = new List<SelectOption>();
            using (GAEntities db = new GAEntities()) {
                var items = db.ItemInfo.Where(o => o.RCompanyID == rcom
                                                            && types.Contains(o.TypeID)
                                                            && o.IsActive == true
                                                            ).OrderBy(o => o.TypeID).ThenBy(o => o.Name1).ToList();
                int i = 0;
                foreach (var v in items) {
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

        public static List<vw_ItemInfo> ListDoc(string Search, string rcom, string itemType) {
            List<vw_ItemInfo> result = new List<vw_ItemInfo>();
            using (GAEntities db = new GAEntities()) {
                var defult = new List<string> { "DISCPER01", "DISCAMT01", "DEFAULTMENU" };
                result = db.vw_ItemInfo.Where(o =>
                                                                (o.ItemID.Contains(Search)
                                                                        || o.Name1.Contains(Search)
                                                                        || o.Group1ID.Contains(Search)
                                                                        || o.Group1Name.Contains(Search)
                                                                        || o.TypeName.Contains(Search)
                                                                        || Search == "")
                                                                && !defult.Contains(o.ItemID)
                                                                && o.IsSysData == false
                                                                && o.RCompanyID == rcom
                                                                && (o.TypeID == itemType || itemType == "")

                                                        ).OrderBy(o => o.TypeID).ThenBy(o => o.ItemID).ToList();

            }
            return result;
        }

        public static ItemInfo GetItem(string itemId, string rcom) {
            ItemInfo result = new ItemInfo();
            using (GAEntities db = new GAEntities()) {
                result = db.ItemInfo.Where(o => o.ItemID == itemId && o.RCompanyID == rcom ).FirstOrDefault();
            }
            return result;
        }

        public static List<vw_ItemInfo> ListViewItemByType(string typeID, string rcom) {
            List<vw_ItemInfo> result = new List<vw_ItemInfo>();
            using (GAEntities db = new GAEntities()) {
                result = db.vw_ItemInfo.Where(o => (o.TypeID == typeID || typeID == "")
                                                    && o.RCompanyID == rcom && o.IsActive).ToList();
            }
            return result;
        }

        public static List<ItemRelate> ListItemRelateByMasterItemID(string itemid, string rcom) {
            List<ItemRelate> result = new List<ItemRelate>();
            using (GAEntities db = new GAEntities()) {
                result = db.ItemRelate.Where(o => o.MasterItemID == itemid
                                                    && o.RCompanyID == rcom && o.IsActive).ToList();
            }
            return result;
        }
        public static List<vw_ItemPriceInfo> ListViewItemPriceInfoALL(string rcom) {
            List<vw_ItemPriceInfo> result = new List<vw_ItemPriceInfo>();
            using (GAEntities db = new GAEntities()) {
                result = db.vw_ItemPriceInfo.Where(o => o.RCompanyID == rcom && o.IsActive).ToList();
            }
            return result;
        }
        #endregion

        #region Save

        public static I_BasicResult Save(ItemInfo doc) {
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
             var ru= AddDefaultUnitConvert(doc);
                if (ru.Result=="fail") {
                    r.Result = ru.Result;
                    r.Message1 = ru.Message1;
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

        public static I_BasicResult checkDupID(ItemInfo doc) {
            I_BasicResult r = new I_BasicResult { Result = "fail", Message1 = "Duplicate iteminfo code", Message2 = "" };
            try {
                using (GAEntities db = new GAEntities()) {
                    var get_id = db.ItemInfo.Where(o => o.ItemID == doc.ItemID && o.RCompanyID == doc.RCompanyID).FirstOrDefault();

                    if (get_id == null) {
                        r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
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

        public static void SaveLog(TransactionLog data, string rcom, string username) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
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
                using (GAEntities db = new GAEntities()) {
                    db.TransactionLog.Add(data);
                    var r = db.SaveChanges();
                }
            } catch (Exception ex) {
                result.Result = "fail";
                if (ex.InnerException != null) {
                    result.Message1 = ex.InnerException.ToString();
                } else {
                    result.Message1 = ex.Message;
                }
            }
        }

        #endregion
        #region ItemPrice
        public static I_BasicResult AddPrice(ItemPriceInfo i, string itemid, string rcom) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                using (GAEntities db = new GAEntities()) {
                    var price = db.ItemPriceInfo.Where(o => o.ItemID == itemid
                                                                && o.RCompanyID == rcom
                                                                && o.CompanyID == i.CompanyID
                                                                && o.CustID == i.CustID
                                                                && o.UseLevel == i.UseLevel
                                                                && o.DateBegin == i.DateBegin
                                                                && o.DateEnd == i.DateEnd
                                                                ).FirstOrDefault();
                    if (price == null) {//add new item
                        db.ItemPriceInfo.Add(i);
                        db.SaveChanges();
                    } else {
                        //exist item
                        price.PriceTaxCondType = i.PriceTaxCondType;
                        price.Price = i.Price;
                        price.UseLevel = i.UseLevel;
                        price.DateBegin = i.DateBegin.Date;
                        price.DateEnd = i.DateEnd.Date;
                        price.IsActive = true;
                        db.SaveChanges();
                    }
                }

            } catch (Exception ex) {
                result.Result = "fail";
                if (ex.InnerException != null) {
                    result.Message1 = ex.InnerException.ToString();
                } else {
                    result.Message1 = ex.Message;
                }
            }
            return result;
        }

        public static I_BasicResult DeleteItemPrice(int id) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                using (GAEntities db = new GAEntities()) {
                    db.ItemPriceInfo.Remove(db.ItemPriceInfo.Where(o => o.ID == id).FirstOrDefault());
                    db.SaveChanges();
                }

            } catch (Exception ex) {
                result.Result = "fail";
                if (ex.InnerException != null) {
                    result.Message1 = ex.InnerException.ToString();
                } else {
                    result.Message1 = ex.Message;
                }
            }
            return result;
        }

        #endregion
        #region ItemInPointRate
        public static I_BasicResult AddItemInPointRate(ItemInPointRate data, string rcom) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };

            try {
                using (GAEntities db = new GAEntities()) {
                    var pointrate = db.ItemInPointRate.Where(o => o.RateID == data.RateID && o.RComID == rcom).FirstOrDefault();

                    if (pointrate == null) {//add new 
                        db.ItemInPointRate.Add(data);
                        db.SaveChanges();
                    } else {

                    }
                }

            } catch (Exception ex) {
                result.Result = "fail";
                if (ex.InnerException != null) {
                    result.Message1 = ex.InnerException.ToString();
                } else {
                    result.Message1 = ex.Message;
                }
            }
            return result;
        }


        public static List<ItemInfo> ListItemInfo(string typeid,string rcom) {
            List<ItemInfo> result = new List<ItemInfo>();
            using (GAEntities db = new GAEntities()) {
                result = db.ItemInfo.Where(o => o.RCompanyID == rcom
                                            && (o.TypeID == typeid || typeid == "")
                                            && o.IsActive == true
                                            ).ToList();
            }
            return result;
        }

        public static I_BasicResult DeleteItemInPointRate(string docid) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                using (GAEntities db = new GAEntities()) {
                    db.ItemInPointRate.Remove(db.ItemInPointRate.Where(o => o.RateID == docid).FirstOrDefault());
                    db.SaveChanges();
                }

            } catch (Exception ex) {
                result.Result = "fail";
                if (ex.InnerException != null) {
                    result.Message1 = ex.InnerException.ToString();
                } else {
                    result.Message1 = ex.Message;
                }
            }
            return result;
        }

        #endregion
        #region ItemRelate
        public static I_BasicResult AddItemItemRelate(List<ItemRelate> data, string itemid, string rcom) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                using (GAEntities db = new GAEntities()) {
                    db.ItemRelate.RemoveRange(db.ItemRelate.Where(o => o.MasterItemID == itemid && o.RCompanyID == rcom));
                    db.ItemRelate.AddRange(data);
                    db.SaveChanges();
                }
            } catch (Exception ex) {
                result.Result = "fail";
                if (ex.InnerException != null) {
                    result.Message1 = ex.InnerException.ToString();
                } else {
                    result.Message1 = ex.Message;
                }
            }
            return result;
        }
        #endregion
        #region ItemOnHold
        public static I_BasicResult UpdateItemOnHold(List<ItemOnHold> data) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                bool isClearAll = false;

                if (data.Count == 0) {
                    return result;
                }
                if (data.Count == 1) {
                    if (data.FirstOrDefault().ItemID == "") {
                        isClearAll = true;
                    }
                }
                string rcom = data.FirstOrDefault().RCompanyID;
                string com = data.FirstOrDefault().CompanyID;

                using (GAEntities db = new GAEntities()) {
                    db.ItemOnHold.RemoveRange(db.ItemOnHold.Where(o => o.RCompanyID == rcom && o.CompanyID == com));
                    if (!isClearAll) {
                        db.ItemOnHold.AddRange(data);
                    }

                    db.SaveChanges();
                }
            } catch (Exception ex) {
                result.Result = "fail";
                if (ex.InnerException != null) {
                    result.Message1 = ex.InnerException.ToString();
                } else {
                    result.Message1 = ex.Message;
                }
            }
            return result;
        }
        public static List<ItemOnHold> GetItemOnHold(string rcom, string com) {
            List<ItemOnHold> output = new List<ItemOnHold>();
            try {

                using (GAEntities db = new GAEntities()) {
                    output = db.ItemOnHold.Where(o => o.RCompanyID == rcom && o.CompanyID == com).ToList();
                }
            } catch (Exception ex) {

            }
            return output;
        }
        #endregion
        #region  add item unit
        public static I_BasicResult AddDefaultUnit(  ItemInfo item) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                if (item.UnitID=="") {
                    return result;
                }
               
                using (GAEntities db = new GAEntities()) {
                    var n = NewItemUnit(item.RCompanyID);
                    n.IsDefault = true;
                    n.ItemID = item.ItemID;
                    n.Unit = item.UnitID; 
                    db.ItemUnit.RemoveRange(db.ItemUnit.Where(o => o.ItemID == item.ItemID && o.RCompanyID == item.RCompanyID && o.IsDefault));
                    db.ItemUnit.Add(n);
                    db.SaveChanges();
                }
            } catch (Exception ex) {
                result.Result = "fail";
                if (ex.InnerException != null) {
                    result.Message1 = ex.InnerException.ToString();
                } else {
                    result.Message1 = ex.Message;
                }
            }
            return result;
        }
  public static I_BasicResult AddDefaultUnitConvert(  ItemInfo item) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                if (item.UnitID=="") {
                    return result;
                } 
                using (GAEntities db = new GAEntities()) {
                    var n = NewItemUnitConvert(item.RCompanyID); 
                    n.ItemID = item.ItemID;
                    n.ToUnit = item.UnitID;
                    n.IsBaseUnit = true; 
                    db.ItemUnitConvert.RemoveRange(db.ItemUnitConvert.Where(o => o.ItemID == item.ItemID && o.RCompanyID == item.RCompanyID && o.IsBaseUnit));
                    db.ItemUnitConvert.Add(n);
                    db.SaveChanges();
                }
            } catch (Exception ex) {
                result.Result = "fail";
                if (ex.InnerException != null) {
                    result.Message1 = ex.InnerException.ToString();
                } else {
                    result.Message1 = ex.Message;
                }
            }
            return result;
        }

        #endregion
        #region ItemUnitConvert
        public static I_BasicResult AddItemUnitConvert(ItemUnitConvert data, string rcom,string unitdefault) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };

            try {
                using (GAEntities db = new GAEntities()) {

                    if (data.ToUnit == unitdefault) {
                        result.Result = "fail";
                        result.Message1 = "แปลงหน่วย ซํ้ากับ หน่วยพื้นฐานในระบบ";
                        return result;
                    }

                    var ItemUnitConvert = db.ItemUnitConvert.Where(o => o.ItemID == data.ItemID && o.ToUnit == data.ToUnit && o.RCompanyID == rcom).FirstOrDefault();

                    if (ItemUnitConvert == null) {
                        db.ItemUnitConvert.Add(data);
                        db.SaveChanges();
                    } else {
                        result.Result = "fail";
                        result.Message1 = "แปลงหน่วย " + data.ToUnit + " มีในระบบแล้ว";
                        return result;
                    }
                }
            } catch (Exception ex) {
                result.Result = "fail";
                if (ex.InnerException != null) {
                    result.Message1 = ex.InnerException.ToString();
                } else {
                    result.Message1 = ex.Message;
                }
            }
            return result;
        }

        public static I_BasicResult DeleteItemUnitConvert(int ID) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                using (GAEntities db = new GAEntities()) {
                    db.ItemUnitConvert.Remove(db.ItemUnitConvert.Where(o => o.ID == ID).FirstOrDefault());
                    db.SaveChanges();
                }

            } catch (Exception ex) {
                result.Result = "fail";
                if (ex.InnerException != null) {
                    result.Message1 = ex.InnerException.ToString();
                } else {
                    result.Message1 = ex.Message;
                }
            }
            return result;
        }

        #endregion

        #region New transaction

        public static I_ItemSet NewTransaction(string username, string rcom) {
            I_ItemSet n = new I_ItemSet();

            n.Info = NewItem(username, rcom);
            n.LineItem = new List<ItemInfo>();
            n.VItemPrice = new List<vw_ItemPriceInfo>();
            n.ItemPointRate = new List<ItemInPointRate>();
            n.ItemUnitConvert = new List<vw_ItemUnitConvert>();
            n.Files = new List<XFilesRef>();
            n.Log = new List<TransactionLog>();

            return n;
        }
        public static ItemInfo NewItem(string username, string rcom) {
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

        public static ItemPriceInfo NewPrice(string username, string rcom) {
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

        public static ItemInPointRate NewItemPoint(string username, string rcom) {
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
        public static ItemUnit NewItemUnit(string rcom) {
            ItemUnit n = new ItemUnit();
  
            n.RCompanyID = rcom;
            n.CompanyID = "";
            n.ItemID = "";
            n.Unit = "";
  n.IsDefault = false;
            return n;
        }
        public static ItemUnitConvert NewItemUnitConvert(string rcom) {
            ItemUnitConvert n = new ItemUnitConvert();

            n.RCompanyID = rcom;
            n.CompanyID = "";
            n.ItemID = "";
            n.ToUnit = "";
            //n.UnitToDesc = "";
            n.QtyInThisUnit = 1;
            n.QtyInBaseUnit = 1;
            n.IsBaseUnit = false;

            return n;
        }
        public static List<SelectOption> ListUseLevel() {
            return new List<SelectOption>() {
                new SelectOption(){ IsSelect = true ,Value= "0", Description="0" ,Sort = 1},
                new SelectOption(){ IsSelect = true ,Value = "1", Description="1", Sort = 2}
            };
        }
        public static List<ItemRelate> Convert2ItemRelate(List<vw_ItemRelate> data) {
            List<ItemRelate> output = new List<ItemRelate>();
            foreach (var d in data) {
                ItemRelate o = new ItemRelate();
                o.ID = d.ID;
                o.RCompanyID = d.RCompanyID;
                o.CompanyID = d.CompanyID;
                o.MasterItemID = d.MasterItemID;
                o.RelateItemID = d.RelateItemID;
                o.RelateType = d.RelateType;
                o.Remark = d.Remark;
                o.IsActive = d.IsActive;
            }
            return output;
        }
        #endregion


        //#region File
        //public I_BasicResult ConvertByte2File() {
        //    I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
        //    try {
        //        using (GAEntities db=new GAEntities()) {
        //            var 
        //        }
        //    } catch (Exception) {

        //    }

        //    return r;
        //}
        //#endregion



    }
}
