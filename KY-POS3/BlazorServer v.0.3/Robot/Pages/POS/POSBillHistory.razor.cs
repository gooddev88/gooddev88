using Robot.Data.DA.POSSY;
using Robot.Data.DA;
using Robot.Data.GADB.TT;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Collections.Generic;
using System.Linq;

using System;
using static Robot.Data.DA.POSSY.ShipToService;
using static Robot.Data.ML.I_Result;
using System.Text.Json;
using Blazored.SessionStorage;
using Robot.Data.DA.LoginCrossApp;
using Robot.Tools;

namespace Robot.Pages.POS {
    public class POSBillHistoryBase : ComponentBase {
        [Inject]
        public CompanyService comService { get; set; }
        [Inject]
        public POSService posService { get; set; }

        [Inject]
        public ISessionStorageService sessionStorage { get; set; }
        [Inject]
        public LogInService login { get; set; }
        [Inject]
        public IJSRuntime JsRuntime { get; set; }
        [Inject]
        public NavigationManager nav { get; set; }
        [Inject]
        public PageHistoryState pageHistory { get; set; }
        [Inject]
        public ShipToService shiptoService { get; set; }
        [Inject]
        public Blazored.LocalStorage.ILocalStorageService localStorage { get; set; }

        [Parameter]
        public string pagecomefrom { get; set; } = "";



     public  string menuCaption = "";
       public string menuGroupCaption = "";


        public bool isLoading = true;
        public POSService.I_BillFilterSet filter = POSService.NewFilterSet();


    
        public string pagingSummaryFormat = "Displaying page {0} of {1} (total {2} records)";
        public int pageSize = 50;
        public int skip = 0;
        public int take = 50;
        public int count;

        public List<ShipToService.ShipTo> ListShipTo = new List<ShipToService.ShipTo>();
        public List<POSSaleConverterService.POS_SaleHeadModel> BillHistoryList = new List<POSSaleConverterService.POS_SaleHeadModel>();
        public List<CompanyService.CompanyInfoList> ListCompany = new List<CompanyService.CompanyInfoList>();

        protected override async Task OnInitializedAsync() {
            pageHistory.AddPageToHistory(nav.Uri);
            await Task.Run(() => login.CheckLogin());
            await Task.Run(() => LoadFilterData());
            await Task.Run(LoadData);
            SetActiveControl();
            isLoading = false;
        }

        private void SetActiveControl() {
            CheckPermission();
        }
        private void CheckPermission() {
            var menu = LogInService.GetMenuInfo(login.LoginInfo, "5004");
            menuCaption = menu.Name;
            menuGroupCaption = LogInService.GetMenuGroup(login.LoginInfo, menu.GroupID).Name;

            if (!login.CanOpen(login.LoginInfo, "5004")) {
                nav.NavigateTo("NoPermissionPage");
            }
        }

        async public void LoadFilterData() {
            await Task.Run(() => GetSessionFilter());
            ListCompany = CompanyService.ListCompanyInfoUIC(login.LoginInfo, "BRANCH", false);
            ListShipTo = ShipToService.ListShipTo();
            ShipTo ship = new ShipTo {
                ShipToID = "X",
                ShipToName = "ทั้งหมด",
                ShortID = "",
                UsePrice = "",
                ImageUrl = ""
            };
            ListShipTo.Insert(0, ship);
            if (string.IsNullOrEmpty(filter.ComID) && ListCompany.Count>0) {
                filter.ComID = ListCompany.FirstOrDefault().CompanyID;
            }
            await InvokeAsync(StateHasChanged);

        }
        async public void LoadData() {
            isLoading = true;
            filter.RComID = login.LoginInfo.CurrentRootCompany.CompanyID;
            await Task.Run(() => SetSessionFilter());
            BillHistoryList = POSService.ListBill(filter, skip, take);
            count = BillHistoryList.Count();
            isLoading = false;
            await InvokeAsync(StateHasChanged);
        }
        public async void GoToPOSSale(POSSaleConverterService.POS_SaleHeadModel data) { 
            posService.DocSet = await Task.Run(() => posService.GetDocSet(data.BillID, data.RComID));
            await sessionStorage.SetItemAsync(POSService.sessionActiveId, data.BillID);
            await Task.Run(() => login.SetLoginSessionLog(login.LoginInfo));
            nav.NavigateTo($"POS/POSSaleDetail/{pagecomefrom}", false);
            await InvokeAsync(StateHasChanged);
        }

        public async void Back() {
            switch (pagecomefrom) {
                case "web1":
                    var reqInfo = LogInCrossAppService.CreateReqInfo("KYPOS", login.LoginInfo.CurrentRootCompany.CompanyID, login.LoginInfo.CurrentUser, "");
                    string rootappurl = login.GetRootApp($"/Account/MyLogin/LoginFromApp?reqid={reqInfo.ReqID}");
                    nav.NavigateTo(rootappurl);
                    break;
                case "web2": 
                    var url = pageHistory.GetGoBackPage();
                    nav.NavigateTo(url);
                    await InvokeAsync(StateHasChanged);
                    break;
            }
        }

        public async Task<I_BasicResult> SetSessionFilter() {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                JsonSerializerOptions jso = new JsonSerializerOptions();
                jso.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
                string json = JsonSerializer.Serialize(filter, jso);
                await sessionStorage.SetItemAsStringAsync("pos_list_filter", json);
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
        public async Task GetSessionFilter() {
            POSService.I_BillFilterSet output = new POSService.I_BillFilterSet();
            try {
                string json = await sessionStorage.GetItemAsStringAsync("pos_list_filter");
                filter = JsonSerializer.Deserialize<POSService.I_BillFilterSet>(json);
            } catch (Exception ex) {
                await SetSessionFilter();
            }


        }

    }
}
