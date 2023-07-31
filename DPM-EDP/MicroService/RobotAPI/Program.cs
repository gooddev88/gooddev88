using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RobotAPI.Controllers.Cims.Data;
using RobotAPI.Data;
using RobotAPI.Data.DA.Data;
using RobotAPI.Data.DA.Portal;
using RobotAPI.Data.DA.User;
using RobotAPI.Data.DA.Weather.Waterboard;
using RobotAPI.Data.DataStoreDB.TT;
using RobotAPI.Data.DpmQDB.TT;
using RobotAPI.Data.MainDB.TT;
using RobotAPI.Data.XFilesCenterDB.TT;
using RobotAPI.Helpers.Jwt;
using RobotAPI.Models;
using RobotAPI.Services.Api;
using RobotAPI.Services.Jwt;


var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;
IWebHostEnvironment environment = builder.Environment;
Globals.CMSConn = configuration["CMSConnection:ConnectionString"];
Globals.CimsConn = configuration["CIMSContext:ConnectionString"];
Globals.RoomConn = configuration["RoomContext:ConnectionString"];
Globals.MainContextConn = configuration["MainContext:ConnectionString"];
Globals.DPMQContextConn = configuration["DPMQContext:ConnectionString"];
Globals.XfilescenterContextConn = configuration["XfilescenterContext:ConnectionString"];

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// Add services to the container.
//builder.Services.AddDbContext<D>(options => options.UseNpgsql(Globals.DataStoreConn));
builder.Services.AddDbContext<CIMSContext>(options => options.UseNpgsql(Globals.CimsConn));
builder.Services.AddDbContext<MainContext>(options => options.UseSqlServer(configuration["MainContext:ConnectionString"]).UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking), ServiceLifetime.Transient);
builder.Services.AddDbContext<DPMQContext>(options => options.UseSqlServer(configuration["DPMQContext:ConnectionString"]).UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking), ServiceLifetime.Transient);



builder.Services.AddDbContext<XfilescenterContext>(options => options.UseSqlServer(configuration["XfilescenterContext:ConnectionString"]).UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking), ServiceLifetime.Transient);

//const string AllowAllHeadersPolicy = "AllowAllHeadersPolicy";
//builder.Services.AddCors(options =>
//{
//    options.AddDefaultPolicy(
//    builder => {
//        builder.WithOrigins("https://localhost:44398")
//        .AllowAnyHeader()
//        .AllowAnyMethod()
//        .AllowCredentials();
//    });
//});


builder.Services.AddCors(p => p.AddPolicy("corsapp", builder => {
    builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
}));




builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => {
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme."
    });
    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
                new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Reference = new Microsoft.OpenApi.Models.OpenApiReference
                    {
                        Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                new string[] {}
        }
    });
});
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options => {
            options.TokenValidationParameters = new TokenValidationParameters {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = TokenHelper.Issuer,
                ValidAudience = TokenHelper.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Convert.FromBase64String(TokenHelper.Secret)),
                ClockSkew = TimeSpan.Zero
            };

        });

builder.Services.AddAuthorization();
builder.Services.AddTransient<IJwtTokenService, JwtTokenService>();
builder.Services.AddTransient<IJwtUserService, JwtUserService>();
builder.Services.AddScoped<ClientService, ClientService>();
builder.Services.AddScoped<PortalService, PortalService>();
builder.Services.AddScoped<Data500Service, Data500Service>();
builder.Services.AddScoped<Data100Service, Data100Service>();
builder.Services.AddScoped<Data200Service, Data200Service>();
builder.Services.AddScoped<Data300Service, Data300Service>();
builder.Services.AddScoped<Data400Service, Data400Service>();
builder.Services.AddScoped<Data600Service, Data600Service>();
builder.Services.AddScoped<Data700Service, Data700Service>();
builder.Services.AddScoped<EOCService, EOCService>();
builder.Services.AddScoped<WaterService, WaterService>();

builder.Services.AddCors(p => p.AddPolicy("corsapp", builder => {
    builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
}));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}
//app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod());
app.UseCors("corsapp");
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
