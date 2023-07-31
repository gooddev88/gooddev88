using Blazored.LocalStorage;
using Blazored.Modal;
using Blazored.SessionStorage;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Robot.Data.DA.DataHelper;
using Robot.Data.DA.Order.SO;
using Robot.Data.DA.Login;
using Robot.Data.DA.Master;
using Robot.Data.DA.USER;
using Robot.Data.GADB.TT;
using Robot.Helper.FCM;
using Robot.Tools;
using Blazorise;
using Blazorise.Icons.FontAwesome;
using Blazorise.Bootstrap5;


using Robot.Service.Api;
using Robot.Service.FileGo;
using Robot.Data.DA.Stock;
using Robot.Data.DA.HR;
using Robot.Data.DA.Order;
using FisSst.BlazorMaps.DependencyInjection;

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

            services.AddTelerikBlazor();
            //services.AddDevExpressServerSideBlazorReportViewer();

            //Blazorise
            services
                  .AddBlazorise(options =>
                  {
                      options.ChangeTextOnKeyPress = true; // optional
                  })
                  .AddBootstrap5Providers()
                  .AddFontAwesomeIcons();



            //services.Configure<FirebaseConfig>(Configuration.GetSection("FirebaseConfig"));

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
            services.AddSwaggerGen(c =>
            {
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

            //services.AddDistributedMemoryCache();
            services.AddSession();
            //services.AddSession(options => {
            //    options.IdleTimeout = TimeSpan.FromSeconds(500);
            //    options.Cookie.HttpOnly = true;
            //    options.Cookie.IsEssential = true;
            //});
            services.AddBlazorLeafletMaps();

            services.AddCors(); // Make sure you call this previous to AddMvc
                                // services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
                                // MvcOptions.EnableEndpointRouting = false;





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
            services.AddScoped<UrlInfo, UrlInfo>();
            services.AddScoped<LocalStorageService, LocalStorageService>();

            services.AddScoped<MyUserService, MyUserService>();
            services.AddScoped<MyUserGroupService, MyUserGroupService>();
            services.AddScoped<UserService, UserService>();
            services.AddScoped<UserGroupService, UserGroupService>();
            services.AddScoped<CompanyService, CompanyService>();
            services.AddScoped<CustomerService, CustomerService>();
            services.AddScoped<MasterTypeService, MasterTypeService>();
            services.AddScoped<BookBankService, BookBankService>();
            services.AddScoped<InventoryService, InventoryService>();
            services.AddScoped<ItemService, ItemService>();
            services.AddScoped<LocService, LocService>();
            services.AddScoped<PromotionService, PromotionService>();
            services.AddScoped<SOService, SOService>();
            services.AddScoped<StkAdjustService, StkAdjustService>();
            services.AddScoped<StkTransferService, StkTransferService>();

            services.AddRazorPages();
            services.AddServerSideBlazor().AddHubOptions(hub => hub.MaximumReceiveMessageSize = 100 * 1024 * 1024);
            services.AddSignalR(e => {
                e.MaximumReceiveMessageSize = null; // no limit
                e.EnableDetailedErrors = true;
            });
            services.AddBlazoredSessionStorage();
            services.AddBlazoredLocalStorage();


            services.AddSweetAlert2();
            services.AddScoped<AuthenticationStateProvider, AuthStateProvider>();
            //services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
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
            Globals.BlazorServer_Front = Configuration["CrossAppUrl:Front"];
            //services.Configure<FirebaseConfig>(Configuration.GetSection("FirebaseConfig"));
            // Make sure you call this before calling app.UseMvc()
            //app.UseCors(options => options.WithOrigins("http://www.mydomain.com").AllowAnyMethod());
            app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod());
            //    app.UseMvc();
            //app.UseDevExpressServerSideBlazorReportViewer();
            //app.UsePathBase("/app");
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSession();
            app.UseRouting();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapControllers();
                endpoints.MapFallbackToPage("/_Host");
            });

        }
    }
}
