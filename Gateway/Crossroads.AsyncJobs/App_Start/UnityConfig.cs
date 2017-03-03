using System.Collections.Specialized;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using System.Configuration;
using System.Linq;
using System.Web.Http;
using Crossroads.AsyncJobs.App_Start;
using Quartz;
using Quartz.Impl;
using Quartz.Unity;
using Unity.WebApi;


namespace Crossroads.AsyncJobs
{
    public static class UnityConfig
    {
        private readonly static object Lock = new object();

        public static void RegisterComponents()
        {
            lock (Lock)
            {
                // Only initialize once
                if (GlobalConfiguration.Configuration.DependencyResolver != null &&
                    GlobalConfiguration.Configuration.DependencyResolver.GetType() == typeof (UnityDependencyResolver))
                {
                    return;
                }

                var container = new UnityContainer();
                var unitySections = new[] { "crossroadsCommonUnity", "unity", "asyncJobsUnity" };

                foreach (var section in unitySections.Select(sectionName => (UnityConfigurationSection)ConfigurationManager.GetSection(sectionName)))
                {
                    container.LoadConfiguration(section);
                }

                QuartzConfig.Register(container);
                
                GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
            }
        }
    }
}