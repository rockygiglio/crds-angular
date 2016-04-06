using System.Collections.Generic;
using System.Linq;
using Crossroads.Utilities.Interfaces;
using MinistryPlatform.Translation.Extensions;
using MinistryPlatform.Translation.Models.GoCincinnati;
using MinistryPlatform.Translation.Services.Interfaces;

namespace MinistryPlatform.Translation.Services
{
    public class GroupConnectorService : BaseService, IGroupConnectorService
    {
        private readonly IMinistryPlatformService _ministryPlatformService;

        public GroupConnectorService(IAuthenticationService authenticationService, IConfigurationWrapper configurationWrapper, IMinistryPlatformService ministryPlatformService)
            : base(authenticationService, configurationWrapper)
        {
            _ministryPlatformService = ministryPlatformService;
        }

        public List<MpGroupConnector> GetGroupConnectorsForOpenOrganizations(int initiativeId, string token)
        {
            var searchString = string.Format(",,,,,true,{0}", initiativeId);
            return GetGroupConnectors(searchString, token);
        }

        public List<MpGroupConnector> GetGroupConnectorsForOrganization(int organizationId, int initiativeId, string token)
        {
            var searchString = string.Format(",,,,{0},,{1}", organizationId, initiativeId);
            return GetGroupConnectors(searchString, token);
        }

        private List<MpGroupConnector> GetGroupConnectors(string searchString, string token)
        {
            //var token = ApiLogin();
            var result = _ministryPlatformService.GetPageViewRecords("GroupConnectorPageView", token, searchString);

            return result.Select(r => new MpGroupConnector
            {
                Id = r.ToInt("GroupConnector_ID"),
                Name = r.ToString("Primary_Registration"),
                PrimaryRegistrationID = r.ToInt("Primary_Registration_Contact_ID"),
                ProjectMaximumVolunteers = r.ToInt("Project_Maximum_Volunteers"),
                AbsoluteMaximumVolunteers = r.ToInt("Absolute_Maximum_Volunteers"),
                ProjectMinimumAge = r.ToInt("Project_Minimum_Age"),
                ProjectName = r.ToString("Project_Name"),
                ProjectType = r.ToString("Project_Type"),
                PreferredLaunchSite = r.ToString("Preferred_Launch_Site"),
                VolunteerCount = r.ToInt("Volunteer_Count")
            }).ToList();
        }
    }
}