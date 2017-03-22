using IdentityServer3.AccessTokenValidation;
using Microsoft.Owin.Cors;
using Owin;
using System;
using System.Collections.Generic;
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
                Authority = "http://localhost:5000",
                RequiredScopes = new[] { "ssbi-api" }
            });
            app.UseWebApi(httpConfiguration);
        }
    }
}