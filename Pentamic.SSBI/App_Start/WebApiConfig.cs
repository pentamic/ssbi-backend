using Pentamic.SSBI.Services;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;

namespace Pentamic.SSBI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            //config.MessageHandlers.Add(new IdentityManagerProxyHandler());

            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
