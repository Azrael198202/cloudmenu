using System.Net.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using cloudmenu_api.DbContextCloudMenu;
using cloudmenu_api.Services;
using LiteX.Storage.FileSystem;

namespace cloudmenu_api
{
    public class Startup
    {
        public static readonly ILoggerFactory MyLoggerFactory = LoggerFactory.Create(builder => { builder.AddConsole(); });
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var aspnetEnvironment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var serverVersion = new MySqlServerVersion(new Version(8, 0, 23));
            if ("Development" == aspnetEnvironment)
            {
                services.AddDbContext<AppDbContext>(
                dbContextOptions => dbContextOptions
                    .UseLoggerFactory(MyLoggerFactory)
                    .UseMySql(Configuration.GetConnectionString("DefaultConnection"), serverVersion)
                    .EnableSensitiveDataLogging() // <-- These two calls are optional but help
                    .EnableDetailedErrors()       // <-- with debugging (remove for production).
                );
            }
            else
            {
                services.AddDbContext<AppDbContext>(
                dbContextOptions => dbContextOptions
                    .UseLoggerFactory(MyLoggerFactory)
                    .UseMySql(Configuration.GetConnectionString("DefaultConnection"), serverVersion)
                // .EnableSensitiveDataLogging() // <-- These two calls are optional but help
                // .EnableDetailedErrors()       // <-- with debugging (remove for production).
                );
            }

            services.AddCors(o => o.AddPolicy("CrosPolicy", builder =>
                {
                    builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                }));

            services.AddScoped<SeatService>();
            services.AddScoped<ChitPrintService>();
            services.AddScoped<CashierService>();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "cloudmenu_api", Version = "v1" });
            });

            var fileSystemStorageConfig = new FileSystemStorageConfig()
            {
                Directory = Configuration["FileSystemStorageConfig:Directory"],
                EnableLogging = true
            };
            services.AddLiteXFileSystemStorageService(fileSystemStorageConfig);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            // todo 
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "cloudmenu_api v1"));

            //app.UseHttpsRedirection();
            app.UseCors("CrosPolicy");

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
