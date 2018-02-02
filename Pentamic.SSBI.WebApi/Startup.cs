using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Data.Entity.Migrations.Infrastructure;
using System.IO;
using System.Linq;
using Breeze.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using Pentamic.SSBI.Data;
using Pentamic.SSBI.Services;
using Pentamic.SSBI.Services.Breeze;
using Pentamic.SSBI.Services.Common;
using Pentamic.SSBI.Services.SSAS.Dax;
using Pentamic.SSBI.Services.SSAS.Metadata;
using Pentamic.SSBI.Services.SSAS.Query;

namespace Pentamic.SSBI.WebApi
{
    public class Startup
    {
        private readonly IHostingEnvironment _env;
        public Startup(IHostingEnvironment env)
        {
            _env = env;
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
            var appConStr = Configuration.GetConnectionString("Application");
            var asConStr = Configuration.GetConnectionString("AnalysisService");
            var baseUploadPath = InitBaseUploadDirectory();
            services.Configure<UploadSettings>(Configuration.GetSection("UploadSettings"));
            services.AddSingleton<IConfiguration>(Configuration);
            services.AddScoped(_ => new AppDbContext(appConStr));
            services.AddScoped<IUserResolver, UserResolver>();
            services.AddTransient<DbPersistenceManager<AppDbContext>>();
            services.AddTransient<DataModelEntityService>();
            services.AddTransient<ReportingEntityService>();
            services.AddTransient<DaxExpressionGenerator>();
            var serviceCollection = services.AddTransient(_ => new DataSourceHelper(baseUploadPath));
            var dsHelper = (DataSourceHelper)serviceCollection.BuildServiceProvider().GetService(typeof(DataSourceHelper));
            services.AddTransient(_ => new MetadataService(asConStr, dsHelper));
            services.AddTransient(_ => new QueryService(asConStr));
            services.AddTransient<DiscoverService>();

            // Add framework services.
            services.AddMvc().AddJsonOptions(opt =>
            {
                var ss = JsonSerializationFns.UpdateWithDefaults(opt.SerializerSettings);
                var resolver = ss.ContractResolver;
                if (resolver != null)
                {
                    if (resolver is DefaultContractResolver res)
                    {
                        res.NamingStrategy = null; // <<!-- this removes the camelcasing
                    }
                }
            });
            //services.AddAuthentication();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            loggerFactory.AddFile("Logs/{Date}.txt");
            InitializeDatabase(app);
            app.UseCors("AllowAll");
            app.UseJwtBearerAuthentication(new JwtBearerOptions()
            {
                Authority = "http://pentamic.ddns.net:5000",
                RequireHttpsMetadata = false,
                Audience = "ssbi-api"
            });
            app.UseMvc();
        }

        public string InitBaseUploadDirectory()
        {
            var uploadSettings = Configuration.GetSection("UploadSettings");
            var baseUploadPath = uploadSettings["BaseUploadPath"];
            if (string.IsNullOrEmpty(baseUploadPath))
            {
                baseUploadPath = Path.Combine(_env.WebRootPath, "uploads");
                uploadSettings["BaseUploadPath"] = baseUploadPath;
            }
            if (!Directory.Exists(baseUploadPath))
            {
                Directory.CreateDirectory(baseUploadPath);
            }
            return baseUploadPath;
        }

        public void InitializeDatabase(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();
                context.Database.Initialize(false);
            }

        }
    }
}
