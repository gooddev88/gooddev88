
using Robot.Data.DA.API.Model;
using Robot.Data.GADB.TT;
using Robot.Service.FileGo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Robot.Data.ML.I_Result;

namespace Robot.Data.DA.API.APP {
    public   class SyncMasterDataService {
     public   FileGo _filego;
        public SyncMasterDataService(FileGo filego) {
            _filego = filego;
        }
        public class I_MasterDataSet {
            public DateTime MyXDate { get; set; }
            public List<IDGenerator> IDGenerator { get; set; }
            public List<ItemInfo> ItemInfo { get; set; }
            public List<vw_ItemRelate> ItemRelate { get; set; }
            public List<ItemPriceInfoM> ItemPriceInfoM { get; set; }
            public List<ItemImageM> ItemImageM { get; set; }
            public List<vw_ItemInSellTime> ItemInSellTime { get; set; }
            public List<CompanyInfo> CompanyInfo { get; set; }
            public List<UserInfoM> UserInfoM { get; set; }
            public List<UserInComM> UserInComM { get; set; }
            public List<UserInMenuM> UserInMenuM { get; set; }
            public List<MasterTypeM> MasterTypes { get; set; }
            public List<POS_Table> Tables { get; set; }
            public I_BasicResult OutputAction { get; set; }

        }

