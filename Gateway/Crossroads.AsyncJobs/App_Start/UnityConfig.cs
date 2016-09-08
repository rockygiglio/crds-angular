using System.Collections.Specialized;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using System.Configuration;
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

                var section = (UnityConfigurationSection)ConfigurationManager.GetSection("unity");
                var container = new UnityContainer();
                
                section.Configure(container);

                QuartzConfig.Register(container);
                
                GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
            }
        }
    }
}