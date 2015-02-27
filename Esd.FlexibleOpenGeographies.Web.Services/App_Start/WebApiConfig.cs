using Esd.FlexibleOpenGeographies.Web.Services.Utils;
using System.Web.Http;
using WebApiContrib.Formatting.Jsonp;

namespace Esd.FlexibleOpenGeographies.Web.Services
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            config.Formatters.Add(new BrowserJsonFormatter());
            config.Formatters.Add(new JsonpMediaTypeFormatter(new BrowserJsonFormatter(), "callback"));

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute("DefaultApi", "{controller}/{id}", new { id = RouteParameter.Optional }
            );
        }
    }
}
