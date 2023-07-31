
using Dapper;
using Robot.Data;
using Robot.Data.DataAccess;
using Robot.Data.FileStore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using static Robot.Data.BL.I_Result;

namespace Robot.Master.DA
{
    public static class POSItemService
    {

        public class I_ItemSet
        {
            public string Action { get; set; }
            public ItemInfo Info { get; set; }
            public List<ItemInfo> LineItem { get; set; }
            public List<ItemPriceInfo> ItemPrice { get; set; }
            public List<vw_ItemPriceInfo> VItemPrice { get; set; }
            public List<ItemInPointRate> ItemPointRate { get; set; }
            public List<TransactionLog> Log { get; set; }
            public bool NeedRunNextID { get; set; }
            public I_BasicResult OutputAction { get; set; }
        }
        public class I_ItemFiterSet
        {
            public DateTime DateFrom { get; set; }
            public DateTime DateTo { get; set; }
            public String DocType { get; set; }
            public String SearchBy { get; set; }
            public String SearchText { get; set; }
            public bool ShowActive { get; set; }
        }

       

        public class POSMenuItem {
            public string ItemID { get; set; }
            public string Name { get; set; }
            public string Company { get; set; }
            public string CustId { get; set; }
            public string TypeName { get; set; }
            public string GroupName { get; set; }
            public string CateID { get; set; }
            public string GroupID { get; set; }
            public string TypeID { get; set; }
            public string AccGroup { get; set; }
            public decimal SellQty { get; set; }
            public decimal SellAmt { get; set; }
            public string ImageUrl { get; set; }
            public decimal Price { get; set; }
            public string PriceTaxCondType { get; set; }
            public decimal Weight { get; set; }
            public string RefID { get; set; }
            public string WUnit { get; set; }
            public string Unit { get; set; }
            public bool IsStockItem { get; set; }
            public bool IsActive { get; set; }

        }
        public class I_ShipTo
        {
            public string ShipToID { get; set; }
            public string ShipToName { get; set; }
        }

        public static string PreviousPage { get { return (string)HttpContext.Current.Session["item_previouspage"]; } set { HttpContext.Current.Session["item_previouspage"] = value; } }

        public static string DetailPreviousPage { get { return (string)HttpContext.Current.Session["itemdetail_previouspage"]; } set { HttpContext.Current.Session["itemdetail_previouspage"] = value; } }
        public static string NewDocPreviousPage { get { return (string)HttpContext.Current.Session["itemnewdoc_previouspage"]; } set { HttpContext.Current.Session["itemnewdoc_previouspage"] = value; } }
        public static bool IsNewDoc { get { return HttpContext.Current.Session["isnewdoc"] == null ? false : (bool)HttpContext.Current.Session["isnewdoc"]; } set { HttpContext.Current.Session["isnewdoc"] = value; } }

        public static I_ItemSet DocSet { get { return (I_ItemSet)HttpContext.Current.Session["itemdocset"]; } set { HttpContext.Current.Session["itemdocset"] = value; } }
        public static List<vw_ItemInfo> DocList { get { return (List<vw_ItemInfo>)HttpContext.Current.Session["itemdoc_list"]; } set { HttpContext.Current.Session["itemdoc_list"] = value; } }
        public static I_ItemFiterSet FilterSet { get { return (I_ItemFiterSet)HttpContext.Current.Session["itemfilter_set"]; } set { HttpContext.Current.Session["itemfilter_set"] = value; } }

