using crds_angular.App_Start;
using System.IO;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Crossroads.Utilities.Services;
using Crossroads.ApiVersioning;
using Crossroads.ClientApiKeys;

namespace crds_angular
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            UnityConfig.RegisterComponents();
            //GlobalConfiguration.Configure(WebApiConfig.Register);
            GlobalConfiguration.Configure(VersionConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AutoMapperConfig.RegisterMappings();
            DomainLockedClientApiKeyConfig.Register(GlobalConfiguration.Configuration);
            TlsHelper.AllowTls12();
            log4net.Config.XmlConfigurator.Configure(new FileInfo(Server.MapPath("~/Web.config")));
        }

    }
}
