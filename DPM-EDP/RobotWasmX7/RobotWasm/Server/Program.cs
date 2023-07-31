using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using RobotWasm.Server.Data;
using RobotWasm.Server.Data.CimsDB.TT;

using Blazorise;
using Blazorise.Bootstrap5;
using RobotWasm.Server.Data.DA.Board;
using Blazored.LocalStorage;
using Blazored.SessionStorage;
using RobotWasm.Server.Data.GaDB;
using RobotWasm.Server.Data.DA.Login;
using RobotWasm.Server.Service.FileGo;
using RobotWasm.Client.Service.Api;
//using RobotWasm.Client.Data.DA.MasterType;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
ConfigurationManager configuration = builder.Configuration;
Globals.CimsConn = configuration["CIMSContext:ConnectionString"];
Globals.GAEntitiesConn = configuration["GAEntities:ConnectionString"];

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddDbContext<cimsContext>(options => options.UseNpgsql(Globals.CimsConn));
builder.Services.AddDbContext<GAEntities>(options => options.UseSqlServer(Globals.GAEntitiesConn).UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking), ServiceLifetime.Transient);

builder.Services.AddBlazoredSessionStorage();
builder.Services.AddBlazoredLocalStorage();
//Blazorise
builder.Services
    .AddBlazorise(options => {
        options.Immediate = true;
    })
    .AddBootstrap5Providers();

builder.Services.AddScoped<ApiMasterService, ApiMasterService>();
builder.Services.AddScoped<ClientService, ClientService>();
builder.Services.AddScoped<LogInService, LogInService>();
builder.Services.AddScoped<LogInCIMSService, LogInCIMSService>();
builder.Services.AddScoped<FileGoService, FileGoService>();

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
