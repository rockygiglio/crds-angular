using System;
using System.Collections.Generic;
using System.Linq;
using Crossroads.Utilities.Interfaces;
using log4net;
using MinistryPlatform.Translation.Extensions;
using MinistryPlatform.Translation.Models.GoCincinnati;
using MinistryPlatform.Translation.Services.Interfaces;
using MinistryPlatform.Translation.Services.Interfaces.GoCincinnati;
using IGroupConnectorService = MinistryPlatform.Translation.Services.Interfaces.GoCincinnati.IGroupConnectorService;

namespace MinistryPlatform.Translation.Services.GoCincinnati
{
    public class GroupConnectorService : BaseService, IGroupConnectorService
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof (RoomService));
        private readonly IMinistryPlatformService _ministryPlatformService;
        private readonly string _apiToken;

        public GroupConnectorService(IMinistryPlatformService ministryPlatformService, IAuthenticationService authenticationService, IConfigurationWrapper configuration)
            : base(authenticationService, configuration)
        {
            _ministryPlatformService = ministryPlatformService;
            _apiToken = ApiLogin();
        }

        public int CreateGroupConnector(int registrationId, bool privateGroup)
        {
            var t = ApiLogin();
            var pageId = _configurationWrapper.GetConfigIntValue("GroupConnectorPageId");
            var dictionary = new Dictionary<string, object>
            {
                {"Primary_Registration", registrationId},
                {"Private", privateGroup}
            };

            try
            {
                var groupConnectorId = _ministryPlatformService.CreateRecord(pageId, dictionary, t, true);
                CreateGroupConnectorRegistration(groupConnectorId, registrationId);
                return groupConnectorId;
            }
            catch (Exception e)
            {
                var msg = string.Format("Error creating Go Cincinnati Group Connector, registration: {0}", registrationId);
                _logger.Error(msg, e);
                throw (new ApplicationException(msg, e));
            }
        }

        public int CreateGroupConnectorRegistration(int groupConnectorId, int registrationId)
        {
            var t = ApiLogin();
            var dictionary = new Dictionary<string, object>
            {
                {"Registration_ID", registrationId}
            };

            try
            {
                return _ministryPlatformService.CreateSubRecord("GroupConnectorRegistrationPageId", groupConnectorId, dictionary, t, true);
            }
            catch (Exception e)
            {
                var msg = string.Format("Error creating Go Cincinnati Group Connector Registration, group connector: {0}, registration: {1}", groupConnectorId, registrationId);
                _logger.Error(msg, e);
                throw (new ApplicationException(msg, e));
            }
        }

        public MpGroupConnector GetGroupConnectorById(int groupConnectorId)
        {
            var searchString = string.Format(",,,,,,,,,,,{0}", groupConnectorId);
            var groupConnectors = GetGroupConnectors(searchString, _apiToken);
            var groupConnector = groupConnectors.SingleOrDefault();
            return groupConnector;
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
                ProjectMinimumAge = r.ToInt("Project_Minimum_Age"),
                ProjectName = r.ToString("Project_Name"),
                ProjectType = r.ToString("Project_Type"),
                PreferredLaunchSite = r.ToString("Preferred_Launch_Site"),
                VolunteerCount = r.ToInt("Volunteer_Count"),
                PreferredLaunchSiteId = r.ToInt("Preferred_Launch_Site_ID")
            }).ToList();
        }
    }
}