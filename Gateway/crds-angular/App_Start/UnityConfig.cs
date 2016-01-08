using System.Data;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using System.Configuration;
using System.Web.Http;
using crds_angular.Controllers.API;
using Unity.WebApi;


namespace crds_angular
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            var section = (UnityConfigurationSection)ConfigurationManager.GetSection("unity");
            IUnityContainer container = new UnityContainer();
            section.Configure(container);
            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}