using System;
using System.Web.Http;

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
            config.MapHttpAttributeRoutes(ApiRouteProvider);
            config.Filters.Add(new DeprecatedVersionFilter());
            config.Filters.Add(new RemovedVersionFilter());
        }
    }
}