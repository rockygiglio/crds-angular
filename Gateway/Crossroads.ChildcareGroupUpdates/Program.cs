using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using crds_angular.App_Start;
using crds_angular.Services;
using Crossroads.Utilities.Services;
using log4net;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using MinistryPlatform.Translation.Repositories;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace Crossroads.ChildcareGroupUpdates
{
    internal class Program
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private static void Main()
        {
            var section = (UnityConfigurationSection) ConfigurationManager.GetSection("unity");
            var container = new UnityContainer();
            section.Configure(container);

            TlsHelper.AllowTls12();

            var eventService = container.Resolve<EventService>();
            var groupService = container.Resolve<GroupService>();
            var userApiService = container.Resolve<ApiUserRepository>();
            var mpRestRepository = container.Resolve<MinistryPlatformRestRepository>();

            // use childcare grouptype and eventtype
            var groupTypeId = 27;
            var eventTypeId = 243;

            try
            {
                Log.Info("Updating Childcare groups");
                var apiToken = userApiService.GetToken();
                AutoMapperConfig.RegisterMappings();

                var parms = new Dictionary<string, object>()
                {
                    {"@Group_Type", groupTypeId},
                    {"@Event_type", eventTypeId}
                };

                //Call Andy's stored proc and loop through
                var results = mpRestRepository.UsingAuthenticationToken(apiToken).GetFromStoredProc<MpEventsMissingGroups>("api_crds_MissingChildcareGroup", parms);
                var eventList = results.FirstOrDefault() ?? new List<MpEventsMissingGroups>();
                foreach (var item in eventList)
                {
                    var groupdto = groupService.GetGroupDetails(item.GroupId);

                    //change the date
                    var mpevent = eventService.GetEvent(item.EventId);
                    groupdto.StartDate = mpevent.EventStartDate;
                    groupdto.Participants?.Clear();
                    groupdto.Events?.Clear();
                    groupdto.MeetingDayId = null;
                    groupdto.MeetingFrequencyID = null;
                    //change the dates
                    
                    var newgroupdto = groupService.CreateGroup(groupdto);

                    //link the new group to the event
                    eventService.AddEventGroup(item.EventId, newgroupdto.GroupId, apiToken);
                }
            }
            catch (Exception ex)
            {
                Log.Error("Childcare Cancellation Notifcation Failed", ex);
                Environment.Exit(9999);
            }
            Environment.Exit(0);
        }
    }
}