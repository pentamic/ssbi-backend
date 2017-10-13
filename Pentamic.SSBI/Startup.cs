using IdentityServer3.AccessTokenValidation;
using Microsoft.Owin.Cors;
using Owin;
using Serilog;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using Hangfire;
using Pentamic.SSBI.Services;

namespace Pentamic.SSBI
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var httpConfiguration = new HttpConfiguration();
            WebApiConfig.Register(httpConfiguration);
            app.UseCors(CorsOptions.AllowAll);
            app.UseIdentityServerBearerTokenAuthentication(new IdentityServerBearerTokenAuthenticationOptions
            {
                Authority = System.Configuration.ConfigurationManager.AppSettings["OidcProviderUrl"],
                RequiredScopes = new[] { "ssbi-api" }
            });
            app.UseWebApi(httpConfiguration);

            Log.Logger = new LoggerConfiguration()
                .WriteTo.RollingFile("Logs/log-{Date}.txt")
                .CreateLogger();

#if DEBUG
#else
            Hangfire.GlobalConfiguration.Configuration.UseSqlServerStorage("BackgroundServiceConnection");
            app.UseHangfireServer();
            app.UseHangfireDashboard();
            RecurringJob.AddOrUpdate<DataModelBackgroundService>(x => x.RunModelRefreshQueue(), Cron.Minutely);
#endif



            //var c1 = new Migrations.ReportingContext.Configuration();
            //var c2 = new Migrations.DataModelContext.Configuration();

            //var dbMigrator = new DbMigrator(c1);
            //dbMigrator.Update();
            //dbMigrator = new DbMigrator(c2);
            //dbMigrator.Update();
        }
    }
}