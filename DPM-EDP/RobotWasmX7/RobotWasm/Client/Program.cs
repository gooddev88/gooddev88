using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
 
using RobotWasm.Client;
using RobotWasm.Client.Data.DA.Login;
using RobotWasm.Client.Pages.DPMBoard.A00;
using RobotWasm.Client.Pages.DPMBoard.A01;
using RobotWasm.Client.Service.Api;
using RobotWasm.Client.Service.Authen;

using Blazorise;
using Blazorise.Bootstrap5;
using Blazorise.Icons.FontAwesome;
using Blazored.SessionStorage;
using RobotWasm.Client.Data.DA.Data;
using RobotWasm.Client.Data.DA.Xfiles;
using RobotWasm.Client.Data.DA.Q;
using CurrieTechnologies.Razor.SweetAlert2;
using RobotWasm.Client.Data.DA.UserGroup;
using RobotWasm.Client.Data.DA.Tableau;
using RobotWasm.Client.Data.DA.Board;
using RobotWasm.Client.Data.DA.Board.G10;
using RobotWasm.Client.Data.DA.Master;
using RobotWasm.Client.Data.DA.Document;
using RobotWasm.Client.Data.DA.DataQuality;
using FisSst.BlazorMaps.DependencyInjection;
using RobotWasm.Client.Data.DA.GenDoc;
using RobotWasm.Client.Data.Helper;


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
builder.Services.AddBlazorLeafletMaps();
builder.Services.AddBlazoredSessionStorage();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, ApiAuthenticationStateProvider>();

builder.Services.AddScoped<LoginService, LoginService>();
builder.Services.AddScoped<MasterTypeService, MasterTypeService>();
builder.Services.AddScoped<LogTranService, LogTranService>();
builder.Services.AddScoped<QService, QService>();
builder.Services.AddScoped<QFloodService, QFloodService>();
builder.Services.AddScoped<TableauService, TableauService>();
builder.Services.AddScoped<ClientService, ClientService>();

builder.Services.AddScoped<FileGoService, FileGoService>();

builder.Services.AddScoped<UserService, UserService>();
builder.Services.AddScoped<UserGroupService, UserGroupService>();
builder.Services.AddScoped<ApiMasterService, ApiMasterService>();
builder.Services.AddScoped<ApiCateService, ApiCateService>();
builder.Services.AddScoped<DocCateService, DocCateService>();
builder.Services.AddScoped<PublishDocumentService, PublishDocumentService>();
builder.Services.AddScoped<PublishDocService, PublishDocService>();
builder.Services.AddScoped<BoardMasterService, BoardMasterService>();
builder.Services.AddScoped<NewsService, NewsService>();
builder.Services.AddScoped<DataQualityService, DataQualityService>();
builder.Services.AddScoped<CompanyGroupService, CompanyGroupService>();
builder.Services.AddScoped<CompanyService, CompanyService>();
builder.Services.AddScoped<IDRuunerService, IDRuunerService>();
builder.Services.AddScoped<IconSetService, IconSetService>();

builder.Services.AddScoped<CustomBoardService, CustomBoardService>();
builder.Services.AddScoped<BoardService, BoardService>();
builder.Services.AddScoped<G10Service, G10Service>();
builder.Services.AddScoped<A00Data, A00Data>();
builder.Services.AddScoped<A01Data, A01Data>();
builder.Services.AddSweetAlert2();

//builder.Services.AddSyncfusionBlazor(options => { options.IgnoreScriptIsolation = true; });

await builder.Build().RunAsync();