        #region Query Transaction
        public static void GetDocSetByID(string docid)
        {
            NewTransaction("");
            DocSet.OutputAction = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try
            {
                var rcom = LoginService.LoginInfo.CurrentRootCompany.CompanyID;
                using (GAEntities db = new GAEntities())
                {
                    DocSet.Info = db.ItemInfo.Where(o => o.ItemID == docid && o.RCompanyID == rcom).FirstOrDefault();
                    DocSet.ItemPrice = db.ItemPriceInfo.Where(o => o.ItemID == docid && o.RCompanyID == rcom && o.IsActive).ToList();
                    DocSet.VItemPrice = db.vw_ItemPriceInfo.Where(o => o.ItemID == docid && o.RCompanyID == rcom && o.IsActive).ToList();
                    DocSet.ItemPointRate = db.ItemInPointRate.Where(o => o.ItemID == docid && o.RComID == rcom && o.IsActive).OrderByDescending(o => o.DateEnd).ToList();

                    DocSet.Log = TransactionInfoService.ListLogByDocID(docid);
                }
            }
            catch (Exception ex)
            {
                DocSet.OutputAction = new I_BasicResult { Result = "fail", Message1 = ex.Message, Message2 = "" };
            }

        }
        public static List<POSMenuItem> ListMenuItem(string comId, string custId)
        {
            List<POSMenuItem> result = new List<POSMenuItem>();
            var rcom = LoginService.LoginInfo.CurrentRootCompany.CompanyID;
            List<string> useType = new List<string> { "FG", "DISCOUNT" };
            var today = DateTime.Now.Date;
            using (GAEntities db = new GAEntities())
            {
                var item_price = db.ItemPriceInfo.Where(o =>
                                                            (o.CompanyID == comId || o.CompanyID == "")
                                                            && o.CustID == custId
                                                            && o.RCompanyID == rcom
                                                            && o.DateBegin<=today && o.DateEnd>=today
                                                            && o.IsActive
                                                            ).ToList();

                var query = db.vw_ItemInfo.Where(o =>
                                          new List<string> { "FG", "DISCOUNT" }.Contains(o.TypeID)
                                            & o.RCompanyID == rcom
                                            && o.IsActive).ToList();
                foreach (var q in query)
                {
                    var price1 = item_price.Where(o => o.ItemID == q.ItemID).ToList();
                    if (price1.Count==0 && q.TypeID != "DISCOUNT") {// ไม่มี config ราคาและ ไม่ใช่ส่วนลด ไม่ต้องเพิ่มในรายการสินค้า
                        continue;
                    }

                    var price = new ItemPriceInfo();
                    //Step1 get promotion price with this store
                    var chkProPirceThisStore = price1.Where(o => o.UseLevel == 1 && o.CompanyID != "").FirstOrDefault();
                    //Step2 get promotion price with all store
                    var chkProPirceAllStore = price1.Where(o => o.UseLevel == 1 && o.CompanyID == "").FirstOrDefault();
                    //Step3 get regular price with  this store
                    var chkRegPirceThisStore = price1.Where(o => o.UseLevel == 0 && o.CompanyID != "").FirstOrDefault();
                    //Step4 get regular price with  all store
                    var chkRegPirceAllStore = price1.Where(o => o.UseLevel == 0 && o.CompanyID == "").FirstOrDefault();
                 
                    if (chkProPirceThisStore!=null) {//pro price with this store ใช้อันนี้ก่อน
                        price = chkProPirceThisStore;
                    } else if (chkProPirceAllStore != null) {//ถ้าไม่มี pro price with this store ใช้ pro price with all store
                        price = chkProPirceAllStore;
                    } else if (chkRegPirceThisStore != null) {// ถ้าไม่มี pro price with all store ใช้  regular price with  this store
                        price = chkRegPirceThisStore;
                    } else {//ท้ายที่สุดไม่มีอะไรให้ใช้ราคา get regular price with  all store
                        price = chkRegPirceAllStore;
                    }
                  


                    POSMenuItem n = new POSMenuItem();
                    n.ItemID = q.ItemID;
                    n.Name = q.Name1;
                    n.GroupName = q.Group1Name;
                    n.CateID = q.CateID;
                    n.Company = q.CompanyID;
                    n.CustId = q.CateID;
                    n.TypeID = q.TypeID;
                    n.GroupID = q.Group1ID;
                    n.TypeName = q.TypeName;
                    n.IsStockItem = q.IsKeepStock;
                    n.AccGroup = q.GLGroupID;
                    n.SellQty = 0;
                    n.SellAmt = 0;
                    n.WUnit = q.UnitID;
                    n.Unit = q.UnitID;
                    n.Price = price == null ? 0 : price.Price;
                    n.PriceTaxCondType = price == null ? "INC VAT" : price.PriceTaxCondType;
                    n.ImageUrl = "";
                    n.RefID = "";
                    n.IsActive = q.IsActive;
                    result.Add(n);
                }


            }
            return result;
        }
        public static void ListDoc()
        {

            var rcom = LoginService.LoginInfo.CurrentRootCompany.CompanyID;
            var f = FilterSet;
            using (GAEntities db = new GAEntities())
            {

                DocList = db.vw_ItemInfo.Where(o =>
                                                                (o.ItemID.Contains(f.SearchText)
                                                                        || o.Name1.Contains(f.SearchText)
                                                                        || o.Group1ID.Contains(f.SearchText)
                                                                        || o.Group1Name.Contains(f.SearchText)
                                                                        || o.TypeID.Contains(f.SearchText)
                                                                        || o.TypeName.Contains(f.SearchText)
                                                                        || f.SearchText == "")
                                                                && o.ItemID != "DEFAULTMENU"
                                                                && o.IsSysData == false
                                                                && o.RCompanyID == rcom
                                                                && (o.TypeID == f.DocType || f.DocType == "")
                                                                && (o.IsActive == f.ShowActive
                                                        )
                                                        ).OrderBy(o => o.TypeID).ThenBy(o => o.ItemID).ToList();

            }
        }

