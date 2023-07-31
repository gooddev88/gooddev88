using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

using RobotWasm.Client;
using RobotWasm.Client.Data.DA.Login;
using RobotWasm.Client.Service.Api;
using RobotWasm.Client.Service.Authen;

using Blazorise;
using Blazorise.Bootstrap5;
using Blazorise.Icons.FontAwesome;
using Blazored.SessionStorage;
using RobotWasm.Client.Data.DA.Xfiles;
using CurrieTechnologies.Razor.SweetAlert2;
using RobotWasm.Client.Data.DA.UserGroup;
using RobotWasm.Client.Data.DA.Master;
using RobotWasm.Client.Data.DA.GenDoc;
using RobotWasm.Client.Data.DA.Order.SO;
using RobotWasm.Client.Data.DA.Stock;
using RobotWasm.Client.Data.DA.Promotion;


//Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NjMyNjE1QDMyMzAyZTMxMmUzME02dHJ1bDhwWnpjOE03MWxmWGRIclRnN1l6SzNUNVhKWUU0MDIxUWRJQnc9;NjMyNjE2QDMyMzAyZTMxMmUzME14dnc3c0pzSTQwMW9ESzNZSXA4U05JYU1MVWU3WTc5dHJFTkIyZDYvckk9");
var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

//Blazorise
builder.Services
    .AddBlazorise(options => {
        options.Immediate = true;
    })
    .AddBootstrap5Providers()
    .AddFontAwesomeIcons();


builder.Services.AddTelerikBlazor();

builder.Services.AddBlazoredSessionStorage();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, ApiAuthenticationStateProvider>();

builder.Services.AddScoped<LoginService, LoginService>();
builder.Services.AddScoped<MasterTypeService, MasterTypeService>();
 
  
builder.Services.AddScoped<ClientService, ClientService>();

builder.Services.AddScoped<FileGoService, FileGoService>();

builder.Services.AddScoped<UserService, UserService>();
builder.Services.AddScoped<UserGroupService, UserGroupService>();
 
 
builder.Services.AddScoped<IDRuunerService, IDRuunerService>();

builder.Services.AddScoped<CompanyService, CompanyService>();
//builder.Services.AddScoped<POSService, POSService>();
//builder.Services.AddScoped<TableInfoService, TableInfoService>();
builder.Services.AddScoped<SOService, SOService>();
builder.Services.AddScoped<PromotionService,PromotionService>();
builder.Services.AddScoped<StockBalanceService, StockBalanceService>();
builder.Services.AddScoped<CustomerService,CustomerService>();
builder.Services.AddSweetAlert2();
 
//builder.Services.AddSyncfusionBlazor(options => { options.IgnoreScriptIsolation = true; });

await builder.Build().RunAsync();
