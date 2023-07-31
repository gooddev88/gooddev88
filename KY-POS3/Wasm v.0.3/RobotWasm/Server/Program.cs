using Microsoft.EntityFrameworkCore;
using RobotWasm.Server.Data;

using Blazorise;
using Blazorise.Bootstrap5;
using Blazored.LocalStorage;
using Blazored.SessionStorage;
using RobotWasm.Server.Data.GaDB;
using RobotWasm.Server.Data.DA.Login;
using RobotWasm.Server.Service.FileGo;
using RobotWasm.Client.Service.Api;
using RobotWasm.Server.Data.DA.Master;
using RobotWasm.Server.Data.DA.POS;
using RobotWasm.Server.Data.DA.GenDoc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
ConfigurationManager configuration = builder.Configuration; 
Globals.GAEntitiesConn = configuration["GAEntities:ConnectionString"]; 
Globals.ApiPrintMasterBaseUrl = configuration["PrintMaster:BaseUrl"];
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages(); 
builder.Services.AddDbContext<GAEntities>(options => options.UseSqlServer(Globals.GAEntitiesConn).UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking), ServiceLifetime.Transient);


builder.Services.AddBlazoredSessionStorage();
builder.Services.AddBlazoredLocalStorage();
//Blazorise
builder.Services
    .AddBlazorise(options => {
        options.Immediate = true;
    })
    .AddBootstrap5Providers();
 
builder.Services.AddScoped<ClientService, ClientService>();
builder.Services.AddScoped<LogInService, LogInService>(); 
builder.Services.AddScoped<FileGoService, FileGoService>();

builder.Services.AddScoped<IDRuunerService, IDRuunerService>();

builder.Services.AddScoped<CompanyService, CompanyService>();
builder.Services.AddScoped<MastertypeService, MastertypeService>();
builder.Services.AddScoped<POSService, POSService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseWebAssemblyDebugging();
} else {
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseCors("corsapp");
app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();


app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