        public static List<vw_ItemPriceInfo> ListItemPrice(ItemPriceList.I_ItemPriceSet filter)
        {
            filter.Search = filter.Search.Trim().ToLower();
            int UseLevel = 0;
            if (filter.UseLevel != "")
            {
                UseLevel = Convert.ToInt32(filter.UseLevel);
            }

            
            List<vw_ItemPriceInfo> result = new List<vw_ItemPriceInfo>();
            using (GAEntities db = new GAEntities())
            {
                result = db.vw_ItemPriceInfo.Where(o =>
                                        (
                                            o.ItemID.ToLower().Contains(filter.Search)
                                            || o.CompanyID.ToLower().Contains(filter.Search)
                                            || o.ItemName.ToLower().Contains(filter.Search)
                                            || o.CompanyName.ToLower().Contains(filter.Search)
                                            || o.CustID.ToLower().Contains(filter.Search)
                                            || filter.Search == ""
                                        )
                                        && (o.ItemID == filter.ItemID || filter.ItemID == "")
                                        && (o.CompanyID == filter.Company || filter.Company == "")
                                        && (o.PriceTaxCondType == filter.PriceTaxcon || filter.PriceTaxcon == "")
                                        && (o.UseLevel == UseLevel || filter.UseLevel == "")
                                        && (o.CustID == filter.ShipTo || filter.ShipTo == "")
                                        && o.IsActive == true
                                        ).OrderBy(o => o.ItemID).ThenByDescending(o => o.CompanyID).ToList();
            }
            return result;

        }

        #endregion

        #region  Method GET


        public static ItemInfo GetItem(string itemId)
        {
            ItemInfo result = new ItemInfo();
            var rcom = LoginService.LoginInfo.CurrentRootCompany.CompanyID;
            using (GAEntities db = new GAEntities())
            {
                result = db.ItemInfo.Where(o => o.ItemID == itemId && o.RCompanyID == rcom).FirstOrDefault();
            }
            return result;
        }

        public static List<vw_ItemUnitConvert> ListViewItemUnitConvert(string itemid) {
            List<vw_ItemUnitConvert> result = new List<vw_ItemUnitConvert>();
            var rcom = LoginService.LoginInfo.CurrentRootCompany.CompanyID;
            using (GAEntities db = new GAEntities()) {
                result = db.vw_ItemUnitConvert.Where(o => o.ItemID == itemid && o.RCompanyID == rcom).ToList();
            }
            return result;
        }

