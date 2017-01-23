using System.Data;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using System.Configuration;
using System.Web.Http;
using Unity.WebApi;


namespace crds_angular
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            IUnityContainer container = new UnityContainer();

            // Crossroads common unity config
            var crossroadsCommonSection = (UnityConfigurationSection)ConfigurationManager.GetSection("crossroadsCommonUnity");
            crossroadsCommonSection.Configure(container);

            // App unity config
            var section = (UnityConfigurationSection)ConfigurationManager.GetSection("unity");
            section.Configure(container);

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}
