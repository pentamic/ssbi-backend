using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Breeze.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using Pentamic.SSBI.Data;
using Pentamic.SSBI.Services.Breeze;
using Pentamic.SSBI.Services.SSAS.Metadata;
using Pentamic.SSBI.Services.SSAS.Query;

namespace Pentamic.SSBI.WebApi
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    builder =>
                    {
                        builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
                    });
            });
            services.AddScoped<AppDbContext>(_ => new AppDbContext(Configuration.GetConnectionString("DefaultConnection")));
            services.AddTransient<DbPersistenceManager<AppDbContext>>();
            services.AddTransient<MetadataService>();
            services.AddTransient<DataModelEntityService>();
            services.AddTransient<ReportingEntityService>();
            services.AddTransient<QueryService>();
            // Add framework services.
            services.AddMvc().AddJsonOptions(opt => {
                var ss = JsonSerializationFns.UpdateWithDefaults(opt.SerializerSettings);
                var resolver = ss.ContractResolver;
                if (resolver != null)
                {
                    if (resolver is DefaultContractResolver res)
                    {
                        res.NamingStrategy = null; // <<!-- this removes the camelcasing
                    }
                }

            }); ;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            loggerFactory.AddFile("Logs/{Date}.txt");
            app.UseCors("AllowAll");
            app.UseMvc();
        }
    }
}