        public static List<vw_ItemInfo> ListViewItemByType(string typeID)
        {
            List<vw_ItemInfo> result = new List<vw_ItemInfo>();
            var rcom = LoginService.LoginInfo.CurrentRootCompany.CompanyID;
            using (GAEntities db = new GAEntities())
            {
                result = db.vw_ItemInfo.Where(o => (o.TypeID == typeID || typeID == "")
                                                    && o.RCompanyID == rcom && o.IsActive).ToList();
            }
            return result;
        }

        public static List<vw_ItemPriceInfo> ListViewItemPriceInfoALL()
        {
            List<vw_ItemPriceInfo> result = new List<vw_ItemPriceInfo>();
            var rcom = LoginService.LoginInfo.CurrentRootCompany.CompanyID;
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
            //result.Add(new I_ShipTo { ShipToID = "GOJEK", ShipToName = "GOJEK" });
            result.Add(new I_ShipTo { ShipToID = "ONLINE", ShipToName = "ONLINE" });
            return result;
        }
        #endregion

        #region Save

        public static void Save(string action)
        {
            action = action.ToLower();
            try
            {
                using (GAEntities db = new GAEntities())
                {
                    if (action == "insert")
                    {
                        checkDupID();
                        if (DocSet.OutputAction.Result == "fail")
                        {
                            return;
                        }

                        db.ItemInfo.Add(DocSet.Info);
                        db.SaveChanges();

                        TransactionInfoService.SaveLog(new TransactionLog { TransactionID = DocSet.Info.ItemID, TableID = "ITEM", ParentID = "", TransactionDate = DateTime.Now, CompanyID = DocSet.Info.CompanyID, Action = "INSERT DATA ITEM" });
                        DocSet.OutputAction = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
                    }
                    if (action == "update")
                    {
                        var i = db.ItemInfo.Where(o => o.ItemID == DocSet.Info.ItemID).FirstOrDefault();

                        i.ItemCode = DocSet.Info.ItemCode;
                        i.GLGroupID = DocSet.Info.GLGroupID;
                        i.RefID = DocSet.Info.RefID;
                        i.BrandID = DocSet.Info.BrandID;
                        i.Barcode = DocSet.Info.Barcode;
                        i.Model = DocSet.Info.Model;
                        i.Size = DocSet.Info.Size;
                        i.Color = DocSet.Info.Color;
                        i.Name1 = DocSet.Info.Name1;
                        i.Name2 = DocSet.Info.Name2;
                        i.TypeID = DocSet.Info.TypeID;
                        i.CateID = DocSet.Info.CateID;
                        i.Group1ID = DocSet.Info.Group1ID;
                        i.Group2ID = DocSet.Info.Group2ID;
                        i.Group3ID = DocSet.Info.Group3ID;
                        i.SerialNumber = DocSet.Info.SerialNumber;
                        i.PackingID = DocSet.Info.PackingID;
                        i.IsPackaging = DocSet.Info.IsPackaging;
                        i.PackagingID = DocSet.Info.PackagingID;
                        i.IsHold = DocSet.Info.IsHold;
                        i.Price = DocSet.Info.Price;
                        i.UnitID = DocSet.Info.UnitID;
                        i.StkUnitID = DocSet.Info.StkUnitID;
                        i.Dimension = DocSet.Info.Dimension;
                        i.VatTypeID = DocSet.Info.VatTypeID;
                        i.IsKeepStock = DocSet.Info.IsKeepStock;
                        i.Remark1 = DocSet.Info.Remark1;
                        i.Remark2 = DocSet.Info.Remark2;
                        i.ModifiedDate = DateTime.Now;
                        i.ModifiedBy = LoginService.LoginInfo.CurrentUser;
                        i.IsHold = DocSet.Info.IsHold;
                        i.IsActive = DocSet.Info.IsActive;

                        db.SaveChanges();
                        TransactionInfoService.SaveLog(new TransactionLog { TransactionID = DocSet.Info.ItemID, TableID = "ITEM", ParentID = "", TransactionDate = DateTime.Now, CompanyID = DocSet.Info.CompanyID, Action = "UPDATE DATA ITEM" });
                        DocSet.OutputAction = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
                    }

                }
            }
            catch (Exception ex)
            {
                DocSet.OutputAction.Result = "fail";
                if (ex.InnerException != null)
                {
                    DocSet.OutputAction.Message1 = ex.InnerException.ToString();
                }
                else
                {
                    DocSet.OutputAction.Message1 = ex.Message;
                }
            }
        }

