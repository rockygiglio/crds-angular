using System;
using System.Configuration;
using System.Reflection;
using crds_angular.App_Start;
using crds_angular.Services.Interfaces;
//using Crossroads.Utilities.Services;
using log4net;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;

namespace Crossroads.ScheduledDataUpdate
{
    class Program
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private static void Main()
        {
            log4net.Config.XmlConfigurator.Configure();
            var section = (UnityConfigurationSection)ConfigurationManager.GetSection("unity");
            var container = new UnityContainer();
            section.Configure(container);

            try
            {
                Log.Info("Starting Auto Complete Tasks");

                var taskService = container.Resolve<ITaskService>();
                taskService.AutoCompleteTasks();

                Log.Info("Finished Auto Complete Tasks successfully");
            }
            catch (Exception ex)
            {
                Log.Error("Auto Complete Tasks failed.", ex);
                Environment.Exit(9999);
            }
        }
    }
}