     async   public   Task<I_MasterDataSet> GetMasterData(string rcom, string com, string username) {
            I_MasterDataSet result = new I_MasterDataSet();
            result.OutputAction = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                result.IDGenerator = ListIDGenerator(rcom, com);
                result.ItemInfo = ListItemFG(rcom);
               result.ItemRelate = ListItemRelate(rcom);
                result.ItemPriceInfoM = ListItemFGPrice(rcom, com);
                result.ItemImageM =await Task.Run(()=> ListItemFGImageM(rcom, com));
                result.ItemInSellTime = ListItemInSellTime(rcom, com);
                result.CompanyInfo = ListCompany(rcom, com);
                result.UserInfoM = ListUserInfoM(rcom, username);
                result.UserInComM = ListUserInCompanyM(rcom, username);
                result.UserInMenuM = ListUserInMenuM(username, rcom);
                result.MasterTypes = ListMasterTypeM(rcom);
                result.Tables = ListPOSTable(rcom);
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


        public static List<IDGenerator> ListIDGenerator(string rcom, string com) {
            List<IDGenerator> result = new List<IDGenerator>();
            DateTime limitDate = DateTime.Now.Date.AddMonths(-2);
            using (GAEntities db = new GAEntities()) {
                result = db.IDGenerator.Where(o => o.ComID == com && o.RComID == rcom && o.FuncID == "SALE" && o.CreatedDate >= limitDate).ToList();

            }
            return result;
        }
        public static List<UserInfoM> ListUserInfoM(string rcom, string username) {
            List<UserInfoM> result = new List<UserInfoM>();
            using (GAEntities db = new GAEntities()) {
                // var uirc = db.UserInRCom.Where(o => o.RComID == rcom).Select(o => o.UserName).ToList();
                var query = db.UserInfo.Where(o => o.IsActive == true
                                                    && o.IsProgramUser
                                                    && o.Username == username
                                                    // && uirc.Contains(o.Username)
                                                    ).ToList();
                foreach (var q in query) {
                    var n = new UserInfoM {
                        Username = q.Username,
                        Password = q.Password,
                        FullName = q.FullName,
                        Tel = q.Tel,
                        Email = q.Email,
                        RComID = "",
                        ComID = "",
                        IsActive = Convert.ToBoolean(q.IsActive)

                    };
                    result.Add(n);
                }
            }
            return result;
        }
        public static List<UserInComM> ListUserInCompanyM(string rcom, string username) {
            List<UserInComM> result = new List<UserInComM>();
            using (GAEntities db = new GAEntities()) {
                var query = db.vw_UIC.Where(o => o.RCompanyID == rcom && o.UserName == username).ToList();
                foreach (var q in query) {
                    var n = new UserInComM {
                        Username = q.UserName,
                        CompanyID = q.CompanyID,
                        RCompanyID = q.RCompanyID,
                        IsActive = true
                    };
                    result.Add(n);
                }
            }
            return result;
        }
        public static List<UserInMenuM> ListUserInMenuM(string user = "", string rcom = "") {
            List<UserInMenuM> result = new List<UserInMenuM>();
            using (GAEntities db = new GAEntities()) {
                var query = db.vw_UIM.Where(o =>
                                                    (o.UserName.ToLower() == user.ToLower() || user == "")
                                                    && o.RComID == rcom
                                                ).ToList();

                foreach (var q in query) {
                    var n = new UserInMenuM {

                        Username = q.UserName,
                        MenuID = q.MenuID,
                        MenuName = q.MenuName,
                        IsOpen = Convert.ToInt32(q.IsOpen),
                        IsCreate = Convert.ToInt32(q.IsCreate),

                        IsEdit = Convert.ToInt32(q.IsEdit),
                        IsPrint = Convert.ToInt32(q.IsPrint),
                        IsDelete = Convert.ToInt32(q.IsDelete)

                    };
                    result.Add(n);
                }

                return result;
            }
        }
        public static List<MasterTypeM> ListMasterTypeM(string rcom) {
            List<MasterTypeM> result = new List<MasterTypeM>();
            using (GAEntities db = new GAEntities()) {
                var query = db.MasterTypeLine.Where(o =>
                                                        o.IsActive == true
                                                        && (o.RComID == rcom || o.RComID == "")
                                                            ).ToList();

                foreach (var q in query) {

                    var n = new MasterTypeM {
                        ID = q.ID,
                        RComID = q.RComID,
                        MasterTypeID = q.MasterTypeID,
                        ValueTXT = q.ValueTXT,
                        ValueNUM = q.ValueNUM,
                        Description1 = q.Description1,
                        Description2 = q.Description2,
                        Description3 = q.Description3,
                        Description4 = q.Description4,
                        ParentID = q.ParentID,
                        Sort = q.Sort
                    };
                    result.Add(n);
                }

                return result;
            }
        }
        public static List<POS_Table> ListPOSTable(string rcom) {
            List<POS_Table> result = new List<POS_Table>();
            using (GAEntities db = new GAEntities()) {

                result = db.POS_Table.Where(o => o.IsActive == true
                                                              && (o.RComID == rcom || rcom == "")
                                                              ).ToList();


            }
            return result;
        }
        public static List<CompanyInfo> ListCompany(string rcom, string com) {
            List<CompanyInfo> result = new List<CompanyInfo>();

            using (GAEntities db = new GAEntities()) {
                var cominfo = db.CompanyInfo.Where(o => o.IsActive == true
                                                         && (o.CompanyID == com || com == "")
                                                         && o.RCompanyID == rcom
                                                         && o.TypeID == "BRANCH"
                                                           ).ToList();
                var rcominfo = db.CompanyInfo.Where(o => o.CompanyID == rcom && o.TypeID == "COMPANY").ToList();
                result.AddRange(rcominfo);
                result.AddRange(cominfo);

            }
            return result;
        }

        public static List<ItemInfo> ListItemFG(string rcom) {
            List<ItemInfo> result = new List<ItemInfo>();

            List<string> filter = new List<string> { "FG", "DISCOUNT" };

            using (GAEntities db = new GAEntities()) {

                result = db.ItemInfo.Where(o => o.IsActive == true
                                            && o.RCompanyID == rcom
                                            && (filter.Contains(o.TypeID))
                                            ).ToList();

                return result;
            }
        }
        public static List<vw_ItemRelate> ListItemRelate(string rcom) {
            List<vw_ItemRelate> result = new List<vw_ItemRelate>();
            using (GAEntities db = new GAEntities()) {

                result = db.vw_ItemRelate.Where(o => o.IsActive == true
                                            && o.RCompanyID == rcom
                                            ).ToList();

                return result;
            }
        }
         public   async Task< List<ItemImageM>> ListItemFGImageM(string rcom, string com) {
            List<ItemImageM> result = new List<ItemImageM>();
            var item = new List<ItemInfo>();


            using (GAEntities db = new GAEntities()) {
                List<string> filter = new List<string> { "FG", "DISCOUNT" };
                item = db.ItemInfo.Where(o => o.IsActive == true
                                                        && o.RCompanyID == rcom
                                                        && (o.CompanyID == com || o.CompanyID == "")
                                                        && (filter.Contains(o.TypeID))
                                                        ).ToList();

                foreach (var i in item) {
                    string get_img = "";
                    //if (i.ItemID== "B003") {
                    //    var xx = "";
                    //}
                 
                    //get_img = XFilesService.GetFileRefByDocAndTableSource2B64(i.RCompanyID, i.CompanyID, i.ItemID, "ITEMS_PHOTO_PROFILE",false, true);
                    get_img =await   Task.Run(()=> _filego.GetThumbBase64(i.RCompanyID, i.CompanyID, "ITEMS_PHOTO_PROFILE", i.ItemID));

                    result.Add(new ItemImageM { ItemID = i.ItemID, Image = get_img, ComID = i.CompanyID, RComID = i.RCompanyID });
                }
                return result;
            }
        }
        public static List<vw_ItemInSellTime> ListItemInSellTime(string rcom, string com) {
            List<vw_ItemInSellTime> result = new List<vw_ItemInSellTime>();
            DateTime today = DateTime.Now.Date;
            using (GAEntities db = new GAEntities()) {
                result = db.vw_ItemInSellTime.Where(o => o.RComID == rcom
                                                            && (o.ComID == com || o.ComID == "")
                                                            && o.IsActive
                                                            && (o.ActiveDateFr <= today && o.ActiveDateTo >= today)
                                                            ).ToList();
            }
            return result;
        }
        public static List<ItemPriceInfoM> ListItemFGPrice(string rcom, string com) {
            List<ItemPriceInfoM> result = new List<ItemPriceInfoM>();

            List<string> filter = new List<string> { "FG", "DISCOUNT" };
            using (GAEntities db = new GAEntities()) {

                var query = db.vw_ItemPriceInfo.Where(o => o.IsActive
                                                                        && o.RCompanyID == rcom
                                                                        && (o.CompanyID == com || o.CompanyID == "")
                                                                        && (filter.Contains(o.TypeID))
                                                                        ).ToList();
                var discount = db.ItemInfo.Where(o => o.TypeID == "DISCOUNT" && o.RCompanyID == rcom && o.IsActive == true).ToList();
                int row = 1;
                foreach (var q in query) {
                    var n = new ItemPriceInfoM {
                        ID = row,
                        ItemID = q.ItemID,
                        ItemName = q.ItemName,
                        CompanyID = q.CompanyID,
                        RCompanyID = q.RCompanyID,
                        UseLevel = q.UseLevel,
                        DateBegin = q.DateBegin,
                        DateEnd = q.DateEnd,
                        SubLocID = "",
                        CustID = q.CustID,
                        TypeID = q.TypeID,
                        CateID = q.CateID,
                        Price = q.Price,
                        PriceTaxCondType = q.PriceTaxCondType,
                        Remark = q.Remark,
                        //Image = Convert.ToBase64String( i.Data)
                        Image = ""
                    };
                    result.Add(n);
                    row++;
                }
                row = 1;
                foreach (var q in discount) {
                    var n = new ItemPriceInfoM {
                        ID = row,
                        ItemID = q.ItemID,
                        ItemName = q.Name1,
                        CompanyID = q.CompanyID,
                        RCompanyID = "",
                        SubLocID = "",
                        CustID = "",
                        TypeID = q.TypeID,
                        CateID = q.CateID,
                        Price = q.Price,
                        PriceTaxCondType = "INC VAT",
                        Remark = "",
                        //Image = Convert.ToBase64String( i.Data)
                        Image = ""
                    };
                    result.Add(n);
                    row++;
                }
            }

            //result = Database.Table<CompanyInfo>().Where(o => o.CompanyID == companyid).FirstOrDefault();
            return result;
        }
    }
}