        public static void SaveV2()
        {
            var h = DocSet.Info;
            var rcom = LoginService.LoginInfo.CurrentRootCompany.CompanyID;
            DocSet.OutputAction = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try
            {
                using (GAEntities db = new GAEntities())
                {
                    var i = db.ItemInfo.Where(o => o.ItemID == h.ItemID && o.RCompanyID == rcom).FirstOrDefault();
                    if (i == null)
                    {
                        checkDupID();
                        if (DocSet.OutputAction.Result == "fail")
                        {
                            return;
                        }

                        db.ItemInfo.Add(DocSet.Info);
                        db.SaveChanges();

                        TransactionInfoService.SaveLog(new TransactionLog { TransactionID = DocSet.Info.ItemID, TableID = "ITEM", ParentID = "", TransactionDate = DateTime.Now, CompanyID = DocSet.Info.CompanyID, Action = "INSERT NEW ITEM" });

                    }
                    else
                    {

                        i.ItemCode = DocSet.Info.ItemCode;
                        i.GLGroupID = DocSet.Info.GLGroupID;
                        i.RefID = DocSet.Info.RefID;
                        i.BrandID = DocSet.Info.BrandID;
                        i.Model = DocSet.Info.Model;
                        i.Size = DocSet.Info.Size;
                        i.Color = DocSet.Info.Color;
                        i.Name1 = DocSet.Info.Name1;
                        i.Name2 = DocSet.Info.Name2;
                        i.TypeID = DocSet.Info.TypeID;
                        i.CateID = DocSet.Info.CateID;
                        i.Group1ID = DocSet.Info.Group1ID;
                        i.Group2ID = DocSet.Info.Group2ID;
                        i.Group3ID = DocSet.Info.Group3ID;
                        i.SerialNumber = DocSet.Info.SerialNumber;
                        i.PackingID = DocSet.Info.PackingID;
                        i.VendorID = DocSet.Info.VendorID;
                        i.IsPackaging = DocSet.Info.IsPackaging;
                        i.PackagingID = DocSet.Info.PackagingID;
                        i.IsHold = DocSet.Info.IsHold;
                        i.Cost = DocSet.Info.Cost;
                        i.Price = DocSet.Info.Price;
                        i.UnitID = DocSet.Info.UnitID;
                        i.StkUnitID = DocSet.Info.StkUnitID;
                        i.Dimension = DocSet.Info.Dimension;
                        i.VatTypeID = DocSet.Info.VatTypeID;
                        i.IsKeepStock = DocSet.Info.IsKeepStock;
                        i.Sort = DocSet.Info.Sort;
                        i.Remark1 = DocSet.Info.Remark1;
                        i.Remark2 = DocSet.Info.Remark2;
                        i.ModifiedDate = DateTime.Now;
                        i.ModifiedBy = LoginService.LoginInfo.CurrentUser;
                        i.IsHold = DocSet.Info.IsHold;
                        i.IsActive = DocSet.Info.IsActive;

                        db.SaveChanges();
                        TransactionInfoService.SaveLog(new TransactionLog { TransactionID = DocSet.Info.ItemID, TableID = "ITEM", ParentID = "", TransactionDate = DateTime.Now, CompanyID = DocSet.Info.CompanyID, Action = "UPDATE ITEM" });

                    }


                }
            }
            catch (Exception ex)
            {
                DocSet.OutputAction.Result = "fail";
                if (ex.InnerException != null)
                {
                    DocSet.OutputAction.Message1 = ex.InnerException.ToString();
                }
                else
                {
                    DocSet.OutputAction.Message1 = ex.Message;
                }
            }
        }


