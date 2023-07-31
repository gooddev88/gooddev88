using Blazored.LocalStorage;
using Blazored.Modal;
using Blazored.SessionStorage;
using Blazorise;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
 
using Robot.Data;
using Robot.Data.DA;
using Robot.Data.DA.LoginCrossApp;
using Robot.Data.DA.MiscService;
using Robot.Data.DA.POSSY;
using Robot.Data.DA.User;
using Robot.Data.FILEDB.TT;
using Robot.Data.GADB.TT;
using System;
using System.IO;
using Blazorise.Bootstrap5;
using Blazorise.Icons.FontAwesome;
using Robot.Service.FileGo;
using Robot.Service.Api;
using Robot.Data.DA.API.APP;
using Robot.Tools;
using Robot.POS.DA;

namespace Robot {
    public class Startup {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services) {
            services.AddDbContext<GAEntities>(options => options.UseSqlServer(Configuration["GAEntities:ConnectionString"]).UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking), ServiceLifetime.Transient);
            services.AddDbContext<FILEEntities>(options => options.UseSqlServer(Configuration["FILEEntities:ConnectionString"]).UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking), ServiceLifetime.Transient);
         
            services.AddTelerikBlazor();
            //Blazorise
            services
                   .AddBlazorise(options => {
                       options.Immediate = true;
                   })
                .AddBootstrap5Providers()
                .AddFontAwesomeIcons();



            //***begin tammon.y add jwt
            //services.AddControllersWithViews(options =>
            //{
            //    options.Filters.Add(new AuthorizeFilter(
            //        new AuthorizationPolicyBuilder().AddAuthenticationSchemes(
            //            JwtBearerDefaults.AuthenticationScheme).RequireAuthenticatedUser().Build()));
            //}).AddNewtonsoftJson(options =>
            //{
            //    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
            //    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            //});


            //services.AddSingleton<IPostConfigureOptions<JwtBearerOptions>, ConfigureJwtBearerOptions>();
            //services.Configure<JwtAuthentication>(Configuration.GetSection("JwtAuthentication"));
            //services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();
            //***end tammon.y add jwt


            //***begin tammon.y add swagger
            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "TEST JWT", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme {
                    In = ParameterLocation.Header,
                    Description = "Please insert JWT with Bearer into field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                });
            });
            //***end tammon.y end swagger

       

            services.AddControllers();
            services.AddCors(options => {
                options.AddPolicy("AllowAll", builder => {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });


            services.AddDistributedMemoryCache();
            services.AddSession(options => {
                options.IdleTimeout = TimeSpan.FromSeconds(500);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            services.AddCors(); // Make sure you call this previous to AddMvc
                                // services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
                                // MvcOptions.EnableEndpointRouting = false;


      
    
 
            services.AddScoped<IDRuunerService, IDRuunerService>();


            //*** ga dependency****
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<StateContainer, StateContainer>();
            //services.AddSingleton<CircuitHandler>(new CircuitHandlerService());

            services.AddSingleton<PageHistoryState>();

            services.AddScoped<LogInService, LogInService>();
            services.AddScoped<FileGo, FileGo>();
            services.AddScoped<ClientService, ClientService>();
            services.AddTransient<LinkService, LinkService>();
            services.AddTransient<FireBaseService, FireBaseService>();

            services.AddScoped<LocalStorageService, LocalStorageService>();

            services.AddScoped<LogInCrossAppService, LogInCrossAppService>();
            services.AddScoped<POSService, POSService>();
            services.AddScoped<POSSaleConverterService, POSSaleConverterService>();
            services.AddScoped<ShipToService, ShipToService>();
            services.AddScoped<TableInfoService, TableInfoService>();


            services.AddScoped<ItemService, ItemService>();
            services.AddScoped<ItemPriceService, ItemPriceService>();
            services.AddScoped<POSItemService, POSItemService>();
            services.AddScoped<MyUserService, MyUserService>();
            services.AddScoped<MyUserGroupService, MyUserGroupService>();


            services.AddScoped<UserService, UserService>();
            services.AddScoped<UserGroupService, UserGroupService>();

            services.AddScoped<CompanyService, CompanyService>();
            services.AddScoped<CustomerService, CustomerService>();
            services.AddScoped<MasterTypeService, MasterTypeService>();
            services.AddScoped<XFilesService, XFilesService>();

            services.AddScoped<SyncMasterDataService, SyncMasterDataService>();
            services.AddScoped<POSOrderService, POSOrderService>();
            services.AddScoped<POS_POService, POS_POService>();
            
            services.AddScoped<POSStockService, POSStockService>();
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddBlazoredSessionStorage();
            services.AddBlazoredLocalStorage();

            services.AddSweetAlert2();
            services.AddScoped<AuthenticationStateProvider, AuthStateProvider>();
            services.AddBlazoredModal();



        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
                //   app.UseRazorComponentsRuntimeCompilation();
            } else {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            Globals.Configure(app.ApplicationServices);
            Globals.GAEntitiesConn = Configuration["GAEntities:ConnectionString"];
            Globals.FirebaseUrl = Configuration["FirebaseConfig:EndpointURL"];
            Globals.FirebaseWebToken = Configuration["FirebaseConfig:Authorization"];
            Globals.ApiPrintMasterBaseUrl = Configuration["PrintMaster:BaseUrl"];
            Globals.AppID = Configuration["AppConfig:AppID"];

            app.UseCors("AllowAll");
            //app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod());

            app.UseStaticFiles();
            app.UseStaticFiles(
                new StaticFileOptions { FileProvider = new PhysicalFileProvider(Path.Combine(@"D:\ImageStorage\KYPOS")), RequestPath = "/ImageStorage" }
                );
            app.UseStaticFiles(
             new StaticFileOptions { FileProvider = new PhysicalFileProvider(Path.Combine(env.WebRootPath, "assets")), RequestPath = "/MyAssets" }
             );







            //services.Configure<FirebaseConfig>(Configuration.GetSection("FirebaseConfig"));

            // Make sure you call this before calling app.UseMvc()
            //app.UseCors(options => options.WithOrigins("http://www.mydomain.com").AllowAnyMethod());
           
        
            app.UsePathBase("/SALE");
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSession();
            app.UseRouting();




            //app.UseSwagger();
            //app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Monitoring"));
            //app.UseAuthentication();
            //app.UseAuthorization();



            app.UseEndpoints(endpoints => {
                endpoints.MapBlazorHub();
                endpoints.MapControllers();
                endpoints.MapFallbackToPage("/_Host");
            });



        }
    }
}
