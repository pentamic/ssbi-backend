﻿using IdentityServer3.AccessTokenValidation;
using Microsoft.Owin.Cors;
using Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Http;

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

            var c1 = new Migrations.ReportingContext.Configuration();
            var c2 = new Migrations.DataModelContext.Configuration();

            var dbMigrator = new DbMigrator(c1);
            dbMigrator.Update();
            dbMigrator = new DbMigrator(c2);
            dbMigrator.Update();
        }
    }
}