        public static I_BasicResult AddPrice(ItemPriceInfo i)
        {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            var h = DocSet.Info;
            var rcom = LoginService.LoginInfo.CurrentRootCompany.CompanyID;
            try
            {
                using (GAEntities db = new GAEntities())
                {
                    var price = db.ItemPriceInfo.Where(o => o.ItemID == h.ItemID
                                                                && o.RCompanyID == rcom
                                                                && o.CompanyID == i.CompanyID
                                                                && o.CustID == i.CustID
                                                                && o.UseLevel == i.UseLevel
                                                                && o.DateBegin == i.DateBegin
                                                                && o.DateEnd == i.DateEnd
                                                                ).FirstOrDefault();
                    if (price == null)
                    {//add new item
                        db.ItemPriceInfo.Add(i);
                        db.SaveChanges();
                    }
                    else
                    {
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
            return result;
        }

        public static I_BasicResult AddItemInPointRate(ItemInPointRate data)
        {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            var h = DocSet.Info;
            var rcom = LoginService.LoginInfo.CurrentRootCompany.CompanyID;
            try
            {
                using (GAEntities db = new GAEntities())
                {
                    var pointrate = db.ItemInPointRate.Where(o=> o.RateID == data.RateID && o.RComID == rcom).FirstOrDefault();

                    if (pointrate == null)
                    {//add new 
                        db.ItemInPointRate.Add(data);
                        db.SaveChanges();
                    }
                    else
                    {

                    }
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
            return result;
        }


        public static bool checkDupID()
        {
            bool result = false;
            try
            {
                var rcom = LoginService.LoginInfo.CurrentRootCompany.CompanyID;
                DocSet.OutputAction = new I_BasicResult { Result = "fail", Message1 = "Duplicate iteminfo code", Message2 = "" };
                string itemid = DocSet.Info.ItemID;
                using (GAEntities db = new GAEntities())
                {
                    var get_id = db.ItemInfo.Where(o => o.ItemID == itemid && o.RCompanyID == rcom).FirstOrDefault();

                    if (get_id == null)
                    {
                        result = true;
                        DocSet.OutputAction = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
                    }


                }
            }
            catch (Exception ex)
            {
            }
            return result;
        }

        #endregion

        #region Delete

        public static void DeleteDoc(string docid)
        {
            DocSet.OutputAction = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            var rcom = LoginService.LoginInfo.CurrentRootCompany.CompanyID;
            try
            {
                using (GAEntities db = new GAEntities())
                {
                    var head = db.ItemInfo.Where(o => o.ItemID == docid && o.RCompanyID == rcom).FirstOrDefault();
                    head.Status = "IN-ACTIV";
                    head.ModifiedBy = LoginService.LoginInfo.CurrentUser;
                    head.ModifiedDate = DateTime.Now;
                    head.IsActive = false;

                    db.SaveChanges();
                    TransactionInfoService.SaveLog(new TransactionLog { TransactionID = DocSet.Info.ItemID, TableID = "ITEM", ParentID = "", TransactionDate = DateTime.Now, CompanyID = DocSet.Info.CompanyID, Action = "DELETE DATA ITEM" });
                }
            }
            catch (Exception ex)
            {
                DocSet.OutputAction.Result = "fail";
                if (ex.InnerException != null)
                {
                    DocSet.OutputAction.Message1 = ex.InnerException.ToString();
                }
                else
                {
                    DocSet.OutputAction.Message1 = ex.Message;
                }
            }
        }

        public static void DeleteItemPrice(int id)
        {
            DocSet.OutputAction = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            var rcom = LoginService.LoginInfo.CurrentRootCompany.CompanyID;
            try
            {
                using (GAEntities db = new GAEntities())
                {
                    db.ItemPriceInfo.Remove(db.ItemPriceInfo.Where(o => o.RCompanyID == rcom && o.ID == id).FirstOrDefault());
                    db.SaveChanges();
                }

            }
            catch (Exception ex)
            {

                DocSet.OutputAction.Result = "fail";

                if (ex.InnerException != null)
                {
                    DocSet.OutputAction.Message1 = ex.InnerException.ToString();
                }
                else
                {
                    DocSet.OutputAction.Message1 = ex.Message;
                }
            }
        }

        public static void DeleteItemInPointRate(string docid)
        {
            DocSet.OutputAction = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            var rcom = LoginService.LoginInfo.CurrentRootCompany.CompanyID;
            try
            {
                using (GAEntities db = new GAEntities())
                {
                    db.ItemInPointRate.Remove(db.ItemInPointRate.Where(o => o.RComID == rcom && o.RateID == docid).FirstOrDefault());
                    db.SaveChanges();
                }

            }
            catch (Exception ex)
            {

                DocSet.OutputAction.Result = "fail";

                if (ex.InnerException != null)
                {
                    DocSet.OutputAction.Message1 = ex.InnerException.ToString();
                }
                else
                {
                    DocSet.OutputAction.Message1 = ex.Message;
                }
            }
        }



        #endregion

        #region New transaction

        public static void NewFilterSet()
        {
            FilterSet = new I_ItemFiterSet();
            FilterSet.DateFrom = DateTime.Now.Date;
            FilterSet.DateTo = DateTime.Now.Date;
            FilterSet.DocType = "";
            FilterSet.SearchBy = "DOCDATE";
            FilterSet.SearchText = "";
            FilterSet.ShowActive = true;
        }

        public static void NewTransaction(string doctype)
        {
            DocSet = new I_ItemSet();
            DocSet.Action = "";
            DocSet.Info = NewItem();
            DocSet.LineItem = new List<ItemInfo>();
            DocSet.VItemPrice = new List<vw_ItemPriceInfo>();
            DocSet.ItemPointRate = new List<ItemInPointRate>();
            DocSet.NeedRunNextID = false;
            DocSet.Log = new List<TransactionLog>();
            DocSet.OutputAction = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            IsNewDoc = false;
        }

        public static ItemInfo NewItem()
        {
            ItemInfo n = new ItemInfo();
            n.ItemID = "";
            n.MasterItemID = "";
            n.CompanyID = LoginService.LoginInfo.CurrentCompany == null ? "" : LoginService.LoginInfo.CurrentCompany.CompanyID;
            n.RCompanyID = LoginService.LoginInfo.CurrentRootCompany.CompanyID;
            n.ItemCode = "";
            n.Barcode = "";
            //n.RootID = "";
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
            n.CreatedBy = LoginService.LoginInfo.CurrentUser;
            n.CreatedDate = DateTime.Now;
            n.ModifiedBy = "";
            n.ModifiedDate = null;
            n.Status = "";
            n.Sort = 0;
            n.IsHold = false;
            n.IsActive = true;
            return n;
        }

        public static ItemPriceInfo NewPrice()
        {
            ItemPriceInfo newdata = new ItemPriceInfo();
            newdata.ItemID = "";
            newdata.CompanyID = LoginService.LoginInfo.CurrentCompany == null ? "" : LoginService.LoginInfo.CurrentCompany.CompanyID;
            newdata.RCompanyID = LoginService.LoginInfo.CurrentRootCompany.CompanyID;
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

        public static ItemInPointRate NewItemPoint()
        {
            ItemInPointRate newdata = new ItemInPointRate();
            newdata.RateID = Guid.NewGuid().ToString();
            newdata.ItemID = "";
            newdata.ComID = LoginService.LoginInfo.CurrentCompany == null ? "" : LoginService.LoginInfo.CurrentCompany.CompanyID;
            newdata.RComID = LoginService.LoginInfo.CurrentRootCompany.CompanyID;
            newdata.AmtPerPointRate = 0;           
            newdata.DateBegin = DateTime.Now.Date;
            newdata.DateEnd = DateTime.Now.Date;
            newdata.ExpireInMont = 0;
            newdata.IsActive = true;
            return newdata;
        }



        #endregion

        #region Item price
        public static I_BasicResult CopyPrice(string copyFromComId, string copyToComId)
        {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };

            var rcom = LoginService.LoginInfo.CurrentRootCompany.CompanyID;
            List<ItemPriceInfo> desc_price = new List<ItemPriceInfo>();
            try
            {
                using (GAEntities db = new GAEntities())
                {
                    var p_source = db.ItemPriceInfo.Where(o => o.RCompanyID == rcom && o.IsActive).ToList();
                    foreach (var p in p_source)
                    {
                        p.CompanyID = copyToComId;
                        desc_price.Add(p);
                    }

                }
                string strCon = DBDapperService.GetDBConnectFromAppConfig();
                using (SqlConnection conn = new SqlConnection(strCon))
                {
                    conn.Open();
                    string sql = "delete from ItemPriceInfo where RCompanyID = @rcom and CompanyID=@com";
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("rcom", rcom);
                    dynamicParameters.Add("com", copyToComId);
                    var row = conn.Execute(sql, dynamicParameters);
                    foreach (var f in desc_price)
                    {

                        sql = @"insert into  ItemPriceInfo  (
                               RCompanyID
                              ,CompanyID
                              ,ItemID
                              ,CustID
                              ,Price
                              ,PriceTaxCondType
                              ,RefID
                              ,DataSource
                              ,Remark
                              ,DateBegin
                              ,DateEnd
                              ,IsActive
	                          ) values(
                               @RCompanyID
                              ,@CompanyID
                              ,@ItemID
                              ,@CustID
                              ,@Price
                              ,@PriceTaxCondType
                              ,@RefID
                              ,@DataSource
                              ,@Remark
                              ,@DateBegin
                              ,@DateEnd
                              ,@IsActive                           
                         )";
                        conn.Execute(sql, new
                        {
                            @RCompanyID=f.RCompanyID,
                            @CompanyID = f.CompanyID,
                            @ItemID = f.ItemID,
                            @CustID = f.CustID,
                            @Price = f.Price,
                            @PriceTaxCondType = f.PriceTaxCondType,
                            @RefID = f.RefID,
                            @DataSource = f.DataSource,
                            @Remark = f.Remark,
                            @DateBegin = f.DateBegin,
                            @DateEnd = f.DateEnd,
                            @IsActive = f.IsActive

                        });
                    }

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
            return result;
        }
        #endregion

        #region function move data management
        public static ItemInfo GetPreviousData(string currID)
        {
            ItemInfo result = new ItemInfo();
            var rcom = LoginService.LoginInfo.CurrentRootCompany.CompanyID;
            using (GAEntities db = new GAEntities())
            {
                var curr = db.ItemInfo.Where(o => o.ItemID == currID && o.RCompanyID == rcom).FirstOrDefault();
                result = db.ItemInfo.Where(o => o.ID < curr.ID && o.RCompanyID == rcom && o.IsActive).FirstOrDefault();
            }
            return result;
        }
        public static ItemInfo GetLastData()
        {
            var rcom = LoginService.LoginInfo.CurrentRootCompany.CompanyID;
            ItemInfo result = new ItemInfo();
            using (GAEntities db = new GAEntities())
            {
                result = db.ItemInfo.Where(o => o.RCompanyID == rcom).OrderByDescending(o => o.ID).FirstOrDefault();
            }
            return result;
        }

        public static ItemInfo GetNextData(string currID)
        {
            var rcom = LoginService.LoginInfo.CurrentRootCompany.CompanyID;
            ItemInfo result = new ItemInfo();
            using (GAEntities db = new GAEntities())
            {
                var curr = db.ItemInfo.Where(o => o.ItemID == currID && o.RCompanyID == rcom).FirstOrDefault();
                result = db.ItemInfo.Where(o => o.ID > curr.ID && o.RCompanyID == rcom && o.IsActive).FirstOrDefault();
            }
            return result;
        }

        #endregion
    }
}