using System;
using System.Configuration;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Crossroads.ApiVersioning
{
    public class VersionConfig
    {
        public static ApiRouteProvider ApiRouteProvider;

        static VersionConfig()
        {
            ApiRouteProvider = new ApiRouteProvider();
        }

        public static void Register(HttpConfiguration config)
        {
            var cors = new EnableCorsAttribute(ConfigurationManager.AppSettings["CORS"], "*", "*");
            cors.SupportsCredentials = true;
            config.EnableCors(cors);

            // Web API configuration and services
            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));

            // Web API routes
            config.MapHttpAttributeRoutes(ApiRouteProvider);
            config.Filters.Add(new DeprecatedVersionFilter());
            config.Filters.Add(new RemovedVersionFilter());
        }
    }
}