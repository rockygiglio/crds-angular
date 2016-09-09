using System;
using System.Configuration;
using System.Reflection;
using crds_angular.Services.Interfaces;
using log4net;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using CommandLine;
using CommandLine.Text;
using Crossroads.Utilities.Services;


namespace Crossroads.ScheduledDataUpdate
{
    public class Program
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private static void Main(string [] args)
        {
            log4net.Config.XmlConfigurator.Configure();

            TlsHelper.AllowTls12();

            var unitySections = new [] { "unity", "scheduledDataUnity" };

            var container = new UnityContainer();
            foreach (var sectionName in unitySections)
            {
                var section = (UnityConfigurationSection)ConfigurationManager.GetSection(sectionName);
                container.LoadConfiguration(section);
            }

            var argString = args == null ? string.Empty : string.Join(" ", args);
            Log.Info($"Starting ScheduledDataUpdate with arguments: {argString}");

            var program = container.Resolve<Program>();
            var exitCode = program.Run(args);

            Log.Info($"Completed Scheduled Data Update, exit code {exitCode}");

            Environment.Exit(exitCode);
        }

        private readonly ITaskService _taskService;
        private readonly IGroupToolService _groupToolService;

        public Program(ITaskService taskService, IGroupToolService groupToolService)
        {
            _taskService = taskService;
            _groupToolService = groupToolService;
        }

        public int Run(string[] args)
        {
            var options = new Options();
            if (!Parser.Default.ParseArguments(args, options))
            {
                Log.Error("Invalid Arguments.");
                Log.Error(options.GetUsage());
                return 1;
            }

            if (options.HelpMode)
            {
                Log.Error(options.GetUsage());
                return 0;
            }

            var exitCode = 0;
            var modeSelected = false;

            if (options.AutoCompleteTasksMode)
            {
                modeSelected = true;
                try
                {
                    Log.Info("Starting Auto Complete Tasks");

                    _taskService.AutoCompleteTasks();

                    Log.Info("Finished Auto Complete Tasks successfully");
                }
                catch (Exception ex)
                {
                    Log.Error("Auto Complete Tasks failed.", ex);
                    exitCode = 9999;
                }
            }

            if (options.SmallGroupInquiryReminderMode)
            {
                modeSelected = true;
                try
                {
                    Log.Info("Starting Small Group Inquiry Reminder");

                    _groupToolService.SendSmallGroupPendingInquiryReminderEmails();

                    Log.Info("Finished Small Group Inquiry Reminder successfully");
                }
                catch (Exception ex)
                {
                    Log.Error("Small Group Inquiry Reminder failed.", ex);
                    exitCode = 9999;
                }
            }

            if (!modeSelected)
            {
                Log.Error(options.GetUsage());
                return 0;
            }

            return exitCode;
        }

        public class Options
        {
            [Option('H', "help", Required = false, DefaultValue = false, MutuallyExclusiveSet = "OpMode",
              HelpText = "Get help on running this program.")]
            public bool HelpMode { get; set; }

            [Option("AutoCompleteTasks", Required = false, DefaultValue = false, MutuallyExclusiveSet = "OpMode",
              HelpText = "Execute 'Auto Complete Tasks' to complete outstanding event/room tasks for users")]
            public bool AutoCompleteTasksMode { get; set; }

            [Option("SmallGroupInquiryReminder", Required = false, DefaultValue = false, MutuallyExclusiveSet = "OpMode",
              HelpText = "Execute 'Small Group Inquiry Reminder' to send emails to group leaders who have pending inquiries on their groups")]
            public bool SmallGroupInquiryReminderMode { get; set; }

            [ParserState]
            public IParserState LastParserState { get; set; }

            [HelpOption]
            public string GetUsage()
            {
                return HelpText.AutoBuild(this, current => HelpText.DefaultParsingErrorsHandler(this, current));
            }
        }
    }
}
