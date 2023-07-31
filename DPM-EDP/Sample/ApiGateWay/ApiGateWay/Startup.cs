using ApiGateWay.Data.CimsDB;
using ApiGateWay.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiGateWay {
    public class Startup {
        public IConfiguration Configuration { get; }
        public IConfiguration OcelotConfiguration { get; }
        public Startup(IConfiguration configuration, IHostEnvironment env) {
            Configuration = configuration;
            var builder = new ConfigurationBuilder();
            builder.SetBasePath(env.ContentRootPath)
                   .AddJsonFile("ocelot.json", optional: false, reloadOnChange: true)
                   .AddEnvironmentVariables();

            OcelotConfiguration = builder.Build();
        }

   

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            services.AddDbContext<CimsContext>(options => options.UseNpgsql(Globals.CimsConn));
            #region ocelot


            services.AddAuthentication().AddJwtBearer("products_auth_scheme", options => {
                options.TokenValidationParameters = new TokenValidationParameters() {
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("my_products_api_secret")),
                    ValidAudience = "productsAudience",
                    ValidIssuer = "productsIssuer",
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };

            }).AddJwtBearer("categories_auth_scheme", options => {
                options.TokenValidationParameters = new TokenValidationParameters() {
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("my_categories_api_secret")),
                    ValidAudience = "categoriesAudience",
                    ValidIssuer = "categoriesIssuer",
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };

            }).AddJwtBearer("my_auth_scheme", options => {
                options.TokenValidationParameters = new TokenValidationParameters() {
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("my_tammonjwt_api_secret")),
                    ValidAudience = "myAudience",
                    ValidIssuer = "myIssuer",
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };

            });

            services.AddOcelot(OcelotConfiguration);
            #endregion
            services.AddControllers();
            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ApiGateWay", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ApiGateWay v1"));
            }
            Globals.CimsConn = Configuration["CIMSContext:ConnectionString"];

            app.UseHttpsRedirection();

            app.UseRouting();

            //app.UseAuthorization();
            //app.UseAuthorization();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });
            app.UseOcelot().Wait();
            app.UseMiddleware<RequestResponseLoggingMiddleware>();
        }
    }
}
